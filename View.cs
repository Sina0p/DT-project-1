public class View
{
    private Service service;

    public View()
    {
        IMyCollection<TaskItem> collection = ChooseCollection();
        service = new Service(collection);
    }

    private IMyCollection<TaskItem> ChooseCollection()
    {
        Console.WriteLine("Choose collection implementation:");
        Console.WriteLine("1. ArrayCollection");
        Console.WriteLine("2. LinkedListCollection");
        Console.WriteLine("3. BinarySearchTreeCollection");

        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                return new ArrayCollection<TaskItem>();

            case "2":
                // return new ArrayCollection<TaskItem>();

            case "3":
                // return new ArrayCollection<TaskItem>();

            default:
                Console.WriteLine("Invalid choice. Using ArrayCollection.");
                return new ArrayCollection<TaskItem>();
        }
    }

    public void to_do_list()
    {
        bool ongoing = true;

        while (ongoing)
        {
            Console.Clear();

            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Filter Task");
            Console.WriteLine("5. Exit");

            string answerCheck = Console.ReadLine()!;
            int answerForToDo;

            if (!int.TryParse(answerCheck, out answerForToDo) || answerForToDo < 1 || answerForToDo > 5)
            {
                answerForToDo = 5;
            }

            if (answerForToDo == 1)
            {
                Console.Clear();
                Console.WriteLine("Choose a task to add");

                string taskToAdd = Console.ReadLine()!;

                if (service.AddTask(taskToAdd))
                {
                    Console.WriteLine($"You have added {taskToAdd}");
                }
                else
                {
                    Console.WriteLine("No space left to add a task");
                }

                Console.ReadLine();
            }
            else if (answerForToDo == 2)
            {
                Console.Clear();

                if (!service.TaskCheck())
                {
                    Console.WriteLine("No tasks to delete...");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Choose a task to delete");
                service.DisplayTasks();

                Console.Write("\nEnter task ID: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int taskId))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.ReadLine();
                    continue;
                }

                bool deleted = service.DeleteTask(taskId);
                Console.WriteLine(deleted ? "Task deleted." : "Task ID not found.");

                Console.ReadLine();
            }
            else if (answerForToDo == 3)
            {
                Console.Clear();

                if (!service.TaskCheck())
                {
                    Console.WriteLine("No tasks to toggle...");
                    Console.ReadLine();
                    continue;
                }

                service.DisplayTasks();
                Console.Write("\nEnter task ID: ");

                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    if (!service.ToggleTask(id))
                    {
                        Console.WriteLine("Task not found");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }

                Console.ReadLine();
            }
            else if (answerForToDo == 4)
            {
                Console.Clear();
                Console.WriteLine("Filter not implemented yet.");
                Console.ReadLine();
            }
            else if (answerForToDo == 5)
            {
                Console.Clear();
                Console.WriteLine("Quitting program...");
                ongoing = false;
            }
        }
    }
}