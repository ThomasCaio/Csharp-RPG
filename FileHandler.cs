namespace FileModule;
using Entities;
using System.IO;
using System;
using System.Text.RegularExpressions;

public static class FileHandler
{
    public static List<string> ListCharacters()
    {
        var files = Directory.GetFiles("./save");
        var filtered_characters = files.Select(m => Path.GetFileNameWithoutExtension(m));
        return filtered_characters.ToList();
    }

    public static void SaveCharacter(Character c)
    {
        string path = "./save";
        Directory.CreateDirectory(path);
        string character = Serializers.Character(c);
        File.WriteAllText($"{path}/{c.Name}.data", character);
    }

    public static void LoadCharacter(RPG.Game game, string name)
    {
        string path = "./save";
        var text = File.ReadAllText($"{path}/{name}.data");
        Character character = Serializers.DeCharacter(text);
        game.Player = character;
    }
}