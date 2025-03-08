using System.Globalization;
using Vigos.Font.Abstractioins;
using Vigos.Font.Sfnt;
using Vigos.Font.TrueType.Models;

namespace Vigos.Font.TrueType;

public class TtfFontInfo : IFontInfo
{
    private Dictionary<string, string>? _fontFamilies;
    private Dictionary<string, string>? _fontSubFamilies;
    private readonly OffsetSubTable _offsetTable;
    private NameRecrod[]? _nameRecords;
    private bool? _isBold = null;
    private bool? _isItalic = null;
    private bool? _isRegular = null;

    public TtfFontInfo(OffsetSubTable offsetTable)
    {
        _offsetTable = offsetTable;
    }

    public bool IsBold
    {
        get
        {
            if (_isBold == null)
            {
                var subFontFamilies = GetFontSubFamilies();
                if (subFontFamilies.TryGetValue("en-US", out var name))
                {
                    _isBold = name == "Bold";
                }
                else
                {
                    _isBold = subFontFamilies.FirstOrDefault().Value == "Bold";
                }
            }

            return _isBold == true;
        }
    }

    public bool IsItalic
    {
        get
        {
            if (_isItalic == null)
            {
                var subFontFamilies = GetFontSubFamilies();
                if (subFontFamilies.TryGetValue("en-US", out var name))
                {
                    _isItalic = name == "Italic";
                }
                else
                {
                    _isItalic = subFontFamilies.FirstOrDefault().Value == "Italic";
                }
            }

            return _isItalic == true;
        }
    }

    public bool IsRegular
    {
        get
        {
            if (_isRegular == null)
            {
                var subFontFamilies = GetFontSubFamilies();
                if (subFontFamilies.TryGetValue("en-US", out var name))
                {
                    _isRegular = name == "Regular";
                }
                else
                {
                    _isRegular = subFontFamilies.FirstOrDefault().Value == "Regular";
                }
            }

            return _isRegular == true;
        }
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

    public Dictionary<string, string> GetFontSubFamilies()
    {
        if (_fontSubFamilies == null)
        {
            _fontSubFamilies = new Dictionary<string, string>();
            var fontFamilies = GetNameRecrods().Where(nr => (NameIdentifier)nr.NameID == NameIdentifier.FontSubFamily && nr.Value != null);

            foreach (var fm in fontFamilies)
            {
                try
                {
                    var culture = new CultureInfo(fm.LanguageID);
                    _fontSubFamilies.Add(culture.Name, fm.Value!);
                }
                catch
                {
                    continue;
                }
            }
        }

        return new Dictionary<string, string>(_fontSubFamilies);
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