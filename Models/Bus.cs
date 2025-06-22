namespace auto_front.Models
{
    public class Bus
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public int Year { get; set; }

        public Bus(Guid id, string licensePlate, string brand, string model, int capacity, int year)
        {
            Id = id;
            LicensePlate = licensePlate;
            Brand = brand;
            Model = model;
            Capacity = capacity;
            Year = year;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Bus() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
