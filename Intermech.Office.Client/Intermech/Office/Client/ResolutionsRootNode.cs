// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionsRootNode
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Office.Client;

internal class ResolutionsRootNode : CompositeNode, IContextAware, INodeNotifications
{
  private readonly long _objectID;
  [NotNull]
  private readonly AdvancedServiceContainer _services;
  [NotNull]
  private readonly RelatedObjectsPart _part;

  public ResolutionsRootNode(long objectID, int objectType, [CanBeNull] IServiceProvider provider)
  {
    this._objectID = objectID;
    this._services = new AdvancedServiceContainer();
    this._services.AdvancedProvider = provider;
    this._part = new RelatedObjectsPart(objectType, this._objectID, RelatedObjectsRole.Composition, OfficeConsts.ReltypeOfficeCompositionID, this.Services);
    this.options = NodeOptions.CanContainsComposition;
  }

  [NotNull]
  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => (IServiceProvider) this._services;
    set => this._services.AdvancedProvider = value;
  }

  public ProcessResult Process(NotificationEventArgs e, [CanBeNull] object additionalInfo)
  {
    return ProcessResult.None;
  }

  [CanBeNull]
  protected override List<PartSlot> CreateFolderSlots()
  {
    if (this.Services.GetService<IViewState>(false) == null)
      return (List<PartSlot>) null;
    return new List<PartSlot>()
    {
      new PartSlot(OfficeConsts.ReltypeOfficeCompositionGuid, (INodePart) this._part)
    };
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    return (content & ContentType.NonFolders) != ContentType.NonFolders ? TreeResolutionsView.DefaultTreeColumns : this._part.GetDefaultColumns();
  }

  public override NodeColumnCollection GetSupportedColumns(
    ContentType content,
    string columnSetName)
  {
    return (content & ContentType.NonFolders) != ContentType.NonFolders ? TreeResolutionsView.SupportedTreeColumns : this._part.GetSupportedColumns(columnSetName);
  }

  public override bool Equals(object obj)
  {
    return !(obj is ResolutionsRootNode resolutionsRootNode) ? base.Equals(obj) : Math.Abs(resolutionsRootNode._objectID) == Math.Abs(this._objectID);
  }

  public override int GetHashCode() => Math.Abs(this._objectID).GetHashCode();
}
