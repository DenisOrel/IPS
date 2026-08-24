// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpArchives
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.Controls;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.IO;
using Intermech.Signs.Interfaces;
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки архивов", "Перекачка данных об архивах")]
[TaskType(PumperType.MetaData)]
public class PumpArchives : PumpSearchClass
{
  private const string _groupName = "Параметры архивов";
  private Dictionary<TypeAttributeItem, SettingsAttributeTypeItem> _archiveAttributesDict = new Dictionary<TypeAttributeItem, SettingsAttributeTypeItem>();
  private int _simpleRelationID = -1;
  private bool _ignoreBindChanged;
  private const string _messageKey = "ARCH";
  private SettingsGroup _sgGroup;

  public PumpArchives(SearchPlugin plugin)
    : base(plugin, "ARCHIVES")
  {
    this._simpleRelationID = plugin.Imdi.RelationTypes.GetByGuid(new Guid("cad00022-306c-11d8-b4e9-00304f19f545")).ID;
    (ServicesManager.GetService(typeof (ISettingsGroupService)) as ISettingsGroupService).ItemBindChangedEvent += new ItemBindChangedEventHandler(this.settingsGroup_ItemBindChangedEvent);
  }

  private void OnItemBindChangedInThisGroup(ItemBindChangedEventArgs e)
  {
    if (this._ignoreBindChanged)
      return;
    switch (IMMessageBox.Show("Настройка параметров архивов", "Применить данную настройку для других архивов?", new IMMessageBoxButton[3]
    {
      new IMMessageBoxButton(Intermech.Consts.YesValue, DialogResult.Yes),
      new IMMessageBoxButton(Intermech.Consts.NoValue, DialogResult.No),
      new IMMessageBoxButton("Больше не спрашивать", DialogResult.Cancel)
    }, IMMessageBoxImage.Question))
    {
      case DialogResult.Cancel:
        this._ignoreBindChanged = true;
        break;
      case DialogResult.Yes:
        int num1 = 0;
        if (e.Group.GroupItems == null)
          break;
        foreach (ISettingsGroupItem groupItem in e.Group.GroupItems)
        {
          if (groupItem.SettingsItems != null)
          {
            ISettingsItem settingsItem = groupItem.SettingsItems.Find((Predicate<ISettingsItem>) (x => x.LongName.Equals(e.Item.LongName)));
            if (settingsItem != null)
            {
              settingsItem.AttrGuid = e.Item.AttrGuid;
              if (settingsItem.Error != null)
                settingsItem.Error = e.Item.Error == null ? (ItemError) null : e.Item.Error.Clone();
              else if (e.Item.Error != null)
                settingsItem.Error = e.Item.Error.Clone();
              ++num1;
              this.CheckAttributes4ObjTypes((ISettingsAttributeTypeItem) settingsItem, (int) groupItem.Tag, groupItem.Caption, -1);
            }
          }
        }
        if (num1 <= 0)
          break;
        int num2 = (int) MessageBox.Show($"Произведено {num1} привязок.", "Настройка параметров архивов", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        break;
    }
  }

  private void settingsGroup_ItemBindChangedEvent(object sender, ItemBindChangedEventArgs e)
  {
    switch (e.Group.GroupType)
    {
      case SettingsGroupType.Archives:
        this.OnItemBindChangedInThisGroup(e);
        break;
      case SettingsGroupType.ArticleTypes:
      case SettingsGroupType.DocTypes:
        using (List<ISettingsGroupItem>.Enumerator enumerator = this._sgGroup.GroupItems.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ISettingsGroupItem current = enumerator.Current;
            foreach (ISettingsAttributeTypeItem settingsItem in current.SettingsItems)
              this.CheckAttributes4ObjTypes(settingsItem, (int) current.Tag, current.Caption, ((SettingsObjectTypeItem) e.Item).ID);
          }
          break;
        }
    }
  }

  protected override Guid GUID => new Guid("B8EF55A5-F7A4-4279-8E9D-AF899BD26EDB");

