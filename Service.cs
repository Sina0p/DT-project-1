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

    public void ToggleTask(int id)
    {
        foreach (Task task in todo)
        {
            if (task != null && task.Id == id)
            {
                task.Status = !task.Status;
                DisplayTasks();
                Console.WriteLine($"\n Status of {task.Name} toggled");
                return;
            }
        }
    }

    public int FindTaskIndex(string taskToDelete)
    {
        for (int i = 0; i < todo.Length; i++)
        {
            if (todo[i] != null && todo[i].Name == taskToDelete)
            {
                return i;
            }
        }
        return -1;
    }
    
    public bool AddTask(string name)
    {
        var tasks = loadTask();

        int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 0;

        Task newTask = new Task(newId, name);
        tasks.Add(newTask);

        saveTask(tasks);

        return true;
    }

    public bool DeleteTask(string taskToDelete)
    {
        int index = FindTaskIndex(taskToDelete);

        if (index == -1)
            return false;

        for (int i = index; i < todo.Length - 1; i++)
        {
            todo[i] = todo[i + 1];
        }

        todo[todo.Length - 1] = null;
        return true;
    }

    public void DisplayTasks()
    {
        Console.WriteLine("All tasks:\n");

        foreach (Task task in todo)
        {
            if (task != null)
            {
                string status = task.Status ? "Done" : "Not Done";
                Console.WriteLine($"{task.Id}. {task.Name} : ({status})");
            }
        }
    }
}