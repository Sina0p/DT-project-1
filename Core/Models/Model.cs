public class TaskItem
{ 
    public int Id { get; set; } 
    public string Name { get; set; } 
    public bool Status { get; set; }
    public DateOnly CreationDate { get; set; }
    public int Priority { get; set; } 
    public TaskItem(int id, string name) 
    { 
        Id = id; 
        Name = name; 
        Status = false;
        CreationDate = DateOnly.FromDateTime(DateTime.Now);
        Priority = 1;
    } 
}

public class LinkedListNode<T>
{
    public T Value { get; set; }
    public LinkedListNode<T>? Next { get; set; }

    public LinkedListNode(T value)
    {
        Value = value;
        Next = null;
    }
}