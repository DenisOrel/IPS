// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ArcArtPump.ArtArtsPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.ArcArtPump;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Article;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ArcArtPump;

[TaskDescription("Инициализация загрузки информации о изделиях", "Загрузка информации о изделиях")]
[TaskType(PumperType.MetaData)]
internal class ArtArtsPump : PumpClass
{
  private IDataBase _searchConnection;
  internal static Guid ClassGuid = new Guid("{1D2BED85-46B4-4119-8E53-8CF995E3CEBA}");

  private void LoadArticlesForTechCard()
  {
    string str = $"SELECT {"F_KEY"}, {"F_ID"}, {"F_VER"}, {"F_NAME"}, {"F_DESIGNATION"} FROM {"TC_ARCARTS"}";
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = str;
    using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
    {
      int ordinal1 = dataReader.GetOrdinal("F_KEY");
      int ordinal2 = dataReader.GetOrdinal("F_ID");
      int ordinal3 = dataReader.GetOrdinal("F_VER");
      int ordinal4 = dataReader.GetOrdinal("F_NAME");
      int ordinal5 = dataReader.GetOrdinal("F_DESIGNATION");
      if (TechPumpData.TechObjects.ArcArtList == null)
        TechPumpData.TechObjects.ArcArtList = new Dictionary<long, ArcArtsObject>();
      else
        TechPumpData.TechObjects.ArcArtList.Clear();
      try
      {
        while (dataReader.Read())
        {
          int result1;
          if (int.TryParse(dataReader.GetString(ordinal2), out result1))
          {
            int int32 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
            int result2;
            if (int.TryParse(dataReader[ordinal3] != DBNull.Value ? dataReader.GetString(ordinal3) : "0", out result2))
            {
              ArcArtsObject arcArtsObject = new ArcArtsObject(result1, result2, Convert.ToString(dataReader[ordinal4]), Convert.ToString(dataReader[ordinal5]));
              TechPumpData.TechObjects.ArcArtList.Add((long) int32, arcArtsObject);
            }
          }
        }
      }
      finally
      {
        dataReader.Close();
      }
    }
  }

  private void LoadArticlesForPortal()
  {
    Dictionary<(int, int), List<long>> dictionary = new Dictionary<(int, int), List<long>>();
    foreach (KeyValuePair<long, ArcArtsObject> arcArt in TechPumpData.TechObjects.ArcArtList)
    {
      (int, int) key;
      ref (int, int) local = ref key;
      ArcArtsObject arcArtsObject = arcArt.Value;
      int artId = arcArtsObject.ArtId;
      arcArtsObject = arcArt.Value;
      int artVer = arcArtsObject.ArtVer;
      local = (artId, artVer);
      List<long> longList;
      if (!dictionary.TryGetValue(key, out longList))
      {
        longList = new List<long>();
        dictionary.Add(key, longList);
      }
      longList.Add(arcArt.Key);
    }
    if (this._searchConnection == null)
      return;
    IPortalSearchArticleVersionCache service = ApplicationServices.Container.GetService<IPortalSearchArticleVersionCache>();
    if (!service.Loaded)
      service.Load();
    foreach (PortalSearchArticleVersion searchArticleVersion in (IEnumerable<PortalSearchArticleVersion>) service.Objects)
    {
      (int, int) key1 = (searchArticleVersion.ArtId, searchArticleVersion.ArtVer);
      List<long> longList;
      if (dictionary.TryGetValue(key1, out longList))
      {
        foreach (long key2 in longList)
        {
          ArcArtsObject arcArt = TechPumpData.TechObjects.ArcArtList[key2] with
          {
            PortalVerGuid = searchArticleVersion.IpsObjVerGuid
          };
          TechPumpData.TechObjects.ArcArtList[key2] = arcArt;
        }
      }
    }
  }

  public ArtArtsPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => ArtArtsPump.ClassGuid;

  public override void Exam()
  {
    this.ExamCheckPoint("Проверка информации об изделиях", 0);
    if (!this.TableExists("TC_ARCARTS"))
    {
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TC_ARCARTS"}' не найдена.");
    }
    else
    {
      this._searchConnection = SearchConnectionsManager.GetConnection();
      if (this._searchConnection == null)
        return;
      this.ExamCheckPoint("Проверка информации об изделиях успешно завершена", 100);
    }
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Загрузка информации об изделиях", 0);
    this.LoadArticlesForTechCard();
    this.PumpCheckPoint("Загрузка информации об изделиях портала", 50);
    this.LoadArticlesForPortal();
    this.PumpCheckPoint("Загрузка информации об изделиях успешно завершена", 100);
    TechCache.WriteOneList(TechCache.CategoryList.ArcArtList, (object) TechPumpData.TechObjects.ArcArtList);
  }
}
