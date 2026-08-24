// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.Decorators.ImportedObjectListDecorator
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter.Decorators;

internal class ImportedObjectListDecorator : IImportedObjectList, IImportedAttributeList
{
  private readonly IImportedObjectList _origin;

  protected Guid OwnerGuid { get; }

  protected virtual void InternalAfterImportEventDelegate(object sender, EventArgs e)
  {
    AfterImportEventDelegate afterImportEvent = this.AfterImportEvent;
    if (afterImportEvent == null)
      return;
    afterImportEvent(sender, e);
  }

  public ImportedObjectListDecorator(IImportedObjectList origin, Guid ownerGuid)
  {
    this._origin = origin;
    this._origin.AfterImportEvent += new AfterImportEventDelegate(this.InternalAfterImportEventDelegate);
    this.OwnerGuid = ownerGuid;
  }

  public virtual AttributeRecord AddAttribute(
    int attrType,
    AttrValueType attrValtype,
    object attrVal,
    int numInList)
  {
    return this._origin.AddAttribute(attrType, attrValtype, attrVal, numInList);
  }

  public virtual AttributeRecord AddAttribute(AttributeRecord relAttr)
  {
    return this._origin.AddAttribute(relAttr);
  }

  public virtual AttributeRecord AddAttributeNull(int attrType)
  {
    return this._origin.AddAttributeNull(attrType);
  }

  public virtual AttributeRecord AddAttributeNull(int attrType, int numInList)
  {
    return this._origin.AddAttributeNull(attrType, numInList);
  }

  public virtual AttributeRecord AddAttributeStr(int attrType, string value)
  {
    return this._origin.AddAttributeStr(attrType, value);
  }

  public virtual AttributeRecord AddAttributeInt(int attrType, long value)
  {
    return this._origin.AddAttributeInt(attrType, value);
  }

  public virtual AttributeRecord AddAttributeDouble(int attrType, double value)
  {
    return this._origin.AddAttributeDouble(attrType, value);
  }

  public virtual AttributeRecord AddAttributeDate(int attrType, DateTime value)
  {
    return this._origin.AddAttributeDate(attrType, value);
  }

  public virtual AttributeRecord AddAttributeLink(int attrType, long value, string caption)
  {
    return this._origin.AddAttributeLink(attrType, value, caption);
  }

  public virtual AttributeRecord AddAttributeLink(
    int attrType,
    long value,
    string caption,
    int inListID)
  {
    return this._origin.AddAttributeLink(attrType, value, caption, inListID);
  }

  public virtual AttributeRecord AddAttributeMeasure(
    int attrType,
    double value,
    long measureID,
    string strValue)
  {
    return this._origin.AddAttributeMeasure(attrType, value, measureID, strValue);
  }

  public virtual AttributeRecord AddAttributeMeasure(
    int attrType,
    double value,
    long measureID,
    string strValue,
    int inListID)
  {
    return this._origin.AddAttributeMeasure(attrType, value, measureID, strValue, inListID);
  }

  public virtual AttributeRecord AddAttributeBlob(
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod)
  {
    return this._origin.AddAttributeBlob(attrType, filePath, fileSize, fileNote, arcMethod);
  }

  public virtual AttributeRecord AddAttributeBlob(
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod,
    int inListId)
  {
    return this._origin.AddAttributeBlob(attrType, filePath, fileSize, fileNote, arcMethod, inListId);
  }

  public virtual void ReplaceAttributeStr(int attributeId, string newValue)
  {
    this._origin.ReplaceAttributeStr(attributeId, newValue);
  }

  public IImportedObjectListItems Items => this._origin.Items;

  public int PacketSize
  {
    get => this._origin.PacketSize;
    set => this._origin.PacketSize = value;
  }

  public bool NewObjectsOnlyInList
  {
    get => this._origin.NewObjectsOnlyInList;
    set => this._origin.NewObjectsOnlyInList = value;
  }

  public event AfterImportEventDelegate AfterImportEvent;

  public virtual Exception GetImportError(int index) => this._origin.GetImportError(index);

  public virtual ObjectRecord AddObject(int objType, int owner)
  {
    return this._origin.AddObject(objType, owner);
  }

  public virtual ObjectRecord AddObject(int objType, int owner, string caption)
  {
    return this._origin.AddObject(objType, owner, caption);
  }

  public virtual ObjectRecord AddObject(ObjectRecord obj) => this._origin.AddObject(obj);

  public virtual ObjectRecord AddObject(
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
    return this._origin.AddObject(objType, owner, lcStep, versionId, userId, objVerType, modifDate, lewelId, createDate, caption);
  }

  public virtual void AddItem(ImportingObject importingObject)
  {
    this._origin.AddItem(importingObject);
  }

  public virtual void Import() => this._origin.Import();

  public virtual void UseObject(long objectID) => this._origin.UseObject(objectID);

  public virtual void UseObject(ObjectRecord obj) => this._origin.UseObject(obj);

  public virtual void UseObject(Guid objectGuid, long objectID)
  {
    this._origin.UseObject(objectGuid, objectID);
  }
}
