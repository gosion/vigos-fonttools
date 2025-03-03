using System.Globalization;
using Vigos.Font.Abstractioins;
using Vigos.Font.Sfnt;
using Vigos.Font.TrueType.Models;

namespace Vigos.Font.TrueType;

public class TtfFontInfo : IFontInfo
{
    private Dictionary<string, string>? _fontFamilies;
    private readonly OffsetSubTable _offsetTable;
    private NameRecrod[]? _nameRecords;

    public TtfFontInfo(OffsetSubTable offsetTable)
    {
        _offsetTable = offsetTable;
    }

    public Dictionary<string, string> GetFontFamilies()
    {
        if (_fontFamilies == null)
        {
            _fontFamilies = new Dictionary<string, string>();
            var fontFamilies = GetNameRecrods().Where(nr => (NameIdentifier)nr.NameID == NameIdentifier.FontFamily && nr.Value != null);

            foreach (var fm in fontFamilies)
            {
                try
                {
                    var culture = new CultureInfo(fm.LanguageID);
                    _fontFamilies.Add(culture.Name, fm.Value!);
                }
                catch
                {
                    continue;
                }
            }
        }

        return new Dictionary<string, string>(_fontFamilies);
    }

    public TableDirectory[] GetTableDirectories()
    {
        return _offsetTable.TableDirectories.ToArray();
    }

    public CmapTable? GetCmapTable()
    {
        return _offsetTable.TableDirectories.FirstOrDefault(t => t.Tag == "cmap")?.FontTable as CmapTable;
    }

    public NameRecrod[] GetNameRecrods()
    {
        if (_nameRecords == null)
        {
            var nameTable = _offsetTable.TableDirectories.FirstOrDefault(t => t.Tag == "name")?.FontTable ?? throw new Exception("Name table not found in font");
            _nameRecords = (nameTable as NameTable)?.NameRecrods ?? Array.Empty<NameRecrod>();
        }

        return _nameRecords;
    }

    public OffsetSubTable GetOffsetTable()
    {
        return _offsetTable;
    }
}