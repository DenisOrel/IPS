// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpCompositionClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки связей состава обьектов", "Перекачка связей состава обьектов")]
public class PumpCompositionClass : PumpClass
{
  protected SearchDataPlugin plugin;
  private CacheCategory _articlesCache;
  private IMeasures _measures;
  private CacheCategory _compositionCache;
  private CacheCategory _vcompositionCache;
  private CacheCategory _pcParamsCache;
  private CacheCategory _documentationCache;
  private CacheCategory _docLinks;
  private CacheCategory _documentsCache;
  private string _paramColumns;
  private string _CADModelSearchTypeIDs;
  private Dictionary<string, DictionaryValue> PCParamsMapper = new Dictionary<string, DictionaryValue>();
  private HashSet<string> _sessionDocLinks = new HashSet<string>();
  private SimpleLogger logger;
  private List<int> _cadModelTypeIDs;

  protected override Guid GUID => new Guid("{CDFFDCED-BF81-4104-8E5B-D03F05C96656}");

  public PumpCompositionClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  private string ParamColumns
  {
    get
    {
      if (this._paramColumns == null)
      {
        if (this._pcParamsCache == null)
          this._pcParamsCache = PumpCache.Category[ImportingCategory.CompositionAttributes];
        this._paramColumns = "";
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this._pcParamsCache.Items)
        {
          string str = keyValuePair.Key.ToString();
          string key = "pars_" + str;
          this._paramColumns += $",pars.{str} as {key}";
          this.PCParamsMapper.Add(key, keyValuePair.Value);
        }
      }
      return this._paramColumns;
    }
  }

  public ImportingRelation ImportingRelationCreator(RelationRecord rec)
  {
    return (ImportingRelation) new ImportingRelationEx(rec);
  }

  private void writer_AfterImportEvent(object sender, EventArgs e)
  {
    IImportedRelationList importedRelationList = (IImportedRelationList) sender;
    for (int index = 0; index < importedRelationList.Items.Count; ++index)
    {
      int num = 0;
      if (importedRelationList.Items[index] is ImportingRelationEx importingRelationEx)
      {
        if (importingRelationEx.Cache != null)
        {
          if (importingRelationEx.Relation != null)
            importingRelationEx.Cache.AddValue(importingRelationEx.OldKey, importingRelationEx.Relation.PrjLinkId, importingRelationEx.Tag);
          else
            num = 2;
        }
        if (importingRelationEx.DocLinkKey != "")
          this._docLinks.AddValue((object) importingRelationEx.DocLinkKey, 1L);
      }
      else
        num = 1;
      if (num != 0)
        BasePumpHelper.AppManager.AddErrorMessage($"Связь не была создана, невозможно записать в кэш! ({num})");
    }
    this._sessionDocLinks.Clear();
  }

  protected string CADModelSearchTypeIDs
  {
    get
    {
      if (this._CADModelSearchTypeIDs == null)
        this._CADModelSearchTypeIDs = string.Join<int>(",", (IEnumerable<int>) PumpHelper.CADModelDocTypes);
      return this._CADModelSearchTypeIDs;
    }
  }

  public void PumpPC(string table)
  {
    bool isNewPcFormat = PumpHelper.IsNewPCFormat;
    using (IDbCommand command = this.plugin.idb2.CreateCommand())
    {
      bool flag1 = table == "PC";
      this.PumpCheckPoint($"Определение количества связей состава {table}для перекачки", 0);
      string str1 = (!flag1 ? ", v_articles v where p.proj_aid = v.vart_id" : ", articles v where p.proj_aid = v.art_id") + $" and v.section_id <> {99999990}";
      command.CommandText = $"select count(*) from {table} p {str1}";
      int int32_1 = Convert.ToInt32(command.ExecuteScalar());
      this.logger.Write($"{command.CommandText}: {int32_1} result(s)");
      string str2 = table + "_PARAMS pars";
      string str3;
      string str4;
      if (flag1)
      {
        string str5 = "";
        if (isNewPcFormat)
          str5 = ", " + this.plugin.idb2.GetIntField("PROJ_VER_ID", "PROJ_VER_ID");
        str3 = $",P.ORDER_ID, P.F_START_DT, F_FINISH_DT, v.section_id as proj_section_id {str5}{this.ParamColumns}";
        str4 = $",{str2},articles v where p.proj_aid = v.art_id and pars.prjlink_id = p.prjlink_id and p.f_finish_dt is null";
      }
      else
      {
        str3 = $",v.art_id as proj_art_id,  {this.plugin.idb2.GetIntField("v.art_ver_id", "proj_ver_id")}, v2.art_id as part_art_id, {this.plugin.idb2.GetIntField("v2.art_ver_id", "part_ver_id")}, v.author as proj_author, v2.author as part_author, v.section_id as proj_section_id  {this.ParamColumns}";
        str4 = $",{str2},v_articles v2,v_articles v where p.proj_aid = v.vart_id and p.part_aid = v2.vart_id and pars.prjlink_id = p.prjlink_id";
      }
      if (!PumpHelper.IsVariantsExists)
        str3 += ",VARGRP,VARNUM";
      string str6 = this.CADModelSearchTypeIDs;
      if (str6 != "")
      {
        List<string> stringList = new List<string>();
        using (IDataReader dataReader = BasePumpHelper.S4Query($"select distinct dt_code from doctypes where doc_type in ({str6})"))
        {
          while (dataReader.Read())
          {
            if (!dataReader.IsDBNull(0))
              stringList.Add(dataReader[0].ToString());
          }
        }
        string str7 = "";
        foreach (string str8 in stringList)
        {
          if (str7 != "")
            str7 += " or ";
          str7 = $"{str7}a.designatio=prja.designatio {PumpHelper.ConcatSign} ' {str8}'";
        }
        if (str7 != "")
        {
          string str9 = "";
          string str10 = "";
          switch (BasePumpHelper.dbType)
          {
            case BasePumpHelper.DBType.Oracle:
              str10 = " and rownum=1";
              break;
            case BasePumpHelper.DBType.MSSQL:
              str9 = " top 1";
              break;
            case BasePumpHelper.DBType.Interbase:
              str9 = " first 1";
              break;
          }
          if (flag1)
            str6 = $",(select{str9} d.doc_id from articles a, articles prja, doclist d where p.proj_aid=prja.art_id and ({str7}) and d.doc_id=a.doc_id and d.doc_type in ({str6}){str10}) as modelDoc";
          else
            str6 = $",(select{str9} d.doc_id from v_articles a, v_articles prja, doclist d where p.proj_aid=prja.vart_id and ({str7}) and d.doc_id=a.doc_id and d.doc_type in ({str6}){str10}) as modelDoc";
        }
        else
          str6 = "";
        str3 += str6;
      }
      string str11 = $"select {this.plugin.idb2.GetIntField("P.PROJ_AID", "PROJ_AID")}, {this.plugin.idb2.GetIntField("P.PART_AID", "PART_AID")}, {this.plugin.idb2.GetIntField("P.COUNT_PC", "COUNT_PC")}, {this.plugin.idb2.GetIntField("P.MU_ID", "MU_ID")}, {this.plugin.idb2.GetIntField("P.RAZDEL", "RAZDEL")}, P.POSITIO, P.NOTE, P.VARIANTS, P.LINK_TYPE, P.FORMAT, CTX_ID, P.PRJLINK_ID {str3} from {table} P {str4} order by P.PRJLINK_ID";
      command.CommandText = str11;
      IDataReader dataReader1 = command.ExecuteReader();
      try
      {
        int index = 1;
        string format = $"Перекачка связей состава обьектов {table} ({{0}} из {{1}})";
        int num1 = 0;
        int num2 = 0;
        long num3 = 0;
        ArticleTag articleTag = (ArticleTag) null;
        IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
        importedRelationList.ImportingRelationCreator = new Intermech.ImpExp.Interface.DataWriter.ImportingRelationCreator(this.ImportingRelationCreator);
        importedRelationList.AfterImportEvent += new AfterImportEventDelegate(this.writer_AfterImportEvent);
        while (dataReader1.Read())
        {
          this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 1, 99));
          this.logger.Flush();
          try
          {
            int num4 = -1;
            int num5 = -1;
            int int32_2 = BasePumpHelper.ToInt32(dataReader1[11]);
            int int32_3 = BasePumpHelper.ToInt32(dataReader1[0]);
            int int32_4 = BasePumpHelper.ToInt32(dataReader1[1]);
            int num6;
            int num7;
            if (flag1)
            {
              num6 = int32_3;
              num7 = int32_4;
              if (isNewPcFormat)
              {
                object obj = dataReader1["proj_ver_id"];
                num4 = !DBNull.Value.Equals(obj) ? Convert.ToInt32(obj) : 0;
              }
              if (this._compositionCache.GetNewKey((object) int32_2) != 0L)
                continue;
            }
            else
            {
              num6 = Convert.ToInt32(dataReader1["proj_art_id"]);
              num4 = Convert.ToInt32(dataReader1["proj_ver_id"]);
              num7 = Convert.ToInt32(dataReader1["part_art_id"]);
              num5 = Convert.ToInt32(dataReader1["part_ver_id"]);
              if (this._vcompositionCache.GetNewKey((object) int32_2) != 0L)
                continue;
            }
            int int32_5 = Convert.ToInt32(dataReader1["proj_section_id"]);
            bool flag2 = false;
            if (!flag1 && !PluginSettings.PumpSysArtVersions)
            {
              object obj = dataReader1["proj_author"];
              flag2 = !DBNull.Value.Equals(obj) && Convert.ToInt32(obj) == -2;
            }
            if (num1 != num6 || num2 != num4)
            {
              DictionaryValue dictionaryValue = this._articlesCache.GetValue((object) num6);
              if (dictionaryValue != null)
              {
                articleTag = dictionaryValue.Tag as ArticleTag;
                if (int32_5 == 1)
                {
                  if (!articleTag.Flags.HasFlag((Enum) ArticleFlag.SBFromSP))
                    continue;
                }
                if (num4 == -1)
                  num4 = articleTag.VersionID;
                if (articleTag.Versions.TryGetValue(num4, out num3))
                {
                  num1 = num6;
                  num2 = num4;
                }
                else
                {
                  if (!flag2)
                    BasePumpHelper.AppManager.AddWarningMessage($"Версия изделия (ART_ID={num6}, VER_ID={num4}) не найдена, невозможно восстановить связь состава ({int32_2})!");
                  num1 = 0;
                  continue;
                }
              }
              else
                continue;
            }
            if (articleTag != null)
            {
              if (flag1)
              {
                if (articleTag.Versions.Count > 1)
                  continue;
              }
              if (!flag1)
              {
                if (articleTag.Versions.Count == 1)
                  continue;
              }
            }
            int relType = 0;
            DictionaryValue info1 = this.plugin.Imdi.ImportedObjects.GetInfo(num3);
            if (info1 == null)
            {
              BasePumpHelper.AppManager.AddWarningMessage($"Изделие (prj, ART_ID={num6}, VER_ID={num4}) не было импортировано, невозможно восстановить связь состава ({int32_2})!");
            }
            else
            {
              int objectType1 = (info1.Tag as ObjectInfo).ObjectType;
              long num8 = 0;
              long objectID = -1;
              bool flag3 = false;
              if (!flag1 && !PluginSettings.PumpSysArtVersions)
              {
                object obj = dataReader1["part_author"];
                flag3 = !DBNull.Value.Equals(obj) && Convert.ToInt32(obj) == -2;
              }
              long num9 = -1;
              DictionaryValue dictionaryValue1 = this._articlesCache.GetValue((object) num7);
              if (dictionaryValue1 != null)
              {
                ArticleTag tag = dictionaryValue1.Tag as ArticleTag;
                if (num5 == -1 & isNewPcFormat)
                  num5 = tag.VersionID;
                if (!tag.Flags.HasFlag((Enum) ArticleFlag.SBFromSP))
                {
                  DictionaryValue dictionaryValue2 = this._documentationCache.GetValue((object) BasePumpHelper.MakeCacheKey(num7, num5));
                  if (dictionaryValue2 != null)
                  {
                    if (num5 == -1)
                    {
                      num8 = dictionaryValue2.NewObjectID;
                    }
                    else
                    {
                      objectID = dictionaryValue2.NewObjectID;
                      num8 = PumpHelper.Plugin.Imdi.ImportedObjects.GetID(objectID);
                    }
                    relType = !PumpHelper.IsBuildingSection(objectType1) ? PumpHelper.RelTypeDocumentationID : PumpHelper.RelTypeBuildingCompositionID;
                  }
                }
                if (num8 == 0L)
                {
                  tag.Versions.TryGetValue(tag.VersionID, out num9);
                  if (!tag.Versions.TryGetValue(num5, out objectID))
                  {
                    if (flag3)
                    {
                      objectID = -1L;
                    }
                    else
                    {
                      BasePumpHelper.AppManager.AddWarningMessage($"Версия изделия (ART_ID={num7}, VER_ID={num5}) не найдена, невозможно восстановить связь состава {int32_2}!");
                      continue;
                    }
                  }
                  num8 = flag1 ? dictionaryValue1.NewObjectID : PumpHelper.Plugin.Imdi.ImportedObjects.GetID(objectID);
                }
              }
              if (num8 != 0L)
              {
                double num10 = BasePumpHelper.ToDouble(dataReader1[2]);
                int int32_6 = BasePumpHelper.ToInt32(dataReader1[3]);
                string str12 = "";
                PumpHelper.MU.TryGetValue(int32_6, out str12);
                string measureShortName = str12.Trim();
                IMeasureItem measure = this._measures.GetMeasure(measureShortName);
                string strValue = $"{num10} {measureShortName}";
                double num11 = num10 * measure.Koef;
                int int32_7 = BasePumpHelper.ToInt32(dataReader1[4]);
                SpecificationSection specificationSection = (SpecificationSection) null;
                PumpHelper.SpecificationSections.TryGetValue(int32_7, out specificationSection);
                string str13 = dataReader1.IsDBNull(5) ? "" : dataReader1.GetString(5);
                string str14 = dataReader1.IsDBNull(6) ? "" : dataReader1.GetString(6);
                char key = ' ';
                if (!dataReader1.IsDBNull(8))
                {
                  string str15 = dataReader1.GetString(8);
                  if (str15.Length > 0)
                    key = str15[0];
                }
                int int32_8 = BasePumpHelper.ToInt32(dataReader1[10]);
                int num12 = -1;
                DateTime crtDate = PumpHelper.MinDBDateTime;
                if (flag1)
                {
                  num12 = dataReader1.IsDBNull(12) ? -1 : BasePumpHelper.ToInt32(dataReader1[12]);
                  crtDate = dataReader1.IsDBNull(13) ? PumpHelper.MinDBDateTime : dataReader1.GetDateTime(13);
                }
                if (relType == 0)
                {
                  DictionaryValue info2 = this.plugin.Imdi.ImportedObjects.GetInfo(objectID != -1L ? objectID : num9);
                  if (info2 == null)
                  {
                    BasePumpHelper.AppManager.AddWarningMessage($"Изделие (part, ART_ID={num6}, VER_ID={num4}) не было импортировано, невозможно восстановить связь состава ({int32_2})!");
                    continue;
                  }
                  int objectType2 = (info2.Tag as ObjectInfo).ObjectType;
                  relType = !PumpHelper.IsInstanceOrParty(objectType1) || !PumpHelper.IsInstanceOrParty(objectType2) ? (PumpHelper.IsBuildingSection(objectType1) || PumpHelper.IsBuildingSection(objectType2) ? PumpHelper.RelTypeBuildingCompositionID : PumpHelper.RelTypeCompositionID) : PumpHelper.RelTypeInstancesID;
                }
                string oldKey1 = "";
                if (relType == PumpHelper.RelTypeDocumentationID)
                {
                  oldKey1 = BasePumpHelper.MakeCacheKey(num3, num8);
                  if (!this._sessionDocLinks.Contains(oldKey1))
                  {
                    if (this._docLinks.GetNewKey((object) oldKey1) != 0L)
                      continue;
                  }
                  else
                    continue;
                }
                this.logger.Write($"Add relation ({relType}): ProjID={num3} -> PartID={num8}");
                importedRelationList.AddRelationFromID(num3, num8, relType, crtDate);
                if (oldKey1 != "")
                  this._sessionDocLinks.Add(oldKey1);
                ImportingRelationEx importingRelationEx = importedRelationList.Items[importedRelationList.Items.CurrentIndex] as ImportingRelationEx;
                if (flag1)
                {
                  importingRelationEx.Cache = this._compositionCache;
                  importingRelationEx.OldKey = (object) int32_2;
                  importingRelationEx.Tag = (ITagImportObject) new CompositionTag(int32_3, int32_4);
                }
                else
                {
                  importingRelationEx.Cache = this._vcompositionCache;
                  importingRelationEx.OldKey = (object) int32_2;
                  importingRelationEx.Tag = (ITagImportObject) new VCompositionTag(int32_3, int32_4, num6, num4, num7, num5);
                }
                importingRelationEx.DocLinkKey = oldKey1;
                if (relType == PumpHelper.RelTypeInstancesID && num5 != -1 && objectID != -1L)
                  importedRelationList.AddAttributeInt(PumpHelper.AttrVerLinkID, objectID);
                if (specificationSection != null)
                  importedRelationList.AddAttributeLink(PumpHelper.AttrSPSectionID, specificationSection.ObjectID, specificationSection.Caption);
                else
                  importedRelationList.AddAttributeLink(PumpHelper.AttrSPSectionID, 0L, "");
                if (num12 != -1)
                  importedRelationList.AddAttributeInt(PumpHelper.AttrSortIndexID, (long) num12);
                importedRelationList.AddAttributeStr(PumpHelper.AttrTypeNoteID, str14);
                if (relType != PumpHelper.RelTypeDocumentationID)
                {
                  importedRelationList.AddAttributeMeasure(PumpHelper.AttrCountID, num11, measure.BaseMeasureId, strValue);
                  importedRelationList.AddAttributeStr(PumpHelper.AttrPositionID, str13);
                  if (int32_8 == 0)
                    PumpHelper.LinkTypesMapper.TryGetValue(key, out int32_8);
                  importedRelationList.AddAttributeInt(PumpHelper.AttrCompositionContextID, (long) int32_8);
                  foreach (KeyValuePair<string, DictionaryValue> keyValuePair in this.PCParamsMapper)
                  {
                    int int32_9 = Convert.ToInt32(keyValuePair.Value.NewObjectID);
                    PumpHelper.AddAttribute(importedRelationList, int32_9, dataReader1[keyValuePair.Key], keyValuePair.Value);
                  }
                  if (!PumpHelper.IsVariantsExists)
                  {
                    int int32_10 = Convert.ToInt32(dataReader1["VARGRP"]);
                    if (int32_10 != 0)
                    {
                      int int32_11 = Convert.ToInt32(dataReader1["VARNUM"]);
                      importedRelationList.AddAttributeInt(PumpHelper.AttrGroupNo, (long) int32_10);
                      importedRelationList.AddAttributeInt(PumpHelper.AttrSubInGroup, (long) int32_11);
                    }
                  }
                  if (str6 != "" && key == ' ' && !DBNull.Value.Equals(dataReader1["modelDoc"]))
                  {
                    importedRelationList.AddAttributeInt(PumpHelper.AttrBasedOnCadModelID, 1L);
                    long newKey = this._documentsCache.GetNewKey((object) Convert.ToInt32(dataReader1["modelDoc"]));
                    if (newKey > 0L)
                    {
                      string oldKey2 = BasePumpHelper.MakeCacheKey(num3, newKey);
                      if (!this._sessionDocLinks.Contains(oldKey2) && this._docLinks.GetNewKey((object) oldKey2) == 0L)
                      {
                        AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, importedRelationList);
                        importedRelationList.AddRelationFromID(num3, newKey, PumpHelper.RelTypeDocumentationID);
                        (importedRelationList.Items[importedRelationList.Items.CurrentIndex] as ImportingRelationEx).DocLinkKey = oldKey2;
                        this._sessionDocLinks.Add(oldKey2);
                      }
                    }
                  }
                }
                else if (this._cadModelTypeIDs.Contains(objectID > 0L ? this.plugin.Imdi.ImportedObjects.GetObjectTypeID(objectID) : this.plugin.Imdi.ImportedObjects.GetObjectTypeIDForID(num8)))
                  importedRelationList.AddAttributeInt(PumpHelper.AttrBasedOnCadModelID, 1L);
                AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, importedRelationList);
              }
            }
          }
          finally
          {
            ++index;
          }
        }
        importedRelationList.Import();
      }
      finally
      {
        dataReader1.Close();
        BlobHelper.Clear();
      }
      this.PumpCheckPoint($"Перекачка связей состава объектов {table} успешно завершена", 100);
    }
  }

  public override void Pump()
  {
    this.logger = BasePumpHelper.Logger;
    this._articlesCache = PumpCache.Category[ImportingCategory.Articles];
    this._measures = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    this._compositionCache = PumpCache.Category[ImportingCategory.Composition];
    this._vcompositionCache = PumpCache.Category[ImportingCategory.VComposition];
    this._documentationCache = PumpCache.Category[ImportingCategory.Documentation];
    this._docLinks = PumpCache.Category[ImportingCategory.DocLinks];
    this._documentsCache = PumpCache.Category[ImportingCategory.Documents];
    this._cadModelTypeIDs = this.plugin.Imdi.ObjectTypes.GetChildTypesRecursive(this.plugin.Imdi.ObjectTypes.GetByGuid(PumpHelper.ModelSBTypeGuid).ID, this.plugin.Imdi.ObjectTypes.GetByGuid(PumpHelper.SBTypeGuid).ID);
    try
    {
      this.PumpPC("PC");
      if (PluginSettings.PumpArtVersions || PluginSettings.PumpSysArtVersions)
        this.PumpPC("V_PC");
      this.logger.Write("=========Pump end\r\n\r\n");
    }
    catch (Exception ex)
    {
      this.logger.Write($"=========Pump abort ({ex.Message})\r\n\r\n");
      throw;
    }
    finally
    {
      this._documentsCache.Release();
      this._docLinks.Release();
      this._articlesCache.Release();
      this._compositionCache.Release();
      this._vcompositionCache.Release();
      this._documentationCache.Release();
    }
  }
}
