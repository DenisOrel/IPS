// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishTypeNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Selections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PublishTypeNode : CompositeNode, IContextAware, INodeNotifications
{
  protected int typeID = -1;
  protected INodePart treePart;
  protected INodePart viewPart;
  private IServiceProvider services;

  public PublishTypeNode(int typeID) => this.typeID = typeID;

  protected override List<PartSlot> CreateFolderSlots()
  {
    if (this.treePart == null)
      this.treePart = (INodePart) new PublishTypesPart(this.Services, this.typeID);
    List<PartSlot> folderSlots = this.SlotsFromSinglePart(this.treePart);
    if (this.typeID != -1)
    {
      ITopBinding binding = this.GetBinding();
      folderSlots.Insert(0, new PartSlot(Intermech.Navigator.Selections.Consts.SelectionsPartGuid, (INodePart) new DescriptorsPart(new DescriptorCollection()
      {
        {
          Intermech.Navigator.Selections.Consts.SelectionsDescriptorGuid,
          (IDescriptor) new HiveDescriptor(MetaDataHelper.GetObjectTypeID(PortalConsts.objtypePortalSelections), binding)
        }
      }, false)));
    }
    return folderSlots;
  }

  protected virtual ITopBinding GetBinding() => (ITopBinding) new PortalTypesBinding(this.typeID);

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    return base.GetData(nodeID, dataFormat);
  }

  public override INode GetChild(INodeID nodeID)
  {
    return nodeID is PublishedObjectNodeID ? (INode) new PublishedObjectNode(this.typeID, ((PublishedObjectNodeID) nodeID).ObjectID) : base.GetChild(nodeID);
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    if (this.viewPart == null)
      this.viewPart = (INodePart) new ContainsPart(this.Services, this.typeID);
    return this.SlotsFromSinglePart(this.viewPart);
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    if (content == ContentType.Folders)
      return this.treePart.GetDefaultColumns();
    return content == ContentType.NonFolders ? Helper.GetPublishedObjectColumns() : base.GetDefaultColumns(content);
  }

  public override NodeColumnCollection GetSupportedColumns(
    ContentType content,
    string ColumnSetName)
  {
    if (content == ContentType.Folders)
      return this.treePart.GetSupportedColumns(ColumnSetName);
    return content == ContentType.NonFolders ? this.viewPart.GetSupportedColumns(ColumnSetName) : base.GetSupportedColumns(content, ColumnSetName);
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this.services;
    set => this.services = value;
  }

  public ProcessResult Process(NotificationEventArgs e, object AdditionalInfo)
  {
    return e.EventName == "OwnComplete" || e.EventName == "PublishObjectsRemoved" ? ProcessResult.RefreshNode : ProcessResult.None;
  }
}
