// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ObjectOptionsEditorViewProvider
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

internal class ObjectOptionsEditorViewProvider : IViewsProvider
{
  private static ICurrentUserAndRole _userAndRole;
  private static volatile bool _registered;

  public ObjectOptionsEditorViewProvider()
  {
    if (ObjectOptionsEditorViewProvider._registered)
      return;
    AdjustableViewsHelper.RegisterView("PdmObjectOptionsEditorView", LocalizationHolder.rm.GetString("PdmConfigurator_72"), LocalizationHolder.rm.GetString("PdmConfigurator_75"), "Intermech.Pdm", "imgPdmConfigurator.Options", true, 14);
    ObjectOptionsEditorViewProvider._registered = true;
  }

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1 || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !MetaDataHelper.IsPdmConfigurableObjectType(itemData.ObjectType))
      return ViewsInfo.Empty;
    IViewState service = services != null ? services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    int num = service == null ? 0 : ((service.ViewState & ViewStateFlags.InParametersCard) > ViewStateFlags.None ? 1 : 0);
    if (ObjectOptionsEditorViewProvider._userAndRole == null)
      ObjectOptionsEditorViewProvider._userAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    ViewsInfo views = new ViewsInfo();
    if (ObjectOptionsEditorViewProvider._userAndRole.EnabledPdmConfigurator)
      views.Add("PdmObjectOptionsEditorView", new ViewInfo(4, 1828, typeof (ObjectOptionsEditorView)));
    return views;
  }
}
