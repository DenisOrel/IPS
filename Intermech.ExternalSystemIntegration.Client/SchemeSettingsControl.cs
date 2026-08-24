// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.SchemeSettingsControl
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class SchemeSettingsControl : UserControl, IPropertyPage
{
  private IContainer components;
  private TabControl tabControlSchemes;
  private TabPage tabPageRequestSchemes;
  private SettingObjectsView _RequestSchemesObjectsView;
  private TabPage tabPageresponceSchemes;
  private SettingObjectsView _ResponceSchemesObjectsView;

  public SchemeSettingsControl()
  {
    this.InitializeComponent();
    Descriptor rootDescriptor1 = new Descriptor(Const.RequestSchemeObjTypeID);
    Descriptor rootDescriptor2 = new Descriptor(Const.ResponceSchemeObjTypeID);
    this._RequestSchemesObjectsView.Initialize((IDescriptor) rootDescriptor1, (IServiceProvider) ServicesManager.ServiceContainer);
    this._RequestSchemesObjectsView.Activate((IView) null);
    this._ResponceSchemesObjectsView.Initialize((IDescriptor) rootDescriptor2, (IServiceProvider) ServicesManager.ServiceContainer);
    this._ResponceSchemesObjectsView.Activate((IView) null);
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

  public string HeaderText => "Схемы трансформации";

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tabControlSchemes = new TabControl();
    this.tabPageRequestSchemes = new TabPage();
    this._RequestSchemesObjectsView = new SettingObjectsView();
    this.tabPageresponceSchemes = new TabPage();
    this._ResponceSchemesObjectsView = new SettingObjectsView();
    this.tabControlSchemes.SuspendLayout();
    this.tabPageRequestSchemes.SuspendLayout();
    this.tabPageresponceSchemes.SuspendLayout();
    this.SuspendLayout();
    this.tabControlSchemes.Controls.Add((System.Windows.Forms.Control) this.tabPageRequestSchemes);
    this.tabControlSchemes.Controls.Add((System.Windows.Forms.Control) this.tabPageresponceSchemes);
    this.tabControlSchemes.Dock = DockStyle.Fill;
    this.tabControlSchemes.Location = new Point(0, 0);
    this.tabControlSchemes.Name = "tabControlSchemes";
    this.tabControlSchemes.Padding = new Point(15, 3);
    this.tabControlSchemes.SelectedIndex = 0;
    this.tabControlSchemes.Size = new Size(740, 412);
    this.tabControlSchemes.TabIndex = 2;
    this.tabPageRequestSchemes.Controls.Add((System.Windows.Forms.Control) this._RequestSchemesObjectsView);
    this.tabPageRequestSchemes.Location = new Point(4, 22);
    this.tabPageRequestSchemes.Name = "tabPageRequestSchemes";
    this.tabPageRequestSchemes.Padding = new Padding(15);
    this.tabPageRequestSchemes.Size = new Size(732, 386);
    this.tabPageRequestSchemes.TabIndex = 0;
    this.tabPageRequestSchemes.Text = "Исходящие схемы";
    this.tabPageRequestSchemes.UseVisualStyleBackColor = true;
    this._RequestSchemesObjectsView.AllowCustomGroupValues = true;
    this._RequestSchemesObjectsView.Control = (object) this._RequestSchemesObjectsView;
    this._RequestSchemesObjectsView.DisableColumnsGrouping = true;
    this._RequestSchemesObjectsView.DisableColumnsSettings = true;
    this._RequestSchemesObjectsView.DisableColumnsSorting = true;
    this._RequestSchemesObjectsView.DisableFiltration = true;
    this._RequestSchemesObjectsView.DisableGroupBox = true;
    this._RequestSchemesObjectsView.DisableHeaderContextMenu = true;
    this._RequestSchemesObjectsView.DisableKeyDownEvents = false;
    this._RequestSchemesObjectsView.DisableStatusBar = true;
    this._RequestSchemesObjectsView.DisableToolBar = true;
    this._RequestSchemesObjectsView.Dock = DockStyle.Fill;
    this._RequestSchemesObjectsView.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this._RequestSchemesObjectsView.Font = new Font("Tahoma", 8.25f);
    this._RequestSchemesObjectsView.Location = new Point(15, 15);
    this._RequestSchemesObjectsView.Name = "_RequestSchemesObjectsView";
    this._RequestSchemesObjectsView.Size = new Size(702, 356);
    this._RequestSchemesObjectsView.TabIndex = 1;
    this.tabPageresponceSchemes.Controls.Add((System.Windows.Forms.Control) this._ResponceSchemesObjectsView);
    this.tabPageresponceSchemes.Location = new Point(4, 22);
    this.tabPageresponceSchemes.Name = "tabPageresponceSchemes";
    this.tabPageresponceSchemes.Padding = new Padding(15);
    this.tabPageresponceSchemes.Size = new Size(732, 386);
    this.tabPageresponceSchemes.TabIndex = 1;
    this.tabPageresponceSchemes.Text = "Входящие схемы";
    this.tabPageresponceSchemes.UseVisualStyleBackColor = true;
    this._ResponceSchemesObjectsView.AllowCustomGroupValues = true;
    this._ResponceSchemesObjectsView.Control = (object) this._ResponceSchemesObjectsView;
    this._ResponceSchemesObjectsView.DisableColumnsGrouping = true;
    this._ResponceSchemesObjectsView.DisableColumnsSettings = true;
    this._ResponceSchemesObjectsView.DisableColumnsSorting = true;
    this._ResponceSchemesObjectsView.DisableFiltration = true;
    this._ResponceSchemesObjectsView.DisableGroupBox = true;
    this._ResponceSchemesObjectsView.DisableHeaderContextMenu = true;
    this._ResponceSchemesObjectsView.DisableKeyDownEvents = false;
    this._ResponceSchemesObjectsView.DisableStatusBar = true;
    this._ResponceSchemesObjectsView.DisableToolBar = true;
    this._ResponceSchemesObjectsView.Dock = DockStyle.Fill;
    this._ResponceSchemesObjectsView.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this._ResponceSchemesObjectsView.Font = new Font("Tahoma", 8.25f);
    this._ResponceSchemesObjectsView.Location = new Point(15, 15);
    this._ResponceSchemesObjectsView.Name = "_ResponceSchemesObjectsView";
    this._ResponceSchemesObjectsView.Size = new Size(702, 356);
    this._ResponceSchemesObjectsView.TabIndex = 1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.tabControlSchemes);
    this.Name = nameof (SchemeSettingsControl);
    this.Size = new Size(740, 412);
    this.tabControlSchemes.ResumeLayout(false);
    this.tabPageRequestSchemes.ResumeLayout(false);
    this.tabPageresponceSchemes.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
