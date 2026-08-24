// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.OrdersViewProvider
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.PdmConfigurator;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
///  Провайдер закладок для мастера по созданию производственных заказов
/// </summary>
internal class OrdersViewProvider : IViewsProvider
{
  /// <summary>
  /// Список родительских типов объектов, в состав которых может входить маршрут обработки
  /// </summary>
  private static List<int> _applsList = new List<int>();
  /// <summary>
  /// Список родительских типов объектов, в состав которых запрещено включать маршрут обработки
  /// </summary>
  private static List<int> _disabledApplsList = new List<int>();

  /// <summary>
  /// Возвращает контейнер со сведениями о закладках, которые должны быть
  /// выведены на экран в указанном контексте, а также о закладках других
  /// провайдеров, вывод которых должен быть подавлен.
  /// </summary>
  /// <param name="items">Коллекция выбранных пользователем элементов навигации.</param>
  /// <param name="services">Контейнер сервисов, которыми может пользоваться закладка.</param>
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    ManufactOrdersEditor service1 = services != null ? services.GetService(typeof (ManufactOrdersEditor)) as ManufactOrdersEditor : (ManufactOrdersEditor) null;
    ManufactureOrderHolder service2 = services != null ? services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder : (ManufactureOrderHolder) null;
    IDBTypedObjectID service3 = services != null ? services.GetService(typeof (IDBTypedObjectID)) as IDBTypedObjectID : (IDBTypedObjectID) null;
    if (service1 == null || service2 == null || service3 == null || items.Count != 1 || services == null)
      return ViewsInfo.Empty;
    NavigatorTreeView service4 = services.GetService(typeof (NavigatorTreeView)) as NavigatorTreeView;
    NavigatorTreeNode navigatorTreeNode = (NavigatorTreeNode) null;
    if (service4 != null)
    {
      NavigatorTreeNode[] selectedNodes = service4.SelectedNodes;
      if (selectedNodes.Length != 0)
        navigatorTreeNode = selectedNodes[0];
    }
    IDBTypedObjectID itemData1 = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IDBRelationID itemData2 = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    if (itemData1 == null || itemData2 == null || navigatorTreeNode == null)
      return ViewsInfo.Empty;
    services?.GetService(typeof (IViewState));
    if (OrdersViewProvider._applsList.Count == 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        DataTable applicabilitiesList = sessionKeeper.Session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(MetaDataHelper.GetRelationTypeID("cad0019f-306c-11d8-b4e9-00304f19f545"), MetaDataHelper.GetObjectTypeID("cad0016f-306c-11d8-b4e9-00304f19f545"), -1);
        if (applicabilitiesList != null)
        {
          if (applicabilitiesList.Rows.Count > 0)
          {
            for (int index = 0; index < applicabilitiesList.Rows.Count; ++index)
            {
              IMSApplicability imsApplicability = new IMSApplicability();
              imsApplicability.Load(applicabilitiesList.Rows[index]);
              int inObjectType = imsApplicability.InObjectType;
              if (inObjectType != -1)
              {
                if (imsApplicability.ApplicabilityMode == ApplicabilityModes.Disabled)
                {
                  if (OrdersViewProvider._disabledApplsList.IndexOf(imsApplicability.InObjectType) < 0)
                    OrdersViewProvider._disabledApplsList.Add(imsApplicability.InObjectType);
                }
                else
                  OrdersViewProvider._applsList.Add(inObjectType);
              }
            }
          }
        }
      }
    }
    IMSAttribute4ObjectType attribute4ObjectType = MetaDataHelper.GetAttribute4ObjectType(itemData1.ObjectType, MetaDataHelper.GetAttributeTypeID("cad0038f-306c-11d8-b4e9-00304f19f545"));
    ViewsInfo views = new ViewsInfo();
    if (navigatorTreeNode.Level == 2 && MetaDataHelper.IsPdmConfigurableObjectType(itemData1.ObjectType) && (ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).EnabledPdmConfigurator)
      views.Add("MRP_PDMConfigurator", new ViewInfo(4, -1, typeof (MRP_PDMConfiguratorView)));
    if (navigatorTreeNode.Level == 1)
    {
      views.Add("PdmObjectOptionsEditorView", new ViewInfo(4, -1, typeof (ObjectOptionsEditorView)));
      views.Suppress("PdmRelationOptionsEditorView", 7);
    }
    else
      views.Suppress("PdmRelationOptionsEditorView", 7);
    if (navigatorTreeNode.Level >= 2 && MetaDataHelper.HasObjectTypeSubstRelTypes(itemData1.ObjectType))
      views.Add("MRP_PDMSubstitutes", new ViewInfo(4, -1, typeof (MRP_PDMSubstitutesView)));
    if (navigatorTreeNode.Level >= 2 && MetaDataHelper.IsEnabledParentType(itemData1.ObjectType, (IEnumerable<int>) OrdersViewProvider._applsList, (IEnumerable<int>) OrdersViewProvider._disabledApplsList, false))
      views.Add("MRP_TechcardRoutes", new ViewInfo(4, -1, typeof (MRP_TechcardRoutesView)));
    if (navigatorTreeNode.Level >= 2 && attribute4ObjectType != null)
      views.Add("MRP_BoughtArticles", new ViewInfo(4, -1, typeof (MRP_BoughtArticlesView)));
    return views;
  }
}
