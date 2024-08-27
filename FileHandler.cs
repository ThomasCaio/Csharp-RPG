namespace FileModule;
using Entities;
using System.IO;

public static class FileHandler
{

    public static List<string> ListCharacters()
    {
        string path = "./saves";
        Directory.CreateDirectory(path);
        var files = Directory.GetFiles(path);
        var filtered_characters = files.Select(m => Path.GetFileNameWithoutExtension(m));
        return filtered_characters.ToList();
    }

    public static void SaveCharacter(Character c)
    {
        string path = "./saves";
        Directory.CreateDirectory(path);
        string character = Serializers.Character(c);
        File.WriteAllText($"{path}/{c.Name}.data", character);
    }

    public static void LoadCharacter(RPG.Game game, string name)
    {
        string path = $"./saves/{name}.data";
        if (File.Exists(path))
        {
            var text = File.ReadAllText(path);
            Character character = Serializers.DeCharacter(text);
            game.Player = character;
        }
        else
        {
            throw new FileNotFoundException($"Character save file '{name}.data' not found.");
        }
    }
}

public class Test{
    public static void Run()
    {
        Character c = new Character("Demnok")
        {
            MaxHealth = 220,
            Health = 150
        };

        // Verifique os valores antes da serialização
        Console.WriteLine($"Before Serialization - Name: {c.Name}, Health: {c.Health}, MaxHealth: {c.MaxHealth}");

        var serial = Serializers.Character(c);
        File.WriteAllText("./tests/serial.test", serial);

        var player = Serializers.DeCharacter(serial);

        // Verifique os valores após a desserialização
        Console.WriteLine($"After Deserialization - Name: {player.Name}, Health: {player.Health}, MaxHealth: {player.MaxHealth}");

        // Compara os valores
        if (player.Health == c.Health && player.MaxHealth == c.MaxHealth)
        {
            Console.WriteLine("Deserialization test passed!");
        }
        else
        {
            Console.WriteLine("Deserialization test failed!");
            Console.WriteLine($"Expected Health: {c.Health}, Got: {player.Health}");
            Console.WriteLine($"Expected MaxHealth: {c.MaxHealth}, Got: {player.MaxHealth}");
        }
    }
}