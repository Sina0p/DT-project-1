public class View
{
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
        }
        else
        {
            Console.WriteLine("ok ");
        }
    }
}

