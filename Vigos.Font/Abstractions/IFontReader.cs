using Vigos.Font.DataTypes;

namespace Vigos.Font.Abstractioins;

public interface IFontReader : IDisposable
{
    Stream Stream { get; }
    void MoveTo(long position);
    void Seek(long offset, SeekOrigin origin);
    long Mark(string name);
    void GoToMark(string name);
    int Read(byte[] buffer, int offset, int lengh);
    int Peek(byte[] buffer, int offset, int lengh, long? position = null);
    short ReadInt16();
    ushort ReadUInt16();
    UInt24 ReadUInt24();
    uint ReadUInt32();
    int ReadInt32();
    string ReadString(int length);
}