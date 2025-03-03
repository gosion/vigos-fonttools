namespace Vigos.FontTest;

public static class Util
{
    public static string GetFontDir() => Path.Combine(AppContext.BaseDirectory, "data/fonts");
    public static string GetFontPath(string filename) => Path.Combine(GetFontDir(), filename);
}