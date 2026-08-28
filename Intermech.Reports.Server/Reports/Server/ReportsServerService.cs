// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Server.ReportsServerService
// Assembly: Intermech.Reports.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97D7940-CE11-4EF0-80CD-76A0AE479D33
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Reports.Server.dll

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Intermech.Document.Server;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Reports;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Search.Interfaces.Signs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

#nullable disable
namespace Intermech.Reports.Server;

internal class ReportsServerService : LongLifeObject, IReportsServerService, IReportsServerUtils
{
  private void LoadDocumentData(
    ReportsBaseDoc[] documents,
    IUserSession session,
    ReportsDocModes docMode)
  {
    if (documents == null || documents.Length == 0 || session == null)
      return;
    Dictionary<int, List<ReportsBaseDoc>> dictionary1 = new Dictionary<int, List<ReportsBaseDoc>>();
    foreach (ReportsBaseDoc document in documents)
    {
      if (document != null && document.ObjectTypeID != -1)
      {
        int objectTypeId = document.ObjectTypeID;
        List<ReportsBaseDoc> reportsBaseDocList;
        if (!dictionary1.TryGetValue(objectTypeId, out reportsBaseDocList))
        {
          reportsBaseDocList = new List<ReportsBaseDoc>();
          dictionary1.Add(objectTypeId, reportsBaseDocList);
        }
        reportsBaseDocList.Add(document);
      }
    }
    foreach (KeyValuePair<int, List<ReportsBaseDoc>> keyValuePair in dictionary1)
    {
      int key = keyValuePair.Key;
      List<ReportsBaseDoc> reportsBaseDocList = keyValuePair.Value;
      bool flag = MetaDataHelper.IsObjectTypeChildOf(key, ReportsConsts.DocumentBaseTypeID) && !MetaDataHelper.IsObjectTypeChildOf(key, ReportsConsts.DocPackageBaseTypeID);
      if (flag)
      {
        if (flag)
        {
          foreach (ReportsBaseDoc reportsBaseDoc in reportsBaseDocList)
          {
            IDBObject dbObject = session.GetObject(reportsBaseDoc.ObjectID, false);
            if (dbObject != null)
            {
              if ((docMode & ReportsDocModes.IncludeCustomAttributes) != ReportsDocModes.None || (docMode & ReportsDocModes.IncludeObligatoryAttributes) != ReportsDocModes.None)
              {
                GetAttributeValuesModes modes = GetAttributeValuesModes.IncludeGuid;
                if ((docMode & ReportsDocModes.IncludeObligatoryAttributes) != ReportsDocModes.None)
                  modes |= GetAttributeValuesModes.IncludeObligatoryAttributes;
                foreach (AttributeValues attributesValue in dbObject.GetAttributesValues(modes))
                {
                  if (attributesValue.Values.Length == 1)
                    reportsBaseDoc.Attributes.Add(attributesValue.AttributeGuid, attributesValue.Values[0]);
                }
              }
              if ((docMode & ReportsDocModes.IncludeDocData) != ReportsDocModes.None && dbObject.GetAttributeByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545")) is IBlobReader attributeByGuid)
              {
                BlobInformation blobInformation = attributeByGuid.OpenBlob(0);
                try
                {
                  if (reportsBaseDoc is ReportsDoc reportsDoc)
                    reportsDoc.Data = attributeByGuid.ReadDataBlock((int) blobInformation.RealFileSize);
                }
                finally
                {
                  attributeByGuid.CloseBlob();
                }
              }
            }
          }
        }
      }
      else if ((docMode & ReportsDocModes.IncludeCustomAttributes) != ReportsDocModes.None || (docMode & ReportsDocModes.IncludeObligatoryAttributes) != ReportsDocModes.None)
      {
        List<IMSAttribute4ObjectType> attribute4ObjectTypeList = MetaDataHelper.GetAttribute4ObjectTypeList(key);
        List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>();
        columnDescriptorList.Add(new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0));
        if ((docMode & ReportsDocModes.IncludeObligatoryAttributes) != ReportsDocModes.None)
        {
          columnDescriptorList.Add(new ColumnDescriptor((object) -3, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0));
          columnDescriptorList.Add(new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0));
          columnDescriptorList.Add(new ColumnDescriptor((object) -50, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0));
        }
        if ((docMode & ReportsDocModes.IncludeCustomAttributes) != ReportsDocModes.None)
        {
          foreach (IMSAttribute4ObjectType attribute4ObjectType in attribute4ObjectTypeList)
          {
            if (attribute4ObjectType != null)
              columnDescriptorList.Add(new ColumnDescriptor((object) attribute4ObjectType.AttributeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0));
          }
        }
        List<ConditionStructure> conditionStructureList = new List<ConditionStructure>();
        Dictionary<long, ReportsBaseDoc> dictionary2 = new Dictionary<long, ReportsBaseDoc>(reportsBaseDocList.Count);
        foreach (ReportsBaseDoc reportsBaseDoc in reportsBaseDocList)
          dictionary2.Add(reportsBaseDoc.ObjectID, reportsBaseDoc);
        List<long> longList = new List<long>((IEnumerable<long>) dictionary2.Keys);
        conditionStructureList.Add(new ConditionStructure(-2, RelationalOperators.In, (object) longList.ToArray(), LogicalOperators.NONE, 0, false));
        if (columnDescriptorList.Count != 1)
        {
          IDBObjectCollection objectCollection = session.GetObjectCollection(key);
          if (objectCollection != null)
          {
            DBRecordSetParams paramSet = new DBRecordSetParams(conditionStructureList.ToArray(), columnDescriptorList.ToArray());
            DataTable dataTable = objectCollection.Select(paramSet);
            if (dataTable != null && dataTable.Rows.Count != 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
              {
                if (row != null)
                {
                  long int64 = Convert.ToInt64(row[0]);
                  ReportsBaseDoc reportsBaseDoc;
                  if (dictionary2.TryGetValue(int64, out reportsBaseDoc) && reportsBaseDoc != null)
                  {
                    for (int index = 1; index < dataTable.Columns.Count; ++index)
                    {
                      string columnName = dataTable.Columns[index].ColumnName;
                      if (GuidHelper.IsGuid(columnName))
                        reportsBaseDoc.Attributes.Add(new Guid(columnName), row[index]);
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  private void LoadDocumentItems(
    ReportsBaseDoc[] documents,
    IUserSession session,
    ReportsDocModes loadMode)
  {
    if (documents == null || documents.Length == 0 || session == null)
      return;
    List<ReportsBaseDoc> source = new List<ReportsBaseDoc>();
    Dictionary<int, bool> dictionary1 = new Dictionary<int, bool>();
    foreach (ReportsBaseDoc document in documents)
    {
      if (document != null && document.ObjectTypeID != -1)
      {
        bool flag;
        if (!dictionary1.TryGetValue(document.ObjectTypeID, out flag))
        {
          if (MetaDataHelper.GetApplicabilityChildObjectTypesID(document.ObjectTypeID, ReportsConsts.SimpleWithSortRelationID).Any<int>((System.Func<int, bool>) (childTypeId => MetaDataHelper.IsObjectTypeChildOf(childTypeId, ReportsConsts.DocPackageBaseTypeID) || MetaDataHelper.IsObjectTypeChildOf(childTypeId, ReportsConsts.DocumentBaseTypeID))))
            flag = true;
          dictionary1.Add(document.ObjectTypeID, flag);
        }
        if (flag)
          source.Add(document);
      }
    }
    if (source.Count == 0)
      return;
    List<long> longList = new List<long>();
    foreach (ReportsBaseDoc reportsBaseDoc in source)
    {
      if (reportsBaseDoc != null && reportsBaseDoc.ObjectID != 0L)
        longList.Add(reportsBaseDoc.ObjectID);
    }
    if (longList.Count == 0)
      return;
    IDBRelationCollection relationCollection = session.GetRelationCollection(ReportsConsts.SimpleWithSortRelationID);
    if (relationCollection == null)
      return;
    relationCollection.RelationTypeID = -1;
    relationCollection.LocalTypesMode = true;
    List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>()
    {
      new ColumnDescriptor((object) -21, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -7, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID("cad00202-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.ASC, 0)
    };
    List<ConditionStructure> conditionStructureList = new List<ConditionStructure>();
    int[] numArray = new int[2]
    {
      ReportsConsts.DocPackageBaseTypeID,
      ReportsConsts.DocumentBaseTypeID
    };
    List<int> list = new List<int>();
    foreach (int parentTypeID in numArray)
    {
      List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(parentTypeID);
      list.AddRange((IEnumerable<int>) childrenIdRecursive);
    }
    GenericListHelper.MakeUnique<int>(list);
    conditionStructureList.Add(new ConditionStructure(-7, RelationalOperators.In, (object) list.ToArray(), LogicalOperators.AND, 0, false));
    conditionStructureList.Add(new ConditionStructure(-23, RelationalOperators.Equal, (object) ReportsConsts.SimpleWithSortRelationID, LogicalOperators.AND, 0, false));
    conditionStructureList.Add(new ConditionStructure(-21, RelationalOperators.In, (object) longList.ToArray(), LogicalOperators.AND, 0, false));
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(new Guid("cadd937c-306c-11d8-b4e9-00304f19f545"));
    conditionStructureList.Add(new ConditionStructure(attributeTypeId, RelationalOperators.Greater, (object) -1, (object) null, LogicalOperators.NONE, 0, false, AttributeSourceTypes.Object, ColumnContents.Text));
    Dictionary<long, ReportsBaseDoc> dictionary2 = source.ToDictionary<ReportsBaseDoc, long>((System.Func<ReportsBaseDoc, long>) (document => document.ObjectID));
    DBRecordSetParams paramSet = new DBRecordSetParams(conditionStructureList.ToArray(), columnDescriptorList.ToArray());
    DataTable dataTable = relationCollection.Select(paramSet);
    if (dataTable == null || dataTable.Rows.Count == 0)
      return;
    List<ReportsBaseDoc> reportsBaseDocList = new List<ReportsBaseDoc>();
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      int int32 = Convert.ToInt32(row[2]);
      if (list.BinarySearch(int32) >= 0)
      {
        long int64_1 = Convert.ToInt64(row[0]);
        long int64_2 = Convert.ToInt64(row[1]);
        long int64_3 = Convert.ToInt64(row[3]);
        if (int64_2 != 0L && int32 != -1)
        {
          ReportsBaseDoc owner = dictionary2[int64_1];
          ReportsBaseDoc reportsBaseDoc = !MetaDataHelper.IsObjectTypeChildOf(int32, ReportsConsts.DocumentBaseTypeID) || MetaDataHelper.IsObjectTypeChildOf(int32, ReportsConsts.DocPackageBaseTypeID) ? (ReportsBaseDoc) new ReportsDocComplect(owner) : (ReportsBaseDoc) new ReportsDoc(owner);
          reportsBaseDoc.ObjectID = int64_2;
          reportsBaseDoc.ObjectTypeID = int32;
          reportsBaseDoc.Order = int64_3;
          owner.Items.Add(reportsBaseDoc);
          reportsBaseDocList.Add(reportsBaseDoc);
        }
      }
    }
    ReportsBaseDoc.RepDocComparer repDocComparer = new ReportsBaseDoc.RepDocComparer();
    foreach (ReportsBaseDoc reportsBaseDoc in dictionary2.Values)
    {
      if (reportsBaseDoc.Items.Count != 0)
        reportsBaseDoc.Items.Sort((IComparer<ReportsBaseDoc>) repDocComparer);
    }
    if (loadMode != ReportsDocModes.None)
      this.LoadDocumentData(reportsBaseDocList.ToArray(), session, loadMode);
    this.LoadDocumentItems(reportsBaseDocList.ToArray(), session, loadMode);
  }

  private bool RestoreComplectData(
    IUserSession session,
    ReportsBaseDoc reportsDoc,
    VisualNode imDocNode)
  {
    if (reportsDoc == null || imDocNode == null)
      return false;
    object obj;
    if (reportsDoc.Attributes.TryGetValue(ReportsConsts.CaptionAttrTypeGuid, out obj))
      imDocNode.Name = Convert.ToString(obj);
    int num = 0;
    foreach (ReportsBaseDoc reportsDoc1 in reportsDoc.Items)
    {
      if (reportsDoc1 != null)
      {
        VisualNode visualNode;
        if (reportsDoc1 is ReportsDocComplect)
          visualNode = (VisualNode) new DocumentsComplect();
        else if (reportsDoc1 is ReportsDoc reportsDoc2)
        {
          byte[] data = reportsDoc2.Data;
          if (data != null && data.Length != 0)
          {
            if (reportsDoc2.Attributes.ContainsKey(ReportsConsts.SourceLinkAttributeTypeGuid))
            {
              visualNode = (VisualNode) ImDocumentDataUtils.UnpackImDocument(data);
            }
            else
            {
              ImDocumentData imDocumentData;
              visualNode = (VisualNode) (imDocumentData = ImDocumentDataUtils.UnpackImDocument(data));
              MemoryStream baseInputStream = new MemoryStream(data);
              InflaterInputStream inflaterInputStream = new InflaterInputStream((Stream) baseInputStream);
              MemoryStream destination = new MemoryStream();
              inflaterInputStream.CopyTo((Stream) destination);
              inflaterInputStream.Dispose();
              baseInputStream.Dispose();
              destination.Position = 0L;
              ImDocumentServerPlugin.Instance.UpdateCheckSum(session, new CheckSumService(), imDocumentData, (Stream) destination, true, true);
              ImDocumentServerPlugin.Instance.UpdateDocumentDBObject(session, imDocumentData, reportsDoc1.ObjectID, false, false);
            }
          }
          else
            continue;
        }
        else
          continue;
        if (visualNode != null)
        {
          imDocNode.InsertChildNode(num++, (DocumentTreeNode) visualNode, false, true, false, false, false);
          this.RestoreComplectData(session, reportsDoc1, visualNode);
        }
      }
    }
    return true;
  }

  bool IReportsServerService.LoadCompectData(
    long objectId,
    out ReportsDocComplect complect,
    Guid sessionGuid,
    ReportsDocModes loadMode)
  {
    return ((IReportsServerService) this).LoadComplectData(objectId, out complect, sessionGuid, loadMode);
  }

  bool IReportsServerService.LoadComplectData(
    long objectId,
    out ReportsDocComplect complect,
    Guid sessionGuid,
    ReportsDocModes loadMode)
  {
    complect = (ReportsDocComplect) null;
    if (objectId == 0L || sessionGuid == Guid.Empty)
      return false;
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    IDBObject dbObject = sessionById?.GetObject(objectId, false);
    if (dbObject == null || !MetaDataHelper.IsObjectTypeChildOf(dbObject.ObjectType, ReportsConsts.DocPackageBaseTypeID))
      return false;
    ref ReportsDocComplect local = ref complect;
    ReportsDocComplect reportsDocComplect = new ReportsDocComplect((ReportsBaseDoc) null);
    reportsDocComplect.ObjectID = dbObject.ObjectID;
    reportsDocComplect.ObjectTypeID = dbObject.ObjectType;
    local = reportsDocComplect;
    if ((loadMode & ReportsDocModes.IncludeCustomAttributes) != ReportsDocModes.None || (loadMode & ReportsDocModes.IncludeObligatoryAttributes) != ReportsDocModes.None)
      this.LoadDocumentData(new ReportsBaseDoc[1]
      {
        (ReportsBaseDoc) complect
      }, sessionById, loadMode);
    this.LoadDocumentItems(new ReportsBaseDoc[1]
    {
      (ReportsBaseDoc) complect
    }, sessionById, loadMode);
    return true;
  }

  public bool RestoreComplectData(
    Guid sessionGuid,
    ReportsBaseDoc reportsDoc,
    out DocumentsComplect complect)
  {
    if (reportsDoc == null)
      throw new ArgumentNullException(nameof (reportsDoc));
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    complect = new DocumentsComplect();
    return this.RestoreComplectData(sessionById, reportsDoc, (VisualNode) complect);
  }
}
