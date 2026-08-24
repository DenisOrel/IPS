// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.HideViewsCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Bars;
using Intermech.Navigator.Controls;
using System;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда для скрытия закладок в окне навигатора</summary>
internal class HideViewsCommand
{
  private NavWindowBase _window;
  public static bool Hide;

  public HideViewsCommand(NavWindowBase window) => this._window = window;

  internal void ClickHandler(object sender, EventArgs e)
  {
    HideViewsCommand.Hide = !HideViewsCommand.Hide;
    if (sender is ButtonItem buttonItem)
      HideViewsCommand.Hide = buttonItem.Checked;
    this._window.ToggleViewsManager(HideViewsCommand.Hide);
  }
}
