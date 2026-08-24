// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.StyleNodeData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public abstract class StyleNodeData
{
  public StyleNodeData()
  {
  }

  public StyleNodeData(XmlNode node, IUserSession ius)
  {
  }

  public virtual void SaveToXml(XmlTextWriter writer)
  {
  }
}
