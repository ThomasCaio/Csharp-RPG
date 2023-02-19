namespace FileModule;
using Entities;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Newtonsoft.Json;

public static class CharacterSerializer
{
    public static void Serialize(Character c)
    {
        var userDataString = JsonConvert.SerializeObject(c);
        File.WriteAllText("character.txt", userDataString);
        Console.WriteLine(c);
    }

    public static void Deserialize()
    {
        var text = File.ReadAllText("character.txt");
        Character c = JsonConvert.DeserializeObject<Character>(text);
        System.Console.WriteLine((c));
    }
}


public class Test{
    public static void Run(){
        Character c = new Character("Demnok");
        CharacterSerializer.Serialize(c);
        CharacterSerializer.Deserialize();
    }
}
