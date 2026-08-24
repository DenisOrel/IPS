// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Documents.RealignStrategy.DocumentRealignStrategy
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.ApplicationModel;
using Intermech.Diagnostics;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Expert;
using Intermech.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Intermech.Reports.Documents.RealignStrategy;

internal abstract class DocumentRealignStrategy
{
  /// <summary>
  /// 
  /// </summary>
  protected ReportsBaseTask _reportTask;
  /// <summary>
  /// 
  /// </summary>
  protected ReportDocument _reportDocument;

  /// <summary>Необходимость в разбиении загруженного документа</summary>
  /// <returns></returns>
  private bool NeedRealignDocument()
  {
    return this._reportDocument.DocRecord != null && !this._reportDocument.DocRecord.state.HasFlag((Enum) DocState.Aligned);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="docRecord"></param>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  private bool SaveDocument(IUserSession session, ImDocument imDocument)
  {
    byte[] doc = ReportsBaseTask.PackImDocument(imDocument);
    lock (this._reportDocument.SyncObject)
    {
      IExpertServer service = ServiceUtils.GetService<IExpertServer>((object) session, true);
      this.AddTraceMessage(this._reportDocument, "Expert server SetDocument ");
      try
      {
        service.SetDocument(this._reportTask.ExpertTaskId, session.SessionGUID, this._reportDocument.DocRecord.docNumber, doc, imDocument.PageCount);
      }
      catch (Exception ex)
      {
        string str1 = string.Empty;
        if (ex is AccessDeniedException)
          str1 = string.Join(Environment.NewLine, session.GetCheckAccessLog(GetAccessModes.LastCheck)) + Environment.NewLine;
        string str2 = Environment.NewLine + str1 + ex.Message;
        this._reportTask.DoErrorOutput(string.Format(LocalizationHolder.rm.GetString("Reports_43"), (object) this._reportDocument.DocRecord.docName, (object) this._reportDocument.DocRecord.objID) + string.Format(LocalizationHolder.rm.GetString("Reports_5"), (object) str2));
        this._reportTask.DoErrorOutput(ex.StackTrace);
      }
    }
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="message"></param>
  protected void AddTraceMessage(ReportDocument reportDocument, string message)
  {
    if (!TraceSupport.DocumentRealign.Enabled)
      return;
    DocRecord docRecord = reportDocument.DocRecord;
    IApplicationEventLogService service = ServiceUtils.GetService<IApplicationEventLogService>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    string str = docRecord != null ? $" ObjectId = {docRecord.objID} DocNum = {docRecord.docNumber} Name = '{docRecord.docName}' : " : string.Empty;
    service.FileLog.Write($"Report ThreadId = {Thread.CurrentThread.ManagedThreadId} TaskId = {Task.CurrentId}. {str} {message}");
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  protected abstract bool LoadDocument(out ImDocument imDocument);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="imDocument"></param>
  protected abstract bool RealignDocument(ImDocument imDocument);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="session"></param>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  protected abstract bool ReloadDocumentInfo(IUserSession session, ImDocument imDocument);

  public bool Execute(
    [NotNull] ReportsBaseTask reportTask,
    [NotNull] ReportDocument reportDocument,
    out ImDocument imDocument)
  {
    imDocument = (ImDocument) null;
    this._reportTask = reportTask;
    this._reportDocument = reportDocument;
    if (!this.LoadDocument(out imDocument) || !this._reportTask.IsActive)
      return false;
    if (!this.NeedRealignDocument())
      return true;
    if (!this._reportTask.IsActive || !this.RealignDocument(imDocument) || !this._reportTask.IsActive)
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return this.SaveDocument(sessionKeeper.Session, imDocument) && this._reportTask.IsActive && this.ReloadDocumentInfo(sessionKeeper.Session, imDocument);
  }
}
