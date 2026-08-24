// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompositionReader
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Threading;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompositionReader
{
  private Thread _thread;
  public CompositionReaderChangeStateDelegate CompositionReaderChangeStateEvent;

  public void Start(List<Tuple<long, int>> compareObjectIDs, CompareObjectsInfo info)
  {
    this._thread = new Thread(new ParameterizedThreadStart(this.ThreadMethod));
    this._thread.IsBackground = true;
    this._thread.Name = $"PDM.CompositionReader_{Guid.NewGuid()}";
    this._thread.Start((object) new object[2]
    {
      (object) compareObjectIDs,
      (object) info
    });
  }

  public void Stop()
  {
    if (this._thread == null || this._thread.ThreadState != ThreadState.Running)
      return;
    this._thread.Abort();
    this._thread.Join();
    this._thread = (Thread) null;
  }

  private void StateChanged(BackgroundState state)
  {
    CompositionReaderChangeStateDelegate changeStateEvent = this.CompositionReaderChangeStateEvent;
    if (changeStateEvent == null)
      return;
    changeStateEvent((object) this, new CompositionReaderChangeStateEventArgs(state));
  }

  private void SetError(Exception error)
  {
    CompositionReaderChangeStateDelegate changeStateEvent = this.CompositionReaderChangeStateEvent;
    if (changeStateEvent == null)
      return;
    changeStateEvent((object) this, new CompositionReaderChangeStateEventArgs(error));
  }

  private void ThreadMethod(object args)
  {
    try
    {
      List<Tuple<long, int>> tupleList = (List<Tuple<long, int>>) ((object[]) args)[0];
      CompareObjectsInfo compareObjectsInfo = (CompareObjectsInfo) ((object[]) args)[1];
      if (!compareObjectsInfo.RelationTypes.ContainsValue(true))
        throw new Exception("Необходимо указать хотя бы один тип связей!");
      this.StateChanged(BackgroundState.Reading);
      using (new FixEditingContext())
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          ICompositionService customService = (ICompositionService) sessionKeeper.Session.GetCustomService(typeof (ICompositionService));
          IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
          List<int> intList = new List<int>();
          foreach (KeyValuePair<int, bool> relationType in compareObjectsInfo.RelationTypes)
          {
            if (relationType.Value)
              intList.Add(relationType.Key);
          }
          List<AttributeSource> attributeSourceList = new List<AttributeSource>(compareObjectsInfo.ColumnAttributes.Count);
          List<ColumnDescriptor> columns = new List<ColumnDescriptor>();
          foreach (NodeColumnID columnAttribute in compareObjectsInfo.ColumnAttributes)
          {
            attributeSourceList.Add(new AttributeSource(columnAttribute.AttributeID, Guid.Empty, columnAttribute.AttrSource));
            columns.Add(new ColumnDescriptor((object) columnAttribute.AttributeID, columnAttribute.AttrSource, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
          }
          RuntimeSearchScheme scheme = new RuntimeSearchScheme(compareObjectsInfo.Recursive ? SearchDirection.RecursiveContains : SearchDirection.Contains, 0L, (int[]) null, intList.ToArray(), attributeSourceList.ToArray(), compareObjectsInfo.Recursive ? SearchOptions.ObjectGrouping : SearchOptions.None);
          compareObjectsInfo.Result = new Dictionary<long, DataTable>(tupleList.Count);
          foreach (Tuple<long, int> tuple in tupleList)
          {
            Guid selectGUID = Guid.NewGuid();
            customService.Select(sessionKeeper.Session.SessionGUID, tuple.Item1, scheme, columns, selectGUID, service.FiltrationServiceOwnerID, (HybridDictionary) null);
            CompositionInfo info;
            for (info = customService.GetInfo(selectGUID); info != null && !info.ErrorPresent && info.Percent < 100; info = customService.GetInfo(selectGUID))
              Thread.Sleep(25);
            if (info.ErrorPresent)
              throw info.ErrorException;
            if (info.Result != null)
            {
              DataTable result = (DataTable) info.Result;
              if (result != null && result.Columns.Count > columns.Count)
              {
                for (int index = 0; index < result.Columns.Count - columns.Count; ++index)
                  result.Columns.RemoveAt(result.Columns.Count - 1);
              }
              compareObjectsInfo.Result.Add(tuple.Item1, result);
            }
            else
              compareObjectsInfo.Result.Add(tuple.Item1, (DataTable) null);
          }
        }
      }
      this.StateChanged(BackgroundState.Fill);
    }
    catch (AbortException ex)
    {
      this.StateChanged(BackgroundState.Empty);
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
  }
}
