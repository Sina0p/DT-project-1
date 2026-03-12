using System.Text.Json;
using Microsoft.VisualBasic;

public class Service
{
    private IMyCollection<Task> _collection;
    private Repository repository = new Repository();

    public Service(IMyCollection<Task> collection)
    {
        _collection = collection;
    }

    public bool ToggleTask(int id)
    {
        var tasks = repository.loadTask();

        var task = tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return false;

        task.Status = !task.Status;

        repository.saveTask(tasks);

        Console.Clear();
        DisplayTasks();

        return true;
    }

    public bool AddTask(string name)
    {
        var tasks = repository.loadTask();

        int idNum = tasks.Count >= 1 ? tasks.Max(task => task.Id) + 1 : 0;

        Task task = new Task(idNum, name);

        tasks.Add(task);

        repository.saveTask(tasks);

        return true;
    }

    public bool DeleteTask(int id)
    {
        var tasks = repository.loadTask();

        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            return false;

        tasks.Remove(task);

        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Id = i;
        }

        repository.saveTask(tasks);
        return true;
    }

    public bool TaskCheck()
    {
        var tasks = repository.loadTask();
        return tasks.Any();
    }

    public void DisplayTasks()
    {
        var tasks = repository.loadTask();

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