// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ReportsBaseTask
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ImSSP;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Reports;
using Intermech.Localization;
using Intermech.Reports.Documents;
using Intermech.Reports.Documents.RealignStrategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Intermech.Reports;

/// <summary>Базовый класс для реализации задач для отчетов</summary>
public abstract class ReportsBaseTask : MarshalByRefObject, IReportsBaseTask
{
  /// <summary>Задержка между вызовами "долгоиграющих" методов</summary>
  internal const int SlowMethodsDelayValue = 100;
  /// <summary>Задержка между вызовами методов в цикле</summary>
  private const int ThreadDelayValue = 300;
  /// <summary>Кол-во задержек для потоков</summary>
  private const int ThreadDelayCount = 2;
  /// <summary>Время жизни задачи ЭС без клиента</summary>
  private const int AliveTimeout = 10000;
  /// <summary>
  /// Интервал / задержка между уведомлении ЭС о наличии клиента
  /// </summary>
  /// <remarks>Общее значение рассчитывается как cnt_Alive_Interval_Delay * cnt_Alive_Interval_Count</remarks>
  private const int AliveIntervalDelay = 500;
  /// <summary>Кол-во интервалов ожидания</summary>
  /// <remarks>Чтобы </remarks>
  private const int AliveIntervalCount = 3;
  /// <summary>Список документов / комплектов задачи</summary>
  private readonly List<DocRecord> _docList;
  /// <summary>Фоновый поток задачи</summary>
  private readonly Thread _taskThread;
  /// <summary>
  /// Спец. поток для уведомления задачи ЭС в наличии работающего клиента
  /// </summary>
  private readonly Thread _aliveThread;
  /// <summary>Серверная служба ЭС</summary>
  protected IExpertServer _expertSrv;

  /// <summary>
  /// Получение списка генерируемых документов / комплектов от ЭС
  /// </summary>
  private void DoAfterGenerateDocList()
  {
    ReportDocEventHandler afterGenerateDocList = this.AfterGenerateDocList;
    if (afterGenerateDocList == null)
      return;
    afterGenerateDocList((object) this, new ReportDocBaseEvent((DocRecord) null));
  }

  /// <summary>Генерация документа</summary>
  /// <param name="docRecord"></param>
  /// <param name="traceInfo"></param>
  private void DoGenerateDocument(DocRecord docRecord, byte[] traceInfo)
  {
    ReportDocEventHandler generateDocument = this.GenerateDocument;
    if (generateDocument == null)
      return;
    generateDocument((object) this, (ReportDocBaseEvent) new ReportDocEvent(docRecord, traceInfo));
  }

  /// <summary>Окончание разбиения документа</summary>
  private void DoAfterRealignDocument(DocRecord docRecord, ImDocument imDocument)
  {
    ReportDocEventHandler afterRealignDocument = this.AfterRealignDocument;
    if (afterRealignDocument == null)
      return;
    afterRealignDocument((object) this, (ReportDocBaseEvent) new ReportDocEvent(docRecord, (ImDocumentData) imDocument));
  }

  /// <summary>Завершение генерации комплекта документов</summary>
  private void DoAfterGenerateComplete()
  {
    ReportDocEventHandler generateComplete = this.AfterGenerateComplete;
    if (generateComplete == null)
      return;
    generateComplete((object) this, new ReportDocBaseEvent((DocRecord) null));
  }

