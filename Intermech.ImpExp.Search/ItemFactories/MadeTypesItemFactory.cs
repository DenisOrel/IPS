// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.MadeTypesItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class MadeTypesItemFactory : PumpItemFactory
{
  public static string TableName = "USERSECT";
  public static string TableColumns = "USER_ID, SECTION_ID";
  private static int idxArticleTypeID = -1;
  private static int idxDocTypeID = -1;

  public MadeTypesItemFactory(string tableName, IDataReader dataReader, IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "USER_ID";
    string fieldName2 = "SECTION_ID";
    MadeTypesItemFactory.idxDocTypeID = this.getFieldIndex(fieldName1);
    MadeTypesItemFactory.idxArticleTypeID = this.getFieldIndex(fieldName2);
  }

  public IMadeTypesItem MadeTypesItems(IDataReader idr)
  {
    MadeTypesItem madeTypesItem = new MadeTypesItem();
    while (idr.Read())
    {
      int int32 = this.getInt32(idr, MadeTypesItemFactory.idxDocTypeID);
      if (int32 > 1000000)
        madeTypesItem.AddItem(int32 - 1000000, this.getInt32(idr, MadeTypesItemFactory.idxArticleTypeID));
    }
    return (IMadeTypesItem) madeTypesItem;
  }
}
