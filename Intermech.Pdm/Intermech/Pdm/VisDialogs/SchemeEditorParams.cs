// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.SchemeEditorParams
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

internal class SchemeEditorParams
{
  public ContainsMode Mode;

  public SchemeEditorParams(ContainsMode mode) => this.Mode = mode;
}
