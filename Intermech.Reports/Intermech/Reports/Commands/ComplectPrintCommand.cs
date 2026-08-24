// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Commands.ComplectPrintCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.DataFormats;
using Intermech.Document.Model.UI;
using Intermech.Interfaces;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Commands;

/// <summary>Выполнение команды меню "PrintDocument"</summary>
internal class ComplectPrintCommand : ComplectBaseCommand
{
  /// <summary>Выполнение команды комплекта</summary>
  protected override void DoExecute_ComplectCommand()
  {
    if (this._docComplect == null || this._docComplect.Nodes == null || this._docComplect.Nodes.Count == 0)
      return;
    IDBObjectID itemData = (IDBObjectID) this._items.GetItemData(0, typeof (IDBObjectID));
    if (itemData != null)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        sessionKeeper.Session.GetObject(itemData.Value).Print();
    }
    this._docComplect.BeforeShowPrintDialog();
    this._docComplect.PrintDocument.PrinterSettings.MinimumPage = 1;
    this._docComplect.PrintDocument.PrinterSettings.FromPage = 1;
    this._docComplect.PrintDocument.PrinterSettings.MaximumPage = this._docComplect.PageCount;
    this._docComplect.PrintDocument.PrinterSettings.ToPage = this._docComplect.PageCount;
    this._docComplect.PrintDocument.PrinterSettings.PrintRange = PrintRange.AllPages;
    if (new PrintComplectDialog(this._docComplect.PrintDocument, this._docComplect).ShowDialog() != DialogResult.OK)
      return;
    this._docComplect.PrintDocument.Print();
  }
}
