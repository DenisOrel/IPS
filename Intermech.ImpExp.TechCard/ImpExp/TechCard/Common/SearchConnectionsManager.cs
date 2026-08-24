// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.SearchConnectionsManager
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.SafeDataProxy;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class SearchConnectionsManager
{
  internal const string SearchDbAlias = "SEARCH PLUGIN CONNECTION";
  private static IDataBase _searchDataBase;

  public static IDataBase GetConnection(bool throwError = false)
  {
    if (SearchConnectionsManager._searchDataBase != null)
      return SearchConnectionsManager._searchDataBase;
    try
    {
      SearchConnectionsManager._searchDataBase = TechcardConsts.Plugin.appManager.DBManager.FindDbByAlias("SEARCH PLUGIN CONNECTION");
      if (SearchConnectionsManager._searchDataBase == null)
      {
        IDbConnection dbConnection = TechcardConsts.Plugin.CustomDbConnection(ConnStrType.Search);
        if (dbConnection != null)
        {
          if (dbConnection.State.HasFlag((Enum) ConnectionState.Open))
            SearchConnectionsManager._searchDataBase = TechcardConsts.Plugin.appManager.DBManager.FindDbByAlias("SEARCH PLUGIN CONNECTION");
        }
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Невозможно подключиться к базе Search: {ex.Message}");
      if (!throwError)
        return (IDataBase) null;
      throw;
    }
    if (SearchConnectionsManager._searchDataBase == null)
    {
      string str = $"Подключение к базе Search \"{"SEARCH PLUGIN CONNECTION"}\" не найдено ";
      TechcardConsts.Plugin.appManager.AddErrorMessage(str);
      if (throwError)
        throw new Exception(str);
    }
    SearchConnectionsManager._searchDataBase = SearchConnectionsManager._searchDataBase is SafeDataBaseProxy ? SearchConnectionsManager._searchDataBase : (IDataBase) new SafeDataBaseProxy(SearchConnectionsManager._searchDataBase, (ISafeProxyErrorHandler) new ImpExpErrorHandler(TechcardConsts.Plugin.appManager));
    return SearchConnectionsManager._searchDataBase;
  }
}
