using System.IO;
using System.Runtime.Serialization.Json;
using Compression.Rle;

namespace Compression.FileSystem
{
  public class FilesDecompressor
  {
    private readonly int _sequenceMin;
    private readonly string _decompresionDirectory;
    private CompressionData _compressionData;

    public FilesDecompressor(int sequenceMin, string decompresionDirectory)
    {
      _sequenceMin = sequenceMin;
      _decompresionDirectory = decompresionDirectory;
    }

    public void DecompressFiles(string decompressionDataFilePath)
    {
      LoadCompressionData(decompressionDataFilePath);
      long location = 0;
      for (var i = 0; i < _compressionData.Files.Count; i++)
      {
        long offset = 0;
        for (var j = 0; j < i; j++)
        {
          offset += _compressionData.Files[j].Item2;
        }
        var length = _compressionData.Files[i].Item3;
        var fileStreamReader = new FileStream(_compressionData.FileName, FileMode.Open, FileAccess.Read);
        using (var fileStreamWriter = new FileStream(FullPath(_compressionData.Files[i].Item1), FileMode.Create, FileAccess.Write))
        using (var rleFileStreamReader = new RleStreamReader(_sequenceMin, fileStreamReader))
        {
          fileStreamReader.Position = offset;
          long offsetPosition = 0;
          int b;
          while ((b = rleFileStreamReader.ReadByte()) >= 0)
          {
            fileStreamWriter.WriteByte((byte) b);
            offsetPosition++;
            if (offsetPosition > length-1)
            {
              break;
            }
          }
        }
      }
    }

    private void LoadCompressionData(string decompressionDataFilePath)
    {
      var serializer = new DataContractJsonSerializer(typeof(CompressionData));
      using (var fileStreamWriter = new FileStream($"{decompressionDataFilePath}\\ComresionData.cps", FileMode.Open, FileAccess.Read))
      {
        _compressionData = (CompressionData)serializer.ReadObject(fileStreamWriter);
      }
    }

    private string FullPath(string fileFullPath)
    {
      return $"{_decompresionDirectory}\\{Path.GetFileName(fileFullPath)}";
    }
  }
}
