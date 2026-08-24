// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ICompareRulesTab
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal interface ICompareRulesTab
{
  Guid ID { get; }

  string Caption { get; }

  string ToolTipText { get; }

  int Index { get; }

  Control Control { get; }

  event TabDataChangedEventHandler TabDataChangedEvent;

  void AnotherTabDataChanged(TabDataChangedEventArgs e);

  void RefreshData();

  int ImageIndex { get; }

  CompoitionSettings Settings { get; set; }
}
