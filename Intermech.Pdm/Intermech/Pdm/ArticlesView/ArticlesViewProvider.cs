// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesView.ArticlesViewProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Pdm.ArticlesView;

internal class ArticlesViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items != null && items.Count == 1 && items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
    {
      IMSObjectType objectType = MetaDataHelper.GetObjectType(itemData.ObjectType);
      if (objectType == null)
        return ViewsInfo.Empty;
      if (!SubscribersArticlesView.Initialized)
        SubscribersArticlesView.Initialize();
      if ((objectType.Options & ObjectTypeOptions.ReleaseArticlesEnabled) == ObjectTypeOptions.ReleaseArticlesEnabled || SubscribersArticlesView.GetSubscriber(objectType.ObjectTypeID) != null)
      {
        ViewsInfo views = new ViewsInfo();
        views.Add("PDM.ArticlesView", new ViewInfo(0, 691, typeof (ListArticlesView)));
        return views;
      }
    }
    return ViewsInfo.Empty;
  }
}
