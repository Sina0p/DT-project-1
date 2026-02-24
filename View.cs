using System.ComponentModel.Design;

public class View
{   
    private Service service = new Service();
    public void to_do_list()
    {
        bool ongoing = true;

        while (ongoing)
        {
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Exit");
            string Answer_for_to_do = Console.ReadLine();

            if (Answer_for_to_do == "4")
            {
                Console.WriteLine("ok 4");
                ongoing = false;
            }
            else if (Answer_for_to_do == "3")
            {
                Console.Clear();
                service.DisplayTasks();
                Console.WriteLine("\n Type task integer to toggle a task");
                service.ToggleTask(int.Parse(Console.ReadLine()));
                //code hadnling voor 3
            }
            else if (Answer_for_to_do == "2")
            {
                Console.WriteLine("What do you want to delete");
                string Task_to_delete = Console.ReadLine();
                if (service.DeleteTask(Task_to_delete))
                {
                    Console.WriteLine($"You have deleted {Task_to_delete}");
                }
                else
                {
                    Console.WriteLine("This task does not exist");
                }
            }
            else if (Answer_for_to_do == "1")
            {
                Console.WriteLine("What would yo like to add");
                string Task_to_add = Console.ReadLine();
                if (service.AddTask(Task_to_add))
                {
                    Console.WriteLine($"You have added {Task_to_add}");
                }
                else
                {
                    Console.WriteLine("No space left to add a task");
                }
            }
            else
            {
                Console.WriteLine("wrong input");
                //code hadnling voor verkeerde input
            }           
        }
    }
}