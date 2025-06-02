namespace WebApplication2.Models.DTO;

public class ReturnTripDTO
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }
    public List<string> Countries { get; set; } = new List<string>();
    public List<ReturnClient> Clients { get; set; } = new List<ReturnClient>();
}

public class ReturnClient
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}