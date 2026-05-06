using System;
using System.IO;
using System.Text.Json;

public class Service : ITaskService
{
    private readonly IMyCollection<TaskItem> _collection;
    private readonly ITaskRepository _repository;
    private IMyCollection<User> _users;
    private int _nextTaskId;

    public Service(IMyCollection<TaskItem> collection, ITaskRepository repository)
    {
        _collection = collection;
        _repository = repository;
        _users = LoadUsers();

        _repository.LoadTasks(_collection);

        _nextTaskId = _collection.Count > 0
            ? _collection.Reduce(0, (max, t) => t.Id > max ? t.Id : max) + 1
            : 1;

        _collection.Dirty = false;
    }

    private IMyCollection<User> LoadUsers()
    {
        IMyCollection<User> users = new ArrayCollection<User>();

        string filePath = "user.json";
        if (!File.Exists(filePath))
            return users;

        string json = File.ReadAllText(filePath);

        User[]? usersArray = JsonSerializer.Deserialize<User[]>(json);

        if (usersArray == null)
            return users;

        for (int i = 0; i < usersArray.Length; i++)
        {
            users.Add(usersArray[i]);
        }

        users.Dirty = false;
        return users;
    }

    public User? ValidateUser(string username, string password)
    {
        return _users.FindBy(username, (u, key) =>
            u.Username == key && u.Password == password
        );
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
        TaskItem newTask = new TaskItem(_nextTaskId, name)
        {
            Priority = priority
        };

        _nextTaskId++;

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

        task.Status = task.Status switch
        {
            TaskStatus.NotDone => TaskStatus.InProgress,
            TaskStatus.InProgress => TaskStatus.Done,
            TaskStatus.Done => TaskStatus.NotDone,
            _ => TaskStatus.NotDone
        };

        _collection.Dirty = true;
        return true;
    }

    public bool SetTaskStatus(int id, TaskStatus status)
    {
        TaskItem task = _collection.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        task.Status = status;
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
        IMyCollection<TaskItem> ordered = _collection.Filter(t => true);
        ordered.Sort((a, b) => a.Id.CompareTo(b.Id));

        IMyIterator<TaskItem> iterator = ordered.GetIterator();

        while (iterator.HasNext())
        {
            TaskItem task = iterator.Next();
            Console.WriteLine($"({task.Id}) {task.Name} | Priority: {task.Priority} | {task.CreationDate} | {FormatStatus(task.Status)}");
        }
    }

    public void DisplayCompletedTasks()
    {
        DisplayByStatus(TaskStatus.Done);
    }

    public void DisplayByStatus(TaskStatus status)
    {
        IMyCollection<TaskItem> filtered = _collection.Filter(t => t.Status == status);
        IMyIterator<TaskItem> iterator = filtered.GetIterator();

        while (iterator.HasNext())
        {
            TaskItem task = iterator.Next();
            Console.WriteLine($"({task.Id}) {task.Name} | Priority: {task.Priority} | {task.CreationDate} | {FormatStatus(task.Status)}");
        }
    }

    public void DisplayByPriority(int priority)
    {
        IMyCollection<TaskItem> filtered = _collection.Filter(t => t.Priority == priority);
        IMyIterator<TaskItem> iterator = filtered.GetIterator();

        while (iterator.HasNext())
        {
            TaskItem task = iterator.Next();
            Console.WriteLine($"({task.Id}) {task.Name} | Priority: {task.Priority} | {task.CreationDate} | {FormatStatus(task.Status)}");
        }
    }

    public void DisplayKanban()
    {
        IMyCollection<TaskItem> todoTasks = _collection.Filter(t => t.Status == TaskStatus.NotDone);
        IMyCollection<TaskItem> inProgressTasks = _collection.Filter(t => t.Status == TaskStatus.InProgress);
        IMyCollection<TaskItem> doneTasks = _collection.Filter(t => t.Status == TaskStatus.Done);

        Console.WriteLine("======================================== KANBAN VIEW ========================================");
        Console.WriteLine("{0,-28} | {1,-28} | {2,-28}", "TO DO", "IN PROGRESS", "DONE");
        Console.WriteLine(new string('-', 92));

        TaskItem[] todoList = ToArray(todoTasks);
        TaskItem[] inProgressList = ToArray(inProgressTasks);
        TaskItem[] doneList = ToArray(doneTasks);

        int maxRows = Math.Max(todoList.Length, Math.Max(inProgressList.Length, doneList.Length));

        for (int i = 0; i < maxRows; i++)
        {
            string todoEntry = i < todoList.Length ? $"[{todoList[i].Id}] {todoList[i].Name}" : "";
            string inProgressEntry = i < inProgressList.Length ? $"[{inProgressList[i].Id}] {inProgressList[i].Name}" : "";
            string doneEntry = i < doneList.Length ? $"[{doneList[i].Id}] {doneList[i].Name}" : "";

            Console.WriteLine(
                "{0,-28} | {1,-28} | {2,-28}",
                Truncate(todoEntry, 26),
                Truncate(inProgressEntry, 26),
                Truncate(doneEntry, 26)
            );
        }

        Console.WriteLine("--------------------------------------------------------------------------------------------");
    }

    private TaskItem[] ToArray(IMyCollection<TaskItem> col)
    {
        TaskItem[] arr = new TaskItem[col.Count];
        int i = 0;

        IMyIterator<TaskItem> iterator = col.GetIterator();

        while (iterator.HasNext())
        {
            arr[i] = iterator.Next();
            i++;
        }

        return arr;
    }

    private string Truncate(string value, int max)
    {
        return value.Length <= max ? value : value.Substring(0, max - 3) + "...";
    }

    private string FormatStatus(TaskStatus status)
    {
        return status switch
        {
            TaskStatus.NotDone => "Not Done",
            TaskStatus.InProgress => "In Progress",
            TaskStatus.Done => "Done",
            _ => "Unknown"
        };
    }
}