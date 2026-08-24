// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Commands.ComplectBaseCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Document.Client;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Reports;

#nullable disable
namespace Intermech.Reports.Commands;

/// <summary>Базовый класс для контекстных команд комплектов</summary>
internal abstract class ComplectBaseCommand : ReportBaseCommand
{
  /// <summary>
  /// 
  /// </summary>
  protected DocumentsComplect _docComplect;

  /// <summary>Загрузка данных комплекта</summary>
  /// <returns></returns>
  protected virtual bool DoExecute_ComplectDataLoad()
  {
    return ComplectBaseCommand.LoadDocumentComplect(this._objInfoList[0].ObjectID, out this._docComplect);
  }

  /// <summary>Выполнение команды комплекта</summary>
  protected abstract void DoExecute_ComplectCommand();

  /// <summary>Загрузка информации об объектах</summary>
  /// <returns></returns>
  protected override bool DoExecute_LoadObjInfo()
  {
    return base.DoExecute_LoadObjInfo() && this._objInfoList != null && this._objInfoList.Count != 0 && MetaDataHelper.IsObjectTypeChildOf(this._objInfoList[0].ObjTypeID, ReportsConsts.DocPackageBaseTypeID);
  }

  /// <summary>Выполнение команды</summary>
  protected override void DoExecute_Command()
  {
    try
    {
      if (!this.DoExecute_ComplectDataLoad())
        return;
      this.DoExecute_ComplectCommand();
    }
    finally
    {
      if (this._docComplect != null)
      {
        this._docComplect.Dispose();
        this._docComplect = (DocumentsComplect) null;
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="objectId"></param>
  /// <param name="docComplect"></param>
  /// <returns></returns>
  public static bool LoadDocumentComplect(long objectId, out DocumentsComplect docComplect)
  {
    docComplect = (DocumentsComplect) null;
    ReportsDocComplect complect;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (ServiceUtils.GetService<IReportsServerService>((object) sessionKeeper.Session, true).LoadComplectData(objectId, out complect, sessionKeeper.Session.SessionGUID, ReportsDocModes.IncludeObligatoryAttributes | ReportsDocModes.IncludeDocData))
      {
        if (complect != null)
          goto label_7;
      }
      return false;
    }
label_7:
    ServiceUtils.GetService<IReportUtils>((object) ApplicationServices.Container, true).RestoreComplectData((ReportsBaseDoc) complect, out docComplect);
    DocumentEditorPlugin.Instance.UpdateDocumentLinks((DocumentTreeNode) docComplect, true, true, false, false, false);
    return true;
  }
}
