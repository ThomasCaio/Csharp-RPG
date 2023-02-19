// TODO: Player Serializer to network and save/load.

namespace FileModule;
using Entities;
using Newtonsoft.Json;

public static class Serializers
{
    public static string Character(Character c)
    {
        var obj = JsonConvert.SerializeObject(c);
        return obj;
    }

    // public static void Deserialize()
    // {
    //     var text = File.ReadAllText("character.txt");
    //     Character c = JsonConvert.DeserializeObject<Character>(text);
    //     System.Console.WriteLine((c));
    // }
}


public class Test{
    public static void Run(){
        Character c = new Character("Demnok");
        Serializers.Character(c);
        Serializers.Deserialize();
    }
}