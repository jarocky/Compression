using System.IO;

namespace Compression.FileSystem.Console.Tests
{
  class Program
  {
    static void Main(string[] args)
    {
      const int sequenceMin = 2;

      const string directoryToCompresion = @"C:\Users\Jarek\Desktop\DirectoryToCompression";
      const string compressionDirectory = @"C:\Users\Jarek\Desktop\CompressionDirectory";
      const string decompressionDirectory = @"C:\Users\Jarek\Desktop\DecompressionDirectory";
      const string compresionFile = "CompressedFile.zzz";

      var filesCompressor = new FilesCompressor(sequenceMin, compressionDirectory, compresionFile);

      var filePaths = Directory.GetFiles(directoryToCompresion);

      foreach (var filePath in filePaths)
      {
        filesCompressor.CompressFile(filePath);
      }

      filesCompressor.SaveCompressionData();

      var filesDecompressor = new FilesDecompressor(sequenceMin, decompressionDirectory);
      filesDecompressor.DecompressFiles(compressionDirectory);
    }
  }
}
