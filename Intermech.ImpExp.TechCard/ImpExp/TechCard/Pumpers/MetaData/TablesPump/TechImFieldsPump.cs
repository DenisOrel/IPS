// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.TechImFieldsPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

[TaskDescription("Инициализация загрузки информации о полях справочников", "Загрузка информации о полях справочников")]
[TaskType(PumperType.MetaData)]
internal class TechImFieldsPump : PumpClass
{
  private readonly Guid _guid = new Guid("{48EA7755-5285-4D4F-A5FE-39CD43CC38C1}");

  public TechImFieldsPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация загрузки информации о полях справочников ", 0);
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = " SELECT \r\n                                   A.*  FROM IM_FIELDS A   WHERE A.F_TABLE_ID IN ( SELECT F_REFERENCE FROM TC_ENTITY_RF  UNION   SELECT F_TBLKEY FROM TC_PREDEFINED ) ";
    using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
    {
      int num = 0;
      try
      {
        ImFieldInfoFactory fieldInfoFactory = new ImFieldInfoFactory(dataReader);
        while (dataReader.Read())
        {
          ImFieldInfo fieldInfo = fieldInfoFactory.CreateItem(dataReader);
          TechPumpData.Tables.ImFieldsData.Add(fieldInfo);
          ++num;
        }
      }
      finally
      {
        dataReader.Close();
      }
      if (num == 0)
        this.plugin.appManager.AddErrorMessage("Ошибка таблицы IM_FIELDS. Дальнейшая закачка невозможна!");
    }
    this.ExamCheckPoint("Инициализация загрузки информации о полях справочников ", 100);
    TechCache.WriteOneList(TechCache.CategoryList.TechImFieldsList, (object) TechPumpData.Tables.ImFieldsData);
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Загрузка информации о полях справочников", 0);
    if (TechcardConsts.Plugin.Idw.GetUserSession() == null)
    {
      this.plugin.appManager.AddWarningMessage("Невозможно получить пользовательскую сессию. Это может привести к невозможности загрузки информации о справочниках.");
      this.PumpCheckPoint("Ошибка загрузки информации о полях справочников!", 0);
    }
    foreach (ImFieldInfo imFieldInfo in TechPumpData.Tables.ImFieldsData.GetAllFieldInfo())
      ;
    this.PumpCheckPoint("Загрузка информации о полях справочников", 100);
    TechCache.WriteOneList(TechCache.CategoryList.TechImFieldsList, (object) TechPumpData.Tables.ImFieldsData);
  }
}
