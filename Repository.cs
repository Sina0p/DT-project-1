using System.Text.Json;

public class Repository
{
    public IMyCollection<TaskItem> loadTask()
    {
        if (!File.Exists("data.json"))
            return new ArrayCollection<TaskItem>();

        var json = File.ReadAllText("data.json");

        TaskItem[]? tasksArray = JsonSerializer.Deserialize<TaskItem[]>(json);

        var collection = new ArrayCollection<TaskItem>();

        if (tasksArray != null)
        {
            foreach (var task in tasksArray)
            {
                collection.Add(task);
            }
        }
        return collection;
    }

    public void saveTask(IMyCollection<TaskItem> tasks)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        TaskItem[] array = new TaskItem[tasks.Count];

        int index = 0;
        foreach (var task in tasks)
        {
            array[index++] = task;
        }

        string json = JsonSerializer.Serialize(array, options);
        File.WriteAllText("data.json", json);
    }
}