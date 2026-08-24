// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.Decorators.ImportedRelationListDecorator
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter.Decorators;

internal class ImportedRelationListDecorator : IImportedRelationList, IImportedAttributeList
{
  private readonly IImportedRelationList _origin;

  protected Guid OwnerGuid { get; }

  protected virtual void InternalAfterImportEventDelegate(object sender, EventArgs e)
  {
    AfterImportEventDelegate afterImportEvent = this.AfterImportEvent;
    if (afterImportEvent == null)
      return;
    afterImportEvent(sender, e);
  }

  public ImportedRelationListDecorator(IImportedRelationList origin, Guid ownerGuid)
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

  public IImportedRelationListItems Items => this._origin.Items;

  public int PacketSize
  {
    get => this._origin.PacketSize;
    set => this._origin.PacketSize = value;
  }

  public event AfterImportEventDelegate AfterImportEvent;

  public virtual RelationRecord AddRelation(long projId, long partId, int relType)
  {
    return this._origin.AddRelation(projId, partId, relType);
  }

  public virtual RelationRecord AddRelation(RelationRecord rel) => this._origin.AddRelation(rel);

  public virtual RelationRecord AddRelation(
    long projId,
    long partId,
    int relType,
    DateTime crtDate)
  {
    return this._origin.AddRelation(projId, partId, relType, crtDate);
  }

  public virtual RelationRecord AddRelationFromID(long projId, long partId, int relType)
  {
    return this._origin.AddRelationFromID(projId, partId, relType);
  }

  public virtual RelationRecord AddRelationFromID(
    long projId,
    long partId,
    int relType,
    DateTime crtDate)
  {
    return this._origin.AddRelationFromID(projId, partId, relType, crtDate);
  }

  public virtual void UseRelation(long prjLinkID) => this._origin.UseRelation(prjLinkID);

  public virtual void UseRelation(RelationRecord rel) => this._origin.UseRelation(rel);

  public virtual void Import() => this._origin.Import();

  public virtual ImportingRelationCreator ImportingRelationCreator
  {
    get => this._origin.ImportingRelationCreator;
    set => this._origin.ImportingRelationCreator = value;
  }
}
