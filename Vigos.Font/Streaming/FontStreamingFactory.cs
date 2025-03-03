using Vigos.Font.Abstractioins;
using Vigos.Font.Sfnt;

namespace Vigos.Font.Streaming;

public static class FontStreamingFactory
{
    private static IFontTableResolver? _fontTableResolver;

    public static IFontReader GetFontReader(Stream stream)
    {
        return new FontReader(stream);
    }

    public static IFontWriter GetFontWriter(Stream stream)
    {
        return new FontWriter(stream);
    }

    public static IFontTableResolver? GetFontTableResolver()
    {
        return _fontTableResolver;
    }

    public static void SetFontTableResolver(IFontTableResolver fontTableResolver)
    {
        _fontTableResolver = fontTableResolver;
    }
}
