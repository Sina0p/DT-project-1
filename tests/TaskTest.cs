using System;

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

        var firstTask = collection.FindBy(1, (t, id) => t.Id == id);
        var secondTask = collection.FindBy(2, (t, id) => t.Id == id);

        if (firstTask == null)
            throw new Exception("Test failed: Expected first task with Id = 1 to exist");

        if (secondTask == null)
            throw new Exception("Test failed: Expected second task with Id = 2 to exist");

        Console.WriteLine("AddTask_ShouldAssignIncrementedId passed!");
    }

    public static void Test_DeleteTask_ShouldRemoveExistingTask()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        service.AddTask("Task", 1);

        bool result = service.DeleteTask(1);

        if (!result)
            throw new Exception("Test failed: Expected DeleteTask to return true");

        if (collection.Count != 0)
            throw new Exception($"Test failed: Expected count 0, got {collection.Count}");

        Console.WriteLine("DeleteTask_ShouldRemoveExistingTask passed!");
    }

    public static void Test_DeleteTask_ShouldReturnFalse_WhenNotFound()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        bool result = service.DeleteTask(999);

        if (result)
            throw new Exception("Test failed: Expected DeleteTask to return false");

        Console.WriteLine("DeleteTask_ShouldReturnFalse_WhenNotFound passed!");
    }

    public static void Test_ToggleTask_ShouldFlipStatus()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        service.AddTask("Task", 1);

        bool firstToggle = service.ToggleTask(1);
        var task = collection.FindBy(1, (t, id) => t.Id == id);

        if (!firstToggle)
            throw new Exception("Test failed: First toggle should return true");

        if (task == null)
            throw new Exception("Test failed: Task not found");

        if (task.Status != TaskStatus.InProgress)
            throw new Exception("Test failed: Status should be InProgress after first toggle");

        bool secondToggle = service.ToggleTask(1);

        if (!secondToggle)
            throw new Exception("Test failed: Second toggle should return true");

        if (task.Status != TaskStatus.Done)
            throw new Exception("Test failed: Status should be Done after second toggle");

        Console.WriteLine("ToggleTask_ShouldFlipStatus passed!");
    }

    public static void Test_UpdateTask_ShouldUpdateFields()
    {
        var collection = new ArrayCollection<TaskItem>();
        var repo = new FakeRepository();
        var service = new Service(collection, repo);

        service.AddTask("Old", 1);

        bool result = service.UpdateTask(1, "New", 5);
        var task = collection.FindBy(1, (t, id) => t.Id == id);

        if (!result)
            throw new Exception("Test failed: Expected UpdateTask to return true");

        if (task == null)
            throw new Exception("Test failed: Task not found");

        Console.WriteLine("UpdateTask_ShouldUpdateFields passed!");
    }
}