// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PacketNode : CompositeNode, IContextAware
{
  private int _objectTypeID;
  private long _objectID;
  private IServiceProvider services;

  public PacketNode()
  {
    this._objectTypeID = -1;
    this._objectID = 0L;
  }

  public PacketNode(int objectTypeID, long objectID)
  {
    this._objectTypeID = objectTypeID;
    this._objectID = objectID;
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this.services;
    set => this.services = value;
  }
}