  /// <summary>Вывод информации об исключении</summary>
  /// <param name="e"></param>
  /// <param name="writeStackInfo"></param>
  private void DoExceptionOutput(Exception e, bool writeStackInfo = true)
  {
    if (this.ErrorOutput == null)
      return;
    this.DoErrorOutput(Environment.NewLine + (string.Format(LocalizationHolder.rm.GetString("Reports_14"), (object) this.Params.ObjectId, (object) this.Params.ScriptObjId) + string.Format(LocalizationHolder.rm.GetString("Reports_5"), (object) e.Message)) + e.Message);
    if (!writeStackInfo)
      return;
    this.DoErrorOutput(e.StackTrace);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="text"></param>
  internal void DoErrorOutput(string text)
  {
    if (this.Options.HasFlag((Enum) ReportTaskOptions.HideOutputWindow))
      return;
    ValueObjChangedHandler errorOutput = this.ErrorOutput;
    if (errorOutput == null)
      return;
    errorOutput((object) this, (object) text);
  }

  /// <summary>Окончание разбиения документа</summary>
  internal bool DoCustomRealignDocument(ReportDocument reportDocument, ImDocument imDocument)
  {
    ReportDocEventHandler customRealignDocument = this.CustomRealignDocument;
    if (customRealignDocument == null)
      return false;
    customRealignDocument((object) this, (ReportDocBaseEvent) new ReportDocEvent(reportDocument.DocRecord, (ImDocumentData) imDocument));
    return true;
  }

  /// <summary>Проверка параметров задачи</summary>
  /// <returns></returns>
  protected abstract ExpertResult ValidateParams();

  /// <summary>Метод выполнения</summary>
  /// <param name="changeLog"></param>
  /// <returns></returns>
  protected abstract ExpertResult ExecuteInternal(out List<ChangeInfo> changeLog);

  /// <summary>Получение и проверка необходимых для выполнения служб</summary>
  /// <returns></returns>
  private ExpertResult GetServices(IUserSession session)
  {
    if (session == null)
    {
      string str = string.Format(LocalizationHolder.rm.GetString(sc_17706.ssp_imclient_17707()), (object) typeof (IUserSession));
      this.DoErrorOutput(string.Format(LocalizationHolder.rm.GetString(sc_17706.ssp_imclient_17708()), (object) str));
      return ExpertResult.ObjectNotFound;
    }
    this._expertSrv = session.GetCustomService(typeof (IExpertServer)) as IExpertServer;
    if (this._expertSrv != null)
      return ExpertResult.OK;
    string str1 = string.Format(LocalizationHolder.rm.GetString("Reports_33"), (object) typeof (IExpertServer));
    this.DoErrorOutput(string.Format(LocalizationHolder.rm.GetString("Reports_5"), (object) str1));
    return ExpertResult.ObjectNotFound;
  }

  /// <summary>Сохранение параметров задачи</summary>
  private void SaveTaskParams()
  {
    AttributeValues[] attributes = this.Params.Attributes;
    if (attributes == null)
      return;
    if (attributes.Length == 0)
      return;
    DocRecord[] docArray;
    try
    {
      docArray = this._expertSrv.GetDocArray(this.ExpertTaskId);
    }
    catch (Exception ex)
    {
      if (ex is ThreadAbortException)
        return;
      this.DoErrorOutput(LocalizationHolder.rm.GetString("Reports_57") + string.Format(LocalizationHolder.rm.GetString("Reports_5"), (object) ex.Message));
      return;
    }
    List<DocRecord> docRecordList = new List<DocRecord>(2);
    foreach (DocRecord docRecord in docArray)
    {
      if (docRecord != null && docRecord.IsComplect() && docRecord.parentIndex == -1 && docRecord.objID == this.Params.ObjectId)
        docRecordList.Add(docRecord);
    }
    if (docRecordList.Count == 0)
      return;
    foreach (AttributeValues attributeValues in attributes)
      attributeValues.ThrowSetException = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (DocRecord docRecord in docRecordList)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(docRecord.docObjectID, false);
        if (dbObject != null)
        {
          AttributeValues[] attributesValues = dbObject.GetAttributesValues(GetAttributeValuesModes.IncludeObligatoryAttributes | GetAttributeValuesModes.CheckVisibility);
          List<int> existingAttrIDs = (attributesValues != null ? ((IEnumerable<AttributeValues>) attributesValues).Select<AttributeValues, int>((Func<AttributeValues, int>) (item => item.AttributeID)).ToList<int>() : (List<int>) null) ?? new List<int>();
          AttributeValues[] array = ((IEnumerable<AttributeValues>) attributes).Where<AttributeValues>((Func<AttributeValues, bool>) (item => !existingAttrIDs.Contains(item.AttributeID))).ToArray<AttributeValues>();
          if (array.Length != 0)
          {
            bool flag = false;
            if (dbObject.ReadOnly)
            {
              if (dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
              {
                flag = dbObject.CheckoutBy == 0L;
                if (flag || dbObject.CheckoutBy == sessionKeeper.Session.UserID)
                  dbObject = dbObject.CheckOut(false);
                else
                  continue;
              }
              else
                continue;
            }
            dbObject.SetAttributesValues(array, false, true);
            if (flag)
              dbObject.CheckIn();
          }
        }
      }
    }
  }

