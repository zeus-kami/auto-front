namespace auto_front.Dtos.Bus;

public class CreateBus(string licensePlate, string brand, string model, int capacity, int year)
{
    public string LicensePlate { get; set; } = licensePlate;
    public string Brand { get; set; } = brand;
    public string Model { get; set; } = model;
    public int Capacity { get; set; } = capacity;
    public int Year { get; set; } = year;
}