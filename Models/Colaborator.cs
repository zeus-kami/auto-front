using auto_front.Models;
namespace auto_front.Models;

public class Collaborator
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public Guid BusId { get; set; }
    public Bus Bus { get; set; }

    public Collaborator(Guid id, string name, string phone, string email, Guid busId, Bus bus)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Email = email;
        BusId = busId;
        Bus = bus;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Collaborator() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}