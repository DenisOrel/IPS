// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.RelatedPublishObjectNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class RelatedPublishObjectNodeID : PublishedObjectNodeID, IPublishRelationID
{
  private long _relationId;

  public RelatedPublishObjectNodeID(
    int objectType,
    long objectID,
    Guid objectGuid,
    string copyKeepers,
    long ownerID,
    long relationId,
    string name)
    : base(objectType, objectID, objectGuid, copyKeepers, ownerID, name)
  {
    this._relationId = relationId;
  }

  public RelatedPublishObjectNodeID(
    int objectType,
    long objectID,
    Guid objectGuid,
    string copyKeepers,
    long ownerID,
    long relationId)
    : this(objectType, objectID, objectGuid, copyKeepers, ownerID, relationId, string.Empty)
  {
  }

  public long RelationID => this._relationId;
}
