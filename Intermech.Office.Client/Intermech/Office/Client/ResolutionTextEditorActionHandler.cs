// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionTextEditorActionHandler
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

internal class ResolutionTextEditorActionHandler : IFormDesignerActionHandler
{
  public bool ButtonEnabled([NotNull] object button, [NotNull] object form)
  {
    DesForm desForm = Intermech.Diagnostics.Check.Is<DesForm>(form, nameof (form));
    if (desForm.Info.ElementKind != AttributableElements.Object)
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBResolution resolution = sessionKeeper.Session.GetResolution(desForm.Info.ElementIdentifier);
      return resolution != null && resolution.IsUserAnyOfRoles(ResolutionUserRoles.AnyRole);
    }
  }

  public void ButtonPressed([NotNull] object button, [NotNull] object form)
  {
    DesForm desForm = Intermech.Diagnostics.Check.Is<DesForm>(form, nameof (form));
    if (desForm.Info == null || desForm.Info.ElementIdentifier == 0L)
      return;
    using (TextResolutionEditor resolutionEditor = new TextResolutionEditor(desForm.Info.ElementIdentifier))
    {
      resolutionEditor.Init();
      int num = (int) resolutionEditor.ShowDialog();
    }
  }
}
