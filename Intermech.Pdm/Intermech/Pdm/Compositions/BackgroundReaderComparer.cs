// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.BackgroundReaderComparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Threading;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class BackgroundReaderComparer(IServiceProvider services) : 
  BaseBackgroundReader(services),
  ICompareBackgroundReader,
  IBackgroundReader
{
  private List<Tuple<long, int>> _objectIDs;
  private CompareObjectsInfo _info;

  public void Execute(
    object mapping,
    CompareObjectsInfo info,
    List<Tuple<long, int>> objectIDs,
    RuntimeSearchScheme scheme)
  {
    this.Mapping = (RecordMapping) mapping;
    this._objectIDs = objectIDs;
    this.scheme = scheme;
    this._info = info;
    this.thread = new Thread(new ThreadStart(this.StartThread))
    {
      Name = $"Contains_{Guid.NewGuid()}",
      IsBackground = true
    };
    this.thread.Start();
  }

  private void ChangeState(BackgroundState state, int percent, long id)
  {
    this.state = state;
    StateChanged stateChangedEvent = this.StateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent((object) this, new StateChangedEventArgs(state, percent, id));
  }

  private void StartThread()
  {
    IFiltrationService service1 = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    HybridDictionary PluginsData = new HybridDictionary();
    if (ServicesManager.GetService(typeof (IClientPluginsService)) is IClientPluginsService service2)
      service2.GetClientPluginsData(ref PluginsData);
    RelationPair service3 = this.Services != null ? this.Services.GetService(typeof (RelationPair)) as RelationPair : (RelationPair) null;
    if (service3 != null && !service3.Empty)
      PluginsData[(object) "{78D53C74-3CF7-4F48-94FC-80C4FCB0BA77}"] = (object) service3;
    FiltrationHelper.BlockPluginFiltrations(PluginsData, (object) "{2FACA180-73B8-4F24-9928-5623661BBBE6}", (object) "{325F5CDB-8B8E-4B2D-9AA9-5624A0A64D7E}", (object) "{529FFE92-FDA7-48B8-AADF-ADB1EE6EF584}");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICompositionService customService = (ICompositionService) sessionKeeper.Session.GetCustomService(typeof (ICompositionService));
      List<ColumnDescriptor> columns = new List<ColumnDescriptor>();
      List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>();
      foreach (NodeColumnID field in this.Mapping.Fields)
      {
        if (field != null)
        {
          ColumnDescriptor columnDescriptor = new ColumnDescriptor()
          {
            AttributeID = (object) field.AttributeID,
            AttributeSource = field.AttrSource,
            ColumnName = ColumnNameMapping.Index
          };
          int index = this.Mapping.SortFields != null ? Array.IndexOf<object>(this.Mapping.SortFields, (object) field) : -1;
          if (index >= 0)
          {
            columnDescriptor.OrderByID = index;
            columnDescriptor.Sort = (SortOrders) Convert.ToInt32((object) this.Mapping.SortOrders[index]);
          }
          else
          {
            columnDescriptor.Sort = SortOrders.NONE;
            columnDescriptor.OrderByID = -1;
          }
          columns.Add(columnDescriptor);
          if (MetaDataHelper.GetAttributeType(field.AttributeID).FieldType == FieldTypes.ftObjectLink)
            columnDescriptorList.Add(new ColumnDescriptor(columnDescriptor.AttributeID, ColumnContents.ID, ColumnNameMapping.Index, SortOrders.NONE, 0));
        }
      }
      if (columnDescriptorList.Count > 0)
      {
        this._info.AttrIDIndexes = new Dictionary<int, int>(columnDescriptorList.Count);
        for (int index = 0; index < columnDescriptorList.Count; ++index)
        {
          this._info.AttrIDIndexes.Add((int) columnDescriptorList[index].AttributeID, columns.Count);
          columns.Add(columnDescriptorList[index]);
        }
      }
      this.ChangeState(BackgroundState.Reading);
      for (int index = 0; index < this._objectIDs.Count; ++index)
      {
        this.queryResult = customService.Select(sessionKeeper.Session.SessionGUID, this._objectIDs[index].Item1, this.scheme, columns, this.selectGuid, service1.Filtration.OwnerID, PluginsData);
        CompositionInfo info;
        int percent;
        while (true)
        {
          info = customService.GetInfo(this.selectGuid);
          if (info != null)
          {
            percent = (int) Math.Floor((double) info.Percent / (double) this._objectIDs.Count);
            if (!info.ErrorPresent)
            {
              if (info.Percent != 100)
              {
                this.ChangeState(BackgroundState.SetPersent, percent);
                Thread.Sleep(1000);
              }
              else
                goto label_25;
            }
            else
              goto label_23;
          }
          else
            break;
        }
        this.ChangeState(BackgroundState.Empty);
        continue;
label_23:
        this.ChangeState(BackgroundState.Error);
        ExceptionHelper.ExceptionService.ShowException(info.ErrorException);
        continue;
label_25:
        this.queryResult = info.Result as DataTable;
        this.ChangeState(BackgroundState.PartComplete, percent, this._objectIDs[index].Item1);
      }
      this.ChangeState(BackgroundState.Fill);
      this.state = BackgroundState.Empty;
    }
  }
}
