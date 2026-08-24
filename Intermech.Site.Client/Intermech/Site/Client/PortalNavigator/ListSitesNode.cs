// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ListSitesNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class ListSitesNode : CompositeNode, IContextAware, INodeNotifications
{
  private int _sitesObjectType;
  private IServiceProvider services;

  public ListSitesNode()
  {
    this._sitesObjectType = MetaDataHelper.GetObjectTypeID(PortalConsts.objtypeSites);
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new ListSitesPart(this._sitesObjectType, this.Services));
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new ListSitesPart(this._sitesObjectType, this.Services));
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this.services;
    set => this.services = value;
  }

  public ProcessResult Process(NotificationEventArgs e, object AdditionalInfo)
  {
    return ProcessResult.None;
  }
}
