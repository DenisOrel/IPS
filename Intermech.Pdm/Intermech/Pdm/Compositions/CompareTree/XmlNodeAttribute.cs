// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.XmlNodeAttribute
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class XmlNodeAttribute
{
  public string Name { get; private set; }

  public XmlMetadataTypes MetadataType { get; private set; }

  public XmlNodeAttribute(string name, XmlMetadataTypes metadataType)
  {
    this.Name = name;
    this.MetadataType = metadataType;
  }
}
