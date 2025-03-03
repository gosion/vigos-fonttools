using Vigos.Font.Abstractioins;
using Vigos.Font.Sfnt;
using Vigos.Font.Streaming;

namespace Vigos.Font.TrueType;

public class TtfParser
{
    public IFontInfo Parse(string filename)
    {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Parse(stream);
    }

    public IFontInfo Parse(Stream stream)
    {
        using var reader = FontStreamingFactory.GetFontReader(stream);
        return Parse(reader);
    }

    public IFontInfo Parse(IFontReader reader)
    {
        var subOffsetTable = new OffsetSubTable();
        subOffsetTable.Decode(reader);

        return new TtfFontInfo(subOffsetTable);
    }
}