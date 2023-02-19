// TODO: Player Serializer to network and save/load.

namespace FileModule;
using Entities;
using Newtonsoft.Json;

public static class Serializers
{
    public static string Character(Character c)
    {
        var obj = JsonConvert.SerializeObject(c, Formatting.Indented, new JsonSerializerSettings{ TypeNameHandling = TypeNameHandling.Auto });
        return obj;
    }

    public static Character DeCharacter(string text)
    {
        Character c = JsonConvert.DeserializeObject<Character>(text, new JsonSerializerSettings{ TypeNameHandling = TypeNameHandling.Auto });
        System.Console.WriteLine((c));
        return c;
    }
}


public class Test{
    public static void Run(){
        Character c = new Character("Demnok");
        // Serializers.Character(c);
        // Serializers.Deserialize();
        FileModule.FileHandler.ListCharacters();
    }
}