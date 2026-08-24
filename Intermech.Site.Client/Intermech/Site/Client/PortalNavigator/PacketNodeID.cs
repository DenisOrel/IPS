// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PacketNodeID : INodeID, IPacketNodeID, IPublishTypedID
{
  private int _objectType;
  private object _cookie;
  private long _objectID;
  private char _creatorID;
  private string _caption;
  private DateTime _createDate;

  public PacketNodeID(
    int objectType,
    long objectID,
    char creatorID,
    string caption,
    DateTime createDate)
  {
    this._objectType = objectType;
    this._objectID = objectID;
    this._creatorID = creatorID;
    this._caption = caption;
    this._createDate = createDate;
  }

  public long ObjectID => this._objectID;

  public string Caption => this._caption;

  public char CreatorID => this._creatorID;

  public DateTime CreateDate => this._createDate;

  public int CategoryID
  {
    [DebuggerStepThrough] get => SiteClientConsts.CategoryPublishPacket;
  }

  public int TypeID
  {
    [DebuggerStepThrough] get => this._objectType;
  }

  public object Cookie
  {
    [DebuggerStepThrough] get => this._cookie;
    [DebuggerStepThrough] set => this._cookie = value;
  }

  public override string ToString() => this._caption;
}
