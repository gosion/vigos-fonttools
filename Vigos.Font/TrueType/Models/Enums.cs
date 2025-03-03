namespace Vigos.Font.TrueType.Models;

public enum PlatformIdentifier : ushort
{
    Unicode = 0,
    Macintosh = 1,
    Reserved = 2,
    Microsoft = 3
}

public enum UnicodeIdentifier : ushort
{
    V1_0,
    V1_1,
    ISO,
    V2_0_BMP,
    V2_0_NON_BMP,
    VariationSequences,
    LastResort
}

public enum WindowsIdentifier : ushort
{
    Symbol,
    UCS_2,
    SHIFT_JIS,
    PRC,
    BigFive,
    Johab,
    UCS_4
}

public enum MacintoshIdentifier : ushort
{
    Roman,
    Japanese,
    TraditionalChinese,
    Korean,
    Arabic,
    Hebrew,
    Greek,
    Russian,
    RSymbol,
    Devanagari,
    Gurmukhi,
    Gujarati,
    Oriya,
    Bengali,
    Tamil,
    Telugu,
    Kannada,
    Malayalam,
    Sinhalese,
    Burmese,
    Khmer,
    Thai,
    Laotian,
    Georgian,
    Armenian,
    SimplifiedChinese,
    Tibetan,
    Mongolian,
    Geez,
    Slavic,
    Vietnamese,
    Sindhi,
    Uninterpreted
}

public enum NameIdentifier : ushort
{
    CopyrightNotice = 0,
    FontFamily = 1,
    FontSubfamily = 2,
    UniqueSubfamilyId = 3,
    FullName = 4,
    NameTableVersion = 5,
    PostScriptName = 6,
    TrademarkNotice = 7,
    ManufacturerName = 8,
    DesignerName = 9,
    Description = 10,
    VendorUrl = 11,
    DesignerUrl = 12,
    LicenseDescription = 13,
    LicenseUrl = 14,
    PreferredFamily = 16,
    PreferredSubfamily = 17,
    CompatibleFull = 18,
    SampleText = 19,
}