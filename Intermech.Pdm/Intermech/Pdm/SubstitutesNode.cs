// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SubstitutesNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm;

internal class SubstitutesNode : CompositeNode
{
  private IServiceProvider _services;
  private int _categoryID;
  private int _typeID;
  private string _filtrationOwnerID;
  private List<long> _contexts;
  private int _projObjType;
  private long _projID;
  private int _objType;
  private long _objID;
  private int _relationTypeID;
  private long _prjLinkID;
  private int _lcStepID;
  private string _caption;
  private long _substitutesGroupNoID;
  private long _substituteInGroup;
  private long _checkedOutBy;
  private long _owner;
  private long _sorting;
  private long _version;
  private long _baseVersion;
  private List<NodeColumnID> _attributes = new List<NodeColumnID>();
  private object[] _values = new object[0];

  internal int CategoryID
  {
    [DebuggerStepThrough] get => this._categoryID;
  }

  internal int TypeID
  {
    [DebuggerStepThrough] get => this._typeID;
  }

  internal string FiltrationOwnerID
  {
    [DebuggerStepThrough] get => this._filtrationOwnerID;
  }

  internal List<long> Contexts
  {
    [DebuggerStepThrough] get => this._contexts;
  }

  internal int ProjObjType
  {
    [DebuggerStepThrough] get => this._projObjType;
  }

  internal long ProjID
  {
    [DebuggerStepThrough] get => this._projID;
  }

  internal long ObjID
  {
    [DebuggerStepThrough] get => this._objID;
  }

  internal int ObjType
  {
    [DebuggerStepThrough] get => this._objType;
  }

  internal int RelationTypeID
  {
    [DebuggerStepThrough] get => this._relationTypeID;
  }

  internal long PrjLinkID
  {
    [DebuggerStepThrough] get => this._prjLinkID;
  }

  internal int LCStepID
  {
    [DebuggerStepThrough] get => this._lcStepID;
  }

  internal string Caption
  {
    [DebuggerStepThrough] get => this._caption;
  }

  internal long SubstitutesGroupNoID
  {
    [DebuggerStepThrough] get => this._substitutesGroupNoID;
  }

  internal long SubstituteInGroup
  {
    [DebuggerStepThrough] get => this._substituteInGroup;
  }

  internal long CheckedOutBy
  {
    [DebuggerStepThrough] get => this._checkedOutBy;
  }

  internal long Owner
  {
    [DebuggerStepThrough] get => this._owner;
  }

  internal long Sorting
  {
    [DebuggerStepThrough] get => this._sorting;
  }

  internal long Version
  {
    [DebuggerStepThrough] get => this._version;
  }

  internal long BaseVersion
  {
    [DebuggerStepThrough] get => this._baseVersion;
  }

  internal List<NodeColumnID> Attributes
  {
    [DebuggerStepThrough] get => this._attributes;
  }

  internal object[] Values
  {
    [DebuggerStepThrough] get => this._values;
  }

  internal object this[int attributeID]
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
  }

  public SubstitutesNode(
    IServiceProvider services,
    string filtrationOwnerID,
    List<long> contexts,
    int projObjType,
    long projID,
    long objID,
    int objType,
    int relationTypeID,
    long prjLinkID,
    int lcStepID,
    string caption,
    long substitutesGroupNoID,
    long substituteInGroup,
    long checkedOutBy,
    long owner,
    long sorting,
    long version,
    long baseVersion,
    List<NodeColumnID> attributes,
    object[] values)
  {
    this._services = services;
    this._filtrationOwnerID = filtrationOwnerID;
    this._contexts = contexts;
    this._projObjType = projObjType;
    this._projID = projID;
    this._objID = objID;
    this._objType = objType;
    this._relationTypeID = relationTypeID;
    this._prjLinkID = prjLinkID;
    this._lcStepID = lcStepID;
    this._caption = caption;
    this._substitutesGroupNoID = substitutesGroupNoID;
    this._substituteInGroup = substituteInGroup;
    this._checkedOutBy = checkedOutBy;
    this._owner = owner;
    this._sorting = sorting;
    this._version = version;
    this._baseVersion = baseVersion;
    this._attributes = attributes;
    this._values = values;
    this.options = NodeOptions.CanContainsComposition;
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    if (this._projID != -1L)
      return this.SlotsFromSinglePart((INodePart) new SubstitutesPart(this._services, this._projObjType, this._projID, this._relationTypeID, this._filtrationOwnerID, this._contexts, this._attributes));
    return this._objID != -1L ? this.SlotsFromSinglePart((INodePart) new SubstitutesPart(this._services, this._objType, this._objID, this._relationTypeID, this._filtrationOwnerID, this._contexts, this._attributes)) : (List<PartSlot>) null;
  }
}
