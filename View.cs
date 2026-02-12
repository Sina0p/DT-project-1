public class View
{
    private Service service;
    public List<Task> tasks = new List<Task>();
    
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
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. Remove Task");
        Console.WriteLine("3. Toggle Task State");
        Console.WriteLine("4. Exit");
        string Answer_for_to_do = Console.ReadLine();

        if (Answer_for_to_do == "4")
        {
            Console.WriteLine("ok 4");
            //code hadnling voor 4
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
            Console.WriteLine("ok 2");
            //code hadnling voor 2
        }
        else if (Answer_for_to_do == "1")
        {
            Console.WriteLine("ok 1");
            //code hadnling voor 1
        }
        else
        {
            Console.WriteLine("wrong input");
            //code hadnling voor verkeerde input
        }
    }
}

