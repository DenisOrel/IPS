// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocumentVisualizerView
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Office.Interfaces;
using Intermech.Workflow;
using System;

#nullable disable
namespace Intermech.Office.Client;

[ViewDescriptionProvider(typeof (OfficeDocumentVisualizerView.OfficeDocumentVisualizerViewDescriptionProvider))]
internal class OfficeDocumentVisualizerView : Intermech.Client.Core.Navigator.Classes.ObjectNode.VisualizerView.VisualizerView
{
  public override int OrderID => 0;

  [NotNull]
  public override string Caption => "Просмотр документа";

  public override void Initialize([NotNull] ISelectedItems items, IServiceProvider services)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._objectId = sessionKeeper.Session.GetObject(items.GetItemData<IDBObjectID>(0).Value).AttributeByID(wfConsts.AttrProcessID).As<IDBObjectLinkAttribute>().DBObject.AttributeByID(OfficeConsts.AttrResolutionIdentityID).AsInteger;
      this._objectType = -1;
    }
    this._dataLoaded = false;
  }

  private sealed class OfficeDocumentVisualizerViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      INamedImageList service = ApplicationServices.Container.GetService(typeof (INamedImageList)) as INamedImageList;
      return new ViewDescription()
      {
        Caption = "Просмотр документа",
        ImageIndex = service != null ? service.ImageIndex("imgView") : 0,
        OrderID = 0
      };
    }
  }
}
