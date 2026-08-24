// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.CompositionPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class CompositionPart : ContainsPart
{
  private long _objID;

  public CompositionPart(long objID, int objectType, IServiceProvider services)
    : base(services, objectType)
  {
    this._objID = objID;
  }

  public override INodeQuery GetQuery()
  {
    return this._objID != 0L ? (INodeQuery) new CompositionQuery((INodeQuerySupport) this, this.objTypeID, this._objID, this.Services) : (INodeQuery) null;
  }

  public override NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    NodeColumnCollection supportedColumns = base.GetSupportedColumns(ColumnSetName);
    IColumnSchemes service1 = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    IPortalMetadata service2 = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    if (service2 != null)
    {
      PortalAttributeType[] relationAttributes = service2.GetPublishRelationAttributes();
      if (relationAttributes != null && relationAttributes.Length != 0)
      {
        for (int index = 0; index < relationAttributes.Length; ++index)
        {
          PortalAttributeType columnID = relationAttributes[index];
          if (columnID.Type != FieldTypes.ftBlob && columnID.Type != FieldTypes.ftMemo && columnID.Type != FieldTypes.ftShortBlob && columnID.Type != FieldTypes.ftFile)
          {
            NodeColumn column = service1.CreateColumn(SiteClientConsts.PublishRelationColumnSchemeGuid, (object) columnID);
            supportedColumns.Add(column);
          }
        }
      }
    }
    return supportedColumns;
  }

  public override object MapColumnToField(NodeColumn column)
  {
    object field = base.MapColumnToField(column);
    return field == null && column.SchemeGuid == SiteClientConsts.PublishRelationColumnSchemeGuid ? (object) new NodeColumnID((object) ((PortalAttributeType) column.ID).ID, AttributeSourceTypes.Relation) : field;
  }

  public override List<object> GetSpecialFields()
  {
    List<object> specialFields = base.GetSpecialFields();
    specialFields.Add((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation));
    return specialFields;
  }

  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    PublishedObjectNodeID nodeId = (PublishedObjectNodeID) base.CreateNodeId(fieldValues, adapter);
    long int64 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) new NodeColumnID((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation))]);
    return (INodeID) new RelatedPublishObjectNodeID(nodeId.TypeID, nodeId.ObjectID, nodeId.ObjectGuid, nodeId.CopyKeepers, nodeId.OwnerID, int64, nodeId.Caption);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    object data = base.GetData(nodeID, dataFormat);
    if (data != null)
      return data;
    RelatedPublishObjectNodeID publishObjectNodeId = nodeID as RelatedPublishObjectNodeID;
    return dataFormat == typeof (IPublishRelationID) && publishObjectNodeId != null ? (object) publishObjectNodeId : (object) null;
  }
}
