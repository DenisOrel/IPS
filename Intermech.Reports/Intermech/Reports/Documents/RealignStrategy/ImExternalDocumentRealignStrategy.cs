// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Documents.RealignStrategy.ImExternalDocumentRealignStrategy
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Document.Model.ExternalDocuments;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Reports;
using System;

#nullable disable
namespace Intermech.Reports.Documents.RealignStrategy;

internal class ImExternalDocumentRealignStrategy : DocumentRealignStrategy
{
  protected override bool LoadDocument(out ImDocument imDocument)
  {
    imDocument = (ImDocument) null;
    if (!this._reportDocument.DocRecord.state.HasFlag((Enum) DocState.DocLink))
      return false;
    this.AddTraceMessage(this._reportDocument, "Load document data ");
    ExternalDocumentCreator externalDocumentCreator = new ExternalDocumentCreator();
    imDocument = (ImDocument) externalDocumentCreator.CreateDocument(this._reportDocument.DocRecord.docObjectID, true);
    this.AddTraceMessage(this._reportDocument, " Unpack document data");
    return imDocument != null;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  protected override bool RealignDocument(ImDocument imDocument)
  {
    this.AddTraceMessage(this._reportDocument, "Update document data ");
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
        new ExternalDocumentCreator().UpdateDocumentDBObject(imDocument as ImExternalDocument, this._reportDocument.DocRecord.docObjectID, true);
      }
    }
    service.ConfirmDocAligned(this._reportTask.ExpertTaskId, this._reportDocument.DocRecord.docNumber);
    return true;
  }
}
