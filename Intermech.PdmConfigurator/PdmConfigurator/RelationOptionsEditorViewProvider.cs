// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.RelationOptionsEditorViewProvider
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.PdmConfigurator;

internal class RelationOptionsEditorViewProvider : IViewsProvider
{
  private static ICurrentUserAndRole _userAndRole;
  private static volatile bool _registered;

  public RelationOptionsEditorViewProvider()
  {
    if (RelationOptionsEditorViewProvider._registered)
      return;
    AdjustableViewsHelper.RegisterView("PdmRelationOptionsEditorView", LocalizationHolder.rm.GetString("PdmConfigurator_95"), LocalizationHolder.rm.GetString("PdmConfigurator_96"), "Intermech.Pdm", "imgPdmConfigurator.Options", true, 0);
    RelationOptionsEditorViewProvider._registered = true;
  }

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    IDBRelationID itemData1 = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    IDBTypedObjectID itemData2 = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    if (itemData1 == null && itemData2 == null || itemData1 != null && !MetaDataHelper.IsPdmPartiallyConfigurableRelationType(itemData1.RelationType) && itemData2 != null && !MetaDataHelper.IsPdmConfigurableObjectType(itemData2.ObjectType) && !MetaDataHelper.IsPdmContextableObjectType(itemData2.ObjectType))
      return ViewsInfo.Empty;
    IViewState service = services != null ? services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    if ((service == null ? 0 : ((service.ViewState & ViewStateFlags.NodeUnderTree) > ViewStateFlags.None ? 1 : 0)) == 0)
      return ViewsInfo.Empty;
    if (RelationOptionsEditorViewProvider._userAndRole == null)
      RelationOptionsEditorViewProvider._userAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    ViewsInfo views = new ViewsInfo();
    views.Add("PdmRelationOptionsEditorView", new ViewInfo(4, -1, typeof (RelationOptionsEditorView)));
    return views;
  }
}
