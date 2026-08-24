// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper.GroupDocuments
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.TechCard.Document.Interfaces.Configs.Common;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper;

internal class GroupDocuments
{
  private readonly IDictionary<int, QuickObjectInfo> _tcKey2ObjectInfoCache = (IDictionary<int, QuickObjectInfo>) new Dictionary<int, QuickObjectInfo>();
  private readonly int _idxFldKey;
  private readonly int _idxFldName = 1;
  private readonly int _idxFldNumStart = 2;
  private readonly int _idxFldNumNext = 3;
  private readonly int _idxFldNumLength = 4;
  private int _nameId;
  private int _firstPageId;
  private int _stepNumberId;
  private int _numberOfCharId;

  private IDictionary<string, QuickObjectInfo> LoadIpsDocumentGroups(IUserSession session)
  {
    if (MetaDataHelper.GetObjectTypeID(GroupDocument.GDObject.GdObjectGuid) == -1)
      return (IDictionary<string, QuickObjectInfo>) null;
    DataTable dataTable = session.GetObjectCollection(new Guid()).Select(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.DESC, 2),
      new ColumnDescriptor((object) MetaDataHelper.GetAttributeID((object) "cad00020-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.ASC, 1)
    }, recordCount: -2));
    Dictionary<string, QuickObjectInfo> dictionary = new Dictionary<string, QuickObjectInfo>();
    if (dataTable.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        string key = Convert.ToString(row[1]);
        if (!dictionary.ContainsKey(key))
          dictionary[key] = session.GetObjectInfo(int64);
      }
    }
    return (IDictionary<string, QuickObjectInfo>) dictionary;
  }

  private IDBObject PumpObject(IUserSession session, IDataReader dataReader)
  {
    IDBObject dbObject = session.GetObjectCollection(GroupDocument.GDObject.GDObjectId).Create();
    dbObject.SetAttributesValues(new List<AttributeValues>()
    {
      new AttributeValues(this._nameId, (object) dataReader[this._idxFldName].ToString()),
      new AttributeValues(this._firstPageId, (object) dataReader[this._idxFldNumStart].ToString()),
      new AttributeValues(this._stepNumberId, (object) dataReader[this._idxFldNumNext].ToString()),
      new AttributeValues(this._numberOfCharId, (object) dataReader[this._idxFldNumLength].ToString())
    }.ToArray());
    dbObject.CommitCreation(true);
    return dbObject;
  }

  public void Pump(IUserSession session)
  {
    IDictionary<string, QuickObjectInfo> dictionary = this.LoadIpsDocumentGroups(session);
    if (dictionary == null)
      return;
    string str = $"SELECT {"F_KEY"}, {"F_NAME"}, {"F_NUM_START"}, {"F_NUM_NEXT"}, {"F_NUM_LENGTH"} FROM {"TC_DOC_GROUP"}";
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = str;
    this._nameId = session.IdentHelper.GetAttributeID(GroupDocument.Name.NameGuidStr());
    this._firstPageId = session.IdentHelper.GetAttributeID(GroupDocument.FirstPage.FirstPageGuidStr());
    this._stepNumberId = session.IdentHelper.GetAttributeID(GroupDocument.StepNumber.StepNumberGuidStr());
    this._numberOfCharId = session.IdentHelper.GetAttributeID(GroupDocument.NumberOfChar.NumberOfCharGuidStr());
    using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
    {
      try
      {
        while (dataReader.Read())
        {
          int int32 = Convert.ToInt32(dataReader.GetInt32(this._idxFldKey));
          string key = dataReader[this._idxFldName].ToString();
          QuickObjectInfo quickObjectInfo;
          if (dictionary.TryGetValue(key, out quickObjectInfo))
          {
            this._tcKey2ObjectInfoCache[int32] = quickObjectInfo;
          }
          else
          {
            IDBObject dbObject = this.PumpObject(session, dataReader);
            if (dbObject != null)
              this._tcKey2ObjectInfoCache[int32] = session.GetObjectInfo(dbObject.ObjectID);
          }
        }
      }
      finally
      {
        dataReader.Close();
      }
    }
  }

  public QuickObjectInfo GetGroupDocumentInfoByTcKey(int tcDocKey)
  {
    QuickObjectInfo documentInfoByTcKey;
    this._tcKey2ObjectInfoCache.TryGetValue(tcDocKey, out documentInfoByTcKey);
    return documentInfoByTcKey;
  }
}
