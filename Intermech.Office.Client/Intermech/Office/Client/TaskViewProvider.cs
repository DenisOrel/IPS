// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.TaskViewProvider
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

internal class TaskViewProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews([NotNull] ISelectedItems items, [CanBeNull] IServiceProvider services)
  {
    if (!TaskViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("ResolutionView", Localization.GetString("Office.Client_64"), "", "", "", true, 0);
      TaskViewProvider._registeredView = true;
    }
    if (items.Count > 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBTypedObjectID itemData = items.GetItemData<IDBTypedObjectID>(0);
      IDBAttribute attributeById = sessionKeeper.Session.GetObject(itemData.ObjectID).AttributeByID(wfConsts.AttrProcessID).As<IDBObjectLinkAttribute>().DBObject.GetAttributeByID(OfficeConsts.AttrResolutionIdentityID);
      if (attributeById != null)
      {
        if (attributeById.AsInteger != 0L)
        {
          views.Add("ResolutionView", new ViewInfo(100, 2590, typeof (ResolutionView)));
          views.Add("OfficeDocumentVisualizerView", new ViewInfo(100, 2591, typeof (OfficeDocumentVisualizerView)));
        }
      }
    }
    return views;
  }
}
