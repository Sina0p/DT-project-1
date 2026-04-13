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

    private void Save()
    {
        if (_collection.Dirty)
            _repository.SaveTasks(_collection);
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
        Save();
        return true;
    }

    public bool DeleteTask(int id)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        _collection.Remove(task);
        Save();
        return true;
    }

    public bool ToggleTask(int id)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        task.Status = !task.Status;
        _collection.Dirty = true;
        Save();
        return true;
    }

    public bool UpdateTask(int id, string newName, int newPriority)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        task.Name = newName;
        task.Priority = newPriority;
        _collection.Dirty = true;
        Save();
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
}