// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ObjectInfoEx
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;

#nullable disable
namespace Intermech.ImpExp.Interface;

public sealed class ObjectInfoEx : ObjectInfo
{
  public long ID;

  public ObjectInfoEx() => this.ID = 0L;

  public ObjectInfoEx(int objectType, long id)
    : base(objectType)
  {
    this.ID = id;
  }

  protected override void LoadData(BinaryReader br)
  {
    base.LoadData(br);
    this.ID = br.ReadInt64();
  }

  protected override void SaveData(BinaryWriter bw)
  {
    base.SaveData(bw);
    bw.Write(this.ID);
  }

  public override short ClassID => 34;
}
