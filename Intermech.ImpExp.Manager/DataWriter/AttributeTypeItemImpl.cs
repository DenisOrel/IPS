// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.AttributeTypeItemImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class AttributeTypeItemImpl : TypeItemImpl, IAttributeTypeItem, ITypeItem
{
  private int _attrValueType;
  private MultiValueModes _multiValueMode;
  private int _maxSize;
  private string _shortName = string.Empty;
  private string _alias = string.Empty;
  private bool _existsInBase;
  private List<IAttributePossibleValue> possibleValues = new List<IAttributePossibleValue>();
  private string _valueFieldName = string.Empty;

  public object DefaultValue { get; set; }

  public AttributeTypeItemImpl(
    IDataWriterProxy dataWriter,
    int attrTypeID,
    int attrValueType,
    Guid attrGuid,
    string attrName,
    string shortName,
    string alias,
    MultiValueModes multiValueMode,
    int maxSize,
    object defValue)
    : base(dataWriter, attrTypeID, attrGuid, attrName)
  {
    this._shortName = shortName;
    this._alias = alias;
    this._attrValueType = attrValueType;
    this._multiValueMode = multiValueMode;
    this._maxSize = maxSize;
    this.DefaultValue = defValue;
  }

  public int AttrValueType
  {
    get => this._attrValueType;
    set
    {
      if (this._attrValueType == value)
        return;
      this._attrValueType = value;
    }
  }

  public MultiValueModes MultiValueMode
  {
    get => this._multiValueMode;
    set
    {
      if (this._multiValueMode == value)
        return;
      this._multiValueMode = value;
    }
  }

  public int MaxSize
  {
    get => this._maxSize;
    set
    {
      if (this._maxSize == value)
        return;
      this._maxSize = value;
    }
  }

  public string ShortName
  {
    get => this._shortName;
    set
    {
      if (!(this._shortName != value))
        return;
      this._shortName = value;
    }
  }

  public string Alias
  {
    get => this._alias;
    set
    {
      if (!(this._alias != value))
        return;
      this._alias = value;
    }
  }

  public bool ExistsInBase
  {
    get => this._existsInBase;
    set
    {
      if (this._existsInBase == value)
        return;
      this._existsInBase = value;
    }
  }

  public IAttributePossibleValue[] GetPossibleValues() => this.possibleValues.ToArray();

  internal void addPossibleValue(IAttributePossibleValue possibleValue)
  {
    this.possibleValues.Add(possibleValue);
  }

  public void AddPossibleValue(IAttributePossibleValue possibleValue)
  {
    if (this.IsExistsPossibleValue(possibleValue))
      return;
    this.dw.CreateAttributePossibleValue(this.ID, possibleValue);
  }

  public bool AddPossibleValue(int inListID, object value, string description)
  {
    try
    {
      AttributePossibleValueImpl possibleValue;
      switch (this.valueFieldName)
      {
        case "F_INTEGER_VALUE":
          possibleValue = new AttributePossibleValueImpl(inListID, description, Convert.ToInt32(value));
          break;
        case "F_DOUBLE_VALUE":
          possibleValue = new AttributePossibleValueImpl(inListID, description, Convert.ToDouble(value));
          break;
        case "F_DATE_VALUE":
          possibleValue = !value.Equals((object) Consts.CurrentDateFunction) ? new AttributePossibleValueImpl(inListID, description, Convert.ToDateTime(value)) : new AttributePossibleValueImpl(inListID, description, Convert.ToString(value));
          break;
        default:
          possibleValue = new AttributePossibleValueImpl(inListID, description, Convert.ToString(value));
          break;
      }
      this.AddPossibleValue((IAttributePossibleValue) possibleValue);
      return true;
    }
    catch
    {
      return false;
    }
  }

  private string valueFieldName
  {
    get
    {
      if (this._valueFieldName.Equals(string.Empty))
      {
        if (this.AttrValueType == 13)
        {
          this._valueFieldName = "F_STRING_VALUE";
        }
        else
        {
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          List<FieldTypes> convertList = new List<FieldTypes>();
          RelationalOperators[] enabledOperators = (RelationalOperators[]) null;
          bool computableAttribute = false;
          AttributeCacheHelper.GetAttributeTypeValues((FieldTypes) this.AttrValueType, this.ID, ref this._valueFieldName, ref empty1, ref convertList, ref enabledOperators, ref computableAttribute, ref empty2);
        }
      }
      return this._valueFieldName;
    }
  }

  public bool IsExistsPossibleValue(IAttributePossibleValue possibleValue)
  {
    bool flag = false;
    if (MultiValueModesHelper.IsValuedFromList(this._multiValueMode))
    {
      foreach (IAttributePossibleValue possibleValue1 in this.possibleValues)
      {
        switch (this.valueFieldName)
        {
          case "F_INTEGER_VALUE":
            flag = possibleValue1.ValueInteger.Equals(possibleValue.ValueInteger);
            break;
          case "F_DOUBLE_VALUE":
            flag = possibleValue1.ValueDouble.Equals(possibleValue.ValueDouble);
            break;
          case "F_DATE_VALUE":
            flag = possibleValue1.ValueDateTime.Equals(possibleValue.ValueDateTime);
            break;
          default:
            flag = possibleValue1.ValueString.Equals(possibleValue.ValueString);
            break;
        }
        if (flag)
          break;
      }
    }
    return flag;
  }
}
