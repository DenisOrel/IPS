// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViewControllers.OutputConfigEditorViewController
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViewControllers;

[ConfigNodeType(NodeType.OutPut)]
internal class OutputConfigEditorViewController(IServiceProvider services) : BaseEditorViewController(services)
{
  private OutputConfigControl editorView = new OutputConfigControl();

  public override event EventHandler<bool> OnDataChanged;

  protected override Control View => (Control) this.editorView;

  protected override void OnBeforeShowEditorConfig()
  {
    this.editorView.Dock = DockStyle.Fill;
    this.editorView.GlobalServices = this.Services;
    this.editorView.OnDataChanged += this.OnDataChanged;
    this.editorView.PerformData();
  }

  public override bool ApplyChanges()
  {
    this.editorView.ApplyChanges();
    return true;
  }

  public override void CancelChanges() => this.OnBeforeShowEditorConfig();
}
