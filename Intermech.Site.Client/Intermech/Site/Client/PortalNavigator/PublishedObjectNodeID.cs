// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedObjectNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PublishedObjectNodeID : INodeID, IPublishObjectID, IPublishTypedID
{
  protected internal string caption;
  protected internal int id;
  private object _cookie;
  private long _objectID;
  private long _ownerID;
  private Guid _objectGuid;
  private string _copyKeepers;

  public long ObjectID => this._objectID;

  public string Caption => this.caption;

  public long OwnerID => this._ownerID;

  public string CopyKeepers => this._copyKeepers;

  public PublishedObjectNodeID(
    int objectType,
    long objectID,
    Guid objectGuid,
    string copyKeepers,
    long ownerID,
    string name)
  {
    this.id = objectType;
    this._objectID = objectID;
    this._objectGuid = objectGuid;
    this._ownerID = ownerID;
    this.caption = name;
    this._copyKeepers = copyKeepers;
  }

  public PublishedObjectNodeID(
    int objectType,
    long objectID,
    Guid objectGuid,
    string copyKeepers,
    long ownerID)
    : this(objectType, objectID, objectGuid, copyKeepers, ownerID, string.Empty)
  {
  }

  public int CategoryID
  {
    [DebuggerStepThrough] get => SiteClientConsts.CategoryPublishObject;
  }

  public int TypeID
  {
    [DebuggerStepThrough] get => this.id;
  }

  public object Cookie
  {
    [DebuggerStepThrough] get => this._cookie;
    [DebuggerStepThrough] set => this._cookie = value;
  }

  public Guid ObjectGuid => this._objectGuid;

  public override bool Equals(object obj)
  {
    return obj is PublishedObjectNodeID publishedObjectNodeId && publishedObjectNodeId._objectID == this._objectID;
  }

  public override int GetHashCode() => this._objectID.GetHashCode();
}
