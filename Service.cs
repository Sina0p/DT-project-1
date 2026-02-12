public class Service
{
    private List<Task> tasks;
    private View view;

    public Service(List<Task> tasks)
    {
        this.tasks = tasks;
    }

    public void ToggleTask(int id)
    {
        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                task.Status = !task.Status;

                view.DisplayTasks();
                Console.WriteLine($"\n Status of {task.Name} toggled");

                return;
            }
        }
    }

    Task[] todo = new Task[99];

    public int FindTaskIndex(string taskToDelete)
    {
        for (int i = 0; i < todo.Length; i++)
        {
            if (todo[i] != null && todo[i].Name == taskToDelete)
            {
                return i;
            }
        }
        return -1;
    }

    public Task[] DeleteTask(string taskToDelete)
    {
        int index = FindTaskIndex(taskToDelete);

        if (index == -1)
            return todo;

        for (int i = index; i < todo.Length - 1; i++)
        {
            todo[i] = todo[i + 1];
        }

        todo[todo.Length - 1] = null;
        return todo;
    }

}