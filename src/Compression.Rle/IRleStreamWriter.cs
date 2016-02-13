namespace Compression.Rle
{
  public interface IRleStreamWriter
  {
    int WriteByte(byte b);
    void WriteAllByte(byte[] bytes);
    int Flush();
  }
}