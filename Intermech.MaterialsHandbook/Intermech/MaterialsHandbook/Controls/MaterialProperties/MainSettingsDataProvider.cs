// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.MainSettingsDataProvider
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

public class MainSettingsDataProvider : DataProvider
{
  public DataTable SurfaceTable { get; private set; }

  public DataTable PrefDestTable { get; private set; }

  public DataTable InternalExternalCoatingTable { get; private set; }

  public DataTable SphereUseTable { get; private set; }

  public DataTable CoatingColorTable { get; private set; }

  public DataTable RALColorTable { get; private set; }

  public override List<Tuple<string, IEnumerable<DataTable>>> LoadData(string imbaseKey)
  {
    List<Tuple<string, IEnumerable<DataTable>>> tupleList = new List<Tuple<string, IEnumerable<DataTable>>>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
        return (List<Tuple<string, IEnumerable<DataTable>>>) null;
      string imbaseKeyGuidFormat1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
      this.SurfaceTable = this.GetSimpleCoatingData(imbaseKey, imbaseKeyGuidFormat1, "COATING_MATERIALS_TABLE_NAME", customService.GetObjectGuidByName("COATING_MATERIALS_COLUMN_MATERIALS").ToString(), customService.GetObjectGuidByName("COATING_MATERIALS_COLUMN_COATING").ToString());
      if (this.SurfaceTable != null)
      {
        this.SurfaceTable.ExtendedProperties.Add((object) "SourceTable", (object) VarnishTables.DtSurface);
        this.SurfaceTable.Columns[0].Caption = LocalizationHolder.rm.GetString("IMH_Varnish_AdditionalPropsPage_Surface_Description");
        tupleList.Add(new Tuple<string, IEnumerable<DataTable>>(LocalizationHolder.rm.GetString("IMH_Surface_Materials"), (IEnumerable<DataTable>) new DataTable[1]
        {
          this.SurfaceTable
        }));
      }
      string imbaseKey1 = imbaseKey;
      string imbaseKeyGuidFormat2 = imbaseKeyGuidFormat1;
      Guid objectGuidByName1 = customService.GetObjectGuidByName("COATING_PREFERRED_DESTINATION_COLUMN_COATING");
      string keyColumnCoating1 = objectGuidByName1.ToString();
      objectGuidByName1 = customService.GetObjectGuidByName("COATING_PREFERRED_DESTINATION_COLUMN_PURPOSE");
      string resultColumnName1 = objectGuidByName1.ToString();
      this.PrefDestTable = this.GetSimpleCoatingData(imbaseKey1, imbaseKeyGuidFormat2, "COATING_PREFERRED_DESTINATION_TABLE_NAME", keyColumnCoating1, resultColumnName1);
      if (this.PrefDestTable != null)
      {
        this.PrefDestTable.ExtendedProperties.Add((object) "SourceTable", (object) VarnishTables.DtPrefDest);
        this.PrefDestTable.Columns[0].Caption = LocalizationHolder.rm.GetString("IMH_Destination");
        tupleList.Add(new Tuple<string, IEnumerable<DataTable>>(LocalizationHolder.rm.GetString("IMH_Pref_Destination"), (IEnumerable<DataTable>) new DataTable[1]
        {
          this.PrefDestTable
        }));
      }
      this.InternalExternalCoatingTable = this.GetCondidtionUseBaseCoatingTable(imbaseKey);
      if (this.InternalExternalCoatingTable != null)
      {
        this.InternalExternalCoatingTable.ExtendedProperties.Add((object) "SourceTable", (object) VarnishTables.DtInternalExternalCoating);
        tupleList.Add(new Tuple<string, IEnumerable<DataTable>>(LocalizationHolder.rm.GetString("IMH_ConditionUse_BaseCoatings"), (IEnumerable<DataTable>) new DataTable[1]
        {
          this.InternalExternalCoatingTable
        }));
      }
      string imbaseKey2 = imbaseKey;
      string imbaseKeyGuidFormat3 = imbaseKeyGuidFormat1;
      Guid objectGuidByName2 = customService.GetObjectGuidByName("COATING_SPHERE_USE_COLUMN_COATING");
      string keyColumnCoating2 = objectGuidByName2.ToString();
      objectGuidByName2 = customService.GetObjectGuidByName("COATING_SPHERE_USE_COLUMN_SPHERE");
      string resultColumnName2 = objectGuidByName2.ToString();
      this.SphereUseTable = this.GetSimpleCoatingData(imbaseKey2, imbaseKeyGuidFormat3, "COATING_SPHERE_USE_TABLE_NAME", keyColumnCoating2, resultColumnName2);
      if (this.SphereUseTable != null)
      {
        this.SphereUseTable.ExtendedProperties.Add((object) "SourceTable", (object) VarnishTables.DtSphere);
        this.SphereUseTable.Columns[0].Caption = LocalizationHolder.rm.GetString("IMH_SpheresOfUse");
        tupleList.Add(new Tuple<string, IEnumerable<DataTable>>(LocalizationHolder.rm.GetString("IMH_SpheresOfUse"), (IEnumerable<DataTable>) new DataTable[1]
        {
          this.SphereUseTable
        }));
      }
      string imbaseKey3 = imbaseKey;
      string imbaseKeyGuidFormat4 = imbaseKeyGuidFormat1;
      Guid objectGuidByName3 = customService.GetObjectGuidByName("COATING_COLOR_COLUMN_COATING");
      string keyColumnCoating3 = objectGuidByName3.ToString();
      objectGuidByName3 = customService.GetObjectGuidByName("COATING_COLOR_COLUMN_COLOR");
      string resultColumnName3 = objectGuidByName3.ToString();
      this.CoatingColorTable = this.GetSimpleCoatingData(imbaseKey3, imbaseKeyGuidFormat4, "COATING_COLOR_TABLE_NAME", keyColumnCoating3, resultColumnName3);
      if (this.CoatingColorTable != null)
      {
        this.CoatingColorTable.ExtendedProperties.Add((object) "SourceTable", (object) VarnishTables.DtCoatingColor);
        this.CoatingColorTable.Columns[0].Caption = LocalizationHolder.rm.GetString("IMH_Color");
        tupleList.Add(new Tuple<string, IEnumerable<DataTable>>(LocalizationHolder.rm.GetString("IMH_Coating_Color"), (IEnumerable<DataTable>) new DataTable[1]
        {
          this.CoatingColorTable
        }));
      }
      long linkId = sessionKeeper.Session.GetObjectInfo(customService.GetObjectGuidByName("COATING_COLOR_RAL_TABLE_NAME")).ObjectID;
      DataSet imbaseDs = IMHHelper.GetImbaseDS("COATING_COLOR_RAL_TABLE_NAME");
      if (imbaseDs == null || !imbaseDs.Tables.Contains("IMS_DATA") || !imbaseDs.Tables.Contains("IMS_ATTR_TYPES"))
      {
        this.RALColorTable = (DataTable) null;
      }
      else
      {
        DataTable table = imbaseDs.Tables["IMS_DATA"];
        this.RALColorTable = new DataTable();
        this.RALColorTable.Columns.Add(new DataColumn("RAL"));
        table.AsEnumerable().ToList<DataRow>().ForEach((Action<DataRow>) (x =>
        {
          DataRow row = this.RALColorTable.NewRow();
          row[0] = (object) ImbaseHelper.MakeInternalImbaseKey(linkId, (long) x.Field<int>("F_KEY"));
          this.RALColorTable.Rows.Add(row);
        }));
      }
    }
    return tupleList;
  }

  private DataTable GetSimpleCoatingData(
    string imbaseKey,
    string imbaseKeyGuidFormat,
    string tableName,
    string keyColumnCoating,
    string resultColumnName)
  {
    DataSet imbaseDs = IMHHelper.GetImbaseDS(tableName);
    if (imbaseDs == null || !imbaseDs.Tables.Contains("IMS_DATA") || !imbaseDs.Tables.Contains("IMS_ATTR_TYPES"))
      return (DataTable) null;
    DataTable table1 = imbaseDs.Tables["IMS_DATA"];
    DataTable table2 = imbaseDs.Tables["IMS_ATTR_TYPES"];
    DataTable retValue = new DataTable();
    DataColumn column = new DataColumn(resultColumnName);
    AttributeOptions optionsFromTable = this.GetAttributeOptionsFromTable(table2, resultColumnName);
    AttributeOptions attributeOptions = MetaDataHelper.GetAttributeType(new Guid(resultColumnName)).Options | optionsFromTable;
    column.ExtendedProperties.Add((object) "F_OPTIONS", (object) attributeOptions);
    retValue.Columns.Add(column);
    table1.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (x => x.Field<string>(keyColumnCoating) == imbaseKey || x.Field<string>(keyColumnCoating) == imbaseKeyGuidFormat)).ToList<DataRow>().ForEach((Action<DataRow>) (x =>
    {
      DataRow row = retValue.NewRow();
      row[0] = (object) x.Field<string>(resultColumnName);
      retValue.Rows.Add(row);
    }));
    return retValue;
  }

  private DataTable GetCondidtionUseBaseCoatingTable(string imbaseKey)
  {
    DataTable baseCoatingTable;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
        return (DataTable) null;
      long linkId = sessionKeeper.Session.GetObjectInfo(customService.GetObjectGuidByName("COATING_TERMS_USE_TABLE_NAME")).ObjectID;
      string imbaseKey2 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
      DataSet imbaseDs1 = IMHHelper.GetImbaseDS("COATING_TERMS_USE_TABLE_NAME");
      if (imbaseDs1 == null || !imbaseDs1.Tables.Contains("IMS_DATA") || !imbaseDs1.Tables.Contains("IMS_ATTR_TYPES"))
        return (DataTable) null;
      DataTable table1 = imbaseDs1.Tables["IMS_DATA"];
      DataTable table2 = imbaseDs1.Tables["IMS_ATTR_TYPES"];
      DataSet imbaseDs2 = IMHHelper.GetImbaseDS("COATING_INTERNAL_EXTERNAL_TABLE_NAME");
      if (imbaseDs2 == null || !imbaseDs2.Tables.Contains("IMS_DATA") || !imbaseDs2.Tables.Contains("IMS_ATTR_TYPES"))
        return (DataTable) null;
      DataTable table3 = imbaseDs2.Tables["IMS_DATA"];
      DataTable table4 = imbaseDs2.Tables["IMS_ATTR_TYPES"];
      string coatingColumnName = customService.GetObjectGuidByName("COATING_TERMS_USE_COLUMN_COATING").ToString();
      Guid objectGuidByName1 = customService.GetObjectGuidByName("COATING_TERMS_USE_COLUMN_TERMS");
      string termsColumnName = objectGuidByName1.ToString();
      Guid objectGuidByName2 = customService.GetObjectGuidByName("COATING_INTERNAL_EXTERNAL_INTERNAL_COLUMN");
      string str = objectGuidByName2.ToString();
      string externalCoatingWithTermColumnName = customService.GetObjectGuidByName("COATING_INTERNAL_EXTERNAL_EXTERNAL_WITH_CONDITION_COLUMN").ToString();
      baseCoatingTable = new DataTable();
      DataColumn column1 = new DataColumn(termsColumnName);
      AttributeOptions optionsFromTable1 = this.GetAttributeOptionsFromTable(table2, termsColumnName);
      AttributeOptions attributeOptions1 = MetaDataHelper.GetAttributeType(objectGuidByName1).Options | optionsFromTable1;
      column1.ExtendedProperties.Add((object) "F_OPTIONS", (object) attributeOptions1);
      column1.Caption = LocalizationHolder.rm.GetString("IMH_Condition_Use");
      baseCoatingTable.Columns.Add(column1);
      DataColumn column2 = new DataColumn(str);
      AttributeOptions optionsFromTable2 = this.GetAttributeOptionsFromTable(table4, str);
      AttributeOptions attributeOptions2 = MetaDataHelper.GetAttributeType(objectGuidByName2).Options | optionsFromTable2;
      column2.ExtendedProperties.Add((object) "F_OPTIONS", (object) attributeOptions2);
      column2.Caption = LocalizationHolder.rm.GetString("IMH_Base_Coating");
      baseCoatingTable.Columns.Add(column2);
      List<\u003C\u003Ef__AnonymousType0<string, string>> list = table1.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (row => row.Field<string>(coatingColumnName) == imbaseKey || row.Field<string>(coatingColumnName) == imbaseKey2)).Select(row => new
      {
        TermAndCoatingRowImbaseKey = ImbaseHelper.MakeInternalImbaseKey(linkId, (long) row.Field<int>("F_KEY")),
        TermImbaseKey = row.Field<string>(termsColumnName)
      }).ToList();
      foreach (DataRow row1 in (InternalDataCollectionBase) table3.Rows)
      {
        DataRow row = row1;
        if (!list.All(k => k.TermAndCoatingRowImbaseKey != row[externalCoatingWithTermColumnName].ToString()))
        {
          DataRow row2 = baseCoatingTable.NewRow();
          row2[0] = (object) list.First(k => k.TermAndCoatingRowImbaseKey == row[externalCoatingWithTermColumnName].ToString()).TermImbaseKey;
          row2[1] = (object) row.Field<string>(str);
          baseCoatingTable.Rows.Add(row2);
        }
      }
    }
    return baseCoatingTable;
  }

  private AttributeOptions GetAttributeOptionsFromTable(DataTable dtAttrTypes, string attrGuid)
  {
    return dtAttrTypes.AsEnumerable().All<DataRow>((System.Func<DataRow, bool>) (x => x["F_ATTRIBUTE_GUID"].ToString() != attrGuid)) ? AttributeOptions.None : (AttributeOptions) dtAttrTypes.AsEnumerable().First<DataRow>((System.Func<DataRow, bool>) (x => x["F_ATTRIBUTE_GUID"].ToString() == attrGuid)).Field<int>("F_OPTIONS");
  }

  public override void SaveData(string imbaseKey, List<Tuple<string, IEnumerable<DataTable>>> data)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
        return;
      string imbaseKeyGuidFormat = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
      foreach (Tuple<string, IEnumerable<DataTable>> tuple in data)
      {
        foreach (DataTable dtValues in tuple.Item2)
        {
          object extendedProperty;
          if (dtValues.ExtendedProperties.ContainsKey((object) "SourceTable") && (extendedProperty = dtValues.ExtendedProperties[(object) "SourceTable"]) is VarnishTables)
          {
            switch ((VarnishTables) extendedProperty)
            {
              case VarnishTables.None:
                continue;
              case VarnishTables.DtSurface:
                this.SaveSimpleCoatingData(imbaseKey, imbaseKeyGuidFormat, dtValues, "COATING_MATERIALS_TABLE_NAME", customService.GetObjectGuidByName("COATING_MATERIALS_COLUMN_COATING").ToString(), customService.GetObjectGuidByName("COATING_MATERIALS_COLUMN_MATERIALS").ToString());
                continue;
              case VarnishTables.DtPrefDest:
                this.SaveSimpleCoatingData(imbaseKey, imbaseKeyGuidFormat, dtValues, "COATING_PREFERRED_DESTINATION_TABLE_NAME", customService.GetObjectGuidByName("COATING_PREFERRED_DESTINATION_COLUMN_COATING").ToString(), customService.GetObjectGuidByName("COATING_PREFERRED_DESTINATION_COLUMN_PURPOSE").ToString());
                continue;
              case VarnishTables.DtInternalExternalCoating:
                this.SaveCondidtionUseBaseCoatingData(imbaseKey, imbaseKeyGuidFormat, dtValues);
                continue;
              case VarnishTables.DtSphere:
                this.SaveSimpleCoatingData(imbaseKey, imbaseKeyGuidFormat, dtValues, "COATING_SPHERE_USE_TABLE_NAME", customService.GetObjectGuidByName("COATING_SPHERE_USE_COLUMN_COATING").ToString(), customService.GetObjectGuidByName("COATING_SPHERE_USE_COLUMN_SPHERE").ToString());
                continue;
              case VarnishTables.DtCoatingColor:
                this.SaveSimpleCoatingData(imbaseKey, imbaseKeyGuidFormat, dtValues, "COATING_COLOR_TABLE_NAME", customService.GetObjectGuidByName("COATING_COLOR_COLUMN_COATING").ToString(), customService.GetObjectGuidByName("COATING_COLOR_COLUMN_COLOR").ToString());
                continue;
              default:
                throw new ArgumentOutOfRangeException();
            }
          }
        }
      }
    }
  }

  private void SaveSimpleCoatingData(
    string imbaseKey,
    string imbaseKeyGuidFormat,
    DataTable dtValues,
    string tableName,
    string keyColumnCoating,
    string resultColumnName)
  {
    long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName(tableName));
    DataSet imbaseDs = IMHHelper.GetImbaseDS(tableName);
    if (imbaseDs == null || !imbaseDs.Tables.Contains("IMS_DATA") || !imbaseDs.Tables.Contains("IMS_ATTR_TYPES"))
      return;
    DataTable dt = imbaseDs.Tables["IMS_DATA"];
    dt.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (row => row.Field<string>(keyColumnCoating) == imbaseKey || row.Field<string>(keyColumnCoating) == imbaseKeyGuidFormat)).ToList<DataRow>().ForEach((Action<DataRow>) (row => row.Delete()));
    dtValues.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row.Field<string>(resultColumnName))).Where<string>((System.Func<string, bool>) (str => str != string.Empty)).ToList<string>().ForEach((Action<string>) (val =>
    {
      DataRow row = dt.NewRow();
      row["F_GUID"] = (object) Guid.NewGuid();
      row[keyColumnCoating] = (object) imbaseKey;
      row[resultColumnName] = (object) val;
      dt.Rows.Add(row);
    }));
    dt.AcceptChanges();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, imbaseDs, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
  }

  private void SaveCondidtionUseBaseCoatingData(
    string imbaseKey,
    string imbaseKeyGuidFormat,
    DataTable dtValues)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
        return;
      string coatingColumnName = customService.GetObjectGuidByName("COATING_TERMS_USE_COLUMN_COATING").ToString();
      string termsColumnName = customService.GetObjectGuidByName("COATING_TERMS_USE_COLUMN_TERMS").ToString();
      string internalCoatingColumnName = customService.GetObjectGuidByName("COATING_INTERNAL_EXTERNAL_INTERNAL_COLUMN").ToString();
      string externalCoatingWithTermColumnName = customService.GetObjectGuidByName("COATING_INTERNAL_EXTERNAL_EXTERNAL_WITH_CONDITION_COLUMN").ToString();
      long coatingTermsUseLinkId = IMHHelper.GetObjectIDByConstName("COATING_TERMS_USE_TABLE_NAME");
      long tableIdByTableRefId1 = IMHHelper.GetTableIDByTableRefID(coatingTermsUseLinkId);
      DataSet imbaseDs1 = IMHHelper.GetImbaseDS("COATING_TERMS_USE_TABLE_NAME");
      if (imbaseDs1 == null || !imbaseDs1.Tables.Contains("IMS_DATA") || !imbaseDs1.Tables.Contains("IMS_ATTR_TYPES"))
        return;
      DataTable dtCoatingTermsUse = imbaseDs1.Tables["IMS_DATA"];
      long tableIdByTableRefId2 = IMHHelper.GetTableIDByTableRefID(IMHHelper.GetObjectIDByConstName("COATING_INTERNAL_EXTERNAL_TABLE_NAME"));
      DataSet imbaseDs2 = IMHHelper.GetImbaseDS("COATING_INTERNAL_EXTERNAL_TABLE_NAME");
      if (imbaseDs2 == null || !imbaseDs2.Tables.Contains("IMS_DATA") || !imbaseDs2.Tables.Contains("IMS_ATTR_TYPES"))
        return;
      DataTable dtCoatingInternalExternal = imbaseDs2.Tables["IMS_DATA"];
      List<string> list = dtValues.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row.Field<string>(termsColumnName))).Where<string>((System.Func<string, bool>) (term => !string.IsNullOrEmpty(term))).Distinct<string>().ToList<string>();
      List<string> removedKeys = new List<string>();
      dtCoatingTermsUse.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (row => row.Field<string>(coatingColumnName) == imbaseKey)).ToList<DataRow>().ForEach((Action<DataRow>) (row =>
      {
        removedKeys.Add(ImbaseHelper.MakeInternalImbaseKey(coatingTermsUseLinkId, (long) row.Field<int>("F_KEY")));
        row.Delete();
      }));
      dtCoatingInternalExternal.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (row => removedKeys.Contains(row.Field<string>(externalCoatingWithTermColumnName)))).ToList<DataRow>().ForEach((Action<DataRow>) (row => row.Delete()));
      Dictionary<string, string> termAndTermUseDictionary = new Dictionary<string, string>();
      Action<string> action = (Action<string>) (termKey =>
      {
        DataRow row = dtCoatingTermsUse.NewRow();
        row["F_GUID"] = (object) Guid.NewGuid();
        row[coatingColumnName] = (object) imbaseKey;
        row[termsColumnName] = (object) termKey;
        dtCoatingTermsUse.Rows.Add(row);
        termAndTermUseDictionary.Add(termKey, ImbaseHelper.MakeInternalImbaseKey(coatingTermsUseLinkId, (long) row.Field<int>("F_KEY")));
      });
      list.ForEach(action);
      dtValues.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (row => !string.IsNullOrEmpty(row.Field<string>(termsColumnName)))).ToList<DataRow>().ForEach((Action<DataRow>) (row =>
      {
        string str;
        if (!termAndTermUseDictionary.TryGetValue(row.Field<string>(termsColumnName), out str))
          return;
        DataRow row1 = dtCoatingInternalExternal.NewRow();
        row1["F_GUID"] = (object) Guid.NewGuid();
        row1[externalCoatingWithTermColumnName] = (object) str;
        row1[internalCoatingColumnName] = (object) row.Field<string>(internalCoatingColumnName);
        dtCoatingInternalExternal.Rows.Add(row1);
      }));
      dtCoatingTermsUse.AcceptChanges();
      dtCoatingInternalExternal.AcceptChanges();
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId1, imbaseDs1, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId2, imbaseDs2, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
    }
  }
}
