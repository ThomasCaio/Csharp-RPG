namespace Logging
{
    public static class Debug
    {
        static readonly string filePath = "tests";

        public static void Write(string data = "", string filename = "main")
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            File.AppendAllText($"{filePath}/{filename}.test", data + Environment.NewLine);
        }
    }
}
