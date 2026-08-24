// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.GroupAttribute
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class GroupAttribute
{
  public bool ExistInBase = true;
  public int Sort;
  public int Flags;
  public long Width;
  public int DataMode;
  public int Required;
  public int DataType;
  public int EnterMode;
  public int AttrFieldType;
  public int PumpPosible;
  public int Key;
  public string Field = string.Empty;
  public string Units = string.Empty;
  public string Data = string.Empty;
  public string LongName = string.Empty;
  public Guid AttrGuid = Guid.Empty;

  public GroupAttribute()
  {
  }

  public GroupAttribute(
    int sort,
    int flags,
    long width,
    int dataMode,
    int required,
    int dataType,
    int enterMode,
    int attrFieldType,
    int pumpPosible,
    int key,
    string field,
    string units,
    string data,
    string longName,
    bool existInBase,
    Guid attrGuid)
  {
    this.Sort = sort;
    this.Flags = flags;
    this.Width = width;
    this.DataMode = dataMode;
    this.Required = required;
    this.DataType = dataType;
    this.EnterMode = enterMode;
    this.AttrFieldType = attrFieldType;
    this.PumpPosible = pumpPosible;
    this.Key = key;
    this.ExistInBase = existInBase;
    this.Field = field;
    this.Units = units;
    this.Data = data;
    this.LongName = longName;
    this.AttrGuid = attrGuid;
  }
}
