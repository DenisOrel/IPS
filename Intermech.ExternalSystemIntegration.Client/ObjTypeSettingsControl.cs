// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ObjTypeSettingsControl
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class ObjTypeSettingsControl : UserControl, IPropertyPage
{
  private IContainer components;
  private SettingObjectsView settingObjectsView;

  public ObjTypeSettingsControl()
  {
    this.InitializeComponent();
    this.settingObjectsView.Initialize((IDescriptor) new Descriptor(Const.TypeSettingItemObjTypeID), (IServiceProvider) ServicesManager.ServiceContainer);
    this.settingObjectsView.Activate((IView) null);
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => string.Empty;

  public void Apply()
  {
  }

  public void Cancel()
  {
  }

  public string HelpTopicID => string.Empty;

  public string HeaderText => "Настройки для типов объектов";

  private void settingObjectsView_OnDataTableChangedDelegate(object sender, DataHelperEventArgs e)
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.settingObjectsView = new SettingObjectsView();
    this.SuspendLayout();
    this.settingObjectsView.AllowCustomGroupValues = true;
    this.settingObjectsView.Control = (object) this.settingObjectsView;
    this.settingObjectsView.DisableColumnsGrouping = true;
    this.settingObjectsView.DisableColumnsSettings = true;
    this.settingObjectsView.DisableColumnsSorting = true;
    this.settingObjectsView.DisableFiltration = true;
    this.settingObjectsView.DisableGroupBox = true;
    this.settingObjectsView.DisableHeaderContextMenu = true;
    this.settingObjectsView.DisableKeyDownEvents = false;
    this.settingObjectsView.DisableStatusBar = true;
    this.settingObjectsView.DisableToolBar = true;
    this.settingObjectsView.Dock = DockStyle.Fill;
    this.settingObjectsView.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.settingObjectsView.Font = new Font("Tahoma", 8.25f);
    this.settingObjectsView.Location = new Point(0, 0);
    this.settingObjectsView.Name = "settingObjectsView";
    this.settingObjectsView.Padding = new Padding(15);
    this.settingObjectsView.Size = new Size(740, 412);
    this.settingObjectsView.TabIndex = 0;
    this.settingObjectsView.OnDataTableChangedDelegate += new EventHandler<DataHelperEventArgs>(this.settingObjectsView_OnDataTableChangedDelegate);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.settingObjectsView);
    this.Name = nameof (ObjTypeSettingsControl);
    this.Size = new Size(740, 412);
    this.ResumeLayout(false);
  }
}
