// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpArticlesClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.Archives.Common;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки изделий", "Перекачка изделий")]
public class PumpArticlesClass : PumpClass
{
  protected SearchDataPlugin plugin;
  private readonly int _docListTypeForPL;
  protected CacheCategory artTypes;
  protected CacheCategory artParams;
  protected CacheCategory themeParams;
  protected ArticlesCache articlesCache;
  protected CacheCategory documentsCache;
  protected CacheCategory documentationCache;
  protected CacheCategory objectGuids;
  protected CacheCategory docLinks;
  protected CacheCategory statusesToLevels;
  protected CacheCategory enabledPLList;
  protected CacheCategory commonParameters;
  protected IImportingData _cacheData;
  private List<int> _projectTypes4CadModels;
  private List<int> _partTypes4CadModels;
  private int _specificationTypeID;
  private ProductionListsCache _productionListsCache;
  private IMeasures _measures;
  private CommonParamsReader _versionsCommonParamsReader;
  private ArticleTParamsService _articleParamsService;
  protected PumpArticlesMode _pumpMode;
  private Dictionary<int, int> _skipArticles = new Dictionary<int, int>();
  private IImportedObjectList _iol;
  private readonly List<string> _sysFields = new List<string>((IEnumerable<string>) new string[1]
  {
    "art_id"
  });
  private List<ArtVerInfo> _versionsToCache = new List<ArtVerInfo>();
  private List<Tuple<int, int, int>> _inImportedList = new List<Tuple<int, int, int>>();
  protected string _cantPumpArticleErr = "Изделие (ART_ID={0}, VER_ID={1}) не может быть закачано.";

  protected override Guid GUID => new Guid("{1B9E0251-C1E9-40db-BC42-9A0A1660064C}");

