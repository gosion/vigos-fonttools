using System.Buffers;
using System.Buffers.Binary;
using Vigos.Font.Abstractioins;
using Vigos.Font.DataTypes;

namespace Vigos.Font.Streaming;

public class FontReader : IFontReader, IDisposable
{
    private readonly byte[] _buffer;
    private readonly Dictionary<string, long> _marks = [];
    public Stream Stream { get; private set; }

    public FontReader(Stream stream)
    {
        Stream = stream;
        _buffer = ArrayPool<byte>.Shared.Rent(sizeof(ulong));
    }

    public int Read(byte[] buffer, int offset, int length) => Stream.Read(buffer, offset, length);

    public void MoveTo(long position) => Stream.Position = position;
    
    public void Seek(long offset, SeekOrigin origin) => Stream.Seek(offset, origin);

    public long Mark(string name)
    {
        var position = Stream.Position;
        _marks[name] = position;
        return position;
    }

    public void GoToMark(string name)
    {
        if (_marks.TryGetValue(name, out var position))
        {
            Stream.Position = position;
        }
    }

    public int Peek(byte[] buffer, int offset, int length, long? position = null)
    {
        var original = Stream.Position;
        if (position > 0) Stream.Seek(position.Value, SeekOrigin.Current);
        var readCount = Stream.Read(buffer, offset, length);
        Stream.Position = original;
        return readCount;
    }

    public short ReadInt16() => BinaryPrimitives.ReadInt16BigEndian(MakeSpan(sizeof(short)));

    public ushort ReadUInt16() => BinaryPrimitives.ReadUInt16BigEndian(MakeSpan(sizeof(ushort)));
    
    public UInt24 ReadUInt24()
    {
        unsafe {return BinaryPrimitivesExtension.ReadUint24BigEndian(MakeSpan(sizeof(UInt24)));}
    }

    public uint ReadUInt32() => BinaryPrimitives.ReadUInt32BigEndian(MakeSpan(sizeof(uint)));

    public int ReadInt32() => BinaryPrimitives.ReadInt32BigEndian(MakeSpan(sizeof(int)));

    public string ReadString(int count)
    {
        var span = ArrayPool<char>.Shared.Rent(count);
        for (var i = 0; i < count; i++)
        {
            span[i] = (char)(byte)Stream.ReadByte();
        }
        var val = new string(span, 0, count);
        ArrayPool<char>.Shared.Return(span);
        return val;
    }

    private Span<byte> MakeSpan(int count)
    {
        var span = _buffer.AsSpan(0, count);
        var readCount = Stream.Read(span);
        if (readCount != count)
        {
            throw new EndOfStreamException();
        }

        return span;
    }

    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        Stream.Close();
        GC.SuppressFinalize(this);
    }
}