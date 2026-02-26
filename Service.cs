using System.Text.Json;
public class Service
{
    Task[] todo = new Task[99];

    private List<Task> loadTask()
    {
        var json = File.ReadAllText("data.json");
        List<Task> tasks = JsonSerializer.Deserialize<List<Task>>(json)!;
        return tasks;
    }

    private void saveTask(List<Task> tasks)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string updatedJson = JsonSerializer.Serialize(tasks, options);
        File.WriteAllText("data.json", updatedJson);
    }

    public bool ToggleTask(int id)
    {
        var tasks = loadTask();

        var task = tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return false;

        task.Status = !task.Status;

        saveTask(tasks);

        Console.Clear();
        DisplayTasks();

        return true;
    }
    
    public bool AddTask(string name)
    {
        var tasks = loadTask();

        int idNum = tasks.Count >= 1 ? tasks.Max(task => task.Id)+ 1 : 0;

        Task task = new Task(idNum, name);

        tasks.Add(task);

        saveTask(tasks);

        return true;
    }

    public bool DeleteTask(int id)
    {
        int index = -1;

        for (int i = 0; i < todo.Length; i++)
        {
            if (todo[i] != null && todo[i].Id == id)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
            return false;

        for (int i = index; i < todo.Length - 1; i++)
        {
            todo[i] = todo[i + 1];
        }

        todo[todo.Length - 1] = null;
        return true;
    }

    public bool TaskCheck()
    {
        var tasks = loadTask();
        return tasks.Any();
    }

    public void DisplayTasks()
    {
        var tasks = loadTask();

        Console.WriteLine("All tasks:\n");

        if (!tasks.Any())
        {
            Console.WriteLine("No tasks found");
            return;
        }

        foreach (var task in tasks)
        {
            string status = task.Status ? "Done" : "Not Done";
            Console.WriteLine($"({task.Id}) {task.Name} ({status})");
        }
    }
}