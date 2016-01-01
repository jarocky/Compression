using System;
using System.Collections.Generic;
using System.IO;

namespace Compression.Rle
{
  public class RleStreamReader : IRleStreamReader, IDisposable
  {
    private readonly int _sequenceMin;
    private readonly Stream _stream;
    private int _remainSequenceCount = 0;
    private int _currentSequenceCount = 0;
    private int _lastByte = -1;

    public RleStreamReader(int sequenceMin, Stream stream)
    {
      if (sequenceMin < 2)
      {
        throw new ArgumentException("Cannot be less than 2", "sequenceMin");
      }

      if (stream == null)
      {
        throw new ArgumentNullException("stream", "Cannot be null");
      }

      _sequenceMin = sequenceMin;
      _stream = stream;
    }

    public int ReadByte()
    {
      if (_remainSequenceCount > 0)
      {
        _remainSequenceCount--;
        return _lastByte;
      }

      var b = _stream.ReadByte();

      if (_currentSequenceCount == _sequenceMin)
      {
        if (b > 0)
        {
          _currentSequenceCount = 0;
          _remainSequenceCount = b - 1;
          return _lastByte;
        }

        if (b == 0)
        {
          ResetSequence();
          b = _stream.ReadByte();
        }
      }

      if (b < 0)
      {
        ResetSequence();
        return -1;
      }

      if (b != _lastByte)
      {
        ResetSequence();
        _lastByte = b;
      }

      _currentSequenceCount++;

      return b;
    }

    public IEnumerable<int> ReadAllBytes()
    {
      int b;
      while ((b = ReadByte()) != -1)
      {
        yield return b;
      }
    }

    public void Dispose()
    {
      _stream.Dispose();
    }

    private void ResetSequence()
    {
      _lastByte = -1;
      _currentSequenceCount = 0;
    }
  }
}