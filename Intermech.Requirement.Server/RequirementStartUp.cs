// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Server.RequirementStartUp
// Assembly: Intermech.Requirement.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C85D341A-B4CB-4985-9EA3-68BB7F9530D7
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Requirement.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Server;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Requirement.Server;

public class RequirementStartUp : IPackage
{
  private static readonly object[] Columns = new object[1]
  {
    (object) ObligatoryObjectAttributes.F_OBJECT_ID
  };
  private readonly DBRecordSetParams _pars = new DBRecordSetParams((ConditionStructure[]) null, RequirementStartUp.Columns);
  private IEventLogHelper _ehelper;
  public int ObjectTypeId;
  public int TzTypeId;

  public void Load(IServiceProvider serviceProvider)
  {
    if (!(serviceProvider.GetService(typeof (IPluginManager)) is IPluginManager service))
      return;
    service.LoadComplete += new EventHandler(this.pluginManager_LoadComplete);
  }

  public void Unload()
  {
    if (!(ServerServices.GetService(typeof (IEventLogHelper)) is IEventLogHelper service))
      return;
    service.BeforeNextLCStepEvent -= new NextLCStepHandler(this.ehelper_BeforeNextLCStepEvent);
    service.AfterNextLCStepEvent -= new NextLCStepHandler(this.ehelper_AfterNextLCStepEvent);
  }

  public string Name => "Серверная часть модуля управления требованиями";

  private void pluginManager_LoadComplete(object sender, EventArgs e)
  {
    this._ehelper = ServerServices.GetService(typeof (IEventLogHelper)) as IEventLogHelper;
    if (this._ehelper != null)
    {
      this.ObjectTypeId = MetaDataHelper.GetObjectTypeID(ServerConst.TechnicalRequirementGuid);
      this.TzTypeId = MetaDataHelper.GetObjectTypeID(ServerConst.SpecificationGuid);
      this._ehelper.BeforeNextLCStepEvent += new NextLCStepHandler(this.ehelper_BeforeNextLCStepEvent);
      this._ehelper.AfterNextLCStepEvent += new NextLCStepHandler(this.ehelper_AfterNextLCStepEvent);
    }
    if (!(ServerServices.GetService(typeof (IDBObjectService)) is ICreatorContainer service))
      return;
    service.AddCreator((object) ServerConst.TechnicalRequirementGuid, (object) new RequirementObjectsCreator());
  }

  private void ehelper_AfterNextLCStepEvent(
    IDBObject sender,
    IDBLifecycleStep nextstep,
    IUserSession session)
  {
    if (sender.ObjectType != this.ObjectTypeId || session.GetLifecycleStep(new Guid(ServerConst.DeletedLifeGuid)).LCStep == nextstep.LCStep)
      return;
    int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(this.ObjectTypeId);
    DataTable dataTable = session.GetRelationCollection(defaultRelationTypeId).EntersIn(this._pars, sender.ID);
    if (dataTable.Rows.Count <= 0)
      return;
    IDBObject parentObj = session.GetObject(Convert.ToInt64(dataTable.Rows[0].ItemArray[0]));
    if (parentObj.ObjectType == this.TzTypeId)
      return;
    ICompositionLoadService customService = session.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
    List<ColumnDescriptor> columns = new List<ColumnDescriptor>()
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    if (customService == null)
      return;
    if (nextstep.IsFirstStep)
    {
      this.ReplaceParentLCStep(nextstep, parentObj);
    }
    else
    {
      DataTable load = customService.LoadComposition((object) session.SessionGUID, parentObj.ObjectID, defaultRelationTypeId, (IEnumerable<ColumnDescriptor>) columns, "");
      if (load == null)
        return;
      if (load.Rows.Count == 1)
        this.ReplaceParentLCStep(nextstep, parentObj);
      else
        this.CheckObjects(session, load, dataTable, parentObj);
    }
  }

  private void CheckObjects(
    IUserSession session,
    DataTable load,
    DataTable dataTable,
    IDBObject parentObj)
  {
    IDBLifecycleStep lifecycleStep1 = session.GetLifecycleStep(new Guid(ServerConst.NotCompleted));
    IDBLifecycleStep lifecycleStep2 = session.GetLifecycleStep(new Guid(ServerConst.InWork));
    IDBLifecycleStep lifecycleStep3 = session.GetLifecycleStep(new Guid(ServerConst.Completed));
    List<int> intList = new List<int>();
    for (int index = 0; index < load.Rows.Count; ++index)
    {
      IDBObject dbObject = session.GetObject(Convert.ToInt64(load.Rows[index].ItemArray[0]));
      intList.Add(dbObject.LCStep);
    }
    if (intList.Contains(lifecycleStep1.LCStep))
      this.ReplaceParentLCStep(lifecycleStep1, parentObj);
    else if (intList.Contains(lifecycleStep2.LCStep))
    {
      this.ReplaceParentLCStep(lifecycleStep2, parentObj);
    }
    else
    {
      if (!intList.Contains(lifecycleStep3.LCStep))
        return;
      this.ReplaceParentLCStep(lifecycleStep3, parentObj);
    }
  }

  private void ReplaceParentLCStep(IDBLifecycleStep nextstep, IDBObject parentObj)
  {
    this._ehelper.BeforeNextLCStepEvent -= new NextLCStepHandler(this.ehelper_BeforeNextLCStepEvent);
    parentObj.LCStep = nextstep.LCStep;
    this._ehelper.BeforeNextLCStepEvent += new NextLCStepHandler(this.ehelper_BeforeNextLCStepEvent);
  }

  private void ehelper_BeforeNextLCStepEvent(
    IDBObject sender,
    IDBLifecycleStep nextstep,
    IUserSession session)
  {
    if (sender.ObjectType != this.ObjectTypeId || session.GetLifecycleStep(new Guid(ServerConst.DeletedLifeGuid)).LCStep == nextstep.LCStep)
      return;
    int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(this.ObjectTypeId);
    ICompositionLoadService customService = session.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
    List<ColumnDescriptor> columns = new List<ColumnDescriptor>()
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    if (customService == null)
      return;
    DataTable dataTable = customService.LoadComposition((object) session.SessionGUID, sender.ObjectID, defaultRelationTypeId, (IEnumerable<ColumnDescriptor>) columns, "");
    if ((dataTable != null ? (dataTable.Rows.Count > 0 ? 1 : 0) : 0) != 0)
      throw new KernelException("Нельзя изменять шаг ЖЦ данного пункта технических требований, т.к. у него есть подпункты. Шаг ЖЦ изменится автоматически после изменения шагов ЖЦ всех подпунктов.");
  }
}
