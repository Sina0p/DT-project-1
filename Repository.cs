using System.Text.Json;

public class Repository : ITaskRepository
{
    public IMyCollection<TaskItem> LoadTasks()
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

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        List<TaskItem> list = new List<TaskItem>();

        foreach (var task in tasks)
        {
            list.Add(task);
        }

        list.Sort((a, b) => a.Id.CompareTo(b.Id));

        TaskItem[] array = list.ToArray();

        string json = JsonSerializer.Serialize(array, options);
        File.WriteAllText("data.json", json);
    }
}