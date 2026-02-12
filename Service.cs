
class List
{
    Task[] todo = new Task[10];

    public string[] DeleteTask(string taskToDelete)
    {
        foreach(Task task in todo)
        {
            if (taskToDelete == task.Name)
            {
                
            }
        }
        return null;
    }
}