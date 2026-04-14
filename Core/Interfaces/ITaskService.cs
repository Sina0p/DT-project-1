public interface ITaskService
{
    void Save();
    bool AddTask(string name, int priority);
    bool DeleteTask(int id);
    bool ToggleTask(int id);
    bool UpdateTask(int id, string newName, int newPriority);

    void DisplayTasks();
    void DisplayCompletedTasks();
    void DisplayByPriority(int priority);
    void DisplayKanban();
}