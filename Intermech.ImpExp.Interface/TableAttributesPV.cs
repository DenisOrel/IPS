// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TableAttributesPV
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.IO;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class TableAttributesPV : ITagImportObject
{
  public List<TableAttributePV> Values { get; set; }

  public TableAttributesPV() => this.Values = new List<TableAttributePV>();

  public byte[] Save()
  {
    using (ImChunkedStream serializationStream = new ImChunkedStream())
    {
      new BinaryFormatter().Serialize((Stream) serializationStream, (object) this.Values);
      return serializationStream.ToArray();
    }
  }

  public void Load(byte[] s)
  {
    using (MemoryStream serializationStream = new MemoryStream(s))
      this.Values = (List<TableAttributePV>) new BinaryFormatter().Deserialize((Stream) serializationStream);
  }

  public short ClassID => 27;
}
