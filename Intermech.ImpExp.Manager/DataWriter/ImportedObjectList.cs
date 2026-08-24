// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportedObjectList
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

public class ImportedObjectList : IImportedObjectList, IImportedAttributeList
{
  private IDBImporter dbImporter;
  private DataWriterImpl dataWriter;
  private bool _newObjectsOnlyInList = true;
  private MetadataInfo _matadataInfo;
  internal ImportedObjectListItems items = new ImportedObjectListItems();
  private Dictionary<int, Exception> _errors = new Dictionary<int, Exception>();
  protected int packetSize;

  public bool NewObjectsOnlyInList
  {
    get => this._newObjectsOnlyInList;
    set => this._newObjectsOnlyInList = value;
  }

  private MetadataInfo matadataInfo
  {
    get
    {
      if (this._matadataInfo == null)
        this._matadataInfo = ServicesManager.GetService(typeof (IMetadataInfo)) as MetadataInfo;
      return this._matadataInfo;
    }
  }

  public event AfterImportEventDelegate AfterImportEvent;

  public Exception GetImportError(int index)
  {
    Exception importError = (Exception) null;
    this._errors.TryGetValue(index, out importError);
    return importError;
  }

  public IImportedObjectListItems Items => (IImportedObjectListItems) this.items;

  public int PacketSize
  {
    get => this.packetSize;
    set
    {
      if (this.packetSize == value)
        return;
      this.packetSize = value;
    }
  }

  internal ImportedObjectList(DataWriterImpl writer)
    : this(writer, -1)
  {
  }

