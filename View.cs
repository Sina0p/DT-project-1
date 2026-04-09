using System;

public class View
{
    private readonly ITaskService _service;

    public View(ITaskService service)
    {
        _service = service;
    }

    public void to_do_list()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== TO-DO LIST ====\n");

            _service.DisplayTasks();

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Delete Task");
            Console.WriteLine("3. Toggle Task");
            Console.WriteLine("4. Exit");

            Console.Write("\nChoose option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AddTask();
                    break;

                case "2":
                    DeleteTask();
                    break;

                case "3":
                    ToggleTask();
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Invalid option. Press any key...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void AddTask()
    {
        Console.Write("\nEnter task name: ");
        string name = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
        {
            _service.AddTask(name);
        }
        else
        {
            Console.WriteLine("Invalid input.");
            Console.ReadKey();
        }
    }

    private void DeleteTask()
    {
        Console.Write("\nEnter task ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (!_service.DeleteTask(id))
            {
                Console.WriteLine("Task not found.");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
            Console.ReadKey();
        }
    }

    private void ToggleTask()
    {
        Console.Write("\nEnter task ID to toggle: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (!_service.ToggleTask(id))
            {
                Console.WriteLine("Task not found.");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
            Console.ReadKey();
        }
    }
}