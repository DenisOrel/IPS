// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesView.StandartArticlesModel
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.ArticlesList;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.ArticlesView;

internal class StandartArticlesModel : ISubscriberArticlesView
{
  private List<int> _modelTypeIDs;

  public StandartArticlesModel()
  {
    this._modelTypeIDs = MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad015cb-306c-11d8-b4e9-00304f19f545"));
  }

  public bool IsHandle(int objectType) => this._modelTypeIDs.Contains(objectType);

  public List<Article> GetArticles(IUserSession session, long documentID)
  {
    DataTable dataTable = session.GetRelationCollection(session.IdentHelper.DocRelationTypeID).EntersInVersion(new DBRecordSetParams((ConditionStructure[]) null, new object[3]
    {
      (object) -2,
      (object) -7,
      (object) -50
    }), documentID);
    List<Article> articles = new List<Article>(dataTable.Rows.Count);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      articles.Add(new Article(Convert.ToInt64(dataTable.Rows[index][0]), false, Convert.ToInt32(dataTable.Rows[index][1]), Convert.ToString(dataTable.Rows[index][2])));
    return articles;
  }
}
