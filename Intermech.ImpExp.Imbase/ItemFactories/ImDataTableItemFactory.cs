// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImDataTableItemFactory
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImDataTableItemFactory : PumpItemFactory
{
  private int idxKEY = -1;
  protected IImportingData cacheData;
  protected List<GroupAttribute> FieldsList = new List<GroupAttribute>();
  public Dictionary<Guid, Type> FieldsTypes = new Dictionary<Guid, Type>();
  private Dictionary<string, long> _blobFIlesCache;
  private DataTableItemOptions _options;

  public ImDataTableItemFactory(
    IImportingData cacheData,
    string tabName,
    IDataReader idr,
    IAppManager appMgr,
    ICollection<GroupAttribute> fieldsCollection,
    DataTableItemOptions options)
    : base(tabName, idr, appMgr)
  {
    this.cacheData = cacheData;
    this.idxKEY = this.getFieldIndex("F_KEY");
    if (fieldsCollection != null)
    {
      List<Guid> guidList = new List<Guid>(fieldsCollection.Count);
      foreach (GroupAttribute fields in (IEnumerable<GroupAttribute>) fieldsCollection)
      {
        if (fields.ExistInBase && !guidList.Contains(fields.AttrGuid))
        {
          guidList.Add(fields.AttrGuid);
          this.FieldsList.Add(fields);
          Type type = typeof (string);
          switch (fields.DataType)
          {
            case 2:
              type = typeof (long);
              break;
            case 3:
              type = typeof (double);
              break;
            case 4:
              type = typeof (bool);
              break;
          }
          this.FieldsTypes.Add(fields.AttrGuid, type);
        }
      }
    }
    Dictionary<object, DictionaryValue> category = cacheData.GetCategory(ImportingCategory.ImbaseBlobs);
    this._blobFIlesCache = new Dictionary<string, long>(category.Count);
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
    {
      string lower = keyValuePair.Value.Caption.ToLower();
      if (!this._blobFIlesCache.ContainsKey(lower))
        this._blobFIlesCache.Add(lower, keyValuePair.Value.NewObjectID);
    }
    this._options = options;
  }

  private string AddMeasureValueToList(
    IImportedObjectList objList,
    int attributeID,
    string defVal,
    IMeasureItem measure,
    object fieldValue,
    bool isNew)
  {
    string strValue = string.Empty;
    double num;
    try
    {
      strValue = Convert.ToString(fieldValue);
      if (strValue.Equals(string.Empty))
        strValue = defVal;
      strValue = strValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
      num = measure == null || strValue.Equals(string.Empty) ? -1.0 : Convert.ToDouble(strValue);
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage($"Значение \"{fieldValue}\" не приведено к вещественному значению, выраженному в ед.измерения: {ex.Message}");
      num = -1.0;
    }
    if (num == -1.0)
    {
      objList.AddAttributeNull(attributeID).IsNew = isNew;
    }
    else
    {
      long measureID;
      if (measure.BaseMeasureId != 0L)
      {
        num *= measure.Koef;
        measureID = measure.BaseMeasureId;
      }
      else
      {
        this.appMngr.AddWarningMessage($"Значение \"{num}\" не приведено к базовой единице измерения, т.к. не найдена базовая ед. измерения для ед. измерения \"{measure.LongName}\"");
        measureID = measure.Id;
      }
      strValue = $"{strValue} {measure.ShortName}";
      objList.AddAttributeMeasure(attributeID, num, measureID, strValue).IsNew = isNew;
    }
    return strValue;
  }

  public void AddFieldsToRecord(
    long keyID,
    bool isFolder,
    ImDataTableItem record,
    IImportedObjectList objList,
    IImportingData cacheData,
    List<int> presentAttributes = null)
  {
    int num = 13;
    IMeasures service1 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    IPhysicalValues service2 = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
    IMetadataInfo service3 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    Dictionary<string, ImDataTableItemFactory.FieldValueInfo> fieldValues = new Dictionary<string, ImDataTableItemFactory.FieldValueInfo>(this.FieldsList.Count);
    bool flag = false;
    if (service3 != null && service1 != null)
    {
      List<Tuple<int, string>> items1 = new List<Tuple<int, string>>();
      List<Tuple<int, string>> items2 = new List<Tuple<int, string>>();
      foreach (GroupAttribute fields in this.FieldsList)
      {
        try
        {
          IAttributeTypeItem byGuid = service3.AttributeTypes.GetByGuid(fields.AttrGuid);
          if (byGuid != null)
          {
            if (record.FieldsValues.ContainsKey(fields.Field))
            {
              AttributeRecord attributeRecord = (AttributeRecord) null;
              bool isNew = presentAttributes == null || !presentAttributes.Contains(byGuid.ID);
              string defVal = string.Empty;
              if (!fields.Data.Equals(string.Empty) && fields.DataMode == 0 && fields.EnterMode == 1)
                defVal = byGuid.MaxSize <= 0 || byGuid.MaxSize >= fields.Data.Length ? fields.Data : fields.Data.Substring(0, byGuid.MaxSize);
              IMeasureItem measure = (IMeasureItem) null;
              if (!fields.Units.Equals(string.Empty) && byGuid.AttrValueType == num)
              {
                long newKey = cacheData.GetNewKey(ImportingCategory.ImbaseBindedMeasures, (object) fields.Units);
                if (newKey != 0L)
                  measure = service1.GetMeasure(newKey);
                string list = this.AddMeasureValueToList(objList, byGuid.ID, defVal, measure, record.FieldsValues[fields.Field], isNew);
                fieldValues.Add(fields.Field, new ImDataTableItemFactory.FieldValueInfo(byGuid.ID, byGuid.AttrValueType, (object) list));
              }
              else
              {
                bool isFormula = false;
                object fieldValue = record.FieldsValues[fields.Field];
                if (fieldValue == null && defVal.Equals(string.Empty))
                {
                  attributeRecord = objList.AddAttributeNull(byGuid.ID);
                }
                else
                {
                  switch (byGuid.AttrValueType)
                  {
                    case 1:
                      if (fieldValue == null || fieldValue.Equals((object) string.Empty))
                        fieldValue = (object) defVal;
                      string str = Convert.ToString(fieldValue);
                      if (str.Contains("{F") && str.Contains("}"))
                      {
                        flag = true;
                        isFormula = true;
                      }
                      attributeRecord = objList.AddAttributeStr(byGuid.ID, Convert.ToString(fieldValue));
                      break;
                    case 2:
                      if (fieldValue == null || fieldValue.Equals((object) string.Empty))
                      {
                        if (defVal != string.Empty)
                          fieldValue = (object) Convert.ToInt32(defVal);
                      }
                      else
                        fieldValue = (object) Convert.ToInt32(fieldValue);
                      attributeRecord = fieldValue == null || fieldValue.Equals((object) string.Empty) ? objList.AddAttributeNull(byGuid.ID) : objList.AddAttributeInt(byGuid.ID, (long) (int) fieldValue);
                      break;
                    case 3:
                      if (fieldValue == null || fieldValue.Equals((object) string.Empty))
                      {
                        if (defVal != string.Empty)
                          fieldValue = (object) Convert.ToDouble(defVal);
                      }
                      else
                        fieldValue = (object) Convert.ToDouble(fieldValue);
                      attributeRecord = fieldValue == null || fieldValue.Equals((object) string.Empty) ? objList.AddAttributeNull(byGuid.ID) : objList.AddAttributeDouble(byGuid.ID, (double) fieldValue);
                      break;
                    case 8:
                      if (isFolder)
                      {
                        items1.Add(new Tuple<int, string>(byGuid.ID, Convert.ToString(fieldValue)));
                        break;
                      }
                      items2.Add(new Tuple<int, string>(byGuid.ID, Convert.ToString(fieldValue)));
                      break;
                    case 13:
                      if (byGuid.MaxSize != 0)
                      {
                        IPhysicalValueItem physicalValue = service2.GetPhysicalValue((long) byGuid.MaxSize);
                        if (physicalValue != null && physicalValue.DefaultMeasureID != 0L && physicalValue.DefaultMeasureID != -1L)
                          measure = service1.GetMeasure(physicalValue.DefaultMeasureID);
                      }
                      fieldValue = (object) this.AddMeasureValueToList(objList, byGuid.ID, defVal, measure, fieldValue, isNew);
                      break;
                  }
                }
                fieldValues.Add(fields.Field, new ImDataTableItemFactory.FieldValueInfo(byGuid.ID, byGuid.AttrValueType, fieldValue, isFormula));
              }
              if (attributeRecord != null)
                attributeRecord.IsNew = isNew;
            }
          }
        }
        catch
        {
        }
      }
      if (items1.Count > 0 && cacheData.GetNewKey(ImportingCategory.ImbaseLinksFolder, (object) keyID) == 0L)
        cacheData.AddValue(ImportingCategory.ImbaseLinksFolder, (object) keyID, long.MaxValue, (ITagImportObject) new LinkTag(items1));
      if (items2.Count > 0 && cacheData.GetNewKey(ImportingCategory.ImbaseLinksTableLinks, (object) keyID) == 0L)
        cacheData.AddValue(ImportingCategory.ImbaseLinksTableLinks, (object) keyID, long.MaxValue, (ITagImportObject) new LinkTag(items2));
    }
    if (!flag)
      return;
    foreach (KeyValuePair<string, ImDataTableItemFactory.FieldValueInfo> keyValuePair in fieldValues)
    {
      if (keyValuePair.Value.IsFormula)
        this.CalculateFormula(fieldValues, keyValuePair.Value, objList);
    }
  }

  private void CalculateFormula(
    Dictionary<string, ImDataTableItemFactory.FieldValueInfo> fieldValues,
    ImDataTableItemFactory.FieldValueInfo value,
    IImportedObjectList objList)
  {
    value.IsFormula = false;
    Regex regex = new Regex("\\{F\\d+\\}");
    string newValue = Convert.ToString(value.Value);
    string input = newValue;
    MatchCollection matchCollection = regex.Matches(input);
    for (int i = 0; i < matchCollection.Count; ++i)
    {
      string str = matchCollection[i].Value.Trim('{', '}');
      foreach (KeyValuePair<string, ImDataTableItemFactory.FieldValueInfo> fieldValue in fieldValues)
      {
        if (fieldValue.Key == str)
        {
          if (fieldValue.Value.IsFormula)
            this.CalculateFormula(fieldValues, fieldValue.Value, objList);
          newValue = newValue.Replace(matchCollection[i].Value, Convert.ToString(fieldValue.Value.Value));
          break;
        }
      }
    }
    objList.ReplaceAttributeStr(value.AttributeID, newValue);
    value.Value = (object) newValue;
  }

  protected void addFields(ImDataTableItem record, IDataReader idr)
  {
    record.RecKey = this.getInt32(idr, this.idxKEY);
    if (this.FieldsList == null)
      return;
    foreach (GroupAttribute fields in this.FieldsList)
    {
      int fieldIndex = this.getFieldIndex(fields.Field);
      if (fieldIndex > -1)
      {
        object obj = (object) null;
        if (fields.DataMode == 2 || fields.DataMode == 3)
        {
          obj = this.getObject(idr, fieldIndex);
          if (obj != null && obj != DBNull.Value)
          {
            int result = -1;
            if (AttributesHelper.IsNumericType(obj.GetType()))
              obj = (object) this.cacheData.GetNewKey(ImportingCategory.ImbaseBlobs, (object) Convert.ToInt32(obj));
            else if (obj is string)
            {
              if (int.TryParse((string) obj, out result))
              {
                obj = (object) this.cacheData.GetNewKey(ImportingCategory.ImbaseBlobs, (object) result);
              }
              else
              {
                long num;
                if (this._blobFIlesCache.TryGetValue(((string) obj).ToLower(), out num))
                  obj = (object) num;
              }
            }
            if ((this._options & DataTableItemOptions.ImageLinkGuids) > DataTableItemOptions.None && obj is long num1 && num1 != 0L)
            {
              Guid objectGuid = ServicesManager.GetService<IMetadataInfo>().ImportedObjects.GetObjectGUID((long) obj);
              if (objectGuid != Guid.Empty)
                obj = (object) objectGuid.ToString();
            }
          }
        }
        else
        {
          switch (fields.DataType)
          {
            case 1:
            case 5:
            case 6:
              string oldKey = this.getObjectString(idr, fieldIndex).Trim();
              if (oldKey != null && !oldKey.Equals(string.Empty) && fields.DataMode == 1)
              {
                if (this.cacheData.IsCategoryPresent(ImportingCategory.ImbaseMixTables) && this.cacheData.GetNewKey(ImportingCategory.ImbaseMixTables, (object) oldKey) != 0L)
                  record.IsMixTableLink = true;
                else
                  record.IsTableLink = true;
              }
              obj = (object) oldKey;
              break;
            case 2:
            case 8:
              obj = this.getObject(idr, fieldIndex);
              break;
            case 3:
              obj = this.getObject(idr, fieldIndex);
              break;
            case 4:
              obj = this.getObject(idr, fieldIndex);
              if (CompareValuesHelper.NormalizedValue(obj) != null && obj.GetType() != typeof (bool))
              {
                string lower = Convert.ToString(obj).ToLower();
                switch (lower)
                {
                  case "t":
                  case "1":
                  case "true":
                  case "+":
                    obj = (object) true;
                    break;
                  case "f":
                    obj = (object) false;
                    break;
                  default:
                    if (!lower.Trim().Equals(string.Empty))
                    {
                      try
                      {
                        obj = (object) Convert.ToBoolean(obj);
                        break;
                      }
                      catch
                      {
                        obj = (object) false;
                        break;
                      }
                    }
                    else
                      goto case "f";
                }
              }
              else
                break;
              break;
          }
        }
        record.FieldsValues.Add(fields.Field, obj);
        record.Data.Add(fields.AttrGuid, obj);
      }
    }
  }

  public override object NewItem(IDataReader idr)
  {
    ImDataTableItem record = new ImDataTableItem();
    this.addFields(record, idr);
    return (object) record;
  }

  private class FieldValueInfo
  {
    public int AttributeID;
    public int FieldType;
    public object Value;
    public bool IsFormula;

    public FieldValueInfo(int attrID, int type, object value)
      : this(attrID, type, value, false)
    {
    }

    public FieldValueInfo(int attrID, int type, object value, bool isFormula)
    {
      this.AttributeID = attrID;
      this.FieldType = type;
      this.Value = value;
      this.IsFormula = isFormula;
    }
  }
}
