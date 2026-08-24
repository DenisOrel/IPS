// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompositionFiltrationSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompositionFiltrationSettings
{
  public VersionsRule VersionsRule { get; }

  public HybridDictionary Tag { get; }

  public long EditingContextID { get; }

  public CompositionFiltrationSettings(
    VersionsRule versionsRule,
    HybridDictionary tag,
    long editingContextID)
  {
    this.VersionsRule = versionsRule;
    this.Tag = tag;
    this.EditingContextID = editingContextID;
  }
}
