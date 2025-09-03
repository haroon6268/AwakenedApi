namespace AwakenedApi.models;

public class Todo
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Rank? Rank { get; set; }
    public int? Xp{ get; set; }
    public int? Gold { get; set; }
    public string? UserId { get; set; }
    public DateTimeOffset? Createddate { get; set; }
    public DateTimeOffset? Duedate { get; set; }
    public bool? Complete { get; set; }
    
}

public enum Rank
{
   DRANK = 0,
   CRANK = 1,
   BRANK = 2,
   ARANK = 3,
   SRANK = 4
}