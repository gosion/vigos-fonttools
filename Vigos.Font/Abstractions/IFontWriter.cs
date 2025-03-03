namespace Vigos.Font.Abstractioins;

public interface IFontWriter : IDisposable
{
    Stream Stream { get; }
    void MoveTo(long position);
    void Seek(long offset, SeekOrigin origin);
    void Write(byte[] buffer, int offset, int lengh);
    void WriteUInt16(ushort value);
    void WriteUInt32(uint value);
    void WriteString(string value);
}