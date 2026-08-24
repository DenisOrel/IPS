// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ITreeListNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

#nullable disable
namespace Intermech.MaterialsHandbook;

internal interface ITreeListNode
{
  FieldTypes NodeType { get; set; }

  object Value { get; set; }
}
