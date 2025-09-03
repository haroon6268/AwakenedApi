namespace AwakenedApi.models;

public class User
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int? Strength { get; set; }
    public int? Speed { get; set; }
    public int? Endurance { get; set; }
    public int? Intelligence { get; set; }
    public int? Charisma { get; set; }
    public int? Gold { get; set; }
    public int? Xp { get; set; }
    public DateTimeOffset? Createdate { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
}