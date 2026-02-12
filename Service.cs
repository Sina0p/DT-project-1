using System.ComponentModel;
using System.Runtime.CompilerServices;

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

    public bool DeleteTask(string taskToDelete)
    {
        int index = FindTaskIndex(taskToDelete);

        if (index == -1)
            return false;

        for (int i = index; i < todo.Length - 1; i++)
        {
            todo[i] = todo[i + 1];
        }

        todo[todo.Length - 1] = null;
        return true;
    }

    public bool AddTask(string name)
    {
        for(int i = 0; i < todo.Length; ++i)
        {
            if(todo[i] == null)
            {
                Task taskToAdd = new Task(i, name);
                todo[i] = taskToAdd;
                // Console.WriteLine($"Id: {todo[i].Id}, Name: {todo[i].Name}, Status: {todo[i].Status}");
                return true;
            }
        }
        return false;
    }

}