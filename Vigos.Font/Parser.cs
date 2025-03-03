using Vigos.Font.Abstractioins;
using Vigos.Font.TrueType;

namespace Vigos.Font;

public class Parser
{
    private static readonly string[] _collectionExts = [".ttc", ".otc"];
    private static readonly string[] _singleExts = [".ttf", ".otf"];

    public Dictionary<string, IFontInfo> Parse(string filename)
    {
        var ext = Path.GetExtension(filename);
        if (_collectionExts.Contains(ext))
        {
            return new TtcParser().Parse(filename);
        }
        else if (_singleExts.Contains(ext))
        {
            var font = new TtfParser().Parse(filename);
            var names = font.GetFontFamilies();
            if (!names.TryGetValue("en-US", out var name))
            {
                name = names.Keys.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(name)) throw new Exception("Name table not found in font");

            return new Dictionary<string, IFontInfo> { [name] = font };
        }

        throw new NotSupportedException();
    }
}