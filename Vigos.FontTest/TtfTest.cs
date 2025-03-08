using Vigos.Font.Streaming;
using Vigos.Font.TrueType;

namespace Vigos.FontTest;

public class TtfTest
{
    public TtfTest()
    {
        FontStreamingFactory.SetFontTableResolver(new TtfFontTableResolver());
    }

    [Theory]
    [MemberData(nameof(TtfTestCases.CaseForTtf), MemberType = typeof(TtfTestCases))]
    public void ShouldParseTtfSuccess(TestCase<string, string> testCase)
    {
        var path = Util.GetFontPath(testCase.Input);
        var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        var ttfInfo = new TtfParser().Parse(stream);
        var fontFamilies = ttfInfo.GetFontFamilies();
        Assert.Equal(testCase.Expected, fontFamilies["en-US"]);
    }

    [Theory]
    [MemberData(nameof(TtfTestCases.CaseForTtc), MemberType = typeof(TtfTestCases))]
    public void ShouldParseTtcSuccess(TestCase<string, string[]> testCase)
    {
        var path = Util.GetFontPath(testCase.Input);
        var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        var ttfInfos = new TtcParser().Parse(stream);
        var len = testCase.Expected.Length;
        Assert.Equal(len, ttfInfos.Count);
        var items = ttfInfos.ToArray();

        for (var i = 0; i < len; i++)
        {
            var fontFamilies = items[i].Value.GetFontFamilies();
            Assert.Equal(testCase.Expected[i], fontFamilies["en-US"]);
        }
    }

    [Theory]
    [MemberData(nameof(TtfTestCases.CaseForToTtf), MemberType = typeof(TtfTestCases))]
    public void ShouldConvertToTtfSuccess(TestCase<string, FontNameDescriptor[]> testCase)
    {
        var path = Util.GetFontPath(testCase.Input);
        var result = new TtcParser().ToTtf(path, Util.GetFontDir());
        var len = testCase.Expected.Length;

        for (var i = 0; i < len; i++)
        {
            var item = testCase.Expected[i];
            Assert.Equal(item.FontName, result[i].FontFamily);
            Assert.Equal(item.Filename, result[i].FontFilePath);
            var filename = Util.GetFontPath(Path.Combine(item.FontName, item.Filename));
            Assert.True(File.Exists(filename));
            var ttfInfo = new TtfParser().Parse(filename);
            Assert.Equal(item.FontName, ttfInfo.GetFontFamilies()["en-US"]);
        }
    }
    
    [Theory]
    [MemberData(nameof(TtfTestCases.CaseForSubFamily), MemberType = typeof(TtfTestCases))]
    public void ShouldReadSubFontFamiliySuccess(TestCase<string, FontStyle> testCase)
    {
        var path = Util.GetFontPath(testCase.Input);
        var fontInfo = new TtfParser().Parse(path);
        var subFamily = fontInfo!.GetFontSubFamilies()["en-US"];
        Assert.Equal(testCase.Expected.Name, subFamily);
        Assert.Equal(testCase.Expected.IsRegular, fontInfo.IsRegular);
        Assert.Equal(testCase.Expected.IsBold, fontInfo.IsBold);
        Assert.Equal(testCase.Expected.IsItalic, fontInfo.IsItalic);
    }
}