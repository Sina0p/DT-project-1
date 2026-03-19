public class Task 
{ 
    public int Id { get; set; } 
    public string Name { get; set; } 
    public bool Status { get; set; }
    public DateOnly CreationDate { get; set; }
    public int Priority { get; set; } 
    public Task(int id, string name) 
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
    public T Waarde { get; set; }
    public LinkedListNode<T>? Volgende { get; set; }

    public LinkedListNode(T waarde)
    {
        Waarde = waarde;
        Volgende = null;
    }
}