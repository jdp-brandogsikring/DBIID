
using DBIID.Application.Services;

var passwords = new List<string>()
{
    "password",
    "P@ssw0rd",
    "1234",
    "Ladida",
    "EtMegetLangtPassword"
};

var passwordService = new PasswordService();

var warmUp = passwordService.IncryptPassword("Test", 1);


        Console.WriteLine($"X;Time;Password;Count;Hashed Password;");

for (int i = 1; i < 10; i++)
{


    foreach (var password in passwords)
    {
        DateTime start = DateTime.Now;
        var hashedPassword = passwordService.IncryptPassword(password, i);
        Console.WriteLine($"{i};{DateTime.Now - start};{password};{hashedPassword.Item2};{hashedPassword.Item1}");
    }

}
Console.ReadLine();