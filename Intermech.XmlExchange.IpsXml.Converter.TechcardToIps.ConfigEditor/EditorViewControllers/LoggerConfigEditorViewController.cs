// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViewControllers.LoggerConfigEditorViewController
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViewControllers;

[ConfigNodeType(NodeType.Logger)]
internal class LoggerConfigEditorViewController(IServiceProvider services) : BaseEditorViewController(services)
{
  private LoggerConfigControl editorView = new LoggerConfigControl();

  protected override Control View => (Control) this.editorView;

  public override event EventHandler<bool> OnDataChanged;

  protected override void OnBeforeShowEditorConfig()
  {
    this.editorView.Dock = DockStyle.Fill;
    this.editorView.GlobalServices = this.Services;
    this.editorView.OnDataChanged += this.OnDataChanged;
    this.editorView.TargetConfig = this.TargetConfig as LoggerConfig;
    this.editorView.PerformData();
  }

  public override bool ApplyChanges()
  {
    this.editorView.ApplyChanges();
    return true;
  }

  public override void CancelChanges() => this.OnBeforeShowEditorConfig();
}
