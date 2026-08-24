// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.ProductionListItemAttribute
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal sealed class ProductionListItemAttribute
{
  public int ParamID { get; set; }

  public string DBFieldName { get; set; }

  public string AttributeName { get; set; }

  public FieldTypes AttributeType { get; set; }

  public int AttributeSize { get; set; }

  public Guid Guid { get; set; }
}
