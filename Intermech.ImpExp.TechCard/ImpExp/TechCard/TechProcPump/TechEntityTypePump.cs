// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TechEntityTypePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

[TaskDescription("Инициализация загрузки о понятий Techcard, разнесенных по типу", "Загрузка типов понятий Techcard")]
[TaskType(PumperType.MetaData)]
internal class TechEntityTypePump(PluginClass plugin) : PumpClass(plugin)
{
  private readonly Guid _guid = new Guid("{03D37BE5-344F-4689-9A1D-7D7001E8CA77}");
  public const int CheckCount = 100;

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация типов понятий Techcard", 0);
    int count = TechPumpData.Entities.EntitiesList.Count;
    try
    {
      EntityTypeList entityTypeList = new EntityTypeList(count);
      int index = 0;
      foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values)
      {
        if (entity != null)
          entityTypeList.AddEntity(entity);
        ++index;
        if (index % 100 == 0 || index == count - 1)
          this.ExamCheckPoint($"Считывание понятий ({index} из {count})", this.CalculatePercent(count, index, 11, 99));
      }
      TechPumpData.EntTypeList = entityTypeList;
      TechCache.WriteOneList(TechCache.CategoryList.EntTypeList, (object) entityTypeList);
    }
    finally
    {
      this.ExamCheckPoint("Считывание типов понятий Techcard успешно завершено", 100);
    }
  }

  public override void Pump()
  {
  }
}
