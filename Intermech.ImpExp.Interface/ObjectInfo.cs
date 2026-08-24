// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ObjectInfo
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ObjectInfo : ITagImportObject
{
  public int ObjectType;

  public ObjectInfo() => this.ObjectType = -1;

  public ObjectInfo(int objectType) => this.ObjectType = objectType;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter bw = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        this.SaveData(bw);
      }
      finally
      {
        bw.Flush();
      }
      return output.ToArray();
    }
  }

  protected virtual void SaveData(BinaryWriter bw) => bw.Write(this.ObjectType);

  protected virtual void LoadData(BinaryReader br) => this.ObjectType = br.ReadInt32();

  public void Load(byte[] s)
  {
    using (MemoryStream input = new MemoryStream(s))
    {
      BinaryReader br = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        this.LoadData(br);
      }
      finally
      {
        br.Close();
      }
    }
  }

  public virtual short ClassID => 15;
}
