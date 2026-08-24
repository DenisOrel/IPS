// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ReportsEditorActionHandler
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

public class ReportsEditorActionHandler : IFormDesignerActionHandler
{
  public bool ButtonEnabled([NotNull] object button, [NotNull] object form)
  {
    long resolutionId = ReportsEditorActionHandler.GetResolutionID(form);
    if (resolutionId == 0L)
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(resolutionId) is IDBResolution dbResolution && dbResolution.IsUserAnyOfRoles(ResolutionUserRoles.AnyRole);
  }

  public void ButtonPressed([NotNull] object button, [NotNull] object form)
  {
    long resolutionId = ReportsEditorActionHandler.GetResolutionID(form);
    if (resolutionId == 0L)
      return;
    using (ReportsEditor reportsEditor = new ReportsEditor(resolutionId))
    {
      reportsEditor.Init();
      int num = (int) reportsEditor.ShowDialog();
    }
  }

  private static long GetResolutionID([NotNull] object form)
  {
    return !(form is DesForm desForm) || desForm.Info == null ? 0L : desForm.Info.ElementIdentifier;
  }
}
