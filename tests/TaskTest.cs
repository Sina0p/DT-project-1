public class FakeRepository : ITaskRepository
{
    public void LoadTasks(IMyCollection<TaskItem> target)
    {
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
    }
}

public class ServiceTests
{
    public static void AddTask_ShouldAssignIncrementedId()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();

        var service = new Service(collection, repo);

        service.AddTask("Task1", 1);
        service.AddTask("Task2", 2);

        var task = collection.FindBy(1, (t, id) => t.Id == id);

        if (task == null)
        {
            throw new Exception("Test failed: task is null");
        }

        if (task.Id != 1)
        {
            throw new Exception($"Test failed: expected Id = 1 but got {task.Id}");
        }

        Console.WriteLine("AddTask_ShouldAssignIncrementedId passed!");
    }

    public static void Test_DeleteTask_ShouldRemoveExistingTask()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        service.AddTask("Task", 1);

        bool result = service.DeleteTask(0);

        if (!result)
            throw new Exception("Test failed: expected true but got false");

        if (collection.Count != 0)
            throw new Exception($"Test failed: expected count 0 but got {collection.Count}");

        Console.WriteLine("DeleteTask_ShouldRemoveExistingTask passed");
    }

    public static void Test_DeleteTask_ShouldReturnFalse_WhenNotFound()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        bool result = service.DeleteTask(999);

        if (result)
            throw new Exception("Test failed: expected false but got true");

        Console.WriteLine("DeleteTask_ShouldReturnFalse_WhenNotFound passed");
    }

    public static void Test_ToggleTask_ShouldFlipStatus()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        service.AddTask("Task", 1);

        service.ToggleTask(0);
        var task = collection.FindBy(0, (t, id) => t.Id == id);

        if (!task.Status)
            throw new Exception("Test failed: expected true after first toggle");

        service.ToggleTask(0);

        if (task.Status)
            throw new Exception("Test failed: expected false after second toggle");

        Console.WriteLine("ToggleTask_ShouldFlipStatus passed");
    }

    public static void Test_UpdateTask_ShouldUpdateFields()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        service.AddTask("Old", 1);

        service.UpdateTask(0, "New", 5);

        var task = collection.FindBy(0, (t, id) => t.Id == id);

        if (task.Name != "New")
            throw new Exception("Test failed: name not updated");

        if (task.Priority != 5)
            throw new Exception("Test failed: priority not updated");

        Console.WriteLine("UpdateTask_ShouldUpdateFields passed");
    }
}