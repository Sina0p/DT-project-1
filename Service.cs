using System;

public class Service
{
    private IMyCollection<TaskItem> _collection;
    private Repository repository = new Repository();

    public Service(IMyCollection<TaskItem> collection)
    {
        _collection = collection;
    }

    public bool ToggleTask(int id)
    {
        var task = _collection.FindBy(id, (t, key) => t.Id == key);

        if (task == null)
            return false;

        task.Status = !task.Status;

        repository.saveTask(_collection);

        return true;
    }

    public bool AddTask(string name)
    {
        int maxId = -1;

        foreach (var task in _collection)
        {
            if (task.Id > maxId)
                maxId = task.Id;
        }

        int newId = maxId + 1;

        TaskItem newTask = new TaskItem(newId, name);

        _collection.Add(newTask);

        repository.saveTask(_collection);

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

        repository.saveTask(_collection);

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