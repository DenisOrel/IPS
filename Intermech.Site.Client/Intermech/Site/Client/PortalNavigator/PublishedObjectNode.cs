// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedObjectNode
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

public class PublishedObjectNode : CompositeNode, IContextAware
{
  private int _objectTypeID;
  private long _objectID;
  private CompositionPart _part;
  private IServiceProvider services;

  internal CompositionPart Part
  {
    get
    {
      if (this._part == null)
        this._part = new CompositionPart(this._objectID, this._objectTypeID, this.Services);
      return this._part;
    }
  }

  public PublishedObjectNode()
  {
    this._objectTypeID = -1;
    this._objectID = 0L;
  }

  public PublishedObjectNode(int objectTypeID, long objectID)
  {
    this._objectTypeID = objectTypeID;
    this._objectID = objectID;
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this.services;
    set => this.services = value;
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    if (content == ContentType.Folders)
      return Helper.GetPublicObjectCaptionOnlyColumns();
    return content == ContentType.NonFolders ? this.Part.GetDefaultColumns() : base.GetDefaultColumns(content);
  }

  public override NodeColumnCollection GetSupportedColumns(
    ContentType content,
    string ColumnSetName)
  {
    if (content == ContentType.Folders)
      return Helper.GetPublishedObjectColumns();
    return content == ContentType.NonFolders ? this.Part.GetSupportedColumns(ColumnSetName) : base.GetSupportedColumns(content, ColumnSetName);
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) this.Part);
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) this.Part);
  }
}
