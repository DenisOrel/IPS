// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.Params4DocTypesItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class Params4DocTypesItemFactory : PumpItemFactory
{
  public static string TableName = "PARAM4DOCTYPE";
  public static string TableColumns = "DOCTYPE_ID, GROUP_ID";
  private static int idxGroupId = -1;
  private static int idxDocTypeID = -1;

  public Params4DocTypesItemFactory(
    string tableName,
    IDataReader dataReader,
    IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "DOCTYPE_ID";
    string fieldName2 = "GROUP_ID";
    Params4DocTypesItemFactory.idxDocTypeID = this.getFieldIndex(fieldName1);
    Params4DocTypesItemFactory.idxGroupId = this.getFieldIndex(fieldName2);
  }

  public IParams4DocTypesItems Params4DocTypes(IDataReader idr)
  {
    Params4DocTypesItems params4DocTypesItems = new Params4DocTypesItems();
    while (idr.Read())
      params4DocTypesItems.AddItem(this.getInt32(idr, Params4DocTypesItemFactory.idxDocTypeID), this.getInt32(idr, Params4DocTypesItemFactory.idxGroupId));
    return (IParams4DocTypesItems) params4DocTypesItems;
  }
}
