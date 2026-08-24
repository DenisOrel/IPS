// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute.TechMaterialGroupSubstitutePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute;

[TaskDescription("Инициализация загрузки информации о группах-заменителей материалов", "Загрузка информации о заменителях материалов")]
internal class TechMaterialGroupSubstitutePump : PumpClass
{
  private static readonly Guid ClassGuid = new Guid("{93457B45-8EEE-4143-9955-5AEEADCC94BB}");

  private void LoadSubstituteData()
  {
    IImportingData importingData1;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      importingData1 = (IImportingData) null;
    else
      importingData1 = service.GetCache(ImportingCategory.TechMaterialGroupReplaceableCache, ImportingCategory.TechMaterialGroupSubstituteCache);
    IImportingData importingData2 = importingData1;
    if (importingData2 == null)
      return;
    try
    {
      string str1 = "SELECT\r\n\r\n                  LINKS_PARENT.F_PARENTTYPE AS F_PARENT2TYPE,\r\n                  LINKS_PARENT.F_PARENTKEY  AS F_PARENT2KEY ,\r\n\r\n                  LINKS.F_PARENTTYPE,\r\n                  LINKS.F_PARENTKEY,\r\n                  LINKS.F_CHILDTYPE,\r\n                  LINKS.F_CHILDKEY ,\r\n                  MG.F_ORDER\r\n\r\n                FROM\r\n                  TP_MAT_LINKS LINKS\r\n\r\n                  LEFT JOIN\r\n                    TP_MAT_GR_D MG_D\r\n                  ON\r\n                    LINKS.F_CHILDKEY = MG_D.F_PARENTKEY\r\n                    AND MG_D.F_ENTITY = '%gmt'\r\n                  LEFT JOIN\r\n                    TP_MAT_GR   MG\r\n                  ON\r\n                    LINKS.F_CHILDKEY = MG.F_KEY\r\n\r\n                  LEFT JOIN\r\n                    TP_MAT_LINKS LINKS_PARENT\r\n                  ON\r\n                    LINKS_PARENT.F_CHILDTYPE    = LINKS.F_PARENTTYPE\r\n                    AND LINKS_PARENT.F_CHILDKEY = LINKS.F_PARENTKEY\r\n                WHERE\r\n                  LINKS.F_CHILDTYPE = 24\r\n                  AND\r\n                  ( \r\n                    MG_D.F_INT_VAL = 2\r\n                    OR\r\n                    MG_D.F_INT_VAL = 0 AND NOT MG.F_NAME LIKE 'Группа%' )";
      string[] array = ((IEnumerable<string>) new string[2]
      {
        TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.TechProc) ? TechDataBuilder<PumpClass>.GetPumpModeCond("LINKS.F_TCKEY", -2) : string.Empty,
        TechDataBuilder<PumpClass>.GetPumpModeCond("LINKS.F_SETKEY", 4)
      }).Where<string>((System.Func<string, bool>) (item => !string.IsNullOrEmpty(item))).ToArray<string>();
      if (((IEnumerable<string>) array).Any<string>())
        str1 = $"{str1} AND ({string.Join(" OR ", array)} )";
      string str2 = str1 + " ORDER BY\r\n                                LINKS.F_PARENTKEY,\r\n                                MG.F_ORDER ";
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = str2;
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_PARENT2TYPE");
        int ordinal2 = dataReader.GetOrdinal("F_PARENT2KEY");
        int ordinal3 = dataReader.GetOrdinal("F_PARENTTYPE");
        int ordinal4 = dataReader.GetOrdinal("F_PARENTKEY");
        int ordinal5 = dataReader.GetOrdinal("F_CHILDTYPE");
        int ordinal6 = dataReader.GetOrdinal("F_CHILDKEY");
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        try
        {
          while (dataReader.Read())
          {
            int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
            int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal3]);
            int int32_3 = BasePumpHelper.ToInt32(dataReader[ordinal4]);
            string objectCacheCode1 = TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(int32_3, int32_2);
            if (importingData2.GetValue(ImportingCategory.TechMaterialGroupReplaceableCache, (object) objectCacheCode1) == null)
              importingData2.AddValue(ImportingCategory.TechMaterialGroupReplaceableCache, (object) objectCacheCode1, (long) int32_1);
            int num;
            if (!dictionary.TryGetValue(objectCacheCode1, out num))
              num = 0;
            else
              ++num;
            dictionary[objectCacheCode1] = num;
            int int32_4 = BasePumpHelper.ToInt32(dataReader[ordinal5]);
            int int32_5 = BasePumpHelper.ToInt32(dataReader[ordinal6]);
            string objectCacheCode2 = TechMaterialGroupSubstituteCacheInfo.GetObjectCacheCode(int32_5, int32_4);
            if (importingData2.GetValue(ImportingCategory.TechMaterialGroupSubstituteCache, (object) objectCacheCode2) == null)
            {
              TechMaterialGroupSubstituteCacheInfo techObject = new TechMaterialGroupSubstituteCacheInfo()
              {
                ReplaceableParentType = BasePumpHelper.ToInt32(dataReader[ordinal1]),
                ReplaceableParentKey = int32_1,
                ReplaceableObjectType = int32_2,
                ReplaceableObjectKey = int32_3,
                SubstituteObjectType = int32_4,
                SubstituteObjectKey = int32_5,
                Order = num
              };
              importingData2.AddValue(ImportingCategory.TechMaterialGroupSubstituteCache, (object) objectCacheCode2, 0L, (ITagImportObject) new TechObjectTag((object) techObject));
            }
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.TechMaterialGroupReplaceableCache, ImportingCategory.TechMaterialGroupSubstituteCache);
    }
  }

  public TechMaterialGroupSubstitutePump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => TechMaterialGroupSubstitutePump.ClassGuid;

  public override void Exam()
  {
    this.ExamCheckPoint("Проверка информации о заменителях материалов ", 0);
    if (!this.TableExists("TP_MAT_GR"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TP_MAT_GR"}' не найдена.");
    else if (!this.TableExists("TP_MAT_GR_D"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TP_MAT_GR_D"}' не найдена.");
    else if (!this.TableExists("TP_MAT_LINKS"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TP_MAT_LINKS"}' не найдена.");
    else
      this.ExamCheckPoint("Проверка информации о заменителях завершена", 100);
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Загрузка информации о заменителях материалов ", 0);
    this.LoadSubstituteData();
    this.PumpCheckPoint("Загрузка информации о заменителях материалов успешно завершена", 100);
  }
}