  /// <summary>
  /// Загрузка, разбиение документа, обновление его на сервере
  /// </summary>
  /// <param name="docRecord"></param>
  /// <param name="prevTasks">Список задач для предыдущих документов</param>
  private bool DocumentRealign(ReportDocument reportDocument, Task[] prevTasks)
  {
    if (reportDocument?.DocRecord == null || !reportDocument.DocRecord.state.HasFlag((Enum) DocState.Ready) || !this.IsActive)
      return false;
    DocumentRealignStrategy strategy = DocumentUpdateStrategyFactory.Instance.CreateStrategy(reportDocument);
    ImDocument imDocument = (ImDocument) null;
    if ((strategy != null ? (!strategy.Execute(this, reportDocument, out imDocument) ? 1 : 0) : 0) != 0)
      return false;
    if (prevTasks != null)
      Task.WaitAll(prevTasks);
    this.DoAfterRealignDocument(reportDocument.DocRecord, imDocument);
    return true;
  }

  /// <summary>Основная процедура потока задачи....</summary>
  private void TaskThreadProc()
  {
    try
    {
      List<Task> taskList = new List<Task>();
      DocRecord[] docArray;
      do
      {
        int num = 0;
        for (int index = Environment.TickCount + 3000; num < 2 && this.IsActive && index >= Environment.TickCount; ++num)
          Thread.Sleep(300);
        if (this.IsActive)
        {
          try
          {
            docArray = this._expertSrv.GetDocArray(this.ExpertTaskId);
          }
          catch (Exception ex)
          {
            if (ex is ThreadAbortException)
              return;
            this.DoExceptionOutput(ex);
            return;
          }
        }
        else
          goto label_58;
      }
      while (docArray == null);
      goto label_11;
label_58:
      return;
label_11:
      lock (this._docList)
        this._docList.AddRange((IEnumerable<DocRecord>) docArray);
      this.DoAfterGenerateDocList();
      ReportDocumentSyncStrategy documentSyncStrategy = new ReportDocumentSyncStrategy();
      List<int> intList = new List<int>();
      int count;
      lock (this._docList)
        count = this._docList.Count;
      for (int index1 = 0; index1 < count; ++index1)
      {
        if (!this.IsActive)
          return;
        DocRecord docRecord;
        lock (this._docList)
          docRecord = this._docList[index1];
        DocState state = docRecord.state;
        if ((state & DocState.Delayed) != DocState.NoFlags)
          intList.Add(index1);
        else if ((state & (DocState.CondFalse | DocState.Empty | DocState.Complect | DocState.GenError)) != DocState.NoFlags)
        {
          this.DoGenerateDocument(docRecord, this._expertSrv.GetTraceInfo(this.ExpertTaskId, docRecord.docNumber));
        }
        else
        {
          bool flag = true;
          while (docRecord.IsDocGenerating())
          {
            if (!flag)
            {
              int num = 0;
              for (int index2 = Environment.TickCount + 3000; num < 2 && this.IsActive && index2 >= Environment.TickCount; ++num)
                Thread.Sleep(300);
            }
            if (!this.IsActive)
              return;
            try
            {
              docRecord = this._expertSrv.GetDocRecord(this.ExpertTaskId, index1);
              state = docRecord.state;
              flag = false;
            }
            catch (Exception ex)
            {
              this.DoExceptionOutput(ex);
              return;
            }
          }
          if (!this.IsActive)
            return;
          byte[] traceInfo;
          try
          {
            traceInfo = this._expertSrv.GetTraceInfo(this.ExpertTaskId, docRecord.docNumber);
          }
          catch (Exception ex)
          {
            this.DoExceptionOutput(ex);
            return;
          }
          if (!this.IsActive)
            return;
          this.DoGenerateDocument(docRecord, traceInfo);
          if ((state & DocState.Ready) != DocState.NoFlags)
          {
            if (!this.IsActive)
              return;
            Task[] prevTasks = taskList.ToArray();
            taskList.Add(Task.Factory.StartNew((Action) (() => this.DocumentRealign(new ReportDocument(documentSyncStrategy.CreateSyncObject(docRecord), docRecord), prevTasks)), TaskCreationOptions.DenyChildAttach).ContinueWith((Action<Task>) (task =>
            {
              if (task.Exception == null)
                return;
              foreach (Exception innerException in task.Exception.Flatten().InnerExceptions)
                this.DoExceptionOutput(innerException);
              if (!this.IsActive)
                return;
              this.DoErrorOutput(LocalizationHolder.rm.GetString("Reports_69"));
              this.Terminate();
            })));
          }
        }
      }
      if (!this.IsActive)
        return;
      for (int index3 = 0; index3 < intList.Count; ++index3)
      {
        if (!this.IsActive)
          return;
        int num1 = intList[index3];
        DocRecord docRecord;
        lock (this._docList)
          docRecord = this._docList[num1];
        DocState state = docRecord.state;
        if ((state & DocState.GenError) != DocState.NoFlags)
        {
          this.DoGenerateDocument(docRecord, this._expertSrv.GetTraceInfo(this.ExpertTaskId, docRecord.docNumber));
        }
        else
        {
          bool flag = true;
          while ((state & DocState.Ready) == DocState.NoFlags && (state & DocState.Delayed) != DocState.NoFlags)
          {
            if (!flag)
            {
              int num2 = 0;
              for (int index4 = Environment.TickCount + 3000; num2 < 2 && this.IsActive && index4 >= Environment.TickCount; ++num2)
                Thread.Sleep(300);
            }
            if (!this.IsActive)
              return;
            docRecord = this._expertSrv.GetDocRecord(this.ExpertTaskId, num1);
            state = docRecord.state;
            flag = false;
            if ((state & (DocState.CondFalse | DocState.GenError)) != DocState.NoFlags)
              break;
          }
          this.DoGenerateDocument(docRecord, this._expertSrv.GetTraceInfo(this.ExpertTaskId, docRecord.docNumber));
          if ((state & DocState.Ready) != DocState.NoFlags)
          {
            Task[] prevTasks = taskList.ToArray();
            taskList.Add(Task.Factory.StartNew((Action) (() => this.DocumentRealign(new ReportDocument(documentSyncStrategy.CreateSyncObject(docRecord), docRecord), prevTasks)), TaskCreationOptions.DenyChildAttach).ContinueWith((Action<Task>) (task =>
            {
              if (task.Exception == null)
                return;
              foreach (Exception innerException in task.Exception.Flatten().InnerExceptions)
                this.DoExceptionOutput(innerException);
              if (!this.IsActive)
                return;
              this.DoErrorOutput(LocalizationHolder.rm.GetString("Reports_69"));
              this.Terminate();
            })));
          }
        }
      }
      Task.WaitAll(taskList.ToArray());
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case ThreadAbortException _:
          break;
        case ThreadInterruptedException _:
          break;
        case AggregateException aggregateException:
          using (IEnumerator<Exception> enumerator = aggregateException.Flatten().InnerExceptions.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.DoExceptionOutput(enumerator.Current);
            break;
          }
        default:
          this.DoExceptionOutput(ex);
          break;
      }
    }
  }

  /// <summary>Основная процедура "живого" потока....</summary>
  private void AliveThreadProc()
  {
    try
    {
      while (this.IsActive)
      {
        this._expertSrv?.IAmAlive(this.ExpertTaskId);
        int num1 = 0;
        int num2 = Environment.TickCount + 1500;
        while (true)
        {
          if (num1 < 3 && this.IsActive && num2 >= Environment.TickCount)
          {
            Thread.Sleep(500);
            ++num1;
          }
          else
            goto label_6;
        }
label_6:;
      }
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case ThreadAbortException _:
          break;
        case ThreadInterruptedException _:
          break;
        default:
          this.DoExceptionOutput(ex);
          break;
      }
    }
  }

  /// <summary>Конструктор</summary>
  /// <param name="objectId">идентификатор объекта (для которого формировать комплект)</param>
  /// <param name="scriptObjId">идентификатор скрипта ЭС (по которому формировать комплект)</param>
  public ReportsBaseTask(IReportTaskParams taskParams)
  {
    this.Params = taskParams ?? throw new ArgumentNullException(nameof (taskParams));
    this._docList = new List<DocRecord>();
    this._taskThread = new Thread(new ThreadStart(this.TaskThreadProc))
    {
      IsBackground = true
    };
    this._aliveThread = new Thread(new ThreadStart(this.AliveThreadProc))
    {
      IsBackground = true
    };
  }

  /// <summary>
  /// 
  /// </summary>
  public void Terminate()
  {
    this.State = ReportTaskState.Terminated;
    this._expertSrv?.AbortProcess(this.ExpertTaskId);
  }

  /// <summary>Статус задачи</summary>
  public ReportTaskState State { get; private set; }

  /// <summary>Опции  задачи</summary>
  public ReportTaskOptions Options { get; set; }

  /// <summary>Параметры задачи генерации</summary>
  public IReportTaskParams Params { get; }

  /// <summary>Список документов / комплектов задачи</summary>
  /// <remarks></remarks>
  public DocRecord[] DocList
  {
    get
    {
      DocRecord[] docList = (DocRecord[]) null;
      if (this._docList != null)
      {
        lock (this._docList)
          docList = this._docList.ToArray();
      }
      return docList;
    }
  }

  /// <summary>Выполнение задачи</summary>
  /// <param name="changeLog">Список изменений</param>
  /// <returns>Код выполнения</returns>
  public virtual ExpertResult Execute(out List<ChangeInfo> changeLog)
  {
    return this.Execute(false, false, ExpertTraceFlags.None, out changeLog, out byte[] _, out string[] _);
  }

  /// <summary>Выполнение</summary>
  /// <param name="changeLog">Список изменений</param>
  /// <returns>Код выполнения</returns>
  /// <param name="traceFlags">Режим трассировки</param>
  /// <param name="traceInfo">Запакованный XML с отладочной информацией</param>
  /// <param name="reportInfo">Доп. информация о ходе выполнения задачи</param>
  public virtual ExpertResult Execute(
    bool traceEnable,
    bool logEnable,
    ExpertTraceFlags traceFlags,
    out List<ChangeInfo> changeLog,
    out byte[] traceInfo,
    out string[] reportData)
  {
    ReportTraceInfo reportTraceInfo;
    int num = (int) this.Execute(traceEnable, logEnable, traceFlags, out reportTraceInfo);
    changeLog = reportTraceInfo?.ChangeLog;
    traceInfo = reportTraceInfo?.TraceInfo;
    reportData = reportTraceInfo?.ReportInfo;
    return (ExpertResult) num;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="traceEnable"></param>
  /// <param name="logEnable"></param>
  /// <param name="traceFlags"></param>
  /// <param name="reportTraceInfo"></param>
  /// <returns></returns>
  public ExpertResult Execute(
    bool traceEnable,
    bool logEnable,
    ExpertTraceFlags traceFlags,
    out ReportTraceInfo reportTraceInfo)
  {
    reportTraceInfo = (ReportTraceInfo) null;
    List<ChangeInfo> changeLog = (List<ChangeInfo>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      ExpertResult expertResult1 = this.ValidateParams();
      if (expertResult1 != ExpertResult.OK)
        return expertResult1;
      ExpertResult services = this.GetServices(session);
      if (services != ExpertResult.OK)
        return services;
      session.StartLogHistory();
      try
      {
        this.ExpertTaskId = this._expertSrv.StartTask(session.SessionGUID);
        this._expertSrv.SetDateTimeFormat(this.ExpertTaskId, Thread.CurrentThread.CurrentCulture.DateTimeFormat);
        this._expertSrv.SetNumberFormat(this.ExpertTaskId, Thread.CurrentThread.CurrentCulture.NumberFormat);
        AttributeValues[] attributes = this.Params.Attributes;
        if (attributes != null && attributes.Length != 0)
        {
          Dictionary<CalcAttrPair, CalculatedAttr> parms = new Dictionary<CalcAttrPair, CalculatedAttr>();
          foreach (AttributeValues attributeValues in attributes)
          {
            object[] values = attributeValues.Values;
            CalcAttrPair calcAttrPair = new CalcAttrPair(-1L, attributeValues.AttributeID);
            CalculatedAttr calculatedAttr = new CalculatedAttr(calcAttrPair, values.Length > 1 ? (object) values : values[0]);
            parms[calcAttrPair] = calculatedAttr;
          }
          if (parms.Count > 0)
            this._expertSrv.SetCalcParms(this.ExpertTaskId, parms);
        }
        this.State = ReportTaskState.Executing;
        this._expertSrv.SetTrace(this.ExpertTaskId, traceEnable);
        this._expertSrv.SetTraceFlags(this.ExpertTaskId, traceFlags);
        this._expertSrv.SetLog(this.ExpertTaskId, logEnable);
        if (this.Params.ArchiveId != 0L)
          this._expertSrv.SetTaskParm(this.ExpertTaskId, "ArchiveID", (object) this.Params.ArchiveId);
        if (this._taskThread != null && (this._taskThread.ThreadState & (ThreadState.Unstarted | ThreadState.Stopped)) != ThreadState.Running)
          this._taskThread.Start();
        if (this._aliveThread != null)
        {
          this._expertSrv.SetTimeInterval(this.ExpertTaskId, new TimeSpan(0, 0, 0, 0, 10000));
          if ((this._aliveThread.ThreadState & (ThreadState.Unstarted | ThreadState.Stopped)) != ThreadState.Running)
            this._aliveThread.Start();
        }
        ExpertResult expertResult2 = this.ExecuteInternal(out changeLog);
        if (this._taskThread != null && this._taskThread.IsAlive)
        {
          ThreadState threadState = this._taskThread.ThreadState;
          if ((threadState & ThreadState.WaitSleepJoin) != ThreadState.Running)
          {
            Thread.Sleep(300);
            this._taskThread.Join();
          }
          else if ((threadState & ThreadState.Background) != ThreadState.Running)
            this._taskThread.Join();
        }
        this.SaveTaskParams();
        this.DoAfterGenerateComplete();
        return expertResult2;
      }
      finally
      {
        byte[] traceInfo = (byte[]) null;
        HybridTableExp objectGlobalTable = (HybridTableExp) null;
        HybridTableExp linkGlobalTable = (HybridTableExp) null;
        if (traceFlags != ExpertTraceFlags.None)
        {
          traceInfo = this._expertSrv.GetTraceInfo(this.ExpertTaskId);
          if (traceFlags.HasFlag((Enum) ExpertTraceFlags.ShowGlobalTables))
          {
            objectGlobalTable = this._expertSrv.GetGlobalObjectsTable(this.ExpertTaskId);
            linkGlobalTable = this._expertSrv.GetGlobalLinksTable(this.ExpertTaskId);
          }
        }
        string[] array = this._expertSrv.GetUserReport(this.ExpertTaskId)?.ToArray();
        session.StopLogHistory();
        reportTraceInfo = new ReportTraceInfo(changeLog, traceInfo, array, objectGlobalTable, linkGlobalTable, (IList<CategoryValue>) session.GetModificationsHistoryList());
        if (this.State != ReportTaskState.Terminated)
          this.State = ReportTaskState.Completed;
        this._expertSrv.EndTask(this.ExpertTaskId);
      }
    }
  }

  /// <summary>Ид. задачи ЭС</summary>
  internal int ExpertTaskId { get; private set; } = -1;

  /// <summary>Признак выполнения задачи</summary>
  internal bool IsActive => this.State == ReportTaskState.Executing;

  /// <summary>
  /// Получение списка генерируемых документов / комплектов от ЭС
  /// </summary>
  public event ReportDocEventHandler AfterGenerateDocList;

  /// <summary>Генерация документа</summary>
  public event ReportDocEventHandler GenerateDocument;

  /// <summary>Окончание разбиение документа</summary>
  public event ReportDocEventHandler AfterRealignDocument;

  /// <summary>
  /// Custom разбиение документа (для вызова в основном потоке приложения)
  /// </summary>
  public event ReportDocEventHandler CustomRealignDocument;

  /// <summary>Вывод сообщений об ошибках</summary>
  public event ValueObjChangedHandler ErrorOutput;

  /// <summary>Завершение генерации комплекта</summary>
  public event ReportDocEventHandler AfterGenerateComplete;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="zipScr"></param>
  /// <param name="updateDoc"></param>
  /// <returns></returns>
  /// <remarks>Вызов метода должен производиться из основного потока приложения</remarks>
  public static ImDocument UnpackImDocument(byte[] zipScr, bool updateDoc)
  {
    if (zipScr == null)
      return (ImDocument) null;
    using (Stream baseInputStream = (Stream) new MemoryStream(zipScr))
    {
      using (InflaterInputStream inflaterInputStream = new InflaterInputStream(baseInputStream))
        return ImDocument.LoadFromXml((Stream) inflaterInputStream, updateDoc, true, false);
    }
  }

  /// <summary>Запаковка документа</summary>
  /// <param name="imDoc"></param>
  /// <returns></returns>
  public static byte[] PackImDocument(ImDocument imDoc)
  {
    return ImDocumentDataUtils.PackImDocument((ImDocumentData) imDoc);
  }
}
