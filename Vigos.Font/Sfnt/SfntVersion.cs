using Vigos.Font.Abstractioins;

namespace Vigos.Font.Sfnt;

public enum FontType
{
    Unknown = -1,
    TrueType = 1,
    OpenType
}

public class SfntVersion : IFontModel
{
    public ushort Major { get; set; }
    public ushort Minor { get; set; }

    public static SfntVersion Parse(IFontReader reader)
    {
        var version = new SfntVersion();
        version.Decode(reader);
        return version;
    }

    public void Decode(IFontReader reader)
    {
        Major = reader.ReadUInt16();
        Minor = reader.ReadUInt16();
    }

    public FontType GetFontType()
    {
        if (
            Major == 0x0001 && Minor == 0x0000
        )
        {
            if (Major == 0x7474 && Minor == 0x6366 || Major == 0x7472 && Minor == 0x7565)
            {
                return FontType.TrueType;
            }
            else if (Major == 0x4F54 && Minor == 0x544F)
            {
                return FontType.OpenType;
            }
        }

        return FontType.Unknown;
    }

    public void Encode(IFontWriter writer)
    {
        writer.WriteUInt16(Major);
        writer.WriteUInt16(Minor);
    }
}