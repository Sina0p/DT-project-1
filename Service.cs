using System;

public class Service : ITaskService
{
    private readonly IMyCollection<TaskItem> _collection;
    private readonly ITaskRepository _repository;

    public Service(IMyCollection<TaskItem> collection, ITaskRepository repository)
    {
        _collection = collection;
        _repository = repository;

        _repository.LoadTasks(_collection);
        _collection.Dirty = false;
    }

    public void Save()
    {
        if (_collection.Dirty)
        {
            _repository.SaveTasks(_collection);
            Console.WriteLine("Changes saved to storage.");
        }
        else
        {
            Console.WriteLine("No changes detected (nothing to save).");
        }
    }

    public bool AddTask(string name, int priority)
    {
        int maxId = _collection.Count > 0
            ? _collection.Reduce(0, (max, t) => t.Id > max ? t.Id : max)
            : -1;

        TaskItem newTask = new TaskItem(maxId + 1, name)
        {
            Priority = priority
        };

        _collection.Add(newTask);
        return true;
    }

    public bool DeleteTask(int id)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        _collection.Remove(task);
        return true;
    }

    public bool ToggleTask(int id)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        task.Status = !task.Status;
        _collection.Dirty = true;
        return true;
    }

    public bool UpdateTask(int id, string newName, int newPriority)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        task.Name = newName;
        task.Priority = newPriority;
        _collection.Dirty = true;
        return true;
    }

    public void DisplayTasks()
    {
        var ordered = _collection.Filter(t => true);
        ordered.Sort((a, b) => a.Id.CompareTo(b.Id));

        foreach (var task in ordered)
        {
            string status = task.Status ? "Done" : "Not Done";
            Console.WriteLine($"({task.Id}) {task.Name} | Priority: {task.Priority} | {task.CreationDate} | {status}");
        }
    }

    public void DisplayCompletedTasks()
    {
        var filtered = _collection.Filter(t => t.Status);

        foreach (var task in filtered)
        {
            string status = task.Status ? "Done" : "Not Done";
            Console.WriteLine($"({task.Id}) {task.Name} | Priority: {task.Priority} | {task.CreationDate} | {status}");
        }
    }

    public void DisplayByPriority(int priority)
    {
        var filtered = _collection.Filter(t => t.Priority == priority);

        foreach (var task in filtered)
        {
            string status = task.Status ? "Done" : "Not Done";
            Console.WriteLine($"({task.Id}) {task.Name} | Priority: {task.Priority} | {task.CreationDate} | {status}");
        }
    }
    public void DisplayKanban()
    {
        Console.Clear();
        
        var todoTasks = _collection.Filter(t => !t.Status);
        var doneTasks = _collection.Filter(t => t.Status);

        Console.WriteLine("\n========================== KANBAN VIEW ==========================");
        Console.WriteLine("{0,-30} | {1,-30}", "TO DO", "DONE");
        Console.WriteLine(new string('-', 65));

        var todoList = ToArray(todoTasks);
        var doneList = ToArray(doneTasks);

        int maxRows = Math.Max(todoList.Length, doneList.Length);

        for (int i = 0; i < maxRows; i++)
        {
            string todoEntry = i < todoList.Length 
                ? $"[{todoList[i].Id}] {todoList[i].Name}" 
                : "";
            string doneEntry = i < doneList.Length 
                ? $"[{doneList[i].Id}] {doneList[i].Name}" 
                : "";

            Console.WriteLine("{0,-30} | {1,-30}", Truncate(todoEntry, 28), Truncate(doneEntry, 28));
        }
        Console.WriteLine("-----------------------------------------------------------------\n");
    }

    private TaskItem[] ToArray(IMyCollection<TaskItem> col)
    {
        TaskItem[] arr = new TaskItem[col.Count];
        int i = 0;
        foreach (var item in col) arr[i++] = item;
        return arr;
    }

    private string Truncate(string value, int max)
    {
        return value.Length <= max ? value : value.Substring(0, max - 3) + "...";
    }
}