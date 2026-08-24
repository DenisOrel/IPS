// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImportingAttributableTag`1
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using Intermech.IO;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.Interface;

public abstract class ImportingAttributableTag<TAttributable> : ITagImportObject where TAttributable : ImportingAttributable
{
  public TAttributable Attributable { get; private set; }

  public abstract short ClassID { get; }

  public ImportingAttributableTag()
  {
  }

  public ImportingAttributableTag(TAttributable attributable) => this.Attributable = attributable;

  public byte[] Save()
  {
    using (ImChunkedStream serializationStream = new ImChunkedStream())
    {
      new BinaryFormatter().Serialize((Stream) serializationStream, (object) this.Attributable);
      return serializationStream.ToArray();
    }
  }

  public void Load(byte[] s)
  {
    using (MemoryStream serializationStream = new MemoryStream(s))
    {
      serializationStream.Position = 0L;
      this.Attributable = (TAttributable) new BinaryFormatter().Deserialize((Stream) serializationStream);
    }
  }

  public abstract TAttributable Clone();
}
