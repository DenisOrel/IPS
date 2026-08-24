// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.TaskPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.DB;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class TaskPart : ObjectsPart
{
  public static NodeColumnID ncTaskStatus = new NodeColumnID((object) PortalConsts.attributeTaskStatus, AttributeSourceTypes.Object);
  public static NodeColumnID ncTaskType = new NodeColumnID((object) PortalConsts.attributeTaskType, AttributeSourceTypes.Object);
  public static NodeColumnID ncEnabled = new NodeColumnID((object) PortalConsts.attributeTaskTransferEnabled, AttributeSourceTypes.Object);

  public TaskPart(IServiceProvider services)
    : base(services)
  {
  }

  public TaskPart(ConditionStructure condition, IServiceProvider services)
    : base(condition, services)
  {
  }

  public TaskPart(ConditionStructure[] conditions, IServiceProvider services)
    : base(conditions, services)
  {
  }

  public TaskPart(IConditionsProvider conditionsProvider, IServiceProvider services)
    : base(conditionsProvider, services)
  {
  }

  public TaskPart(int objTypeID, IServiceProvider services)
    : base(objTypeID, services)
  {
  }

  public TaskPart(int objTypeID, ConditionStructure condition, IServiceProvider services)
    : base(objTypeID, condition, services)
  {
  }

  public TaskPart(int objTypeID, ConditionStructure[] conditions, IServiceProvider services)
    : base(objTypeID, conditions, services)
  {
  }

  public TaskPart(int objTypeID, IConditionsProvider conditionsProvider, IServiceProvider services)
    : base(objTypeID, conditionsProvider, services)
  {
  }

  public TaskPart(
    int objTypeID,
    ConditionStructure[] conditions,
    IConditionsProvider conditionsProvider,
    IServiceProvider services)
    : base(objTypeID, conditions, conditionsProvider, services)
  {
  }

  public override List<object> GetSpecialFields()
  {
    List<object> specialFields = base.GetSpecialFields();
    specialFields.Add((object) TaskPart.ncTaskStatus);
    specialFields.Add((object) TaskPart.ncTaskType);
    specialFields.Add((object) TaskPart.ncEnabled);
    return specialFields;
  }

  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    TaskStatus int32_1 = (TaskStatus) Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) TaskPart.ncTaskStatus)]);
    TaskType int32_2 = (TaskType) Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) TaskPart.ncTaskType)]);
    bool boolean = Convert.ToBoolean(fieldValues[adapter.GetFieldIndex((object) TaskPart.ncEnabled)]);
    NodeID nodeId = (NodeID) base.CreateNodeId(fieldValues, adapter);
    return (INodeID) new TaskNodeID(new CreateObjectNodeParams(nodeId.ObjectTypeID, nodeId.ObjectID, nodeId.ID, nodeId.CheckedOutBy, nodeId.PrjLinkID, nodeId.LCStepID, nodeId.Caption, nodeId.RelationTypeID, nodeId.Owner, nodeId.Sorting, nodeId.State, nodeId.Version, nodeId.BaseVersion, nodeId.SiteID, nodeId.ProjID, nodeId.RelGuid, nodeId.ModificationID), int32_1, int32_2, boolean);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    if (nodeID == null || !(dataFormat == typeof (IDBTaskID)))
      return base.GetData(nodeID, dataFormat);
    TaskNodeID taskNodeId = nodeID as TaskNodeID;
    return (object) new DBTaskID(taskNodeId.Type, taskNodeId.Status, taskNodeId.Enabled);
  }
}
