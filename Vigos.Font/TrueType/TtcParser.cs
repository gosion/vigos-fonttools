using Vigos.Font.Abstractioins;
using Vigos.Font.Streaming;
using Vigos.Font.TrueType.Models;

namespace Vigos.Font.TrueType;

public class TtcParser
{
    public Dictionary<string, IFontInfo> Parse(string filename)
    {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Parse(stream);
    }

    public Dictionary<string, IFontInfo> Parse(Stream stream)
    {
        using var reader = FontStreamingFactory.GetFontReader(stream);
        return Parse(reader);
    }

    public Dictionary<string, IFontInfo> Parse(IFontReader reader)
    {
        var header = TtcHeader.Parse(reader);
        if (header.TableDirectoryOffsets == null || header.TableDirectoryOffsets.Length == 0) throw new Exception("Invalid font format");

        var result = new Dictionary<string, IFontInfo>();

        foreach (var offset in header.TableDirectoryOffsets)
        {
            reader.Stream.Position = offset;

            var font = new TtfParser().Parse(reader);
            var fontFamilies = font.GetFontFamilies();
            if (!fontFamilies.TryGetValue("en-US", out var name))
            {
                name = font.GetFontFamilies().Values.FirstOrDefault();
            }

            result.Add(name!, font);
        }

        reader.Dispose();

        return result;
    }

    public ParseResult[] ToTtf(string filename, string dir)
    {
        return ToTtf(Parse(filename), dir);
    }

    public ParseResult[] ToTtf(Stream stream, string dir)
    {
        return ToTtf(Parse(stream), dir);
    }

    private ParseResult[] ToTtf(Dictionary<string, IFontInfo> fonts, string dir)
    {
        var result = new List<ParseResult>();
        foreach (var font in fonts)
        {
            var package = Path.Combine(dir, font.Key);
            if (!Directory.Exists(package))
            {
                Directory.CreateDirectory(package);
            }

            var fontFileName = font.Key.Replace(" ", "");
            var fontInfo = font.Value;
            if (fontInfo.IsBold) fontFileName += "-Bold";
            if (fontInfo.IsItalic) fontFileName += "-Italic";
            
            fontFileName += ".ttf";
            var fs = new FileStream(Path.Combine(package, fontFileName), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using var writer = FontStreamingFactory.GetFontWriter(fs);
            (font.Value as TtfFontInfo)!.GetOffsetTable().Encode(writer);
            fs.Flush();
            fs.Close();
            result.Add(new() { FontFamily = font.Key, FontFilePath = fontFileName});
        }

        return result.ToArray();
    }
}

public class ParseResult
{
    public string FontFamily { get; set; } = "";
    public string FontFilePath { get; set; } = "";
}