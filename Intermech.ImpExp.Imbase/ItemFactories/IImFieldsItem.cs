// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.IImFieldsItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal interface IImFieldsItem : ISettingsAttributeTypeItem, ISettingsItem
{
  int TableId { get; }

  string Field { get; }

  string Units { get; }

  int Sort { get; }

  int Flags { get; }

  long Width { get; set; }

  ImDataMode DataMode { get; }

  int Required { get; }

  ImDataTypeEx DataType { get; set; }

  ImEnterMode EnterMode { get; set; }

  string Data { get; }

  int Key { get; }

  FieldTypes AttrFieldType { get; set; }

  AttributeOptions Options { get; }

  AttributeCheckResult PumpPosible { get; set; }

  string UniqueKey { get; }
}
