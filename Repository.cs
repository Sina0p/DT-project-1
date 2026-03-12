
using System.Text.Json;
public class Repository
{
    public List<Task> loadTask()
    {
        var json = File.ReadAllText("data.json");
        List<Task> tasks = JsonSerializer.Deserialize<List<Task>>(json)!;
        return tasks;
    }

    public void saveTask(List<Task> tasks)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string updatedJson = JsonSerializer.Serialize(tasks, options);
        File.WriteAllText("data.json", updatedJson);
    }
}