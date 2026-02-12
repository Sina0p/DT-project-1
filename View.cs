using System.ComponentModel.Design;

public class View
{
    private Service service;
    public List<Task> tasks = new List<Task>();
    public bool exit = false;
    
    public void DisplayTasks()
    {
        
        Console.WriteLine("All tasks:\n");

        {
            foreach (Task task in tasks)
            {
                string status = task.Status ? "Done" : "Not Done";
                Console.WriteLine($"{task.Name} : ({status})");
            }
        }
    }

    public void to_do_list()
    {
        while(!exit)
        {
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Exit");
            string Answer_for_to_do = Console.ReadLine();

            if (Answer_for_to_do == "4")
            {
                exit = true;
            }
            else if (Answer_for_to_do == "3")
            {
                Console.Clear();
                DisplayTasks();
                Console.WriteLine("\n Type task integer to toggle a task");
                service.ToggleTask(int.Parse(Console.ReadLine()));
                //code hadnling voor 3
            }
            else if (Answer_for_to_do == "2")
            {
                Console.WriteLine("What do you want to delete");
                var delete = new Service(tasks);
                string Task_to_delete = Console.ReadLine();
                if(delete.DeleteTask(Task_to_delete))
                {
                    Console.WriteLine($"You have deleted {Task_to_delete}");
                    continue;
                }
                Console.WriteLine("This task does not exist");
                continue;
            }
            else if (Answer_for_to_do == "1")
            {
                Console.WriteLine("What would yo like to add");
                var add = new Service(tasks);
                string Task_to_add = Console.ReadLine();
                if(add.AddTask(Task_to_add))
                {
                    Console.WriteLine($"You have added {Task_to_add}");
                    continue;
                }
                Console.WriteLine("No space left to add a task");
                continue;
            }
            else
            {
                Console.WriteLine("wrong input");
                //code hadnling voor verkeerde input
            }
        }
    }
}

