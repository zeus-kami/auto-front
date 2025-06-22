// See https://aka.ms/new-console-template for more information
using auto_front.Services;

Console.WriteLine("Hello, World!");
UserService user = new(new auto_front.DBContext.BusContext());
var a = user.Login("testuser", "testpassword1").GetAwaiter().GetResult();
Console.WriteLine(a.Success ? "Login successful!" : $"Login failed: {a.Message}");
var b = user.Login("testuser", "testpassword").GetAwaiter().GetResult();
Console.WriteLine(b.Success ? "Login successful!" : $"Login failed: {b.Message}");