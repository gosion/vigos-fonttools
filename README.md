# Vigos Font Tools

A font tool implemented in dotnet

## Usage

* Convert .ttc to .ttf file

```csharp
var result = new TtcParser().ToTtf("wqy-microhei-0.2.0-beta.ttc", "output-dir");

foreach (var x in result)
{
    Console.WriteLine(x.FontFamily);
}

// WenQuanYi Micro Hei
// WenQuanYi Micro Hei Mono

.
├── WenQuanYi Micro Hei
│   └── WenQuanYiMicroHei.ttf
└── WenQuanYi Micro Hei Mono
    └── WenQuanYiMicroHeiMono.ttf
```

* List Font Families in .ttc/.ttf

```csharp
var ttfInfos = new TtcParser().Parse("wqy-microhei-0.2.0-beta.ttc");

for (var i = 0; i < len; i++)
{
    var fontFamilies = items[i].Value.GetFontFamilies();
    foreach (var item in fontFamilies)
    {
        Console.WriteLine($"{item.Key} - {item.Value}")
    }
    Console.WriteLine("");
}

// zh-TW - 文泉驛微米黑
// en-US - WenQuanYi Micro Hei
// zh-CN - 文泉驿微米黑
// zh-HK - 文泉驛微米黑
// zh-SG - 文泉驿微米黑
// zh-MO - 文泉驛微米黑
// zh-TW - 文泉驛等寬微米黑

// en-US - WenQuanYi Micro Hei Mono
// zh-CN - 文泉驿等宽微米黑
// zh-HK - 文泉驛等寬微米黑
// zh-SG - 文泉驿等宽微米黑
// zh-MO - 文泉驛等寬微米黑
```