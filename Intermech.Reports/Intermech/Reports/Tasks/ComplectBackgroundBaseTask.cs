// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.ComplectBackgroundBaseTask
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Client.Core;
using Intermech.Docking;
using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Expert.User;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Client.NotificationService;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Reports;
using Intermech.IO;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Search.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>Базовый класс фоновых задач</summary>
internal abstract class ComplectBackgroundBaseTask : 
  ReportBackgroundBaseTask,
  IReportBackgroundTask,
  IBackgroundTask
{
  /// <summary>Максимальное число одновременно выполняющихся задач</summary>
  private static readonly int MaxActiveTaskCount = 20;
  /// <summary>Текущее количество выполняемых задач</summary>
  private static long _activeTaskCount;
  /// <summary>
  /// 
  /// </summary>
  private static readonly int InvokeTimeOut = 10000;
  /// <summary>Описание объекта с ид. версии = _objectID</summary>
  private QuickObjectInfo _objectInfo;
  /// <summary>Флаг необходимости открытия окна документов</summary>
  private bool _needOpenDocumentWindow;
  /// <summary>Текущая итерация задачи генерации документа</summary>
  private int _iterationNo;
  /// <summary>
  /// Кэш элементов (узлов) комплекта
  /// Key   - индекс документа в списке ЭС
  /// Value - элемент
  /// </summary>
  private readonly IDictionary<int, ComplectBackgroundBaseTask.ComplectNode> _docNodesCache = (IDictionary<int, ComplectBackgroundBaseTask.ComplectNode>) new ConcurrentDictionary<int, ComplectBackgroundBaseTask.ComplectNode>();
  /// <summary>Список комплектов документов</summary>
  private IList<DocumentsComplect> _complectList;
  /// <summary>Информация о трассировке</summary>
  private RepDocTraceInfo _docTraceInfo;
  /// <summary>Окно просмотра комплекта документов</summary>
  private ImDocumentEditorForm _docEditForm;
  /// <summary>Признак закрытия окна документов</summary>
  private bool _docWindowClosed;
  /// <summary>
  /// Окно навигатора, из которого была вызвана текущая задача
  /// (На тек. момент требуется для отправки уведомлений)
  /// </summary>
  private NavWindow _nawWindow;
  /// <summary>Класс - генератор комплекта</summary>
  protected ReportsBaseTask _reportsTask;

  /// <summary>Инициализация параметров класса</summary>
  protected override void InitializeData()
  {
    base.InitializeData();
    this._complectList = (IList<DocumentsComplect>) new List<DocumentsComplect>();
    DockManager service = ServiceUtils.GetService<DockManager>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    this._nawWindow = service.ActiveDockControl as NavWindow;
  }

  /// <summary>Инициализация параметров</summary>
  private void InitializeTaskData()
  {
    if (this._reportsTask == null)
      return;
    this._reportsTask.AfterGenerateDocList += new ReportDocEventHandler(this._generator_AfterGenerateDocList);
    this._reportsTask.GenerateDocument += new ReportDocEventHandler(this._generator_GenerateDocument);
    this._reportsTask.CustomRealignDocument += new ReportDocEventHandler(this._generator_CustomRealignDocument);
    this._reportsTask.AfterRealignDocument += new ReportDocEventHandler(this._generator_AfterRealignDocument);
    this._reportsTask.AfterGenerateComplete += new ReportDocEventHandler(this._generator_AfterGenerateComplete);
    this._reportsTask.ErrorOutput += new ValueObjChangedHandler(this._generator_ErrorOutput);
  }

  /// <summary>Создание задачи</summary>
  protected abstract void CreateReportTask();

  /// <summary>
  /// 
  /// </summary>
  protected override void CustomThreadProc()
  {
  }

  /// <summary>Основная процедура потока....</summary>
  protected override void ThreadProc()
  {
    bool flag = false;
    try
    {
      this._event?.WaitOne();
      if (ComplectBackgroundBaseTask.MaxActiveTaskCount > 0)
      {
        while (Interlocked.Read(ref ComplectBackgroundBaseTask._activeTaskCount) >= (long) ComplectBackgroundBaseTask.MaxActiveTaskCount)
          Thread.Sleep(100);
      }
      Interlocked.Increment(ref ComplectBackgroundBaseTask._activeTaskCount);
      IExpertUser service = ServiceUtils.GetService<IExpertUser>((object) ApplicationServices.Container, true);
      this._needOpenDocumentWindow = !this.Options.HasFlag((Enum) ReportTaskOptions.HideDocWindow);
      bool traceEnable = service.ShowTraceWindow && !this.Options.HasFlag((Enum) ReportTaskOptions.HideTraceWindow);
      this._docTraceInfo = traceEnable ? new RepDocTraceInfo(this._name) : (RepDocTraceInfo) null;
      try
      {
        this._reportsTask.Options = this.Options;
        ReportTraceInfo reportTraceInfo;
        int num = (int) this._reportsTask.Execute(traceEnable, service.ReportLog, ExpertTask.GetConfTraceFlags(), out reportTraceInfo);
        Interlocked.Decrement(ref ComplectBackgroundBaseTask._activeTaskCount);
        if (num == 1)
          this.DoFireNotification(reportTraceInfo);
        if (reportTraceInfo?.ReportInfo != null && reportTraceInfo.ReportInfo.Length != 0)
          this.WriteOutput(string.Join(Environment.NewLine, reportTraceInfo.ReportInfo));
        flag = true;
        this.OnChanged(BackgroundTaskChangedType.Dispose);
        if (traceEnable)
        {
          if (this._docTraceInfo != null)
          {
            this._docTraceInfo.CopyFrom(reportTraceInfo);
            this.DocTraceInfoExecute((object) this._docTraceInfo, (object) reportTraceInfo);
          }
        }
      }
      finally
      {
        if (this._docEditForm != null)
          this._docEditForm.Closed -= new EventHandler(this._docForm_close);
      }
    }
    catch (Exception ex)
    {
      Interlocked.Decrement(ref ComplectBackgroundBaseTask._activeTaskCount);
      switch (ex)
      {
        case ThreadAbortException _:
        case ThreadInterruptedException _:
          break;
        default:
          this.SetState(BackgroundTaskState.Error);
          this.SetThrow(ex);
          break;
      }
    }
    if (flag)
      return;
    Thread.Sleep(100);
    this.OnChanged(BackgroundTaskChangedType.Dispose);
  }

  /// <summary>Рассылка уведомлений</summary>
  /// <param name="reportTraceInfo"></param>
  private void DoFireNotification(ReportTraceInfo reportTraceInfo)
  {
    if (reportTraceInfo == null || this.Options.HasFlag((Enum) ReportTaskOptions.HideDocWindow))
      return;
    List<ChangeInfo> changeLog = reportTraceInfo.ChangeLog;
    if (changeLog == null || changeLog.Count == 0)
      return;
    INotificationQueue notificationQueue = reportTraceInfo.ModificationLog != null ? NotificationHelper.GetQueue(reportTraceInfo.ModificationLog) : (INotificationQueue) new NotificationQueue();
    List<ChangeRelInfo> source1 = new List<ChangeRelInfo>();
    List<ChangeRelInfo> source2 = new List<ChangeRelInfo>();
    List<ChangeObjInfo> source3 = new List<ChangeObjInfo>();
    List<ChangeObjInfo> changeObjInfoList = new List<ChangeObjInfo>();
    foreach (ChangeInfo changeInfo in changeLog)
    {
      switch (changeInfo.ElemType)
      {
        case AttributableElements.Object:
          if (changeInfo.ElemType == AttributableElements.Object && changeInfo is ChangeObjInfo changeObjInfo)
          {
            switch (changeInfo.OperType)
            {
              case DocOperType.Created:
                source3.Add(changeObjInfo);
                continue;
              case DocOperType.Changed:
                changeObjInfoList.Add(changeObjInfo);
                continue;
              default:
                continue;
            }
          }
          else
            continue;
        case AttributableElements.Relation:
          if (changeInfo.ElemType == AttributableElements.Relation && changeInfo is ChangeRelInfo changeRelInfo)
          {
            switch (changeInfo.OperType)
            {
              case DocOperType.Created:
                source1.Add(changeRelInfo);
                continue;
              case DocOperType.Deleted:
                source2.Add(changeRelInfo);
                continue;
              default:
                continue;
            }
          }
          else
            continue;
        default:
          continue;
      }
    }
    if (source1.Count != 0 || source2.Count != 0)
    {
      if (source1.Count != 0)
        notificationQueue.QueueEvent((NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) source1.Select<ChangeRelInfo, long>((Func<ChangeRelInfo, long>) (item => item.ID)).ToList<long>(), (IList<long>) source1.Select<ChangeRelInfo, long>((Func<ChangeRelInfo, long>) (item => item.ProjID)).ToList<long>(), (IList<int>) null, (IList<int>) source1.Select<ChangeRelInfo, int>((Func<ChangeRelInfo, int>) (item => item.TypeID)).ToList<int>()));
      if (source2.Count != 0)
        notificationQueue.QueueEvent((NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", (IList<long>) source2.Select<ChangeRelInfo, long>((Func<ChangeRelInfo, long>) (item => item.ID)).ToList<long>(), (IList<long>) source2.Select<ChangeRelInfo, long>((Func<ChangeRelInfo, long>) (item => item.ProjID)).ToList<long>(), (IList<int>) null, (IList<int>) source2.Select<ChangeRelInfo, int>((Func<ChangeRelInfo, int>) (item => item.TypeID)).ToList<int>()));
    }
    if (source3.Count != 0)
      notificationQueue.QueueEvent((NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", (IList<long>) source3.Select<ChangeObjInfo, long>((Func<ChangeObjInfo, long>) (item => item.ID)).ToList<long>(), (IList<int>) source3.Select<ChangeObjInfo, int>((Func<ChangeObjInfo, int>) (item => item.TypeID)).ToList<int>()));
    IInvokeService service = ServiceUtils.GetService<IInvokeService>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    foreach (NotificationEventArgs notificationEventArgs in notificationQueue.ToEnumerable())
    {
      try
      {
        NotificationEventArgs @event = notificationEventArgs;
        service.InvokeAction(ComplectBackgroundBaseTask.InvokeTimeOut, (Action) (() => this.DoFireEvent(@event)));
      }
      catch
      {
      }
    }
  }

  /// <summary>Internal fire event</summary>
  /// <param name="args"></param>
  private void DoFireEvent(NotificationEventArgs args)
  {
    bool flag = false;
    INotificationWindowService nawWindow = (INotificationWindowService) this._nawWindow;
    if (nawWindow != null)
      flag = nawWindow.FireEvent((object) this, args);
    if (flag)
      return;
    ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, args);
  }

  /// <summary>Internal fire global event</summary>
  /// <param name="args"></param>
  protected void DoFireGlobalEvent(NotificationEventArgs args)
  {
    ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, args);
  }

  /// <summary>Вывод сообщения об ошибке при генерации док-та</summary>
  /// <param name="docRecord"></param>
  protected virtual void DoDocErrorOutput(DocRecord docRecord)
  {
    if (docRecord == null || docRecord.errorMsg == string.Empty)
      return;
    this.WriteOutput(string.Format(LocalizationHolder.rm.GetString("Reports_37"), (object) docRecord.docName, (object) docRecord.objID) + docRecord.errorMsg);
  }

  /// <summary>Отображение результатов трассировки</summary>
  /// <param name="traceData"></param>
  private void DocTraceInfoExecute(params object[] traceData)
  {
    if (traceData == null || traceData.Length == 0 || !(traceData[0] is RepDocTraceInfo repDocTraceInfo))
      return;
    if (this.InvokeRequired)
    {
      this.BeginInvoke((Delegate) (parameters => this.DocTraceInfoExecute(parameters as object[])), (object) traceData);
    }
    else
    {
      ReportTraceInfo reportTraceInfo = traceData.Length > 1 ? traceData[1] as ReportTraceInfo : (ReportTraceInfo) null;
      ShowDoc showDoc = new ShowDoc()
      {
        DocItem = (DocTraceInfo) repDocTraceInfo
      };
      if (reportTraceInfo != null)
      {
        showDoc.ObjectsGlobalTable = reportTraceInfo.ObjectGlobalTable;
        showDoc.RelationsGlobalTable = reportTraceInfo.LinkGlobalTable;
      }
      showDoc.TopMost = true;
      showDoc.Show();
    }
  }

  /// <summary>Получение комплекта по ид.-ру</summary>
  /// <param name="docIndex"></param>
  /// <returns></returns>
  private ComplectBackgroundBaseTask.ComplectNode GetDocumentNode(int docIndex)
  {
    ComplectBackgroundBaseTask.ComplectNode documentNode;
    if (this._docNodesCache.TryGetValue(docIndex, out documentNode) || docIndex != -1)
      return documentNode;
    DocumentsComplect documentsComplect = new DocumentsComplect();
    documentsComplect.Name = this.Name;
    DocumentsComplect node = documentsComplect;
    documentNode = new ComplectBackgroundBaseTask.ComplectNode((VisualNode) node);
    this._docNodesCache.Add(docIndex, documentNode);
    this._complectList.Add(node);
    return documentNode;
  }

  /// <summary>Добавление элемента в комплекта</summary>
  /// <param name="docRecord"></param>
  private void InsertComplectItem(DocRecord docRecord)
  {
    if (docRecord == null || (docRecord.state & (DocState.CondFalse | DocState.Empty | DocState.GenError)) != DocState.NoFlags)
      return;
    if ((docRecord.state & DocState.Complect) == DocState.NoFlags && (docRecord.state & (DocState.Ready | DocState.Aligned | DocState.Delayed)) != DocState.NoFlags)
    {
      int docNumber = docRecord.docNumber;
      int parentIndex = docRecord.parentIndex;
      if (this.GetDocumentNode(parentIndex) != null)
        return;
      this.SetThrow(new Exception(string.Format(LocalizationHolder.rm.GetString("Reports_42"), (object) docRecord.docName, (object) docNumber, (object) parentIndex)));
    }
    else
    {
      if ((docRecord.state & DocState.Complect) == DocState.NoFlags)
        return;
      int docNumber = docRecord.docNumber;
      int parentIndex = docRecord.parentIndex;
      ComplectBackgroundBaseTask.ComplectNode documentNode = this.GetDocumentNode(parentIndex);
      if (documentNode == null)
      {
        if (parentIndex != -1)
        {
          this.SetThrow(new Exception(string.Format(LocalizationHolder.rm.GetString("Reports_42"), (object) docRecord.docName, (object) docNumber, (object) parentIndex)));
          return;
        }
        if ((docRecord.state & DocState.Complect) != DocState.NoFlags)
        {
          DocumentsComplect documentsComplect = new DocumentsComplect();
          documentsComplect.Name = docRecord.docName;
          DocumentsComplect node = documentsComplect;
          this._docNodesCache.Add(docNumber, new ComplectBackgroundBaseTask.ComplectNode((VisualNode) node));
          this._complectList.Add(node);
        }
      }
      if (documentNode == null)
        return;
      VisualNode visualNode = (VisualNode) null;
      if ((docRecord.state & DocState.Complect) != DocState.NoFlags)
      {
        DocumentsComplect documentsComplect = new DocumentsComplect();
        documentsComplect.Name = docRecord.docName;
        visualNode = (VisualNode) documentsComplect;
      }
      if (visualNode == null)
        return;
      this._docNodesCache.Add(docNumber, new ComplectBackgroundBaseTask.ComplectNode(visualNode));
      lock (documentNode)
      {
        documentNode.ChildNodeIdxList.Add(docNumber);
        if (this.Options.HasFlag((Enum) ReportTaskOptions.HideDocWindow) || this._docWindowClosed || documentNode.Node.Nodes == null)
          return;
        int childPos = -1;
        try
        {
          this.InsertDocChildNode(documentNode.Node, visualNode, childPos);
        }
        catch (Exception ex)
        {
          this.SetThrow(ex);
        }
      }
    }
  }

  /// <summary>Добавление realigned документа в шаблон</summary>
  /// <param name="docRecord"></param>
  /// <param name="documentData"></param>
  private void InsertDocumentItem(DocRecord docRecord, ImDocumentData documentData)
  {
    if (docRecord == null || documentData == null)
      return;
    int docNumber = docRecord.docNumber;
    int parentIndex = docRecord.parentIndex;
    ComplectBackgroundBaseTask.ComplectNode documentNode = this.GetDocumentNode(parentIndex);
    if (documentNode == null)
      this.SetThrow(new Exception(string.Format(LocalizationHolder.rm.GetString("Reports_42"), (object) docRecord.docName, (object) docNumber, (object) parentIndex)));
    else if ((docRecord.state & (DocState.CondFalse | DocState.Empty | DocState.Complect | DocState.GenError)) != DocState.NoFlags)
    {
      lock (documentNode)
        documentNode.ChildNodeIdxList.Remove(docNumber);
    }
    else
    {
      if ((docRecord.state & (DocState.Ready | DocState.Aligned | DocState.Delayed)) == DocState.NoFlags)
        return;
      this._docNodesCache.Add(docNumber, new ComplectBackgroundBaseTask.ComplectNode((VisualNode) documentData));
      lock (documentNode)
      {
        if (!documentNode.ChildNodeIdxList.Contains(docNumber))
        {
          documentNode.ChildNodeIdxList.Add(docNumber);
          documentNode.ChildNodeIdxList.Sort();
        }
        int childPos = documentNode.ChildNodeIdxList.IndexOf(docNumber);
        if (!this.Options.HasFlag((Enum) ReportTaskOptions.HideDocWindow))
        {
          if (!this._docWindowClosed)
          {
            try
            {
              this.InsertDocChildNode(documentNode.Node, (VisualNode) documentData, childPos);
            }
            catch (Exception ex)
            {
              this.SetThrow(ex);
              return;
            }
          }
        }
      }
      if (this._complectList.Count == 0 || !this._needOpenDocumentWindow)
        return;
      this._needOpenDocumentWindow = false;
      try
      {
        this.OpenDocumentWindow(this._complectList[0]);
      }
      catch (Exception ex)
      {
        this.SetThrow(ex);
      }
    }
  }

  /// <summary>Вставка документа / комплекта в родительский узел</summary>
  /// <param name="projNode">Родительский элемент</param>
  /// <param name="childNode">Вставляемый элемент</param>
  /// <param name="childPos">Позиция для вставки</param>
  private void InsertDocChildNode(VisualNode projNode, VisualNode childNode, int childPos)
  {
    if (this.InvokeRequired)
    {
      this.BeginInvoke((Delegate) new ComplectBackgroundBaseTask.InsertDocChildNodeDelegate(this.InsertDocChildNode), (object) projNode, (object) childNode, (object) childPos);
    }
    else
    {
      if (projNode?.Nodes == null || childNode == null)
        return;
      if (childPos == -1)
        childPos = projNode.Nodes.Count;
      projNode.InsertChildNode(childPos, (DocumentTreeNode) childNode, false, true, true, false, false);
    }
  }

  /// <summary>Открытие окна документов</summary>
  /// <param name="complect"></param>
  protected void OpenDocumentWindow(DocumentsComplect complect)
  {
    if (this.Options.HasFlag((Enum) ReportTaskOptions.HideDocWindow))
      return;
    if (this.InvokeRequired)
    {
      this.BeginInvoke((Delegate) new ComplectBackgroundBaseTask.OpenDocumentDelegate(this.OpenDocumentWindow), (object) complect);
    }
    else
    {
      this._docEditForm = DocumentEditorPlugin.Instance.OpenDocument((DocumentTreeNode) complect, true, true);
      this._docEditForm.Closed += new EventHandler(this._docForm_close);
      this._docEditForm.Text = complect.Name;
    }
  }

  /// <summary>Обновление комплекта после его генерации</summary>
  /// <param name="complect"></param>
  protected void UpdateComplect(DocumentsComplect complect)
  {
    if (complect == null)
      return;
    if (this.InvokeRequired)
      this.Invoke((Delegate) new ComplectBackgroundBaseTask.OpenDocumentDelegate(this.UpdateComplect), (object) complect);
    else
      DocumentEditorPlugin.Instance.UpdateDocumentLinks((DocumentTreeNode) complect, false, true, false, true, false);
  }

  /// <summary>Конструктор</summary>
  /// <param name="paramObj">Параметры</param>
  protected ComplectBackgroundBaseTask(IReportTaskParams taskParams)
  {
    this.Params = taskParams ?? throw new ArgumentNullException(nameof (taskParams));
    this.InitializeData();
    this.CreateReportTask();
    this.InitializeTaskData();
  }

  /// <summary>Инициализация / запуск задачи</summary>
  public virtual void Execute()
  {
    using (FixEditingContext fixEditingContext = new FixEditingContext())
    {
      this._thread = new Thread(fixEditingContext.SendEditingContextToThread(new ThreadStart(((CustomThreadBackgroundTask) this).ThreadProc)))
      {
        IsBackground = true
      };
      this.Start();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public IReportTaskParams Params { get; }

  /// <summary>Опции  задачи</summary>
  public ReportTaskOptions Options { get; set; }

  /// <summary>Задача генерации ведомостей</summary>
  public IReportsBaseTask Task => (IReportsBaseTask) this._reportsTask;

  /// <summary>Описание объекта с ид. версии = _objectID</summary>
  public QuickObjectInfo ObjectInfo
  {
    get
    {
      if (this.Params != null && this.Params.ObjectId != this._objectInfo.ObjectID)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          this._objectInfo = sessionKeeper.Session.GetObjectInfo(this.Params.ObjectId);
      }
      return this._objectInfo;
    }
    set => this._objectInfo = value;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _generator_AfterGenerateDocList(object sender, ReportDocBaseEvent e)
  {
    if (!(sender is IReportsBaseTask reportsBaseTask))
      return;
    DocRecord[] docList = reportsBaseTask.DocList;
    this._iterationNo = 0;
    this.MinimumValue = 0;
    this.MaximumValue = 2 + (docList != null ? docList.Length : 0);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _generator_GenerateDocument(object sender, ReportDocBaseEvent e)
  {
    if (!(sender is IReportsBaseTask))
      return;
    this.SetValue((object) ++this._iterationNo);
    DocRecord documentRecord = e?.DocumentRecord;
    this.DoDocErrorOutput(documentRecord);
    if (this._docTraceInfo != null && documentRecord != null)
    {
      RepDocTraceInfo traceInfo1 = this._docTraceInfo.GetTraceInfo(documentRecord.parentIndex);
      RepDocTraceInfo repDocTraceInfo = new RepDocTraceInfo(documentRecord, (byte[]) null);
      if (traceInfo1 != null)
        traceInfo1.AddChildItem((DocTraceInfo) repDocTraceInfo);
      else
        this._docTraceInfo.AddChildItem((DocTraceInfo) repDocTraceInfo);
      if (e is ReportDocEvent reportDocEvent)
      {
        byte[] traceInfo2 = reportDocEvent.TraceInfo;
        if (traceInfo2 != null)
          repDocTraceInfo.TraceData = traceInfo2;
      }
    }
    this.InsertComplectItem(documentRecord);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _generator_CustomRealignDocument(object sender, ReportDocBaseEvent e)
  {
    if (!(sender is IReportsBaseTask) || (e is ReportDocEvent reportDocEvent ? reportDocEvent.DocumentData : (ImDocumentData) null) == null)
      return;
    if (this.InvokeRequired)
      this.Invoke((Delegate) new ReportDocEventHandler(this._generator_CustomRealignDocument), sender, (object) e);
    else
      reportDocEvent.DocumentData.UpdateLayout(true, false);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _generator_AfterRealignDocument(object sender, ReportDocBaseEvent e)
  {
    if (!(sender is IReportsBaseTask) || !(e is ReportDocEvent reportDocEvent))
      return;
    this.DoDocErrorOutput(reportDocEvent.DocumentRecord);
    if (this._docTraceInfo != null && reportDocEvent.DocumentRecord != null)
    {
      RepDocTraceInfo traceInfo = this._docTraceInfo.GetTraceInfo(reportDocEvent.DocumentRecord.docNumber);
      if (traceInfo != null)
      {
        using (Stream stream = (Stream) new ImChunkedStream())
        {
          reportDocEvent.DocumentData.SaveToXml(stream);
          stream.Position = 0L;
          traceInfo.Doc = ImDocument.LoadFromStream(stream, false, false, false);
        }
      }
    }
    this.InsertDocumentItem(reportDocEvent.DocumentRecord, reportDocEvent.DocumentData);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="value"></param>
  private void _generator_ErrorOutput(object sender, object value)
  {
    if (value == null)
      return;
    this.WriteOutput(value.ToString());
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _generator_AfterGenerateComplete(object sender, ReportDocBaseEvent e)
  {
    if (!(sender is IReportsBaseTask))
      return;
    this.SetValue((object) ++this._iterationNo);
    if (this._complectList.Count != 0)
    {
      DocumentsComplect complect = this._complectList[0];
      if (complect != null && !this.Options.HasFlag((Enum) ReportTaskOptions.HideDocWindow))
      {
        if (!this._docWindowClosed)
        {
          try
          {
            this.UpdateComplect(complect);
          }
          catch (Exception ex)
          {
            this.SetThrow(ex);
          }
        }
      }
    }
    this.SetValue((object) ++this._iterationNo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _docForm_close(object sender, EventArgs e)
  {
    if (this.InvokeRequired)
    {
      this.BeginInvoke((Delegate) new EventHandler(this._docForm_close), sender, (object) e);
    }
    else
    {
      this._docWindowClosed = true;
      this._docEditForm = (ImDocumentEditorForm) null;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="text"></param>
  protected override void DoWriteOutput(string text)
  {
    if (this.Options.HasFlag((Enum) ReportTaskOptions.HideOutputWindow))
      return;
    base.DoWriteOutput(text);
  }

  /// <summary>
  /// 
  /// </summary>
  public override void Stop()
  {
    if (!this.CanStop() || MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("Reports_45"), (object) this._name), LocalizationHolder.rm.GetString("Reports_46"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
      return;
    this._reportsTask?.Terminate();
    base.Stop();
  }

  /// <summary>
  /// 
  /// </summary>
  public override void Terminate()
  {
    if (!this.CanTerminate())
      return;
    this._reportsTask?.Terminate();
    base.Terminate();
  }

  /// <summary>Invoke method</summary>
  /// <param name="method"></param>
  /// <param name="args"></param>
  /// <returns></returns>
  public override object Invoke(Delegate method, params object[] args)
  {
    return this._mainThreadControl == null ? (object) null : base.Invoke(method, args);
  }

  /// <summary>Invoke method</summary>
  /// <param name="method"></param>
  /// <param name="args"></param>
  /// <returns></returns>
  public override object BeginInvoke(Delegate method, params object[] args)
  {
    return this._mainThreadControl == null ? (object) null : base.BeginInvoke(method, args);
  }

  /// <summary>Класс - описание элемента комплекта</summary>
  private class ComplectNode
  {
    /// <summary>Конструктор</summary>
    /// <param name="node">Соответствующий элемент комплекта</param>
    public ComplectNode(VisualNode node)
    {
      this.ChildNodeIdxList = new List<int>();
      this.Node = node;
    }

    /// <summary>Соответствующий элемент комплекта</summary>
    public VisualNode Node { get; }

    /// <summary>Список индексов дочерних элементов</summary>
    public List<int> ChildNodeIdxList { get; }
  }

  /// <summary>Делегат для вставки элемента комплекта</summary>
  /// <param name="projNode"></param>
  /// <param name="childNode"></param>
  /// <param name="childPos"></param>
  public delegate void InsertDocChildNodeDelegate(
    VisualNode projNode,
    VisualNode childNode,
    int childPos);

  /// <summary>Делегат для создания окна документов</summary>
  /// <param name="complect"></param>
  public delegate void OpenDocumentDelegate(DocumentsComplect complect);
}
