using System;
using System.IO;

namespace Compression.Rle
{
  public class RleStreamWriter : IRleStreamWriter, IDisposable
  {
    private readonly int _sequenceMin;
    private readonly Stream _stream;
    private int _currentAllSequenceCount = 0;
    private byte _lastByte = 0;

    public RleStreamWriter(int sequenceMin, Stream stream)
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

    public void WriteByte(byte b)
    {
      if (b == _lastByte)
      {
        _currentAllSequenceCount++;
        if (_currentAllSequenceCount <= _sequenceMin)
        {
          _stream.WriteByte(b);
        }

        SequenceOverflow();
      }
      else
      {
        WriteSequenceCount();
        _currentAllSequenceCount = 1;
        _stream.WriteByte(b);
      }
      _lastByte = b;
    }

    public void Flush()
    {
      WriteSequenceCount();
    }

    public void Dispose()
    {
      Flush();
      _stream.Dispose();
    }

    private void WriteSequenceCount()
    {
      if (_currentAllSequenceCount >= _sequenceMin)
      {
        _stream.WriteByte((byte)(_currentAllSequenceCount - _sequenceMin));
      }
    }

    private void SequenceOverflow()
    {
      if (_currentAllSequenceCount - _sequenceMin == 255)
      {
        _stream.WriteByte(255);
        _currentAllSequenceCount = 0;
        _lastByte = 0;
      }
    }
  }
}