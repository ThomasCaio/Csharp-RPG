namespace Logging;

public static class Debug
{
    static string filePath = "tests";

    public static void Write(string data="", string filename="main"){
        File.AppendAllText($"{filePath}/{filename}.test", data + Environment.NewLine);
    }
}
