using System.Buffers;
using System.Buffers.Binary;
using Vigos.Font.Abstractioins;

namespace Vigos.Font.Streaming;

public class FontWriter : IFontWriter, IDisposable
{
    private readonly byte[] _buffer;
    public Stream Stream { get; private set; }

    public FontWriter(Stream stream)
    {
        Stream = stream;
        _buffer = ArrayPool<byte>.Shared.Rent(sizeof(ulong));
    }

    public void MoveTo(long position) => Stream.Position = position;

    public void Seek(long offset, SeekOrigin origin) => Stream.Seek(offset, origin);

    public void Write(byte[] buffer, int offset, int lengh) => Stream.Write(buffer, offset, lengh);

    public void WriteUInt16(ushort value)
    {
        var span = _buffer.AsSpan(0, sizeof(ushort));
        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        Stream.Write(span);
    }

    public void WriteUInt32(uint value)
    {
        var span = _buffer.AsSpan(0, sizeof(uint));
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        Stream.Write(span);
    }

    public void WriteString(string value)
    {
        var count = value.Length;
        for (var i = 0; i < count; i++)
        {
            Stream.WriteByte((byte)value[i]);
        }
    }

    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        Stream.Close();
        GC.SuppressFinalize(this);
    }
}