  private List<int> GetDocTypesForArchive(ISettingsAttributeTypeItem item, int archiveID)
  {
    List<int> docTypesForArchive;
    if (item.Tag != null && item.Tag is List<int>)
    {
      docTypesForArchive = (List<int>) item.Tag;
    }
    else
    {
      IDataReader dataReader = this.GetDataReader($"select t.doc_type from doclist t where t.archive_id={archiveID} group by t.doc_type");
      docTypesForArchive = new List<int>();
      try
      {
        while (dataReader.Read())
          docTypesForArchive.Add(Convert.ToInt32(dataReader[0]));
        item.Tag = (object) docTypesForArchive;
      }
      finally
      {
        dataReader.Close();
      }
    }
    return docTypesForArchive;
  }

  private void CheckAttributes4ObjTypes(
    ISettingsAttributeTypeItem item,
    int archiveID,
    string archiveCaption,
    int changedTypeID)
  {
    List<int> docTypesForArchive = this.GetDocTypesForArchive(item, archiveID);
    if (changedTypeID != -1 && !docTypesForArchive.Contains(changedTypeID))
      return;
    IAttributeTypeItem byGuid = this.plugin.Imdi.AttributeTypes.GetByGuid(item.AttrGuid);
    foreach (int oldDocTypeID in docTypesForArchive)
      this.CheckAttribute4ObjType(oldDocTypeID, byGuid, item, archiveCaption);
  }

  private void CheckAttribute4ObjType(
    int oldDocTypeID,
    IAttributeTypeItem attrType,
    ISettingsAttributeTypeItem item,
    string archiveCaption)
  {
    ISettingsGroup settingsGroup = (ServicesManager.GetService(typeof (ISettingsGroupService)) as ISettingsGroupService).Groups.Find((Predicate<ISettingsGroup>) (x => x.GroupType == SettingsGroupType.DocTypes));
    if (settingsGroup == null || settingsGroup.GroupItems == null || !(settingsGroup.GroupItems.Find((Predicate<ISettingsGroupItem>) (y => ((SettingsObjectTypeItem) y).ID == oldDocTypeID)) is SettingsObjectTypeItem settingsObjectTypeItem) || settingsObjectTypeItem.AttrGuid == Guid.Empty)
      return;
    string errorKey = "ARCH" + oldDocTypeID.ToString();
    if (item.Error != null)
      item.Error.ErrorMessages.RemoveAll((Predicate<MessageItem>) (x => x.Key.Equals(errorKey)));
    IObjectTypeItem byGuid = this.plugin.Imdi.ObjectTypes.GetByGuid(settingsObjectTypeItem.AttrGuid);
    if (byGuid == null || byGuid.AnyAttribute || attrType != null && byGuid.AttrTypeExists(attrType.ID))
      return;
    string str = $"Архив {archiveCaption} содержит атрибут {(attrType != null ? (object) attrType.Name : (object) item.LongName)} который нельзя добавить объекту типа {byGuid.Name} (в SEARCH: {settingsObjectTypeItem.LongName})";
    if (item.Error == null)
      item.Error = new ItemError(ItemErrorType.Error, errorKey, str);
    else
      item.Error.AddMessage(ItemErrorType.Error, errorKey, str);
  }

