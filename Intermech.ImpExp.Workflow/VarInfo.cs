// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.VarInfo
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Workflow;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class VarInfo
{
  private readonly string _name;
  private readonly int _attrID;
  private readonly VarType _type;
  private readonly Guid _attrGuid;
  public string PossibleValues;

  public string Name => this._name;

  public int AttrID => this._attrID;

  public VarType Type => this._type;

  public Guid AttrGuid => this._attrGuid;

  public VarInfo(DataRow row)
  {
    this._name = row["F_NAME"].ToString();
    this._attrID = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
    this._attrGuid = new Guid(row["F_GUID"].ToString());
    this._type = MiscFunx.DetermineVarType(row);
  }

  public VarInfo(string name, int id, VarType type, Guid attrGuid)
  {
    this._name = name;
    this._attrID = id;
    this._type = type;
    this._attrGuid = attrGuid;
  }
}
