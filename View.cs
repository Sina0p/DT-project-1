using System;

public class View
{
    private readonly ITaskService _service;
    private bool _isKanbanMode = false;
    private User? _currentUser;

    public View(ITaskService service)
    {
        _service = service;
    }

    public bool Login()
    {
        Console.Clear();
        Console.WriteLine("==== LOGIN SYSTEM ====");
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        var user = (_service as Service)?.ValidateUser(username, password);

        if (user != null)
        {
            _currentUser = user;
            Console.WriteLine($"\nWelcome {user.Username}. Role: {(user.IsEmployee ? "Employee" : "Admin")}");
            Pause();
            return true;
        }

        Console.WriteLine("\nAccount doesn't exist. Access denied.");
        return false;
    }

    public void to_do_list()
    {
        if (_currentUser == null) return;

        while (true)
        {
            Console.Clear();
            if (_isKanbanMode)
            {
                _service.DisplayKanban();
            }
            else
            {
                Console.WriteLine("==== TO-DO LIST ====");
                _service.DisplayTasks();
            }

            Console.WriteLine("\nOptions:");

            if (_currentUser.IsEmployee)
            {
                DisplayEmployeeMenu();
                if (HandleEmployeeInput()) break;
            }
            else
            {
                DisplayAdminMenu();
                if (HandleAdminInput()) break;
            }
        }
    }

    private void DisplayAdminMenu()
    {
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. Delete Task");
        Console.WriteLine("3. Toggle Task Status");
        Console.WriteLine("4. Update Task");
        Console.WriteLine("5. Show Completed Tasks");
        Console.WriteLine("6. Filter by Priority");
        Console.WriteLine(_isKanbanMode ? "7. Toggle List View" : "7. Toggle Kanban View");
        Console.WriteLine("8. Save Changes");
        Console.WriteLine("9. Exit");
    }

    private void DisplayEmployeeMenu()
    {
        Console.WriteLine("1. Toggle Task Status");
        Console.WriteLine("2. Show Completed Tasks");
        Console.WriteLine("3. Filter by Priority");
        Console.WriteLine(_isKanbanMode ? "4. Toggle List View" : "4. Toggle Kanban View");
        Console.WriteLine("5. Save Changes");
        Console.WriteLine("6. Exit");
    }

    private bool HandleAdminInput()
    {
        Console.Write("\nChoose option: ");
        string input = Console.ReadLine() ?? "";

        switch (input)
        {
            case "1": AddTask(); break;
            case "2": DeleteTask(); break;
            case "3": ToggleTask(); break;
            case "4": UpdateTask(); break;
            case "5": _service.DisplayCompletedTasks(); Pause(); break;
            case "6": FilterPriority(); break;
            case "7": _isKanbanMode = !_isKanbanMode; break;
            case "8": _service.Save(); Pause(); break;
            case "9": return true;
            default: Console.WriteLine("Invalid option"); Pause(); break;
        }
        return false;
    }

    private bool HandleEmployeeInput()
    {
        Console.Write("\nChoose option: ");
        string input = Console.ReadLine() ?? "";

        switch (input)
        {
            case "1": ToggleTask(); break;
            case "2": _service.DisplayCompletedTasks(); Pause(); break;
            case "3": FilterPriority(); break;
            case "4": _isKanbanMode = !_isKanbanMode; break;
            case "5": _service.Save(); Pause(); break;
            case "6": return true;
            default: Console.WriteLine("Invalid option"); Pause(); break;
        }
        return false;
    }

    private void AddTask()
    {
        Console.Write("Name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Priority (1-5): ");
        int.TryParse(Console.ReadLine(), out int priority);
        _service.AddTask(name, priority);
    }

    private void DeleteTask()
    {
        Console.Write("ID: ");
        int.TryParse(Console.ReadLine(), out int id);
        if (!_service.DeleteTask(id)) { Console.WriteLine("Task not found"); Pause(); }
    }

    private void ToggleTask()
    {
        Console.Write("ID to toggle: ");
        int.TryParse(Console.ReadLine(), out int id);
        if (!_service.ToggleTask(id)) { Console.WriteLine("Task not found"); Pause(); }
    }

    private void UpdateTask()
    {
        Console.Write("ID: ");
        int.TryParse(Console.ReadLine(), out int id);
        Console.Write("New name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("New priority: ");
        int.TryParse(Console.ReadLine(), out int priority);
        if (!_service.UpdateTask(id, name, priority)) { Console.WriteLine("Task not found"); Pause(); }
    }

    private void FilterPriority()
    {
        Console.Write("Priority: ");
        int.TryParse(Console.ReadLine(), out int priority);
        _service.DisplayByPriority(priority);
        Pause();
    }

    private void Pause()
    {
        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }
}