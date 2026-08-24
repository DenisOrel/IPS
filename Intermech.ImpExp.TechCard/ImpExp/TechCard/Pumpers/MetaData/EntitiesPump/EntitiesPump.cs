// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitiesPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

[TaskDescription("Инициализация перекачки понятий Techcard", "Перекачка понятий Techcard")]
[TaskType(PumperType.MetaData)]
internal class EntitiesPump : PumpClass
{
  private const int CheckCount = 100;
  private readonly Guid _guid = new Guid("{7B514101-5D13-4531-A5EF-69E8D6F09AC1}");

  public EntitiesPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override IDataReader GetBehaviorDataReader(
    string tableName,
    string tableColumns,
    CommandBehavior commandBehavior)
  {
    if (!this.TableExists(tableName))
      return (IDataReader) null;
    IDbCommand command = this.plugin.idb.CreateCommand();
    command.CommandText = $"SELECT {tableColumns} FROM {tableName.ToUpper()}";
    if (commandBehavior == CommandBehavior.SchemaOnly)
      command.CommandText += " WHERE 1=0";
    return command.ExecuteReader(commandBehavior);
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация считывания понятий TechCard", 0);
    EntityProductionList entProdlist = new EntityProductionList();
    IDataReader defaultDataReader1 = this.GetDefaultDataReader("TC_ENTITY_PR");
    try
    {
      EntityProduction.ParseSchema(this.GetTableColumns(defaultDataReader1));
      while (defaultDataReader1.Read())
      {
        EntityProduction entityProduction = EntityProduction.Parse(defaultDataReader1);
        if (entityProduction != null && entityProduction.Production != 0)
        {
          List<int> intList;
          if (entProdlist.TryGetValue(entityProduction.Code, out intList))
          {
            if (!intList.Contains(entityProduction.Production))
              intList.Add(entityProduction.Production);
          }
          else
          {
            intList = new List<int>();
            intList.Add(entityProduction.Production);
            entProdlist.Add(entityProduction.Code, intList);
          }
        }
      }
      if (entProdlist.Count == 0)
        this.plugin.appManager.AddWarningMessage($"Таблица \"{"TC_ENTITY_PR"}\" пуста");
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Невозможно прочитать таблицу \"{"TC_ENTITY_PR"}\". Данные о применяемости понятий в видах производства будут утеряны! Ошибка: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    finally
    {
      defaultDataReader1.Close();
      TechPumpData.Entities.EntityProductionList = entProdlist;
    }
    int tableRecordsCount1 = this.GetTableRecordsCount("TC_ENTITY_RF");
    IDataReader defaultDataReader2 = this.GetDefaultDataReader("TC_ENTITY_RF");
    try
    {
      int index = 0;
      HashSet<string> stringSet = new HashSet<string>();
      List<string> list = new List<string>();
      EntityReference.ParseSchema(this.GetTableColumns(defaultDataReader2));
      while (defaultDataReader2.Read())
      {
        EntityReference entityReference = EntityReference.Parse(defaultDataReader2);
        if (entityReference != null)
        {
          TechPumpData.Entities.EntityRefDataList.Add(entityReference);
          if (stringSet.Contains(entityReference.Code))
            list.Add(entityReference.Code);
          stringSet.Add(entityReference.Code);
        }
        ++index;
        if (index % 100 == 0 || index == tableRecordsCount1)
          this.ExamCheckPoint($"Считывание привязок понятий ({index} из {tableRecordsCount1})", this.CalculatePercent(tableRecordsCount1, index, 0, 100));
      }
      TechCache.WriteOneList(TechCache.CategoryList.EnityRefDirectoreList, (object) TechPumpData.Entities.EntityRefDataList);
      if (list.Count != 0)
      {
        GenericListHelper.MakeUnique<string>(list);
        this.plugin.appManager.AddErrorMessage($"В таблице \"{"TC_ENTITY_RF"}\" найдены не уникальные понятия: {string.Join(",", list.ToArray())}");
      }
      this.ExamCheckPoint("Считывание привязок понятий из базы TechCard успешно завершено", 100);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddErrorMessage($"Невозможно прочесть таблицу \"{"TC_ENTITY_RF"}\": {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    finally
    {
      defaultDataReader2.Close();
    }
    int tableRecordsCount2 = this.GetTableRecordsCount("TC_ENTITY");
    IDataReader defaultDataReader3 = this.GetDefaultDataReader("TC_ENTITY");
    try
    {
      int index = 0;
      Entity.ParseSchema(this.GetTableColumns(defaultDataReader3));
      HashSet<string> stringSet = new HashSet<string>();
      while (defaultDataReader3.Read())
      {
        Entity entity = Entity.Parse(defaultDataReader3);
        if (entity != null)
        {
          entity.InitializeProduction(entProdlist);
          entity.InitializeSetting((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values, true);
          if (!TechPumpData.Entities.EntitiesList.ContainsKey(entity.Code))
            TechPumpData.Entities.EntitiesList.Add(entity.Code, entity);
          if (stringSet.Contains(entity.Code))
            this.plugin.appManager.AddWarningMessage($"Для понятия \"{entity.Code}\" найдено несколько записей в таблице {"TC_ENTITY"}");
          else
            stringSet.Add(entity.Code);
        }
        ++index;
        if (index % 100 == 0 || index == tableRecordsCount2)
          this.ExamCheckPoint($"Считывание понятий ({index} из {tableRecordsCount2})", this.CalculatePercent(tableRecordsCount2, index, 0, 100));
      }
      this.ExamCheckPoint("Считывание понятий из базы TechCard успешно завершено", 100);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddErrorMessage($"Невозможно прочитать таблицу \"{"TC_ENTITY"}\": {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    finally
    {
      defaultDataReader3.Close();
    }
  }

  public override void Pump()
  {
  }

  protected override Guid GUID => this._guid;
}
