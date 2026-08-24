// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CompositionTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class CompositionTag : ITagImportObject
{
  public int ProjAID;
  public int PartAID;

  public CompositionTag(int projAID, int partAID)
  {
    this.ProjAID = projAID;
    this.PartAID = partAID;
  }

  public CompositionTag()
  {
  }

  protected virtual void InternalSave(BinaryWriter bw)
  {
    bw.Write(this.ProjAID);
    bw.Write(this.PartAID);
  }

  public virtual byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter bw = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        this.InternalSave(bw);
      }
      finally
      {
        bw.Flush();
      }
      return output.ToArray();
    }
  }

  protected virtual void InternalLoad(BinaryReader br)
  {
    this.ProjAID = br.ReadInt32();
    this.PartAID = br.ReadInt32();
  }

  public virtual void Load(byte[] s)
  {
    using (MemoryStream input = new MemoryStream(s))
    {
      BinaryReader br = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        this.InternalLoad(br);
      }
      finally
      {
        br.Close();
      }
    }
  }

  public virtual short ClassID => 10;
}
