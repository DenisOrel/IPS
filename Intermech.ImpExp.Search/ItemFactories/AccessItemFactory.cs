// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.AccessItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal sealed class AccessItemFactory(IDataReader dataReader, IAppManager appManager) : 
  PumpItemFactory(AccessItemFactory.TableName, dataReader, appManager)
{
  public static string TableName = "ACCESS_LEVELS";
  public static string TableColumns = "LEVEL_ID, LEVEL_NAME";

  public Tuple<int, string> NewItem(IDataReader idr)
  {
    return new Tuple<int, string>(this.getInt32(idr, 0), this.getString(idr, 1));
  }
}
