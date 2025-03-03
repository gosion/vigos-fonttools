namespace Vigos.FontTest;

public static class TtfTestCases
{
    public static IEnumerable<object[]> CaseForTtf =>
    [
        [new TestCase<string, string> { Input = @"Arial.ttf", Expected = "Arial" }],
        [new TestCase<string, string> { Input = @"STIXGeneral.otf", Expected = "STIXGeneral" }],
    ];

    public static IEnumerable<object[]> CaseForTtc =>
    [
        [new TestCase<string, string[]> { Input = @"wqy-microhei-0.2.0-beta.ttc", Expected = ["WenQuanYi Micro Hei", "WenQuanYi Micro Hei Mono"]}],
    ];

    public static IEnumerable<object[]> CaseForToTtf =>
    [
        [
            new TestCase<string, FontNameDescriptor[]> {
                Input = @"wqy-microhei-0.2.0-beta.ttc",
                Expected = [
                    new FontNameDescriptor{ Filename="WenQuanYiMicroHei.ttf", FontName = "WenQuanYi Micro Hei"},
                    new FontNameDescriptor{ Filename="WenQuanYiMicroHeiMono.ttf", FontName = "WenQuanYi Micro Hei Mono"},
                ]
            }
        ],
    ];
}

public class TestCase<I, E>
{
    public required I Input { get; set; }
    public required E Expected { get; set; }
}

public class FontNameDescriptor
{
    public required string Filename { get; set; }
    public required string FontName { get; set; }
}
