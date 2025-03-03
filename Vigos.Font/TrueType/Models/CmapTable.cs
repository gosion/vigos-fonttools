using Vigos.Font.Abstractioins;
using Vigos.Font.DataTypes;
using Vigos.Font.Sfnt;

namespace Vigos.Font.TrueType.Models;

public class CmapTable : IFontTable
{
    public ushort Version { get; set; }
    public ushort NumberSubtables { get; set; }
    public CmapSubTable[] CmapSubTables { get; set; } = Array.Empty<CmapSubTable>();

    public static CmapTable Parse(IFontReader reader, in TableDirectory tableDirectory)
    {
        var table = new CmapTable();
        table.Decode(reader);
        return table;
    }

    public void Decode(IFontReader reader)
    {
        _ = reader.Mark("cmap");
        Version = reader.ReadUInt16();
        NumberSubtables = reader.ReadUInt16();
        CmapSubTables = new CmapSubTable[NumberSubtables];

        for (var i = 0; i < NumberSubtables; i++)
        {
            CmapSubTables[i] = CmapSubTable.Parse(reader);
        }

        for (var i = 0; i < NumberSubtables; i++)
        {
            CmapSubTables[i].ParseFormat(reader);
        }
    }
}

public class CmapSubTable
{
    public ushort PlatformID { get; set; }
    public PlatformIdentifier PlatformSpecificID { get; set; }
    public uint Offset { get; set; }
    public CmapFormat? Format { get; set; }

    public static CmapSubTable Parse(IFontReader reader)
    {
        var table = new CmapSubTable();
        table.Decode(reader);
        return table;
    }

    public void Decode(IFontReader reader)
    {
        PlatformID = reader.ReadUInt16();
        PlatformSpecificID = (PlatformIdentifier)reader.ReadUInt16();
        Offset = reader.ReadUInt32();
    }

    public void ParseFormat(IFontReader reader)
    {
        reader.GoToMark("cmap");
        reader.Seek(Offset, SeekOrigin.Current);
        var format = reader.ReadUInt16();
        Console.WriteLine(format);
        Format = CmapFormatMap.Get(format)(reader);
    }
}

public delegate CmapFormat MakeCmapFormat(IFontReader reader);

public static class CmapFormatMap
{
    private static readonly Dictionary<ushort, MakeCmapFormat> _formats = new Dictionary<ushort, MakeCmapFormat>
    {
        [0] = CmapFormat0.Parse,
        [2] = CmapFormat2.Parse,
        [4] = CmapFormat4.Parse,
        [8] = CmapFormat8.Parse,
        [10] = CmapFormat10.Parse,
        [12] = CmapFormat12.Parse,
        [13] = CmapFormat13.Parse,
        [14] = CmapFormat14.Parse,
    };

    public static MakeCmapFormat Get(ushort format)
    {
        if (_formats.TryGetValue(format, out var formatMaker)) return formatMaker;
        Console.WriteLine($"format: {format}");
        throw new Exception($"Format maker ({format}) not found.");
    }
}

public abstract class CmapFormat
{
    public ushort Format { get; }

    public CmapFormat(ushort format)
    {
        Format = format;
    }

    public abstract void Decode(IFontReader reader);
}

public class CmapFormat0 : CmapFormat
{
    public CmapFormat0() : base(0) { }

    public ushort Length { get; set; }
    public ushort Language { get; set; }
    // public byte[] GlyphIndexArray { get; set; } = [];

    public static CmapFormat0 Parse(IFontReader reader)
    {
        var format = new CmapFormat0();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader)
    {
        Length = reader.ReadUInt16();
        Language = reader.ReadUInt16();
        // GlyphIndexArray = new byte[256];
    }
}

public class CmapFormat2 : CmapFormat
{
    public CmapFormat2() : base(2) { }

    public ushort Length { get; set; }
    public ushort Language { get; set; }
    // public ushort[] SubHeaderKeys { get; set; } = [];
    // public SubHeader[] SubHeaders { get; set; } = [];
    // public byte[] GlyphIndexArray { get; set; } = [];

    public static CmapFormat2 Parse(IFontReader reader)
    {
        var format = new CmapFormat2();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader)
    {
        Length = reader.ReadUInt16();
        Language = reader.ReadUInt16();
        // SubHeaderKeys = new ushort[256];
        // SubHeaders = new byte[];
        // GlyphIndexArray = new byte[];
    }
}

public struct SubHeader
{
    public ushort FirstCode { get; set; }
    public ushort EntryCount { get; set; }
    public short IdDelta { get; set; }
    public ushort IdRangeOffset { get; set; }
}

public class CmapFormat4 : CmapFormat
{
    public CmapFormat4() : base(4) { }

    public static CmapFormat4 Parse(IFontReader reader)
    {
        var format = new CmapFormat4();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader) { }

}

public class CmapFormat6 : CmapFormat
{
    public CmapFormat6() : base(6) { }

    public static CmapFormat6 Parse(IFontReader reader)
    {
        var format = new CmapFormat6();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader) { }
}

