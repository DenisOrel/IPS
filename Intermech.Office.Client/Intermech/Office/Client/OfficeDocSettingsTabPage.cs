// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocSettingsTabPage
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DatabaseConfigurator;
using Intermech.Diagnostics;
using Intermech.PropertyEditors;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class OfficeDocSettingsTabPage : BaseTabPage, IAdditionalTabPage
{
  [NotNull]
  private readonly OfficeDocSettingsForm _officeForm;

  public OfficeDocSettingsTabPage(Guid aInstGuid)
    : base(aInstGuid, Localization.GetString("Office.Client_3"))
  {
    this._officeForm = new OfficeDocSettingsForm(aInstGuid, this);
  }

  public override void DockToPanel(Panel panel) => this._officeForm.SetParent(panel);

  [NotNull]
  public override ITabPageForm TabPageProcessingForm => (ITabPageForm) this._officeForm;

  public int Index => 3;

  [NotNull]
  public IBaseTabPage TabPage => (IBaseTabPage) this;
}
