// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypePumper
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

[TaskDescription("Инициализация загрузки типов объектов Techcard", "Загрузка типов объектов Techcard")]
[TaskType(PumperType.MetaData)]
internal class TechTypePumper : PumpClass
{
  private const int CheckCount = 100;
  private readonly Guid _guid = new Guid("{4DE6AEA7-246B-4f6b-893D-B7E75018F620}");
  private string _tableName = "TC_TPRECORDS";

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

  private void LoadTypes()
  {
    int tableRecordsCount = this.GetTableRecordsCount(this._tableName);
    IDataReader defaultDataReader = this.GetDefaultDataReader(this._tableName);
    try
    {
      TechTypeRecord.ParseSchema(this.GetTableColumns(defaultDataReader));
      int index = 0;
      while (defaultDataReader.Read())
      {
        TechTypeInfo typeInfo = TechTypeRecord.Parse(defaultDataReader);
        if (typeInfo != null)
          TechPumpData.TechType.TechTypeList.AddType(typeInfo);
        ++index;
        if (index % 100 == 0 || index == tableRecordsCount)
          this.ExamCheckPoint($"Загрузки типа ({index} из {tableRecordsCount})", this.CalculatePercent(tableRecordsCount, index, 11, 99));
      }
    }
    finally
    {
      defaultDataReader.Close();
    }
  }

  public TechTypePumper(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация загрузки типов объектов Techcard", 0);
    try
    {
      this.LoadTypes();
    }
    finally
    {
      this.ExamCheckPoint("Считывание загрузки типов объектов из базы Techcard успешно завершено", 100);
    }
  }

  public override void Pump()
  {
  }
}
