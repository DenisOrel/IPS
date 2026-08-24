// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpProductionListsAfterPumping
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.SearchData.ItemFactories;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.MRP2;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal sealed class PumpProductionListsAfterPumping : PumpProductionLists
{
  private readonly int _objTypeArticles;
  private readonly int _objTypeMaterials;
  private readonly int _objTypeDocuments;
  private readonly int _attributeSearchID;
  private readonly int _attributeSearchVerID;

  public PumpProductionListsAfterPumping(SearchDataPlugin plugin)
    : base(plugin)
  {
    this._objTypeArticles = MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545");
    this._objTypeMaterials = MetaDataHelper.GetObjectTypeID("cad00170-306c-11d8-b4e9-00304f19f545");
    this._objTypeDocuments = MetaDataHelper.GetObjectTypeID("cad00070-306c-11d8-b4e9-00304f19f545");
    this._attributeSearchID = MetaDataHelper.GetAttributeID((object) new Guid("cad0132b-306c-11d8-b4e9-00304f19f545"));
    this._attributeSearchVerID = MetaDataHelper.GetAttributeID((object) new Guid("cad007a4-306c-11d8-b4e9-00304f19f545"));
    this.taskPump.Repumpble = true;
    this.taskExam.Repumpble = true;
  }

  protected override long GetArticleID(
    IUserSession session,
    IImportingData cacheData,
    int artID,
    int artVerID,
    string searchHash,
    out string caption,
    out int objectType,
    out long id)
  {
    long articleId = base.GetArticleID(session, cacheData, artID, artVerID, searchHash, out caption, out objectType, out id);
    if (articleId == 0L)
    {
      QuickObjectInfo articleObject = this.FindArticleObject(session, artID, artVerID);
      if (!articleObject.Empty)
      {
        articleId = articleObject.ObjectID;
        id = articleObject.ID;
        caption = articleObject.Caption;
        objectType = articleObject.ObjectTypeID;
      }
      else
      {
        articleId = 0L;
        id = 0L;
        caption = string.Empty;
        objectType = -1;
      }
    }
    return articleId;
  }

  private long FindInCollection(IUserSession session, int objTypeID, int artID, int artVerID)
  {
    DataTable dataTable = session.GetObjectCollection(objTypeID).Select(new DBRecordSetParams(new ConditionStructure[2]
    {
      new ConditionStructure(this._attributeSearchID, RelationalOperators.Equal, (object) artID, LogicalOperators.AND, 0, false),
      new ConditionStructure(this._attributeSearchVerID, RelationalOperators.Equal, (object) artVerID, LogicalOperators.AND, 0, false)
    }, new object[1]{ (object) -2 }));
    return dataTable.Rows.Count <= 0 ? 0L : Convert.ToInt64(dataTable.Rows[0][0]);
  }

  private QuickObjectInfo FindOnSearchID(IUserSession session, int artID, int artVerID)
  {
    long inCollection = this.FindInCollection(session, this._objTypeArticles, artID, artVerID);
    if (inCollection == 0L)
      inCollection = this.FindInCollection(session, this._objTypeMaterials, artID, artVerID);
    if (inCollection == 0L)
      inCollection = this.FindInCollection(session, MRP2Consts.objtypeIdProductionLists, artID, artVerID);
    if (inCollection != 0L)
      return session.GetObjectInfo(inCollection);
    return new QuickObjectInfo() { ObjectTypeID = -1 };
  }

  private QuickObjectInfo FindArticleObject(IUserSession session, int artID, int artVerID)
  {
    string str = Convert.ToString(BasePumpHelper.S4ObjectQuery("select g.f_guid from v_articles a, guids_vart g where a.vart_id = g.vart_id and a.art_id = @p1 and a.art_ver_id = @p2", (object) artID, (object) artVerID));
    return string.IsNullOrEmpty(str) || !GuidHelper.IsGuid(str) ? this.FindOnSearchID(session, artID, artVerID) : session.GetObjectInfo(new Guid(str));
  }

  private long GetDocumentID(
    Dictionary<object, DictionaryValue> documentsCache,
    Dictionary<object, DictionaryValue> objectGuids,
    int docID,
    out long objectID,
    out int objectTypeID,
    out bool techcardDocument)
  {
    if (DocLinksHelper.GetDocumentID(documentsCache, objectGuids, docID, out objectID, out objectTypeID, out techcardDocument) == 0L)
    {
      IUserSession userSession = ((PumpClass) this).plugin.Idw.GetUserSession();
      string str = Convert.ToString(BasePumpHelper.S4ObjectQuery("select f_guid from guids_doc where doc_id = @p1", (object) docID));
      IDBObject objectById;
      if (string.IsNullOrEmpty(str) || !GuidHelper.IsGuid(str))
      {
        DataTable dataTable = userSession.GetObjectCollection(MetaDataHelper.GetObjectTypeID("cad00070-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(this._attributeSearchID, RelationalOperators.Equal, (object) docID, LogicalOperators.AND, 0, false)
        }, new object[1]{ (object) -2 }));
        objectById = dataTable.Rows.Count > 0 ? userSession.GetObject(Convert.ToInt64(dataTable.Rows[0][0]), false) : (IDBObject) null;
      }
      else
        objectById = userSession.GetObjectByID(new Guid(str), false);
      if (objectById != null)
      {
        objectID = objectById.ObjectID;
        objectTypeID = objectById.ObjectType;
        techcardDocument = false;
        return objectById.ID;
      }
    }
    return 0;
  }

  protected override long CreateProductionСopy(
    IUserSession session,
    IImportingData cacheData,
    IImportedObjectList writer,
    IImportingData currentZakazCache,
    ProductionListItem productionListItem,
    bool writeMessage,
    out string caption,
    out int objectTypeID,
    out long id,
    int ctx_id,
    string searchHash,
    out bool isNewObject)
  {
    caption = string.Empty;
    int partArticleId = productionListItem.PartArticleID;
    int partArticleVer = productionListItem.PartArticleVer;
    int zparentRecId = productionListItem.ZParentRecID;
    long productionСopy = base.CreateProductionСopy(session, cacheData, writer, currentZakazCache, productionListItem, false, out caption, out objectTypeID, out id, ctx_id, searchHash, out isNewObject);
    switch (productionСopy)
    {
      case -1:
        return 0;
      case 0:
        QuickObjectInfo articleObject = this.FindArticleObject(session, partArticleId, partArticleVer);
        if (articleObject.Empty)
        {
          BasePumpHelper.AppManager.AddWarningMessage($"Изделие part_aid={partArticleId} part_ver={partArticleVer} не найдено ни в кэше закачанных изделий ни в базе-приемнике (linkType = {productionListItem.LinkType})");
          return 0;
        }
        if (ctx_id != 0)
        {
          caption = articleObject.Caption;
          id = articleObject.ID;
          objectTypeID = articleObject.ObjectTypeID;
          return articleObject.ObjectID;
        }
        string pcdse = this.GetPCDSE(cacheData, $"{partArticleId}", articleObject.ObjectID, Guid.Empty);
        objectTypeID = this.GetProductionCopyTypeID(zparentRecId, articleObject.ObjectTypeID);
        writer.AddObject(objectTypeID, 0, articleObject.Caption);
        foreach (DataRow row in (InternalDataCollectionBase) ((PumpClass) this).plugin.Imdi.dbImporter.GetAttributeValues(articleObject.ObjectTypeID, articleObject.ObjectID).Rows)
        {
          int int32 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
          AttributeRecord relAttr = new AttributeRecord(int32, 0L)
          {
            InlistId = Convert.ToInt32(row["F_INLIST_ID"]),
            IntegerValue = row["F_INTEGER_VALUE"],
            DoubleValue = row["F_DOUBLE_VALUE"],
            DateValue = row["F_DATE_VALUE"],
            StringValue = row["F_STRING_VALUE"]
          };
          if (int32 == PumpHelper.AttrContentModifiedDate)
            relAttr.DateValue = (object) DateTime.UtcNow;
          writer.AddAttribute(relAttr);
        }
        writer.AddAttributeStr(MRP2Consts.attrIdPKDSE_Id, pcdse);
        writer.AddAttributeStr(MRP2Consts.attrIdHashSearch, searchHash);
        writer.AddAttributeLink(MRP2Consts.attrIdArticleLink, articleObject.ObjectID, articleObject.Caption);
        AttributesHelper.CorrectObligatoryObjectAttributes(session, writer);
        writer.Import();
        ObjectRecord objectRecord = writer.Items[0].Object;
        long objectId = objectRecord.Object_id;
        caption = articleObject.Caption;
        id = objectRecord.Id;
        isNewObject = true;
        cacheData.AddValue(ImportingCategory.ProductionСopyIDCache, (object) objectId, id, pcdse);
        cacheData.AddValue(ImportingCategory.ProductionCopiesHash, (object) searchHash, objectId, caption, (ITagImportObject) new ObjectInfoEx(objectTypeID, id));
        if (cacheData.GetValue(ImportingCategory.ArticlePCDSECache, (object) articleObject.ObjectID) == null && !string.IsNullOrEmpty(pcdse))
          cacheData.AddValue(ImportingCategory.ArticlePCDSECache, (object) articleObject.ObjectID, 0L, pcdse);
        writer.Items.Clear();
        return objectId;
      default:
        return productionСopy;
    }
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Определение доступных производственных ведомостей для миграции", 1);
    this.ReadPLList(2, 99);
    this.ExamCheckPoint("Инициализация метаданных успешно завершена", 100);
  }

  protected override void RestoreDocLinks(
    IUserSession session,
    Dictionary<object, DictionaryValue> documentsCache,
    Dictionary<object, DictionaryValue> objectGuids,
    int partArticleID,
    int partArticleVer,
    long plObjectID)
  {
    DocsLinks docLinksForArticle = DocLinksHelper.GetDocLinksForArticle(partArticleID);
    if (docLinksForArticle == null || docLinksForArticle.Count == 0)
      return;
    int relationTypeId = MetaDataHelper.GetRelationTypeID(this.idHelper.RelationTypes.F5Type.RelationType);
    DataTable dataTable = session.GetRelationCollection(relationTypeId).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) -18, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
    }), plObjectID);
    List<Guid> guidList = new List<Guid>();
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      guidList.Add(new Guid(Convert.ToString(row[0])));
    long projId = plObjectID;
    IImportedRelationList importedRelationList = ((PumpClass) this).plugin.Idw.CreateImportedRelationList(0);
    foreach (DocLink docLink in (List<DocLink>) docLinksForArticle)
    {
      if (!(docLink is DocLinkEx docLinkEx) || partArticleVer >= docLinkEx.ArtVerID && (docLinkEx.DelVerID == -1 || partArticleVer < docLinkEx.DelVerID))
      {
        string str = Convert.ToString(BasePumpHelper.S4ObjectQuery("select f_guid from guids_doc where doc_id = @p1", (object) docLink.DocID));
        if (string.IsNullOrEmpty(str))
          BasePumpHelper.AppManager.AddWarningMessage($"Для документа doc_id={docLink.DocID} не найден guid в таблице guids_doc!");
        if (!GuidHelper.IsGuid(str))
        {
          BasePumpHelper.AppManager.AddWarningMessage($"Для документа doc_id={docLink.DocID} некорректное значение guid - \"{str}\" в таблице guids_doc!");
        }
        else
        {
          Guid guid = new Guid(str);
          if (!guidList.Contains(guid))
          {
            long documentId = this.GetDocumentID(documentsCache, objectGuids, docLink.DocID, out long _, out int _, out bool _);
            if (documentId != 0L)
            {
              importedRelationList.AddRelationFromID(projId, documentId, relationTypeId);
              AttributesHelper.AddObligatoryRelationAttributes(((PumpClass) this).plugin.Idw, importedRelationList);
              importedRelationList.Import();
              importedRelationList.Items.Clear();
            }
          }
        }
      }
    }
  }

  protected override long FindDocumentation(
    int artId,
    int artVerId,
    out string caption,
    out int objectTypeID,
    out long id)
  {
    object obj = BasePumpHelper.S4ObjectQuery("select r.doc_guid from v_articles a left join rc r on a.doc_id = r.doc_id and a.doc_ver_id = r.version_id where a.art_id = @p1 and a.art_ver_id = @p2", (object) artId, (object) artVerId);
    if (obj == DBNull.Value || string.IsNullOrEmpty(Convert.ToString(obj)))
      return base.FindDocumentation(artId, artVerId, out caption, out objectTypeID, out id);
    QuickObjectInfo objectInfo = ((PumpClass) this).plugin.Idw.GetUserSession().GetObjectInfo(new Guid(Convert.ToString(obj)));
    if (objectInfo.Empty)
      return base.FindDocumentation(artId, artVerId, out caption, out objectTypeID, out id);
    caption = objectInfo.Caption;
    objectTypeID = objectInfo.ObjectTypeID;
    id = objectInfo.ID;
    return objectInfo.ObjectID;
  }
}
