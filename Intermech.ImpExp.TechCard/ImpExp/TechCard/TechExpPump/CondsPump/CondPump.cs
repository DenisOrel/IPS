// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.CondsPump.CondPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.CondsPump;

[TaskDescription("Инициализация перекачки условий экспертной системы", "Перекачка условий экспертной системы")]
[TaskType(PumperType.MetaData)]
internal class CondPump : TechExpBasePump
{
  private readonly Guid _guid = new Guid("{BBD93069-D1BD-4754-9D24-0B3385709938}");
  private const int CheckCount = 100;
  private readonly Dictionary<TechExpCond.TC_CTLCOND, FormulaData> _condList;
  private readonly Dictionary<int, TechExpCond.TC_CTLCONDBLOBS> _condBlobs;
  private readonly Dictionary<int, List<TechExpCond.TC_CTLCONDPARMS>> _condParams;

  protected override Guid GUID => this._guid;

  private string GetExtraSQlCond(long lastCondKey)
  {
    return lastCondKey != 0L ? "AND F_KEY > " + (object) lastCondKey : string.Empty;
  }

  public CondPump(PluginClass plugin)
    : base(plugin)
  {
    this._impExpObjType = -3;
    this._condList = new Dictionary<TechExpCond.TC_CTLCOND, FormulaData>();
    this._condBlobs = new Dictionary<int, TechExpCond.TC_CTLCONDBLOBS>();
    this._condParams = new Dictionary<int, List<TechExpCond.TC_CTLCONDPARMS>>();
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Подготовка к закачке условий экспертной системы:", 0);
    if (!this.TableExists("TC_CTLCOND"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TC_CTLCOND"}' не найдена.");
    else if (!this.TableExists("TC_CTLCONDBLOBS"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TC_CTLCONDBLOBS"}' не найдена.");
    else if (!this.TableExists("TC_CTLCONDPARMS"))
    {
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TC_CTLCONDPARMS"}' не найдена.");
    }
    else
    {
      this.ExamCheckPoint("Проверка таблиц успешно завершена", 3);
      string extraSqlCond = this.GetExtraSQlCond(0L);
      string sqlText = string.Format(TechExpCond.SQL.CondParams, (object) extraSqlCond);
      int recordsCount = this.GetRecordsCount(string.Format(TechExpCond.SQL.CondParamsCount, (object) extraSqlCond));
      int index = 0;
      IDataReader dataReader = this.GetDataReader(sqlText);
      try
      {
        TechExpCond.TC_CTLCONDPARMS.ParseSchema(this.GetTableColumns(dataReader));
        while (dataReader.Read())
        {
          ++index;
          TechExpCond.TC_CTLCONDPARMS tcCtlcondparms = new TechExpCond.TC_CTLCONDPARMS(dataReader);
          if (!TechPumpData.Entities.EntitiesList.ContainsKey(tcCtlcondparms.fCode))
            this.plugin.appManager.AddWarningMessage($"Понятие '{tcCtlcondparms.fCode}' не найдено для условия TC_CTLCOND = {tcCtlcondparms.fCondKey}.");
          if (index % 100 == 0 || index == recordsCount)
            this.ExamCheckPoint($"Анализ параметров условий экспертной системы ({index} из {recordsCount})", this.CalculatePercent(recordsCount, index, 4, 99));
        }
      }
      finally
      {
        dataReader.Close();
      }
      this.ExamCheckPoint("Подготовка к закачке условий экспертной системы: успешно завершена", 100);
    }
  }

  protected override IDataReader GetDataReader(string sqlText, CommandBehavior commandBehavior)
  {
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = sqlText;
    return command.ExecuteReader(commandBehavior);
  }

  protected override IDataReader GetBehaviorDataReader(
    string tableName,
    string tableColumns,
    CommandBehavior commandBehavior)
  {
    if (!this.TableExists(tableName))
      return (IDataReader) null;
    string sqlText = $"SELECT {tableColumns} FROM {tableName.ToUpper()}";
    if (commandBehavior == CommandBehavior.SchemaOnly)
      sqlText += " WHERE 1=0";
    return this.GetDataReader(sqlText, commandBehavior);
  }

  protected override bool NeedPumpExpData() => true;

  protected override void LoadExpertObjData()
  {
    string extraSqlCond = this.GetExtraSQlCond(this._lastObjId);
    this.PumpCheckPoint("Считывание структуры условий", 0);
    string sqlText1 = string.Format(TechExpCond.SQL.Cond, (object) extraSqlCond);
    int recordsCount1 = this.GetRecordsCount(string.Format(TechExpCond.SQL.CondCount, (object) extraSqlCond));
    int index1 = 0;
    List<TechExpCond.TC_CTLCOND> tcCtlcondList = new List<TechExpCond.TC_CTLCOND>();
    IDataReader dataReader1 = this.GetDataReader(sqlText1);
    try
    {
      TechExpCond.TC_CTLCOND.ParseSchema(this.GetTableColumns(dataReader1));
      while (dataReader1.Read())
      {
        ++index1;
        TechExpCond.TC_CTLCOND key = new TechExpCond.TC_CTLCOND(dataReader1);
        this._condList.Add(key, (FormulaData) null);
        tcCtlcondList.Add(key);
        if (index1 % 100 == 0 || index1 == recordsCount1)
          this.PumpCheckPoint($"Считывание условий экспертной системы ({index1} из {recordsCount1})", this.CalculatePercent(recordsCount1, index1, 1, 10));
      }
    }
    finally
    {
      dataReader1.Close();
    }
    string sqlText2 = string.Format(TechExpCond.SQL.CondBlob, (object) extraSqlCond, (object) extraSqlCond);
    int recordsCount2 = this.GetRecordsCount(string.Format(TechExpCond.SQL.CondBlobCount, (object) extraSqlCond, (object) extraSqlCond));
    int index2 = 0;
    IDataReader dataReader2 = this.GetDataReader(sqlText2);
    try
    {
      TechExpCond.TC_CTLCONDBLOBS.ParseSchema(this.GetTableColumns(dataReader2));
      while (dataReader2.Read())
      {
        ++index2;
        TechExpCond.TC_CTLCONDBLOBS tcCtlcondblobs = new TechExpCond.TC_CTLCONDBLOBS(dataReader2);
        this._condBlobs.Add(tcCtlcondblobs.fKey, tcCtlcondblobs);
        if (index2 % 100 == 0 || index2 == recordsCount2)
          this.PumpCheckPoint($"Считывание структуры условий экспертной системы ({index2} из {recordsCount2})", this.CalculatePercent(recordsCount2, index2, 11, 20));
      }
    }
    finally
    {
      dataReader2.Close();
    }
    string sqlText3 = string.Format(TechExpCond.SQL.CondParams, (object) extraSqlCond);
    int recordsCount3 = this.GetRecordsCount(string.Format(TechExpCond.SQL.CondParamsCount, (object) extraSqlCond));
    int index3 = 0;
    IDataReader dataReader3 = this.GetDataReader(sqlText3);
    try
    {
      TechExpCond.TC_CTLCONDPARMS.ParseSchema(this.GetTableColumns(dataReader3));
      while (dataReader3.Read())
      {
        ++index3;
        TechExpCond.TC_CTLCONDPARMS tcCtlcondparms = new TechExpCond.TC_CTLCONDPARMS(dataReader3);
        List<TechExpCond.TC_CTLCONDPARMS> tcCtlcondparmsList;
        if (!this._condParams.TryGetValue(tcCtlcondparms.fCondKey, out tcCtlcondparmsList))
        {
          tcCtlcondparmsList = new List<TechExpCond.TC_CTLCONDPARMS>();
          this._condParams.Add(tcCtlcondparms.fCondKey, tcCtlcondparmsList);
        }
        tcCtlcondparmsList?.Add(tcCtlcondparms);
        if (index3 % 100 == 0 || index3 == recordsCount3)
          this.PumpCheckPoint($"Считывание параметров условий экспертной системы ({index3} из {recordsCount3})", this.CalculatePercent(recordsCount3, index3, 21, 30));
      }
    }
    finally
    {
      dataReader3.Close();
    }
    int index4 = 0;
    foreach (TechExpCond.TC_CTLCOND key in tcCtlcondList)
    {
      ++index4;
      FormulaData formulaData = new FormulaData((short) key.fResType);
      TechExpCond.TC_CTLCONDBLOBS tcCtlcondblobs;
      if (this._condBlobs.TryGetValue(key.fCond, out tcCtlcondblobs))
      {
        short length = (short) tcCtlcondblobs.fBlob.Length;
        MemoryStream memoryStream = new MemoryStream((int) length + 2);
        BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream);
        binaryWriter.Write(length);
        binaryWriter.Write(tcCtlcondblobs.fBlob);
        formulaData.Data.Load_Raw(new BinaryReader((Stream) memoryStream, Encoding.Default)
        {
          BaseStream = {
            Position = 0L
          }
        });
      }
      if (this._condBlobs.TryGetValue(key.fCondCmp, out tcCtlcondblobs))
      {
        short length = (short) tcCtlcondblobs.fBlob.Length;
        MemoryStream memoryStream = new MemoryStream((int) length + 2);
        BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream);
        binaryWriter.Write(length);
        binaryWriter.Write(tcCtlcondblobs.fBlob);
        formulaData.CData.Load_Raw(new BinaryReader((Stream) memoryStream, Encoding.Default)
        {
          BaseStream = {
            Position = 0L
          }
        });
      }
      List<TechExpCond.TC_CTLCONDPARMS> tcCtlcondparmsList;
      if (this._condParams.TryGetValue(key.fKey, out tcCtlcondparmsList) && tcCtlcondparmsList != null)
      {
        foreach (TechExpCond.TC_CTLCONDPARMS tcCtlcondparms in tcCtlcondparmsList)
          formulaData.ID.Add(tcCtlcondparms.fCode);
      }
      this._condList[key] = formulaData;
      if (index4 % 500 == 0 || index4 == recordsCount3 - 1)
        this.PumpCheckPoint($"Загрузка условий экспертной системы ({index4} из {tcCtlcondList.Count})", this.CalculatePercent(tcCtlcondList.Count, index4, 31 /*0x1F*/, 40));
    }
    this._condBlobs.Clear();
    this._condParams.Clear();
    this.PumpCheckPoint("Считывание и загрузка условий завершена успешно", 41);
  }

  protected override void PumpExpertObjData()
  {
    this.PumpCheckPoint("Обработка условий экспертной системы", 42);
    int num = 0;
    int index = 0;
    int count = this._condList.Keys.Count;
    if (ServicesManager.GetService(typeof (ICache)) is ICache service)
      this._importingData = service.GetCache(ImportingCategory.ImbaseFolders, ImportingCategory.TechCeh, ImportingCategory.ImbaseTableLinksKeyToObjectID, ImportingCategory.TechExpObjStruct);
    Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaPump formulaPump1 = new Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaPump(this.plugin);
    formulaPump1._importingData = this._importingData;
    Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaPump formulaPump2 = formulaPump1;
    try
    {
      foreach (KeyValuePair<TechExpCond.TC_CTLCOND, FormulaData> cond in this._condList)
      {
        if (cond.Key == null || cond.Value == null)
          ++index;
        else if (this._importingData?.GetValue(ImportingCategory.TechExpObjStruct, (object) cond.Key.fKey) != null)
        {
          ++index;
        }
        else
        {
          FormulaData formulaData = cond.Value;
          TempFormula ipsFormulaData = (TempFormula) null;
          try
          {
            formulaPump2.ConvertExpertData((short) cond.Key.fResType, formulaData.Data, formulaData.ID, out ipsFormulaData);
          }
          catch (Exception ex)
          {
            switch (ex)
            {
              case TokenConvertException _:
              case CommonDataTypeCheckFailException _:
              case CommonDataTypeConvertException _:
              case EntitySettNotExistException _:
              case FormulaConvertException _:
                ipsFormulaData = (TempFormula) null;
                this.plugin.appManager.AddWarningMessage(ex.Message);
                break;
              case FormulaCompileException _:
                this.plugin.appManager.AddWarningMessage(ex.Message);
                break;
              default:
                throw;
            }
          }
          if (this._importingData != null && ipsFormulaData != null)
          {
            TechObjectTag tag = new TechObjectTag((object) ipsFormulaData);
            this._importingData.AddValue(ImportingCategory.TechExpObjStruct, (object) cond.Key.fKey, -1L, (ITagImportObject) tag);
          }
          ++index;
          if (index % 500 == 0 || index == count - 1)
            this.PumpCheckPoint($"Обработка условий экспертной системы ({index} из {count})", this.CalculatePercent(count, index, 43, 99));
        }
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseFolders, ImportingCategory.TechCeh, ImportingCategory.ImbaseTableLinksKeyToObjectID, ImportingCategory.TechExpObjStruct);
    }
    this.PumpCheckPoint(num.Equals(0) ? "Закачка условий экспертной системы завершена успешно" : $"Закачка условий экспертной системы завершена c ошибками, \"{num}\" условий не закачано", 100);
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._condList.Clear();
    this._condBlobs.Clear();
    this._condParams.Clear();
  }
}
