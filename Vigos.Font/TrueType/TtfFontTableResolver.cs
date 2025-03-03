using Vigos.Font.Abstractioins;
using Vigos.Font.Sfnt;
using Vigos.Font.TrueType.Models;

namespace Vigos.Font.TrueType;

delegate IFontTable CreateFontTable(IFontReader reader, in TableDirectory tableDirectory);

public class TtfFontTableResolver : IFontTableResolver
{
    private readonly Dictionary<string, CreateFontTable> _resolvers = new()
    {
        // ["cmap"] = CmapTable.Parse,
        ["name"] = NameTable.Parse
    };

    public IFontTable? Resolve(string name, IFontReader reader, in TableDirectory tableDirectory)
    {
        if (!_resolvers.TryGetValue(name, out var resolver)) return null;

        return resolver(reader, tableDirectory);
    }
}