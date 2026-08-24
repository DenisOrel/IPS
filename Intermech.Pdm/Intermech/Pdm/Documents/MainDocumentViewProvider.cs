// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Documents.MainDocumentViewProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Documents;

internal class MainDocumentViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    string format = "FormDesignerObject = {0}";
    if (items != null && items.Count == 1 && items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
    {
      List<int> objectTypeChildrenId = MetaDataHelper.GetObjectTypeChildrenID(new Guid("cad00268-306c-11d8-b4e9-00304f19f545"));
      objectTypeChildrenId.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenID(new Guid("cad00583-306c-11d8-b4e9-00304f19f545")));
      if (objectTypeChildrenId.Contains(itemData.ObjectType))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IFormDesignerService customService1 = sessionKeeper.Session.GetCustomService(typeof (IFormDesignerService)) as IFormDesignerService;
          if (!(sessionKeeper.Session.GetCustomService(typeof (IArticleService)) is IArticleService customService2) || customService1 == null)
            return ViewsInfo.Empty;
          IFiltrationService service = (IFiltrationService) ServicesManager.GetService(typeof (IFiltrationService));
          if (service != null)
          {
            string filtrationServiceOwnerId = service.FiltrationServiceOwnerID;
          }
          long mainDocumentId = customService2.FindMainDocumentID(itemData.ObjectID, service.FiltrationServiceOwnerID, (object) sessionKeeper.Session.SessionGUID);
          if (mainDocumentId == 0L)
            return ViewsInfo.Empty;
          ICollection<FormInformation> formsForObject = customService1.GetFormsForObject(mainDocumentId, sessionKeeper.Session.SessionGUID);
          if (formsForObject != null)
          {
            if (formsForObject.Count > 0)
            {
              ViewsInfo views = new ViewsInfo();
              Type controlType = typeof (MainDocumentView);
              foreach (FormInformation formInformation in (IEnumerable<FormInformation>) formsForObject)
              {
                string viewName = string.Format(format, (object) formInformation.ToString(true));
                views.Add(viewName, new ViewInfo(4, controlType));
              }
              return views;
            }
          }
        }
      }
    }
    return ViewsInfo.Empty;
  }
}
