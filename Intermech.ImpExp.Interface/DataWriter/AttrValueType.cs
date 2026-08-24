// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.AttrValueType
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>
/// Допустимые при импорте/экспорте типы значений атрибутов (для объектов и связей)
/// </summary>
public enum AttrValueType
{
  [Description("Unknown type value")] unknownVal,
  [Description("String type value")] stringVal,
  [Description("Integer type value")] integerVal,
  [Description("Double type value")] doubleVal,
  [Description("DateTime type value")] datetimeVal,
}
