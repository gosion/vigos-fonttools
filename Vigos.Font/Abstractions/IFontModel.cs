namespace Vigos.Font.Abstractioins;

public interface IFontModel
{
    void Encode(IFontWriter writer);
    void Decode(IFontReader reader);
}