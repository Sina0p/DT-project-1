public class Service : ITaskService
{
    private readonly IMyCollection<TaskItem> _collection;
    private readonly ITaskRepository _repository;

    public Service(IMyCollection<TaskItem> collection, ITaskRepository repository)
    {
        _collection = collection;
        _repository = repository;

        var loadedTasks = _repository.LoadTasks();
        foreach (var task in loadedTasks)
        {
            _collection.Add(task);
        }
    }

    private void Save()
    {
        _repository.SaveTasks(_collection);
    }

    public bool ToggleTask(int id)
    {
        var task = _collection.FindBy(id, (t, key) => t.Id == key);

        if (task == null)
            return false;

        task.Status = !task.Status;
        Save();
        return true;
    }

    public bool AddTask(string name)
    {
        Console.WriteLine(_collection.GetType().Name);
        int maxId = _collection.Count > 0 ? _collection.Reduce(0, (max, t) => t.Id > max ? t.Id : max) : -1;

        TaskItem newTask = new TaskItem(maxId + 1, name);

        _collection.Add(newTask);

        Save();
        return true;
    }

    public bool DeleteTask(int id)
    {
        var task = _collection.FindBy(id, (t, key) => t.Id == key);

        if (task == null)
            return false;

        _collection.Remove(task);

        int index = 0;
        foreach (var t in _collection)
        {
            t.Id = index++;
        }

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

        foreach (var task in _collection)
        {
            string status = task.Status ? "Done" : "Not Done";
            Console.WriteLine($"({task.Id}) {task.Name} ({status})");
        }
    }
}