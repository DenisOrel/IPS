// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.Tp2LinkPump.Tp2Obj2LinkPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Data.Obj2Link;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.Tp2LinkPump;

[TaskDescription("Инициализация загрузки информации о связях ТП с изделиями", "Загрузка информации о связях ТП")]
internal class Tp2Obj2LinkPump : PumpClass
{
  public static readonly Guid ClassGuid = new Guid("{961FB550-F8C7-48F4-94F4-F4FC6A4A4002}");

  private void LoadLinkData()
  {
    string format = "SELECT \r\n                                {0}, {1}, {2}, {3}\r\n                               FROM \r\n                                 {4}\r\n                               WHERE \r\n                                 {2} = 1 \r\n                                 {5}";
    string str1 = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.TechProc) ? TechDataBuilder<PumpClass>.GetPumpModeCond("F_OBJ_KEY", -2) : string.Empty;
    if (str1 != string.Empty)
      str1 = " AND " + str1;
    string str2 = string.Format(format, (object[]) new string[6]
    {
      "F_KEY",
      "F_OBJ_KEY",
      "F_OBJ_TYPE",
      "F_ART_TCKEY",
      "TC_OBJ2LINK",
      str1
    });
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = str2;
    using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
    {
      int ordinal1 = dataReader.GetOrdinal("F_KEY");
      int ordinal2 = dataReader.GetOrdinal("F_OBJ_KEY");
      int ordinal3 = dataReader.GetOrdinal("F_OBJ_TYPE");
      int ordinal4 = dataReader.GetOrdinal("F_ART_TCKEY");
      if (TechPumpData.TechObjects.Tp2LinkList == null)
        TechPumpData.TechObjects.Tp2LinkList = new Dictionary<long, List<Obj2LinkInfoObject>>();
      else
        TechPumpData.TechObjects.Tp2LinkList.Clear();
      try
      {
        while (dataReader.Read())
        {
          int int32 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
          Obj2LinkInfoObject obj2LinkInfoObject = new Obj2LinkInfoObject(BasePumpHelper.ToInt32(dataReader[ordinal1]), int32, (byte) BasePumpHelper.ToInt32(dataReader[ordinal3]), BasePumpHelper.ToInt32(dataReader[ordinal4]));
          List<Obj2LinkInfoObject> obj2LinkInfoObjectList;
          if (!TechPumpData.TechObjects.Tp2LinkList.TryGetValue((long) int32, out obj2LinkInfoObjectList))
          {
            obj2LinkInfoObjectList = new List<Obj2LinkInfoObject>();
            TechPumpData.TechObjects.Tp2LinkList[(long) int32] = obj2LinkInfoObjectList;
          }
          obj2LinkInfoObjectList.Add(obj2LinkInfoObject);
        }
      }
      finally
      {
        dataReader.Close();
      }
    }
  }

  public Tp2Obj2LinkPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => Tp2Obj2LinkPump.ClassGuid;

  public override void Exam()
  {
    this.ExamCheckPoint("Проверка информации о связях ТП", 0);
    if (!this.TableExists("TC_OBJ2LINK"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TC_OBJ2LINK"}' не найдена.");
    else
      this.ExamCheckPoint("Проверка информации о связях ТП успешно завершена", 100);
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Загрузка информации о связях ТП", 0);
    this.LoadLinkData();
    this.PumpCheckPoint("Загрузка информации о связях ТП успешно завершена", 100);
    TechCache.WriteOneList(TechCache.CategoryList.TechTp2ObjLinkList, (object) TechPumpData.TechObjects.Tp2LinkList);
  }
}
