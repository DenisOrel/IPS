// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.SchemaInfo
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class SchemaInfo
{
  private string layoutAlgoritmName = string.Empty;
  private bool isMultiContainsMode;
  private bool isLoadStatuses;
  private Statistic statistic = new Statistic();
  private long ipsObjectVersionId;
  private long id;
  private string caption = string.Empty;
  private int objectTypeId = -1;
  private DateTime dateOflastCheck;

  public SchemaInfo(
    long objVerId,
    long ID,
    string capt,
    int objtypeId,
    string layoutAlgName,
    WinSettings setting)
    : this()
  {
    this.ipsObjectVersionId = objVerId;
    this.caption = capt;
    this.objectTypeId = objtypeId;
    this.layoutAlgoritmName = layoutAlgName;
    this.id = ID;
    this.isLoadStatuses = setting.ShowStatuses;
  }

  public SchemaInfo(SchemaInfo parentShema, string layoutAlgName)
    : this()
  {
    this.ipsObjectVersionId = parentShema.ObjectVersionId;
    this.caption = parentShema.caption;
    this.objectTypeId = parentShema.objectTypeId;
    this.layoutAlgoritmName = layoutAlgName;
    this.id = parentShema.ID;
  }

  public SchemaInfo() => this.dateOflastCheck = DateTime.Now;

  public Statistic Statistic
  {
    get => this.statistic;
    set => this.statistic = value;
  }

  public bool IsLoadStatuses
  {
    get => this.isLoadStatuses;
    set => this.isLoadStatuses = value;
  }

  public bool IsMultiContainsMode
  {
    get => this.isMultiContainsMode;
    set => this.isMultiContainsMode = value;
  }

  public string LayoutAlgoritmName => this.layoutAlgoritmName;

  public long ID => this.id;

  public DateTime DateOflastCheck
  {
    get => this.dateOflastCheck;
    set => this.dateOflastCheck = value;
  }

  public int ObjectTypeId
  {
    get => this.objectTypeId;
    set => this.objectTypeId = value;
  }

  public string Caption
  {
    get => this.caption;
    set => this.caption = value;
  }

  public long ObjectVersionId
  {
    get => this.ipsObjectVersionId;
    set => this.ipsObjectVersionId = value;
  }

  public override bool Equals(object obj)
  {
    return obj is SchemaInfo schemaInfo && this.ipsObjectVersionId == schemaInfo.ipsObjectVersionId && this.caption == schemaInfo.caption && this.objectTypeId == schemaInfo.objectTypeId && this.layoutAlgoritmName == schemaInfo.layoutAlgoritmName;
  }

  public override int GetHashCode() => this.ToString().GetHashCode();

  public override string ToString()
  {
    return !this.caption.Equals(string.Empty) ? this.caption + this.layoutAlgoritmName : $"{this.ipsObjectVersionId}" + this.layoutAlgoritmName;
  }
}
