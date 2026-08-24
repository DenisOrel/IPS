// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ArticleTypeLinksItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class ArticleTypeLinksItemFactory : PumpItemFactory
{
  public static string TableName = "OBJ_TYPES_LINKS";
  private static int idxInObjectType = -1;
  private static int idxObjectType = -1;
  private static int idxLinkType = -1;
  private static int idxRequired = -1;

  public ArticleTypeLinksItemFactory(
    string tableName,
    IDataReader dataReader,
    IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "F_INOBJECT_TYPE";
    string fieldName2 = "F_OBJECT_TYPE";
    string fieldName3 = "F_LINK_TYPE";
    string fieldName4 = "F_REQUIRED";
    ArticleTypeLinksItemFactory.idxInObjectType = this.getFieldIndex(fieldName1);
    ArticleTypeLinksItemFactory.idxObjectType = this.getFieldIndex(fieldName2);
    ArticleTypeLinksItemFactory.idxLinkType = this.getFieldIndex(fieldName3);
    ArticleTypeLinksItemFactory.idxRequired = this.getFieldIndex(fieldName4);
  }

  public IArticleTypeLinksItem NewItem(IDataReader idr)
  {
    return (IArticleTypeLinksItem) new ArticleTypeLinksItemFactory.ArticleTypeLinksItem()
    {
      inObjectType = this.getInt32(idr, ArticleTypeLinksItemFactory.idxInObjectType),
      objectType = this.getInt32(idr, ArticleTypeLinksItemFactory.idxObjectType),
      linkType = this.getInt32(idr, ArticleTypeLinksItemFactory.idxLinkType),
      required = this.getInt32(idr, ArticleTypeLinksItemFactory.idxRequired)
    };
  }

  private class ArticleTypeLinksItem : IArticleTypeLinksItem
  {
    internal int inObjectType;
    internal int objectType;
    internal int linkType;
    internal int required;

    public int InObjectType => this.inObjectType;

    public int ObjectType => this.objectType;

    public int LinkType => this.linkType;

    public int Required => this.required;
  }
}
