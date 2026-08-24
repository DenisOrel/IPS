// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitiesPumpSettings
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using DevExpress.IM.Utils;
using DevExpress.IM.Utils.Menu;
using DevExpress.IM.XtraGrid;
using DevExpress.IM.XtraGrid.Menu;
using DevExpress.IM.XtraGrid.Views.Base;
using DevExpress.IM.XtraGrid.Views.Grid;
using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Advanced;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class EntitiesPumpSettings : StepControl
{
  private Image _image;
  private bool _onSaveState;
  private readonly Dictionary<DataRow, Entity> _rowMapping = new Dictionary<DataRow, Entity>();
  private Point _curRowPosition;
  private readonly Dictionary<int, string> _typeNameCacheList = new Dictionary<int, string>();
  private bool _readOnly;
  private bool _modified;
  private IContainer components;
  private ToolTipController tips;
  private Label lblFormCaption;
  protected GridControl gctlEntities;
  private ContextMenuStrip cmPumpTypes;
  private ToolStripMenuItem tsmiPumpTypesAdd;
  private ToolStripMenuItem tsmiPumpTypesDelete;
  private ToolStripMenuItem tsmiPumpTypesClear;
  private PropertyGrid pgEntProperty;
  private SplitContainer splitContainer1;
  private Button btnCancel;
  protected Button btnSave;
  protected Button btnLoad;
  private Button btnApply;
  private TableLayoutPanel tableLayoutPanel4;
  private Panel panel1;
  private TableLayoutPanel tableLayoutPanel5;
  protected ContextMenuStrip cmsEntities;
  protected ToolStripMenuItem tsmiFixSettings;
  protected ToolStripMenuItem tsmiFixSettingAlreadyExistAttr;
  private GridView gvEntities;

  protected virtual void InitializeData() => this.gvEntities.RowHeight = 14;

  protected virtual void UpdateControls()
  {
    this.btnApply.Enabled = this.btnCancel.Enabled = this.Modified;
    this.gvEntities.OptionsCustomization.AllowSort = !this.Modified;
    this.gvEntities.OptionsCustomization.AllowGroup = !this.Modified;
  }

  protected virtual Entity GetCurrentEntity()
  {
    Entity currentEntity = (Entity) null;
    if (this.gvEntities.SelectedRowsCount == 1 && this.gvEntities.GetRow(this.gvEntities.GetSelectedRows()[0]) is DataRowView row)
      currentEntity = this._rowMapping[row.Row];
    return currentEntity;
  }

  protected virtual void UpdateCurrentEntity(Entity entity)
  {
    if (this.gvEntities.SelectedRowsCount != 1 || !(this.gvEntities.GetRow(this.gvEntities.GetSelectedRows()[0]) is DataRowView row1))
      return;
    DataRow row2 = row1.Row;
    row2.ClearErrors();
    row2.ItemArray = this.ParseRowValues(entity);
  }

  protected virtual Entity GetEntity(int rowHandle)
  {
    Entity entity = (Entity) null;
    if (this.gvEntities.GetRow(rowHandle) is DataRowView row)
      entity = this._rowMapping[row.Row];
    return entity;
  }

  protected virtual bool SaveCurrentEntity(int rowHandle)
  {
    return this.SaveCurrentEntity(rowHandle, false, out bool _);
  }

  protected virtual bool SaveCurrentEntity(int rowHandle, bool showWarning, out bool hasError)
  {
    hasError = false;
    if (!this.Modified || this._onSaveState || this.pgEntProperty.SelectedObject == null)
      return false;
    bool flag1 = false;
    this._onSaveState = true;
    try
    {
      if (!(this.gvEntities.GetRow(rowHandle) is DataRowView row1))
        return false;
      if (showWarning && this.ShowEntityYesNoMessageDialog("Свойства атрибута изменились. Принять изменения?") == DialogResult.No)
      {
        this.Modified = false;
        return false;
      }
      DataRow row2 = row1.Row;
      row2.ClearErrors();
      Entity entity1 = this._rowMapping[row2];
      if (this.pgEntProperty.SelectedObject is EntityDescriptor selectedObject)
      {
        Entity entity2 = selectedObject.Entity;
        EntityProperties properties = entity2.Settings.Properties;
        List<Entity> hashtable = EntityHelper.ParseHashtable(this._rowMapping);
        bool flag2 = false;
        switch (properties.Status)
        {
          case EntityPumpStatus.None:
          case EntityPumpStatus.NotPump:
            flag2 = true;
            break;
          case EntityPumpStatus.Exists:
          case EntityPumpStatus.New:
          case EntityPumpStatus.Commited:
            IEnumerable<EntityErrorRecord> errors;
            new EntitySettingsValidator().Execute((IEnumerable<Entity>) new Entity[1]
            {
              entity2
            }, out errors);
            if (errors != null && !errors.IsEmpty<EntityErrorRecord>())
            {
              hasError = true;
              this.pgEntProperty.SelectedObject = (object) new EntityDescriptor(entity2, hashtable);
              this.ShowEntityOkMessageDialog(errors.First<EntityErrorRecord>().Message);
              break;
            }
            if (properties.Status == EntityPumpStatus.New)
            {
              EntityExistsStatus entityExistsStatus = EntityHelper.CheckEntitySett((IEnumerable<Entity>) hashtable, entity2);
              if (entityExistsStatus != EntityExistsStatus.None)
              {
                this.pgEntProperty.SelectedObject = (object) new EntityDescriptor(entity2, hashtable);
                this.ShowEntityOkMessageDialog($"Атрибут с таким параметрами ({EnumTypeHelper.GetCaption((Enum) entityExistsStatus)}) уже существует в списке.");
                break;
              }
              break;
            }
            break;
        }
        if (flag2)
        {
          List<Entity> entitySettRefList = EntityHelper.GetEntitySett_RefList(EntityHelper.ParseHashtable(this._rowMapping), entity1);
          if (entitySettRefList.Count > 0)
          {
            string str = string.Join(", ", entitySettRefList.Select<Entity, string>((System.Func<Entity, string>) (errorEntity => errorEntity.ToString())).ToArray<string>());
            this.ShowEntityOkMessageDialog("Проверка закачки понятий: " + string.Format("Изменения параметров понятия \"{0}\" привели к тому что поняти{2} \"{1}\" не буд{3}т закачиваться. Проверьте правильность настроек понятия \"{0}\".", (object) entity1, (object) str, entitySettRefList.Count > 1 ? (object) "я" : (object) "е", entitySettRefList.Count > 1 ? (object) "я" : (object) "е"));
          }
        }
        if (!hasError)
        {
          entity1.IsPermisibleAttr2TypeObj = entity2.IsPermisibleAttr2TypeObj;
          entity1.Settings.CopyData(entity2.Settings);
          TechPumpData.Entities.EntitiesList[entity1.Code] = entity1;
          flag1 = true;
        }
      }
      if (!hasError)
      {
        row2.ItemArray = this.ParseRowValues(entity1);
        this.Modified = false;
      }
      return flag1;
    }
    finally
    {
      this._onSaveState = false;
    }
  }

  protected virtual object[] ParseRowValues(Entity entity)
  {
    if (entity == null)
      return (object[]) null;
    string str1 = string.Empty;
    string empty = string.Empty;
    string str2 = string.Empty;
    switch (entity.Settings.Properties.Status)
    {
      case EntityPumpStatus.Exists:
      case EntityPumpStatus.Commited:
        if (entity.Settings.PumpTo != null)
        {
          if (entity.Settings.PumpTo is Guid)
          {
            if (!((Guid) entity.Settings.PumpTo).Equals(Guid.Empty))
            {
              if (TechcardConsts.Plugin != null)
              {
                IAttributeTypeItem byGuid = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid((Guid) entity.Settings.PumpTo);
                if (byGuid != null)
                  str1 = byGuid.Name;
                str2 = this.GetEntityWarningData(entity, byGuid != null ? (FieldTypes) byGuid.AttrValueType : FieldTypes.ftUnknown);
                break;
              }
              IMSAttributeType attributeType = MetaDataHelper.GetAttributeType((Guid) entity.Settings.PumpTo);
              if (attributeType != null)
                str1 = attributeType.Name;
              str2 = this.GetEntityWarningData(entity, attributeType != null ? attributeType.FieldType : FieldTypes.ftUnknown);
              break;
            }
            break;
          }
          if (entity.Settings.PumpTo is Entity pumpTo)
          {
            empty = pumpTo.ToString();
            break;
          }
          break;
        }
        break;
      case EntityPumpStatus.New:
        if (entity.Settings.PumpMode == EntityPumModes.NewAttr)
        {
          str1 = entity.Settings.Properties.Name;
          break;
        }
        break;
    }
    string str3;
    if (!this._typeNameCacheList.TryGetValue(entity.RecordID, out str3))
    {
      str3 = string.Empty;
      TechTypeInfo typeRecByRecordId = TechPumpData.TechType.TechTypeList.GetTypeRecByRecordId(entity.RecordID);
      if (typeRecByRecordId != null)
      {
        str3 = typeRecByRecordId.Name;
        this._typeNameCacheList.Add(entity.RecordID, str3);
      }
      if (entity.RecordID == 25)
        str3 = "Понятия для КТП";
    }
    EntityProperties properties = entity.Settings.Properties;
    return new object[9]
    {
      (object) str3,
      (object) entity.Type,
      (object) entity.Code,
      (object) entity.Name,
      (object) EnumTypeHelper.GetCaption((Enum) properties.FieldType),
      (object) EnumTypeHelper.GetCaption((Enum) properties.Status),
      !string.IsNullOrWhiteSpace(str1) ? (object) str1 : (object) DBNull.Value,
      !string.IsNullOrWhiteSpace(empty) ? (object) empty : (object) DBNull.Value,
      !string.IsNullOrWhiteSpace(str2) ? (object) str2 : (object) DBNull.Value
    };
  }

  protected string GetEntityWarningData(Entity entity, FieldTypes attrFieldType)
  {
    FieldTypes fieldTypesByType = EntityHelper.GetFieldTypesByType(entity.Type);
    string str = "Возможна потеря данных при конвертации в другой тип";
    return fieldTypesByType == FieldTypes.ftInteger && entity.EntityReference != null && entity.EntityReference.MasterCode == entity.Code ? (attrFieldType == FieldTypes.ftObjectLink ? string.Empty : "Ожидаемый тип атрибута: ссылка на объект") : (attrFieldType != fieldTypesByType && attrFieldType != FieldTypes.ftString ? str : string.Empty);
  }

  protected virtual void CheckRowError(GridView view, int rowHandle)
  {
    DataRow row = view.GetDataRow(rowHandle);
    if (row.HasErrors && !this._onSaveState)
      return;
    row.RowError = string.Empty;
    Entity entity = this.GetEntity(rowHandle);
    if (entity == null)
      return;
    IEnumerable<EntityErrorRecord> errors;
    new EntitySettingsValidator().Execute((IEnumerable<Entity>) new Entity[1]
    {
      entity
    }, out errors);
    if (errors != null)
      errors.InvokeForAll<EntityErrorRecord>((Action<EntityErrorRecord>) (item => row.RowError += $"{item.Message};"));
    if (entity.Settings.Properties.Status != EntityPumpStatus.None || !entity.IsMasterAttr)
      return;
    string str = "Не настроен мастер-атрибут";
    row.RowError += $"{str};";
  }

  protected virtual void SetupPredefinedSettings(Dictionary<string, Entity> entities)
  {
    new EntitySettingsFixed().Setup((IEnumerable<Entity>) entities.Values);
    new EntitySettingsDefault().Setup((IEnumerable<Entity>) entities.Values);
    new EntitySettingsIpsAttributes().Setup((IEnumerable<Entity>) entities.Values);
  }

  protected virtual void SaveEntitySettings()
  {
    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
    {
      saveFileDialog.Filter = "Entities File (*.tce)|*.tce";
      saveFileDialog.FilterIndex = 0;
      saveFileDialog.AddExtension = true;
      saveFileDialog.CheckPathExists = true;
      saveFileDialog.RestoreDirectory = true;
      if (!saveFileDialog.ShowDialog().Equals((object) DialogResult.OK))
        return;
      this.SaveEntitySettings(saveFileDialog.FileName);
    }
  }

  protected virtual void SaveEntitySettings(string fileName)
  {
    MemoryStream memoryStream = new MemoryStream();
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    this.UpdateEntityList();
    Dictionary<string, Entity> entitiesList = TechPumpData.Entities.EntitiesList;
    MemoryStream serializationStream = memoryStream;
    Dictionary<string, Entity> graph = entitiesList;
    binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
    byte[] array = memoryStream.ToArray();
    FileStream fileStream = System.IO.File.Create(fileName);
    try
    {
      fileStream.Write(array, 0, array.Length);
      fileStream.Flush();
    }
    finally
    {
      fileStream.Close();
    }
  }

  protected virtual void UpdateEntityList()
  {
    Dictionary<string, Entity> dictionary = EntityHelper.ParseHashtable(this._rowMapping).ToDictionary<Entity, string>((System.Func<Entity, string>) (ents => ents.Code));
    List<string> stringList = new List<string>(TechPumpData.Entities.EntitiesList.Count);
    foreach (KeyValuePair<string, Entity> keyValuePair in dictionary)
    {
      Entity entity;
      if (!TechPumpData.Entities.EntitiesList.TryGetValue(keyValuePair.Key, out entity))
        stringList.Add(keyValuePair.Key);
      else if (entity != keyValuePair.Value)
        stringList.Add(keyValuePair.Key);
    }
    if (stringList.Count != 0)
    {
      string str = string.Join(",", stringList.ToArray());
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Для понятий \"{str}\" отличаются настроки с основным списком");
    }
    TechPumpData.Entities._entitiesList = dictionary;
  }

  protected virtual void LoadEntitySettings()
  {
    using (OpenFileDialog openFileDialog = new OpenFileDialog())
    {
      openFileDialog.Filter = "Entities File (*.tce)|*.tce";
      openFileDialog.FilterIndex = 0;
      openFileDialog.AddExtension = true;
      openFileDialog.CheckPathExists = true;
      openFileDialog.CheckFileExists = true;
      openFileDialog.RestoreDirectory = true;
      if (!openFileDialog.ShowDialog().Equals((object) DialogResult.OK))
        return;
      this.LoadEntitySettingsFromFile(openFileDialog.FileName);
    }
  }

  protected virtual void LoadEntitySettingsFromFile(string fileName)
  {
    if (fileName.Equals(string.Empty) || !System.IO.File.Exists(fileName))
      return;
    FileStream fileStream = System.IO.File.OpenRead(fileName);
    try
    {
      if (TechcardConsts.Plugin != null)
        TechcardConsts.Plugin.appManager.AddInfoMessage($"Чтение настроек правил перекачки понятий из файла: \"{fileName}\"");
      byte[] buffer = new byte[fileStream.Length];
      fileStream.Read(buffer, 0, buffer.Length);
      if (new BinaryFormatter().Deserialize((Stream) new MemoryStream(buffer)) is Dictionary<string, Entity> entSettings && entSettings.Count < 500)
      {
        string Message = $"Ошибка при десериализации понятий. Загружено :{entSettings.Count}";
        if (TechcardConsts.Plugin != null)
          TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
      }
      this.LoadEntitySettings(entSettings);
    }
    finally
    {
      fileStream.Close();
    }
  }

  public virtual void LoadEntitySettings(Dictionary<string, Entity> entSettings)
  {
    if (entSettings == null || entSettings.Count == 0)
      return;
    bool flag1 = false;
    bool flag2 = false;
    foreach (Entity entity1 in TechPumpData.Entities.EntitiesList.Values)
    {
      Entity entity2;
      if (entSettings.TryGetValue(entity1.Code, out entity2) && entity2 != null && entity1.RecordID == entity2.RecordID && !(entity1.Type != entity2.Type))
      {
        flag1 = !this.VerifyEntitySettings(entity2);
        entity1.Settings.CopyData(entity2.Settings);
        entity1.IsPermisibleAttr2TypeObj = entity2.IsPermisibleAttr2TypeObj;
        if (entity1.Settings.PumpTo is Entity pumpTo)
        {
          Entity entity3;
          TechPumpData.Entities.EntitiesList.TryGetValue(pumpTo.Code, out entity3);
          entity1.Settings.PumpTo = (object) entity3;
        }
        if (entity1.Productions == null)
        {
          entity1.Productions = new int[0];
          flag1 = true;
          flag2 = true;
        }
        if (entity1.Productions.Length == 0 && TechPumpData.Entities.EntityProductionList.ContainsKey(entity1.Code))
          entity1.Productions = TechPumpData.Entities.EntityProductionList[entity1.Code].ToArray();
      }
    }
    if (flag2 && TechcardConsts.Plugin != null)
      TechcardConsts.Plugin.appManager.AddWarningMessage("Неполная версия файла настроек правил перекачки понятий в атрибуты!");
    if (flag1 && TechcardConsts.Plugin != null)
      TechcardConsts.Plugin.appManager.AddWarningMessage("В файле настроек имеются ошибки");
    this.SetValues(TechPumpData.Entities.EntitiesList);
  }

  protected virtual bool SaveEntitiesSettings(Dictionary<string, Entity> entlist)
  {
    bool flag = true;
    Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
    string entsSettingsString = this.GetEntsSettingsString(entlist);
    if (entsSettingsString == string.Empty)
    {
      string Message = "EntitiesPumpSettings.SaveEntitiesSettings невозможно получить строковое представление понятий";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    }
    try
    {
      settings.Add("SaveSettingsEnts", new SaveSettingsAttribute[1]
      {
        new SaveSettingsAttribute("SaveSettingsEnts", entsSettingsString)
      });
      if (!(ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) is ISaveSettings service))
        throw new Exception("Служба ISaveSettings не найдена.");
      service.SetSettings("TECHCARDSETTINGS", settings);
    }
    catch (Exception ex)
    {
      flag = false;
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно сохранить настройки правил перекачки понятий: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return flag;
  }

  protected virtual string GetEntsSettingsString(Dictionary<string, Entity> entlist)
  {
    string entsSettingsString = string.Empty;
    if (entlist != null)
    {
      if (entlist.Count != 0)
      {
        try
        {
          MemoryStream serializationStream = new MemoryStream();
          new BinaryFormatter().Serialize((Stream) serializationStream, (object) entlist);
          entsSettingsString = Convert.ToBase64String(serializationStream.ToArray());
        }
        catch (Exception ex)
        {
          TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно сохранить настройки правил перекачки понятий: {ex.Message}");
          if (ex is OutOfMemoryException)
            throw;
        }
        return entsSettingsString;
      }
    }
    string Message = "EntitiesPumpSettings.GetEntsSettingsString(Dictionary<string, Entity> entlist) ошибка входного параметра";
    TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    return entsSettingsString;
  }

  protected virtual bool VerifyEntitySettings(Entity entity)
  {
    bool flag = false;
    if (entity == null)
      return false;
    IEnumerable<EntityErrorRecord> errors;
    new EntitySettingsValidator(true).Execute((IEnumerable<Entity>) new Entity[1]
    {
      entity
    }, out errors);
    if (errors != null && !errors.IsEmpty<EntityErrorRecord>())
    {
      flag = true;
      entity.Settings.Properties.Status = EntityPumpStatus.None;
      if (TechcardConsts.Plugin != null)
      {
        string Message = $"Чтение настроек из файла: Понятие {entity} - {errors.First<EntityErrorRecord>().Message}. Статус закачки изменен на \"{EnumDescConverter.GetEnumDescription((Enum) EntityPumpStatus.None)}\"";
        TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
      }
    }
    EntitySetting settings = entity.Settings;
    Entity entity1;
    if (settings != null && settings.Properties.FieldType == FieldTypes.ftObjectLink && TechPumpData.Entities.EntitiesList.TryGetValue(entity.Code, out entity1) && (entity.EntityReference == null || entity1.EntityReference == null))
    {
      flag = true;
      entity.EntityReference = entity1.EntityReference;
      if (entity.Settings.Properties.Status != EntityPumpStatus.NotPump)
        entity.Settings.Properties.Status = EntityPumpStatus.None;
      if (entity1.EntityReference == null)
      {
        entity.IsMasterAttr = false;
        entity.Settings.Properties.FieldType = FieldTypes.ftInteger;
      }
      if (TechcardConsts.Plugin != null)
      {
        string Message = $"Чтение настроек из файла: Понятие {entity} настроено на тип \"Ссылка на объект\", но у понятия нет ссылки на справочник. Статус закачки изменен на \"{EnumDescConverter.GetEnumDescription((Enum) EntityPumpStatus.None)}\"";
        TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
      }
    }
    return !flag;
  }

  protected virtual void FixAlreadyExistsAttributes()
  {
    if (TechPumpData.Entities.EntitiesList == null || TechPumpData.Entities.EntitiesList.Count == 0 || TechcardConsts.Plugin == null)
      return;
    bool flag = false;
    foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values)
    {
      if (entity != null)
      {
        EntitySetting settings = entity.Settings;
        if (settings != null)
        {
          EntityProperties properties = settings.Properties;
          if (properties != null && properties.Status != EntityPumpStatus.NotPump)
          {
            switch (settings.PumpMode)
            {
              case EntityPumModes.NewAttr:
                if (EntitySettingsIpsAttributes.LookupIpsAttribute(entity, (IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values))
                {
                  flag = true;
                  continue;
                }
                continue;
              case EntityPumModes.ExistAttr:
                if (settings.PumpTo is Guid)
                {
                  Guid pumpTo = (Guid) settings.PumpTo;
                  if (!pumpTo.Equals(Guid.Empty))
                  {
                    IAttributeTypeItem byGuid = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(pumpTo);
                    if (byGuid == null)
                    {
                      properties.Status = EntityPumpStatus.None;
                      flag = true;
                      continue;
                    }
                    if (string.IsNullOrEmpty(properties.Alias) || !(properties.Alias == byGuid.Alias))
                    {
                      if (((FieldTypes) byGuid.AttrValueType == properties.FieldType || properties.FieldType == FieldTypes.ftDouble && byGuid.AttrValueType == 13) && byGuid.MultiValueMode == properties.MultipleValued)
                      {
                        settings.PumpMode = EntityPumModes.ExistAttr;
                        settings.PumpTo = (object) byGuid.GUID;
                        if (properties.Status != EntityPumpStatus.NotPump)
                          properties.Status = EntityPumpStatus.Commited;
                        flag = true;
                        continue;
                      }
                      if (pumpTo == byGuid.GUID)
                      {
                        properties.Status = EntityPumpStatus.None;
                        flag = true;
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
    if (!flag)
      return;
    this.SetValues(TechPumpData.Entities.EntitiesList);
  }

  protected void TreatExistEntity(Entity entity)
  {
    Guid attributeGuid;
    if (!EntityHelper.GetAttributeGuid(entity, out attributeGuid) || attributeGuid == Guid.Empty)
      return;
    if (TechcardConsts.TechcardCommon.Code2AttributeGuid.ContainsKey(entity.Code))
      TechcardConsts.TechcardCommon.Code2AttributeGuid[entity.Code] = attributeGuid;
    else
      TechcardConsts.TechcardCommon.Code2AttributeGuid.Add(entity.Code, attributeGuid);
  }

  private void TreatNewEntity(Entity entity) => entity.IsPermisibleAttr2TypeObj = true;

  private void TreatNoneEntity(Entity entity)
  {
  }

  private void TreatCommittedEntity(Entity entity)
  {
    Guid attributeGuid;
    if (!EntityHelper.GetAttributeGuid(entity, out attributeGuid) || attributeGuid == Guid.Empty)
      return;
    if (TechcardConsts.TechcardCommon.Code2AttributeGuid.ContainsKey(entity.Code))
      TechcardConsts.TechcardCommon.Code2AttributeGuid[entity.Code] = attributeGuid;
    else
      TechcardConsts.TechcardCommon.Code2AttributeGuid.Add(entity.Code, attributeGuid);
  }

  public bool PumpEntities()
  {
    this.UpdateEntityList();
    IEnumerable<EntityErrorRecord> errors;
    new EntitySettingsValidator().Execute((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values, out errors);
    if (errors != null && !errors.IsEmpty<EntityErrorRecord>())
    {
      EntitySettingsErrorReport settingsErrorReport = new EntitySettingsErrorReport();
      settingsErrorReport.LoadErrors(errors);
      if (settingsErrorReport.ShowDialog() == DialogResult.Abort)
      {
        int rowHandle = this.gvEntities.LocateByDisplayText(0, this.gvEntities.Columns.ColumnByFieldName("Код понятия"), settingsErrorReport.EntityCode.Substring(0, settingsErrorReport.EntityCode.IndexOfAny(new char[2]
        {
          ' ',
          '('
        }, 0)));
        if (rowHandle >= 0)
        {
          this.gvEntities.FocusedRowHandle = rowHandle;
          this.gvEntities.SelectRow(rowHandle);
        }
      }
      return false;
    }
    using (StepControlProgress stepControlProgress = new StepControlProgress())
    {
      stepControlProgress.Text = "Сохранение понятий";
      stepControlProgress.SetCenterParentLocation(TechcardConsts.Plugin.appManager as Control);
      stepControlProgress.SetProgress("Подготовка понятий", 0);
      stepControlProgress.Visible = true;
      int posValue = 0;
      TechcardConsts.TechcardCommon.Code2AttributeGuid.Clear();
      foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values)
      {
        if (entity.Settings != null && entity.Settings.Properties != null)
        {
          switch (entity.Settings.Properties.Status)
          {
            case EntityPumpStatus.None:
              this.TreatNoneEntity(entity);
              break;
            case EntityPumpStatus.Exists:
              this.TreatExistEntity(entity);
              break;
            case EntityPumpStatus.New:
              this.TreatNewEntity(entity);
              break;
            case EntityPumpStatus.Commited:
              this.TreatCommittedEntity(entity);
              break;
          }
          ++posValue;
          stepControlProgress.SetProgress("Сохранение настроек", 0, 80 /*0x50*/, posValue, TechPumpData.Entities.EntitiesList.Count);
        }
      }
      TechCache.WriteOneList(TechCache.CategoryList.EntitiesListOrg, (object) TechPumpData.Entities.EntitiesList);
      TechCache.WriteOneList(TechCache.CategoryList.EntitiesList, (object) TechPumpData.Entities.EntitiesList);
      stepControlProgress.SetProgress("Сохранение настроек завершено", 100);
    }
    return true;
  }

  public void CheckPumpedBeforeEntities()
  {
    object cacheData;
    if (!TechCache.ReadOneList(TechCache.CategoryList.EntitiesList, out cacheData, false) || cacheData == null)
      return;
    this.UpdateEntityList();
    List<Entity> entityList = new List<Entity>();
    foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values)
    {
      if (entity.Settings != null && entity.Settings.Properties != null)
      {
        switch (entity.Settings.Properties.Status)
        {
          case EntityPumpStatus.New:
          case EntityPumpStatus.Commited:
            entityList.Add(entity);
            continue;
          default:
            continue;
        }
      }
    }
    IEnumerable<EntityErrorRecord> errors;
    new EntitySettingsValidator().Execute((IEnumerable<Entity>) entityList, out errors);
    if (errors == null || errors.IsEmpty<EntityErrorRecord>())
      return;
    int num = (int) MessageBox.Show("Обнаружены в кэше ошибки настройки понятий предыдущей закачки в существующие атрибуты IPS. Рекомендуется произвести корректировку настроек по команде контекстного меню \"Автозамена настроек\" ", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
  }

  public EntitiesPumpSettings(object owner)
    : base(owner)
  {
    this.stepPrevAllowed = true;
    this.stepRepumpble = true;
    this.InitializeComponent();
    this.InitializeData();
  }

  protected override string getCaption()
  {
    return "Настройка перекачки технологических  параметров (понятий)";
  }

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgTechParams")];
    return this._image;
  }

  public override SaveSettingsResult SaveSettings() => SaveSettingsResult.ssrOk;

  public override bool LeaveControl()
  {
    int num = this.PumpEntities() ? 1 : 0;
    if (num == 0)
      return num != 0;
    this.SaveEntitiesSettings(TechPumpData.Entities.EntitiesList);
    return num != 0;
  }

  public override void RefreshControl()
  {
    base.RefreshControl();
    this.CheckPumpedBeforeEntities();
  }

  public virtual Dictionary<string, Entity> GetEntitySettingsAtPumpSett()
  {
    try
    {
      if (!(ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) is ISaveSettings service))
      {
        string Message = "Метод EntitiesPumpSettings.GetEntitySettingsAtPumpSett() невозможно получить службу сохранения настроек \"ISaveSettings\" ";
        TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
        throw new Exception("Служба ISaveSettings не найдена.");
      }
      Dictionary<string, SaveSettingsAttribute[]> settings = service.GetSettings("TECHCARDSETTINGS");
      if (settings == null)
        return (Dictionary<string, Entity>) null;
      if (settings.ContainsKey("SaveSettingsEnts"))
        return new BinaryFormatter().Deserialize((Stream) new MemoryStream(Convert.FromBase64String(settings["SaveSettingsEnts"][0].AttributeValue))) as Dictionary<string, Entity>;
      string Message1 = $"Метод EntitiesPumpSettings.GetEntitySettingsAtPumpSett() не может найти в SaveSettings ветку с тегом {"SaveSettingsEnts"}";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message1);
      return (Dictionary<string, Entity>) null;
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно загрузить настройки правил перекачки понятий: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return (Dictionary<string, Entity>) null;
      throw;
    }
  }

  public virtual void SetValues(Dictionary<string, Entity> entities)
  {
    this.SetupPredefinedSettings(entities);
    TechPumpData.Entities._entitiesList = entities;
    this.gctlEntities.BeginUpdate();
    try
    {
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add(new DataColumn("Технологический тип записи", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Тип понятия", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Код понятия", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Наименование понятия", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Тип данных", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Статус", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Атрибут для закачки", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Понятие для закачки", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Замечания", typeof (string)));
      foreach (DataColumn column in (InternalDataCollectionBase) dataTable.Columns)
      {
        if (column.DataType == typeof (string))
          column.DefaultValue = (object) string.Empty;
      }
      this._rowMapping.Clear();
      foreach (Entity entity in entities.Values)
        this._rowMapping[dataTable.Rows.Add(this.ParseRowValues(entity))] = entity;
      dataTable.AcceptChanges();
      this.gctlEntities.DataSource = (object) dataTable;
      this.gvEntities.Columns[0].Group();
      this.gvEntities.FormatConditions.Add(new StyleFormatCondition(FormatConditionEnum.Equal, (object) null, "IsNotSetup", (object) EnumTypeHelper.GetCaption((Enum) EntityPumpStatus.None), (object) null, this.gvEntities.Columns[4], true));
    }
    finally
    {
      this.gctlEntities.EndUpdate();
    }
  }

  public bool ReadOnly
  {
    get => this._readOnly;
    set => this._readOnly = value;
  }

  public bool Modified
  {
    get => this._modified;
    set
    {
      if (this._modified == value)
        return;
      this._modified = value;
      this.UpdateControls();
    }
  }

  private void btnSave_Click(object sender, EventArgs e) => this.SaveEntitySettings();

  private void btnLoad_Click(object sender, EventArgs e) => this.LoadEntitySettings();

  private void btnApply_Click(object sender, EventArgs e)
  {
    if (this.gvEntities.SelectedRowsCount != 1)
      throw new ArgumentException("Невозможно применить изменения т.к. выбрано несколько атрибутов.");
    this.SaveCurrentEntity(this.gvEntities.GetSelectedRows()[0]);
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this.pgEntProperty.Refresh();
    this.Modified = false;
  }

  private void tsmiFixSettingAlreadyExistAttr_Click(object sender, EventArgs e)
  {
    if (!MessageBox.Show($"Запустить процедуру замены настроек : изменение режима закачки с \"{EnumDescConverter.GetEnumDescription((Enum) EntityPumModes.NewAttr)}\" на \"{EnumDescConverter.GetEnumDescription((Enum) EntityPumModes.ExistAttr)}\", и корректировку существующих атрибутов типы которых совпадают с настройками понятий? Рекомендуется предварительно сохранить настройки.", "Внимание рекомендуется предварительно сохранить настройки", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2).Equals((object) DialogResult.Yes))
      return;
    this.FixAlreadyExistsAttributes();
  }

  private void gvEntities_BeforeLeaveRow(object sender, RowAllowEventArgs e)
  {
    bool hasError;
    this.SaveCurrentEntity(e.RowHandle, true, out hasError);
    e.Allow = !hasError;
  }

  private void gvEntity_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
  {
    this.pgEntProperty.SelectedObject = (object) null;
    this.ReadOnly = false;
    Entity entity1;
    if (!(this.gvEntities.GetRow(e.FocusedRowHandle) is DataRowView row) || !this._rowMapping.TryGetValue(row.Row, out entity1) || entity1 == null)
      return;
    Entity entity2 = entity1.Clone();
    switch (entity1.Settings.PumpMode)
    {
      case EntityPumModes.NewAttr:
        this.ReadOnly = false;
        break;
      case EntityPumModes.ExistAttr:
        switch (entity1.Settings.Properties.Status)
        {
          case EntityPumpStatus.None:
          case EntityPumpStatus.NotPump:
            break;
          default:
            if (entity1.Settings.PumpTo == null)
            {
              if (this.ShowEntityYesNoMessageDialog($"Понятие {entity1} помечено как: \"Существующее понятие\", но не указано в какой атрибут/понятие осуществлять закачку. Желаете ли Вы указать куда производить закачку?") == DialogResult.Yes)
              {
                EntityProperties properties = entity1.Settings.Properties;
                ISelectorForm selectorForm = (ISelectorForm) new AttributeTypeSelectorForm("Выберите атрибут", entity1);
                using (selectorForm)
                {
                  if (selectorForm.ShowDialog() == DialogResult.OK)
                  {
                    object selectedItem = selectorForm.SelectedItem;
                    if (selectedItem != null)
                    {
                      entity1.Settings.PumpTo = selectedItem;
                      properties.Status = EntityPumpStatus.Commited;
                      this.UpdateCurrentEntity(entity1);
                      break;
                    }
                    entity1.Settings.PumpMode = EntityPumModes.NewAttr;
                    entity1.Settings.Properties.Status = EntityPumpStatus.None;
                    break;
                  }
                  entity1.Settings.Properties.Status = EntityPumpStatus.None;
                  return;
                }
              }
              entity1.Settings.PumpMode = EntityPumModes.NewAttr;
              break;
            }
            break;
        }
        break;
    }
    this.pgEntProperty.SelectedObject = (object) new EntityDescriptor(entity2, EntityHelper.ParseHashtable(this._rowMapping));
  }

  private void gvEntities_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
  {
    if (e.Column.VisibleIndex != 0)
      return;
    this.CheckRowError(sender as GridView, e.RowHandle);
    if (e.RowHandle != this.gvEntities.FocusedRowHandle)
      return;
    this._curRowPosition = e.Bounds.Location;
  }

  private void gvEntities_RowCellStyle(object sender, RowCellStyleEventArgs e)
  {
    if (this.gvEntities.GetRow(e.RowHandle) is DataRowView row && e.Column.AbsoluteIndex == 6 && this._rowMapping[row.Row].Settings.PumpMode == EntityPumModes.NewAttr)
    {
      string styleName = "Pump2NewEntityStyle";
      ViewStyleEx viewStyleEx = !this.gctlEntities.Styles.Contains(styleName) ? this.gctlEntities.Styles.Add(styleName, string.Empty) as ViewStyleEx : this.gctlEntities.Styles[styleName] as ViewStyleEx;
      if (viewStyleEx != null)
      {
        viewStyleEx.Font = new Font(viewStyleEx.Font, FontStyle.Bold);
        e.CellStyle = (ViewStyle) viewStyleEx;
      }
    }
    if (row == null || string.IsNullOrEmpty(Convert.ToString(row.Row[8])))
      return;
    bool flag = this.gvEntities.FocusedRowHandle == e.RowHandle;
    string styleName1 = flag ? "EntityWithWarningFocusedStyle" : "EntityWithWarningStyle";
    ViewStyleEx viewStyleEx1;
    if (this.gctlEntities.Styles.Contains(styleName1))
    {
      viewStyleEx1 = this.gctlEntities.Styles[styleName1] as ViewStyleEx;
    }
    else
    {
      viewStyleEx1 = this.gctlEntities.Styles.Add(styleName1, string.Empty) as ViewStyleEx;
      if (flag)
      {
        if (viewStyleEx1 != null)
        {
          viewStyleEx1.BackColor = SystemColors.Highlight;
          viewStyleEx1.ForeColor = Color.Khaki;
        }
      }
      else if (viewStyleEx1 != null)
        viewStyleEx1.BackColor = Color.Khaki;
    }
    e.CellStyle = (ViewStyle) viewStyleEx1;
  }

  private void gvEntities_ShowGridMenu(object sender, GridMenuEventArgs e)
  {
    if (e.Menu is GridViewGroupPanelMenu)
    {
      foreach (DXMenuItem dxMenuItem in (CollectionBase) e.Menu.Items)
      {
        switch (dxMenuItem.Caption)
        {
          case "Full Expand":
            dxMenuItem.Caption = "Раскрыть все";
            continue;
          case "Full Collapse":
            dxMenuItem.Caption = "Собрать все";
            continue;
          case "Clear Grouping":
            dxMenuItem.Caption = "Сбросить группировки";
            continue;
          default:
            continue;
        }
      }
    }
    else if (e.Menu is GridViewColumnMenu)
    {
      foreach (DXMenuItem dxMenuItem in (CollectionBase) e.Menu.Items)
      {
        switch (dxMenuItem.Caption)
        {
          case "Best Fit":
            dxMenuItem.Caption = "Наилучшая подборка (текущие столбцы)";
            continue;
          case "Best Fit (all columns)":
            dxMenuItem.Caption = "Наилучшая подборка (все столбцы)";
            continue;
          case "Clear Filter":
            dxMenuItem.Caption = "Убрать фильтры";
            continue;
          case "Group By Box":
            dxMenuItem.Caption = "Группировать по выбранному";
            continue;
          case "Group By This Field":
            dxMenuItem.Caption = "Группировать по текущему полю";
            continue;
          case "Runtime Column Customization":
            dxMenuItem.Caption = "Настройка столбцов";
            continue;
          case "Sort Ascending":
            dxMenuItem.Caption = "Восходящая сортировка";
            continue;
          case "Sort Descending":
            dxMenuItem.Caption = "Нисходящая сортировка";
            continue;
          case "UnGroup":
            dxMenuItem.Caption = "Разгруппировать";
            continue;
          default:
            continue;
        }
      }
    }
    else
    {
      GridViewFooterMenu menu = e.Menu as GridViewFooterMenu;
    }
  }

  private void gvEntities_ShowCustomizationForm(object sender, EventArgs e)
  {
    this.gvEntities.CustomizationForm.Text = "Настройки столбцов";
    this.gvEntities.CustomizationForm.Opacity = 0.7;
  }

  private void pgEntProperty_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    if (this.ReadOnly)
      return;
    this.Modified = true;
  }

  private void ShowEntityOkMessageDialog(string messageText)
  {
    using (EntityOkMessageDialog entityOkMessageDialog = new EntityOkMessageDialog())
    {
      try
      {
        entityOkMessageDialog.setPosition(this.ParentForm, new Point(this._curRowPosition.X + 2, this._curRowPosition.Y + 100));
        entityOkMessageDialog.MessageText = messageText;
        int num = (int) entityOkMessageDialog.ShowDialog();
      }
      finally
      {
        this.pgEntProperty.Refresh();
      }
    }
  }

  private DialogResult ShowEntityYesNoMessageDialog(string messageText)
  {
    using (EntityYesNoMessageDialog yesNoMessageDialog = new EntityYesNoMessageDialog())
    {
      try
      {
        yesNoMessageDialog.setPosition(this.ParentForm, new Point(this._curRowPosition.X + 2, this._curRowPosition.Y + 100));
        yesNoMessageDialog.MessageText = messageText;
        return yesNoMessageDialog.ShowDialog();
      }
      finally
      {
        this.pgEntProperty.Refresh();
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EntitiesPumpSettings));
    this.cmPumpTypes = new ContextMenuStrip(this.components);
    this.tsmiPumpTypesAdd = new ToolStripMenuItem();
    this.tsmiPumpTypesDelete = new ToolStripMenuItem();
    this.tsmiPumpTypesClear = new ToolStripMenuItem();
    this.pgEntProperty = new PropertyGrid();
    this.tips = new ToolTipController(this.components);
    this.btnApply = new Button();
    this.btnLoad = new Button();
    this.btnSave = new Button();
    this.btnCancel = new Button();
    this.lblFormCaption = new Label();
    this.gctlEntities = new GridControl();
    this.cmsEntities = new ContextMenuStrip(this.components);
    this.tsmiFixSettings = new ToolStripMenuItem();
    this.tsmiFixSettingAlreadyExistAttr = new ToolStripMenuItem();
    this.gvEntities = new GridView();
    this.splitContainer1 = new SplitContainer();
    this.tableLayoutPanel4 = new TableLayoutPanel();
    this.panel1 = new Panel();
    this.tableLayoutPanel5 = new TableLayoutPanel();
    this.cmPumpTypes.SuspendLayout();
    this.gctlEntities.BeginInit();
    this.cmsEntities.SuspendLayout();
    this.gvEntities.BeginInit();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.tableLayoutPanel4.SuspendLayout();
    this.panel1.SuspendLayout();
    this.tableLayoutPanel5.SuspendLayout();
    this.SuspendLayout();
    this.cmPumpTypes.Items.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this.tsmiPumpTypesAdd,
      (ToolStripItem) this.tsmiPumpTypesDelete,
      (ToolStripItem) this.tsmiPumpTypesClear
    });
    this.cmPumpTypes.Name = "otMenu";
    componentResourceManager.ApplyResources((object) this.cmPumpTypes, "cmPumpTypes");
    this.tsmiPumpTypesAdd.Name = "tsmiPumpTypesAdd";
    componentResourceManager.ApplyResources((object) this.tsmiPumpTypesAdd, "tsmiPumpTypesAdd");
    this.tsmiPumpTypesDelete.Name = "tsmiPumpTypesDelete";
    componentResourceManager.ApplyResources((object) this.tsmiPumpTypesDelete, "tsmiPumpTypesDelete");
    this.tsmiPumpTypesClear.Name = "tsmiPumpTypesClear";
    componentResourceManager.ApplyResources((object) this.tsmiPumpTypesClear, "tsmiPumpTypesClear");
    componentResourceManager.ApplyResources((object) this.pgEntProperty, "pgEntProperty");
    this.pgEntProperty.Name = "pgEntProperty";
    this.pgEntProperty.PropertyValueChanged += new PropertyValueChangedEventHandler(this.pgEntProperty_PropertyValueChanged);
    this.tips.Style = new ViewStyle("ToolTip style");
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Name = "btnApply";
    this.tips.SetToolTip((Control) this.btnApply, "Применить изменения текущего правила");
    this.btnApply.UseVisualStyleBackColor = true;
    this.btnApply.Click += new EventHandler(this.btnApply_Click);
    componentResourceManager.ApplyResources((object) this.btnLoad, "btnLoad");
    this.btnLoad.Name = "btnLoad";
    this.tips.SetToolTip((Control) this.btnLoad, "Загрузить правила преобразования технологических параметров в типы атрибутов");
    this.btnLoad.UseVisualStyleBackColor = true;
    this.btnLoad.Click += new EventHandler(this.btnLoad_Click);
    componentResourceManager.ApplyResources((object) this.btnSave, "btnSave");
    this.btnSave.Name = "btnSave";
    this.tips.SetToolTip((Control) this.btnSave, "Сохранить правила преобразования технологических параметров в типы атрибутов");
    this.btnSave.UseVisualStyleBackColor = true;
    this.btnSave.Click += new EventHandler(this.btnSave_Click);
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Name = "btnCancel";
    this.tips.SetToolTip((Control) this.btnCancel, "Отменить изменения текущего правила");
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    componentResourceManager.ApplyResources((object) this.lblFormCaption, "lblFormCaption");
    this.lblFormCaption.Name = "lblFormCaption";
    this.gctlEntities.ContextMenuStrip = this.cmsEntities;
    componentResourceManager.ApplyResources((object) this.gctlEntities, "gctlEntities");
    this.gctlEntities.EmbeddedNavigator.Name = "";
    this.gctlEntities.MainView = (BaseView) this.gvEntities;
    this.gctlEntities.Name = "gctlEntities";
    this.gctlEntities.Styles.AddReplace("Style1", (object) new ViewStyleEx("Style1", "", Color.Maroon, SystemColors.HotTrack, Color.Empty, LinearGradientMode.Horizontal));
    this.gctlEntities.Styles.AddReplace("FocusedRow", (object) new ViewStyleEx("FocusedRow", "Grid", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, SystemColors.Highlight, SystemColors.HighlightText, Color.Empty, LinearGradientMode.Horizontal));
    this.gctlEntities.Styles.AddReplace("FocusedCell", (object) new ViewStyleEx("FocusedCell", "Grid", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, SystemColors.Window, SystemColors.WindowText, Color.Empty, LinearGradientMode.Horizontal));
    this.gctlEntities.Styles.AddReplace("IsNotSetup", (object) new ViewStyleEx("IsNotSetup", "", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), StyleOptions.StyleEnabled | StyleOptions.UseForeColor, SystemColors.Window, Color.Red, Color.Empty, LinearGradientMode.Horizontal));
    this.cmsEntities.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.tsmiFixSettings
    });
    this.cmsEntities.Name = "cmsEntities";
    componentResourceManager.ApplyResources((object) this.cmsEntities, "cmsEntities");
    this.tsmiFixSettings.DropDownItems.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.tsmiFixSettingAlreadyExistAttr
    });
    this.tsmiFixSettings.Name = "tsmiFixSettings";
    componentResourceManager.ApplyResources((object) this.tsmiFixSettings, "tsmiFixSettings");
    this.tsmiFixSettingAlreadyExistAttr.Name = "tsmiFixSettingAlreadyExistAttr";
    componentResourceManager.ApplyResources((object) this.tsmiFixSettingAlreadyExistAttr, "tsmiFixSettingAlreadyExistAttr");
    this.tsmiFixSettingAlreadyExistAttr.Click += new EventHandler(this.tsmiFixSettingAlreadyExistAttr_Click);
    this.gvEntities.GridControl = this.gctlEntities;
    componentResourceManager.ApplyResources((object) this.gvEntities, "gvEntities");
    this.gvEntities.Name = "gvEntities";
    this.gvEntities.OptionsBehavior.AllowIncrementalSearch = true;
    this.gvEntities.OptionsBehavior.Editable = false;
    this.gvEntities.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvEntities_CustomDrawCell);
    this.gvEntities.RowCellStyle += new RowCellStyleEventHandler(this.gvEntities_RowCellStyle);
    this.gvEntities.ShowGridMenu += new GridMenuEventHandler(this.gvEntities_ShowGridMenu);
    this.gvEntities.ShowCustomizationForm += new EventHandler(this.gvEntities_ShowCustomizationForm);
    this.gvEntities.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gvEntity_FocusedRowChanged);
    this.gvEntities.BeforeLeaveRow += new RowAllowEventHandler(this.gvEntities_BeforeLeaveRow);
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.gctlEntities);
    this.splitContainer1.Panel2.BackColor = SystemColors.Control;
    this.splitContainer1.Panel2.Controls.Add((Control) this.pgEntProperty);
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel4, "tableLayoutPanel4");
    this.tableLayoutPanel4.Controls.Add((Control) this.lblFormCaption, 0, 0);
    this.tableLayoutPanel4.Controls.Add((Control) this.splitContainer1, 0, 1);
    this.tableLayoutPanel4.Controls.Add((Control) this.panel1, 0, 2);
    this.tableLayoutPanel4.Name = "tableLayoutPanel4";
    this.panel1.BackColor = SystemColors.Control;
    this.panel1.Controls.Add((Control) this.tableLayoutPanel5);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel5, "tableLayoutPanel5");
    this.tableLayoutPanel5.Controls.Add((Control) this.btnCancel, 4, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.btnApply, 3, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.btnSave, 0, 0);
    this.tableLayoutPanel5.Controls.Add((Control) this.btnLoad, 1, 0);
    this.tableLayoutPanel5.Name = "tableLayoutPanel5";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel4);
    this.Name = nameof (EntitiesPumpSettings);
    this.Tag = (object) " ";
    this.cmPumpTypes.ResumeLayout(false);
    this.gctlEntities.EndInit();
    this.cmsEntities.ResumeLayout(false);
    this.gvEntities.EndInit();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.tableLayoutPanel4.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.tableLayoutPanel5.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
