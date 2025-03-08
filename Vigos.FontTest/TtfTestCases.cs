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
                Input = @"MSYH.TTC",
                Expected = [
                    new FontNameDescriptor{ Filename="MicrosoftYaHei.ttf", FontName = "Microsoft YaHei"},
                ]
            }
        ],
        [
            new TestCase<string, FontNameDescriptor[]> {
                Input = @"msyhbd.ttc",
                Expected = [
                    new FontNameDescriptor{ Filename="MicrosoftYaHei-Bold.ttf", FontName = "Microsoft YaHei"},
                ]
            }
        ],
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

    public static IEnumerable<object[]> CaseForSubFamily =>
    [
        [
            new TestCase<string, FontStyle> {
                Input = "MicrosoftYaHei.ttf",
                Expected = new() {
                    Name = "Regular",
                    IsRegular = true,
                    IsBold = false,
                    IsItalic = false
                }
            }
        ],
        [
            new TestCase<string, FontStyle> {
                Input = "MicrosoftYaHei-Bold.ttf",
                Expected = new() {
                    Name = "Bold",
                    IsRegular = false,
                    IsBold = true,
                    IsItalic = false
                }
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

public class FontStyle
{
    public string Name { get; set; } = "";
    public bool IsRegular { get; set; }
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
}
