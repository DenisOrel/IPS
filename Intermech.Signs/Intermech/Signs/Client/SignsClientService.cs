// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsClientService
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

internal class SignsClientService : ISignsClientService
{
  /// <summary>
  /// Предлагает юзеру выбрать в каких графах и должностях нужно подписывать или создавать замечания для объекта objectID.
  /// </summary>
  /// <param name="objectID">Идентификатор версии объекта.</param>
  /// <returns>Массив с информацией о выбранных должностях и графах. Массив пустой, если юзер ничего не выбрал или отменил выбор.</returns>
  public RankGraphsInfo[] ShowUserGraphsDialog(long objectID)
  {
    List<IDBTypedObjectID> typedObjectIDs = new List<IDBTypedObjectID>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(objectID);
      typedObjectIDs.Add((IDBTypedObjectID) new DBTypedObjectID(dbObject.ObjectType, dbObject.ObjectID, dbObject.ID, dbObject.Caption, dbObject.OwnerID, (long) dbObject.VersionID, Convert.ToInt64(dbObject.IsBaseVersion), dbObject.SiteID, dbObject.ModificationID));
    }
    return this.ShowUserGraphsDialog(typedObjectIDs);
  }

  /// <summary>
  /// Предлагает юзеру выбрать в каких графах и должностях нужно подписывать или создавать замечания для объектов typedObjectIDs.
  /// </summary>
  /// <param name="typedObjectIDs">Список версий объектов.</param>
  /// <returns>Массив с информацией о выбранных должностях и графах. Массив пустой, если юзер ничего не выбрал или отменил выбор.</returns>
  public RankGraphsInfo[] ShowUserGraphsDialog(List<IDBTypedObjectID> typedObjectIDs)
  {
    UserRankInformation[] userRankInformationArray = (UserRankInformation[]) null;
    List<long> rankIDs = new List<long>();
    if (SignsCache.UserSignsCard.IsUserCanSign(typedObjectIDs, out rankIDs))
    {
      List<string> graphs = SignsCache.UserSignsCard.GetGraphs(rankIDs[0], typedObjectIDs);
      if (rankIDs.Count == 1 && graphs.Count == 1)
      {
        UserRankInformation userRankInformation = new UserRankInformation(rankIDs[0]);
        userRankInformation.Graphs.AddRange((IEnumerable<string>) graphs);
        userRankInformationArray = new UserRankInformation[1]
        {
          userRankInformation
        };
      }
      else
      {
        using (SelectRank selectRank = new SelectRank(rankIDs, typedObjectIDs, SignsCache.UserSignsCard, (GraphsSet) null))
        {
          if (selectRank.ShowDialog().Equals((object) DialogResult.OK))
          {
            if (!selectRank.SelectedItems.Count.Equals(0))
              userRankInformationArray = selectRank.SelectedItems.ToArray(typeof (UserRankInformation)) as UserRankInformation[];
          }
        }
      }
    }
    RankGraphsInfo[] rankGraphsInfoArray;
    if (userRankInformationArray == null)
    {
      rankGraphsInfoArray = new RankGraphsInfo[0];
    }
    else
    {
      rankGraphsInfoArray = new RankGraphsInfo[userRankInformationArray.Length];
      for (int index1 = 0; index1 < userRankInformationArray.Length; ++index1)
      {
        Tuple<string, string>[] tpls = new Tuple<string, string>[userRankInformationArray[index1].Graphs.Count];
        for (int index2 = 0; index2 < tpls.Length; ++index2)
        {
          if (SignsCache.PossibleGraphs.ContainsKey(userRankInformationArray[index1].Graphs[index2]))
            tpls[index2] = new Tuple<string, string>(userRankInformationArray[index1].Graphs[index2], SignsCache.PossibleGraphs[userRankInformationArray[index1].Graphs[index2]]);
        }
        rankGraphsInfoArray[index1] = new RankGraphsInfo(userRankInformationArray[index1].RankID, userRankInformationArray[index1].RankCaption, this.ExcludeNullTuples(tpls));
      }
    }
    return rankGraphsInfoArray;
  }

  /// <summary>
  /// Возвращает массив граф, в которых текущий пользователь может подписать объект objectID. Если objectID == 0, то возвращает весь список граф для данного юзера от всех его должностей.
  /// </summary>
  /// <param name="objectID">Ид. версии объекта.</param>
  /// <returns>Массив граф в виде строкового идентификатора графы и его расшифровки&gt;.</returns>
  public Tuple<string, string>[] GetUserGraphs(long objectID)
  {
    List<string> result;
    if (objectID == 0L)
    {
      result = SignsCache.UserSignsCard.GetGraphs();
    }
    else
    {
      result = new List<string>();
      QuickObjectInfo objectInfo = ApplicationServices.Container.GetService<IObjectsInfoCache>().GetObjectInfo(objectID);
      IList<long> ranks = (IList<long>) SignsCache.UserSignsCard.GetRanks(objectInfo.ObjectTypeID);
      for (int index = 0; index < ranks.Count; ++index)
      {
        List<string> graphs = SignsCache.UserSignsCard.GetGraphs(ranks[index], objectInfo.ObjectTypeID);
        this.AddGraphs(result, graphs);
      }
    }
    Tuple<string, string>[] tpls = new Tuple<string, string>[result.Count];
    for (int index = 0; index < result.Count; ++index)
    {
      if (SignsCache.PossibleGraphs.ContainsKey(result[index]))
        tpls[index] = new Tuple<string, string>(result[index], SignsCache.PossibleGraphs[result[index]]);
    }
    return this.ExcludeNullTuples(tpls);
  }

  private Tuple<string, string>[] ExcludeNullTuples(Tuple<string, string>[] tpls)
  {
    int length = 0;
    for (int index = 0; index < tpls.Length; ++index)
    {
      if (tpls[index] != null)
        ++length;
    }
    Tuple<string, string>[] tupleArray = new Tuple<string, string>[length];
    int index1 = 0;
    for (int index2 = 0; index2 < tpls.Length; ++index2)
    {
      if (tpls[index2] != null)
      {
        tupleArray[index1] = tpls[index2];
        ++index1;
      }
    }
    return tupleArray;
  }

  /// <summary>
  /// Метод добавляет в список строк result строки из add_graphs, которых ещё нет в result
  /// </summary>
  /// <param name="result">Итоговый список строк.</param>
  /// <param name="add_graphs">Добавляемые строки.</param>
  private void AddGraphs(List<string> result, List<string> add_graphs)
  {
    for (int index = 0; index < add_graphs.Count; ++index)
    {
      if (result.IndexOf(add_graphs[index]) < 0)
        result.Add(add_graphs[index]);
    }
  }
}
