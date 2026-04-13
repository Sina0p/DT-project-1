using System.Text.Json;

public class Repository : ITaskRepository
{
    private const string FilePath = "data.json";

    public void LoadTasks(IMyCollection<TaskItem> target)
    {
        if (!File.Exists(FilePath))
            return;

        string json = File.ReadAllText(FilePath);
        TaskItem[]? tasksArray = JsonSerializer.Deserialize<TaskItem[]>(json);

        if (tasksArray == null)
            return;

        foreach (TaskItem task in tasksArray)
        {
            target.Add(task);
        }

        target.Dirty = false;
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        TaskItem[] array = new TaskItem[tasks.Count];
        int index = 0;

        foreach (TaskItem task in tasks)
        {
            array[index++] = task;
        }

        Array.Sort(array, (a, b) => a.Id.CompareTo(b.Id));

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(array, options);
        File.WriteAllText(FilePath, json);

        tasks.Dirty = false;
    }
}