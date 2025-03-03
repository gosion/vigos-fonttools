using System.Buffers;
using System.Text;
using Vigos.Font.Abstractioins;
using Vigos.Font.Sfnt;

namespace Vigos.Font.TrueType.Models;

public class NameTable : IFontTable
{
    public ushort Format { get; set; }
    public ushort Count { get; set; }
    public ushort StringOffset { get; set; }
    public NameRecrod[] NameRecrods { get; set; } = [];
    public string Variable { get; set; } = "";

    public static NameTable Parse(IFontReader reader, in TableDirectory tableDirectory)
    {
        var table = new NameTable();
        table.Decode(reader, tableDirectory);
        return table;
    }

    public void Decode(IFontReader reader, in TableDirectory tableDirectory)
    {
        reader.MoveTo(tableDirectory.Offset);
        Format = reader.ReadUInt16();
        Count = reader.ReadUInt16();
        StringOffset = reader.ReadUInt16();
        var nameRecords = new NameRecrod[Count];

        for (var i = 0; i < Count; i++)
        {
            var nameRecord = new NameRecrod();
            nameRecord.Decode(reader);
            nameRecords[i] = nameRecord;
        }

        for (var i = 0; i < Count; i++)
        {
            var nameRecord = nameRecords[i];
            var buffer = ArrayPool<byte>.Shared.Rent(nameRecord.Length);
            var readCount = reader.Peek(buffer, 0, nameRecord.Length, nameRecord.Offset);

            if (readCount != nameRecord.Length)
            {
                throw new EndOfStreamException();
            }

            var value = nameRecord.PlatformID switch
            {
                PlatformIdentifier.Microsoft => Encoding.BigEndianUnicode.GetString(buffer, 0, nameRecord.Length),
                PlatformIdentifier.Unicode => Encoding.BigEndianUnicode.GetString(buffer, 0, nameRecord.Length),
                PlatformIdentifier.Macintosh => null,
                PlatformIdentifier.Reserved => null,
                _ => throw new NotImplementedException(),
            };

            nameRecord.Value = value;

            ArrayPool<byte>.Shared.Return(buffer);
        }

        NameRecrods = nameRecords;
    }

    public void Encode(IFontWriter writer, int offset)
    {
        throw new NotImplementedException();
    }
}

public class NameRecrod
{
    public PlatformIdentifier PlatformID { get; set; }
    public ushort PlatformSpecificID { get; set; }
    public ushort LanguageID { get; set; }
    public ushort NameID { get; set; }
    public ushort Length { get; set; }
    public ushort Offset { get; set; }
    public string? Value { get; set; }

    public void Decode(IFontReader reader)
    {
        PlatformID = (PlatformIdentifier)reader.ReadInt16();
        PlatformSpecificID = reader.ReadUInt16();
        LanguageID = reader.ReadUInt16();
        NameID = reader.ReadUInt16();
        Length = reader.ReadUInt16();
        Offset = reader.ReadUInt16();
    }

    public void Encode(IFontWriter writer)
    {
        writer.WriteUInt16((ushort)PlatformID);
        writer.WriteUInt16(PlatformSpecificID);
        writer.WriteUInt16(LanguageID);
        writer.WriteUInt16(NameID);
        writer.WriteUInt16(Length);
        writer.WriteUInt16(Offset);
    }
}