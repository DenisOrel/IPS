// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseFormulaParser
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal sealed class ImbaseFormulaParser : FormulaParser
{
  private IDictionary<string, IImFieldsItem> _fields;

  public ImbaseFormulaParser(IImFieldsItem fieldItem, IDictionary<string, IImFieldsItem> fields)
    : base(fieldItem.AttrGuid, fieldItem.AttrFieldType)
  {
    this._fields = fields;
  }

  protected override SortedDictionary<string, string> GetFieldsList()
  {
    SortedDictionary<string, string> fieldsList = new SortedDictionary<string, string>((IComparer<string>) new locComparer());
    foreach (KeyValuePair<string, IImFieldsItem> field in (IEnumerable<KeyValuePair<string, IImFieldsItem>>) this._fields)
    {
      IAttributeTypeItem byGuid = ImbasePumpServiceImpl.imPlugin.Imdi.AttributeTypes.GetByGuid(field.Value.AttrGuid);
      if (byGuid != null)
        fieldsList.Add(field.Key, byGuid.GUID.ToString());
    }
    for (int index = 0; index < 999; ++index)
    {
      string key = "F" + index.ToString();
      if (!fieldsList.ContainsKey(key))
        fieldsList.Add(key, string.Empty);
    }
    return fieldsList;
  }

  protected override string GetFieldGuid(string field)
  {
    if (!this._fields.ContainsKey(field))
      return field;
    IAttributeTypeItem byGuid = ImbasePumpServiceImpl.imPlugin.Imdi.AttributeTypes.GetByGuid(this._fields[field].AttrGuid);
    return byGuid == null ? field : byGuid.GUID.ToString();
  }

  protected override string GetFieldStr(string fldStr)
  {
    IImFieldsItem imFieldsItem;
    if (!this._fields.TryGetValue(fldStr, out imFieldsItem))
      return fldStr;
    IAttributeTypeItem byGuid = ImbasePumpServiceImpl.imPlugin.Imdi.AttributeTypes.GetByGuid(imFieldsItem.AttrGuid);
    if (byGuid == null)
      return fldStr;
    if (byGuid.AttrValueType == 3 || byGuid.AttrValueType == 13)
    {
      int num = Math.Abs(imFieldsItem.Required);
      if (num > 0 && num < 15)
        return $"{$"STR([{byGuid.GUID}],'"}{string.Format((IFormatProvider) CultureInfo.InvariantCulture, $"{{0{$":F{num}"}}}", (object) 0)}')";
    }
    return base.GetFieldStr(fldStr);
  }

  protected override FormulaParser.AttributeInfo GetAttributeInfo(Guid attributeGuid)
  {
    IAttributeTypeItem byGuid = ImbasePumpServiceImpl.imPlugin.Imdi.AttributeTypes.GetByGuid(attributeGuid);
    return byGuid != null ? new FormulaParser.AttributeInfo(byGuid.GUID, (FieldTypes) byGuid.AttrValueType) : (FormulaParser.AttributeInfo) null;
  }

  protected override bool IsNumberAttribute(FieldTypes dataType, Guid attrGUID)
  {
    switch (dataType)
    {
      case FieldTypes.ftInteger:
      case FieldTypes.ftDouble:
      case FieldTypes.ftExternalLink:
      case FieldTypes.ftObjectLink:
      case FieldTypes.ftMeasured:
      case FieldTypes.ftAutoInc:
        return true;
      case FieldTypes.ftSystem:
        IDBAttributeType attributeType = ImbasePumpServiceImpl.imPlugin.Idw.GetUserSession().GetAttributeType(attrGUID);
        if (attributeType != null)
          return attributeType.ValueFieldName.Equals("F_INTEGER_VALUE");
        break;
    }
    return false;
  }
}
