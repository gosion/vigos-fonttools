using Vigos.Font.Abstractioins;
using Vigos.Font.Streaming;

namespace Vigos.Font.Sfnt;

public class TableDirectory : IFontModel
{
    public string Tag { get; set; } = "";
    public uint Checksum { get; set; }
    public uint Offset { get; set; }
    public uint Length { get; set; }
    public byte[] Content { get; set; } = [];
    public IFontTable? FontTable { get; set; }

    public void Decode(IFontReader reader)
    {
        Tag = reader.ReadString(4);
        Checksum = reader.ReadUInt32();
        Offset = reader.ReadUInt32();
        Length = reader.ReadUInt32();
    }

    public void Encode(IFontWriter writer)
    {
        writer.WriteString(Tag);
        writer.WriteUInt32(Checksum);
        writer.WriteUInt32(Offset);
        writer.WriteUInt32(Length);
    }

    public void DecodeFontTables(IFontReader reader)
    {
        reader.MoveTo(Offset);
        var buffer = new byte[Length];
        reader.Read(buffer, 0, Convert.ToInt32(Length));
        Content = buffer;

        FontTable = FontStreamingFactory.GetFontTableResolver()?.Resolve(Tag, reader, this);
    }

    public void EncodeFontTables(IFontWriter writer)
    {
        writer.MoveTo(Offset);
        writer.Write(Content, 0, Convert.ToInt32(Length));
    }
}