// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.SiteNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class SiteNodeID : INodeID, IComparable
{
  private long _objId;
  private Guid _siteGuid;
  private char _code;
  private string _name;
  private int _typeID;
  protected object cookie;

  public SiteNodeID(long objId, int objTypeId, Guid siteGuid, char code, string name)
  {
    this._objId = objId;
    this._siteGuid = siteGuid;
    this._code = code;
    this._name = name;
    this._typeID = objTypeId;
    this.cookie = (object) null;
  }

  public override bool Equals(object obj)
  {
    return !(obj is SiteNodeID siteNodeId) ? base.Equals(obj) : siteNodeId._objId == this._objId;
  }

  public override int GetHashCode() => this._objId.GetHashCode();

  public int CompareTo(object obj) => !this.Equals(obj) ? 1 : 0;

  public virtual int CategoryID
  {
    [DebuggerStepThrough] get => SiteClientConsts.CategorySiteNode;
  }

  public int TypeID
  {
    [DebuggerStepThrough] get => 0;
  }

  public object Cookie
  {
    [DebuggerStepThrough] get => this.cookie;
    set => this.cookie = value;
  }

  public long ObjectID
  {
    [DebuggerStepThrough] get => this._objId;
  }

  public long ObjectTypeID
  {
    [DebuggerStepThrough] get => (long) this._typeID;
  }

  public string Name
  {
    [DebuggerStepThrough] get => this._name;
  }

  public char Code
  {
    [DebuggerStepThrough] get => this._code;
  }

  public Guid Guid
  {
    [DebuggerStepThrough] get => this._siteGuid;
  }
}
