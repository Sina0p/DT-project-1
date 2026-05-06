public interface ITaskService
{
    void Save();
    bool AddTask(string name, int priority);
    bool DeleteTask(int id);
    bool ToggleTask(int id);
    bool SetTaskStatus(int id, TaskStatus status);
    bool UpdateTask(int id, string newName, int newPriority);
    void DisplayTasks();
    void DisplayCompletedTasks();
    void DisplayByStatus(TaskStatus status);
    void DisplayByPriority(int priority);
    void DisplayKanban();
}