  internal ImportedObjectList(DataWriterImpl writer, int packetSize)
  {
    this.dataWriter = writer;
    this.dbImporter = writer.metadataInfo.dbImporter;
    if (packetSize == -1)
      this.packetSize = (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
    else
      this.packetSize = packetSize;
  }

  private ImportingObject _addObject(ObjectRecord objRec)
  {
    return this._addObject(objRec, (AttributeRecord[]) null, (LCStepRecord[]) null);
  }

  private ImportingObject _addObject(ObjectRecord objRec, AttributeRecord[] objAttrs)
  {
    return this._addObject(objRec, objAttrs, (LCStepRecord[]) null);
  }

  private ImportingObject _addObject(
    ObjectRecord objRec,
    AttributeRecord[] objAttrs,
    LCStepRecord[] objSteps)
  {
    if (this.packetSize > 0 && this.packetSize == this.items.Count)
      this.Import();
    ImportingObject io = new ImportingObject(objRec);
    if (objSteps != null)
    {
      foreach (LCStepRecord objStep in objSteps)
        io.AddLCStep(objStep);
    }
    else
    {
      LCStepRecord step = new LCStepRecord(objRec.Id, objRec.Lc_step, DateTime.Now);
      io.AddLCStep(step);
    }
    if (objAttrs != null)
    {
      foreach (AttributeRecord objAttr in objAttrs)
        io.AddAttribute(objAttr);
    }
    this.items.Add(io);
    return io;
  }

  public void UseObject(ObjectRecord obj)
  {
    if (this.items.UseObject(obj.Object_id))
      return;
    this.items.Add(new ImportingObject(obj));
  }

  public void UseObject(long objectID)
  {
    if (this.packetSize > 0 && this.packetSize == this.items.Count)
      this.Import();
    if (this.items.UseObject(objectID))
      return;
    ObjectRecord objectRecord = new ObjectRecord();
    objectRecord.Object_id = objectID;
    DictionaryValue info = this.matadataInfo.ImportedObjects.GetInfo(objectID);
    if (info == null)
      throw new Exception("Объект не был импортирован !!!!");
    objectRecord.VersionId = -1;
    objectRecord.Id = info.NewObjectID;
    objectRecord.ObjectType = (info.Tag as ObjectInfo).ObjectType;
    this.items.Add(new ImportingObject(objectRecord));
  }

  public void UseObject(Guid objectGuid, long objectID)
  {
    if (this.packetSize > 0 && this.packetSize == this.items.Count)
      this.Import();
    if (this.items.UseObject(objectGuid))
      return;
    this.UseObject(objectID);
  }

  public ObjectRecord AddObject(int objType, int owner)
  {
    return this.AddObject(objType, owner, string.Empty);
  }

  public ObjectRecord AddObject(int objType, int owner, string caption)
  {
    int lcStep = 0;
    int versionId = 0;
    int objVerType = 0;
    int lewelId = 0;
    DateTime universalTime = DateTime.Now.ToUniversalTime();
    DateTime createDate = universalTime;
    string caption1 = caption;
    return this.AddObject(objType, owner, lcStep, versionId, 0, objVerType, universalTime, lewelId, createDate, caption1);
  }

  public ObjectRecord AddObject(
    int objType,
    int owner,
    int lcStep,
    int versionId,
    int userId,
    int objVerType,
    DateTime modifDate,
    int lewelId,
    DateTime createDate,
    string caption)
  {
    ObjectRecord objRec = new ObjectRecord();
    objRec.Object_id = 0L;
    objRec.ObjectGuid = (object) this.matadataInfo.NewPumpGuid();
    objRec.Id = 0L;
    objRec.IdGuid = (object) this.matadataInfo.NewPumpGuid();
    objRec.Lc_step = lcStep;
    objRec.VersionId = 0;
    objRec.ParentVersionId = -1L;
    if (userId != 0)
    {
      DictionaryValue dictionaryValue = this.matadataInfo.ImportedUsers.GetValue(userId);
      objRec.ChkoutBy = dictionaryValue == null ? this.matadataInfo.UserID : dictionaryValue.NewObjectID;
      objRec.ChkoutGuid = (object) (dictionaryValue == null ? this.matadataInfo.UserGUID : (dictionaryValue.Tag as UserTag).Guid);
    }
    else
    {
      objRec.ChkoutBy = this.matadataInfo.UserID;
      objRec.ChkoutGuid = (object) this.matadataInfo.UserGUID;
    }
    objRec.ObjectVerType = 0;
    objRec.ObjectType = objType;
    if (owner != 0)
    {
      DictionaryValue dictionaryValue = this.matadataInfo.ImportedUsers.GetValue(owner);
      objRec.OwnerId = dictionaryValue == null ? this.matadataInfo.UserID : dictionaryValue.NewObjectID;
      objRec.OwnerGuid = (object) (dictionaryValue == null || dictionaryValue.Tag == null ? this.matadataInfo.UserGUID : (dictionaryValue.Tag as UserTag).Guid);
    }
    else
    {
      objRec.OwnerId = this.matadataInfo.UserID;
      objRec.OwnerGuid = (object) this.matadataInfo.UserGUID;
    }
    objRec.ModifyDate = modifDate;
    objRec.LevelId = lewelId;
    objRec.ObjCreate = createDate;
    objRec.Caption = caption;
    if (objRec.Lc_step == 0)
      objRec.Lc_step = this.dataWriter.getObjTypeLcStep(objType);
    if (objRec.LevelId == 0)
      objRec.LevelId = this.dataWriter.getLcStepLevelID(objRec.Lc_step);
    objRec.IsBaseVersion = true;
    objRec.SiteID = string.Empty;
    this._addObject(objRec);
    return objRec;
  }

  public ObjectRecord AddObject(ObjectRecord obj)
  {
    this._addObject(obj);
    return obj;
  }

  public AttributeRecord AddAttribute(
    int attrType,
    AttrValueType attrValtype,
    object attrVal,
    int numInList)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType, numInList);
    if (attrVal != null)
    {
      if (attrVal != DBNull.Value)
      {
        try
        {
          switch (attrValtype)
          {
            case AttrValueType.stringVal:
              newAttributeRecord.StringValue = (object) Convert.ToString(attrVal);
              break;
            case AttrValueType.integerVal:
              newAttributeRecord.IntegerValue = (object) Convert.ToInt64(attrVal);
              break;
            case AttrValueType.doubleVal:
              newAttributeRecord.DoubleValue = (object) Convert.ToDouble(attrVal, (IFormatProvider) CultureInfo.InvariantCulture);
              break;
            case AttrValueType.datetimeVal:
              newAttributeRecord.DateValue = (object) Convert.ToDateTime(attrVal, (IFormatProvider) CultureInfo.InvariantCulture);
              break;
          }
        }
        catch
        {
        }
      }
    }
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttribute(AttributeRecord attr)
  {
    this.items[this.items.CurrentIndex].AddAttribute(attr);
    return attr;
  }

  public AttributeRecord AddAttributeNull(int attrType) => this.AddAttributeNull(attrType, 0);

