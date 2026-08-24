// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.CompositionNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class CompositionNode : CompositeNode, IContextAware
{
  private long _objID;
  private int _objectType;
  public NodeColumnCollection Columns;
  private IServiceProvider services;

  public CompositionNode()
  {
    this._objID = 0L;
    this.options = NodeOptions.CanContainsRelationsList;
  }

  public CompositionNode(long objID, int objectType)
  {
    this._objID = objID;
    this._objectType = objectType;
    this.options = NodeOptions.CanContainsRelationsList;
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this.services;
    set => this.services = value;
  }

  public INodeQuery GetReportQuery() => (INodeQuery) null;

  protected override List<PartSlot> CreateFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new CompositionPart(this._objID, this._objectType, this.Services));
  }
}
