using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Compression.FileSystem
{
  [DataContract]
  public class CompressionData
  {
    private List<Tuple<string, long, long>> _files = new List<Tuple<string, long, long>>();

    [DataMember]
    public List<Tuple<string, long, long>> Files
    {
      get { return _files; }
      set { _files = value; }
    }

    [DataMember]
    public string FileName { get; set; }
  }
}