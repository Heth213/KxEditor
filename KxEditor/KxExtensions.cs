namespace KxExtension
{
    public static class Strings
    {
        public static void WriteToFile(this string input, string filePath)
        {
            using (var textWriter = new System.IO.StreamWriter(filePath, false))
            {
                textWriter.Write(input);
                return;
            }
        }
    }
}