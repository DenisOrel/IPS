// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SettingsSyncAttributes.ListItems
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

#nullable disable
namespace Intermech.Pdm.SettingsSyncAttributes;

public class ListItems
{
  private int _attrId;
  private string _attrName;
  private int _myImageIndex;

  public int AttrID
  {
    get => this._attrId;
    set => this._attrId = value;
  }

  public string AttrName
  {
    get => this._attrName;
    set => this._attrName = value;
  }

  public int ImageIndex
  {
    get => this._myImageIndex;
    set => this._myImageIndex = value;
  }

  public ListItems(int id, string name)
    : this(id, name, -1)
  {
  }

  public ListItems(int id, string name, int index)
  {
    this._attrId = id;
    this._attrName = name;
    this._myImageIndex = index;
  }

  public override string ToString() => this._attrName;
}
