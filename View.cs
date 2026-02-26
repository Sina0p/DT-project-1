public class View
{   
    private Service service = new Service();
    public void to_do_list()
    {
        bool ongoing = true;

        while (ongoing)
        {
            Console.Clear();

            //menu
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Filter Task");
            Console.WriteLine("5. Exit");

            string Answer_check = Console.ReadLine()!;
            int Answer_for_to_do;

            //check voor keuzemenu anders quit (4)
            if (!int.TryParse(Answer_check, out Answer_for_to_do) || Answer_for_to_do < 1 || Answer_for_to_do > 4)
            {
                Answer_for_to_do = 4;
            }

            //adding tasks
            if (Answer_for_to_do == 1)
            {
                Console.Clear();
                Console.WriteLine("Choose a task to add");
            
                string Task_to_add = Console.ReadLine()!;
    
                if (service.AddTask(Task_to_add))
                {
                    Console.WriteLine($"You have added {Task_to_add}");
                }
            
                else
                {
                    Console.WriteLine("No space left to add a task");
                }
            }
        
            //deleting tasks
            else if (Answer_for_to_do == 2)
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
    
            //toggle task status
            else if (Answer_for_to_do == 3)
            {
                Console.Clear();

                if (!service.TaskCheck())
                {
                    Console.WriteLine("No tasks to toggle...");
                    Console.ReadLine();
                    continue;
                }

                service.DisplayTasks();

                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    if (!service.ToggleTask(id))
                    {
                        Console.WriteLine("Task not found");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input");
                    Console.ReadLine();
                }
            }
            
            //quit program
            else if (Answer_for_to_do == 4)
            {
                Console.Clear();
                Console.WriteLine("Choose filter to filter on");
                Console.WriteLine("\n( 1 ) Priority");
                Console.WriteLine("( 2 ) Status");
                Console.WriteLine("( 3 ) Creation date");

                Console.Write("\nEnter filter ID: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int filterID))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.ReadLine();
                    continue;
                }

                
            }

            else if (Answer_for_to_do == 5)
            {
                Console.Clear();
                Console.WriteLine("Quitting program...");

                ongoing = false;
            }         
        }
    }
}