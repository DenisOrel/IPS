// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TrvNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Localization;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class TrvNode
{
  internal string LvItemData { get; set; }

  internal string TrvNodeData { get; set; }

  internal List<object> Params { get; set; }

  internal string Caption { get; set; }

  internal int Index { get; set; }

  internal string Designation { get; set; }

  internal TrvNode(
    string lvItemData,
    string trvNodeData,
    List<object> parameters,
    string caption,
    int index)
  {
    this.LvItemData = lvItemData;
    this.TrvNodeData = trvNodeData;
    this.Params = parameters;
    this.Designation = caption;
    this.Caption = $"{LocalizationHolder.rm.GetString("IMH_Coating")}: {caption}";
    this.Index = index;
  }
}
