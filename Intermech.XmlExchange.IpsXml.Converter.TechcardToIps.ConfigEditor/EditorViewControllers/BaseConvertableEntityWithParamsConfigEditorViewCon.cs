// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViewControllers.BaseConvertableEntityWithParamsConfigEditorViewController
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

[ConfigNodeType(NodeType.ObjectConfig)]
[ConfigNodeType(NodeType.RelationConfig)]
internal class BaseConvertableEntityWithParamsConfigEditorViewController(IServiceProvider services) : 
  BaseEditorViewController(services)
{
  private BaseConvertableEntityWithParamsConfigEditorView editorView = new BaseConvertableEntityWithParamsConfigEditorView();

  protected override Control View => (Control) this.editorView;

  public override event EventHandler<bool> OnDataChanged;

  public override bool ApplyChanges() => this.editorView.ApplyChanges();

  public override void CancelChanges() => this.OnBeforeShowEditorConfig();

  protected override void OnBeforeShowEditorConfig()
  {
    this.editorView.Dock = DockStyle.Fill;
    this.editorView.TargetConfig = this.TargetConfig as BaseConvertableEntityWithParamsConfig;
    this.editorView.GlobalServices = this.Services;
    this.editorView.OnDataChanged += this.OnDataChanged;
    this.editorView.PerformData();
  }
}
