// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpThematicParamsGroups
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки групп тематических параметров", "Перекачка групп тематических параметров")]
[TaskType(PumperType.MetaData)]
internal class PumpThematicParamsGroups(SearchPlugin plugin) : PumpSearchClass(plugin, "TP_GROUPS")
{
  private Guid _guid = new Guid("{370993EB-A5FB-405b-AB53-F05BADECD964}");
  private Dictionary<IThematicParamsGroupItem, SettingsAttributeGroupItem> _thematicParamsGroupsDict;
  internal static IDictionary<int, Guid> ThematicParamsGroups;
  internal static IDictionary<int, List<int>> ThematicParamsInGroups;

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    SettingsGroup settingsGroup = new SettingsGroup("Группы тематических параметров", SettingsGroupType.ThematicParamsGroups);
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) settingsGroup);
    settingsGroup.ObjectCreated += new ObjectCreatedEventHandler(this.sgGroup_AttributesTypesCreated);
    this.ExamCheckPoint("Определение количества записей", 0);
    int tableRecordsCount = this.GetTableRecordsCount(ThematicParamsGroupItemFactory.TableName);
    int index = 0;
    this._thematicParamsGroupsDict = new Dictionary<IThematicParamsGroupItem, SettingsAttributeGroupItem>(tableRecordsCount);
    this.ExamCheckPoint("Получение данных из таблицы " + ThematicParamsGroupItemFactory.TableName, 1);
    IDataReader sequentialDataReader = this.GetSequentialDataReader(ThematicParamsGroupItemFactory.TableName, ThematicParamsGroupItemFactory.TableColumns);
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
    try
    {
      string format = $"Обработка записи из таблицы {ThematicParamsGroupItemFactory.TableName} ({{0}} из {{1}})";
      ThematicParamsGroupItemFactory groupItemFactory = new ThematicParamsGroupItemFactory(ThematicParamsGroupItemFactory.TableName, sequentialDataReader, this.plugin.Idw.AppManager);
      IAttributeGroupToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IAttributeGroupToCreateList)) as IAttributeGroupToCreateList;
      while (sequentialDataReader.Read())
      {
        ++index;
        this.ExamCheckPoint(string.Format(format, (object) index, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index, 2, 49));
        IThematicParamsGroupItem key = groupItemFactory.NewItem(sequentialDataReader);
        SettingsAttributeGroupItem attributeGroupItem = new SettingsAttributeGroupItem(key.Label);
        bool flag = false;
        if (settings != null && settings.ContainsKey(key.Label))
        {
          SaveSettingsAttribute[] settingsAttributeArray = settings[key.Label];
          if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
          {
            foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
            {
              if (settingsAttribute.AttributeName.Equals("GUID"))
              {
                attributeGroupItem.AttrGuid = new Guid(settingsAttribute.AttributeValue);
                flag = true;
                break;
              }
            }
          }
        }
        if (!flag && service2 != null)
        {
          IAttributeGroupToCreate byName = service2.GetByName(key.Label);
          if (byName != null)
          {
            attributeGroupItem.AttrGuid = byName.GUID;
          }
          else
          {
            IAttributeGroupToCreate attributeGroupToCreate = service2.AddItem(true, key.Label, key.Guid, long.MaxValue);
            if (settings != null && settings.ContainsKey(key.Label))
            {
              SaveSettingsAttribute[] settingsAttributeArray = settings[key.Label];
              if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
              {
                foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                {
                  if (settingsAttribute.AttributeName.Equals("NEW_NAME"))
                    attributeGroupToCreate.Name = settingsAttribute.AttributeValue;
                  if (settingsAttribute.AttributeName.Equals("NOTE"))
                    attributeGroupToCreate.Note = settingsAttribute.AttributeValue;
                }
              }
            }
            attributeGroupItem.AttrGuid = key.Guid;
          }
        }
        settingsGroup.GroupItems.Add((ISettingsGroupItem) attributeGroupItem);
        this._thematicParamsGroupsDict.Add(key, attributeGroupItem);
      }
    }
    finally
    {
      sequentialDataReader.Close();
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  private void sgGroup_AttributesTypesCreated()
  {
    IMetadataInfo service1 = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>(1);
    PumpThematicParamsGroups.ThematicParamsGroups = (IDictionary<int, Guid>) new Dictionary<int, Guid>(1);
    PumpThematicParamsGroups.ThematicParamsInGroups = (IDictionary<int, List<int>>) new Dictionary<int, List<int>>(1);
    if (!(ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) is ISaveSettings service2))
      return;
    ICache service3 = ServicesManager.GetService(typeof (ICache)) as ICache;
    try
    {
      service3.DeleteCache(ImportingCategory.ThematicParamsGroup);
      IImportingData cache = service3.GetCache(ImportingCategory.ThematicParamsGroup);
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this._thematicParamsGroupsDict.GetEnumerator();
      while (enumerator.MoveNext())
      {
        IThematicParamsGroupItem key = (IThematicParamsGroupItem) enumerator.Key;
        SettingsAttributeGroupItem attributeGroupItem = (SettingsAttributeGroupItem) enumerator.Value;
        List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>(1);
        IAttributeGroupItem byGuid = service1.AttributeGroups.GetByGuid(attributeGroupItem.AttrGuid);
        if (key != null && byGuid != null)
        {
          cache.AddValue((object) key.GroupId, (long) byGuid.ID);
          if (key.Guid != byGuid.GUID)
          {
            settingsAttributeList.Add(new SaveSettingsAttribute("GUID", byGuid.GUID.ToString()));
          }
          else
          {
            if (byGuid.Name != key.Label)
              settingsAttributeList.Add(new SaveSettingsAttribute("NEW_NAME", byGuid.Name));
            if (byGuid.Note != key.Note)
              settingsAttributeList.Add(new SaveSettingsAttribute("NOTE", byGuid.Note));
          }
          PumpThematicParamsGroups.ThematicParamsGroups.Add(key.GroupId, byGuid.GUID);
          if (settingsAttributeList.Count > 0)
            settings.Add(key.Label, settingsAttributeList.ToArray());
          PumpThematicParamsGroups.ThematicParamsInGroups.Add(key.GroupId, new List<int>(1));
        }
      }
      if (settings.Count > 0)
        service2.SetSettings(this.SettingsName, settings);
      else
        service2.ClearSettings(this.SettingsName);
    }
    finally
    {
      service3?.ReleaseCache(ImportingCategory.ThematicParamsGroup);
      if (this._thematicParamsGroupsDict != null)
        this._thematicParamsGroupsDict.Clear();
    }
  }
}
