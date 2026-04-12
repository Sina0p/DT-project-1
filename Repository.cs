using System.Text.Json;

public class Repository : ITaskRepository
{
    private const string FilePath = "data.json";

    public IMyCollection<TaskItem> LoadTasks()
    {
        ArrayCollection<TaskItem> collection = new ArrayCollection<TaskItem>();

        if (!File.Exists(FilePath))
            return collection;

        string json = File.ReadAllText(FilePath);
        TaskItem[]? tasksArray = JsonSerializer.Deserialize<TaskItem[]>(json);

        if (tasksArray == null)
            return collection;

        for (int i = 0; i < tasksArray.Length; i++)
        {
            collection.Add(tasksArray[i]);
        }

        collection.Dirty = false;
        return collection;
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        ArrayCollection<TaskItem> ordered = new ArrayCollection<TaskItem>();

        foreach (TaskItem task in tasks)
        {
            ordered.Add(task);
        }

        ordered.Sort((a, b) => a.Id.CompareTo(b.Id));

        TaskItem[] array = new TaskItem[ordered.Count];
        int index = 0;

        foreach (TaskItem task in ordered)
        {
            array[index++] = task;
        }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(array, options);
        File.WriteAllText(FilePath, json);

        tasks.Dirty = false;
    }
}
