public interface ITaskRepository
{
    void LoadTasks(IMyCollection<TaskItem> target);
    void SaveTasks(IMyCollection<TaskItem> tasks);
}