  public override void Exam()
  {
    Dictionary<int, IArchivesItem> dictionary = (Dictionary<int, IArchivesItem>) null;
    this._sgGroup = new SettingsGroup("Параметры архивов", SettingsGroupType.Archives);
    if (ServicesManager.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) this._sgGroup);
    this._sgGroup.ObjectCreated += new ObjectCreatedEventHandler(this._sgGroup_ObjectCreated);
    ICache service2 = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service2.GetCache(ImportingCategory.Archives);
    try
    {
      this.ExamCheckPoint("Проверка наличия атрибута для архивов", 0);
      this.plugin.CheckIdAttribute(this.plugin.NameSearchIdArchive, this.plugin.GuidSearchIdArchive, FieldTypes.ftInteger);
      this.ExamCheckPoint("Определение количества записей", 1);
      int tableRecordsCount = this.GetTableRecordsCount(ArchivesItemFactory.TableName);
      int index1 = 0;
      dictionary = new Dictionary<int, IArchivesItem>(tableRecordsCount);
      this.ExamCheckPoint("Получение данных из таблицы " + ArchivesItemFactory.TableName, 1);
      IDataReader sequentialDataReader = this.GetSequentialDataReader(ArchivesItemFactory.TableName, ArchivesItemFactory.TableColumns);
      try
      {
        string format = $"Обработка записи из таблицы {ArchivesItemFactory.TableName} ({{0}} из {{1}})";
        ArchivesItemFactory archivesItemFactory = new ArchivesItemFactory(sequentialDataReader, this.plugin.Idw.AppManager);
        while (sequentialDataReader.Read())
        {
          ++index1;
          this.ExamCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 2, 10));
          IArchivesItem archivesItem = archivesItemFactory.NewItem(sequentialDataReader);
          if (cache.GetNewKey(ImportingCategory.Archives, (object) archivesItem.ArchiveID) == 0L)
            cache.AddValue(ImportingCategory.Archives, (object) archivesItem.ArchiveID, long.MinValue, archivesItem.Descriptio, (ITagImportObject) new Archive(archivesItem.ArchiveID, archivesItem.StrongSign, archivesItem.PersonId, archivesItem.ParentID, archivesItem.ChkRights, archivesItem.StorageId, archivesItem.SignStamp, archivesItem.Descriptio, archivesItem.Alias, archivesItem.FileName));
          dictionary.Add(archivesItem.ArchiveID, archivesItem);
          this.plugin.archSettings.AddArchive(archivesItem.Descriptio, archivesItem.ArchiveID);
        }
      }
      finally
      {
        sequentialDataReader.Close();
      }
      this.ExamCheckPoint("Получение данных из базы", 11);
      IAttributeTypeToCreateList service3 = ServicesManager.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
      Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
      int count = dictionary.Count;
      int index2 = 0;
      foreach (IArchivesItem archivesItem in dictionary.Values)
      {
        SettingsGroupItem settingsGroupItem = new SettingsGroupItem(archivesItem.Descriptio)
        {
          Tag = (object) archivesItem.ArchiveID
        };
        string format = "Получение информации о таблице архива ({0} из {1})";
        ++index2;
        this.ExamCheckPoint(string.Format(format, (object) index2, (object) count), this.CalculatePercent(count, index2, 12, 99));
        IDataReader defaultDataReader = this.GetDefaultDataReader(archivesItem.Alias);
        if (defaultDataReader == null)
        {
          this.plugin.appManager.AddWarningMessage($"Таблицы базы данных SEARCH \"{archivesItem.Alias}\" укананной для архива \"{archivesItem.Descriptio}\" не существует!");
        }
        else
        {
          try
          {
            foreach (DataRow row in (InternalDataCollectionBase) defaultDataReader.GetSchemaTable().Rows)
            {
              string str1 = Convert.ToString(row["ColumnName"]);
              if (!str1.Equals("DOC_ID"))
              {
                string str2 = archivesItem.CfgData.ContainsKey(str1) ? archivesItem.CfgData[str1] : str1;
                int size;
                FieldTypes typeFromSchemaRow = PumpAttributesHelper.GetFieldTypeFromSchemaRow(row, out size);
                SettingsAttributeTypeItem sattrItem = new SettingsAttributeTypeItem(str2, str2, str1, typeFromSchemaRow);
                TypeAttributeItem key1 = new TypeAttributeItem(archivesItem.ArchiveID, str1, str2, typeFromSchemaRow, size);
                bool flag = false;
                string key2 = this.SetSaveSettingsName((ITypeAttributeItem) key1);
                if (settings != null && settings.ContainsKey(key2))
                {
                  SaveSettingsAttribute[] settingsAttributeArray = settings[key2];
                  if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
                  {
                    foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                    {
                      if (settingsAttribute.AttributeName.Equals("GUID"))
                      {
                        Guid guid = new Guid(settingsAttribute.AttributeValue);
                        if (service3.GetByGuid(guid) != null)
                        {
                          sattrItem.AttrGuid = guid;
                          key1.GUID = guid;
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
                  IAttributeTypeToCreate attribute = SearchHelper.FindAttribute(service3, sattrItem, key1.AttributeName, key1.DBFieldName, key1.AttributeType, key1.AttributeSize, key1.GUID, key1.DefaultValue, MultiValueModes.SingleValue);
                  if (settings != null && settings.ContainsKey(key1.AttributeName))
                  {
                    SaveSettingsAttribute[] settingsAttributeArray = settings[key1.AttributeName];
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
                this._archiveAttributesDict.Add(key1, sattrItem);
                settingsGroupItem.SettingsItems.Add((ISettingsItem) sattrItem);
              }
            }
            this._sgGroup.GroupItems.Add((ISettingsGroupItem) settingsGroupItem);
          }
          finally
          {
            defaultDataReader.Close();
          }
          foreach (ISettingsAttributeTypeItem settingsItem in settingsGroupItem.SettingsItems)
            this.CheckAttributes4ObjTypes(settingsItem, (int) settingsGroupItem.Tag, settingsGroupItem.Caption, -1);
        }
      }
      this.plugin.archSettings.RefreshData();
      this.ExamCheckPoint("Проверка успешно завершена", 100);
    }
    finally
    {
      service2.ReleaseCache(ImportingCategory.Archives);
      dictionary?.Clear();
    }
  }

  private string SetSaveSettingsName(ITypeAttributeItem item)
  {
    return $"{item.TypeID.ToString()}|{item.AttributeName}";
  }

  private void _sgGroup_ObjectCreated()
  {
    IMetadataInfo service1 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    ISaveSettings service2 = ServicesManager.GetService(typeof (ISaveSettings)) as ISaveSettings;
    ICache service3 = ServicesManager.GetService(typeof (ICache)) as ICache;
    try
    {
      service3.DeleteCache(ImportingCategory.ArchiveParameters);
      IImportingData cache = service3.GetCache(ImportingCategory.ArchiveParameters);
      Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>(1);
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this._archiveAttributesDict.GetEnumerator();
      while (enumerator.MoveNext())
      {
        TypeAttributeItem key = (TypeAttributeItem) enumerator.Key;
        SettingsAttributeTypeItem attributeTypeItem = (SettingsAttributeTypeItem) enumerator.Value;
        IAttributeTypeItem byGuid = service1.AttributeTypes.GetByGuid(attributeTypeItem.AttrGuid);
        cache.AddValue(ImportingCategory.ArchiveParameters, (object) $"{key.TypeID}.{key.DBFieldName.ToLower()}", (long) byGuid.ID, byGuid.GUID.ToString());
        List<SaveSettingsAttribute> settingsAttributeList1 = new List<SaveSettingsAttribute>();
        if (key.GUID != byGuid.GUID)
        {
          settingsAttributeList1.Add(new SaveSettingsAttribute("GUID", byGuid.GUID.ToString()));
          key.GUID = byGuid.GUID;
        }
        else
        {
          if (byGuid.Name != key.AttributeName)
            settingsAttributeList1.Add(new SaveSettingsAttribute("NEW_NAME", byGuid.Name));
          int num;
          if ((FieldTypes) byGuid.AttrValueType != key.AttributeType)
          {
            List<SaveSettingsAttribute> settingsAttributeList2 = settingsAttributeList1;
            num = byGuid.AttrValueType;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("FIELDTYPE", num.ToString());
            settingsAttributeList2.Add(settingsAttribute);
          }
          if (byGuid.MaxSize != key.AttributeSize)
          {
            List<SaveSettingsAttribute> settingsAttributeList3 = settingsAttributeList1;
            num = byGuid.MaxSize;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("SIZE", num.ToString());
            settingsAttributeList3.Add(settingsAttribute);
          }
          if (settingsAttributeList1.Count > 0)
            settingsAttributeList1.Add(new SaveSettingsAttribute("GUID", byGuid.GUID.ToString()));
        }
        if (settingsAttributeList1.Count > 0)
          settings.Add(this.SetSaveSettingsName((ITypeAttributeItem) key), settingsAttributeList1.ToArray());
      }
      if (settings.Count > 0)
        service2.SetSettings(this.SettingsName, settings);
      else
        service2.ClearSettings(this.SettingsName);
    }
    finally
    {
      service3?.ReleaseCache(ImportingCategory.ArchiveParameters);
      if (this._archiveAttributesDict != null)
        this._archiveAttributesDict.Clear();
    }
  }

  public override void Pump()
  {
    List<string> stringList = new List<string>();
    ICache service1 = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service1.GetCache(ImportingCategory.Archives, ImportingCategory.ArchivesTree, ImportingCategory.RankList, ImportingCategory.ArchiveParameters);
    try
    {
      Dictionary<object, DictionaryValue> category1 = cacheData.GetCategory(ImportingCategory.ArchiveParameters);
      SortedList<int, List<Guid>> sortedList = new SortedList<int, List<Guid>>();
      foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category1)
      {
        string[] strArray = ((string) keyValuePair.Key).Split('.');
        if (strArray != null && strArray.Length == 2)
        {
          int int32 = Convert.ToInt32(strArray[0]);
          Guid guid = new Guid(keyValuePair.Value.Caption);
          List<Guid> guidList = (List<Guid>) null;
          if (sortedList.TryGetValue(int32, out guidList))
          {
            guidList.Add(guid);
          }
          else
          {
            guidList = new List<Guid>(1);
            guidList.Add(guid);
            sortedList.Add(int32, guidList);
          }
        }
      }
      int id1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad0011e-306c-11d8-b4e9-00304f19f545")).ID;
      int id2 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00120-306c-11d8-b4e9-00304f19f545")).ID;
      int id3 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00121-306c-11d8-b4e9-00304f19f545")).ID;
      int id4 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0001c-306c-11d8-b4e9-00304f19f545")).ID;
      int id5 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
      int id6 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00148-306c-11d8-b4e9-00304f19f545")).ID;
      int id7 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0005f-306c-11d8-b4e9-00304f19f545")).ID;
      int id8 = this.plugin.Imdi.AttributeTypes.GetByName(this.plugin.NameSearchIdArchive).ID;
      int id9 = this.plugin.Imdi.RelationTypes.GetByGuid(this.plugin.reltypeSimpleGuid).ID;
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid("cad00141-306c-11d8-b4e9-00304f19f545"));
      List<object> valuesDescriptions = attributeType.PossibleValuesDescriptions;
      List<object> possibleValues = attributeType.PossibleValues;
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      Dictionary<object, DictionaryValue> category2 = cacheData.GetCategory(ImportingCategory.Archives);
      this.PumpCheckPoint("Определение количества архивов", 0);
      if (category2 == null)
      {
        this.plugin.appManager.AddInfoMessage($"Создано архивов: {0}");
        this.PumpCheckPoint("Перекачка данных успешно завершена", 100);
      }
      else
      {
        int count = category2.Count;
        int index1 = 0;
        string format1 = "Закачка данных об архивах: {0} из {1}";
        int i = 0;
        IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
        List<int> importArchives = new List<int>(SearchHelper.PacketSize);
        int importedCount = 0;
        iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          for (int index2 = 0; index2 < iolIm.Items.Count; ++index2)
          {
            if (iolIm.Items[index2].Object.Object_id != 0L)
            {
              cacheData.SetNewKey(ImportingCategory.Archives, (object) importArchives[index2], iolIm.Items[index2].Object.Object_id);
              ++importedCount;
            }
            else
              this.plugin.appManager.AddWarningMessage($"Архив {importArchives[index2]} не импортирован. См. серверный лог.");
          }
          importArchives.Clear();
        });
        List<PumpArchives.ArchiveTreeItem> archiveTreeItemList = new List<PumpArchives.ArchiveTreeItem>();
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category2)
        {
          int int32 = Convert.ToInt32(keyValuePair.Key);
          ++index1;
          this.PumpCheckPoint(string.Format(format1, (object) index1, (object) count), this.CalculatePercent(count, index1, 1, 80 /*0x50*/));
          if (keyValuePair.Value.NewObjectID == long.MinValue)
          {
            Archive tag = keyValuePair.Value.Tag as Archive;
            int objType = tag.PersonId != 0 ? id3 : id2;
            iolIm.AddObject(objType, tag.PersonId, tag.Descriptio);
            iolIm.AddAttribute(id8, AttrValueType.integerVal, (object) tag.ArchiveID, 0);
            iolIm.AddAttribute(id5, AttrValueType.stringVal, (object) tag.Descriptio, 0);
            iolIm.AddAttribute(id4, AttrValueType.stringVal, (object) tag.Descriptio, 0);
            GraphsSet graphsSet = new GraphsSet();
            GraphsCollection graphsCollection = new GraphsCollection();
            if (tag.SignStamp != null)
            {
              foreach (char oldKey in tag.SignStamp)
              {
                if (oldKey != ' ')
                {
                  string caption = cacheData.GetCaption(ImportingCategory.RankList, (object) oldKey);
                  int index3 = valuesDescriptions.IndexOf((object) caption);
                  if (index3 >= 0)
                  {
                    string str = (string) possibleValues[index3];
                    if (!graphsCollection.Contains(str))
                      graphsCollection.Add(new GraphClass(str, tag.StrongSign > 0, false));
                  }
                }
              }
            }
            graphsSet.Add("Группа подписей", graphsCollection);
            using (ImChunkedStream imChunkedStream = new ImChunkedStream())
            {
              graphsSet.Save((Stream) imChunkedStream);
              imChunkedStream.Position = 0L;
              IPackedStream service2 = (IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream));
              string str = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.tmp");
              stringList.Add(str);
              using (FileStream outStream = File.OpenWrite(str))
              {
                service2.PackStream((Stream) outStream, (Stream) imChunkedStream, 9);
                iolIm.AddAttributeBlob(id6, str, outStream.Length, "", ArcMethods.ZLibPacked);
              }
            }
            int numInList = 0;
            List<Guid> guidList;
            if (sortedList.TryGetValue(int32, out guidList))
            {
              foreach (Guid guid in guidList)
              {
                iolIm.AddAttribute(id7, AttrValueType.stringVal, (object) guid.ToString(), numInList);
                ++numInList;
              }
            }
            AttributesHelper.AddObligatoryObjectAttributes(userSession, iolIm);
            importArchives.Add(int32);
            if (tag.ParentID != 0)
              archiveTreeItemList.Add(new PumpArchives.ArchiveTreeItem(tag.ArchiveID, tag.ParentID));
          }
          i++;
        }
        iolIm.Import();
        this.PumpCheckPoint("Создание иерархии архивов", 81);
        string format2 = "Создание иерархии архивов: {0} из {1}";
        List<long> codes = new List<long>((ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize);
        IImportedRelationList irl = this.plugin.Idw.CreateImportedRelationList();
        irl.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          int index4 = 0;
          while (i < irl.Items.Count)
          {
            if (irl.Items[index4].Relation.PrjLinkId != -1L && irl.Items[index4].Relation.PrjLinkId != 0L)
              cacheData.AddValue(ImportingCategory.ArchivesTree, (object) codes[index4], irl.Items[index4].Relation.PrjLinkId);
            else
              this.plugin.appManager.AddWarningMessage($"Связь между архивами {irl.Items[index4].Relation.ProjId} и {irl.Items[index4].Relation.PartId} не импортирована");
            ++index4;
          }
          codes.Clear();
        });
        for (int index5 = 0; index5 < archiveTreeItemList.Count; ++index5)
        {
          PumpArchives.ArchiveTreeItem archiveTreeItem = archiveTreeItemList[index5];
          this.PumpCheckPoint(string.Format(format2, (object) (index5 + 1), (object) archiveTreeItemList.Count), this.CalculatePercent(archiveTreeItemList.Count, index5 + 1, 82, 99));
          long newKey1 = cacheData.GetNewKey(ImportingCategory.Archives, (object) archiveTreeItem.ArchiveID);
          long newKey2 = cacheData.GetNewKey(ImportingCategory.Archives, (object) archiveTreeItem.ParentID);
          if (newKey1 == 0L)
            this.plugin.appManager.AddWarningMessage($"Идентификатор архива {archiveTreeItem.ArchiveID} не найден в кэше закачанных архивов");
          else if (newKey2 == 0L)
          {
            this.plugin.appManager.AddWarningMessage($"Идентификатор архива {archiveTreeItem.ParentID} не найден в кэше закачанных архивов");
          }
          else
          {
            long oldKey = Convert.ToInt64(archiveTreeItem.ArchiveID) << 32 /*0x20*/ | (long) archiveTreeItem.ParentID;
            if (cacheData.GetNewKey(ImportingCategory.ArchivesTree, (object) oldKey) == 0L)
            {
              irl.AddRelation(newKey2, newKey1, this._simpleRelationID);
              AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, irl);
              codes.Add(oldKey);
            }
          }
        }
        irl.Import();
        this.plugin.appManager.AddInfoMessage($"Создано архивов: {importedCount}");
        this.PumpCheckPoint("Перекачка данных успешно завершена", 100);
      }
    }
    finally
    {
      if (stringList.Count > 0)
      {
        foreach (string path in stringList)
          File.Delete(path);
      }
      service1?.ReleaseCache(ImportingCategory.Archives, ImportingCategory.ArchivesTree, ImportingCategory.RankList, ImportingCategory.ArchiveParameters);
    }
  }

  private class ArchiveTreeItem
  {
    public int ArchiveID;
    public int ParentID;

    public ArchiveTreeItem(int archiveID, int parentID)
    {
      this.ArchiveID = archiveID;
      this.ParentID = parentID;
    }
  }
}
