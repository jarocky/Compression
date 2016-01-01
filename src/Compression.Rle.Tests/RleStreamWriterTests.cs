using System;
using System.IO;
using NUnit.Framework;

namespace Compression.Rle.Tests
{
  public class RleStreamWriterTests
  {
    [Test]
    public void Write_SequenceMinLessThanTwo_ThrowArgumentException()
    {
      Assert.Throws<ArgumentException>(() => new RleStreamWriter(1, new MemoryStream()));
    }

    [Test]
    public void Write_StreamIsNull_ThrowArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(() => new RleStreamWriter(2, null));
    }

    [Test]
    public void Write_OneByte()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      var b = (byte) 127;

      rleStreamWriter.WriteByte(b);

      memoryStream.Position = 0;
      Assert.AreEqual(b, memoryStream.ReadByte());
    }

    [Test]
    public void Write_OneByte_LengthEqualOne()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);

      rleStreamWriter.WriteByte(127);

      Assert.AreEqual(1, memoryStream.Length);
    }

    [Test]
    public void Write_TwoTwoDifferentBytes()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      var b1 = (byte)127;
      var b2 = (byte)100;

      rleStreamWriter.WriteByte(b1);
      rleStreamWriter.WriteByte(b2);

      memoryStream.Position = 0;
      Assert.AreEqual(b1, memoryStream.ReadByte());
      Assert.AreEqual(b2, memoryStream.ReadByte());
    }

    [Test]
    public void Write_TwoDifferentBytes_LengthEqualTwo()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);

      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(100);

      Assert.AreEqual(2, memoryStream.Length);
    }

    [Test]
    public void Write_TwoTheSameBytes()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      var b = (byte)127;

      rleStreamWriter.WriteByte(b);
      rleStreamWriter.WriteByte(b);

      memoryStream.Position = 0;
      Assert.AreEqual(b, memoryStream.ReadByte());
      Assert.AreEqual(b, memoryStream.ReadByte());
    }

    [Test]
    public void Write_TwoTheSameBytes_LengthEqualTwo()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);

      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(127);

      Assert.AreEqual(2, memoryStream.Length);
    }

    [Test]
    public void Write_ThreeTheSameBytes_ThirdByteEqualOne()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      var b = (byte)127;

      rleStreamWriter.WriteByte(b);
      rleStreamWriter.WriteByte(b);
      rleStreamWriter.WriteByte(b);

      memoryStream.Position = 2;
      Assert.AreEqual(-1, memoryStream.ReadByte());
    }

    [Test]
    public void Write_ThreeTheSameBytes_LengthEqualTwo()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);

      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(127);

      Assert.AreEqual(2, memoryStream.Length);
    }

    [Test]
    public void Write_TwoTheSameBytesAndOneOther_ThirdByteEqualOne()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      
      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(100);

      memoryStream.Position = 2;
      Assert.AreEqual(0, memoryStream.ReadByte());
    }

    [Test]
    public void Write_TwoTheSameBytesAndOneOther_FourthByteEqualOne()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      var thirdByte = (byte) 100;

      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(thirdByte);

      memoryStream.Position = 3;
      Assert.AreEqual(thirdByte, memoryStream.ReadByte());
    }

    [Test]
    public void Write_255TheSameBytesSequence_LengthEqualThree()
    {
      const int sequenceMin = 2;
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(sequenceMin, memoryStream);
      
      for (int i = 0; i < 255 + sequenceMin; i++)
      {
        rleStreamWriter.WriteByte(127);
      }
      
      Assert.AreEqual(3, memoryStream.Length);
    }

    [Test] public void Write_255TheSameBytesSequence_ThirdByteEqual255()
    {
      const int sequenceMin = 2;
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(sequenceMin, memoryStream);

      for (int i = 0; i < 255 + sequenceMin; i++)
      {
        rleStreamWriter.WriteByte(127);
      }

      memoryStream.Position = 2;
      Assert.AreEqual(255, memoryStream.ReadByte());
    }

    [Test]
    public void Flush_ThreeTheSameBytes_ThirdByteEqualOne()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      var b = (byte)127;
      rleStreamWriter.WriteByte(b);
      rleStreamWriter.WriteByte(b);
      rleStreamWriter.WriteByte(b);

      rleStreamWriter.Flush();

      memoryStream.Position = 2;
      Assert.AreEqual(1, memoryStream.ReadByte());
    }

    [Test]
    public void Flush_ThreeTheSameBytes_LengthEqualThree()
    {
      var memoryStream = new MemoryStream();
      var rleStreamWriter = new RleStreamWriter(2, memoryStream);
      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(127);
      rleStreamWriter.WriteByte(127);

      rleStreamWriter.Flush();

      Assert.AreEqual(3, memoryStream.Length);
    }
  }
}