// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpSelections
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.SearchData.ItemFactories;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.SelectionService;
using Intermech.Kernel.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки выборок Search", "Перекачка данных о выборках Search")]
internal class PumpSelections(SearchDataPlugin plugin) : PumpClass((PluginClass) plugin)
{
  private Guid _objTypeArticles = new Guid("cad00268-306c-11d8-b4e9-00304f19f545");
  private Guid _objTypeDocuments = new Guid("cad00070-306c-11d8-b4e9-00304f19f545");
  private Dictionary<int, PumpSelections.SampleSelectionObjects> _sampleSelections;
  private const string _curDateTime = "Текущая дата";
  private long _kgObjectID;
  private int _relTypeDoc = -1;
  private int _objtypeDocID = -1;
  private int _objtypeArtID = -1;
  private int _attrtypePP;
  private int _attrLitera;
  private int _attrNote;
  private int _attrWeight;
  private int _attrCodeOKP;
  private int _attrVersionCode;
  private int _attrName;
  private int _attrDesignation;
  private int _attrPurchased;
  private int _attrImbaseKey;
  private int _attrFile;
  private int _attrArchive;
  private int _attrFormat;

  protected override Guid GUID => new Guid("105B0F1E-3BE8-4cbd-851C-0E8CB84BC3FC");

  private RelationalOperators GetOperator(int oldOper)
  {
    RelationalOperators relationalOperators = RelationalOperators.None;
    switch (oldOper)
    {
      case 0:
        relationalOperators = RelationalOperators.Equal;
        break;
      case 1:
        relationalOperators = RelationalOperators.NotEqual;
        break;
      case 2:
        relationalOperators = RelationalOperators.Substring;
        break;
      case 3:
        relationalOperators = RelationalOperators.Less;
        break;
      case 4:
        relationalOperators = RelationalOperators.LessOrEqual;
        break;
      case 5:
        relationalOperators = RelationalOperators.Greater;
        break;
      case 6:
        relationalOperators = RelationalOperators.GreaterOrEqual;
        break;
      case 7:
        relationalOperators = RelationalOperators.Between;
        break;
      case 8:
        relationalOperators = RelationalOperators.StartString;
        break;
      case 9:
        relationalOperators = RelationalOperators.EndString;
        break;
      case 10:
        relationalOperators = RelationalOperators.NOP;
        break;
      case 12:
        relationalOperators = RelationalOperators.None;
        break;
      case 13:
        relationalOperators = RelationalOperators.Equal;
        break;
      case 14:
        relationalOperators = RelationalOperators.NotEqual;
        break;
      case 15:
        relationalOperators = RelationalOperators.NotSubstring;
        break;
      case 16 /*0x10*/:
        relationalOperators = RelationalOperators.NOP;
        break;
      case 17:
        relationalOperators = RelationalOperators.NOP;
        break;
      case 18:
        relationalOperators = RelationalOperators.LastNDays;
        break;
    }
    return relationalOperators;
  }

