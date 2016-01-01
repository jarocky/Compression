namespace Compression.Rle
{
  public interface IRleStreamWriter
  {
    void WriteByte(byte b);
    void Flush();
  }
}