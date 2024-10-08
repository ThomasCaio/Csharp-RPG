namespace FileModule;
using EntityModule;
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
        var c = JsonConvert.DeserializeObject<Character>(text, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        return c!;
    }
}