  private void GetBetweenValues(
    string value,
    Type type,
    out object val1,
    out object val2,
    object throwValue)
  {
    object obj1 = (object) null;
    object obj2 = (object) null;
    try
    {
      int length = value.IndexOf(" .. ");
      if (length > 0)
      {
        if (type == typeof (DateTime))
        {
          string str1 = value.Substring(0, length);
          string str2 = value.Substring(length + 4);
          obj1 = !str1.Equals("Текущая дата") ? (object) Convert.ToDateTime(str1, (IFormatProvider) CultureInfo.CurrentCulture) : (object) Consts.CurrentDateFunction;
          obj2 = !str2.Equals("Текущая дата") ? (object) Convert.ToDateTime(str2, (IFormatProvider) CultureInfo.CurrentCulture) : (object) Consts.CurrentDateFunction;
        }
        else
        {
          obj1 = Convert.ChangeType((object) value.Substring(0, length), type, (IFormatProvider) CultureInfo.InvariantCulture);
          obj2 = Convert.ChangeType((object) value.Substring(length + 4), type, (IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
    }
    catch
    {
      val1 = throwValue;
      val2 = throwValue;
    }
    val1 = obj1;
    val2 = obj2;
  }

  private ConditionStructure[] SelectionConditions(
    List<string> selectionFLT,
    int archID,
    IImportingData cacheData)
  {
    string empty1 = string.Empty;
    Dictionary<string, PumpSelections.Condition> dictionary = new Dictionary<string, PumpSelections.Condition>(1);
    List<ConditionStructure> conditionStructureList1 = new List<ConditionStructure>();
    foreach (string str1 in selectionFLT)
    {
      if (!(str1 == string.Empty))
      {
        string empty2 = string.Empty;
        string[] strArray1 = str1.Split('.');
        string str2;
        int length;
        if (strArray1.Length == 3)
        {
          str2 = strArray1[0];
          length = strArray1[0].Length;
        }
        else
        {
          length = str1.IndexOf('.');
          if (length > 0)
            str2 = str1.Substring(0, length);
          else
            continue;
        }
        PumpSelections.Condition condition1;
        if (dictionary.ContainsKey(str2))
        {
          condition1 = dictionary[str2];
        }
        else
        {
          PumpSelections.Condition condition2 = new PumpSelections.Condition(str2);
          dictionary.Add(str2, condition2);
          condition1 = condition2;
        }
        string str3 = str1.Substring(length + 1);
        if (!str3.StartsWith("UPPER") && !str3.StartsWith("P_VALUE.UPPER"))
        {
          if (str3.StartsWith("OPERATION") || str3.StartsWith("P_VALUE.OPERATION"))
          {
            string[] strArray2 = str3.Remove(0, str3.StartsWith("P_VALUE") ? 17 : 9).Split('=');
            if (strArray2.Length == 2)
              condition1.AddOperator(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]));
          }
          else if (str3.StartsWith("VALUE") || str3.StartsWith("P_VALUE.VALUE"))
          {
            string[] strArray3 = str3.Remove(0, str3.StartsWith("P_VALUE") ? 13 : 5).Split('=');
            if (strArray3.Length == 2)
              condition1.Values.Add(Convert.ToInt32(strArray3[0]), (object) strArray3[1]);
          }
          else if (str3.StartsWith("LABEL") || str3.StartsWith("P_VALUE.LABEL"))
          {
            string[] strArray4 = str3.Remove(0, str3.StartsWith("P_VALUE") ? 13 : 5).Split('=');
            if (strArray4.Length == 2)
              condition1.Labels.Add(Convert.ToInt32(strArray4[0]), strArray4[1]);
          }
          else if (str3.StartsWith("USERID="))
          {
            string str4 = str3.Remove(0, 7);
            condition1.Users.Add(0, Convert.ToInt32(str4));
          }
          else if (str3.StartsWith("USER"))
          {
            string[] strArray5 = str3.Remove(0, 4).Split('=');
            if (strArray5.Length == 2)
              condition1.Users.Add(Convert.ToInt32(strArray5[0]), Convert.ToInt32(strArray5[1]));
          }
        }
      }
    }
    foreach (PumpSelections.Condition condition in dictionary.Values)
    {
      if (condition.IsValid)
      {
        List<ConditionStructure> conditionStructureList2 = new List<ConditionStructure>();
        for (int index = 0; index < condition.OperatorNo.Count; ++index)
        {
          RelationalOperators relationalOperators = this.GetOperator(condition.GetOperator(index));
          switch (condition.Alias)
          {
            case "AFILENAME":
            case "FILENAME":
              conditionStructureList2.Add(new ConditionStructure(this._attrFile, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "ARCHIVE_ID":
              object conditionValue1 = (object) null;
              if (condition.Values.Count > 0 && Convert.ToInt32(condition.GetValue(index)) > 0)
                conditionValue1 = (object) cacheData.GetNewKey(ImportingCategory.Archives, condition.GetValue(index));
              conditionStructureList2.Add(new ConditionStructure(this._attrArchive, relationalOperators, conditionValue1, LogicalOperators.AND, 0, true));
              continue;
            case "ARC_DIR_ID":
            case "DIR_NAME":
            case "NEED_SVOD_DOC":
            case "ПАПКА ПОЧТЫ":
            case "ПОДПИСИ":
            case "РАССЫЛКА":
              continue;
            case "ART_ID":
              object val1_1 = (object) 0L;
              object val2_1 = (object) 0L;
              if (condition.Values.Count > 0)
              {
                if (relationalOperators == RelationalOperators.Between)
                  this.GetBetweenValues(Convert.ToString(condition.GetValue(index)), typeof (long), out val1_1, out val2_1, (object) 0L);
                else
                  val1_1 = (object) cacheData.GetNewKey(ImportingCategory.Articles, condition.GetValue(index));
              }
              if ((long) val1_1 != 0L)
              {
                if ((long) val2_1 != 0L)
                  conditionStructureList2.Add(new ConditionStructure(-2, relationalOperators, val1_1, val2_1, LogicalOperators.AND, 0, true));
                else
                  conditionStructureList2.Add(new ConditionStructure(-2, relationalOperators, val1_1, LogicalOperators.AND, 0, true));
              }
              else
                conditionStructureList2.Add(new ConditionStructure(-2, relationalOperators, (object) null, LogicalOperators.AND, 0, true));
              if (this._objtypeArtID != -1)
              {
                conditionStructureList2.Add(new ConditionStructure(-7, RelationalOperators.Equal, (object) this._objtypeArtID, LogicalOperators.AND, 0, true));
                continue;
              }
              continue;
            case "AUTHOR":
            case "DESIGNERID":
              long conditionValue2 = 0;
              if (condition.Values.Count > 0)
                conditionValue2 = this.plugin.Imdi.ImportedUsers.GetNewKey(Convert.ToInt32(condition.GetValue(index)));
              if (conditionValue2 != 0L)
              {
                conditionStructureList2.Add(new ConditionStructure(-8, relationalOperators, (object) conditionValue2, LogicalOperators.AND, 0, true));
                continue;
              }
              conditionStructureList2.Add(new ConditionStructure(-8, relationalOperators, (object) null, LogicalOperators.AND, 0, true));
              continue;
            case "CHKINDATE":
            case "CREATEDATE":
              object val1_2 = (object) DateTime.MinValue;
              object val2_2 = (object) DateTime.MinValue;
              if (condition.Values.Count > 0 && condition.Values[0] != null)
              {
                switch (relationalOperators)
                {
                  case RelationalOperators.Between:
                    this.GetBetweenValues(Convert.ToString(condition.GetValue(index)), typeof (DateTime), out val1_2, out val2_2, (object) DateTime.MinValue);
                    break;
                  case RelationalOperators.LastNDays:
                    object conditionValue3;
                    try
                    {
                      conditionValue3 = (object) Convert.ToInt32(condition.GetValue(index));
                    }
                    catch
                    {
                      conditionValue3 = (object) 0;
                    }
                    conditionStructureList2.Add(new ConditionStructure(-13, relationalOperators, conditionValue3, LogicalOperators.AND, 0, true));
                    continue;
                  default:
                    try
                    {
                      val1_2 = Convert.ToString(condition.GetValue(index)).Equals("Текущая дата") ? (object) Consts.CurrentDateFunction : (object) Convert.ToDateTime(condition.Values[0], (IFormatProvider) CultureInfo.CurrentCulture);
                      break;
                    }
                    catch
                    {
                      val1_2 = (object) DateTime.MinValue;
                      break;
                    }
                }
              }
              if (Convert.ToString(val1_2).Equals(Consts.CurrentDateFunction) || (DateTime) val1_2 != DateTime.MinValue)
              {
                if (Convert.ToString(val2_2).Equals(Consts.CurrentDateFunction) || (DateTime) val2_2 != DateTime.MinValue)
                {
                  conditionStructureList2.Add(new ConditionStructure(-13, relationalOperators, val1_2, val2_2, LogicalOperators.AND, 0, true));
                  continue;
                }
                conditionStructureList2.Add(new ConditionStructure(-13, relationalOperators, val1_2, LogicalOperators.AND, 0, true));
                continue;
              }
              conditionStructureList2.Add(new ConditionStructure(-13, relationalOperators, (object) null, LogicalOperators.AND, 0, true));
              continue;
            case "DESIGNATIO":
              conditionStructureList2.Add(new ConditionStructure(this._attrDesignation, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "DOC_ID":
              object val1_3 = (object) 0L;
              object val2_3 = (object) 0L;
              if (condition.Values.Count > 0)
              {
                if (relationalOperators == RelationalOperators.Between)
                  this.GetBetweenValues(Convert.ToString(condition.GetValue(index)), typeof (long), out val1_3, out val2_3, (object) 0L);
                else
                  val1_3 = (object) cacheData.GetNewKey(ImportingCategory.Documents, condition.Values[index]);
              }
              if (val1_3 != null && (long) val1_3 != 0L)
              {
                if (val2_3 != null && (long) val2_3 != 0L)
                  conditionStructureList2.Add(new ConditionStructure(-2, relationalOperators, val1_3, val2_3, LogicalOperators.AND, 0, true));
                else
                  conditionStructureList2.Add(new ConditionStructure(-2, relationalOperators, val1_3, LogicalOperators.AND, 0, true));
              }
              else
                conditionStructureList2.Add(new ConditionStructure(-2, relationalOperators, (object) null, LogicalOperators.AND, 0, true));
              if (this._objtypeDocID != -1)
              {
                conditionStructureList2.Add(new ConditionStructure(-7, RelationalOperators.Equal, (object) this._objtypeDocID, LogicalOperators.AND, 0, true));
                continue;
              }
              continue;
            case "DOC_STATUS":
              object conditionValue4 = (object) null;
              if (condition.Values.Count > 0)
                conditionValue4 = Convert.ToInt32(condition.Values[0]) <= 0 ? (object) 0 : (object) this.plugin.Imdi.ImportedUsers.GetNewKey(Convert.ToInt32(condition.GetValue(index)));
              conditionStructureList2.Add(new ConditionStructure(-6, relationalOperators, conditionValue4, LogicalOperators.AND, 0, true));
              continue;
            case "DOC_TYPE":
              if (condition.Values.Count > 0)
              {
                long newKey = cacheData.GetNewKey(ImportingCategory.DocTypes, condition.GetValue(index));
                if (newKey != 0L)
                {
                  conditionStructureList2.Add(new ConditionStructure(-7, relationalOperators, (object) Convert.ToInt32(newKey), LogicalOperators.AND, 0, true));
                  continue;
                }
                conditionStructureList2.Add(new ConditionStructure(-7, relationalOperators, (object) null, LogicalOperators.AND, 0, true));
                continue;
              }
              conditionStructureList2.Add(new ConditionStructure(-7, relationalOperators, (object) null, LogicalOperators.AND, 0, true));
              continue;
            case "FORMAT":
              conditionStructureList2.Add(new ConditionStructure(this._attrFormat, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "IMBASE_KEY":
              conditionStructureList2.Add(new ConditionStructure(this._attrImbaseKey, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "ISP_CODE":
              conditionStructureList2.Add(new ConditionStructure(this._attrVersionCode, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "LITERA":
              conditionStructureList2.Add(new ConditionStructure(this._attrLitera, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "MASSA":
              object val1_4 = (object) double.MinValue;
              object val2_4 = (object) double.MinValue;
              if (condition.Values.Count > 0)
              {
                if (this._kgObjectID != 0L)
                {
                  try
                  {
                    if (relationalOperators == RelationalOperators.Between)
                      this.GetBetweenValues(Convert.ToString(condition.GetValue(index)), typeof (double), out val1_4, out val2_4, (object) double.MinValue);
                    else
                      val1_4 = (object) Convert.ToDouble(condition.GetValue(index));
                  }
                  catch
                  {
                  }
                }
              }
              conditionStructureList2.Add(new ConditionStructure(this._attrWeight, relationalOperators, (double) val1_4 != double.MinValue ? (object) new MeasuredValue((double) val1_4, this._kgObjectID) : (object) (MeasuredValue) null, (double) val2_4 != double.MinValue ? (object) new MeasuredValue((double) val2_4, this._kgObjectID) : (object) (MeasuredValue) null, LogicalOperators.AND, 0, true));
              continue;
            case "NAME":
              conditionStructureList2.Add(new ConditionStructure(this._attrName, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "NOTE":
              conditionStructureList2.Add(new ConditionStructure(this._attrNote, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "OKP_CODE":
              conditionStructureList2.Add(new ConditionStructure(this._attrCodeOKP, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
              continue;
            case "PR_ID":
              if (this._attrtypePP != 0)
              {
                conditionStructureList2.Add(new ConditionStructure(this._attrtypePP, relationalOperators, condition.GetValue(index), LogicalOperators.AND, 0, true));
                continue;
              }
              continue;
            case "PURCHASED":
              object conditionValue5 = (object) null;
              if (condition.Values.Count > 0)
              {
                if (condition.GetValue(index).Equals((object) string.Empty) || condition.GetValue(index).Equals((object) "-"))
                  conditionValue5 = (object) 1;
                else if (condition.GetValue(index).Equals((object) "+"))
                  conditionValue5 = (object) 2;
                else if (condition.GetValue(index).Equals((object) "*"))
                  conditionValue5 = (object) 3;
              }
              conditionStructureList2.Add(new ConditionStructure(this._attrPurchased, relationalOperators, conditionValue5, LogicalOperators.AND, 0, true));
              continue;
            case "SECTION_ID":
              long num1 = -1;
              if (condition.Values.Count > 0)
                num1 = cacheData.GetNewKey(ImportingCategory.ArticleTypes, (object) Convert.ToInt32(condition.GetValue(index)));
              conditionStructureList2.Add(new ConditionStructure(-7, relationalOperators, (object) Convert.ToInt32(num1), LogicalOperators.AND, 0, true));
              continue;
            case "ЗАПРОС SQL":
              if (condition.Values.Count > 0)
              {
                conditionStructureList2.Add(new ConditionStructure($"/* {condition.Values[0]} */", LogicalOperators.AND, 0, (object) null));
                continue;
              }
              continue;
            case "ТИП ДОКУМЕНТОВ, ПРИСОЕДИНЕННЫХ К ОБЪЕКТУ":
              RelationalOperators relationalOperator = RelationalOperators.ConsistFromType;
              long num2 = 0;
              if (condition.Values.Count > 0)
                num2 = cacheData.GetNewKey(ImportingCategory.DocTypes, condition.GetValue(index));
              if (num2 != 0L)
                conditionStructureList2.Add(new ConditionStructure((string) null, relationalOperator, (object) Convert.ToInt32(num2), LogicalOperators.AND, 0, true));
              else
                conditionStructureList2.Add(new ConditionStructure((string) null, relationalOperator, (object) null, LogicalOperators.AND, 0, true));
              if (this._relTypeDoc != -1)
              {
                conditionStructureList2.Add(new ConditionStructure(-23, RelationalOperators.Equal, (object) this._relTypeDoc, LogicalOperators.AND, 0, false));
                continue;
              }
              conditionStructureList2.Add(new ConditionStructure(-23, RelationalOperators.Equal, (object) null, LogicalOperators.AND, 0, false));
              continue;
            default:
              long num3 = 0;
              if (condition.Alias.StartsWith("PV"))
              {
                string str5 = condition.Alias.Replace("_", string.Empty);
                int oldKey = -1;
                if (str5.Length > 3)
                {
                  string str6 = str5.Remove(0, 3);
                  try
                  {
                    oldKey = Convert.ToInt32(str6, 16 /*0x10*/);
                  }
                  catch
                  {
                  }
                  if (oldKey != -1)
                    num3 = cacheData.GetNewKey(ImportingCategory.ThematicParams, (object) oldKey);
                }
              }
              if (num3 != 0L)
              {
                this.AddCustomAttribute(Convert.ToInt32(num3), relationalOperators, condition.GetValue(index), conditionStructureList2);
                continue;
              }
              if (archID > 0)
              {
                long newKey = cacheData.GetNewKey(ImportingCategory.ArchiveParameters, (object) $"{archID}.{condition.Alias.ToLower()}");
                if (newKey != 0L)
                {
                  this.AddCustomAttribute(Convert.ToInt32(newKey), relationalOperators, condition.GetValue(index), conditionStructureList2);
                  continue;
                }
                continue;
              }
              continue;
          }
        }
        if (condition.OperatorNo.Count > 1 && conditionStructureList2.Count > 1)
        {
          for (int index = 0; index < conditionStructureList2.Count; ++index)
          {
            int groupID = 0;
            LogicalOperators logicalOperator = LogicalOperators.OR;
            if (index == 0)
            {
              logicalOperator = LogicalOperators.AND;
              groupID = 1;
            }
            else if (index == conditionStructureList2.Count - 1)
              groupID = -1;
            if (conditionStructureList2[index].Attribute != null)
              conditionStructureList1.Add(new ConditionStructure((int) conditionStructureList2[index].Attribute, conditionStructureList2[index].RelationalOperator, conditionStructureList2[index].Value, conditionStructureList2[index].Value2, logicalOperator, groupID, conditionStructureList2[index].CaseSensitive));
            else
              conditionStructureList1.Add(new ConditionStructure((string) null, conditionStructureList2[index].RelationalOperator, conditionStructureList2[index].Value, conditionStructureList2[index].Value2, logicalOperator, groupID, conditionStructureList2[index].CaseSensitive));
          }
        }
        else if (conditionStructureList2.Count > 0)
          conditionStructureList1.AddRange((IEnumerable<ConditionStructure>) conditionStructureList2);
      }
    }
    return conditionStructureList1.Count <= 0 ? (ConditionStructure[]) null : conditionStructureList1.ToArray();
  }

  private void AddCustomAttribute(
    int attributeID,
    RelationalOperators ra,
    object value,
    List<ConditionStructure> result)
  {
    IAttributeTypeItem byId = this.plugin.Imdi.AttributeTypes.GetByID(attributeID);
    switch (ra)
    {
      case RelationalOperators.Between:
        object val1 = (object) null;
        object val2 = (object) null;
        if (CompareValuesHelper.NormalizedValue(value) == null)
          break;
        Type type1 = typeof (string);
        Type type2;
        switch (byId.AttrValueType)
        {
          case 2:
            type2 = typeof (long);
            break;
          case 3:
            type2 = typeof (double);
            break;
          case 4:
            type2 = typeof (DateTime);
            break;
          default:
            type2 = typeof (string);
            break;
        }
        this.GetBetweenValues(Convert.ToString(value), type2, out val1, out val2, (object) null);
        if (val1 != null)
        {
          if (val2 != null)
          {
            result.Add(new ConditionStructure(attributeID, ra, val1, val2, LogicalOperators.AND, 0, true));
            break;
          }
          result.Add(new ConditionStructure(attributeID, ra, val1, LogicalOperators.AND, 0, true));
          break;
        }
        result.Add(new ConditionStructure(attributeID, ra, (object) null, LogicalOperators.AND, 0, true));
        break;
      case RelationalOperators.LastNDays:
        object conditionValue;
        try
        {
          conditionValue = (object) Convert.ToInt32(value);
        }
        catch
        {
          conditionValue = (object) null;
        }
        result.Add(new ConditionStructure(attributeID, ra, conditionValue, LogicalOperators.AND, 0, true));
        break;
      default:
        if (CompareValuesHelper.NormalizedValue(value) != null)
        {
          try
          {
            if (byId.AttrValueType == 4)
              value = !Convert.ToString(value).Equals("Текущая дата") ? (object) Convert.ToDateTime(value, (IFormatProvider) CultureInfo.CurrentCulture) : (object) Consts.CurrentDateFunction;
            if (byId.AttrValueType == 3)
              value = (object) Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture);
            if (byId.AttrValueType != 2 && byId.AttrValueType != 14)
            {
              if (byId.AttrValueType != 8)
                goto label_27;
            }
            value = (object) Convert.ToInt64(value, (IFormatProvider) CultureInfo.InvariantCulture);
          }
          catch (Exception ex)
          {
            value = (object) null;
          }
        }
        else
          value = (object) null;
label_27:
        result.Add(new ConditionStructure(attributeID, ra, value, LogicalOperators.AND, 0, true));
        break;
    }
  }

  public override void Pump()
  {
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    IDBObject dbObject = userSession.GetObject(new Guid("cad002eb-306c-11d8-b4e9-00304f19f545"), false);
    if (dbObject != null)
      this._kgObjectID = dbObject.ObjectID;
    this._relTypeDoc = this.plugin.Imdi.RelationTypes.GetByGuid(new Guid("cad00154-306c-11d8-b4e9-00304f19f545")).ID;
    this._objtypeDocID = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00070-306c-11d8-b4e9-00304f19f545")).ID;
    this._objtypeArtID = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00268-306c-11d8-b4e9-00304f19f545")).ID;
    IAttributeTypeItem byName = this.plugin.Imdi.AttributeTypes.GetByName("Признак принадлежности");
    if (byName != null)
      this._attrtypePP = byName.ID;
    this._attrLitera = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0038b-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrNote = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrWeight = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00275-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrCodeOKP = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0038a-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrVersionCode = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad001fa-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrName = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrDesignation = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrPurchased = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0038f-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrImbaseKey = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00162-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrFile = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrArchive = this.plugin.Imdi.AttributeTypes.GetByGuid(SystemGUIDs.attributeArchive).ID;
    this._attrFormat = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00255-306c-11d8-b4e9-00304f19f545")).ID;
    int id1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00122-306c-11d8-b4e9-00304f19f545")).ID;
    int id2 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00123-306c-11d8-b4e9-00304f19f545")).ID;
    IObjectTypeItem byGuid1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00140-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid2 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00149-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid3 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0013d-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid4 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00158-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid5 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0013e-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid6 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad01485-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid7 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid8 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid9 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00155-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid10 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0069b-306c-11d8-b4e9-00304f19f545"));
    ICache service1 = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service1.GetCache(ImportingCategory.ArchiveParameters, ImportingCategory.ArticleAttributes, ImportingCategory.Selections, ImportingCategory.SelectionsTree, ImportingCategory.SelectionsImages, ImportingCategory.Archives, ImportingCategory.Articles, ImportingCategory.Documents, ImportingCategory.DocTypes, ImportingCategory.ThematicParams, ImportingCategory.ArticleTypes);
    List<string> stringList = (List<string>) null;
    try
    {
      this.PumpCheckPoint("Импорт изображений выборок", 1);
      int tableRecordsCount1 = this.GetTableRecordsCount(SelectionsPicturesFactory.TableName);
      int index1 = 0;
      IConfigurationService service2 = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
      int packetSize = service2.Configuration.PacketSize;
      IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
      List<int> package = new List<int>(packetSize);
      stringList = new List<string>(tableRecordsCount1);
      iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
      {
        for (int index2 = 0; index2 < iolIm.Items.Count; ++index2)
        {
          if (iolIm.Items[index2].Object.Object_id != 0L && iolIm.Items[index2].Object.Object_id != -1L)
            cacheData.AddValue(ImportingCategory.SelectionsImages, (object) package[index2], iolIm.Items[index2].Object.Object_id);
          else
            this.plugin.appManager.AddWarningMessage($"Иконка выборки {package[index2]} не импортирована. См. серверный лог.");
        }
        package.Clear();
      });
      IDataReader sequentialDataReader1 = this.GetSequentialDataReader(SelectionsPicturesFactory.TableName, SelectionsPicturesFactory.TableColumns);
      string format1 = "Импорт изображений выборок ({0} из {1})";
      try
      {
        SelectionsPicturesFactory selectionsPicturesFactory = new SelectionsPicturesFactory(sequentialDataReader1, this.plugin.Idw.AppManager);
        while (sequentialDataReader1.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format1, (object) index1, (object) tableRecordsCount1), this.CalculatePercent(tableRecordsCount1, index1, 2, 10));
          ISelectionsPicture selectionsPicture = selectionsPicturesFactory.NewItem(sequentialDataReader1);
          if (cacheData.GetNewKey(ImportingCategory.SelectionsImages, (object) selectionsPicture.SampleID) == 0L && selectionsPicture.FileBody != null && selectionsPicture.FileExt != string.Empty)
          {
            iolIm.AddObject(byGuid1.ID, 0);
            string str = Path.Combine(Path.GetTempPath(), $"selection_icon_{index1}.{selectionsPicture.FileExt}");
            stringList.Add(str);
            int fileSize = 0;
            FileStream output = File.OpenWrite(str);
            BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
            try
            {
              binaryWriter.Write(selectionsPicture.FileBody);
              fileSize = Convert.ToInt32(output.Length);
            }
            finally
            {
              binaryWriter.Flush();
              binaryWriter.Close();
            }
            iolIm.AddAttributeBlob(byGuid3.ID, str, (long) fileSize, str, ArcMethods.ZLibPacked);
            AttributesHelper.AddObligatoryObjectAttributes(userSession, iolIm);
            package.Add(selectionsPicture.SampleID);
          }
        }
        iolIm.Import();
      }
      finally
      {
        sequentialDataReader1.Close();
        foreach (string str in stringList)
        {
          if (new FileInfo(str).Exists)
            File.Delete(str);
        }
      }
      this.PumpCheckPoint("Чтение идентификаторов объектов включенных в ручные выборки", 11);
      int capacity = this.GetTableRecordsCount("artsamples") + this.GetTableRecordsCount("samples");
      IDataReader dataReader = this.GetDataReader("select t1.sample_id, t1.art_id as id, 0 from artsamples t1 union select t2.sample_id, t2.doc_id as id, 1 from samples t2");
      this._sampleSelections = new Dictionary<int, PumpSelections.SampleSelectionObjects>(capacity);
      try
      {
        while (dataReader.Read())
        {
          int int32_1 = BasePumpHelper.ToInt32(dataReader[0]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader[1]);
          bool boolean = Convert.ToBoolean(BasePumpHelper.ToInt32(dataReader[2]));
          PumpSelections.SampleSelectionObjects selectionObjects = (PumpSelections.SampleSelectionObjects) null;
          if (this._sampleSelections.TryGetValue(int32_1, out selectionObjects))
          {
            if (boolean)
              selectionObjects.DocIDs.Add(int32_2);
            else
              selectionObjects.ArticleIDs.Add(int32_2);
          }
          else
          {
            selectionObjects = new PumpSelections.SampleSelectionObjects();
            if (boolean)
              selectionObjects.DocIDs.Add(int32_2);
            else
              selectionObjects.ArticleIDs.Add(int32_2);
            this._sampleSelections.Add(int32_1, selectionObjects);
          }
        }
      }
      finally
      {
        dataReader.Close();
      }
      this.PumpCheckPoint("Импорт выборок", 15);
      string format2 = "Импорт выборок ({0} из {1})";
      int index3 = 0;
      int tableRecordsCount2 = this.GetTableRecordsCount(SelectionsFactory.TableName);
      this.SetCountPumpRecords(tableRecordsCount2);
      Dictionary<int, int> dictionary = new Dictionary<int, int>(tableRecordsCount2);
      IDataReader sequentialDataReader2;
      using (sequentialDataReader2 = this.GetSequentialDataReader(SelectionsFactory.TableName, SelectionsFactory.TableColumns))
      {
        iolIm = this.plugin.Idw.CreateImportedObjectList();
        List<ISelectionItem> packageSel = new List<ISelectionItem>(packetSize);
        string format3 = Path.Combine(Path.GetTempPath(), "selectionBlob{0}.tmp");
        iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          try
          {
            for (int index4 = 0; index4 < iolIm.Items.Count; ++index4)
            {
              if (iolIm.Items[index4].Object.Object_id != 0L)
              {
                if (packageSel[index4].IsSample)
                {
                  PumpSelections.SampleSelectionObjects selectionObjects = (PumpSelections.SampleSelectionObjects) null;
                  if (this._sampleSelections.TryGetValue(packageSel[index4].SampleID, out selectionObjects))
                  {
                    for (int index5 = 0; index5 < selectionObjects.ArticleIDs.Count; ++index5)
                    {
                      DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.Articles, (object) selectionObjects.ArticleIDs[index5]);
                      long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
                      if (newObjectId != 0L)
                      {
                        ArticleTag tag = dictionaryValue.Tag as ArticleTag;
                        try
                        {
                          this.plugin.Idw.IncludeObjectIntoSelection(iolIm.Items[index4].Object.Object_id, string.Empty, tag.Versions[tag.VersionID], newObjectId);
                        }
                        catch (Exception ex)
                        {
                          this.plugin.appManager.AddWarningMessage($"Изделие {newObjectId} не включено в ручную выборку {iolIm.Items[index4].Object.Object_id} : {ex.Message}");
                        }
                      }
                      else
                        this.plugin.appManager.AddWarningMessage($"В кэше не найден идентификатор изделия SEARCH {selectionObjects.ArticleIDs[index5]}");
                    }
                    for (int index6 = 0; index6 < selectionObjects.DocIDs.Count; ++index6)
                    {
                      DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.Documents, (object) selectionObjects.DocIDs[index6]);
                      long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
                      if (newObjectId != 0L)
                      {
                        DocumentTag tag = dictionaryValue.Tag as DocumentTag;
                        try
                        {
                          this.plugin.Idw.IncludeObjectIntoSelection(iolIm.Items[index4].Object.Object_id, string.Empty, tag.Versions[tag.VersionID], newObjectId);
                        }
                        catch (Exception ex)
                        {
                          this.plugin.appManager.AddWarningMessage($"Документ {newObjectId} не включен в ручную выборку {iolIm.Items[index4].Object.Object_id}: {ex.Message}");
                        }
                      }
                      else
                        this.plugin.appManager.AddWarningMessage($"В кэше не найден идентификатор документа SEARCH {selectionObjects.DocIDs[index6]}");
                    }
                  }
                }
                cacheData.AddValue(ImportingCategory.Selections, (object) packageSel[index4].SampleID, iolIm.Items[index4].Object.Object_id, packageSel[index4].SampleName);
              }
            }
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage($"Ошибка во время записи результатов импорта выборок в кэш: {ex.Message} StackTrace: {ex.StackTrace}");
          }
          finally
          {
            packageSel.Clear();
          }
        });
        SelectionWrapper selectionWrapper = new SelectionWrapper(false);
        stringList.Clear();
        SelectionsFactory selectionsFactory = new SelectionsFactory(sequentialDataReader2, this.plugin.Idw.AppManager);
        while (sequentialDataReader2.Read())
        {
          ++index3;
          ISelectionItem selectionItem = selectionsFactory.NewItem(sequentialDataReader2);
          if (selectionItem.Smp_IN != -1)
            dictionary.Add(selectionItem.SampleID, selectionItem.Smp_IN);
          if (cacheData.GetNewKey(ImportingCategory.Selections, (object) selectionItem.SampleID) == 0L)
          {
            this.PumpCheckPoint(string.Format(format2, (object) index3, (object) tableRecordsCount2), this.CalculatePercent(tableRecordsCount2, index3, 16 /*0x10*/, 79));
            ConditionStructure[] conditionStructures = (ConditionStructure[]) null;
            try
            {
              if (selectionItem.SampleFLT != null)
                conditionStructures = this.SelectionConditions(selectionItem.SampleFLT, selectionItem.ArchID, cacheData);
            }
            catch (Exception ex)
            {
              conditionStructures = (ConditionStructure[]) null;
              this.plugin.appManager.AddWarningMessage($"Ошибка анализа условия для выборки SEARCH {selectionItem.SampleID} : {ex.Message}");
            }
            int objType = selectionItem.IsCommon == 1 ? id1 : id2;
            iolIm.AddObject(objType, selectionItem.UserID, selectionItem.SampleName);
            iolIm.AddAttributeStr(byGuid7.ID, selectionItem.SampleName);
            iolIm.AddAttributeStr(byGuid8.ID, selectionItem.Description);
            iolIm.AddAttributeInt(byGuid9.ID, selectionItem.IsSample ? 1L : 0L);
            long newKey = cacheData.GetNewKey(ImportingCategory.SelectionsImages, (object) selectionItem.SampleID);
            if (newKey != 0L)
              iolIm.AddAttributeLink(byGuid5.ID, newKey, "Иконка выборки");
            if (selectionItem.Smp_IN == -1)
            {
              if (selectionItem.ArchID > 0)
              {
                DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.Archives, (object) selectionItem.ArchID);
                if (dictionaryValue != null)
                {
                  iolIm.AddAttributeInt(byGuid4.ID, 1L);
                  iolIm.AddAttributeLink(byGuid6.ID, dictionaryValue.NewObjectID, dictionaryValue.Caption);
                }
                else
                  iolIm.AddAttributeInt(byGuid4.ID, 2L);
              }
              if (selectionItem.ArchID == -2)
                iolIm.AddAttributeInt(byGuid4.ID, 4L);
              if (selectionItem.ArchID < -2)
              {
                bool flag = false;
                string caption = cacheData.GetCaption(ImportingCategory.ArticleTypes, (object) Math.Abs(selectionItem.ArchID + 10));
                if (caption != string.Empty)
                {
                  IObjectTypeItem byGuid11 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid(caption));
                  if (byGuid11 != null)
                  {
                    iolIm.AddAttributeInt(byGuid4.ID, 3L);
                    iolIm.AddAttributeStr(byGuid2.ID, byGuid11.GUID.ToString());
                    flag = true;
                  }
                }
                if (!flag)
                  iolIm.AddAttributeInt(byGuid4.ID, 4L);
              }
            }
            else
              iolIm.AddAttributeInt(byGuid4.ID, 0L);
            if (conditionStructures != null)
            {
              if (conditionStructures.Length != 0)
              {
                try
                {
                  XmlDocument xml = selectionWrapper.SaveToXML(userSession, conditionStructures);
                  string str = string.Format(format3, (object) stringList.Count);
                  string filename = str;
                  xml.Save(filename);
                  FileInfo fileInfo = new FileInfo(str);
                  stringList.Add(str);
                  iolIm.AddAttributeBlob(byGuid10.ID, str, fileInfo.Length, selectionItem.Description, ArcMethods.NotPacked);
                }
                catch (Exception ex)
                {
                  this.plugin.appManager.AddWarningMessage($"Не импортированы условия для выборки ID SEARCH {selectionItem.SampleID}: {ex.Message}");
                }
              }
            }
            AttributesHelper.AddObligatoryObjectAttributes(userSession, iolIm);
            packageSel.Add(selectionItem);
          }
        }
      }
      iolIm.Import();
      this.PumpCheckPoint("Построение дерева выборок", 80 /*0x50*/);
      if (dictionary.Count > 0)
      {
        string format4 = "Создание связи между выборками ({0} из {1})";
        int index7 = 0;
        int count = dictionary.Count;
        int relationType = userSession.GetRelationType(new Guid("cad00151-306c-11d8-b4e9-00304f19f545")).RelationType;
        List<int> importingList = new List<int>(service2.Configuration.PacketSize);
        IImportedRelationList irlWs = this.plugin.Idw.CreateImportedRelationList();
        irlWs.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          for (int index8 = 0; index8 < irlWs.Items.Count; ++index8)
          {
            if (irlWs.Items[index8].Relation.PrjLinkId != 0L)
              cacheData.AddValue(ImportingCategory.SelectionsTree, (object) importingList[index8], irlWs.Items[index8].Relation.PrjLinkId);
            else
              this.plugin.appManager.AddWarningMessage($"Связь выборки {importingList[index8]} с родительской выборкой не импортирована. См. серверный лог.");
          }
          importingList.Clear();
        });
        foreach (KeyValuePair<int, int> keyValuePair in dictionary)
        {
          this.PumpCheckPoint(string.Format(format4, (object) index7, (object) count), this.CalculatePercent(count, index7, 81, 99));
          if (cacheData.GetNewKey(ImportingCategory.SelectionsTree, (object) keyValuePair.Key) == 0L)
          {
            long newKey1 = cacheData.GetNewKey(ImportingCategory.Selections, (object) keyValuePair.Value);
            long newKey2 = cacheData.GetNewKey(ImportingCategory.Selections, (object) keyValuePair.Key);
            irlWs.AddRelation(newKey1, newKey2, relationType);
            importingList.Add(keyValuePair.Key);
          }
        }
        irlWs.Import();
      }
      dictionary.Clear();
    }
    finally
    {
      if (stringList != null)
      {
        foreach (string str in stringList)
        {
          if (new FileInfo(str).Exists)
            File.Delete(str);
        }
      }
      if (this._sampleSelections != null)
        this._sampleSelections.Clear();
      service1?.ReleaseCache(ImportingCategory.ArchiveParameters, ImportingCategory.ArticleAttributes, ImportingCategory.Selections, ImportingCategory.SelectionsTree, ImportingCategory.SelectionsImages, ImportingCategory.Archives, ImportingCategory.Articles, ImportingCategory.Documents, ImportingCategory.DocTypes, ImportingCategory.ThematicParams, ImportingCategory.ArticleTypes);
    }
    this.PumpCheckPoint("Импорт выборок успешно завершен", 100);
  }

  private class SampleSelectionObjects
  {
    public List<int> ArticleIDs;
    public List<int> DocIDs;

    public SampleSelectionObjects()
    {
      this.ArticleIDs = new List<int>(100);
      this.DocIDs = new List<int>(100);
    }
  }

  private class Condition
  {
    public string Alias;
    public Dictionary<int, int> OperatorNo;
    public Dictionary<int, object> Values;
    public Dictionary<int, string> Labels;
    public Dictionary<int, int> Users;
    private bool _isValid;

    public bool IsValid => this._isValid;

    public Condition(string alias)
    {
      this.Alias = alias;
      this.OperatorNo = new Dictionary<int, int>();
      this.Values = new Dictionary<int, object>();
      this.Labels = new Dictionary<int, string>();
      this.Users = new Dictionary<int, int>();
    }

    public void AddOperator(int index, int operNo)
    {
      this.OperatorNo.Add(index, operNo);
      this._isValid = true;
    }

    public int GetOperator(int index)
    {
      if (this.OperatorNo.Count <= 0)
        return 22;
      if (this.OperatorNo.ContainsKey(index))
        return this.OperatorNo[index];
      // ISSUE: variable of a boxed type
      __Boxed<Dictionary<int, int>.Enumerator> enumerator = (System.ValueType) this.OperatorNo.GetEnumerator();
      ((IEnumerator) enumerator).MoveNext();
      return (int) ((IDictionaryEnumerator) enumerator).Value;
    }

    public object GetValue()
    {
      if (this.Values.Count <= 0)
        return (object) null;
      // ISSUE: variable of a boxed type
      __Boxed<Dictionary<int, object>.Enumerator> enumerator = (System.ValueType) this.Values.GetEnumerator();
      ((IEnumerator) enumerator).MoveNext();
      return ((IDictionaryEnumerator) enumerator).Value;
    }

    public object GetValue(int index)
    {
      if (this.Values.Count <= 0)
        return (object) null;
      return !this.Values.ContainsKey(index) ? this.GetValue() : this.Values[index];
    }
  }
}
