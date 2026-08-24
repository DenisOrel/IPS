// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpThematicParams
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки тематических параметров", "Перекачка тематических параметров")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpThematicParams(SearchPlugin plugin) : PumpSearchClass(plugin, "TPARAMS")
{
  private Guid _guid = new Guid("{CB28FF2B-8443-407d-9304-4EB374917B55}");
  private const string _groupName = "Тематические параметры";
  private Dictionary<IThematicParamsItem, SettingsAttributeTypeItem> _thematicParamsDict;
  internal static IDictionary<int, Guid> ThematicParams;
  private SettingsGroup _sgGroup;

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this._sgGroup = new SettingsGroup("Тематические параметры", SettingsGroupType.ThematicParams);
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) this._sgGroup);
    this._sgGroup.ObjectCreated += new ObjectCreatedEventHandler(this.sgGroup_AttributesTypesCreated);
    this.ExamCheckPoint("Определение количества записей", 1);
    int tableRecordsCount = this.GetTableRecordsCount(ThematicParamsItemFactory.TableName);
    int index1 = 0;
    this._thematicParamsDict = new Dictionary<IThematicParamsItem, SettingsAttributeTypeItem>(tableRecordsCount);
    this.ExamCheckPoint("Получение данных из таблицы " + ThematicParamsItemFactory.TableName, 3);
    IDataReader sequentialDataReader1 = this.GetSequentialDataReader(ThematicParamsItemFactory.TableName, ThematicParamsItemFactory.TableColumns);
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
    IAttributeTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    string format1 = $"Чтение записи из таблицы {ThematicParamsItemFactory.TableName} ({{0}} из {{1}})";
    List<IThematicParamsItem> thematicParamsItemList = new List<IThematicParamsItem>();
    try
    {
      ThematicParamsItemFactory paramsItemFactory = new ThematicParamsItemFactory(ThematicParamsItemFactory.TableName, sequentialDataReader1, this.plugin.Idw.AppManager);
      while (sequentialDataReader1.Read())
      {
        ++index1;
        this.ExamCheckPoint(string.Format(format1, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 3, 29));
        thematicParamsItemList.Add(paramsItemFactory.NewItem(sequentialDataReader1));
      }
    }
    finally
    {
      sequentialDataReader1.Close();
    }
    string format2 = $"Обработка записи из таблицы {ThematicParamsItemFactory.TableName} ({{0}} из {{1}})";
    int index2 = 0;
    foreach (IThematicParamsItem key in thematicParamsItemList)
    {
      ++index2;
      this.ExamCheckPoint(string.Format(format2, (object) index2, (object) thematicParamsItemList.Count), this.CalculatePercent(thematicParamsItemList.Count, index2, 30, 99));
      if (service1 != null)
      {
        SettingsAttributeTypeItem sattrItem = new SettingsAttributeTypeItem(key.Label, key.UName, string.Empty, key.NewFieldType);
        if (service2 != null)
        {
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
                  Guid guid = new Guid(settingsAttribute.AttributeValue);
                  if (service2.GetByGuid(guid) != null)
                  {
                    sattrItem.AttrGuid = guid;
                    flag = true;
                    break;
                  }
                  break;
                }
              }
            }
          }
          if (!flag)
          {
            ParamSizeItemFactory paramSizeItemFactory = new ParamSizeItemFactory(key.AliasArt, key.AliasDoc);
            if (paramSizeItemFactory.TableName != string.Empty)
            {
              IDataReader sequentialDataReader2 = this.GetSequentialDataReader(paramSizeItemFactory.TableName);
              if (sequentialDataReader2 != null)
              {
                try
                {
                  key.Size = paramSizeItemFactory.GetSize(sequentialDataReader2);
                }
                finally
                {
                  sequentialDataReader2.Close();
                }
              }
              else
              {
                int num = 0;
                switch (key.NewFieldType)
                {
                  case FieldTypes.ftString:
                    num = Consts.MaxStringSize;
                    break;
                  case FieldTypes.ftInteger:
                  case FieldTypes.ftDouble:
                    num = Consts.MaxNumericSize;
                    break;
                  case FieldTypes.ftMemo:
                    num = Consts.MaxShortBlobSize;
                    break;
                }
                key.Size = num;
              }
            }
            MultiValueModes multiMode = key.LisValues.Count > 1 ? MultiValueModes.SingleValueFromList : MultiValueModes.SingleValue;
            IAttributeTypeToCreate attribute = SearchHelper.FindAttribute(service2, sattrItem, key.Label, string.Empty, key.NewFieldType, key.Size, key.Guid, (object) key.DefValue, multiMode);
            if (settings != null && settings.ContainsKey(key.Label))
            {
              SaveSettingsAttribute[] settingsAttributeArray = settings[key.Label];
              if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
              {
                foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                {
                  if (settingsAttribute.AttributeName.Equals("NEW_NAME"))
                    attribute.Name = settingsAttribute.AttributeValue;
                  if (settingsAttribute.AttributeName.Equals("FIELDTYPE"))
                    attribute.FieldType = (FieldTypes) Convert.ToInt32(settingsAttribute.AttributeValue);
                  if (settingsAttribute.AttributeName.Equals("SIZE"))
                    attribute.Size = (long) Convert.ToInt32(settingsAttribute.AttributeValue);
                }
              }
            }
          }
        }
        this._thematicParamsDict.Add(key, sattrItem);
        this._sgGroup.GroupItems.Add((ISettingsGroupItem) sattrItem);
      }
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  private void sgGroup_AttributesTypesCreated()
  {
    IMetadataInfo service1 = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    ISaveSettings service2 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    ICache service3 = ServicesManager.GetService(typeof (ICache)) as ICache;
    try
    {
      service3.DeleteCache(ImportingCategory.ThematicParams);
      IImportingData cache = service3.GetCache(ImportingCategory.ThematicParams);
      Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>(1);
      PumpThematicParams.ThematicParams = (IDictionary<int, Guid>) new Dictionary<int, Guid>(1);
      IDictionaryEnumerator enumerator1 = (IDictionaryEnumerator) this._thematicParamsDict.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        IThematicParamsItem key = (IThematicParamsItem) enumerator1.Key;
        SettingsAttributeTypeItem attributeTypeItem = (SettingsAttributeTypeItem) enumerator1.Value;
        IAttributeTypeItem byGuid = service1.AttributeTypes.GetByGuid(attributeTypeItem.AttrGuid);
        if (byGuid != null)
        {
          cache.AddValue((object) key.ParamId, (long) byGuid.ID, $"{key.AliasArt},{key.AliasDoc}");
          List<string> lisValues = key.LisValues;
          if (lisValues.Count > 0)
          {
            try
            {
              string empty1 = string.Empty;
              string empty2 = string.Empty;
              string empty3 = string.Empty;
              List<FieldTypes> convertList = new List<FieldTypes>();
              RelationalOperators[] enabledOperators = new RelationalOperators[0];
              bool computableAttribute = false;
              AttributeCacheHelper.GetAttributeTypeValues((FieldTypes) byGuid.AttrValueType, byGuid.ID, ref empty1, ref empty3, ref convertList, ref enabledOperators, ref computableAttribute, ref empty2);
              List<object> objectList = new List<object>();
              switch (empty2)
              {
                case "F_DATE_VALUE":
                  using (List<string>.Enumerator enumerator2 = lisValues.GetEnumerator())
                  {
                    while (enumerator2.MoveNext())
                    {
                      string current = enumerator2.Current;
                      if (current != string.Empty)
                      {
                        if (current.Equals(Consts.CurrentDateFunction))
                          objectList.Add((object) current);
                        else
                          objectList.Add((object) SearchHelper.GetDateValue(current));
                      }
                    }
                    break;
                  }
                case "F_INTEGER_VALUE":
                  using (List<string>.Enumerator enumerator3 = lisValues.GetEnumerator())
                  {
                    while (enumerator3.MoveNext())
                    {
                      string current = enumerator3.Current;
                      if (current != string.Empty)
                        objectList.Add((object) Convert.ToInt32(current));
                    }
                    break;
                  }
                case "F_DOUBLE_VALUE":
                  using (List<string>.Enumerator enumerator4 = lisValues.GetEnumerator())
                  {
                    while (enumerator4.MoveNext())
                    {
                      string current = enumerator4.Current;
                      if (current != string.Empty)
                        objectList.Add((object) SearchHelper.GetDoubleValue(current));
                    }
                    break;
                  }
                default:
                  using (List<string>.Enumerator enumerator5 = lisValues.GetEnumerator())
                  {
                    while (enumerator5.MoveNext())
                    {
                      string current = enumerator5.Current;
                      if (current != string.Empty)
                        objectList.Add((object) current);
                    }
                    break;
                  }
              }
              if (objectList.Count > 0)
              {
                IAttributePossibleValue[] possibleValues = byGuid.GetPossibleValues();
                int inListID = 0;
                foreach (object obj1 in objectList)
                {
                  bool flag = false;
                  if (possibleValues != null && possibleValues.Length != 0)
                  {
                    foreach (IAttributePossibleValue attributePossibleValue in possibleValues)
                    {
                      object obj2 = (object) null;
                      switch (empty2)
                      {
                        case "F_DATE_VALUE":
                          obj2 = (object) attributePossibleValue.ValueDateTime;
                          break;
                        case "F_INTEGER_VALUE":
                          obj2 = (object) attributePossibleValue.ValueInteger;
                          break;
                        case "F_DOUBLE_VALUE":
                          obj2 = (object) attributePossibleValue.ValueDouble;
                          break;
                        case "F_STRING_VALUE":
                          obj2 = (object) attributePossibleValue.ValueString;
                          break;
                      }
                      if (obj1.Equals(obj2))
                      {
                        flag = true;
                        break;
                      }
                    }
                  }
                  if (!flag)
                  {
                    byGuid.AddPossibleValue(inListID, obj1, obj1.ToString());
                    ++inListID;
                  }
                }
              }
            }
            catch (Exception ex)
            {
              this.plugin.Idw.AppManager.AddWarningMessage($"Ошибка изменения списка допустимых значений атрибута \"{byGuid.Name}\": {ex.Message}");
            }
          }
          List<SaveSettingsAttribute> settingsAttributeList1 = new List<SaveSettingsAttribute>();
          if (key.Guid != byGuid.GUID)
          {
            settingsAttributeList1.Add(new SaveSettingsAttribute("GUID", byGuid.GUID.ToString()));
          }
          else
          {
            if (byGuid.Name != key.Label)
              settingsAttributeList1.Add(new SaveSettingsAttribute("NEW_NAME", byGuid.Name));
            int num;
            if ((FieldTypes) byGuid.AttrValueType != key.NewFieldType)
            {
              List<SaveSettingsAttribute> settingsAttributeList2 = settingsAttributeList1;
              num = byGuid.AttrValueType;
              SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("FIELDTYPE", num.ToString());
              settingsAttributeList2.Add(settingsAttribute);
            }
            if (byGuid.MaxSize != key.Size)
            {
              List<SaveSettingsAttribute> settingsAttributeList3 = settingsAttributeList1;
              num = byGuid.MaxSize;
              SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("SIZE", num.ToString());
              settingsAttributeList3.Add(settingsAttribute);
            }
          }
          if (settingsAttributeList1.Count > 0)
            settings.Add(key.Label, settingsAttributeList1.ToArray());
          List<int> intList;
          PumpThematicParamsGroups.ThematicParamsInGroups.TryGetValue(key.GroupId, out intList);
          if (intList != null)
          {
            Guid thematicParamsGroup = PumpThematicParamsGroups.ThematicParamsGroups[key.GroupId];
            service1.AttributeGroups.LinkAttributeTypeToGroup(byGuid.ID, byGuid.GUID, service1.AttributeGroups.GetByGuid(thematicParamsGroup).ID);
            intList.Add(key.ParamId);
          }
          PumpThematicParams.ThematicParams.Add(key.ParamId, byGuid.GUID);
        }
      }
      if (settings.Count > 0)
        service2.SetSettings(this.SettingsName, settings);
      else
        service2.ClearSettings(this.SettingsName);
    }
    finally
    {
      service3?.ReleaseCache(ImportingCategory.ThematicParams);
      if (this._thematicParamsDict != null)
        this._thematicParamsDict.Clear();
    }
  }
}
