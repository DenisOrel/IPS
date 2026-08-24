// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs.TechConfigPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs;

[TaskDescription("Инициализация загрузки информации о конфигурациях TechCard", "Загрузка информации о конфигурациях TechCard")]
[TaskType(PumperType.MetaData)]
internal class TechConfigPump(PluginClass plugin) : PumpClass(plugin)
{
  private readonly Guid _guid = new Guid("{D2A3DA82-7A43-48F3-BBBC-A5712BA40136}");

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация загрузки информации о конфигурациях", 0);
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = " SELECT * FROM TC_CONFIGS";
    using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
    {
      int num = 0;
      try
      {
        TechConfigInfoFactory configInfoFactory = new TechConfigInfoFactory(dataReader);
        while (dataReader.Read())
        {
          TechConfigInfo configInfo = configInfoFactory.CreateItem(dataReader);
          TechPumpData.Configs.Cache.Add(configInfo);
          ++num;
        }
      }
      finally
      {
        dataReader.Close();
      }
      if (num == 0)
        this.plugin.appManager.AddErrorMessage("Ошибка таблицы TC_CONFIGS. Дальнейшая закачка невозможна!");
    }
    this.ExamCheckPoint("Инициализация загрузки информации о конфигурациях ", 100);
    TechCache.WriteOneList(TechCache.CategoryList.TechConfigData, (object) TechPumpData.Configs.Cache);
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Загрузка информации о конфигурациях", 0);
    this.PumpCheckPoint("Загрузка информации о конфигурациях", 100);
  }
}
