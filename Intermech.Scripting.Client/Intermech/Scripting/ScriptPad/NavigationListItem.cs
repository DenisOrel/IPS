// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.NavigationListItem
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using Telerik.WinControls.UI;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class NavigationListItem : RadListDataItem
{
  public NavigationListItem(NavigationItem navigationItem)
  {
    if (navigationItem == null)
      throw new ArgumentNullException(nameof (navigationItem));
    this.NavigationItem = new NavigationItem(navigationItem.FullName, navigationItem.Range, navigationItem.SelectionRange, navigationItem.Type, navigationItem.Children);
    this.Text = this.NavigationItem.FullName;
    this.Image = CodeCompletionIconProvider.GetIcon(this.NavigationItem.Type);
  }

  public NavigationItem NavigationItem { get; private set; }

  public void Update(NavigationItem navigationItem)
  {
    if (navigationItem == null)
      throw new ArgumentNullException(nameof (navigationItem));
    this.NavigationItem.CopyData(navigationItem);
    this.Text = this.NavigationItem.FullName;
    this.Image = CodeCompletionIconProvider.GetIcon(this.NavigationItem.Type);
  }

  public override bool Equals(object obj)
  {
    return obj is NavigationListItem navigationListItem && this.NavigationItem.Equals((object) navigationListItem.NavigationItem);
  }

  public override int GetHashCode() => 601059876 + this.NavigationItem.GetHashCode();
}
