// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Commands.ComplectShowCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Document.Client;
using Intermech.Interfaces.Document;

#nullable disable
namespace Intermech.Reports.Commands;

/// <summary>Выполнение команды меню "ViewDocument"</summary>
internal class ComplectShowCommand : ComplectBaseCommand
{
  /// <summary>Выполнение команды комплекта</summary>
  protected override void DoExecute_ComplectCommand()
  {
    if (this._docComplect?.Nodes == null || this._docComplect.Nodes.Count == 0 || DocumentEditorPlugin.Instance.OpenDocument((DocumentTreeNode) this._docComplect, true, true) == null)
      return;
    this._docComplect = (DocumentsComplect) null;
  }
}
