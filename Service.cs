
class List
{
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

}