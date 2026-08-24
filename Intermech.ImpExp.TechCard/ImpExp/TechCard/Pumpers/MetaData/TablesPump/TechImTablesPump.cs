// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.TechImTablesPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

[TaskDescription("Инициализация загрузки информации о справочниках", "Загрузка информации о справочниках")]
[TaskType(PumperType.MetaData)]
internal class TechImTablesPump : PumpClass
{
  private readonly Guid _guid = new Guid("{1D2AED85-44B4-4119-8E53-8CF995E3CEAA}");

  public TechImTablesPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация загрузки информации о справочниках ", 0);
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = " SELECT \r\n                                   a.F_ID,    a.F_NAME,    b.F_KEY AS F_TBLKEY,    b.F_TABLE FROM TC_PREDEFINED A RIGHT JOIN IM_TABLES B   ON a.F_TBLKEY = b.F_KEY WHERE b.F_KEY IN ( SELECT F_REFERENCE FROM TC_ENTITY_RF  UNION   SELECT F_TBLKEY FROM TC_PREDEFINED ) ";
    using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
    {
      int num = 0;
      try
      {
        ImTableInfoFactory tableInfoFactory = new ImTableInfoFactory(dataReader);
        while (dataReader.Read())
        {
          ImTableInfo tableInfo = tableInfoFactory.CreateItem(dataReader);
          TechPumpData.Tables.ImTablesData.Add(tableInfo);
          ++num;
        }
      }
      finally
      {
        dataReader.Close();
      }
      if (num == 0)
        this.plugin.appManager.AddErrorMessage("Ошибка таблицы IM_TABLES. Дальнейшая закачка невозможна!");
    }
    this.ExamCheckPoint("Инициализация загрузки информации о справочниках ", 100);
    TechCache.WriteOneList(TechCache.CategoryList.TechImTablesList, (object) TechPumpData.Tables.ImTablesData);
  }

  public override void Pump()
  {
    IImportingData importingData1;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      importingData1 = (IImportingData) null;
    else
      importingData1 = service.GetCache(ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseCatalogs);
    IImportingData importingData2 = importingData1;
    try
    {
      this.PumpCheckPoint("Загрузка информации о справочниках", 0);
      IUserSession userSession = TechcardConsts.Plugin.Idw.GetUserSession();
      if (userSession == null)
      {
        this.plugin.appManager.AddWarningMessage("Невозможно получить пользовательскую сессию. Это может привести к невозможности загрузки информации о справочниках.");
        this.PumpCheckPoint("Ошибка загрузки информации о справочниках!", 0);
      }
      foreach (ImTableInfo imTableInfo in TechPumpData.Tables.ImTablesData.GetAllTableInfo())
      {
        int tableKey = imTableInfo.TableKey;
        long newKey = ImportingDataHelper.Instance.GetNewKey(importingData2, ImportingCategory.ImbaseCatalogs, (object) tableKey);
        if (newKey != 0L)
        {
          imTableInfo.IpsObjectVersionId = newKey;
          if (userSession != null)
          {
            QuickObjectInfo objectInfo = userSession.GetObjectInfo(newKey);
            if (!objectInfo.Empty)
              imTableInfo.IpsObjectVersionGuid = objectInfo.VersionGuid;
          }
        }
      }
      this.PumpCheckPoint("Загрузка информации о справочниках", 100);
      TechCache.WriteOneList(TechCache.CategoryList.TechImTablesList, (object) TechPumpData.Tables.ImTablesData);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseCatalogs);
    }
  }
}
