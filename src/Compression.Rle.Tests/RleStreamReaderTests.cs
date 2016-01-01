using System;
using System.IO;
using NUnit.Framework;

namespace Compression.Rle.Tests
{
  public class RleStreamReaderTests
  {
    [Test]
    public void Constructor_SequenceMinLessThanTwo_ThrowArgumentException()
    {
      Assert.Throws<ArgumentException>(() => new RleStreamReader(1, new MemoryStream()));
    }

    [Test]
    public void Constructor_StreamIsNull_ThrowArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(() => new RleStreamReader(2, null));
    }

    [Test]
    public void Read_OneByte()
    {
      var bytes = new byte[] { 127 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      var b = rleStreamRead.ReadByte();

      Assert.AreEqual(bytes[0], b);
    }

    [Test]
    public void Read_OneByte_EndStreamAfter()
    {
      var bytes = new byte[] { 127 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      rleStreamRead.ReadByte();
      var b = rleStreamRead.ReadByte();

      Assert.AreEqual(-1, b);
    }

    [Test]
    public void Read_TwoTheSameBytes()
    {
      var bytes = new byte[] { 127, 127 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      var b1 = rleStreamRead.ReadByte();
      var b2 = rleStreamRead.ReadByte();

      Assert.AreEqual(bytes[0], b1);
      Assert.AreEqual(bytes[0], b2);
    }

    [Test]
    public void Read_TwoTheSameBytes_EndStreamAfter()
    {
      var bytes = new byte[] { 127, 127 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      rleStreamRead.ReadByte();
      rleStreamRead.ReadByte();
      var b = rleStreamRead.ReadByte();

      Assert.AreEqual(-1, b);
    }

    [Test]
    public void Read_TwoTheDifferentBytes()
    {
      var bytes = new byte[] { 127, 100 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      var b1 = rleStreamRead.ReadByte();
      var b2 = rleStreamRead.ReadByte();

      Assert.AreEqual(bytes[0], b1);
      Assert.AreEqual(bytes[1], b2);
    }

    [Test]
    public void Read_TwoTheDifferentBytes_EndStreamAfter()
    {
      var bytes = new byte[] { 127, 100 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      rleStreamRead.ReadByte();
      rleStreamRead.ReadByte();
      var b = rleStreamRead.ReadByte();

      Assert.AreEqual(-1, b);
    }

    [Test]
    public void Read_TwoTheSameBytesAndSequance0()
    {
      var bytes = new byte[] { 127, 127, 0 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      var b1 = rleStreamRead.ReadByte();
      var b2 = rleStreamRead.ReadByte();

      Assert.AreEqual(bytes[0], b1);
      Assert.AreEqual(bytes[0], b2);
    }

    [Test]
    public void Read_TwoTheSameBytesAndSequance0_EndStreamAfter()
    {
      var bytes = new byte[] { 127, 127, 0 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      rleStreamRead.ReadByte();
      rleStreamRead.ReadByte();
      var b = rleStreamRead.ReadByte();

      Assert.AreEqual(-1, b);
    }

    [Test]
    public void Read_TwoTheSameBytesAndSequance1()
    {
      var bytes = new byte[] { 127, 127, 1 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      var b1 = rleStreamRead.ReadByte();
      var b2 = rleStreamRead.ReadByte();
      var b3 = rleStreamRead.ReadByte();

      Assert.AreEqual(bytes[0], b1);
      Assert.AreEqual(bytes[0], b2);
      Assert.AreEqual(bytes[0], b3);
    }

    [Test]
    public void Read_TwoTheSameBytesAndSequance1_EndStreamAfter()
    {
      var bytes = new byte[] { 127, 127, 1 };
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(bytes));

      rleStreamRead.ReadByte();
      rleStreamRead.ReadByte();
      rleStreamRead.ReadByte();
      var b = rleStreamRead.ReadByte();

      Assert.AreEqual(-1, b);
    }

    [Test]
    public void ReadAllBytes_TwoTheSameBytesAndSequance0()
    {
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(new byte[] { 127, 127, 0 }));

      var bytes = rleStreamRead.ReadAllBytes();

      CollectionAssert.AreEqual(new byte[] { 127, 127 }, bytes);
    }

    [Test]
    public void ReadAllBytes_TwoTheSameBytesAndSequance1()
    {
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(new byte[] { 127, 127, 1 }));

      var bytes = rleStreamRead.ReadAllBytes();

      CollectionAssert.AreEqual(new byte[] { 127, 127, 127 }, bytes);
    }

    [Test]
    public void ReadAllBytes_TwoTheSameBytesAndSequance2()
    {
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(new byte[] { 127, 127, 2 }));

      var bytes = rleStreamRead.ReadAllBytes();

      CollectionAssert.AreEqual(new byte[] { 127, 127, 127, 127 }, bytes);
    }

    [Test]
    public void ReadAllBytes_TwoTheSameBytesAndSequance2AndOneOther()
    {
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(new byte[] { 127, 127, 2, 12 }));

      var bytes = rleStreamRead.ReadAllBytes();

      CollectionAssert.AreEqual(new byte[] { 127, 127, 127, 127, 12 }, bytes);
    }

    [Test]
    public void ReadAllBytes_SequenceMin2_Decoding()
    {
      var rleStreamRead = new RleStreamReader(2, new MemoryStream(new byte[] { 123, 34, 127, 127, 2, 12, 13, 13, 5, 33 }));

      var bytes = rleStreamRead.ReadAllBytes();

      CollectionAssert.AreEqual(new byte[] { 123, 34, 127, 127, 127, 127, 12, 13, 13, 13, 13, 13, 13, 13, 33 }, bytes);
    }

    [Test]
    public void ReadAllBytes_SequenceMin3_Decoding()
    {
      var rleStreamRead = new RleStreamReader(3, new MemoryStream(new byte[] { 123, 34, 127, 127, 2, 12, 13, 13, 13, 5, 33 }));

      var bytes = rleStreamRead.ReadAllBytes();

      CollectionAssert.AreEqual(new byte[] { 123, 34, 127, 127, 2, 12, 13, 13, 13, 13, 13, 13, 13, 13, 33 }, bytes);
    }
  }
}