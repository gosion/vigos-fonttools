using Vigos.Font.Abstractioins;

namespace Vigos.Font.Sfnt;

public interface IFontTableResolver
{
    IFontTable? Resolve(string name, IFontReader reader, in TableDirectory tableDirectory);
}