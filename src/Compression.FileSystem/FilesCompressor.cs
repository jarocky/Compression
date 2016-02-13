using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Compression.Rle;

namespace Compression.FileSystem
{
  public class FilesCompressor
  {
    private readonly int _sequenceMin;
    private readonly string _filePath;
    private readonly string _fileName;
    private readonly CompressionData _compressionData;

    public FilesCompressor(int sequenceMin, string filePath, string fileName)
    {
      _sequenceMin = sequenceMin;
      _filePath = filePath;
      _fileName = fileName;
      _compressionData = new CompressionData();
    }

    public void CompressFile(string fileFullPath)
    {
      var fileMode = File.Exists(FullPath(_filePath, _fileName)) ? FileMode.Append : FileMode.CreateNew;
      long position = 0;
      long lenght = 0;
      using (var fileStreamReader = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
      using (var rleFileStreamWriter = new RleStreamWriter(_sequenceMin, 
        new FileStream(FullPath(_filePath, _fileName), fileMode, FileAccess.Write)))
      {
        int b;
        while ((b = fileStreamReader.ReadByte()) >= 0)
        {
          position += rleFileStreamWriter.WriteByte((byte)b);
          lenght++;
        }
      }

      AddCompresionData(fileFullPath, position, lenght);
    }

    public void SaveCompressionData()
    {
      _compressionData.FileName = FullPath(_filePath, _fileName);
      var serializer = new DataContractJsonSerializer(typeof (CompressionData));
      using (var fileStreamWriter = new FileStream($"{_filePath}\\ComresionData.cps", FileMode.Create, FileAccess.Write))
      {
        serializer.WriteObject(fileStreamWriter, _compressionData);
      }
    }

    private void AddCompresionData(string filePath, long position, long length)
    {
      _compressionData.Files.Add(new Tuple<string, long, long>(filePath, position, length));
    }

    private string FullPath(string filePath, string fileName)
    {
      return $"{filePath}\\{fileName}";
    }
  }
}