  public PumpArticlesClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
    this._docListTypeForPL = MetaDataHelper.GetRelationTypeID(new ProductionListIDHelper().RelationTypes.F5Type.RelationType);
  }

  public override void Exam()
  {
    FileStores.MainDBConnection = this.plugin.idb.DbConnection;
    using (IDbCommand command = this.plugin.idb2.CreateCommand())
    {
      this.ExamCheckPoint("Патч привязки разных версий сборочных единиц к одной версии спецификации", 1);
      command.CommandText = "UPDATE V_ARTICLES SET DOC_ID = -2, DOC_VER_ID = 0 WHERE DOC_ID > 0 AND EXISTS (SELECT V.VART_ID FROM V_ARTICLES V WHERE V.ART_ID = V_ARTICLES.ART_ID AND V.DOC_ID = V_ARTICLES.DOC_ID AND V.DOC_VER_ID = V_ARTICLES.DOC_VER_ID AND V.VART_ID > V_ARTICLES.VART_ID)";
      command.ExecuteNonQuery();
      this.ExamCheckPoint("Проверка целостности данных таблицы ARTICLES ", 10);
      command.CommandText = "SELECT ART_ID , SECTION_ID FROM ARTICLES where SECTION_ID NOT IN (SELECT SECTION_ID FROM SSECTIONS)";
      IDataReader dataReader1 = command.ExecuteReader(CommandBehavior.SequentialAccess);
      try
      {
        while (dataReader1.Read())
          this.plugin._settingsControl.AddInvalidObject(new InvalidObject("Изделие", BasePumpHelper.ToInt32(dataReader1[0]), -1, BasePumpHelper.ToInt32(dataReader1[1])));
      }
      finally
      {
        dataReader1.Close();
      }
      this.ExamCheckPoint("Проверка целостности данных таблицы V_ARTICLES ", 50);
      command.CommandText = "SELECT ART_ID, SECTION_ID FROM V_ARTICLES where SECTION_ID NOT IN (SELECT SECTION_ID FROM SSECTIONS)";
      IDataReader dataReader2 = command.ExecuteReader(CommandBehavior.SequentialAccess);
      try
      {
        while (dataReader2.Read())
          this.plugin._settingsControl.AddInvalidObject(new InvalidObject("Версия изделия", BasePumpHelper.ToInt32(dataReader2[0]), -1, BasePumpHelper.ToInt32(dataReader2[1])));
      }
      finally
      {
        dataReader2.Close();
      }
      this.ExamCheckPoint("Поиск изделий с одинаковыми обозначениями ", 80 /*0x50*/);
      command.CommandText = "select count(*) from articles a, articles a2 where a.art_id>0 and a2.art_id>0 and a.art_id <> a2.art_id and a.designatio <> '' and a.designatio = a2.designatio";
      IDataReader dataReader3 = command.ExecuteReader();
      try
      {
        if (dataReader3.Read())
        {
          int num = BasePumpHelper.ToInt32(dataReader3[0]) / 2;
          if (num > 0)
          {
            if (MessageBox.Show($"В базе Search найдено {num} изделий, для которые имеются другие изделия, совпадающие по обозначению. Рекомендуется до начала перекачки выполнить процедуру Search \"Сервис - Утилиты администратора - Поиск одинаковых объектов\" и устранить дубликаты. Продолжить перекачку?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) != DialogResult.Yes)
              Application.Exit();
          }
        }
      }
      finally
      {
        dataReader3.Close();
      }
      this.ExamCheckPoint("Поиск дубликатов версий изделий", 90);
      command.CommandText = "select va.art_id, va.art_ver_id, count(*) from V_ARTICLES va group by va.art_id, va.art_ver_id having count(*) > 1";
      IDataReader dataReader4 = command.ExecuteReader();
      try
      {
        int num = 0;
        while (dataReader4.Read())
        {
          ++num;
          this.plugin.appManager.AddErrorMessage($"Найден дубликат версии изделия ART_ID={BasePumpHelper.ToInt32(dataReader4[0])} ART_VER_ID={BasePumpHelper.ToInt32(dataReader4[1])}");
        }
        if (num > 0)
        {
          if (MessageBox.Show($"В базе Search найдено {num} дубликатов версий изделий. Рекомендуется до начала перекачки устранить дубликаты. Продолжить перекачку?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) != DialogResult.Yes)
            Application.Exit();
        }
      }
      finally
      {
        dataReader4.Close();
      }
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  protected virtual void BeforePump()
  {
    this.artTypes = PumpCache.Category[ImportingCategory.ArticleTypes];
    this.artParams = PumpCache.Category[ImportingCategory.ArticleAttributes];
    this.themeParams = PumpCache.Category[ImportingCategory.ThematicParams];
    this.articlesCache = new ArticlesCache(PumpCache.Category[ImportingCategory.Articles]);
    this.documentsCache = PumpCache.Category[ImportingCategory.Documents];
    this.documentationCache = PumpCache.Category[ImportingCategory.Documentation];
    this.objectGuids = PumpCache.Category[ImportingCategory.ObjectGUIDs];
    this.docLinks = PumpCache.Category[ImportingCategory.DocLinks];
    this.statusesToLevels = PumpCache.Category[ImportingCategory.StatusesToLevels];
    this.enabledPLList = PumpCache.Category[ImportingCategory.EnabledProductionLists];
    this.commonParameters = PumpCache.Category[ImportingCategory.ArticleCommonParameters];
    this._versionsCommonParamsReader = new CommonParamsReader(this.commonParameters, "VART_PARAMS", "VART_ID");
    this._measures = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    PumpHelper.SPDocTypes.Contains(0);
    this._cacheData = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.ImbaseTableLinksKeyToObjectID);
    this._projectTypes4CadModels = this.plugin.Imdi.ObjectTypes.GetChildTypesRecursive(this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00250-306c-11d8-b4e9-00304f19f545")).ID, this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad0038d-306c-11d8-b4e9-00304f19f545")).ID, this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00252-306c-11d8-b4e9-00304f19f545")).ID);
    this._partTypes4CadModels = this.plugin.Imdi.ObjectTypes.GetChildTypesRecursive(this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00900-306c-11d8-b4e9-00304f19f545")).ID, this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cadd94d6-306c-11d8-b4e9-00304f19f545")).ID);
    this._specificationTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00133-306c-11d8-b4e9-00304f19f545")).ID;
    this._productionListsCache = new ProductionListsCache();
  }

  protected virtual void AfterPump()
  {
    this.docLinks.Release();
    this.objectGuids.Release();
    this.artTypes.Release();
    this.artParams.Release();
    this.themeParams.Release();
    this.articlesCache.Release();
    this.documentsCache.Release();
    this.documentationCache.Release();
    this.statusesToLevels.Release();
    this.enabledPLList.Release();
    this.commonParameters.Release();
    this._productionListsCache.Close();
    (ServicesManager.GetService(typeof (ICache)) as ICache).ReleaseCache(ImportingCategory.ImbaseTableLinksKeyToObjectID);
  }

  protected virtual string WhereInit => "where a.art_id > 0";

  protected virtual Dictionary<int, string> WhereSections
  {
    get
    {
      return new Dictionary<int, string>()
      {
        {
          7,
          " and a.section_id=7"
        },
        {
          0,
          $" and a.section_id<>7 and a.section_id<>{99999990}"
        },
        {
          99999990,
          $" and a.section_id= {99999990}"
        }
      };
    }
  }

  private List<List<T>> SplitList<T>(List<T> source)
  {
    return source.Select((x, i) => new
    {
      Index = i,
      Value = x
    }).GroupBy(x => x.Index / this.plugin.idb2.MaxInOperator).Select<IGrouping<int, \u003C\u003Ef__AnonymousType0<int, T>>, List<T>>(x => x.Select(v => v.Value).ToList<T>()).ToList<List<T>>();
  }

  private string QueryINBuilder(List<int> articleIDs)
  {
    StringBuilder stringBuilder = new StringBuilder();
    List<List<int>> intListList = this.SplitList<int>(articleIDs);
    if (intListList.Count > 1)
      stringBuilder.Append("(");
    bool flag = true;
    foreach (List<int> values in intListList)
    {
      if (flag)
        flag = false;
      else
        stringBuilder.Append(" OR ");
      stringBuilder.Append("a.art_id IN(");
      stringBuilder.Append(string.Join<int>(",", (IEnumerable<int>) values));
      stringBuilder.Append(")");
    }
    if (intListList.Count > 1)
      stringBuilder.Append(")");
    return stringBuilder.ToString();
  }

  private string AdditionalSectionWhere(int sectionID)
  {
    if (sectionID != 99999990)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    Dictionary<object, DictionaryValue> items = this.enabledPLList.Items;
    if (items == null || items.Count == 0)
    {
      stringBuilder.Append(" and 1=0");
    }
    else
    {
      stringBuilder.Append(" AND ");
      stringBuilder.Append(this.QueryINBuilder(items.Keys.ToList<object>().ConvertAll<int>((Converter<object, int>) (_ => Convert.ToInt32(_)))));
    }
    return stringBuilder.ToString();
  }

  public override void Pump()
  {
    this.BeforePump();
    SimpleLogger logger = BasePumpHelper.Logger;
    try
    {
      this.PumpCheckPoint("Определение изделий, совпадающих по обозначениям без учета суффикса", 0);
      this.FillArticlesToSkip();
      if (PluginSettings.OptimizeReadTParams)
      {
        this.PumpCheckPoint("Чтение тематических параметров для изделий", 0);
        this._articleParamsService = new ArticleTParamsService(this.plugin.idb3.DbConnection, BasePumpHelper.Logger, this.articlesCache.Cache, this.themeParams);
        this._articleParamsService.Read();
      }
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества изделий для перекачки", 1);
        string whereInit = this.WhereInit;
        command.CommandText = "select count(*) from articles a " + whereInit;
        int int32 = Convert.ToInt32(command.ExecuteScalar());
        this.SetCountPumpRecords(int32);
        for (this._pumpMode = PumpArticlesMode.Articles; this._pumpMode <= PumpArticlesMode.LinkSuffixed; ++this._pumpMode)
        {
          int num1 = 1;
          int num2 = 0;
          if (this._pumpMode == PumpArticlesMode.LinkSuffixed)
            num2 = num1;
          logger.Write($"{command.CommandText}: {int32} result(s)");
          foreach (KeyValuePair<int, string> whereSection in this.WhereSections)
          {
            string str1 = whereInit + whereSection.Value;
            this.Iol.Items.Clear();
            string str2 = ", (select 1 from dbversion where exists (select art_id from articles a2 where a2.art_id <> a.art_id and a2.doc_id = a.doc_id)) as hasGrouping ";
            string str3 = str1 + " and a.doc_id=d.doc_id " + this.AdditionalSectionWhere(whereSection.Key);
            string str4 = "select a.*, d.doc_type, d.archive_id, (select 1 from dbversion where a.section_id = 4 and (exists (select * from pc where pc.part_aid = a.art_id and upper(pc.format) = 'БЧ')";
            if (PumpHelper.IsVariantsExists)
              str4 += " or exists (select * from variants v where v.part_aid = a.art_id and upper(v.format) = 'БЧ') or exists (select * from boundpos v where v.part_aid = a.art_id and upper(v.format) = 'БЧ')";
            string str5 = $"{str4})) as bch {str2} from articles a, doclist d {str3} order by a.doc_id,a.art_id";
            command.CommandText = str5;
            IDataReader mainReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
            try
            {
              string format = "Перекачка изделий ({0} из {1})";
              Article article = new Article();
              DataReadResult dataReadResult;
              while ((dataReadResult = this.ReadArticle(mainReader, article)) != DataReadResult.NoData)
              {
                this.PumpCheckPoint(string.Format(format, (object) num1, (object) int32), this.CalculatePercent(int32 * 2, num1 + num2, 2, 99));
                logger.Flush();
                if (dataReadResult == DataReadResult.OK)
                  this.PumpArticle(article);
                ++num1;
              }
              this.CheckDataPacket(true);
            }
            finally
            {
              mainReader.Close();
              BlobHelper.Clear();
            }
          }
        }
        this.PumpCheckPoint("Перекачка изделий успешно завершена", 100);
        logger.Write("=========Pump end\r\n\r\n");
      }
    }
    catch (Exception ex)
    {
      logger.Write($"=========Pump abort ({ex.Message}\r\n{ex.StackTrace})\r\n\r\n");
      throw;
    }
    finally
    {
      this.AfterPump();
    }
  }

  private void FillArticlesToSkip()
  {
    string str1 = "";
    this._skipArticles.Clear();
    if (PluginSettings.ArtSuffixesToDelete != null)
    {
      foreach (string str2 in PluginSettings.ArtSuffixesToDelete)
      {
        if (str1 != "")
          str1 += " or ";
        str1 += $"(a.designatio = a2.designatio {PumpHelper.ConcatSign} ' {str2}')";
      }
    }
    if (!(str1 != ""))
      return;
    string str3 = string.Format("and (not a.section_id in ({0},{1}) and not a2.section_id in ({0},{1})) ", (object) 1, (object) PumpHelper.BuildingDocumentationSectID);
    using (IDataReader dataReader = BasePumpHelper.S4Query($"select a.art_id as dup_id, a2.art_id as art_id, a.designatio, a2.designatio from articles a, articles a2 where a.art_id >0 and a2.art_id>0 and a.art_id <> a2.art_id and a.designatio <> '' and ({str1}) {str3}order by 1,2"))
    {
      while (dataReader.Read())
      {
        int int32 = BasePumpHelper.ToInt32(dataReader[0]);
        if (!this._skipArticles.ContainsKey(int32))
          this._skipArticles.Add(int32, BasePumpHelper.ToInt32(dataReader[1]));
      }
    }
  }

  protected IImportedObjectList Iol
  {
    get
    {
      if (this._iol == null)
      {
        this._iol = this.plugin.Idw.CreateImportedObjectList(0);
        this._iol.NewObjectsOnlyInList = false;
      }
      return this._iol;
    }
  }

  protected void CheckDataPacket(bool ForcePump)
  {
    if (!ForcePump && this._inImportedList.Count < BasePumpHelper.PacketSize)
      return;
    this.Iol.Import();
    ArticleTag atag = (ArticleTag) null;
    long newArtID = 0;
    int num1 = 0;
    int sectID = 0;
    for (int index1 = 0; index1 < this._versionsToCache.Count; ++index1)
    {
      ImportingObject importingObject = (ImportingObject) null;
      ArtVerInfo ai = this._versionsToCache[index1];
      int artID = ai.ID != 0 ? ai.ID : num1;
      Tuple<int, int, int> tuple = this._inImportedList.Find((Predicate<Tuple<int, int, int>>) (x => x.Item1 == artID && x.Item2 == ai.VerID));
      int index2 = tuple != null ? tuple.Item3 : -1;
      if (ai.ID != 0 && atag != null)
      {
        this.articlesCache.Add(num1, newArtID, atag, ai.SectID);
        atag = (ArticleTag) null;
      }
      if (ai.NewArtObjectID == 0L)
      {
        if (index2 >= 0)
          importingObject = this.Iol.Items[index2];
        if (importingObject == null || importingObject.Object.Object_id == 0L)
        {
          Exception importError = this.Iol.GetImportError(index2);
          BasePumpHelper.AppManager.AddWarningMessage($"Ошибка перекачки изделия (ART_ID={artID}, VER_ID={ai.VerID}, Ext=[{ai.ExtInfo}]): " + (importError != null ? importError.Message : ""));
          continue;
        }
        this._productionListsCache.AddArticle(importingObject, artID, ai.VerID);
      }
      if (ai.ID != 0 && atag == null)
      {
        atag = new ArticleTag()
        {
          ID = ai.ID,
          VersionID = ai.ActualVerID,
          Flags = ai.Flags
        };
        newArtID = importingObject == null || importingObject.Object.Id == 0L ? PumpHelper.Plugin.Imdi.ImportedObjects.GetID(ai.NewArtObjectID) : importingObject.Object.Id;
        num1 = ai.ID;
        sectID = ai.SectID;
      }
      if (atag != null)
      {
        if (importingObject != null)
          ai.NewArtObjectID = importingObject.Object.Object_id;
        atag.Versions.Add(ai.VerID, ai.NewArtObjectID);
        if (ai.IsDocumentation && this._pumpMode == PumpArticlesMode.Articles)
        {
          long newKey = ai.NewArtObjectID;
          if (ai.VerID == -1)
            newKey = newArtID;
          this.documentationCache.AddValue((object) BasePumpHelper.MakeCacheKey(num1, ai.VerID), newKey);
        }
        if (ai.SectID == 7 && importingObject != null)
          PumpHelper.MetadataInfo.Materials.AddToCache(ai.Name, ai.ImbaseKey, importingObject.Object.ObjectType, ai.NewArtObjectID);
      }
    }
    if (atag != null)
      this.articlesCache.Add(num1, newArtID, atag, sectID);
    this.Iol.Items.Clear();
    IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
    importedRelationList.ImportingRelationCreator = new Intermech.ImpExp.Interface.DataWriter.ImportingRelationCreator(this.ImportingRelationCreator);
    importedRelationList.AfterImportEvent += new AfterImportEventDelegate(this.Writer_AfterImportEvent);
    foreach (ArtVerInfo artVerInfo in this._versionsToCache)
    {
      long newArtObjectId = artVerInfo.NewArtObjectID;
      if (artVerInfo.DocID != -2)
      {
        DictionaryValue dictionaryValue = this.documentsCache.GetValue((object) artVerInfo.DocID);
        if (dictionaryValue == null)
        {
          BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={artVerInfo.DocID}) не закачан, невозможно привязать как документацию на объект!");
          continue;
        }
        long newObjectId = dictionaryValue.NewObjectID;
        DocumentTag tag = dictionaryValue.Tag as DocumentTag;
        if (!tag.HasFlag(DocumentFlag.Techcard))
        {
          long objectID = 0;
          if (artVerInfo.DocVerID != -1)
            tag.Versions.TryGetValue(artVerInfo.DocVerID, out objectID);
          importedRelationList.AddRelationFromID(newArtObjectId, newObjectId, PumpHelper.RelTypeDocumentationID);
          if (objectID > 0L)
            importedRelationList.AddAttributeInt(PumpHelper.AttrVerLinkID, objectID);
          int num2 = objectID > 0L ? this.plugin.Imdi.ImportedObjects.GetObjectTypeID(objectID) : this.plugin.Imdi.ImportedObjects.GetObjectTypeIDForID(newObjectId);
          if (num2 != this._specificationTypeID)
          {
            if (this._projectTypes4CadModels.Contains(this.plugin.Imdi.ImportedObjects.GetObjectTypeID(newArtObjectId)) && this._partTypes4CadModels.Contains(num2))
            {
              string articleDesignation = PumpHelper.GetArticleDesignation(artVerInfo.ID, artVerInfo.VerID);
              string documentDesignation = PumpHelper.GetDocumentDesignation(artVerInfo.DocID);
              importedRelationList.AddAttributeInt(PumpHelper.AttrBasedOnCadModelID, articleDesignation.Equals(documentDesignation) ? 1L : 0L);
            }
            else
              importedRelationList.AddAttributeInt(PumpHelper.AttrBasedOnCadModelID, 1L);
          }
          if (importedRelationList.Items[importedRelationList.Items.CurrentIndex] is ImportingRelationEx importingRelationEx)
            importingRelationEx.OldKey = (object) BasePumpHelper.MakeCacheKey(newArtObjectId, newObjectId);
        }
        else
          continue;
      }
      DocsLinks docsLinks = artVerInfo.DocsLinks;
      if (PumpHelper.IsNewDocsLinksFormat && docsLinks == null && artVerInfo.MainVI != null)
        docsLinks = artVerInfo.MainVI.DocsLinks;
      if (docsLinks != null)
      {
        int objectTypeId = this.plugin.Imdi.ImportedObjects.GetObjectTypeID(newArtObjectId);
        foreach (DocLink docLink in (List<DocLink>) docsLinks)
        {
          if ((!(docLink is DocLinkEx docLinkEx) || artVerInfo.VerID >= docLinkEx.ArtVerID && (docLinkEx.DelVerID == -1 || artVerInfo.VerID < docLinkEx.DelVerID)) && artVerInfo.DocID != docLink.DocID)
          {
            int objectTypeID;
            bool techcardDocument;
            long documentId = DocLinksHelper.GetDocumentID(this.documentsCache.Items, this.objectGuids.Items, docLink.DocID, out long _, out objectTypeID, out techcardDocument);
            if (documentId == 0L)
              BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={docLink.DocID}) не закачан, невозможно привязать как документацию на объект [v]!");
            else if (!techcardDocument && !PumpHelper.IsTechProcess(objectTypeID))
            {
              int relationTypeForDocLink = this.GetRelationTypeForDocLink(objectTypeId, artVerInfo.Flags);
              importedRelationList.AddRelationFromID(newArtObjectId, documentId, relationTypeForDocLink);
              if (importedRelationList.Items[importedRelationList.Items.CurrentIndex] is ImportingRelationEx importingRelationEx)
                importingRelationEx.OldKey = (object) BasePumpHelper.MakeCacheKey(newArtObjectId, documentId);
            }
          }
        }
      }
    }
    importedRelationList.Import();
    this._versionsToCache.Clear();
    this._inImportedList.Clear();
    BlobHelper.Reset();
  }

  private int GetRelationTypeForDocLink(int artObjectType, ArticleFlag articleFlag)
  {
    if (artObjectType == PumpHelper.ObjTypeProductionListsID)
      return this._docListTypeForPL;
    return articleFlag != ArticleFlag.Documentation ? PumpHelper.RelTypeDocumentationID : PumpHelper.RelTypeDocRefID;
  }

  public ImportingRelation ImportingRelationCreator(RelationRecord rec)
  {
    return (ImportingRelation) new ImportingRelationEx(rec);
  }

  private void Writer_AfterImportEvent(object sender, EventArgs e)
  {
    IImportedRelationList importedRelationList = (IImportedRelationList) sender;
    for (int index = 0; index < importedRelationList.Items.Count; ++index)
    {
      ImportingRelationEx importingRelationEx = importedRelationList.Items[index] as ImportingRelationEx;
      if (this.docLinks.GetNewKey(importingRelationEx.OldKey) == 0L)
        this.docLinks.AddValue(importingRelationEx.OldKey, 1L);
    }
  }

  private void CheckDataPacket() => this.CheckDataPacket(false);

  protected DataReadResult ReadArticle(IDataReader mainReader, Article art)
  {
    art.Clear();
    bool flag = mainReader.Read();
    if (flag)
    {
      BasePumpHelper.ReaderRowToS4Table(mainReader, art.Data, true);
      if (!PumpHelper.IsNewPCFormat)
        art.HackArtVerID(-1);
      if (this._pumpMode == PumpArticlesMode.Articles && this._skipArticles.ContainsKey(art.ID) || this._pumpMode == PumpArticlesMode.LinkSuffixed && !this._skipArticles.ContainsKey(art.ID) || this.articlesCache.CheckID(art.ID, art.SectID) > 0L)
        return DataReadResult.Skipped;
      if (PluginSettings.PumpArtVersions)
      {
        string str1 = ",(select 1 from dbversion where a.section_id = 4 and (exists (select * from v_pc where v_pc.part_aid = a.vart_id and upper(v_pc.format) = 'БЧ') ";
        if (PumpHelper.IsVariantsExists)
          str1 += "or exists (select * from v_variants v where v.part_aid = a.vart_id and upper(v.format) = 'БЧ') or exists (select * from v_boundpos v where v.part_aid = a.vart_id and upper(v.format) = 'БЧ')";
        string str2 = str1 + ")) as bch ";
        string cmdtext = $"select a.*, r.doc_type, r.archive_id {str2} from V_ARTICLES a left join RC r on (a.doc_id = r.doc_id and a.doc_ver_id = r.version_id) where a.ART_ID = @p1 order by a.PREV_ART_VER_ID, a.ART_VER_ID";
        if (BasePumpHelper.dbType == BasePumpHelper.DBType.Oracle)
          cmdtext = $"select a.*, r.doc_type, r.archive_id {str2} from V_ARTICLES a, RC r where a.ART_ID = @p1 and  r.doc_id(+)=a.doc_id and r.version_id(+)=a.doc_ver_id order by a.PREV_ART_VER_ID, a.ART_VER_ID";
        using (IDataReader reader = BasePumpHelper.S4Query(cmdtext, (object) art.ID))
        {
          while (reader.Read())
          {
            int int32 = Convert.ToInt32(reader["ART_VER_ID"]);
            if (!PluginSettings.PumpSysArtVersions)
            {
              object obj = reader["AUTHOR"];
              if (!DBNull.Value.Equals(obj) && Convert.ToInt32(obj) == -2)
                continue;
            }
            Article article = new Article();
            BasePumpHelper.ReaderRowToS4Table(reader, article.Data, false);
            if (!art.Versions.ContainsKey(int32))
            {
              art.Versions.Add(int32, article);
              if (DBNull.Value.Equals(article.Data["doc_type"]))
                article.Data["doc_type"] = (object) -2;
              if (DBNull.Value.Equals(article.Data["archive_id"]))
                article.Data["archive_id"] = (object) 0;
            }
          }
        }
      }
      List<Article> plainList = art.PlainList;
      string fileExt = (string) null;
      foreach (Article article in plainList)
      {
        if (article.DocID != -2)
        {
          if (article.SectID == 1 && PumpHelper.SPDocTypes.Contains(article.DocType))
            article.SBFromSP = true;
          else if (article.SectID == 1 || article.SectID == PumpHelper.BuildingDocumentationSectID)
          {
            DictionaryValue dictionaryValue = this.documentsCache.GetValue((object) article.DocID);
            if (dictionaryValue != null)
            {
              long newObjectId = dictionaryValue.NewObjectID;
              DocumentTag tag = dictionaryValue.Tag as DocumentTag;
              if (tag.HasFlag(DocumentFlag.Techcard))
                return DataReadResult.Skipped;
              object obj = (object) -1;
              if (article.ArtVerID != -1 && article.Data.TryGetValue("doc_ver_id", out obj))
              {
                if (tag.Versions.TryGetValue(Convert.ToInt32(obj), out newObjectId))
                  article.ExistedDocObjectID = newObjectId;
                else
                  article.HackDocID(-2);
              }
              else
                article.ExistedDocObjectID = tag.Versions.ContainsKey(tag.VersionID) ? tag.Versions[tag.VersionID] : tag.Versions.First<KeyValuePair<int, long>>().Value;
            }
          }
        }
        else if (article.SectID == 1)
        {
          if (art.DocID != -2 && fileExt == null)
          {
            using (IDataReader dataReader = BasePumpHelper.S4Query("select filename from rc where doc_id=@p1", (object) art.DocID))
            {
              if (dataReader.Read())
              {
                object obj = dataReader["filename"];
                fileExt = !DBNull.Value.Equals(obj) ? Path.GetExtension(Convert.ToString(obj)) : string.Empty;
              }
            }
          }
          article.DocTypeToCreate = DTSuffixesHelper.FindDocTypeBySuffix(art.Designation, fileExt);
        }
      }
      if (this._pumpMode == PumpArticlesMode.LinkSuffixed)
      {
        int num;
        if (this._skipArticles.TryGetValue(art.ID, out num))
        {
          foreach (Article article in plainList)
            article.ExistedArtID = num;
        }
      }
      else
      {
        using (IDataReader reader = BasePumpHelper.S4Query($"select * from {art.SectTableName} where ART_ID = @p1", (object) art.ID))
        {
          if (reader.Read())
            BasePumpHelper.ReaderRowToS4Table(reader, art.AddData, this._sysFields, true);
        }
        if (PluginSettings.OptimizeReadTParams)
        {
          List<TParamValue> tparamValueList = this._articleParamsService.GetParams(art.ID);
          if (tparamValueList != null && tparamValueList.Count > 0)
          {
            foreach (TParamValue tparamValue in tparamValueList)
              art.ThemeData.Add(tparamValue.ParameterID.ToString(), tparamValue.Value);
          }
          this._articleParamsService.ClearValues(art.ID);
        }
        else
        {
          HashSet<int> intSet = new HashSet<int>();
          using (IDataReader dataReader = BasePumpHelper.S4Query("select PARAM_ID from PARAM4ART where ART_ID = @p1", (object) art.ID))
          {
            while (dataReader.Read())
              intSet.Add(Convert.ToInt32(dataReader[0]));
          }
          foreach (int oldKey in intSet)
          {
            DictionaryValue dictionaryValue = this.themeParams.GetValue((object) oldKey);
            if (dictionaryValue != null)
            {
              string str = dictionaryValue.Caption.Split(',')[0];
              if (str != "")
              {
                using (IDataReader dataReader = BasePumpHelper.S4Query($"select P_VALUE from {str} where ART_ID = @p1", (object) art.ID))
                {
                  if (dataReader.Read())
                    art.ThemeData.Add(oldKey.ToString(), dataReader[0]);
                }
              }
            }
          }
        }
      }
      art.DocsLinks = DocLinksHelper.GetDocLinksForArticle(art.ID);
      string cmdtext1 = "select TODOC_ID, USER_ID from DOCSLINKS where ART_ID = @p1";
      if (PumpHelper.IsNewDocsLinksFormat)
        cmdtext1 = "select TODOC_ID, USER_ID, DOC_VER_ID, ART_VER_ID, DEL_VER_ID from DOCSLINKS where ART_ID = @p1";
      using (IDataReader dataReader = BasePumpHelper.S4Query(cmdtext1, (object) art.ID))
      {
        DocsLinks docsLinks = (DocsLinks) null;
        if (dataReader.Read())
        {
          docsLinks = new DocsLinks();
          do
          {
            int userID = dataReader.IsDBNull(1) ? -1 : BasePumpHelper.ToInt32(dataReader[1]);
            DocLink docLink;
            if (PumpHelper.IsNewDocsLinksFormat)
            {
              int num = dataReader.IsDBNull(2) ? -1 : BasePumpHelper.ToInt32(dataReader[2]);
              int artVerID = dataReader.IsDBNull(3) ? -1 : BasePumpHelper.ToInt32(dataReader[3]);
              int delVerID = dataReader.IsDBNull(4) ? -1 : BasePumpHelper.ToInt32(dataReader[4]);
              DocLinkEx docLinkEx = new DocLinkEx(BasePumpHelper.ToInt32(dataReader[0]), userID, artVerID, delVerID);
              docLinkEx.VerID = num;
              docLink = (DocLink) docLinkEx;
            }
            else
              docLink = new DocLink(BasePumpHelper.ToInt32(dataReader[0]), userID);
            docsLinks.Add(docLink);
          }
          while (dataReader.Read());
        }
        art.DocsLinks = docsLinks;
      }
      foreach (KeyValuePair<int, Article> version in art.Versions)
      {
        Article article = version.Value;
        article.ThemeData = art.ThemeData;
        int int32 = Convert.ToInt32(article.Data["vart_id"]);
        using (IDataReader reader = BasePumpHelper.S4Query($"select * from VSECT_{(object) article.SectID} where VART_ID = @p1", (object) int32))
        {
          try
          {
            if (reader.Read())
              BasePumpHelper.ReaderRowToS4Table(reader, article.AddData, this._sysFields, true);
          }
          finally
          {
            reader.Close();
          }
        }
        if (!PumpHelper.IsNewDocsLinksFormat)
        {
          using (IDataReader dataReader = BasePumpHelper.S4Query("select a.DOC_ID, v.USER_ID, a.DOC_VER_ID from V_ARTICLES a, VDOCSLINKS v where v.ART_ID = @p1 and a.VART_ID = v.TODOC_ID and a.DOC_ID <> -2", (object) int32))
          {
            try
            {
              DocsLinks docsLinks = (DocsLinks) null;
              if (dataReader.Read())
              {
                docsLinks = new DocsLinks();
                do
                {
                  int userID = dataReader.IsDBNull(1) ? -1 : BasePumpHelper.ToInt32(dataReader[1]);
                  DocLink docLink = new DocLink(BasePumpHelper.ToInt32(dataReader[0]), userID)
                  {
                    VerID = BasePumpHelper.ToInt32(dataReader[2])
                  };
                  docsLinks.Add(docLink);
                }
                while (dataReader.Read());
              }
              article.DocsLinks = docsLinks;
            }
            finally
            {
              dataReader.Close();
            }
          }
        }
        this._versionsCommonParamsReader.Read(article.CommonParamsData, int32);
      }
    }
    return flag ? DataReadResult.OK : DataReadResult.NoData;
  }

  protected virtual string DetermineObjType(
    Article art,
    ref int objTypeID,
    bool docObjExists,
    ref ArtClass artClass,
    ref bool isBCH)
  {
    if (art.SBFromSP)
      objTypeID = PumpHelper.ObjTypeAssemblyUnitID;
    else if (art.SectID == 99999990)
      objTypeID = PumpHelper.ObjTypeProductionListsID;
    else if (art.DocTypeToCreate == -1 && !docObjExists)
    {
      int sectId = art.SectID;
      object obj = art.Data["bch"];
      if (art.DocID == -2 && sectId == 4 && !DBNull.Value.Equals(obj) && Convert.ToInt32(obj) == 1)
      {
        objTypeID = PumpHelper.ObjTypePartWithoutDrawingID;
        isBCH = true;
      }
      else
        objTypeID = Convert.ToInt32(this.artTypes.GetNewKey((object) sectId));
      if (objTypeID <= 0)
        return $"Тип изделия, соответствующий типу ({art.SectID}), в новой базе не найден!";
      if (artClass == ArtClass.Party || artClass == ArtClass.Instance)
      {
        int instanceObjectType = PumpHelper.GetInstanceObjectType(objTypeID, artClass);
        if (instanceObjectType > 0)
          objTypeID = instanceObjectType;
        else
          BasePumpHelper.AppManager.AddWarningMessage($"Тип экземпляра/партии, соответствующий типу ({art.SectID}), в новой базе не найден, закачиваем как обычное изделие (ART_ID={art.ID}, VER_ID={art.ArtVerID}).");
      }
    }
    else if (!docObjExists)
    {
      objTypeID = art.DocTypeToCreate;
      if (objTypeID <= 0)
        return $"Не найден соответствующий тип документа ({objTypeID}).";
    }
    return (string) null;
  }

  private Guid GetGuid(int id, bool isVersion)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query(isVersion ? "select f_guid from GUIDS_VART t where t.vart_id=@p1" : "select f_guid from GUIDS_ART t where t.art_id=@p1", (object) id))
    {
      if (dataReader.Read())
      {
        if (!dataReader.IsDBNull(0))
          return new Guid(Convert.ToString(dataReader[0]));
      }
    }
    Guid guid = this.plugin.Imdi.NewPumpGuid();
    BasePumpHelper.S4NonQuery(isVersion ? "INSERT INTO GUIDS_VART (VART_ID, F_GUID) VALUES (@p1, @p2)" : "INSERT INTO GUIDS_ART (ART_ID, F_GUID) VALUES (@p1, @p2)", (object) id, (object) guid.ToString("B").ToUpper());
    return guid;
  }

  protected void PumpArticle(Article baseArticle)
  {
    IImportedObjectList iol = this.Iol;
    this.CheckDataPacket();
    List<Article> plainList = baseArticle.PlainList;
    ArtClass artClass = baseArticle.ArtClass;
    int count = plainList.Count;
    Guid guid1 = Guid.Empty;
    int num1 = plainList.Count<Article>((System.Func<Article, bool>) (x => x.DocID == -2 && x.SectID == 1));
    if (num1 > 0 && num1 < plainList.Count)
    {
      List<Article> all = plainList.FindAll((Predicate<Article>) (x => x.ExistedDocObjectID != 0L));
      if (all.Count > 0)
      {
        long objectID = all[0].ExistedDocObjectID;
        if (all.Count > 1)
        {
          for (int index = 1; index < all.Count; ++index)
          {
            if (all[index].ExistedDocObjectID != objectID)
            {
              objectID = 0L;
              break;
            }
          }
        }
        if (objectID != 0L)
          guid1 = this.plugin.Imdi.ImportedObjects.GetGUID(objectID);
      }
    }
    Guid guid2 = guid1 != Guid.Empty ? guid1 : this.GetGuid(baseArticle.ID, false);
    ArtVerInfo artVerInfo1 = (ArtVerInfo) null;
    bool flag1 = true;
    bool flag2 = false;
    for (int index = 0; index < count; ++index)
    {
      Article art = plainList[index];
      int artVerId = art.ArtVerID;
      if (art.VartType == VartType.Version)
      {
        int objTypeID = 0;
        bool isBCH = false;
        bool flag3 = art.ExistedDocObjectID != 0L;
        bool flag4 = art.ExistedArtID != 0;
        string objType = this.DetermineObjType(art, ref objTypeID, flag3 | flag4, ref artClass, ref isBCH);
        if (objType != null)
        {
          string Message = $"{string.Format(this._cantPumpArticleErr, (object) art.ID, (object) artVerId)} {objType}";
          BasePumpHelper.AppManager.AddWarningMessage(Message);
        }
        else
        {
          ObjectRecord objRec = (ObjectRecord) null;
          bool flag5 = art.DocTypeToCreate != -1 | flag3;
          if (!flag5 || this._pumpMode != PumpArticlesMode.Articles || this.documentationCache.GetNewKey((object) BasePumpHelper.MakeCacheKey(baseArticle.ID, artVerId)) == 0L)
          {
            bool flag6 = true;
            bool flag7 = false;
            if (flag3)
            {
              objRec = new ObjectRecord()
              {
                Object_id = art.ExistedDocObjectID
              };
              iol.UseObject(objRec);
              flag7 = true;
            }
            else if (flag4)
            {
              flag6 = false;
            }
            else
            {
              BasePumpHelper.Logger.Write($"Pump article ART_ID={art.ID},VER_ID={artVerId} (T={art.SectID})");
              objRec = iol.AddObject(objTypeID, 0, PumpHelper.GetArticleCaption(art.Designation, art.Name));
              objRec.IdGuid = (object) guid2;
              int id = (art.Versions.Count > 0 ? (S4DBItem) art.Versions[artVerId] : (S4DBItem) art).Data.AsInteger("vart_id");
              if (id != -1)
                objRec.ObjectGuid = (object) this.GetGuid(id, true);
              PumpHelper.SetUpLCStep(objRec, art.Data, artVerId, this.statusesToLevels);
              flag7 = true;
            }
            ArtVerInfo artVerInfo2;
            if (flag1)
            {
              flag1 = false;
              artVerInfo2 = new ArtVerInfo(baseArticle.ID, artVerId, baseArticle.ArtVerID);
              artVerInfo1 = artVerInfo2;
              if (PumpHelper.IsNewDocsLinksFormat && baseArticle.DocsLinks != null)
                artVerInfo1.DocsLinks = baseArticle.DocsLinks;
            }
            else
              artVerInfo2 = new ArtVerInfo(0, artVerId)
              {
                MainVI = artVerInfo1
              };
            if (flag4)
            {
              DictionaryValue dictionaryValue = this.articlesCache.GetValue(art.ExistedArtID);
              if (dictionaryValue == null)
              {
                BasePumpHelper.AppManager.AddWarningMessage($"Версия изделия (ART_ID={art.ExistedArtID}) не найдена, невозможно привязать изделие ART_ID={art.ID}!");
                continue;
              }
              ArticleTag tag = dictionaryValue.Tag as ArticleTag;
              if (!tag.Versions.TryGetValue(tag.VersionID, out artVerInfo2.NewArtObjectID))
              {
                BasePumpHelper.AppManager.AddWarningMessage($"Версия изделия (ART_ID={art.ExistedArtID}, VER_ID={tag.VersionID}) не найдена, невозможно привязать изделие ART_ID={art.ID}!");
                continue;
              }
              artVerInfo2.ExtInfo = "ArtObjectID: " + artVerInfo2.NewArtObjectID.ToString();
            }
            else if (flag3)
            {
              artVerInfo2.NewArtObjectID = art.ExistedDocObjectID;
              artVerInfo2.ExtInfo = "UseObjectID: " + art.ExistedDocObjectID.ToString();
              artVerInfo2.DocsLinks = baseArticle.DocsLinks;
            }
            else
              artVerInfo2.ExtInfo = "Type: " + objTypeID.ToString();
            if (this._pumpMode == PumpArticlesMode.Orders)
              artVerInfo2.ID = Convert.ToInt32(art.Data["prjlink_id"].ToString());
            if (flag5)
              artVerInfo2.Flags |= ArticleFlag.Documentation;
            if (flag4)
              artVerInfo2.Flags |= ArticleFlag.LinkedBySuffix;
            if (art.SBFromSP)
              artVerInfo2.Flags |= ArticleFlag.SBFromSP;
            artVerInfo2.SectID = art.SectID;
            artVerInfo2.Name = art.Name;
            if (objRec != null)
            {
              objRec.VersionId = artVerId;
              objRec.IsBaseVersion = baseArticle.ArtVerID == artVerId;
              if (objRec.IsBaseVersion)
                flag2 = true;
              if (!flag2 && index == count - 1)
                objRec.IsBaseVersion = true;
            }
            this._versionsToCache.Add(artVerInfo2);
            if (flag7)
              this._inImportedList.Add(new Tuple<int, int, int>(baseArticle.ID, artVerId, iol.Items.CurrentIndex));
            if (!flag3 | flag4)
            {
              artVerInfo2.DocID = art.DocID;
              object obj = (object) -1;
              if (art.Data.TryGetValue("doc_ver_id", out obj))
                artVerInfo2.DocVerID = Convert.ToInt32(obj);
              artVerInfo2.DocsLinks = baseArticle.DocsLinks;
            }
            if (!flag3 & flag6)
            {
              iol.AddAttributeStr(PumpHelper.AttrTypeDesignationID, art.Designation);
              iol.AddAttributeStr(PumpHelper.AttrTypeNameID, art.Name);
              art.Data["okp_code"] = (object) art.Data["okp_code"].ToString().Trim();
              if (PluginSettings.AddArtID)
              {
                iol.AddAttributeInt(BasePumpHelper.AttrSearchID, (long) art.ID);
                if (artVerId != -1)
                  iol.AddAttributeInt(BasePumpHelper.AttrSearchVersionID, (long) artVerId);
              }
              iol.AddAttributeStr(PumpHelper.AttrTypeVersionCodeID, art.Data["isp_code"].ToString());
              iol.AddAttributeStr(PumpHelper.AttrTypeOKPCodeID, art.Data["okp_code"].ToString());
              string str1 = "";
              PumpHelper.MU.TryGetValue(Convert.ToInt32(art.Data["mu_id"]), out str1);
              string measureShortName = str1.Trim();
              IMeasureItem measure = this._measures.GetMeasure(measureShortName);
              object obj1 = art.Data["massa"];
              double num2 = DBNull.Value.Equals(obj1) ? 0.0 : Convert.ToDouble(obj1);
              string strValue = $"{num2} {measureShortName}";
              double num3 = num2 * measure.Koef;
              iol.AddAttributeMeasure(PumpHelper.AttrTypeMassaID, num3, measure.BaseMeasureId, strValue);
              long num4 = PumpHelper.PurchasedToLong(art.Data["purchased"]);
              AttributeRecord attributeRecord = iol.AddAttributeInt(PumpHelper.AttrPurchasedID, num4);
              if (num4 == -1L)
                attributeRecord.IntegerValue = (object) DBNull.Value;
              object obj2 = art.Data["imbase_key"];
              if (obj2 is string && obj2.ToString() != "")
              {
                artVerInfo2.ImbaseKey = obj2.ToString();
                if (!ImbaseImportHelper.ImbaseKeyHandler(iol, PumpHelper.AttrImbaseLinkID, PumpHelper.AttrImbaseCodeID, artVerInfo2.ImbaseKey, this._cacheData))
                  iol.AddAttributeStr(PumpHelper.AttrImbaseKeyID, obj2.ToString());
              }
              string str2 = !art.Data.ContainsKey("vart_note") || art.Data["vart_note"] == null || art.Data["vart_note"] == DBNull.Value ? Convert.ToString(art.Data["note"]) : Convert.ToString(art.Data["vart_note"]);
              iol.AddAttributeStr(PumpHelper.AttrTypeNoteID, str2);
              iol.AddAttributeStr(PumpHelper.AttrTypeLiteraID, PumpHelper.LiteraToString(art.Data["litera"]));
              object obj3 = art.Data["author"];
              if (!DBNull.Value.Equals(obj3))
              {
                int int32 = Convert.ToInt32(obj3);
                objRec.OwnerId = BasePumpHelper.UsersCache.GetNewKey((object) int32);
              }
              DateTime dateTime1 = PumpHelper.MinDBDateTime;
              object obj4 = art.Data["chkindate"];
              if (!DBNull.Value.Equals(obj4))
              {
                dateTime1 = Convert.ToDateTime(obj4);
              }
              else
              {
                Article article;
                if (baseArticle == art && art.Versions.TryGetValue(artVerId, out article))
                {
                  obj4 = article.Data["chkindate"];
                  if (!DBNull.Value.Equals(obj4))
                    dateTime1 = Convert.ToDateTime(obj4);
                }
              }
              if (dateTime1 != PumpHelper.MinDBDateTime)
                dateTime1 = dateTime1.ToUniversalTime();
              objRec.ObjCreate = dateTime1;
              if (art.Data.TryGetValue("prev_art_ver_id", out obj4) && !DBNull.Value.Equals(obj4))
                objRec.ParentVersionNo = Convert.ToInt32(obj4);
              DateTime dateTime2 = PumpHelper.MinDBDateTime;
              if (art.Data.TryGetValue("modifdate", out obj4) && !DBNull.Value.Equals(obj4))
                dateTime2 = Convert.ToDateTime(obj4).ToUniversalTime();
              objRec.ModifyDate = dateTime2;
              if (artClass == ArtClass.Party || artClass == ArtClass.Instance)
              {
                string key = artClass == ArtClass.Instance ? "serial_no" : "set_no";
                object obj5 = art.Data[key];
                string str3 = "";
                if (!DBNull.Value.Equals(obj5))
                  str3 = obj5.ToString();
                iol.AddAttributeStr(PumpHelper.AttrSerialNoID, str3);
              }
              else
              {
                iol.AddAttributeInt(PumpHelper.AttrArtStorageID, (long) artClass);
                string groupingGuid = art.GroupingGuid;
                if (groupingGuid != "")
                  iol.AddAttributeStr(PumpHelper.AttrTypeGroupInstanceID, groupingGuid);
              }
            }
            if (flag6)
            {
              if (art.SectID == 99999916)
              {
                int oldKey1 = art.AddData.AsInteger("adoc_id");
                DictionaryValue dictionaryValue1 = this.documentsCache.GetValue((object) oldKey1);
                if (dictionaryValue1 == null)
                {
                  BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={oldKey1}) не закачан, невозможно связать с копией!");
                  continue;
                }
                iol.AddAttributeInt(ConstsHolder.OriginalObjectID, dictionaryValue1.NewObjectID);
                int key = art.AddData.AsInteger("aversion_id");
                long num5;
                if (key > -1 && (dictionaryValue1.Tag as DocumentTag).Versions.TryGetValue(key, out num5))
                  iol.AddAttributeInt(ConstsHolder.OriginalObjectVersionID, Math.Abs(num5));
                int num6 = art.AddData.AsInteger("no_of_sheets", 0);
                if (num6 > 0)
                {
                  iol.AddAttributeInt(ConstsHolder.ListsCountID, (long) num6);
                  iol.AddAttributeInt(PumpHelper.AttrSheetsCountID, (long) num6);
                }
                int num7 = art.AddData.AsInteger("a4_sheets", 0);
                if (num7 > 0)
                  iol.AddAttributeInt(ConstsHolder.A4ListNumberID, (long) num7);
                if (!DBNull.Value.Equals(art.AddData["inv_nomer"]))
                  iol.AddAttributeStr(ConstsHolder.InventoryNumberID, art.AddData["inv_nomer"].ToString());
                int num8 = art.AddData.AsInteger("acopy_number");
                if (num8 > -1)
                  iol.AddAttributeInt(ConstsHolder.IndexOfCopyID, (long) num8);
                using (IDataReader dataReader = BasePumpHelper.S4Query("select d.receipt_date, d.return_date, d.group_id, g.user_id from distribute_list d, groups g where d.art_id=@p1 and d.group_id  = g.group_id order by return_date, receipt_date", (object) art.ID))
                {
                  DateTime dateTime3 = DateTime.MinValue;
                  DateTime dateTime4 = DateTime.MinValue;
                  CacheCategory cacheCategory1 = (CacheCategory) null;
                  int oldKey2 = 0;
                  CacheCategory cacheCategory2 = (CacheCategory) null;
                  int oldKey3 = 0;
                  while (dataReader.Read())
                  {
                    object obj6 = dataReader["return_date"];
                    bool flag8 = !DBNull.Value.Equals(obj6);
                    if (flag8)
                    {
                      dateTime4 = Convert.ToDateTime(obj6);
                      oldKey3 = Convert.ToInt32(dataReader["user_id"]);
                      if (oldKey3 != 0)
                      {
                        cacheCategory2 = BasePumpHelper.UsersCache;
                      }
                      else
                      {
                        oldKey3 = Convert.ToInt32(dataReader["group_id"]);
                        cacheCategory2 = BasePumpHelper.GroupsCache;
                      }
                    }
                    if (dateTime3 == DateTime.MinValue || !flag8)
                    {
                      object obj7 = dataReader["receipt_date"];
                      if (!DBNull.Value.Equals(obj7))
                        dateTime3 = Convert.ToDateTime(obj7);
                      oldKey2 = Convert.ToInt32(dataReader["user_id"]);
                      if (oldKey2 != 0)
                      {
                        cacheCategory1 = BasePumpHelper.UsersCache;
                      }
                      else
                      {
                        oldKey2 = Convert.ToInt32(dataReader["group_id"]);
                        cacheCategory1 = BasePumpHelper.GroupsCache;
                      }
                    }
                  }
                  if (dateTime3 != DateTime.MinValue)
                    iol.AddAttributeDate(ConstsHolder.ReceiptDateID, dateTime3);
                  if (dateTime4 != DateTime.MinValue)
                  {
                    iol.AddAttributeDate(ConstsHolder.ReturnDateID, dateTime4);
                    if (oldKey3 == 0)
                    {
                      oldKey3 = oldKey2;
                      cacheCategory2 = cacheCategory1;
                    }
                  }
                  if (cacheCategory1 != null)
                  {
                    DictionaryValue dictionaryValue2 = cacheCategory1.GetValue((object) oldKey2);
                    if (dictionaryValue2 != null)
                    {
                      iol.AddAttributeLink(ConstsHolder.RecipientID, dictionaryValue2.NewObjectID, dictionaryValue2.Caption);
                      iol.AddAttributeLink(ConstsHolder.AlbumSubscriberID, dictionaryValue2.NewObjectID, dictionaryValue2.Caption);
                    }
                  }
                  if (cacheCategory2 != null)
                  {
                    DictionaryValue dictionaryValue3 = cacheCategory2.GetValue((object) oldKey3);
                    if (dictionaryValue3 != null)
                      iol.AddAttributeLink(ConstsHolder.WhoReturnID, dictionaryValue3.NewObjectID, dictionaryValue3.Caption);
                  }
                  int num9 = 0;
                  int num10 = 0;
                  if (dateTime4 != DateTime.MinValue)
                  {
                    num9 = ConstsHolder.ReturnLCStepID;
                    num10 = ConstsHolder.LevelKeepingId;
                  }
                  else if (dateTime3 != DateTime.MinValue)
                  {
                    num9 = ConstsHolder.SendLCStepID;
                    num10 = ConstsHolder.LevelManufacturingId;
                  }
                  if (num9 != 0)
                  {
                    objRec.Lc_step = num9;
                    objRec.LevelId = num10;
                  }
                }
              }
              int sectId = art.SectID;
              foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) art.AddData)
              {
                string str = $"{sectId}.{keyValuePair.Key}";
                DictionaryValue artInfo = this.artParams.GetValue((object) str);
                if (artInfo != null)
                  PumpHelper.AddAttribute(iol, (int) artInfo.NewObjectID, keyValuePair.Value, artInfo, str);
              }
              foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) art.ThemeData)
              {
                int newKey = (int) this.themeParams.GetNewKey((object) Convert.ToInt32(keyValuePair.Key));
                if (newKey > 0)
                  PumpHelper.AddAttribute(iol, newKey, keyValuePair.Value);
              }
              foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) art.CommonParamsData)
                PumpHelper.AddAttribute(iol, Convert.ToInt32(keyValuePair.Key), keyValuePair.Value);
              if (isBCH)
              {
                object obj8;
                art.AddData.TryGetValue("material", out obj8);
                string s = "";
                if (obj8 != null)
                {
                  s = obj8.ToString();
                  int startIndex = Article.SuffixPos(s, false);
                  if (startIndex > 0)
                    s = s.Remove(startIndex);
                }
                object obj9;
                art.AddData.TryGetValue("sizes", out obj9);
                string str = $"{art.Name} {s} {(obj9 != null ? obj9.ToString() : "")}";
                iol.AddAttributeStr(PumpHelper.AttrBCHNameID, str);
              }
              if (!flag3)
                AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, iol);
            }
          }
        }
      }
    }
  }

  public static PumpClass GetPumpClass(bool afterPump, SearchDataPlugin plugin)
  {
    return !afterPump ? (PumpClass) new PumpArticlesClass(plugin) : (PumpClass) new PumpPLArticlesClass(plugin);
  }
}
