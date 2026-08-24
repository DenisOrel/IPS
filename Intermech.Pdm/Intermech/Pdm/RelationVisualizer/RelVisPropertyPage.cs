// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.RelVisPropertyPage
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class RelVisPropertyPage : IPropertyPage, IPropertyPageSearchOptionEvents
{
  private IServiceProvider _provider;
  private IRelVisSettings settins;

  public RelVisPropertyPage(IServiceProvider provider)
  {
    this.settins = ServicesManager.GetService(typeof (IRelVisSettings)) as IRelVisSettings;
    this._provider = provider;
    if (!(this._provider.GetService(typeof (IPropertyPagesService)) is IPropertyPagesService service))
      return;
    service.AddPage(LocalizationHolder.rm.GetString("Pdm_rv_24"), (IPropertyPage) this);
  }

  protected virtual void UpdateControls()
  {
  }

  protected virtual void LoadFromEditors()
  {
  }

  private void OnChanged()
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, new EventArgs());
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Object;

  public object Control => (object) this.settins.Settings;

  public string PageName => LocalizationHolder.rm.GetString("Pdm_rv_16");

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public void Apply()
  {
    this.LoadFromEditors();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.settins.SaveSettings(sessionKeeper.Session);
    this.OnChanged();
  }

  public void Cancel()
  {
  }

  public string HelpTopicID => "-1";

  public List<string> GetOptionNames()
  {
    return this.Control == null ? new List<string>() : IPropertyPageHelper.GetOptionNames(this.Control);
  }

  private void propertyGrid_Props_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
  }
}
