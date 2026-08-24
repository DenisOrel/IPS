// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Analogs.AnalogsViewsProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Search.Pdm.Analogs;

public sealed class AnalogsViewsProvider : IViewsProvider
{
  public static bool CheckParamsForAnalogsView(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    if (SelectedItemsHelper.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeID(selectedItems, out typedObjectID) && AnalogsHelper.IsObjectTypeSupportedAnalogs(typedObjectID.ObjectType))
      return true;
    typedObjectID = (IDBTypedObjectID) null;
    return false;
  }

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    ViewsInfo views = new ViewsInfo();
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (AnalogsViewsProvider.CheckParamsForAnalogsView(items, (IServiceProvider) null, out typedObjectID))
      views.Add("Analogs", new ViewInfo(-1, typeof (AnalogsView)));
    return views;
  }
}
