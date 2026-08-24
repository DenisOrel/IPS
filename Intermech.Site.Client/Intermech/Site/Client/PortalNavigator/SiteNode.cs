// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.SiteNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class SiteNode : CompositeNode, IContextAware, INodeNotifications
{
  private Guid _siteGuid;
  private IServiceProvider _services;

  public SiteNode(Guid siteGuid) => this._siteGuid = siteGuid;

  protected override List<PartSlot> CreateFolderSlots() => (List<PartSlot>) null;

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new SiteUsersPart(this._services, this._siteGuid));
  }

  public override INode GetChild(INodeID nodeID) => (INode) null;

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set => this._services = value;
  }

  public ProcessResult Process(NotificationEventArgs e, object AdditionalInfo)
  {
    return ProcessResult.None;
  }
}
