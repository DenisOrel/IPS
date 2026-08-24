// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TechEntityFixPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

[TaskDescription("Инициализация загрузки фиксированных понятий Techcard", "Загрузка понятий Techcard")]
[TaskType(PumperType.MetaData)]
internal class TechEntityFixPump(PluginClass plugin) : PumpClass(plugin)
{
  private readonly Guid _guid = new Guid("{1D1AED35-47B4-4239-8E53-8CF285C3CFAA}");
  private string TableName = "TC_ENTITY_FX";
  public const int CheckCount = 100;

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация считывания фиксированных понятий Techcard", 0);
    int tableRecordsCount = this.GetTableRecordsCount(this.TableName);
    IDataReader defaultDataReader = this.GetDefaultDataReader(this.TableName);
    try
    {
      TechEntityFix.ParseSchema(this.GetTableColumns(defaultDataReader));
      TechEntFixList techEntFixList = new TechEntFixList();
      int index = 0;
      while (defaultDataReader.Read())
      {
        TechEntityFix techEntFix = TechEntityFix.Parse(defaultDataReader);
        if (techEntFix != null)
          techEntFixList.AddEntity(techEntFix);
        ++index;
        if (index % 100 == 0 || index == tableRecordsCount)
          this.ExamCheckPoint($"Считывание понятий ({index} из {tableRecordsCount})", this.CalculatePercent(tableRecordsCount, index, 11, 99));
      }
      TechPumpData.EntFixList = techEntFixList;
      TechCache.WriteOneList(TechCache.CategoryList.TechEntFixList, (object) techEntFixList);
    }
    finally
    {
      defaultDataReader.Close();
      this.ExamCheckPoint("Считывание фиксированных понятий из базы Techcard успешно завершено", 100);
    }
  }

  public override void Pump()
  {
  }
}
