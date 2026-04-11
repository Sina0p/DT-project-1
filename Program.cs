Console.WriteLine("Choose collection type:");
Console.WriteLine("1. Array");
Console.WriteLine("2. Linked List");
Console.WriteLine("3. Binary Search Tree");
Console.WriteLine("4. Hash Map");
Console.Write("Enter choice: ");

string choice = Console.ReadLine();

IMyCollection<TaskItem> collection;

if (choice == "2")
{
    collection = new LinkedListCollection<TaskItem>();
}
else if (choice == "3")
{
    collection = new BSTCollection<TaskItem>((a, b) => a.Id.CompareTo(b.Id));
}
else if (choice == "4")
{
    collection = new HashMapCollection<TaskItem>(task => task.GetHashCode());
}
else
{
    collection = new ArrayCollection<TaskItem>();
}

ITaskRepository repository = new Repository();

ITaskService service = new Service(collection, repository);
View view = new View(service);

view.to_do_list();