  public AttributeRecord AddAttributeNull(int attrType, int numInList)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType, numInList);
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeStr(int attrType, string value)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType);
    newAttributeRecord.StringValue = (object) value;
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeInt(int attrType, long value)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType);
    newAttributeRecord.IntegerValue = (object) value;
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeDouble(int attrType, double value)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType);
    newAttributeRecord.DoubleValue = (object) value;
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeDate(int attrType, DateTime value)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType);
    newAttributeRecord.DateValue = (object) AttributesHelper.CorrectDbDateTimeValue(value);
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeLink(int attrType, long value, string caption)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType);
    newAttributeRecord.IntegerValue = (object) value;
    newAttributeRecord.StringValue = (object) caption;
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeLink(int attrType, long value, string caption, int inListID)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType, inListID);
    newAttributeRecord.IntegerValue = (object) value;
    newAttributeRecord.StringValue = (object) caption;
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeMeasure(
    int attrType,
    double value,
    long measureID,
    string strValue)
  {
    return this.AddAttributeMeasure(attrType, value, measureID, strValue, 0);
  }

  public AttributeRecord AddAttributeMeasure(
    int attrType,
    double value,
    long measureID,
    string strValue,
    int inListID)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType);
    newAttributeRecord.DoubleValue = (object) value;
    newAttributeRecord.IntegerValue = (object) measureID;
    newAttributeRecord.StringValue = (object) strValue;
    newAttributeRecord.InlistId = inListID;
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public AttributeRecord AddAttributeBlob(
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod)
  {
    return this.AddAttributeBlob(attrType, filePath, fileSize, fileNote, arcMethod, 0);
  }

  public AttributeRecord AddAttributeBlob(
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod,
    int inListId)
  {
    AttributeRecord newAttributeRecord = this.dataWriter.getNewAttributeRecord(0L, attrType);
    newAttributeRecord.Path2File = filePath;
    newAttributeRecord.FileSize = (object) fileSize;
    newAttributeRecord.FileNote = (object) fileNote;
    newAttributeRecord.StringValue = (object) fileNote;
    newAttributeRecord.ArcMethod = (object) (int) arcMethod;
    newAttributeRecord.InlistId = inListId;
    this.items[this.items.CurrentIndex].AddAttribute(newAttributeRecord);
    return newAttributeRecord;
  }

  public void ReplaceAttributeStr(int attributeId, string newValue)
  {
    if (this.items.CurrentIndex < 0)
      return;
    ImportingObject importingObject = this.items[this.items.CurrentIndex];
    if (importingObject.Attributes == null)
      return;
    for (int index = 0; index < importingObject.Attributes.Count; ++index)
    {
      if (importingObject.Attributes[index].AttributeId == attributeId)
      {
        importingObject.Attributes[index].StringValue = (object) newValue;
        break;
      }
    }
  }

  public void Import()
  {
    if (new Random().Next(0, 25) == 0)
      this.matadataInfo.UserSession.Test();
    lock (this)
    {
      IImportedObjectInfo[] importedObjectInfoArray1 = (IImportedObjectInfo[]) null;
      this._errors = new Dictionary<int, Exception>(this.items.Count);
      int num = 3;
      while (importedObjectInfoArray1 == null)
      {
        if (num > 0)
        {
          try
          {
            importedObjectInfoArray1 = this.dbImporter.ImportObjects(this.items.ToArray(), false);
          }
          catch (Exception ex)
          {
            ex.Data[(object) "DbImportException"] = (object) true;
            throw;
          }
          --num;
        }
        else
          break;
      }
      if (importedObjectInfoArray1 != null)
      {
        ImportedObjectListItems importedObjectListItems = new ImportedObjectListItems();
        for (int index = 0; index < importedObjectInfoArray1.Length; ++index)
        {
          IImportedObjectInfo importedObjectInfo = importedObjectInfoArray1[index] ?? (IImportedObjectInfo) new ImportedObjectInfo(new Exception("Неизвестная ошибка импорта"));
          if (importedObjectInfo.ObjectID != 0L)
          {
            if (this.items[index].Object.Object_id == 0L || this.items[index].Object.Object_id == -1L)
            {
              this.items[index].Object.Id = importedObjectInfo.ID;
              this.items[index].Object.Object_id = importedObjectInfo.ObjectID;
              this.matadataInfo.ImportedObjects.AddValue(importedObjectInfo.ObjectID, importedObjectInfo.ID, this.items[index].Object.ObjectType, (Guid) this.items[index].Object.ObjectGuid, (Guid) this.items[index].Object.IdGuid);
              importedObjectListItems.Add(this.items[index]);
            }
            else if (!this._newObjectsOnlyInList)
              importedObjectListItems.Add(this.items[index]);
          }
          else
          {
            if (this.items[index].Tag != null)
              importedObjectListItems.Add(this.items[index]);
            else
              importedObjectListItems.Add((ImportingObject) null);
            this._errors.Add(importedObjectListItems.Count - 1, importedObjectInfo.ImportMessage);
          }
        }
        this.items = importedObjectListItems;
      }
      else
      {
        IImportedObjectInfo[] importedObjectInfoArray2 = new IImportedObjectInfo[0];
      }
    }
    AfterImportEventDelegate afterImportEvent = this.AfterImportEvent;
    if (afterImportEvent != null)
      afterImportEvent((object) this, new EventArgs());
    if (this.packetSize <= 0)
      return;
    this.items.Clear();
    this._errors.Clear();
  }

  public void AddItem(ImportingObject importingObject)
  {
    this._addObject(importingObject.Object, importingObject.Attributes?.ToArray(), importingObject.LCSteps?.ToArray());
  }
}
