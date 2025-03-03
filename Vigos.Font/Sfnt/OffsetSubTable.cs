using Vigos.Font.Abstractioins;

namespace Vigos.Font.Sfnt;

public class OffsetSubTable : IFontModel
{
    public SfntVersion SfntVersion { get; set; } = new();
    public ushort NumTables { get; set; }
    public ushort SearchRange { get; set; }
    public ushort EntrySelector { get; set; }
    public ushort RangeShift { get; set; }
    public IList<TableDirectory> TableDirectories { get; set; } = [];

    public void Decode(IFontReader reader)
    {
        SfntVersion = SfntVersion.Parse(reader);
        NumTables = reader.ReadUInt16();
        SearchRange = reader.ReadUInt16();
        EntrySelector = reader.ReadUInt16();
        RangeShift = reader.ReadUInt16();

        for (var i = 0; i < NumTables; i++)
        {
            var directory = new TableDirectory();
            directory.Decode(reader);
            TableDirectories.Add(directory);
        }

        foreach (var directory in TableDirectories)
        {
            directory.DecodeFontTables(reader);
        }
    }

    public void Encode(IFontWriter writer)
    {
        writer.MoveTo(0);
        SfntVersion.Encode(writer);
        writer.WriteUInt16(NumTables);
        writer.WriteUInt16(SearchRange);
        writer.WriteUInt16(EntrySelector);
        writer.WriteUInt16(RangeShift);

        for (var i = 0; i < TableDirectories.Count; i++)
        {
            TableDirectories[i].Encode(writer);
        }

        for (var i = 0; i < TableDirectories.Count; i++)
        {
            TableDirectories[i].EncodeFontTables(writer);
        }
    }
}
