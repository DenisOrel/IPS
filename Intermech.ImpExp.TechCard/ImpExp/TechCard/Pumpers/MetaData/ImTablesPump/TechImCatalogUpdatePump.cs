// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ImTablesPump.TechImCatalogUpdatePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ImTablesPump;

[TaskDescription("Инициализация данных для перекачки - Корректировка справочников TechCard", "Корректировка информации в справочниках TechCard")]
[TaskType(PumperType.MetaData)]
internal class TechImCatalogUpdatePump : PumpClass
{
  private readonly Guid _guid = new Guid("{2C711119-4211-4AC4-8EE2-39B6EF0C0107}");

  public TechImCatalogUpdatePump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = false;
    this.taskPump.Repumpble = false;
  }

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация данных для перекачки - Корректировка справочников TechCard", 0);
    if (!this.TableExists("TC_PREDEFINED"))
      this.plugin.appManager.AddErrorMessage("Таблица 'TC_PREDEFINED' не найдена.");
    else if (!this.TableExists("TC_TPRECORDS"))
      this.plugin.appManager.AddErrorMessage("Таблица 'TC_TPRECORDS' не найдена.");
    else
      this.ExamCheckPoint("Инициализация данных для перекачки - Корректировка справочников TechCard", 100);
  }

  public override void Pump()
  {
    IImportingData importingData1;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      importingData1 = (IImportingData) null;
    else
      importingData1 = service.GetCache(ImportingCategory.ImbaseCatalogs, ImportingCategory.ImbaseCatalogsCreated);
    IImportingData importingData2 = importingData1;
    try
    {
      this.PumpCheckPoint("Корректировка информации в справочниках TechCard", 0);
      IUserSession userSession = TechcardConsts.Plugin.Idw.GetUserSession();
      if (userSession == null)
      {
        this.plugin.appManager.AddErrorMessage("Невозможно получить пользовательскую сессию. Это может привести к невозможности загрузки информации о справочниках.");
        this.PumpCheckPoint("Ошибка корректировка информации о справочниках!", 0);
      }
      else
      {
        IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
        command.CommandText = "SELECT R.F_RECORDID,\r\n                                           R.F_PREDEFID,\r\n                                           P.F_TBLKEY\r\n                                    FROM   TC_TPRECORDS R\r\n                                           LEFT JOIN TC_PREDEFINED P\r\n                                                  ON R.F_PREDEFID = P.F_ID\r\n                                    WHERE R.F_PREDEFID > 0 ";
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal1 = dataReader.GetOrdinal("F_RECORDID");
          int ordinal2 = dataReader.GetOrdinal("F_TBLKEY");
          int attributeId = MetaDataHelper.GetAttributeID((object) Intermech.Imbase.Consts.CreatedObjectAttGUID);
          int num = 0;
          try
          {
            while (dataReader.Read())
            {
              int int32_1 = ReaderHelper.GetInt32(dataReader, ordinal1);
              int int32_2 = ReaderHelper.GetInt32(dataReader, ordinal2);
              TechTypeInfo typeRecByRecordId = TechPumpData.TechType.TechTypeList.GetTypeRecByRecordId(int32_1);
              if (typeRecByRecordId != null && typeRecByRecordId.TypeSett.Mode != TechTypePumpMode.NotPumpType && !(typeRecByRecordId.TypeSett.ObjType == Guid.Empty))
              {
                long newKey = ImportingDataHelper.Instance.GetNewKey(importingData2, ImportingCategory.ImbaseCatalogs, (object) int32_2);
                if (newKey == 0L)
                {
                  this.plugin.appManager.AddWarningMessage("Справочник Imbase F_KEY = {imbaseCatalogKey} не найден в кэше. Привязка типа записи RecordId = {recordId} невозможна.");
                }
                else
                {
                  IDBObject dbObject = userSession.GetObject(newKey, false);
                  if (dbObject == null)
                  {
                    this.plugin.appManager.AddWarningMessage("Справочник Imbase ObjectId = {ipsCatalogObjectId} не найден в базе IPS. Привязка типа записи RecordId = {recordId} невозможна.");
                  }
                  else
                  {
                    IDBAttribute attributeById = dbObject.GetAttributeByID(attributeId);
                    if (attributeById == null || attributeById.IsNull)
                    {
                      AttributeValues attributeValues = new AttributeValues(attributeId, (object) typeRecByRecordId.TypeSett.ObjType.ToString());
                      dbObject.SetAttributesValues(new AttributeValues[1]
                      {
                        attributeValues
                      });
                      ++num;
                    }
                  }
                }
              }
            }
            this.plugin.appManager.AddInfoMessage($"Внесены корректировки в {num} справочник(а) IPS");
          }
          finally
          {
            dataReader.Close();
          }
        }
        this.PumpCheckPoint("Корректировка информации в справочниках TechCard", 100);
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseCatalogs, ImportingCategory.ImbaseCatalogsCreated);
    }
  }
}
