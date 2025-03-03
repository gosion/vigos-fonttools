using Vigos.Font.Abstractioins;

namespace Vigos.Font.TrueType.Models;

public class TtcHeader : IFontModel
{
    public string Tag { get; set; } = "ttcf";
    public short Major { get; set; }
    public short Minor { get; set; }
    public int NumFonts { get; set; }
    public int[]? TableDirectoryOffsets { get; set; }
    public int? DsigTag { get; set; }
    public int? DsigLength { get; set; }
    public int? DsigOffset { get; set; }

    public static TtcHeader Parse(IFontReader reader)
    {
        var header = new TtcHeader();
        header.Decode(reader);
        return header;
    }

    public void Decode(IFontReader reader)
    {
        Tag = reader.ReadString(4);
        Major = reader.ReadInt16();
        Minor = reader.ReadInt16();
        NumFonts = reader.ReadInt32();

        TableDirectoryOffsets = new int[NumFonts];

        for (var i = 0; i < NumFonts; i++)
        {
            TableDirectoryOffsets[i] = reader.ReadInt32();
        }

        if (Major == 2)
        {
            DsigTag = reader.ReadInt32();
            DsigLength = reader.ReadInt32();
            DsigOffset = reader.ReadInt32();
        }
    }

    public void Encode(IFontWriter writer)
    {
        throw new NotImplementedException();
    }
}