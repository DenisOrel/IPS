// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Documents.RealignStrategy.ImDocumentRealignStrategy
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Reports;
using System;

#nullable disable
namespace Intermech.Reports.Documents.RealignStrategy;

internal class ImDocumentRealignStrategy : DocumentRealignStrategy
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  protected override bool LoadDocument(out ImDocument imDocument)
  {
    imDocument = (ImDocument) null;
    this.AddTraceMessage(this._reportDocument, "Load document data ");
    byte[] zipScr;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      zipScr = ServiceUtils.GetService<IExpertServer>((object) sessionKeeper.Session, true).GetDocument(this._reportTask.ExpertTaskId, this._reportDocument.DocRecord.docNumber);
      if (zipScr == null)
      {
        if (this._reportDocument.DocRecord.state.HasFlag((Enum) DocState.Aligned))
        {
          lock (this._reportDocument.SyncObject)
          {
            if (sessionKeeper.Session.GetObject(this._reportDocument.DocRecord.docObjectID, false)?.GetAttributeByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"), false) is IBlobReader attributeByGuid)
            {
              BlobInformation blobInformation = attributeByGuid.OpenBlob(0);
              try
              {
                zipScr = attributeByGuid.ReadDataBlock((int) blobInformation.RealFileSize);
              }
              finally
              {
                attributeByGuid.CloseBlob();
              }
            }
          }
        }
      }
    }
    if (zipScr == null || zipScr.Length == 0 || !this._reportTask.IsActive)
      return false;
    this.AddTraceMessage(this._reportDocument, " Unpack document data");
    imDocument = ReportsBaseTask.UnpackImDocument(zipScr, false);
    return imDocument != null;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  protected override bool RealignDocument(ImDocument imDocument)
  {
    if (!Consts.IsUndefinedObjectId(this._reportDocument.DocRecord.oldObjectID))
    {
      imDocument.SaveValueFromRefToDBAttr = false;
      this.AddTraceMessage(this._reportDocument, "Update document data ");
      lock (this._reportDocument.SyncObject)
        DocumentEditorPlugin.UpdateDocumentDBObject(imDocument, this._reportDocument.DocRecord.oldObjectID, true, false);
    }
    this.AddTraceMessage(this._reportDocument, "Realign document");
    if (!this._reportTask.DoCustomRealignDocument(this._reportDocument, imDocument))
      imDocument.UpdateLayout(true, false);
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="session"></param>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  protected override bool ReloadDocumentInfo(IUserSession session, ImDocument imDocument)
  {
    IExpertServer service = ServiceUtils.GetService<IExpertServer>((object) session, true);
    this._reportDocument.DocRecord = service.GetDocRecord(this._reportTask.ExpertTaskId, this._reportDocument.DocRecord.docNumber);
    if (!this._reportTask.Options.HasFlag((Enum) ReportTaskOptions.HideDocWindow) && !Consts.IsUndefinedObjectId(this._reportDocument.DocRecord.docObjectID) && this._reportDocument.DocRecord.docObjectID != this._reportDocument.DocRecord.oldObjectID)
    {
      lock (this._reportDocument.SyncObject)
      {
        DocumentEditorPlugin.Instance.UpdateCheckSum((IUserSession) null, (ImDocumentData) imDocument, this._reportDocument.DocRecord.docObjectID, ReportsConsts.FileAttributeTypeID, 0, true);
        DocumentEditorPlugin.UpdateDocumentDBObject(imDocument, this._reportDocument.DocRecord.docObjectID, true, false);
      }
    }
    service.ConfirmDocAligned(this._reportTask.ExpertTaskId, this._reportDocument.DocRecord.docNumber);
    return true;
  }
}
