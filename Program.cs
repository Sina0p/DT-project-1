using System;

Console.WriteLine("Run tests? (y/n)");
string? runTests = Console.ReadLine();

if (runTests?.ToLower() == "y")
{
    RunTests();
    return;
}

Console.Clear();
Console.WriteLine("Choose collection type:");
Console.WriteLine("1. Array");
Console.WriteLine("2. Linked List");
Console.WriteLine("3. Hash Map");
Console.Write("Enter choice: ");

string? choice = Console.ReadLine();

IMyCollection<TaskItem> collection;

if (choice == "2")
{
    collection = new LinkedListCollection<TaskItem>();
}
else if (choice == "3")
{
    collection = new HashMapCollection<TaskItem>(task => task.Id);
}
else
{
    collection = new ArrayCollection<TaskItem>();
}

ITaskRepository repository = new Repository();
ITaskService service = new Service(collection, repository);
View view = new View(service);

if (!view.Login())
{
    Console.WriteLine("\nProgram terminated due to failed login.");
    return; 
}

view.to_do_list();

Console.WriteLine("\nClosing... changes will be saved.");
service.Save();
Console.WriteLine("Application closed.");

void RunTests()
{
    Console.WriteLine("Running tests...\n");

    try
    {
        ServiceTests.AddTask_ShouldAssignIncrementedId();
        ServiceTests.Test_DeleteTask_ShouldRemoveExistingTask();
        ServiceTests.Test_DeleteTask_ShouldReturnFalse_WhenNotFound();
        ServiceTests.Test_ToggleTask_ShouldFlipStatus();
        ServiceTests.Test_UpdateTask_ShouldUpdateFields();
        Console.WriteLine("All tests passed!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test failed: {ex.Message}");
    }
}