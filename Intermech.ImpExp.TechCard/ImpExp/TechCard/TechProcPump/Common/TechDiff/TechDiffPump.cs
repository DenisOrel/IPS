// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff.TechDiffPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.Pools;
using Intermech.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;

[TaskDescription("Инициализация загрузки информации о групповых ТП", "Загрузка информации о групповых ТП")]
internal class TechDiffPump : PumpClass
{
  private readonly Guid _guid = new Guid("{1D2BED85-46B4-4119-8E53-8CF115E3CEBA}");

  public TechDiffPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
    TechDiffCache.DiffPumper = this;
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Подготовка к закачке данных ГТП:", 0);
    if (!this.TableExists("TP_DIFF_VALUE"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TP_DIFF_VALUE"}' не найдена.");
    else
      this.ExamCheckPoint("Проверка таблиц успешно завершена", 100);
  }

  public override void Pump()
  {
  }

  protected override Guid GUID => this._guid;

  public void LoadDiffData(int lastTpRecKey, params int[] recordIds)
  {
    TechDiffCache.DiffRecList = new TechDiffRecList();
    if (recordIds.Length == 0 || ((IEnumerable<int>) recordIds).All<int>((System.Func<int, bool>) (item => item <= 0)))
      return;
    this.LoadDiffData($"F_RECORDID IN ({string.Join<int>(",", (IEnumerable<int>) recordIds)}) AND   F_TPRECKEY >= {(object) lastTpRecKey}");
  }

  public void LoadDiffData(string condition)
  {
    TechDiffCache.DiffRecList = new TechDiffRecList();
    string str1 = " SELECT COUNT(*) FROM TP_DIFF_VALUE";
    string str2 = string.Format(" SELECT DVAL.*,  \r\n                                                      DOC.{3}\r\n                                                   FROM \r\n                                                       {0} DVAL\r\n                                                       LEFT JOIN\r\n                                                       {1} VER\r\n                                                       ON\r\n                                                       DVAL.{4} = 1 AND\r\n                                                       DVAL.{5} = VER.{7} \r\n                                                       LEFT JOIN\r\n                                                       {2} DOC\r\n                                                       ON \r\n                                                       VER.{6} = DOC.{7}", (object) "TP_DIFF_VALUE", (object) "TP_VERSIONS", (object) "TC_ARCDOCS", (object) "F_PRODUCTION", (object) "F_ENT_TYPE", (object) "F_DOCTCKEY", (object) "F_TCKEY", (object) "F_KEY");
    string str3 = string.Empty;
    if (condition != string.Empty)
      str3 = $"{str3} WHERE {condition}";
    string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond("F_DOCTCKEY", -2);
    if (pumpModeCond != string.Empty)
      str3 = !(condition != string.Empty) ? $"{str3} WHERE {pumpModeCond}" : $"{str3} AND {pumpModeCond}";
    int recordsCount = this.GetRecordsCount(str1 + str3);
    this.PumpCheckPoint("Закачка информации об отличиях в ГТП", 0);
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      command.CommandText = str2 + str3 + $" ORDER BY    \r\n                                                   {"F_DOCTCKEY"},\r\n                                                   {"F_ARTTCKEY"},\r\n                                                   {"F_TPRECKEY"},\r\n                                                   {"F_RECORDID"},\r\n                                                   {"F_ENTITY"},\r\n                                                   {"F_ROW"}";
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_KEY");
        int ordinal2 = dataReader.GetOrdinal("F_DOCTCKEY");
        int ordinal3 = dataReader.GetOrdinal("F_ARTTCKEY");
        int ordinal4 = dataReader.GetOrdinal("F_TPRECKEY");
        int ordinal5 = dataReader.GetOrdinal("F_ENTITY");
        int ordinal6 = dataReader.GetOrdinal("F_ROW");
        int ordinal7 = dataReader.GetOrdinal("F_VALUE");
        int ordinal8 = dataReader.GetOrdinal("F_NUM_VALUE");
        int ordinal9 = dataReader.GetOrdinal("F_ENT_TYPE");
        int ordinal10 = dataReader.GetOrdinal("F_RECORDID");
        int ordinal11 = dataReader.GetOrdinal("F_PRODUCTION");
        int val1 = 0;
        int key = 0;
        int docTcKey = 0;
        int artTcKey = 0;
        int tpRecKey = 0;
        int num1 = 0;
        string entity = string.Empty;
        int entType = 0;
        int num2 = -1;
        double numValue = 0.0;
        using (ObjectPoolScope<StringBuilder> objectPoolScope = TextServices.StringBuilderPool.Allocate(2048 /*0x0800*/))
        {
          StringBuilder stringBuilder = objectPoolScope.Object;
          while (dataReader.Read())
          {
            int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
            int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
            int int32_3 = BasePumpHelper.ToInt32(dataReader[ordinal3]);
            int int32_4 = BasePumpHelper.ToInt32(dataReader[ordinal4]);
            int int32_5 = BasePumpHelper.ToInt32(dataReader[ordinal10]);
            string str4 = dataReader.IsDBNull(ordinal5) ? string.Empty : dataReader.GetString(ordinal5);
            int int32_6 = BasePumpHelper.ToInt32(dataReader[ordinal9]);
            int int32_7 = BasePumpHelper.ToInt32(dataReader[ordinal6]);
            string str5 = dataReader.IsDBNull(ordinal7) ? string.Empty : dataReader.GetString(ordinal7);
            int productionId = dataReader.IsDBNull(ordinal11) ? -1 : Convert.ToInt32(dataReader.GetValue(ordinal11));
            int digits = 8;
            if (productionId != -1)
              digits = TechPumpData.Configs.MaxDigitsAfter(productionId);
            double num3 = dataReader.IsDBNull(ordinal8) ? 0.0 : Math.Round(Convert.ToDouble(dataReader.GetValue(ordinal8)), digits);
            if (int32_2 == docTcKey && int32_3 == artTcKey && int32_4 == tpRecKey && int32_5 == num1 && str4 == entity)
            {
              if (int32_3 != 0 && int32_7 != num2)
              {
                stringBuilder.Append(str5);
                numValue = num3;
              }
            }
            else
            {
              if (key != 0)
                TechDiffCache.DiffRecList.Add(key, tpRecKey, docTcKey, artTcKey, entity, 0, stringBuilder.ToString(), numValue, entType);
              key = int32_1;
              docTcKey = int32_2;
              artTcKey = int32_3;
              tpRecKey = int32_4;
              num1 = int32_5;
              entity = str4;
              entType = int32_6;
              num2 = int32_7;
              stringBuilder.Clear();
              stringBuilder.Append(str5);
              numValue = num3;
            }
            ++val1;
            if (val1 % 500 == 0 || val1 == recordsCount - 1)
              this.ExamCheckPoint($"Закачка информации об отличиях ГТП ({val1} из {recordsCount})", this.CalculatePercent(recordsCount, Math.Min(val1, recordsCount), 0, 100));
          }
          dataReader.Close();
          if (key != 0)
            TechDiffCache.DiffRecList.Add(key, tpRecKey, docTcKey, artTcKey, entity, 0, stringBuilder.ToString(), numValue, entType);
        }
      }
    }
    this.ExamCheckPoint("Закачка информации об отличиях в ГТП успешно завершена", 100);
  }
}
