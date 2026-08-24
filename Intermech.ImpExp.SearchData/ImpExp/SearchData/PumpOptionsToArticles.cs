// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpOptionsToArticles
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("", "Настройка опций конфигуратора составов для изделий ")]
internal class PumpOptionsToArticles : PumpClass
{
  protected SearchDataPlugin plugin;

  private int _countRecInPackage
  {
    get
    {
      return (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
    }
  }

  public PumpOptionsToArticles(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  protected override Guid GUID => new Guid("1CB4E93B-0BA3-45e6-90AD-B41A8F12200D");

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ConfiguratorOptions, ImportingCategory.Articles, ImportingCategory.ConfiguratorOptionsToArticle, ImportingCategory.ConfiguratorOptionValues, ImportingCategory.ConfiguratorHideValOptionsToArticle, ImportingCategory.ConfiguratorRestrictRules, ImportingCategory.ConfiguratorApplicabilitiesCriterions, ImportingCategory.Composition, ImportingCategory.VComposition);
    try
    {
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      this.PumpCheckPoint("Настройка опций конфигуратора составов для изделий", 1);
      this.PumpOptionsToArticle(userSession, cache, 2, 50);
      this.PumpCheckPoint("Настройка скрытых значений опций для изделий", 51);
      this.PumpHideValues(userSession, cache, 52, 61);
      this.PumpCheckPoint("Настройка несовместимости опций для изделий", 62);
      this.PumpRestrictRules(userSession, cache, 63 /*0x3F*/, 81);
      this.PumpCheckPoint("Настройка условий применения объектов", 82);
      this.PumpApplicabilitiesCriterions(userSession, cache, 83, 99);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ConfiguratorOptions, ImportingCategory.Articles, ImportingCategory.ConfiguratorOptionsToArticle, ImportingCategory.ConfiguratorOptionValues, ImportingCategory.ConfiguratorHideValOptionsToArticle, ImportingCategory.ConfiguratorRestrictRules, ImportingCategory.ConfiguratorApplicabilitiesCriterions, ImportingCategory.Composition, ImportingCategory.VComposition);
    }
    this.PumpCheckPoint("Настройка опций конфигуратора составов для изделий успешно завершена", 100);
  }

  private void ImportVisibleOptions(
    IImportedObjectList iolIm,
    IImportingData cacheData,
    int articleID,
    List<int> hideOptions,
    long lastArticle,
    int attributeVisibleOptionValuesID,
    List<int> hidePackage)
  {
    if (!(cacheData.GetTag(ImportingCategory.ConfiguratorOptionsToArticle, (object) articleID) is ListIntTag tag1))
      return;
    VisibleOptionValues visibleOptionValues = new VisibleOptionValues();
    for (int index1 = 0; index1 < tag1.Items.Count; ++index1)
    {
      ArticleOptionsTag tag2 = cacheData.GetTag(ImportingCategory.ConfiguratorOptions, (object) tag1.Items[index1]) as ArticleOptionsTag;
      bool obligatory = false;
      bool flag = false;
      int oldKey = -1;
      string empty1 = string.Empty;
      using (IDataReader dataReader = BasePumpHelper.S4Query(this.plugin.idb2.DbConnection, "select need_in_zakaz, need_default, default_value, msg from pc_options_restrict where proj_aid=@p1 and opt_id=@p2", CommandBehavior.Default, (object) articleID, (object) tag1.Items[index1]))
      {
        while (dataReader.Read())
        {
          obligatory = BasePumpHelper.ToInt32(dataReader[0]) == 1;
          flag = !dataReader.IsDBNull(1) && BasePumpHelper.ToInt32(dataReader[1]) == 1;
          oldKey = dataReader.IsDBNull(2) ? -1 : BasePumpHelper.ToInt32(dataReader[2]);
          if (!dataReader.IsDBNull(3))
          {
            dataReader.GetString(3);
          }
          else
          {
            string empty2 = string.Empty;
          }
        }
      }
      List<int> intList = new List<int>();
      for (int index2 = 0; index2 < tag2.OptionValues.Count; ++index2)
      {
        string caption = cacheData.GetCaption(ImportingCategory.ConfiguratorOptionValues, (object) tag2.OptionValues[index2]);
        visibleOptionValues.SetVisibleOptionValue(tag2.Guid, caption, hideOptions.IndexOf(tag2.OptionValues[index2]) < 0);
      }
      visibleOptionValues.SetObligatoryOption(tag2.Guid, obligatory);
      if (flag && oldKey >= 0)
      {
        string caption = cacheData.GetCaption(ImportingCategory.ConfiguratorOptionValues, (object) oldKey);
        if (caption != string.Empty)
          visibleOptionValues.SetDefaultOptionValue(tag2.Guid, caption);
        else
          this.plugin.appManager.AddWarningMessage($"Не удалось установить значение по умолчанию optval_id={oldKey} опции {tag2.Guid} для объекта {lastArticle}.");
      }
    }
    iolIm.UseObject(lastArticle);
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeVisibleOptionValuesID);
    List<string> stringList = StringsHelper.SplitString(visibleOptionValues.ToString(attributeVisibleOptionValuesID), (int) attributeType.SizeType);
    for (int index = 0; index < stringList.Count; ++index)
      iolIm.AddAttribute(attributeVisibleOptionValuesID, AttrValueType.stringVal, (object) stringList[index], index);
    hideOptions.Clear();
    hidePackage.Add(articleID);
  }

  private void ImportOptionLink(
    IImportedObjectList iolIm,
    IImportingData cacheData,
    long newArticleObjectID,
    int articleID,
    int attributeOptionsLinkID,
    Dictionary<long, string> locOptions)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query(this.plugin.idb2.DbConnection, "select val_id, opt_id from pc_options_restrictrule where proj_aid=@p1", CommandBehavior.Default, (object) articleID))
    {
      while (dataReader.Read())
      {
        int int32 = Convert.ToInt32(cacheData.GetNewKey(ImportingCategory.ConfiguratorOptionValues, (object) BasePumpHelper.ToInt32(dataReader[0])));
        DictionaryValue dictionaryValue1 = cacheData.GetValue(ImportingCategory.ConfiguratorOptions, (object) int32);
        if (!locOptions.ContainsKey(dictionaryValue1.NewObjectID))
          locOptions.Add(dictionaryValue1.NewObjectID, dictionaryValue1.Caption);
        if (!dataReader.IsDBNull(1))
        {
          DictionaryValue dictionaryValue2 = cacheData.GetValue(ImportingCategory.ConfiguratorOptions, (object) BasePumpHelper.ToInt32(dataReader[1]));
          if (!locOptions.ContainsKey(dictionaryValue2.NewObjectID))
            locOptions.Add(dictionaryValue2.NewObjectID, dictionaryValue2.Caption);
        }
      }
    }
    iolIm.UseObject(newArticleObjectID);
    int inListID = 0;
    foreach (KeyValuePair<long, string> locOption in locOptions)
    {
      iolIm.AddAttributeLink(attributeOptionsLinkID, locOption.Key, locOption.Value, inListID);
      ++inListID;
    }
    locOptions.Clear();
  }

  private void ImportRestrictRule(
    IImportedObjectList iolIm,
    IImportingData cacheData,
    PumpOptionsToArticles.Rule rule,
    int attributeRestrictRulesID)
  {
    if (rule.Options.Count == 0)
      return;
    ObjectIncompatibilitiesCollection incompatibilitiesCollection = new ObjectIncompatibilitiesCollection();
    PumpOptionsToArticles.RuleOption ruleOption = (PumpOptionsToArticles.RuleOption) null;
    ObjectIncompatibilityCriterion incompatibilityCriterion = (ObjectIncompatibilityCriterion) null;
    for (int index = 0; index < rule.Options.Count; ++index)
    {
      if (ruleOption == null || !ruleOption.OptionGuid.Equals(rule.Options[index].OptionGuid))
      {
        incompatibilityCriterion = new ObjectIncompatibilityCriterion(rule.Options[index].OptionGuid, string.Empty, Guid.Empty, string.Empty, Operator.Equals, LogicalFunction.Or, new PdmCriterionsCollection());
        incompatibilityCriterion.CriterionType = PdmCriterionType.Stub;
        incompatibilitiesCollection.Add((IPdmCriterion) incompatibilityCriterion);
      }
      ObjectIncompatibilityCriterion parent = new ObjectIncompatibilityCriterion(rule.Options[index].OptionGuid, rule.Options[index].OpValIndex, Guid.Empty, string.Empty, Operator.Equals, LogicalFunction.Or, new PdmCriterionsCollection());
      parent.CriterionType = PdmCriterionType.Stub;
      incompatibilityCriterion.Items.Add((IPdmCriterion) parent);
      this.AddChildCriteries(cacheData, rule.Options[index].OptionGuid, rule.Options[index].OpValIndex, rule.Options[index].Items, (IPdmCriterion) parent, LogicalFunction.Or);
      ruleOption = rule.Options[index];
    }
    object[] attributeValues = incompatibilitiesCollection.ToAttributeValues(attributeRestrictRulesID);
    if (attributeValues == null)
      return;
    iolIm.UseObject(rule.NewArticleID);
    for (int numInList = 0; numInList < attributeValues.Length; ++numInList)
      iolIm.AddAttribute(attributeRestrictRulesID, AttrValueType.stringVal, attributeValues[numInList], numInList);
  }

  private void AddChildApplicabilities(
    IImportingData cacheData,
    List<PumpOptionsToArticles.TreeCondition> items,
    IPdmCriterion parent,
    LogicalFunction function)
  {
    if (items == null || items.Count == 0)
      return;
    for (int index = 0; index < items.Count; ++index)
    {
      if (items[index] is PumpOptionsToArticles.ConditionCollection)
      {
        if (items[index].Items.Count > 0)
        {
          ObjectsApplicabilitiesCriterionsCollection parent1 = new ObjectsApplicabilitiesCriterionsCollection(function);
          this.AddToParent(parent, (IPdmCriterion) parent1);
          this.AddChildApplicabilities(cacheData, items[index].Items, (IPdmCriterion) parent1, ((PumpOptionsToArticles.ConditionCollection) items[index]).LogicalFunction);
        }
      }
      else
      {
        DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ConfiguratorOptionValues, (object) ((PumpOptionsToArticles.RuleCondition) items[index]).OptValID);
        ObjectsApplicabilitiesCriterion applicabilitiesCriterion = new ObjectsApplicabilitiesCriterion((cacheData.GetTag(ImportingCategory.ConfiguratorOptions, (object) Convert.ToInt32(dictionaryValue.NewObjectID)) as ArticleOptionsTag).Guid, dictionaryValue.Caption, ((PumpOptionsToArticles.RuleCondition) items[index]).Operator, function, new ObjectsApplicabilitiesCriterionsCollection((object) ((PumpOptionsToArticles.RuleCondition) items[index]).Operator));
        this.AddToParent(parent, (IPdmCriterion) applicabilitiesCriterion);
      }
    }
  }

  private void AddChildCriteries(
    IImportingData cacheData,
    Guid optionGuid,
    string optionValue,
    List<PumpOptionsToArticles.TreeCondition> items,
    IPdmCriterion parent,
    LogicalFunction function)
  {
    if (items == null || items.Count == 0)
      return;
    for (int index = 0; index < items.Count; ++index)
    {
      if (items[index] is PumpOptionsToArticles.ConditionCollection)
      {
        if (items[index].Items.Count > 0)
        {
          if (parent is ObjectIncompatibilityCriterion && ((PdmCriterion) parent).CriterionType == PdmCriterionType.Stub && items.Count == 1)
          {
            this.AddChildCriteries(cacheData, optionGuid, optionValue, items[index].Items, parent, ((PumpOptionsToArticles.ConditionCollection) items[index]).LogicalFunction);
          }
          else
          {
            ObjectIncompatibilitiesCollection parent1 = new ObjectIncompatibilitiesCollection(function);
            this.AddToParent(parent, (IPdmCriterion) parent1);
            this.AddChildCriteries(cacheData, optionGuid, optionValue, items[index].Items, (IPdmCriterion) parent1, ((PumpOptionsToArticles.ConditionCollection) items[index]).LogicalFunction);
          }
        }
      }
      else
      {
        DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ConfiguratorOptionValues, (object) ((PumpOptionsToArticles.RuleCondition) items[index]).OptValID);
        ArticleOptionsTag tag = cacheData.GetTag(ImportingCategory.ConfiguratorOptions, (object) Convert.ToInt32(dictionaryValue.NewObjectID)) as ArticleOptionsTag;
        ObjectIncompatibilityCriterion incompatibilityCriterion = new ObjectIncompatibilityCriterion(optionGuid, optionValue, tag.Guid, dictionaryValue.Caption, ((PumpOptionsToArticles.RuleCondition) items[index]).Operator, function, new PdmCriterionsCollection());
        this.AddToParent(parent, (IPdmCriterion) incompatibilityCriterion);
      }
    }
  }

  private void AddToParent(IPdmCriterion parent, IPdmCriterion item)
  {
    switch (parent)
    {
      case PdmCriterion _:
        ((PdmCriterion) parent).Items.Add(item);
        break;
      case PdmCriterionsCollection _:
        ((PdmCriterionsCollection) parent).Add(item);
        break;
    }
  }

  private ObjectIncompatibilityCriterion AddStub(ObjectIncompatibilityCriterion critery)
  {
    PdmCriterionsCollection criterionsCollection = new PdmCriterionsCollection();
    ObjectIncompatibilityCriterion incompatibilityCriterion1 = new ObjectIncompatibilityCriterion(critery.Option, critery.Value, Guid.Empty, string.Empty, Operator.Equals, LogicalFunction.Or, new PdmCriterionsCollection());
    incompatibilityCriterion1.CriterionType = PdmCriterionType.Stub;
    incompatibilityCriterion1.Items.Add((IPdmCriterion) critery);
    ObjectIncompatibilityCriterion incompatibilityCriterion2 = new ObjectIncompatibilityCriterion(critery.Option, string.Empty, Guid.Empty, string.Empty, Operator.Equals, LogicalFunction.Or, new PdmCriterionsCollection());
    incompatibilityCriterion2.Items.Add((IPdmCriterion) incompatibilityCriterion1);
    incompatibilityCriterion2.CriterionType = PdmCriterionType.Stub;
    return incompatibilityCriterion2;
  }

  private void PumpOptionsToArticle(
    IUserSession session,
    IImportingData cacheData,
    int percentStart,
    int percentEnd)
  {
    string format = "Настройка опций конфигуратора составов для изделий ({0} из {1})";
    int index1 = 0;
    int tableRecordsCount = this.GetTableRecordsCount("PC_OPTART");
    int attributeId = session.GetAttributeType(new Guid("cad015a9-306c-11d8-b4e9-00304f19f545")).AttributeID;
    IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
    iolIm.NewObjectsOnlyInList = false;
    Dictionary<long, List<int>> package = new Dictionary<long, List<int>>(this._countRecInPackage);
    iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      int index2 = 0;
      foreach (KeyValuePair<long, List<int>> keyValuePair in package)
      {
        if (iolIm.Items[index2].Object.Object_id != 0L && iolIm.Items[index2].Object.Object_id != -1L)
          cacheData.AddValue(ImportingCategory.ConfiguratorOptionsToArticle, (object) keyValuePair.Key, iolIm.Items[index2].Object.Object_id, (ITagImportObject) new ListIntTag(keyValuePair.Value));
        else
          this.plugin.appManager.AddWarningMessage($"Опции для изделия {keyValuePair.Key} не установлены. См. серверный лог.");
        ++index2;
      }
      package.Clear();
    });
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "select proj_aid, opt_id from pc_optart order by proj_aid";
      List<PumpOptionsToArticles.ArticleOptions> articleOptionsList = new List<PumpOptionsToArticles.ArticleOptions>();
      IDataReader dataReader = command.ExecuteReader();
      try
      {
        while (dataReader.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, percentStart, percentEnd));
          int int32_1 = BasePumpHelper.ToInt32(dataReader[0]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader[1]);
          DictionaryValue dictionaryValue1 = cacheData.GetValue(ImportingCategory.Articles, (object) int32_1);
          if ((dictionaryValue1 != null ? dictionaryValue1.NewObjectID : 0L) == 0L)
          {
            this.plugin.appManager.AddWarningMessage($"Изделие {int32_1} не импортировано. Настройка опций конфигуратора составов для него невозможна.");
          }
          else
          {
            DictionaryValue dictionaryValue2 = cacheData.GetValue(ImportingCategory.ConfiguratorOptions, (object) int32_2);
            long newObjectId = dictionaryValue2 != null ? dictionaryValue2.NewObjectID : 0L;
            if (newObjectId == 0L)
            {
              this.plugin.appManager.AddWarningMessage($"Опция конфигуратора составов {int32_2} не импортирована. Настройка его для изделия невозможна.");
            }
            else
            {
              ArticleTag tag = dictionaryValue1.Tag as ArticleTag;
              long newArticleObjectID = tag.Versions[tag.VersionID];
              if (cacheData.GetNewKey(ImportingCategory.ConfiguratorOptionsToArticle, (object) newArticleObjectID) == 0L)
              {
                PumpOptionsToArticles.ArticleOptions articleOptions = articleOptionsList.Find((Predicate<PumpOptionsToArticles.ArticleOptions>) (item => item.NewObjectID == newArticleObjectID));
                if (articleOptions == null)
                {
                  articleOptions = new PumpOptionsToArticles.ArticleOptions(newArticleObjectID, int32_1);
                  articleOptionsList.Add(articleOptions);
                }
                articleOptions.AddOption(newObjectId, int32_2, dictionaryValue2.Caption);
              }
            }
          }
        }
      }
      finally
      {
        dataReader.Close();
      }
      for (int index3 = 0; index3 < articleOptionsList.Count; ++index3)
      {
        PumpOptionsToArticles.ArticleOptions articleOptions = articleOptionsList[index3];
        List<int> intList = new List<int>(articleOptions.Options.Count);
        Dictionary<long, string> locOptions = new Dictionary<long, string>(articleOptions.Options.Count);
        for (int index4 = 0; index4 < articleOptions.Options.Count; ++index4)
        {
          PumpOptionsToArticles.OptionInfo option = articleOptions.Options[index4];
          intList.Add(option.OldID);
          locOptions.Add(option.NewObjectID, option.Caption);
        }
        this.ImportOptionLink(iolIm, cacheData, articleOptions.NewObjectID, articleOptions.OldID, attributeId, locOptions);
        package.Add(articleOptions.NewObjectID, intList);
      }
      iolIm.Import();
    }
  }

  private void PumpHideValues(
    IUserSession session,
    IImportingData cacheData,
    int percentStart,
    int percentEnd)
  {
    string format = "Настройка скрытых значений опций для изделий ({0} из {1})";
    int index1 = 0;
    int tableRecordsCount = this.GetTableRecordsCount("pc_option_hideval");
    int attributeId = session.GetAttributeType(new Guid("cad015a1-306c-11d8-b4e9-00304f19f545")).AttributeID;
    IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
    iolIm.NewObjectsOnlyInList = false;
    List<int> hidePackage = new List<int>();
    iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index2 = 0; index2 < iolIm.Items.Count; ++index2)
      {
        if (iolIm.Items[index2].Object.Object_id != 0L && iolIm.Items[index2].Object.Object_id != -1L)
          cacheData.AddValue(ImportingCategory.ConfiguratorHideValOptionsToArticle, (object) hidePackage[index2], iolIm.Items[index2].Object.Object_id);
        else
          this.plugin.appManager.AddWarningMessage($"Скрытие опций для изделия {hidePackage[index2]} не установлены. См. серверный лог.");
      }
      hidePackage.Clear();
    });
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "select optval_id, proj_aid from pc_option_hideval order by proj_aid";
      IDataReader dataReader = command.ExecuteReader();
      try
      {
        long lastArticle = -1;
        int articleID = -1;
        List<int> hideOptions = new List<int>();
        while (dataReader.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, percentStart, percentEnd));
          int int32_1 = BasePumpHelper.ToInt32(dataReader[0]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader[1]);
          if (cacheData.GetNewKey(ImportingCategory.ConfiguratorHideValOptionsToArticle, (object) int32_2) == 0L)
          {
            DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.Articles, (object) int32_2);
            if ((dictionaryValue != null ? dictionaryValue.NewObjectID : 0L) == 0L)
            {
              this.plugin.appManager.AddWarningMessage($"Изделие {int32_2} не импортировано. Настройка опций конфигуратора составов для него невозможна.");
            }
            else
            {
              ArticleTag tag = dictionaryValue.Tag as ArticleTag;
              long version = tag.Versions[tag.VersionID];
              if (lastArticle != -1L && lastArticle != version)
                this.ImportVisibleOptions(iolIm, cacheData, articleID, hideOptions, lastArticle, attributeId, hidePackage);
              lastArticle = version;
              articleID = int32_2;
              hideOptions.Add(int32_1);
            }
          }
        }
        if (lastArticle == -1L)
          return;
        this.ImportVisibleOptions(iolIm, cacheData, articleID, hideOptions, lastArticle, attributeId, hidePackage);
        iolIm.Import();
      }
      finally
      {
        dataReader.Close();
      }
    }
  }

  private void PumpRestrictRules(
    IUserSession session,
    IImportingData cacheData,
    int percentStart,
    int percentEnd)
  {
    int attributeId = session.GetAttributeType(new Guid("cad015ab-306c-11d8-b4e9-00304f19f545")).AttributeID;
    string format = "Настройка несовместимости опций для изделий ({0} из {1})";
    int index1 = 0;
    int tableRecordsCount = this.GetTableRecordsCount("PC_OPTIONS_RESTRICTRULE");
    IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
    iolIm.NewObjectsOnlyInList = false;
    List<PumpOptionsToArticles.Rule> rules = new List<PumpOptionsToArticles.Rule>();
    iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index2 = 0; index2 < iolIm.Items.Count; ++index2)
      {
        if (iolIm.Items[index2].Object.Object_id != 0L && iolIm.Items[index2].Object.Object_id != -1L)
          cacheData.AddValue(ImportingCategory.ConfiguratorRestrictRules, (object) rules[index2].ArticleID, iolIm.Items[index2].Object.Object_id);
        else
          this.plugin.appManager.AddWarningMessage($"Настройки несовместимости опций для изделия {rules[index2].ArticleID} не установлены. См. серверный лог.");
      }
      rules.Clear();
    });
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "select proj_aid, val_id, order_id, opt_id, oper_id, optval_id, par_ndx from pc_options_restrictrule order by proj_aid, val_id, order_id";
      IDataReader dataReader = command.ExecuteReader();
      try
      {
        int num = -1;
        while (dataReader.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, percentStart, percentEnd));
          int int32_1 = BasePumpHelper.ToInt32(dataReader[0]);
          if (cacheData.GetNewKey(ImportingCategory.ConfiguratorRestrictRules, (object) int32_1) == 0L)
          {
            DictionaryValue dictionaryValue1 = cacheData.GetValue(ImportingCategory.Articles, (object) int32_1);
            if ((dictionaryValue1 != null ? dictionaryValue1.NewObjectID : 0L) == 0L)
            {
              this.plugin.appManager.AddWarningMessage($"Изделие {int32_1} не импортировано. Настройка несовместимости опций конфигуратора составов для него невозможна.");
            }
            else
            {
              ArticleTag tag1 = dictionaryValue1.Tag as ArticleTag;
              int int32_2 = BasePumpHelper.ToInt32(dataReader[1]);
              if (num != -1 && num != int32_1)
                this.ImportRestrictRule(iolIm, cacheData, rules[rules.Count - 1], attributeId);
              num = int32_1;
              PumpOptionsToArticles.Rule rule = (PumpOptionsToArticles.Rule) null;
              bool flag1 = false;
              for (int index3 = 0; index3 < rules.Count; ++index3)
              {
                if (rules[index3].ArticleID == int32_1)
                {
                  rule = rules[index3];
                  flag1 = true;
                  break;
                }
              }
              if (!flag1)
              {
                rule = new PumpOptionsToArticles.Rule(int32_1, tag1.Versions[tag1.VersionID]);
                rules.Add(rule);
              }
              PumpOptionsToArticles.RuleOption ruleOption = (PumpOptionsToArticles.RuleOption) null;
              bool flag2 = false;
              for (int index4 = 0; index4 < rule.Options.Count; ++index4)
              {
                if (rule.Options[index4].OptValID == int32_2)
                {
                  ruleOption = rule.Options[index4];
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
              {
                DictionaryValue dictionaryValue2 = cacheData.GetValue(ImportingCategory.ConfiguratorOptionValues, (object) int32_2);
                ArticleOptionsTag tag2 = cacheData.GetTag(ImportingCategory.ConfiguratorOptions, (object) Convert.ToInt32(dictionaryValue2.NewObjectID)) as ArticleOptionsTag;
                ruleOption = new PumpOptionsToArticles.RuleOption(int32_2, tag2.Guid, dictionaryValue2.Caption);
                rule.Options.Add(ruleOption);
              }
              if (dataReader.IsDBNull(3))
              {
                ruleOption.AddItem((PumpOptionsToArticles.TreeCondition) new PumpOptionsToArticles.ConditionCollection(this.GetLogicalFunction(BasePumpHelper.ToInt32(dataReader[4])), BasePumpHelper.ToInt32(dataReader[2]), BasePumpHelper.ToInt32(dataReader[6])));
              }
              else
              {
                PumpOptionsToArticles.RuleCondition condition = new PumpOptionsToArticles.RuleCondition();
                condition.ParentID = BasePumpHelper.ToInt32(dataReader[6]);
                condition.OrderID = BasePumpHelper.ToInt32(dataReader[2]);
                condition.Operator = this.GetOperator(BasePumpHelper.ToInt32(dataReader[4]));
                condition.OptValID = BasePumpHelper.ToInt32(dataReader[5]);
                ruleOption.AddItem((PumpOptionsToArticles.TreeCondition) condition);
              }
            }
          }
        }
        if (num == -1)
          return;
        this.ImportRestrictRule(iolIm, cacheData, rules[rules.Count - 1], attributeId);
        iolIm.Import();
      }
      finally
      {
        dataReader.Close();
      }
    }
  }

  private Operator GetOperator(int operID)
  {
    switch (operID)
    {
      case 1:
        return Operator.Equals;
      case 2:
        return Operator.NotEquals;
      case 3:
        return Operator.Less;
      case 4:
        return Operator.Greater;
      case 5:
        return Operator.LessEquals;
      case 6:
        return Operator.GreaterEquals;
      default:
        return Operator.Undefined;
    }
  }

  private LogicalFunction GetLogicalFunction(int funcID)
  {
    if (funcID == 1)
      return LogicalFunction.And;
    return funcID == 2 ? LogicalFunction.Or : LogicalFunction.Or;
  }

  private void PumpApplicabilitiesCriterions(
    IUserSession session,
    IImportingData cacheData,
    int percentStart,
    int percentEnd)
  {
    int attributeId1 = session.GetAttributeType(new Guid("cad015ac-306c-11d8-b4e9-00304f19f545")).AttributeID;
    int attributeId2 = session.GetAttributeType(new Guid("cad015a6-306c-11d8-b4e9-00304f19f545")).AttributeID;
    string format = "Настройка условия применения объектов в конфигураторе составов  ({0} из {1})";
    int index1 = 0;
    int count = 0;
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "select count(*) from pc where opt_link is not null";
      object obj1 = command.ExecuteScalar();
      if (CompareValuesHelper.NormalizedValue(obj1) != null)
        count = Convert.ToInt32(obj1);
      command.CommandText = "select count(*) from v_pc where opt_link is not null";
      object obj2 = command.ExecuteScalar();
      if (CompareValuesHelper.NormalizedValue(obj2) != null)
        count += Convert.ToInt32(obj2);
    }
    if (count == 0)
      return;
    IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
    List<string> links = new List<string>();
    importedRelationList.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index2 = 0; index2 < links.Count; ++index2)
        cacheData.AddValue(ImportingCategory.ConfiguratorApplicabilitiesCriterions, (object) links[index2], 1000L);
      links.Clear();
    });
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "select 0, prjlink_id, opt_link, proj_aid, part_aid from pc where opt_link is not null union select 1, prjlink_id, opt_link, proj_aid, part_aid from v_pc where opt_link is not null";
      IDataReader dataReader1 = command.ExecuteReader();
      try
      {
        while (dataReader1.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) count), this.CalculatePercent(count, index1, percentStart, percentEnd));
          int int32_1 = BasePumpHelper.ToInt32(dataReader1[1]);
          bool flag = BasePumpHelper.ToInt32(dataReader1[0]) == 1;
          string oldKey = $"{BasePumpHelper.ToInt32(dataReader1[0])}{int32_1}";
          if (cacheData.GetNewKey(ImportingCategory.ConfiguratorApplicabilitiesCriterions, (object) oldKey) == 0L)
          {
            long prjLinkID = flag ? cacheData.GetNewKey(ImportingCategory.VComposition, (object) int32_1) : cacheData.GetNewKey(ImportingCategory.Composition, (object) int32_1);
            if (prjLinkID == 0L)
            {
              this.plugin.appManager.AddWarningMessage($"Связь {int32_1} не импортирована. Настройка применимости объектов для нее невозможна.");
            }
            else
            {
              int int32_2 = BasePumpHelper.ToInt32(dataReader1[2]);
              List<PumpOptionsToArticles.Cond> condList = new List<PumpOptionsToArticles.Cond>();
              using (IDataReader dataReader2 = BasePumpHelper.S4Query(this.plugin.idb2.DbConnection, "select cond_id, order_id from pc_options where opt_link=@p1", CommandBehavior.Default, (object) int32_2))
              {
                while (dataReader2.Read())
                  condList.Add(new PumpOptionsToArticles.Cond(BasePumpHelper.ToInt32(dataReader2[0]), BasePumpHelper.ToInt32(dataReader2[1])));
              }
              if (condList.Count != 0)
              {
                List<PumpOptionsToArticles.RuleOption> ruleOptionList = new List<PumpOptionsToArticles.RuleOption>(condList.Count);
                object[] objArray = (object[]) null;
                List<string> stringList = (List<string>) null;
                for (int index3 = 0; index3 < condList.Count; ++index3)
                {
                  PumpOptionsToArticles.RuleOption ruleOption = new PumpOptionsToArticles.RuleOption();
                  using (IDataReader dataReader3 = BasePumpHelper.S4Query(this.plugin.idb2.DbConnection, "select order_id, opt_id, oper_id, optval_id, par_ndx from pc_options_cond where cond_id=@p1 order by order_id", CommandBehavior.Default, (object) condList[index3].CondID))
                  {
                    while (dataReader3.Read())
                    {
                      if (dataReader3.IsDBNull(1))
                      {
                        ruleOption.AddItem((PumpOptionsToArticles.TreeCondition) new PumpOptionsToArticles.ConditionCollection(this.GetLogicalFunction(BasePumpHelper.ToInt32(dataReader3[2])), BasePumpHelper.ToInt32(dataReader3[0]), BasePumpHelper.ToInt32(dataReader3[4])));
                      }
                      else
                      {
                        PumpOptionsToArticles.RuleCondition condition = new PumpOptionsToArticles.RuleCondition();
                        condition.ParentID = BasePumpHelper.ToInt32(dataReader3[4]);
                        condition.OrderID = BasePumpHelper.ToInt32(dataReader3[0]);
                        condition.Operator = this.GetOperator(BasePumpHelper.ToInt32(dataReader3[2]));
                        condition.OptValID = BasePumpHelper.ToInt32(dataReader3[3]);
                        ruleOption.AddItem((PumpOptionsToArticles.TreeCondition) condition);
                      }
                    }
                  }
                  if (ruleOption.Items.Count > 0 && (ruleOption.Items.Count != 1 || !(ruleOption.Items[0] is PumpOptionsToArticles.ConditionCollection) || ruleOption.Items[0].Items.Count != 0))
                    ruleOptionList.Add(ruleOption);
                  using (IDataReader dataReader4 = BasePumpHelper.S4Query(this.plugin.idb2.DbConnection, "select opt_id, optval_id from pc_options_init where cond_id=@p1", CommandBehavior.Default, (object) condList[index3].CondID))
                  {
                    PdmConfiguratorContext configuratorContext = new PdmConfiguratorContext((PdmConfiguratorContextsCache) null);
                    while (dataReader4.Read())
                    {
                      DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ConfiguratorOptionValues, (object) BasePumpHelper.ToInt32(dataReader4[1]));
                      ArticleOptionsTag tag = cacheData.GetTag(ImportingCategory.ConfiguratorOptions, (object) Convert.ToInt32(dictionaryValue.NewObjectID)) as ArticleOptionsTag;
                      if (!configuratorContext.OptionsValues.ContainsKey(tag.Guid))
                        configuratorContext.OptionsValues.Add(tag.Guid, dictionaryValue.Caption);
                    }
                    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeId2);
                    if (configuratorContext.OptionsValues.Count > 0)
                      stringList = StringsHelper.SplitString(configuratorContext.ToLongString(attributeId2), (int) attributeType.SizeType);
                  }
                }
                if (ruleOptionList.Count > 0)
                {
                  ObjectsApplicabilitiesCriterionsCollection criterionsCollection = new ObjectsApplicabilitiesCriterionsCollection(LogicalFunction.Or);
                  for (int index4 = 0; index4 < ruleOptionList.Count; ++index4)
                  {
                    IPdmCriterion parent = ruleOptionList.Count <= 1 || ruleOptionList[index4].Items.Count <= 1 ? (IPdmCriterion) criterionsCollection : (IPdmCriterion) new ObjectsApplicabilitiesCriterionsCollection(LogicalFunction.Or);
                    this.AddChildApplicabilities(cacheData, ruleOptionList[index4].Items, parent, parent.Function);
                  }
                  objArray = criterionsCollection.ToAttributeValues(attributeId1);
                }
                if (objArray != null || stringList != null)
                {
                  importedRelationList.UseRelation(prjLinkID);
                  if (objArray != null)
                  {
                    for (int numInList = 0; numInList < objArray.Length; ++numInList)
                      importedRelationList.AddAttribute(attributeId1, AttrValueType.stringVal, objArray[numInList], numInList);
                  }
                  if (stringList != null)
                  {
                    for (int index5 = 0; index5 < stringList.Count; ++index5)
                      importedRelationList.AddAttribute(attributeId2, AttrValueType.stringVal, (object) stringList[index5], index5);
                  }
                  links.Add(oldKey);
                }
              }
            }
          }
        }
        importedRelationList.Import();
      }
      finally
      {
        dataReader1.Close();
      }
    }
  }

  private class ImportedObject
  {
    public long NewObjectID { get; private set; }

    public int OldID { get; private set; }

    public ImportedObject(long newObjectID, int oldID)
    {
      this.NewObjectID = newObjectID;
      this.OldID = oldID;
    }
  }

  private class ArticleOptions : PumpOptionsToArticles.ImportedObject
  {
    public List<PumpOptionsToArticles.OptionInfo> Options { get; private set; }

    public ArticleOptions(long newObjectID, int oldID)
      : this(newObjectID, oldID, new List<PumpOptionsToArticles.OptionInfo>())
    {
    }

    public ArticleOptions(
      long newObjectID,
      int oldID,
      List<PumpOptionsToArticles.OptionInfo> options)
      : base(newObjectID, oldID)
    {
      this.Options = options;
    }

    public void AddOption(long newObjectID, int oldID, string caption)
    {
      this.Options.Add(new PumpOptionsToArticles.OptionInfo(newObjectID, oldID, caption));
    }
  }

  private class OptionInfo : PumpOptionsToArticles.ImportedObject
  {
    public string Caption { get; private set; }

    public OptionInfo(long newObjectID, int oldID, string caption)
      : base(newObjectID, oldID)
    {
      this.Caption = caption;
    }
  }

  private class Rule
  {
    public int ArticleID;
    public long NewArticleID;
    public List<PumpOptionsToArticles.RuleOption> Options;

    public Rule(int articleID, long newArticleID)
    {
      this.ArticleID = articleID;
      this.NewArticleID = newArticleID;
      this.Options = new List<PumpOptionsToArticles.RuleOption>();
    }
  }

  private class RuleOption
  {
    public Guid OptionGuid;
    public string OpValIndex;
    public int OptValID;
    public List<PumpOptionsToArticles.TreeCondition> Items;
    private Dictionary<int, PumpOptionsToArticles.TreeCondition> _indexes;

    public RuleOption()
    {
      this.Items = new List<PumpOptionsToArticles.TreeCondition>();
      this._indexes = new Dictionary<int, PumpOptionsToArticles.TreeCondition>();
    }

    public RuleOption(int optvalID, Guid optionGuid, string optvalIndex)
      : this()
    {
      this.OptValID = optvalID;
      this.OptionGuid = optionGuid;
      this.OpValIndex = optvalIndex;
    }

    public void AddItem(PumpOptionsToArticles.TreeCondition condition)
    {
      if (condition.ParentID == -1)
        this.Items.Add(condition);
      else if (this._indexes.ContainsKey(condition.ParentID))
        this._indexes[condition.ParentID].Items.Add(condition);
      this._indexes.Add(condition.OrderID, condition);
    }
  }

  private class TreeCondition
  {
    public int ParentID;
    public int OrderID;
    public List<PumpOptionsToArticles.TreeCondition> Items;

    public TreeCondition() => this.Items = new List<PumpOptionsToArticles.TreeCondition>();

    public TreeCondition(int orderID, int parentID)
      : this()
    {
      this.OrderID = orderID;
      this.ParentID = parentID;
    }
  }

  private class ConditionCollection : PumpOptionsToArticles.TreeCondition
  {
    public LogicalFunction LogicalFunction;

    public ConditionCollection(LogicalFunction funct, int orderID, int parentID)
      : base(orderID, parentID)
    {
      this.LogicalFunction = funct;
    }
  }

  private class RuleCondition : PumpOptionsToArticles.TreeCondition
  {
    public Operator Operator;
    public int OptValID;
  }

  private class Cond
  {
    public int CondID;
    public int OrderID;

    public Cond(int condID, int orderID)
    {
      this.CondID = condID;
      this.OrderID = orderID;
    }
  }
}
