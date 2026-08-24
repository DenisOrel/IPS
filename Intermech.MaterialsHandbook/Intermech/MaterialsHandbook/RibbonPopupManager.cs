// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonPopupManager
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public static class RibbonPopupManager
{
  private static List<RibbonPopup> pops = new List<RibbonPopup>();

  internal static RibbonPopup LastPopup
  {
    get
    {
      return RibbonPopupManager.pops.Count <= 0 ? (RibbonPopup) null : RibbonPopupManager.pops[RibbonPopupManager.pops.Count - 1];
    }
  }

  public static void Dismiss(RibbonPopupManager.DismissReason reason)
  {
    RibbonPopupManager.Dismiss(0, reason);
  }

  public static void Dismiss(RibbonPopup startPopup, RibbonPopupManager.DismissReason reason)
  {
    int startPopup1 = RibbonPopupManager.pops.IndexOf(startPopup);
    if (startPopup1 < 0)
      return;
    RibbonPopupManager.Dismiss(startPopup1, reason);
  }

  private static void Dismiss(int startPopup, RibbonPopupManager.DismissReason reason)
  {
    for (int index = RibbonPopupManager.pops.Count - 1; index >= startPopup; --index)
      RibbonPopupManager.pops[index].Close();
  }

  public static void DismissChildren(RibbonPopup parent, RibbonPopupManager.DismissReason reason)
  {
    int num = RibbonPopupManager.pops.IndexOf(parent);
    if (num < 0)
      return;
    RibbonPopupManager.Dismiss(num + 1, reason);
  }

  internal static void FeedHookClick(MouseEventArgs e)
  {
    foreach (RibbonPopup pop in RibbonPopupManager.pops)
    {
      if (pop.WrappedDropDown.Bounds.Contains(e.Location))
        return;
    }
    RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.AppClicked);
  }

  internal static void Register(RibbonPopup p)
  {
    if (RibbonPopupManager.pops.Contains(p))
      return;
    RibbonPopupManager.pops.Add(p);
  }

  internal static void Unregister(RibbonPopup p)
  {
    if (!RibbonPopupManager.pops.Contains(p))
      return;
    RibbonPopupManager.pops.Remove(p);
  }

  public enum DismissReason
  {
    ItemClicked,
    AppClicked,
    NewPopup,
    AppFocusChanged,
    EscapePressed,
  }
}
