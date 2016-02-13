using System;
using System.IO;

namespace Compression.Rle
{
  public class RleStreamWriter : IRleStreamWriter, IDisposable
  {
    private readonly int _sequenceMin;
    private readonly Stream _stream;
    private int _currentAllSequenceCount = 0;
    private int _lastByte = -1;

    public RleStreamWriter(int sequenceMin, Stream stream)
    {
      if (sequenceMin < 2)
      {
        throw new ArgumentException("Cannot be less than 2", nameof(sequenceMin));
      }

      if (stream == null)
      {
        throw new ArgumentNullException(nameof(stream), "Cannot be null");
      }

      _sequenceMin = sequenceMin;
      _stream = stream;
    }

    public int WriteByte(byte b)
    {
      var bytesCount = 0;
      if (b == _lastByte)
      {
        _currentAllSequenceCount++;
        if (_currentAllSequenceCount <= _sequenceMin)
        {
          _stream.WriteByte(b);
          bytesCount++;
        }

        bytesCount += SequenceOverflow();
      }
      else
      {
        bytesCount += WriteSequenceCount();
        _currentAllSequenceCount = 1;
        _stream.WriteByte(b);
        bytesCount++;
      }
      _lastByte = b;
      return bytesCount;
    }

    public void WriteAllByte(byte[] bytes)
    {
      foreach (var b in bytes)
      {
        WriteByte(b);
      }
      Flush();
    }

    public int Flush()
    {
      return WriteSequenceCount();
    }

    public void Dispose()
    {
      Flush();
      _stream.Dispose();
    }

    private int WriteSequenceCount()
    {
      if (_currentAllSequenceCount >= _sequenceMin)
      {
        _stream.WriteByte((byte)(_currentAllSequenceCount - _sequenceMin));
        _currentAllSequenceCount = 0;
        _lastByte = -1;
        return 1;
      }
      return 0;
    }

    private int SequenceOverflow()
    {
      if (_currentAllSequenceCount - _sequenceMin == 255)
      {
        _stream.WriteByte(255);
        _currentAllSequenceCount = 0;
        _lastByte = -1;
        return 1;
      }
      return 0;
    }
  }
}