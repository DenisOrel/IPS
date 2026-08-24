// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpExternalProgs
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Tools;
using Intermech.Tools.LaunchActions;
using Intermech.Tools.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки списка внешних программ в инструменты IPS", "Перекачка списка внешних программ в инструменты IPS")]
[TaskType(PumperType.MetaData)]
internal class PumpExternalProgs(SearchPlugin plugin) : PumpSearchClass(plugin, "EX_PROGS")
{
  private const string _tableName = "EXTENS";
  private readonly ExtAppSettingsValidator extAppValidator = new ExtAppSettingsValidator();
  private readonly ExtAppSettingsCodec extAppCodec = new ExtAppSettingsCodec();
  private readonly ShellVerbSettingsValidator shellVerbValidator = new ShellVerbSettingsValidator();
  private readonly ShellVerbSettingsCodec shellVerbCodec = new ShellVerbSettingsCodec();

  protected override Guid GUID => new Guid("52327112-D7BB-4d31-89F8-E8642FEE1FA7");

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ExternalProgs, ImportingCategory.Users, ImportingCategory.DocTypes);
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    ILaunchActionServer customService = (ILaunchActionServer) userSession.GetCustomService(typeof (ILaunchActionServer));
    try
    {
      this.PumpCheckPoint("Определение количества запмсей о внешних программах", 1);
      int tableRecordsCount = this.GetTableRecordsCount("EXTENS");
      int index1 = 0;
      string format = "Закачка списка внешних программ в инструменты IPS ({0} из {1})";
      IDataReader sequentialDataReader = this.GetSequentialDataReader("EXTENS");
      int num1 = 0;
      try
      {
        Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader);
        int i1 = tableColumns["EXT_ID"];
        int i2 = tableColumns["USER_ID"];
        int i3 = tableColumns["EXTENSION"];
        int i4 = tableColumns["PROG_NAME"];
        int i5 = tableColumns["PROG_PARAM"];
        int i6 = tableColumns["PROG_DIR"];
        int i7 = tableColumns["PROG_TYPE"];
        int i8 = tableColumns["RUN_STYLE"];
        int i9 = tableColumns["PROTOTYPE"];
        int i10 = tableColumns["DOC_TYPE"];
        int i11 = tableColumns["DDECOMMAND"];
        int i12 = tableColumns["EXECTYPE"];
        while (sequentialDataReader.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 2));
          int int32_1 = BasePumpHelper.ToInt32(sequentialDataReader[i1]);
          if (cache.GetNewKey(ImportingCategory.ExternalProgs, (object) int32_1) == 0L)
          {
            long userId = 0;
            int oldKey = sequentialDataReader.IsDBNull(i2) ? -1 : BasePumpHelper.ToInt32(sequentialDataReader[i2]);
            if (oldKey != -1)
            {
              userId = cache.GetNewKey(ImportingCategory.Users, (object) oldKey);
              if (userId == 0L)
              {
                this.plugin.appManager.AddWarningMessage($"Среди импортированных не найден пользователь SEARCH с идентификатором {oldKey}");
                continue;
              }
            }
            if (!sequentialDataReader.IsDBNull(i3))
            {
              sequentialDataReader.GetString(i3).Trim();
            }
            else
            {
              string empty1 = string.Empty;
            }
            string fileName = sequentialDataReader.IsDBNull(i4) ? string.Empty : sequentialDataReader.GetString(i4).Trim();
            string str1 = sequentialDataReader.IsDBNull(i5) ? string.Empty : sequentialDataReader.GetString(i5).Trim();
            string str2 = sequentialDataReader.IsDBNull(i6) ? string.Empty : sequentialDataReader.GetString(i6).Trim();
            string str3 = sequentialDataReader.IsDBNull(i7) ? string.Empty : sequentialDataReader.GetString(i7).Trim();
            switch (this.plugin.imConnection.DataBaseType)
            {
              case "IntermechConnection.Oracle":
                int int16_1 = (int) Convert.ToInt16(sequentialDataReader.GetValue(i8));
                break;
              case "IntermechConnection.MsSQL":
              case "IntermechConnection.Interbase":
                int int16_2 = (int) sequentialDataReader.GetInt16(i8);
                break;
            }
            if (!sequentialDataReader.IsDBNull(i9))
            {
              sequentialDataReader.GetString(i9).Trim();
            }
            else
            {
              string empty2 = string.Empty;
            }
            int int32_2 = sequentialDataReader.IsDBNull(i10) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader[i10]);
            long num2 = 0;
            if (int32_2 > 0)
            {
              num2 = cache.GetNewKey(ImportingCategory.DocTypes, (object) int32_2);
              if (num2 == 0L)
              {
                this.plugin.appManager.AddWarningMessage($"Среди импортированных не найден тип документов SEARCH с идентификатором {int32_2}");
                continue;
              }
            }
            string str4 = sequentialDataReader.IsDBNull(i11) ? string.Empty : sequentialDataReader.GetString(i11).Trim();
            if (sequentialDataReader.IsDBNull(i12))
            {
              this.plugin.appManager.AddWarningMessage($"Не указан тип для идентификатора внешней программы {int32_1} (таблица {"EXTENS"})");
            }
            else
            {
              short num3 = -1;
              switch (this.plugin.imConnection.DataBaseType)
              {
                case "IntermechConnection.Oracle":
                  num3 = sequentialDataReader.IsDBNull(i12) ? (short) -1 : Convert.ToInt16(sequentialDataReader.GetValue(i12));
                  break;
                case "IntermechConnection.MsSQL":
                case "IntermechConnection.Interbase":
                  num3 = sequentialDataReader.IsDBNull(i12) ? (short) -1 : sequentialDataReader.GetInt16(i12);
                  break;
              }
              Guid empty3 = Guid.Empty;
              XmlDocument xmlDocument;
              Guid handlerId;
              switch (num3)
              {
                case 0:
                  ExtAppSettings extAppSettings = new ExtAppSettings();
                  if (fileName != string.Empty)
                  {
                    try
                    {
                      extAppSettings.ApplicationName = new FileInfo(fileName).Name;
                    }
                    catch
                    {
                      extAppSettings.ApplicationName = fileName;
                    }
                  }
                  extAppSettings.Arguments = str1;
                  extAppSettings.Executable = fileName;
                  extAppSettings.WorkDirectory = str2;
                  extAppSettings.WindowStyle = ProcessWindowStyle.Normal;
                  try
                  {
                    this.extAppValidator.Validate((ISettingsObject) extAppSettings, SettingsValidatorContext.SettingsObjectOnly);
                  }
                  catch (Exception ex)
                  {
                    this.plugin.appManager.AddWarningMessage($"Ошибка при перекачке информации о внешней программе {int32_1} (таблица {"EXTENS"}): {ex.Message}");
                    continue;
                  }
                  xmlDocument = this.extAppCodec.Encode((ISettingsObject) extAppSettings);
                  handlerId = ExtAppSettings.HandlerId;
                  break;
                case 1:
                  ShellVerbSettings shellVerbSettings = new ShellVerbSettings();
                  shellVerbSettings.Verb = str4;
                  try
                  {
                    this.shellVerbValidator.Validate((ISettingsObject) shellVerbSettings, SettingsValidatorContext.SettingsObjectOnly);
                  }
                  catch (Exception ex)
                  {
                    this.plugin.appManager.AddWarningMessage($"Ошибка при перекачке информации о внешней программе {int32_1} (таблица {"EXTENS"}): {ex.Message}");
                    continue;
                  }
                  xmlDocument = this.shellVerbCodec.Encode((ISettingsObject) shellVerbSettings);
                  handlerId = ShellVerbSettings.HandlerId;
                  break;
                default:
                  this.plugin.appManager.AddWarningMessage($"Неверный тип {num3} внешней программы {int32_1} (таблица {"EXTENS"})");
                  continue;
              }
              IDBObjectType objectType = userSession.GetObjectType(Convert.ToInt32(num2));
              ITarget target;
              if (userId == 0L)
              {
                target = (ITarget) AllUsersTarget.Value;
              }
              else
              {
                if (!(cache.GetTag(ImportingCategory.Users, (object) oldKey) is UserTag tag))
                {
                  this.plugin.appManager.AddWarningMessage($"Не найден GUID пользователя в кэше (идентификатор в базе IPS {userId} ) при перекачке информации по внешней программе {int32_1} (таблица {"EXTENS"})");
                  continue;
                }
                target = (ITarget) new UserTarget(userId, tag.Guid);
              }
              List<LaunchType> launchTypeList = new List<LaunchType>();
              switch (str3)
              {
                case "U":
                  launchTypeList.Add(LaunchType.Edit);
                  launchTypeList.Add(LaunchType.Print);
                  launchTypeList.Add(LaunchType.View);
                  break;
                case "P":
                  launchTypeList.Add(LaunchType.Print);
                  break;
                case "V":
                  launchTypeList.Add(LaunchType.View);
                  break;
                case "E":
                  launchTypeList.Add(LaunchType.Edit);
                  break;
              }
              if (launchTypeList.Count == 0)
              {
                this.plugin.appManager.AddWarningMessage($"Неизвестный тип \"{str3}\" внешней программы {int32_1} (таблица {"EXTENS"})");
              }
              else
              {
                for (int index2 = 0; index2 < launchTypeList.Count; ++index2)
                  customService.CreateAction((objectType as IDBGuid).GUID, target, launchTypeList[index2], handlerId, xmlDocument.OuterXml);
                cache.AddValue(ImportingCategory.ExternalProgs, (object) int32_1, 1L);
                ++num1;
              }
            }
          }
        }
      }
      finally
      {
        sequentialDataReader.Close();
      }
      this.plugin.appManager.AddInfoMessage("Добавлено запмсей о внешних программах в инструменты IPS: " + num1.ToString());
      this.PumpCheckPoint("Перекачка данных успешно завершена", 100);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ExternalProgs, ImportingCategory.Users, ImportingCategory.DocTypes);
    }
  }
}
