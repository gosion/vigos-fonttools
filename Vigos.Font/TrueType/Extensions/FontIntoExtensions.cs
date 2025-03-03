using Vigos.Font.Abstractioins;
using Vigos.Font.Sfnt;
using Vigos.Font.TrueType.Models;

namespace Vigos.Font.TrueType.Extensions;

public static class FontIntoExtensions
{
    public static NameRecrod[] GetNameRecrods(this IFontInfo font)
    {
        return (font as TtfFontInfo)?.GetNameRecrods() ?? Array.Empty<NameRecrod>();
    }
    
    public static TableDirectory[] GetTableDirectories(this IFontInfo font)
    {
        return (font as TtfFontInfo)?.GetTableDirectories() ?? Array.Empty<TableDirectory>();
    }

    public static CmapTable? GetCmapTable(this IFontInfo font)
    {
        return (font as TtfFontInfo)?.GetCmapTable();
    }
}
