// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Commands.ComplectSaveCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using ImSSP;
using Intermech.Document.UI;
using Intermech.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Commands;

/// <summary>Выполнение команды меню "SaveToDisk"</summary>
internal class ComplectSaveCommand : ComplectBaseCommand
{
  /// <summary>Имя файла сохраненного комплекта</summary>
  private string _complectFileName = string.Empty;
  /// <summary>Признак сжатия файла</summary>
  private bool _complectPackFile;

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  protected override bool DoExecute_ComplectDataLoad()
  {
    if (this._objInfoList == null || this._objInfoList.Count == sc_17679.ssp_imclient_17680(630711448))
      return false;
    string str1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(this._objInfoList[0].ObjectID);
      str1 = !objectInfo.Empty ? objectInfo.Caption : "Documents Complect";
    }
    string str2 = str1 + ".idcx";
    SaveFileDialog saveFileDialog = ImDocumentEditorFormBase.CreateSaveFileDialog();
    saveFileDialog.Filter = ImDocumentEditorFormBase.ImDocumentsComplectFilter;
    saveFileDialog.FileName = str2;
    if (saveFileDialog.ShowDialog() != DialogResult.OK)
      return false;
    this._complectFileName = saveFileDialog.FileName;
    this._complectPackFile = saveFileDialog.Filter.IndexOf(".zimd", StringComparison.OrdinalIgnoreCase) != -1 || saveFileDialog.Filter.IndexOf(".zidc", StringComparison.OrdinalIgnoreCase) != -1;
    return base.DoExecute_ComplectDataLoad();
  }

  /// <summary>Выполнение команды комплекта</summary>
  protected override void DoExecute_ComplectCommand()
  {
    if (this._docComplect?.Nodes == null || this._docComplect.Nodes.Count == 0)
      return;
    this._docComplect.SaveToXml(this._complectFileName, this._complectPackFile);
  }
}
