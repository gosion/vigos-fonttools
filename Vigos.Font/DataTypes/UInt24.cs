using System.Runtime.InteropServices;

namespace Vigos.Font.DataTypes;

[StructLayout(LayoutKind.Sequential)]
public struct UInt24
{
    private Byte _b0;
    private Byte _b1;
    private Byte _b2;

    public UInt24(byte[] bytes)
    {
        var len = bytes.Length;
        _b0 = len > 0 ? bytes[0] : (byte)0x00;
        _b1 = len > 1 ? bytes[1] : (byte)0x00;
        _b2 = len > 2 ? bytes[2] : (byte)0x00;
    }

    public static UInt24 From(UInt32 value)
    {
        var b0 = (byte)((value) & 0xFF);
        var b1 = (byte)((value >> 8) & 0xFF);
        var b2 = (byte)((value >> 16) & 0xFF);
        return new UInt24(new byte[] { b0, b1, b2 });
    }

    public UInt32 Value
    {
        get
        {
            return (uint)(_b0 | (_b1 << 8) | (_b2 << 16));
        }
    }
}