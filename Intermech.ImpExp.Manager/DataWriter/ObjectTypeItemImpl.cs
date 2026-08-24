// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ObjectTypeItemImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class ObjectTypeItemImpl : 
  AttributableTypeItem,
  IObjectTypeItem,
  ITypeItem,
  IAttributableTypeItem
{
  private string shortName = string.Empty;
  private string objectName = string.Empty;
  private Guid relationID = Guid.Empty;
  private string note = string.Empty;
  private string area = string.Empty;
  private ObjectVersionModes versionableMode;
  private int captionAttributeID;
  private lcType publicLifeCycle;
  private int daysBeforeDelete;
  private Guid shemaId = Guid.Empty;
  private Guid parentID = Guid.Empty;
  private bool anyAttribute;
  private List<int> childIDs = new List<int>();

  public ObjectTypeItemImpl(
    IDataWriterProxy dataWriter,
    int typeId,
    Guid relationId,
    Guid typeGuid,
    string name,
    string shortName,
    string objectName,
    string area,
    ObjectVersionModes versionableMode,
    int captionAttributeID,
    lcType publicLifeCycle,
    int daysBeforeDelete,
    Guid shemaId,
    bool anyAttr,
    string note,
    byte[] icon)
    : base(dataWriter, typeId, typeGuid, name)
  {
    this.shortName = shortName;
    this.objectName = objectName;
    this.relationID = relationId;
    this.area = area;
    this.versionableMode = versionableMode;
    this.captionAttributeID = captionAttributeID;
    this.publicLifeCycle = publicLifeCycle;
    this.daysBeforeDelete = daysBeforeDelete;
    this.shemaId = shemaId;
    this.anyAttribute = anyAttr;
    this.note = note;
    this.Icon = icon;
  }

  public void childIdAdd(int childID)
  {
    if (this.childIDs.Contains(childID))
      return;
    this.childIDs.Add(childID);
  }

  public string ShortName
  {
    get => this.shortName;
    set
    {
      if (!(this.shortName != value))
        return;
      this.shortName = value;
    }
  }

  public string ObjectName
  {
    get => this.objectName;
    set
    {
      if (!(this.objectName != value))
        return;
      this.objectName = value;
    }
  }

  public Guid RelationID
  {
    get => this.relationID;
    set
    {
      if (!(this.relationID != value))
        return;
      this.relationID = value;
    }
  }

  public ObjectVersionModes VersionableMode
  {
    get => this.versionableMode;
    set
    {
      if (this.versionableMode == value)
        return;
      this.versionableMode = value;
    }
  }

  public string Area
  {
    get => this.area;
    set
    {
      if (!(this.area != value))
        return;
      this.area = value;
    }
  }

  public int CaptionAttributeID
  {
    get => this.captionAttributeID;
    set
    {
      if (this.captionAttributeID == value)
        return;
      this.captionAttributeID = value;
    }
  }

  public lcType PublicLifeCycle
  {
    get => this.publicLifeCycle;
    set
    {
      if (this.publicLifeCycle == value)
        return;
      this.publicLifeCycle = value;
    }
  }

  public int DaysBeforeDelete
  {
    get => this.daysBeforeDelete;
    set
    {
      if (this.daysBeforeDelete == value)
        return;
      this.daysBeforeDelete = value;
    }
  }

  public Guid ShemaId
  {
    get => this.shemaId;
    set
    {
      if (!(this.shemaId != value))
        return;
      this.shemaId = value;
    }
  }

  public Guid ParentID
  {
    get => this.parentID;
    set
    {
      if (!(this.parentID != value))
        return;
      this.parentID = value;
    }
  }

  public bool AnyAttribute => this.anyAttribute;

  public int[] ChildIDs => this.childIDs.ToArray();

  public bool ChildExists(int childID) => this.childIDs.Contains(childID);

  public string Note
  {
    get => this.note;
    set => this.note = value;
  }

  public byte[] Icon { get; set; }
}
