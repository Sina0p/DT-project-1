public interface ITaskService
{
    bool AddTask(string name);
    bool DeleteTask(int id);
    bool ToggleTask(int id);
    bool TaskCheck();
    void DisplayTasks();
}