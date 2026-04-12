public class Service : ITaskService
{
    private readonly IMyCollection<TaskItem> _collection;
    private readonly ITaskRepository _repository;

    public Service(IMyCollection<TaskItem> collection, ITaskRepository repository)
    {
        _collection = collection;
        _repository = repository;

        IMyCollection<TaskItem> loadedTasks = _repository.LoadTasks();
        foreach (TaskItem task in loadedTasks)
        {
            _collection.Add(task);
        }

        _collection.Dirty = false;
    }

    private void Save()
    {
        if (_collection.Dirty)
        {
            _repository.SaveTasks(_collection);
        }
    }

    public bool ToggleTask(int id)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);

        if (task == null)
            return false;

        task.Status = !task.Status;
        _collection.Dirty = true;
        Save();
        return true;
    }

    public bool AddTask(string name)
    {
        int maxId = _collection.Count > 0
            ? _collection.Reduce(0, (max, t) => t.Id > max ? t.Id : max)
            : -1;

        TaskItem newTask = new TaskItem(maxId + 1, name);
        _collection.Add(newTask);
        Save();
        return true;
    }

    public bool DeleteTask(int id)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);

        if (task == null)
            return false;

        _collection.Remove(task);
        Save();
        return true;
    }

    public bool TaskCheck()
    {
        return _collection.Count > 0;
    }

    public void DisplayTasks()
    {
        Console.WriteLine("All tasks:\n");

        if (_collection.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }

        IMyCollection<TaskItem> ordered = _collection.Filter(t => true);
        ordered.Sort((a, b) => a.Id.CompareTo(b.Id));

        foreach (TaskItem task in ordered)
        {
            string status = task.Status ? "Done" : "Not Done";
            Console.WriteLine($"({task.Id}) {task.Name} ({status})");
        }
    }
}
