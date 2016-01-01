using System.Collections.Generic;

namespace Compression.Rle
{
  public interface IRleStreamReader
  {
    int ReadByte();
    IEnumerable<int> ReadAllBytes();
  }
}