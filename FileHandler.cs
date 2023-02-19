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
        string path = "./saves";
        var text = File.ReadAllText($"{path}/{name}.data");
        Character character = Serializers.DeCharacter(text);
        game.Player = character;
    }
}

public class Test{
    public static void Run(){
        Character c = new Character("Demnok");
        var serial = Serializers.Character(c);
        var player = Serializers.DeCharacter(serial);
        FileModule.FileHandler.ListCharacters();
        File.WriteAllText("./tests/serial.test", serial);
        System.Console.WriteLine(player.Name);
    }
}