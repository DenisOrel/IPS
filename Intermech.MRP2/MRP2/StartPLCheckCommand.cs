// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.StartPLCheckCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда меню запустить проверку ПВ</summary>
internal class StartPLCheckCommand
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICSharpScriptExecutor service1 = ServiceUtils.GetService<ICSharpScriptExecutor>((object) ApplicationServices.Container, true);
      string scriptCode = StartPLCheckCommand.GetScriptCode(sessionKeeper.Session);
      if (string.IsNullOrEmpty(scriptCode))
        throw new NotificationException("Cкрипт для проверки производственных ведомостей не настроен");
      for (int index1 = 0; index1 < items.Count; ++index1)
      {
        IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(index1, typeof (IDBTypedObjectID));
        AttributeValidationScriptParameters scriptParameters1 = new AttributeValidationScriptParameters()
        {
          UserSession = sessionKeeper.Session,
          ObjectID = itemData.ObjectID,
          RelationID = 0,
          ObjectAttributeValues = new List<AttributeValues>(),
          RelationAttributeValues = new List<AttributeValues>()
        };
        AttributeValidationScriptParameters scriptParameters2 = (AttributeValidationScriptParameters) service1.Execute(scriptCode, CSharpScriptInvocationOptions.Default, (object) scriptParameters1);
        IViewsManager service2 = viewServices.GetService<IViewsManager>();
        for (int index2 = 0; index2 < service2.ViewPages.Count - 1; ++index2)
        {
          if (service2.ViewPages[index2].Name == "MRP2.ProductionListReportView")
          {
            service2.ActiveViewPage = service2.ViewPages[index2];
            break;
          }
        }
      }
    }
  }

  internal static string GetScriptCode(IUserSession session)
  {
    long objectID = session.Configurations.ReadInteger("MRP2", "MRP2", "scriptID", 0L, DBConfigMode.GlobalOnly);
    return session.GetObject(objectID, false)?.GetAttributeByGuid(new Guid("cad00366-306c-11d8-b4e9-00304f19f545"))?.Value.ToString();
  }
}
