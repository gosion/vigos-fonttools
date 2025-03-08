namespace Vigos.Font.Abstractioins;

public interface IFontInfo
{
    bool IsBold { get; }
    bool IsItalic { get; }
    bool IsRegular { get; }
    Dictionary<string, string> GetFontFamilies();
    Dictionary<string, string> GetFontSubFamilies();
}