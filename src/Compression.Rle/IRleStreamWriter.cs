namespace Compression.Rle
{
  public interface IRleStreamWriter
  {
    void WriteByte(byte b);
    void WriteAllByte(byte[] bytes);
    void Flush();
  }
}