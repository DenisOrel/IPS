// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.OptionEditorViewProvider
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

internal class OptionEditorViewProvider : IViewsProvider
{
  private static ICurrentUserAndRole _userAndRole;
  private static volatile bool _registered;

  public OptionEditorViewProvider()
  {
    if (OptionEditorViewProvider._registered)
      return;
    AdjustableViewsHelper.RegisterView("PdmConfiguratorOptionView", LocalizationHolder.rm.GetString("PdmConfigurator_82"), LocalizationHolder.rm.GetString("PdmConfigurator_85"), "Intermech.Pdm", "imgPdmConfigurator.Options", true, 0);
    OptionEditorViewProvider._registered = true;
  }

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1 || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID))
      return ViewsInfo.Empty;
    IViewState service = services != null ? services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    int num = service == null ? 0 : ((service.ViewState & ViewStateFlags.InParametersCard) > ViewStateFlags.None ? 1 : 0);
    if (OptionEditorViewProvider._userAndRole == null)
      OptionEditorViewProvider._userAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    ViewsInfo views = new ViewsInfo();
    views.Add("PdmConfiguratorOptionView", new ViewInfo(4, 1826, typeof (OptionEditorView)));
    return views;
  }
}
