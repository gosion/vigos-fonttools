using System.Buffers.Binary;

namespace Vigos.Font.DataTypes;

public static class BinaryPrimitivesExtension
{
    public static UInt24 ReadUint24BigEndian(ReadOnlySpan<byte> source)
    {
        var b0 = source[0];
        var b1 = source[1];
        var b2 = source[2];
        return new UInt24([b2, b1, b0]);
    }
}
