// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SubstitutesDescriptor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.VirtualNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm;

public class SubstitutesDescriptor : HiveDescriptor
{
  internal static int ProjectRelationTypeID = -1;
  private string _filtrationOwnerID;
  private List<long> _contexts;
  private int _objType;
  private long _objID;
  private long _ID;
  private int _relationTypeID;
  private new string _caption;
  private long _checkedOutBy;
  private long _version;
  private long _baseVersion;
  private string _siteID;
  private List<NodeColumnID> _attributes = new List<NodeColumnID>();
  private object[] _values = new object[0];
  private IServiceProvider _services;

  internal static void CorrectStatics()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      SubstitutesDescriptor.ProjectRelationTypeID = SubstitutesDescriptor.ProjectRelationTypeID == -1 ? sessionKeeper.Session.IdentHelper.SPRelationTypeID : SubstitutesDescriptor.ProjectRelationTypeID;
  }

  public string FiltrationOwnerID
  {
    [DebuggerStepThrough] get => this._filtrationOwnerID;
    set
    {
      this._filtrationOwnerID = value != string.Empty ? value : "cad001e2-306c-11d8-b4e9-00304f19f545";
    }
  }

  public List<long> Contexts
  {
    [DebuggerStepThrough] get => this._contexts;
    set
    {
      if (value == null || value.Count <= 0)
        return;
      this._contexts = new List<long>(value.Count);
      for (int index = 0; index < value.Count; ++index)
        this._contexts.Add(value[index]);
    }
  }

  public int ObjType
  {
    [DebuggerStepThrough] get => this._objType;
  }

  public long ObjID
  {
    [DebuggerStepThrough] get => this._objID;
    set
    {
      if (this._objID == value)
        return;
      this._objID = value;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._objID);
        this._objType = dbObject.ObjectType;
        this._ID = dbObject.ID;
        this._checkedOutBy = dbObject.CheckoutBy;
        this._caption = dbObject.Caption;
        for (int index = 0; index < this._attributes.Count; ++index)
        {
          IDBAttribute byId = dbObject.Attributes.FindByID((int) this._attributes[index].ID);
          this[(int) this._attributes[index].ID] = byId?.Value;
        }
      }
    }
  }

  public int RelationTypeID
  {
    [DebuggerStepThrough] get => this._relationTypeID;
    set => this._relationTypeID = value >= 0 ? value : SubstitutesDescriptor.ProjectRelationTypeID;
  }

  public new string Caption
  {
    [DebuggerStepThrough] get => this._caption;
    set => this._caption = value;
  }

  public long CheckedOutBy
  {
    [DebuggerStepThrough] get => this._checkedOutBy;
    set => this._checkedOutBy = value;
  }

  public long Version
  {
    [DebuggerStepThrough] get => this._version;
    set => this._version = value;
  }

  public long BaseVersion
  {
    [DebuggerStepThrough] get => this._baseVersion;
    set => this._baseVersion = value;
  }

  public string SiteID
  {
    [DebuggerStepThrough] get => this._siteID;
    set => this._siteID = value;
  }

  public List<NodeColumnID> Attributes
  {
    [DebuggerStepThrough] get => this._attributes;
    set
    {
      this._attributes = value != null ? value : new List<NodeColumnID>();
      if (this._values != null && this._values.Length == this._attributes.Count)
        return;
      this._values = new object[this._attributes.Count];
    }
  }

  public object[] Values
  {
    [DebuggerStepThrough] get => this._values;
    set
    {
      this._values = value == null || value.Length != this._attributes.Count ? new object[this._attributes.Count] : value;
    }
  }

  public object this[int attributeID]
  {
    get
    {
      for (int index = 0; index < this._attributes.Count; ++index)
      {
        if (this._attributes[index].ID.Equals((object) attributeID))
          return this._values[index];
      }
      return (object) null;
    }
    set
    {
      for (int index = 0; index < this._attributes.Count; ++index)
      {
        if (this._attributes[index].ID.Equals((object) attributeID))
          this._values[index] = value;
      }
    }
  }

  public SubstitutesDescriptor(
    int categoryID,
    int typeID,
    IServiceProvider services,
    string filtrationOwnerID,
    List<long> contexts,
    long objID,
    int objType,
    int relationTypeID,
    string caption,
    long checkedOutBy,
    long version,
    long baseVersion,
    List<NodeColumnID> attributes)
    : base(categoryID, typeID, caption)
  {
    SubstitutesDescriptor.CorrectStatics();
    this._services = services;
    this.FiltrationOwnerID = filtrationOwnerID;
    this.Contexts = contexts;
    this.CheckedOutBy = checkedOutBy;
    this.Attributes = attributes;
    this.ObjID = objID;
    this.RelationTypeID = relationTypeID;
    this.Version = version;
    this.BaseVersion = baseVersion;
  }

  [DebuggerStepThrough]
  public override PersistentState Serialize(INodeID nodeID) => (PersistentState) null;

  [DebuggerStepThrough]
  public new virtual INodeID Deserialize(PersistentState persistNodeID) => (INodeID) null;

  public override INodeID GetRecordNodeID()
  {
    return (INodeID) new SubstitutesNodeID((CreateObjectNodeParams) new CreateSubstituteNodeParams(this.ObjType, this.ObjID, this._ID, this.CheckedOutBy, 0L, 0, this.Caption, this.RelationTypeID, 0L, 0L, ObjectFiltrationState.fsNotRequired, this.Version, this.BaseVersion, this.SiteID, 0L, this.FiltrationOwnerID, this.Contexts, this.ObjType, this.ObjID, this.Attributes, this.Values, 0L, 0L), this._services);
  }

  public override INode GetChild(INodeID nodeID)
  {
    return !(nodeID is SubstitutesNodeID substitutesNodeId) ? base.GetChild(nodeID) : (INode) new SubstitutesNode(substitutesNodeId.Services, substitutesNodeId.FiltrationOwnerID, substitutesNodeId.Contexts, substitutesNodeId.ProjObjType, substitutesNodeId.ProjID, substitutesNodeId.ObjectID, substitutesNodeId.ObjectTypeID, substitutesNodeId.RelationTypeID, substitutesNodeId.PrjLinkID, substitutesNodeId.LCStepID, substitutesNodeId.Caption, substitutesNodeId.SubstitutesGroupNoID, substitutesNodeId.SubstituteInGroup, substitutesNodeId.CheckedOutBy, substitutesNodeId.Owner, substitutesNodeId.Sorting, substitutesNodeId.Version, substitutesNodeId.BaseVersion, substitutesNodeId.Attributes, substitutesNodeId.Values);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    if (dataFormat == typeof (IDescriptor))
      return (object) new SubstitutesDescriptor(this._categoryID, this._typeID, this._services, this.FiltrationOwnerID, this.Contexts, this.ObjID, this.ObjType, this.RelationTypeID, this.Caption, this.CheckedOutBy, this.Version, this.BaseVersion, this.Attributes);
    if (dataFormat == typeof (ICanOpenInNewWindow))
      return (object) new CanOpenInNewWindow();
    if (nodeID is SubstitutesNodeID substitutesNodeId)
    {
      if (dataFormat == typeof (IDBTypedObjectID))
        return (object) new DBTypedObjectID(substitutesNodeId.ObjectTypeID, substitutesNodeId.ObjectID, substitutesNodeId.ID, substitutesNodeId.Caption, substitutesNodeId.Owner, substitutesNodeId.Version, substitutesNodeId.BaseVersion, substitutesNodeId.SiteID, substitutesNodeId.ModificationID);
      if (dataFormat == typeof (IDBObjectID))
        return (object) new DBObjectID(substitutesNodeId.ObjectID, substitutesNodeId.ID, substitutesNodeId.Caption, substitutesNodeId.Owner);
      if (dataFormat == typeof (IDBRelationID))
        return (object) new DBRelationID(substitutesNodeId.PrjLinkID, substitutesNodeId.ObjectID, substitutesNodeId.RelationTypeID, substitutesNodeId.Sorting, substitutesNodeId.RelGuid, substitutesNodeId.ProjID);
      if (dataFormat == typeof (IDBObjectTypeID))
        return (object) new DBObjectTypeID(substitutesNodeId.ObjectTypeID);
      if (dataFormat == typeof (IDBCheckedOutByID))
        return (object) new DBCheckedOutByID(substitutesNodeId.ObjectID, substitutesNodeId.CheckedOutBy, substitutesNodeId.Owner);
    }
    return base.GetData(nodeID, dataFormat);
  }
}
