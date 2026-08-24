// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportedRelationList
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class ImportedRelationList : IImportedRelationList, IImportedAttributeList
{
  private IDBImporter dbImporter;
  private DataWriterImpl dataWriter;
  private MetadataInfo _matadataInfo;
  internal ImportedRelationListItems items = new ImportedRelationListItems();
  protected int packetSize;
  private ImportingRelationCreator _importingRelationCreator;

  public ImportedRelationList(DataWriterImpl writer)
    : this(writer, -1)
  {
  }

  public ImportedRelationList(DataWriterImpl writer, int packetSize)
  {
    this.dataWriter = writer;
    this.dbImporter = writer.metadataInfo.dbImporter;
    if (packetSize == -1)
      this.packetSize = (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
    else
      this.packetSize = packetSize;
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

  public IImportedRelationListItems Items => (IImportedRelationListItems) this.items;

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

  public RelationRecord AddRelation(long projId, long partId, int relType)
  {
    return this.AddRelation(projId, partId, relType, DateTime.Now.ToUniversalTime());
  }

  public RelationRecord AddRelation(long projId, long partId, int relType, DateTime crtDate)
  {
    long id = this.matadataInfo.ImportedObjects.GetID(partId);
    if (id == 0L)
      throw new Exception($"Среди закаченных объектов не найден объект {partId}, который является участником создаваемой связи.");
    RelationRecord relRec = new RelationRecord(0L, (object) this.matadataInfo.NewPumpGuid(), (object) projId, (object) id, relType, (object) crtDate, 0L);
    this._addRelation(relRec);
    return relRec;
  }

  public RelationRecord AddRelation(RelationRecord rel)
  {
    this._addRelation(rel);
    return rel;
  }

  public RelationRecord AddRelationFromID(long projId, long partId, int relType, DateTime crtDate)
  {
    RelationRecord relRec = new RelationRecord(0L, (object) this.matadataInfo.NewPumpGuid(), (object) projId, (object) partId, relType, (object) crtDate, 0L);
    this._addRelation(relRec);
    return relRec;
  }

  public RelationRecord AddRelationFromID(long projId, long partId, int relType)
  {
    return this.AddRelationFromID(projId, partId, relType, DateTime.Now.ToUniversalTime());
  }

  public AttributeRecord AddAttribute(
    int attrType,
    AttrValueType attrValtype,
    object attrVal,
    int numInList)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    if (attrVal != null)
    {
      if (attrVal != DBNull.Value)
      {
        try
        {
          switch (attrValtype)
          {
            case AttrValueType.stringVal:
              relationAttributeRecord.StringValue = (object) Convert.ToString(attrVal);
              break;
            case AttrValueType.integerVal:
              relationAttributeRecord.IntegerValue = (object) Convert.ToInt64(attrVal);
              break;
            case AttrValueType.doubleVal:
              relationAttributeRecord.DoubleValue = (object) Convert.ToDouble(attrVal, (IFormatProvider) CultureInfo.InvariantCulture);
              break;
            case AttrValueType.datetimeVal:
              relationAttributeRecord.DateValue = (object) Convert.ToDateTime(attrVal, (IFormatProvider) CultureInfo.InvariantCulture);
              break;
          }
        }
        catch
        {
        }
      }
    }
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public AttributeRecord AddAttribute(AttributeRecord attr)
  {
    this.items[this.items.CurrentIndex].AddAttribute(attr);
    return attr;
  }

  public AttributeRecord AddAttributeNull(int attrType, int numInList)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType, numInList);
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public AttributeRecord AddAttributeNull(int attrType) => this.AddAttributeNull(attrType, 0);

  public AttributeRecord AddAttributeStr(int attrType, string value)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    relationAttributeRecord.StringValue = (object) value;
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public AttributeRecord AddAttributeInt(int attrType, long value)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    relationAttributeRecord.IntegerValue = (object) value;
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public AttributeRecord AddAttributeDouble(int attrType, double value)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    relationAttributeRecord.DoubleValue = (object) value;
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public AttributeRecord AddAttributeDate(int attrType, DateTime value)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    relationAttributeRecord.DateValue = (object) AttributesHelper.CorrectDbDateTimeValue(value);
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public AttributeRecord AddAttributeLink(int attrType, long value, string caption)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    relationAttributeRecord.IntegerValue = (object) value;
    relationAttributeRecord.StringValue = (object) caption;
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public AttributeRecord AddAttributeLink(int attrType, long value, string caption, int inListID)
  {
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType, inListID);
    relationAttributeRecord.IntegerValue = (object) value;
    relationAttributeRecord.StringValue = (object) caption;
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
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
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    relationAttributeRecord.DoubleValue = (object) value;
    relationAttributeRecord.IntegerValue = (object) measureID;
    relationAttributeRecord.StringValue = (object) strValue;
    relationAttributeRecord.InlistId = inListID;
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
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
    AttributeRecord relationAttributeRecord = this.dataWriter.getNewRelationAttributeRecord(0L, attrType);
    relationAttributeRecord.Path2File = filePath;
    relationAttributeRecord.FileSize = (object) fileSize;
    relationAttributeRecord.FileNote = (object) fileNote;
    relationAttributeRecord.StringValue = (object) fileNote;
    relationAttributeRecord.ArcMethod = (object) (int) arcMethod;
    relationAttributeRecord.InlistId = inListId;
    this.items[this.items.CurrentIndex].AddAttribute(relationAttributeRecord);
    return relationAttributeRecord;
  }

  public void ReplaceAttributeStr(int attributeId, string newValue)
  {
    if (this.items.CurrentIndex < 0)
      return;
    ImportingRelation importingRelation = this.items[this.items.CurrentIndex];
    if (importingRelation.Attributes == null)
      return;
    foreach (AttributeRecord attribute in importingRelation.Attributes)
    {
      if (attribute.AttributeId == attributeId)
      {
        attribute.StringValue = (object) newValue;
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
      long[] numArray1 = (long[]) null;
      int num = 3;
      while (numArray1 == null)
      {
        if (num > 0)
        {
          try
          {
            numArray1 = this.dbImporter.ImportRelations(this.items.ToArray(), false);
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
      if (numArray1 != null)
      {
        ImportedRelationListItems relationListItems = new ImportedRelationListItems();
        for (int index = 0; index < numArray1.Length; ++index)
        {
          if (numArray1[index] != 0L && numArray1[index] != -1L)
          {
            if (this.items[index].Relation.PrjLinkId == 0L || this.items[index].Relation.PrjLinkId == -1L)
            {
              this.items[index].Relation.PrjLinkId = numArray1[index];
              this.matadataInfo.ImportedRelations.AddValue(this.items[index].Relation.PrjLinkId, this.items[index].Relation.RelationType);
              relationListItems.Add(this.items[index]);
            }
          }
          else
            relationListItems.Add((ImportingRelation) null);
        }
        this.items = relationListItems;
      }
      else
      {
        long[] numArray2 = new long[0];
      }
    }
    if (this.AfterImportEvent != null)
      this.AfterImportEvent((object) this, new EventArgs());
    if (this.packetSize <= 0)
      return;
    this.items.Clear();
  }

  public void UseRelation(RelationRecord rel)
  {
    if (this.packetSize > 0 && this.packetSize == this.items.Count)
      this.Import();
    if (rel.PrjLinkId != 0L && rel.PrjLinkId != -1L)
    {
      if (this.items.UseRelation(rel.PrjLinkId))
        return;
      this.items.Add(new ImportingRelation(rel));
    }
    else
    {
      if (this.items.UseRelation(Convert.ToInt64(rel.ProjId), Convert.ToInt64(rel.PartId)))
        return;
      this.items.Add(new ImportingRelation(rel));
    }
  }

  public void UseRelation(long prjLinkID)
  {
    if (this.packetSize > 0 && this.packetSize == this.items.Count)
      this.Import();
    if (this.items.UseRelation(prjLinkID))
      return;
    RelationRecord rel = new RelationRecord();
    rel.PrjLinkId = prjLinkID;
    int relationTypeId = this.matadataInfo.ImportedRelations.GetRelationTypeID(prjLinkID);
    switch (relationTypeId)
    {
      case -1:
      case 0:
        throw new Exception("Связь не была импортирована !");
      default:
        rel.RelationType = relationTypeId;
        this.items.Add(new ImportingRelation(rel));
        break;
    }
  }

  private RelationRecord _addRelation(RelationRecord relRec, AttributeRecord[] relAttrs)
  {
    if (this.packetSize > 0 && this.packetSize == this.items.Count)
      this.Import();
    ImportingRelation io = this.ImportingRelationCreator == null ? new ImportingRelation(relRec) : this.ImportingRelationCreator(relRec);
    if (relAttrs != null)
    {
      foreach (AttributeRecord relAttr in relAttrs)
        io.AddAttribute(relAttr);
    }
    this.items.Add(io);
    return relRec;
  }

  private RelationRecord _addRelation(RelationRecord relRec)
  {
    return this._addRelation(relRec, (AttributeRecord[]) null);
  }

  public ImportingRelationCreator ImportingRelationCreator
  {
    get => this._importingRelationCreator;
    set => this._importingRelationCreator = value;
  }
}
