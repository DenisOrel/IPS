// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ITypeAttributeItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface ITypeAttributeItem
{
  int TypeID { get; set; }

  string DBFieldName { get; set; }

  string AttributeName { get; set; }

  FieldTypes AttributeType { get; set; }

  int AttributeSize { get; set; }

  object DefaultValue { get; set; }

  Guid GUID { get; set; }

  Guid CreateObjTypeGUID { get; set; }
}
