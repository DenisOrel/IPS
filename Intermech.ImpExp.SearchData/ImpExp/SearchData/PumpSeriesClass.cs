// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpSeriesClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Sets;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки номерных изделий", "Перекачка номерных изделий")]
public class PumpSeriesClass : PumpClass
{
  protected SearchDataPlugin plugin;
  private CacheCategory _artFamilies;
  private CacheCategory _artSeries;
  private CacheCategory _articlesCache;
  private CacheCategory _pumpedCache;

  protected override Guid GUID => new Guid("{58A5475F-95E1-4B80-9779-BE83A64E4A16}");

  public PumpSeriesClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  private void doPump()
  {
    this._artFamilies = PumpCache.Category[ImportingCategory.ArtFamilies];
    this._artSeries = PumpCache.Category[ImportingCategory.ArtSeries];
    this._articlesCache = PumpCache.Category[ImportingCategory.Articles];
    this._pumpedCache = PumpCache.Category[ImportingCategory.ObjectsWithSeries];
    try
    {
      this.PumpCheckPoint("Перекачка номерных изделий", 0);
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        command.CommandText = "select FAMILY_ID, FAMILY_NAME from SP_ART_FAMILY order by FAMILY_ID";
        IDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
        try
        {
          IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList(0);
          while (dataReader.Read())
          {
            int int32 = BasePumpHelper.ToInt32(dataReader[0]);
            if (this._artFamilies.GetNewKey((object) int32) <= 0L)
            {
              string caption = dataReader.GetString(1);
              importedObjectList.Items.Clear();
              importedObjectList.AddObject(PumpHelper.ObjTypeHeadArticle, 0, caption);
              AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, importedObjectList);
              importedObjectList.Import();
              long objectId = importedObjectList.Items[0].Object.Object_id;
              this._artFamilies.AddValue((object) int32, objectId);
            }
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        command.CommandText = "select n.rec_id, n.art_id, n.vart_id, n.family_id, n.start_num, n.end_num, v.art_ver_id, v.v_flag from sp_art_nums n, v_articles v where n.vart_id=v.vart_id order by n.art_id, n.vart_id, n.family_id, n.start_num";
        IDataReader dataReader = command.ExecuteReader();
        try
        {
          IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList();
          SeriesDatesApplicabilityCollection applicabilityCollection = (SeriesDatesApplicabilityCollection) null;
          SeriesDatesApplicability datesApplicability = (SeriesDatesApplicability) null;
          int oldKey1 = 0;
          int num1 = 0;
          int num2 = 0;
          int key = 0;
          List<int> intList = new List<int>();
          string str = "";
          while (dataReader.Read())
          {
            int int32_1 = BasePumpHelper.ToInt32(dataReader[0]);
            if (this._artSeries.GetNewKey((object) int32_1) <= 0L)
            {
              intList.Add(int32_1);
              int int32_2 = BasePumpHelper.ToInt32(dataReader[1]);
              int int32_3 = BasePumpHelper.ToInt32(dataReader[2]);
              int int32_4 = BasePumpHelper.ToInt32(dataReader[6]);
              int int32_5 = BasePumpHelper.ToInt32(dataReader[3]);
              int int32_6 = BasePumpHelper.ToInt32(dataReader[6]);
              int int32_7 = BasePumpHelper.ToInt32(dataReader[7]);
              if (applicabilityCollection == null)
                applicabilityCollection = new SeriesDatesApplicabilityCollection();
              bool flag = num2 > 0 && int32_3 != num2;
              if (((num1 <= 0 ? 0 : (int32_5 != num1 ? 1 : 0)) | (flag ? 1 : 0)) != 0)
              {
                if (datesApplicability != null)
                {
                  datesApplicability.AsEditableString = str;
                  applicabilityCollection.Items.Add(datesApplicability);
                }
                str = "";
                datesApplicability = (SeriesDatesApplicability) null;
              }
              if (flag)
              {
                DictionaryValue dictionaryValue = this._articlesCache.GetValue((object) oldKey1);
                if (dictionaryValue != null)
                {
                  ArticleTag tag = dictionaryValue.Tag as ArticleTag;
                  long num3 = 0;
                  if (tag.Versions.TryGetValue(key, out num3))
                  {
                    if (this._pumpedCache.GetNewKey((object) num3) == 0L)
                    {
                      importedObjectList.Items.Clear();
                      importedObjectList.UseObject(num3);
                      List<string> stringValues = applicabilityCollection.ToStringValues();
                      for (int index = 0; index < stringValues.Count; ++index)
                        importedObjectList.AddAttribute(PumpHelper.AttrSeriesID, AttrValueType.stringVal, (object) stringValues[index], index);
                      importedObjectList.Import();
                      this._pumpedCache.AddValue((object) num3, 1L);
                      foreach (int oldKey2 in intList)
                        this._artSeries.AddValue((object) oldKey2, 1L);
                    }
                  }
                  else
                    BasePumpHelper.AppManager.AddWarningMessage($"Версия изделия (ART_ID={int32_2}, VER_ID={int32_4}) не найдена, невозможно привязать к головному изделию!");
                }
                else
                  BasePumpHelper.AppManager.AddWarningMessage($"Изделие (ART_ID={int32_2}) не найдено, невозможно привязать к головному изделию!");
                applicabilityCollection = (SeriesDatesApplicabilityCollection) null;
                datesApplicability = (SeriesDatesApplicability) null;
                intList.Clear();
              }
              long newKey = this._artFamilies.GetNewKey((object) int32_5);
              if (newKey == 0L)
              {
                BasePumpHelper.AppManager.AddWarningMessage($"Головное изделие (ID={int32_5}) не найдено, невозможно привязать серии (ART_ID={int32_2}, VER_ID={int32_4})");
              }
              else
              {
                if (datesApplicability == null)
                  datesApplicability = new SeriesDatesApplicability();
                datesApplicability.MainObjectID = newKey;
                datesApplicability.Applicability = ApplicabilityBy.Series;
                datesApplicability.Annuled = int32_7 == 1;
                if (str != "")
                  str += ",";
                if (!dataReader.IsDBNull(4))
                  str += dataReader.GetValue(4).ToString();
                str += "..";
                if (!dataReader.IsDBNull(5))
                  str += dataReader.GetValue(5).ToString();
                oldKey1 = int32_2;
                num1 = int32_5;
                num2 = int32_3;
                key = int32_6;
              }
            }
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
      this.PumpCheckPoint("Перекачка номерных изделий успешно завершена", 100);
    }
    catch (Exception ex)
    {
      BasePumpHelper.AppManager.AddWarningMessage(ex.Message);
      throw;
    }
    finally
    {
      this._pumpedCache.Release();
      this._articlesCache.Release();
      this._artSeries.Release();
      this._artFamilies.Release();
    }
  }

  public override void Pump() => this.doPump();
}
