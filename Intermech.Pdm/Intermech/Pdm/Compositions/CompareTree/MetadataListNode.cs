// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.MetadataListNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class MetadataListNode
{
  public int ID { get; private set; }

  public string Name { get; private set; }

  public object Tag { get; set; }

  public MetadataListNode(int id, string name)
    : this(id, name, (object) null)
  {
  }

  public MetadataListNode(int id, string name, object tag)
  {
    this.ID = id;
    this.Name = name;
    this.Tag = tag;
  }

  public override string ToString() => this.Name;
}
