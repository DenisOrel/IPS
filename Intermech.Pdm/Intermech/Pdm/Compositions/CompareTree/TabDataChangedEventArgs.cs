// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.TabDataChangedEventArgs
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class TabDataChangedEventArgs
{
  public Guid TabGuid { get; private set; }

  public TabDataChangedEventArgs(Guid tabGuid) => this.TabGuid = tabGuid;
}
