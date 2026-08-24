// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpSubstitutesClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки допустимых заменителей Search", "Перекачка данных о допустимых заменителях Search")]
internal class PumpSubstitutesClass(SearchDataPlugin plugin) : PumpClass((PluginClass) plugin)
{
  private Dictionary<int, List<IBoundPosItem>> _boundPos = new Dictionary<int, List<IBoundPosItem>>();
  private Dictionary<int, List<IVariantsItem>> _variants = new Dictionary<int, List<IVariantsItem>>();

  protected override Guid GUID => new Guid("3372809B-669B-4db5-A7AD-2C38B7DB4179");

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.Articles, ImportingCategory.Substitutes, ImportingCategory.VSubstitutes, ImportingCategory.SubstitutesGroups, ImportingCategory.Composition, ImportingCategory.VComposition, ImportingCategory.VSubstitutesGroups);
    try
    {
      this.GetVariants(cache, ImportingCategory.Substitutes, BoundPosFactory.TableName, VariantsFactory.TableName, 1);
      int num1 = this.PumpSubstitutes(cache, ImportingCategory.Composition, ImportingCategory.Substitutes, ImportingCategory.SubstitutesGroups, 6, 50);
      this.GetVariants(cache, ImportingCategory.VSubstitutes, BoundPosFactory.TableNameVersions, VariantsFactory.TableNameVersions, 51);
      int num2 = num1 + this.PumpSubstitutes(cache, ImportingCategory.VComposition, ImportingCategory.VSubstitutes, ImportingCategory.VSubstitutesGroups, 56, 99);
      this.PumpCheckPoint("Перекачка допустимых замен успешно завершена", 100);
      this.plugin.appManager.AddInfoMessage($"Импортировано допустимых замен: {num2}");
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.Articles, ImportingCategory.Substitutes, ImportingCategory.VSubstitutes, ImportingCategory.SubstitutesGroups, ImportingCategory.Composition, ImportingCategory.VComposition, ImportingCategory.VSubstitutesGroups);
      if (this._boundPos != null)
        this._boundPos.Clear();
      if (this._variants != null)
        this._variants.Clear();
    }
  }

  private void GetVariants(
    IImportingData cacheData,
    ImportingCategory category,
    string tableNameBoundPos,
    string tableNameVariants,
    int startPercents)
  {
    int index1 = 0;
    this._boundPos.Clear();
    this.PumpCheckPoint("Получение данных из таблицы " + tableNameBoundPos, startPercents);
    int tableRecordsCount1 = this.GetTableRecordsCount(tableNameBoundPos);
    string empty = string.Empty;
    IDataReader dataReader1 = this.GetDataReader(!(tableNameBoundPos == BoundPosFactory.TableNameVersions) ? "SELECT p.PRJLINK_ID, p.PROJ_AID, p.PART_AID, p.COUNT_PC, p.MU_ID, p.RAZDEL, p.POSITIO, p.NOTE, p.FORMAT, p.PR_ID, p.CTX_ID, p.CTX_FL FROM BOUNDPOS P ORDER BY P.PRJLINK_ID" : "SELECT P.PRJLINK_ID, P.PROJ_AID, P.PART_AID, P.COUNT_PC, P.MU_ID, P.RAZDEL, P.POSITIO, P.NOTE, P.FORMAT, P.PR_ID, P.CTX_ID, P.CTX_FL,V2.ART_ID AS PART_ART_ID, V2.ART_VER_ID AS PART_VER_ID FROM V_BOUNDPOS P, V_ARTICLES V2  WHERE P.PART_AID = V2.VART_ID ORDER BY P.PRJLINK_ID");
    string format1 = $"Обработка записи из таблицы {tableNameBoundPos} ({{0}} из {{1}})";
    try
    {
      BoundPosFactory boundPosFactory = new BoundPosFactory(dataReader1, this.plugin.Idw.AppManager, tableNameBoundPos == BoundPosFactory.TableNameVersions);
      while (dataReader1.Read())
      {
        ++index1;
        this.PumpCheckPoint(string.Format(format1, (object) index1, (object) tableRecordsCount1), this.CalculatePercent(tableRecordsCount1, index1, startPercents, startPercents + 1));
        IBoundPosItem boundPosItem = boundPosFactory.NewItem(dataReader1);
        if (cacheData.GetNewKey(category, (object) boundPosItem.PrjLinkID) == 0L)
        {
          if (!this._boundPos.ContainsKey(boundPosItem.PrjLinkID))
            this._boundPos.Add(boundPosItem.PrjLinkID, new List<IBoundPosItem>());
          this._boundPos[boundPosItem.PrjLinkID].Add(boundPosItem);
        }
      }
    }
    finally
    {
      dataReader1.Close();
    }
    this._variants.Clear();
    this.PumpCheckPoint("Получение данных из таблицы " + tableNameVariants, startPercents + 2);
    int index2 = 0;
    int tableRecordsCount2 = this.GetTableRecordsCount(tableNameVariants);
    IDataReader dataReader2 = this.GetDataReader(!(tableNameVariants == VariantsFactory.TableNameVersions) ? "SELECT P.PRJLINK_ID, P.PROJ_AID, P.PART_AID, P.COUNT_PC, P.MU_ID, P.RAZDEL, P.POSITIO, P.NOTE, P.VAR_MODE, P.VARNO, P.FORMAT, P.PR_ID, P.CTX_ID, P.CTX_FL FROM VARIANTS P ORDER BY P.PRJLINK_ID, P.VARNO" : "SELECT P.PRJLINK_ID, P.PROJ_AID, P.PART_AID, P.COUNT_PC, P.MU_ID, P.RAZDEL, P.POSITIO, P.NOTE, P.VAR_MODE, P.VARNO, P.FORMAT, P.PR_ID, P.CTX_ID, P.CTX_FL,V2.ART_ID AS PART_ART_ID, V2.ART_VER_ID AS PART_VER_ID FROM V_VARIANTS P, V_ARTICLES V2  WHERE P.PART_AID = V2.VART_ID ORDER BY P.PRJLINK_ID, P.VARNO");
    string format2 = $"Обработка записи из таблицы {tableNameVariants} ({{0}} из {{1}})";
    try
    {
      VariantsFactory variantsFactory = new VariantsFactory(dataReader2, this.plugin.Idw.AppManager, tableNameVariants == VariantsFactory.TableNameVersions);
      while (dataReader2.Read())
      {
        ++index2;
        this.PumpCheckPoint(string.Format(format2, (object) index2, (object) tableRecordsCount2), this.CalculatePercent(tableRecordsCount2, index2, startPercents + 2, startPercents + 5));
        IVariantsItem variantsItem = variantsFactory.NewItem(dataReader2);
        if (cacheData.GetNewKey(category, (object) variantsItem.PrjLinkID) == 0L)
        {
          if (!this._variants.ContainsKey(variantsItem.PrjLinkID))
            this._variants.Add(variantsItem.PrjLinkID, new List<IVariantsItem>());
          this._variants[variantsItem.PrjLinkID].Add(variantsItem);
        }
      }
    }
    finally
    {
      dataReader2.Close();
    }
  }

  private int PumpSubstitutes(
    IImportingData cacheData,
    ImportingCategory category,
    ImportingCategory saveCategory,
    ImportingCategory saveGroupCategory,
    int startPercent,
    int endPercent)
  {
    bool flag1 = category == ImportingCategory.Composition;
    int num1 = 0;
    this.PumpCheckPoint("Определение количества допустимых замен", startPercent + 1);
    Dictionary<object, DictionaryValue> category1 = cacheData.GetCategory(category);
    int index1 = 0;
    int count = category1.Count;
    string format = "Обработка записи из таблицы PC ({0} из {1})";
    IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair1 in category1)
    {
      int int32 = Convert.ToInt32(keyValuePair1.Key);
      DictionaryValue dictionaryValue1 = keyValuePair1.Value;
      CompositionTag tag1 = dictionaryValue1.Tag as CompositionTag;
      int partAid = tag1.PartAID;
      int projAid = tag1.ProjAID;
      ++index1;
      this.PumpCheckPoint(string.Format(format, (object) index1, (object) count), this.CalculatePercent(count, index1, startPercent + 1, endPercent));
      Dictionary<int, List<PumpSubstitutesClass.SubstitutionRelation>> dictionary = new Dictionary<int, List<PumpSubstitutesClass.SubstitutionRelation>>(10);
      if (cacheData.GetNewKey(saveCategory, (object) int32) == 0L)
      {
        dictionary.Add(0, new List<PumpSubstitutesClass.SubstitutionRelation>());
        if (flag1)
          dictionary[0].Add(new PumpSubstitutesClass.SubstitutionRelation(partAid, -1));
        else if (dictionaryValue1.Tag is VCompositionTag tag2)
        {
          dictionary[0].Add(new PumpSubstitutesClass.SubstitutionRelation(tag2.PartArtID, tag2.PartArtVerID));
        }
        else
        {
          this.plugin.Idw.AppManager.AddWarningMessage($"Не найдено дополнительных данных по связи {int32} между projAID {projAid}-> partAID {partAid} в SEARCH");
          continue;
        }
        List<IBoundPosItem> boundPosItemList = (List<IBoundPosItem>) null;
        if (this._boundPos.TryGetValue(int32, out boundPosItemList))
        {
          foreach (IBoundPosItem boundPosItem in boundPosItemList)
          {
            if (flag1)
              dictionary[0].Add(new PumpSubstitutesClass.SubstitutionRelation(boundPosItem.PartAID, -1, boundPosItem.Note, PumpHelper.GetCountAttrValue(boundPosItem.MuID, boundPosItem.CountPC), boundPosItem.CtxID, boundPosItem.Razdel, boundPosItem.Positio));
            else
              dictionary[0].Add(new PumpSubstitutesClass.SubstitutionRelation(boundPosItem.ArtInfo.PartID, boundPosItem.ArtInfo.PartVerID, boundPosItem.Note, PumpHelper.GetCountAttrValue(boundPosItem.MuID, boundPosItem.CountPC), boundPosItem.CtxID, boundPosItem.Razdel, boundPosItem.Positio));
          }
        }
        List<IVariantsItem> variantsItemList = (List<IVariantsItem>) null;
        int key1 = 0;
        int num2 = -1;
        if (this._variants.TryGetValue(int32, out variantsItemList))
        {
          variantsItemList.Sort((IComparer<IVariantsItem>) new PumpSubstitutesClass.VarNoComparer());
          for (int index2 = 0; index2 < variantsItemList.Count; ++index2)
          {
            IVariantsItem variantsItem = variantsItemList[index2];
            if (index2 == 0)
            {
              key1 = 1;
              dictionary.Add(key1, new List<PumpSubstitutesClass.SubstitutionRelation>());
              int varNo = variantsItem.VarNo;
            }
            else if (variantsItem.VarNo - 1 != num2)
            {
              ++key1;
              dictionary.Add(key1, new List<PumpSubstitutesClass.SubstitutionRelation>());
            }
            if (flag1)
              dictionary[key1].Add(new PumpSubstitutesClass.SubstitutionRelation(variantsItem.PartAID, -1, variantsItem.Note, PumpHelper.GetCountAttrValue(variantsItem.MuID, variantsItem.CountPC), variantsItem.CtxID, variantsItem.Razdel, variantsItem.Positio));
            else
              dictionary[key1].Add(new PumpSubstitutesClass.SubstitutionRelation(variantsItem.ArtInfo.PartID, variantsItem.ArtInfo.PartVerID, variantsItem.Note, PumpHelper.GetCountAttrValue(variantsItem.MuID, variantsItem.CountPC), variantsItem.CtxID, variantsItem.Razdel, variantsItem.Positio));
            num2 = variantsItem.VarNo;
          }
        }
        if (dictionary.Count > 1)
        {
          long newKey1 = cacheData.GetNewKey(saveGroupCategory, (object) projAid);
          long newKey2;
          if (newKey1 == 0L)
          {
            newKey2 = 1L;
            cacheData.AddValue(saveGroupCategory, (object) projAid, 1L);
          }
          else
          {
            newKey2 = newKey1 + 1L;
            cacheData.SetNewKey(saveGroupCategory, (object) projAid, newKey2);
          }
          long projId = 0;
          try
          {
            if (flag1)
            {
              ArticleTag tag3 = (cacheData.GetValue(ImportingCategory.Articles, (object) projAid) ?? throw new Exception($"Не найден ProjAID = {projAid}")).Tag as ArticleTag;
              projId = tag3.Versions[tag3.VersionID];
            }
            else
            {
              VCompositionTag tag4 = dictionaryValue1.Tag as VCompositionTag;
              DictionaryValue dictionaryValue2 = cacheData.GetValue(ImportingCategory.Articles, (object) tag4.PrjArtID);
              if (dictionaryValue2 == null)
                throw new Exception($"Не найден ProjAID = {tag4.PrjArtID}");
              if (!(dictionaryValue2.Tag as ArticleTag).Versions.TryGetValue(tag4.PrjArtVerID, out projId))
              {
                if (!PluginSettings.PumpSysArtVersions)
                  throw new AbortException();
                throw new Exception($"Не найдена версия ProjAID = {tag4.PrjArtID}, VerID = {tag4.PrjArtVerID}");
              }
            }
            foreach (List<PumpSubstitutesClass.SubstitutionRelation> substitutionRelationList in dictionary.Values)
            {
              foreach (PumpSubstitutesClass.SubstitutionRelation substitutionRelation in substitutionRelationList)
              {
                ArticleTag tag5 = (cacheData.GetValue(ImportingCategory.Articles, (object) substitutionRelation.PartAID) ?? throw new Exception($"Не найден PartAID = {substitutionRelation.PartAID}")).Tag as ArticleTag;
                int key2 = flag1 ? tag5.VersionID : substitutionRelation.VersionID;
                if (!tag5.Versions.TryGetValue(key2, out substitutionRelation.newPartID))
                {
                  if (!PluginSettings.PumpSysArtVersions)
                    throw new AbortException();
                  throw new Exception($"Не найдена версия PartAID = {substitutionRelation.PartAID}, VerID = {key2}");
                }
              }
            }
          }
          catch (Exception ex)
          {
            if (!(ex is AbortException))
            {
              this.plugin.Idw.AppManager.AddWarningMessage(ex.Message);
              continue;
            }
            continue;
          }
          foreach (KeyValuePair<int, List<PumpSubstitutesClass.SubstitutionRelation>> keyValuePair2 in dictionary)
          {
            int key3 = keyValuePair2.Key;
            List<PumpSubstitutesClass.SubstitutionRelation> substitutionRelationList = keyValuePair2.Value;
            for (int index3 = 0; index3 < substitutionRelationList.Count; ++index3)
            {
              if (substitutionRelationList[index3].newPartID != -1L)
              {
                bool flag2 = false;
                List<int> intList = new List<int>(7);
                long prjLinkID = 0;
                if (key3 == 0)
                {
                  prjLinkID = cacheData.GetNewKey(category, (object) int32);
                  if (prjLinkID == 0L)
                    this.plugin.Idw.AppManager.AddWarningMessage($"Не найдена связь {int32} в SEARCH");
                }
                if (prjLinkID == 0L)
                {
                  importedRelationList.AddRelation(projId, substitutionRelationList[index3].newPartID, PumpHelper.RelTypeCompositionID);
                  flag2 = true;
                }
                else
                  importedRelationList.UseRelation(prjLinkID);
                importedRelationList.AddAttributeInt(PumpHelper.AttrGroupNo, newKey2);
                importedRelationList.AddAttributeInt(PumpHelper.AttrSubInGroup, (long) key3);
                if (flag2)
                {
                  importedRelationList.AddAttributeInt(PumpHelper.AttrVerLinkID, substitutionRelationList[index3].newPartID);
                  importedRelationList.AddAttributeStr(PumpHelper.AttrTypeNoteID, substitutionRelationList[index3].Note);
                  importedRelationList.AddAttributeInt(PumpHelper.AttrCompositionContextID, (long) substitutionRelationList[index3].ContextID);
                  if (substitutionRelationList[index3].SpecificationSection != null)
                    importedRelationList.AddAttributeLink(PumpHelper.AttrSPSectionID, substitutionRelationList[index3].SpecificationSection.ObjectID, substitutionRelationList[index3].SpecificationSection.Caption);
                  importedRelationList.AddAttributeStr(PumpHelper.AttrPositionID, substitutionRelationList[index3].Position);
                  if (substitutionRelationList[index3].MeasuredValue != null)
                  {
                    MeasuredValue baseMeasure = MeasureHelper.ConvertToBaseMeasure(substitutionRelationList[index3].MeasuredValue);
                    importedRelationList.AddAttributeMeasure(PumpHelper.AttrCountID, baseMeasure.Value, baseMeasure.MeasureID, substitutionRelationList[index3].MeasuredValue.Caption);
                  }
                  AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, importedRelationList);
                }
              }
            }
          }
        }
        dictionary.Clear();
        ++num1;
        cacheData.AddValue(saveCategory, (object) int32, long.MinValue);
      }
    }
    importedRelationList.Import();
    return num1;
  }

  private class VarNoComparer : IComparer<IVariantsItem>
  {
    public int Compare(IVariantsItem x, IVariantsItem y) => x.VarNo.CompareTo(y.VarNo);
  }

  private class SubstitutionRelation
  {
    public long newPartID;
    public int PartAID;
    public int VersionID;
    public MeasuredValue MeasuredValue;
    public string Note;
    public int ContextID;
    public SpecificationSection SpecificationSection;
    public string Position;

    public SubstitutionRelation(int partAID, int versionID)
    {
      this.PartAID = partAID;
      this.VersionID = versionID;
      this.Note = string.Empty;
      this.newPartID = 0L;
      this.ContextID = 0;
      this.Position = string.Empty;
      this.SpecificationSection = (SpecificationSection) null;
      this.MeasuredValue = (MeasuredValue) null;
    }

    public SubstitutionRelation(
      int partAID,
      int versionID,
      string note,
      MeasuredValue measuredValue,
      int contextID,
      int sectID,
      string position)
    {
      this.PartAID = partAID;
      this.VersionID = versionID;
      this.Note = note;
      this.newPartID = 0L;
      this.ContextID = contextID;
      this.MeasuredValue = measuredValue;
      this.SpecificationSection = (SpecificationSection) null;
      PumpHelper.SpecificationSections.TryGetValue(sectID, out this.SpecificationSection);
      this.Position = position;
    }
  }
}
