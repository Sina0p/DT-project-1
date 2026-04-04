// [TestClass]
// public class TaskTest
// {
//     [DataTestMethod]
//     [DataRow("random_1")]
//     public void AddTaskTest(string name)
//     {
//         Service service = new Service();
//         var tasksBeforeAdding = service.loadTask();
//         int countBefore = tasksBeforeAdding.Count;

//         service.AddTask(name);
//         var tasksAfterAdding = service.loadTask();
//         int countAfter = tasksAfterAdding.Count;

//         Assert.AreEqual(1, countAfter-countBefore);
//     }
// }