public class CmapFormat8 : CmapFormat
{
    public CmapFormat8() : base(8) { }

    public uint Length { get; set; }
    public uint Language { get; set; }
    // public byte[] Is32 { get; set; } = new byte[65536];
    // public uint NGroups { get; set; }
    // public GlyphGroup[] GlyphGroups { get; set; } = [];

    public static CmapFormat8 Parse(IFontReader reader)
    {
        var format = new CmapFormat8();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader)
    {
        Length = reader.ReadUInt32();
        _ = reader.ReadUInt16();
        Language = reader.ReadUInt32();
        // NGroups = reader.ReadUInt32();

        // GlyphGroups = new GlyphGroup[NGroups];
        // for (var i=0; i < NGroups; i++)
        // {
        //     var group = new GlyphGroup();
        //     group.Decode(reader);
        //     GlyphGroups[i] = group;
        // }
    }
}

public class CmapFormat10 : CmapFormat
{
    public CmapFormat10() : base(10) { }

    public uint Length { get; set; }
    public uint Language { get; set; }
    public uint StartCharCode { get; set; }
    public uint NumChars { get; set; }
    public ushort[] Glyphs { get; set; } = Array.Empty<ushort>();

    public static CmapFormat10 Parse(IFontReader reader)
    {
        var format = new CmapFormat10();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader)
    {
        Length = reader.ReadUInt32();
        _ = reader.ReadUInt16();
        Language = reader.ReadUInt32();
        StartCharCode = reader.ReadUInt32();
        NumChars = reader.ReadUInt32();
    }
}

public class CmapFormat12 : CmapFormat
{
    public CmapFormat12() : base(12) { }

    public uint Length { get; set; }
    public uint Language { get; set; }
    public uint NGroups { get; set; }
    public GlyphGroup[] GlyphGroups { get; set; } = Array.Empty<GlyphGroup>();

    public static CmapFormat12 Parse(IFontReader reader)
    {
        var format = new CmapFormat12();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader)
    {
        Length = reader.ReadUInt32();
        _ = reader.ReadUInt16();
        Language = reader.ReadUInt32();
        NGroups = reader.ReadUInt32();

        GlyphGroups = new GlyphGroup[NGroups];
        for (var i = 0; i < NGroups; i++)
        {
            var group = new GlyphGroup();
            group.Decode(reader);
            GlyphGroups[i] = group;
        }
    }
}

public class CmapFormat13 : CmapFormat
{
    public CmapFormat13() : base(13) { }

    public uint Length { get; set; }
    public uint Language { get; set; }
    public uint NGroups { get; set; }
    public GlyphGroup[] GlyphGroups { get; set; } = Array.Empty<GlyphGroup>();

    public static CmapFormat13 Parse(IFontReader reader)
    {
        var format = new CmapFormat13();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader)
    {
        Length = reader.ReadUInt32();
        _ = reader.ReadUInt16();
        Language = reader.ReadUInt32();
        NGroups = reader.ReadUInt32();

        var group = new GlyphGroup
        {
            StartCharCode = 0x4E00,
            EndCharCode = 0x9FCB,
            StartGlyphCode = 47
        };

        GlyphGroups = new GlyphGroup[NGroups];
        Array.Fill(GlyphGroups, group);
    }
}

public class GlyphGroup
{
    public uint StartCharCode { get; set; }
    public uint EndCharCode { get; set; }
    public uint StartGlyphCode { get; set; }

    public void Decode(IFontReader reader)
    {
        StartCharCode = reader.ReadUInt32();
        EndCharCode = reader.ReadUInt32();
        StartGlyphCode = reader.ReadUInt32();
    }
}

public class CmapFormat14 : CmapFormat
{
    public CmapFormat14() : base(14) { }

    public uint Length { get; set; }
    public uint Language { get; set; }
    public uint NumVarSelectorRecords { get; set; }
    public VariationSelector[] VariationSelectors { get; set; } = Array.Empty<VariationSelector>();

    public static CmapFormat14 Parse(IFontReader reader)
    {
        var format = new CmapFormat14();
        format.Decode(reader);
        return format;
    }

    public override void Decode(IFontReader reader)
    {
        Length = reader.ReadUInt32();
        _ = reader.ReadUInt16();
        Language = reader.ReadUInt32();
        NumVarSelectorRecords = reader.ReadUInt32();

        VariationSelectors = new VariationSelector[NumVarSelectorRecords];
        for (var i = 0; i < NumVarSelectorRecords; i++)
        {
            var variationSelector = new VariationSelector();
            variationSelector.Decode(reader);
            VariationSelectors[i] = variationSelector;
        }
    }
}

public class VariationSelector
{
    public UInt24 VarSelector { get; set; }
    public uint DefaultUVSOffset { get; set; }
    public uint NonDefaultUVSOffset { get; set; }

    public void Decode(IFontReader reader)
    {
        VarSelector = reader.ReadUInt24();
        DefaultUVSOffset = reader.ReadUInt32();
        NonDefaultUVSOffset = reader.ReadUInt32();
    }
}