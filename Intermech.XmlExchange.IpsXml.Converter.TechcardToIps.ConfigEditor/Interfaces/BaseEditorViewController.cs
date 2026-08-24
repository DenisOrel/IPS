// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Interfaces.BaseEditorViewController
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Interfaces;

internal abstract class BaseEditorViewController : IEditorViewController
{
  protected readonly IServiceProvider Services;

  public BaseEditorViewController(IServiceProvider services) => this.Services = services;

  public BaseConfig TargetConfig { get; set; }

  public BaseConfig TargetParentConfig { get; set; }

  public void ShowOnParent(Control parent)
  {
    this.OnBeforeShowEditorConfig();
    parent.Controls.Clear();
    parent.Controls.Add(this.View);
  }

  public abstract bool ApplyChanges();

  public abstract void CancelChanges();

  public abstract event EventHandler<bool> OnDataChanged;

  protected abstract void OnBeforeShowEditorConfig();

  protected abstract Control View { get; }
}
