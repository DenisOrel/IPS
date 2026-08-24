// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesView.ISubscriberArticlesView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Navigator.ArticlesList;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.ArticlesView;

internal interface ISubscriberArticlesView
{
  bool IsHandle(int objectType);

  List<Article> GetArticles(IUserSession session, long documentID);
}
