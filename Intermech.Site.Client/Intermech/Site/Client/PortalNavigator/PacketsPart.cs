// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketsPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.DB;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal sealed class PacketsPart : ContainsPart
{
  public PacketsPart(IServiceProvider services, int objTypeID)
    : base(services, objTypeID)
  {
  }

  public PacketsPart(
    IServiceProvider services,
    IConditionsProvider conditionsProvider,
    int objTypeID)
    : base(services, conditionsProvider, objTypeID)
  {
  }

  public override List<object> GetSpecialFields()
  {
    return new List<object>()
    {
      (object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJ_CREATE, AttributeSourceTypes.Object),
      (object) new NodeColumnID((object) PortalConsts.attributeFirstPublishSite, AttributeSourceTypes.Object)
    };
  }

  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    long int64 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object))]);
    int int32 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object))]);
    string str1 = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object))]);
    string str2 = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) PortalConsts.attributeFirstPublishSite, AttributeSourceTypes.Object))]);
    DateTime dateTime = Convert.ToDateTime(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJ_CREATE, AttributeSourceTypes.Object))]);
    long objectID = int64;
    int creatorID = (int) str2[0];
    string caption = str1;
    DateTime createDate = dateTime;
    return (INodeID) new PacketNodeID(int32, objectID, (char) creatorID, caption, createDate);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    PacketNodeID data = nodeID as PacketNodeID;
    if (dataFormat == typeof (IPacketNodeID) && data != null)
      return (object) data;
    if (dataFormat == typeof (IPublishTypedID) && data != null)
      return (object) data;
    if (dataFormat == typeof (INode))
      return (object) this.GetChild(nodeID);
    if (dataFormat == typeof (IDescriptor))
      return (object) new PacketDescriptor(data.TypeID, data.ObjectID, data.CreatorID, data.Caption, data.CreateDate);
    return dataFormat == typeof (ICanOpenInNewWindow) ? (object) new CanOpenInNewWindow() : (object) null;
  }

  public override NodeColumnCollection GetDefaultColumns() => Helper.GetPublishedPacketColumns();

  public override object MapColumnToField(NodeColumn column)
  {
    object field = base.MapColumnToField(column);
    if (field != null)
      return field;
    return column.SchemeGuid == SiteClientConsts.PublishPacketsObligatoryColumnSchemeGuid ? (object) new NodeColumnID(column.ID, AttributeSourceTypes.Object) : (object) null;
  }

  public override INode GetChild(INodeID nodeID)
  {
    return nodeID is PacketNodeID packetNodeId ? (INode) new PacketNode(packetNodeId.TypeID, packetNodeId.ObjectID) : (INode) null;
  }
}
