public class Task 
{ 
    public int Id { get; set; } 
    public string Name { get; set; } 
    public bool Status { get; set; } 
    public Task(int id, string name, bool status) 
    { 
        Id = id; 
        Name = name; 
        Status = status; 
    } 
}