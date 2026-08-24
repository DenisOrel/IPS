// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ParamSizeItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class ParamSizeItemFactory
{
  public string TableName = string.Empty;

  public ParamSizeItemFactory(string AliasArt, string AliasDoc)
  {
    if (AliasArt != string.Empty)
    {
      this.TableName = AliasArt;
    }
    else
    {
      if (!(AliasDoc != string.Empty))
        return;
      this.TableName = AliasDoc;
    }
  }

  public int GetSize(IDataReader idr)
  {
    DataRow[] dataRowArray = idr.GetSchemaTable().Select("ColumnName = 'P_VALUE'");
    return dataRowArray.Length != 0 ? Convert.ToInt32(dataRowArray[0]["ColumnSize"]) : 0;
  }
}
