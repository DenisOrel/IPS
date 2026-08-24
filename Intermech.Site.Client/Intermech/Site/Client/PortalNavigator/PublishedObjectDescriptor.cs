// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedObjectDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PublishedObjectDescriptor : HiveDescriptor
{
  private readonly long _publishObjectID;
  private Guid _publishObjectGuid = Guid.Empty;
  private readonly long _ownerID;
  private readonly string _copyKeepers = string.Empty;
  private PublishedObjectNode _rootNode;

  public PublishedObjectDescriptor(
    long publishObjectId,
    Guid publishObjectGuid,
    int publishType,
    string copyKeepers,
    long ownerID,
    string caption)
    : base(SiteClientConsts.CategoryPublishObject, publishType, caption)
  {
    this._publishObjectID = publishObjectId;
    this._publishObjectGuid = publishObjectGuid;
    this._copyKeepers = copyKeepers;
    this._ownerID = ownerID;
    this._rootNode = new PublishedObjectNode(publishType, publishObjectId);
  }

  public Guid Guid => SiteClientConsts.CategoryPublishObjectGuid;

  public override INode GetChild(INodeID nodeID) => (INode) this._rootNode;

  public override INodeID GetRecordNodeID()
  {
    return (INodeID) new PublishedObjectNodeID(this._typeID, this._publishObjectID, this._publishObjectGuid, this._copyKeepers, this._ownerID, this._caption);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    return this._rootNode.Part.GetData(nodeID, dataFormat);
  }

  public override object MapColumnToField(NodeColumn column)
  {
    return this._rootNode.Part.MapColumnToField(column);
  }

  public override object[] GetRecordValues(INodeID nodeID, object[] fields)
  {
    if (!(nodeID is IPublishObjectID))
      return base.GetRecordValues(nodeID, fields);
    List<string> stringList = new List<string>(fields.Length);
    foreach (NodeColumnID field in fields)
      stringList.Add(field.ID.ToString());
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      Guid connectGuid = Guid.Empty;
      IPortalConnector customService = (IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector));
      try
      {
        connectGuid = customService.Login(sessionKeeper.Session.SessionGUID);
        PublishAttribute[] objectAttributes = customService.GetObjectAttributes(connectGuid, this._publishObjectID, stringList.ToArray());
        IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
        List<object> objectList = new List<object>(objectAttributes.Length);
        for (int index = 0; index < objectAttributes.Length; ++index)
        {
          string empty = string.Empty;
          Type type = typeof (string);
          DataErrors errors = DataErrors.None;
          object attributeValue = Helper.GetAttributeValue(sessionKeeper.Session, service, objectAttributes[index], ref empty, ref type, ref errors);
          if (attributeValue is object[])
            objectList.Add(((object[]) attributeValue)[0]);
          else
            objectList.Add(attributeValue);
        }
        return objectList.ToArray();
      }
      finally
      {
        if (connectGuid != Guid.Empty && customService != null)
          customService.Logout(connectGuid);
      }
    }
  }
}
