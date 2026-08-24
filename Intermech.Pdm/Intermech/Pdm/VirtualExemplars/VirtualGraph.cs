// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.VirtualGraph
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class VirtualGraph
{
  private List<VirtualExemplar> _exemplars;
  private List<VirtualRelation> _relations;
  private bool _errorPresent;
  private IDictionary<long, List<ExistsExemplar>> _existsExemplars;
  private IDictionary<Guid, VirtualExemplar> _exemplarGuids;

  public event AddStringHandler AddStringEvent;

  public bool ErrorPresent => this._errorPresent;

  public List<VirtualExemplar> Exemplars => this._exemplars;

  public List<VirtualRelation> Relations => this._relations;

  private void AddMessage(string message)
  {
    if (this.AddStringEvent == null)
      return;
    this.AddStringEvent(message);
  }

  public VirtualExemplar GetExemplar(Guid guid) => this._exemplarGuids[guid];

  public void GetTree(IUserSession session, long rootArticle, string ownerID)
  {
    this._existsExemplars = (IDictionary<long, List<ExistsExemplar>>) new Dictionary<long, List<ExistsExemplar>>();
    this._exemplars = new List<VirtualExemplar>();
    this._relations = new List<VirtualRelation>();
    this._exemplarGuids = (IDictionary<Guid, VirtualExemplar>) new Dictionary<Guid, VirtualExemplar>();
    int exRellTypeID = -1;
    IDBRelationType relationType = session.GetRelationType(PDMHelper.relationTypeInstances, false);
    if (relationType != null)
      exRellTypeID = relationType.RelationType;
    IDBRelationCollection relationCollection1 = session.GetRelationCollection(session.IdentHelper.SPRelationTypeID, ownerID);
    IDBAttributeType attributeType1 = session.GetAttributeType(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    IDBAttributeType attributeType2 = session.GetAttributeType(PDMHelper.attributeStorageArticle);
    DBRecordSetParams dbParams = new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[5]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) attributeType2.AttributeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) attributeType1.AttributeID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -50, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
    });
    dbParams.Tags = new HybridDictionary()
    {
      {
        (object) "{82E381A1-8952-416A-B303-F81BA2945F8F}",
        (object) true
      },
      {
        (object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}",
        (object) new List<long>(1) { 0L, 1L }
      }
    };
    IDBObject dbObject = session.GetObject(rootArticle, false);
    if (dbObject == null)
      return;
    IDBRelationCollection relationCollection2 = session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PDMHelper.relationTypeInstances));
    List<object> columns = new List<object>();
    columns.Add((object) -2);
    columns.Add((object) -7);
    IDBAttributeType attributeType3 = session.GetAttributeType(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"), true);
    columns.Add((object) attributeType3.AttributeID);
    IDBAttributeType attributeType4 = session.GetAttributeType(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"), true);
    columns.Add((object) attributeType4.AttributeID);
    IDBAttributeType attributeType5 = session.GetAttributeType(PDMHelper.attributeSerialNo, true);
    columns.Add((object) attributeType5.AttributeID);
    columns.Add((object) -3);
    columns.Add((object) -9);
    IDBAttribute attributeById = dbObject.GetAttributeByID(attributeType2.AttributeID);
    ArticlesInManufacture articlesInManufacture = attributeById != null ? (ArticlesInManufacture) attributeById.AsInteger : ArticlesInManufacture.Parties;
    IDBObjectType instanceObjectType = PDMHelper.GetInstanceObjectType(session, dbObject.ObjectType, articlesInManufacture);
    if (instanceObjectType == null)
    {
      this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_438"), (object) dbObject.NameInMessages));
    }
    else
    {
      VirtualExemplar Parent = new VirtualExemplar(rootArticle, instanceObjectType.ObjectType, articlesInManufacture);
      this._exemplars.Add(Parent);
      this._exemplarGuids.Add(Parent.Guid, Parent);
      Parent.Name = dbObject.Caption;
      this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_441"), (object) dbObject.NameInMessages, (object) Parent.ArticleID));
      this.GetTreeNode(session, Parent, relationCollection1, dbParams, relationCollection2, columns, exRellTypeID);
    }
  }

  public void RollbackTree()
  {
    if (this.Exemplars == null)
      return;
    foreach (VirtualExemplar exemplar in this.Exemplars)
      exemplar.Rollback();
  }

  private VirtualExemplar AddExemplar(
    IUserSession session,
    long articleID,
    int exemplarObjectType,
    ArticlesInManufacture articlesInManufacture,
    IDBRelationCollection rellCollInst,
    List<object> columns,
    int exRellTypeID)
  {
    VirtualExemplar exemplar1 = (VirtualExemplar) null;
    if (articlesInManufacture == ArticlesInManufacture.Parties)
    {
      foreach (VirtualExemplar exemplar2 in this._exemplars)
      {
        if (exemplar2.ArticleID == articleID)
          return exemplar2;
      }
    }
    if (exemplar1 == null)
    {
      exemplar1 = new VirtualExemplar(articleID, exemplarObjectType, articlesInManufacture);
      if (!this._existsExemplars.ContainsKey(Math.Abs(exemplar1.ArticleID)))
      {
        IDBObjectCollection objectCollection = session.GetObjectCollection(exemplar1.ExemplarObjectType);
        List<ConditionStructure> conditionStructureList = new List<ConditionStructure>();
        conditionStructureList.Add(new ConditionStructure(new Guid("cad00622-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) Math.Abs(exemplar1.ArticleID), LogicalOperators.AND, 0));
        if (exemplar1.ArticlesInManufacture == ArticlesInManufacture.Parties)
          conditionStructureList.Add(new ConditionStructure(PDMHelper.attributeActiveParty, RelationalOperators.Equal, (object) true, LogicalOperators.AND, 0));
        if (exemplar1.ArticlesInManufacture == ArticlesInManufacture.Instances)
        {
          DataTable applicabilitiesList = session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(exRellTypeID, exemplar1.ExemplarObjectType, -1);
          if (applicabilitiesList.Rows.Count > 0)
          {
            List<int> intList = new List<int>(applicabilitiesList.Rows.Count);
            foreach (DataRow row in (InternalDataCollectionBase) applicabilitiesList.Rows)
            {
              if (Convert.ToInt32(row["F_MIN_LINKS"]) != -1)
                intList.Add(Convert.ToInt32(row["F_INOBJECT_TYPE"]));
            }
            if (intList.Count > 0)
              conditionStructureList.Add(new ConditionStructure(0, RelationalOperators.NotEntersInType, (object) intList.ToArray(), LogicalOperators.AND, 0, false));
          }
        }
        DataTable dataTable = objectCollection.Select(new DBRecordSetParams(conditionStructureList.ToArray(), columns.ToArray()));
        if (dataTable.Rows.Count > 0)
        {
          List<ExistsExemplar> existsExemplarList = new List<ExistsExemplar>();
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            existsExemplarList.Add(new ExistsExemplar(Convert.ToInt64(row[0]), Convert.ToInt32(row[1]), Convert.ToString(row[4]), Convert.ToString(row[2]), Convert.ToString(row[3])));
          this._existsExemplars.Add(Math.Abs(exemplar1.ArticleID), existsExemplarList);
        }
        else
        {
          if (exemplar1.ArticlesInManufacture == ArticlesInManufacture.Parties)
            exemplar1.SetActive = true;
          this._existsExemplars.Add(Math.Abs(exemplar1.ArticleID), (List<ExistsExemplar>) null);
        }
      }
      if (this._existsExemplars[Math.Abs(exemplar1.ArticleID)] != null && this._existsExemplars[Math.Abs(exemplar1.ArticleID)].Count > 0)
      {
        if (this._existsExemplars[Math.Abs(exemplar1.ArticleID)].Count == 1 && exemplar1.ArticlesInManufacture == ArticlesInManufacture.Parties)
        {
          exemplar1.ExemplarID = this._existsExemplars[Math.Abs(exemplar1.ArticleID)][0].InstanceID;
        }
        else
        {
          ExistsExemplarForm existsExemplarForm = new ExistsExemplarForm();
          existsExemplarForm.SetFormData(this._existsExemplars[Math.Abs(exemplar1.ArticleID)], exemplar1);
          if (existsExemplarForm.ShowDialog() == DialogResult.OK && existsExemplarForm.SelectedExistsExemplar != null)
          {
            exemplar1.ExemplarID = existsExemplarForm.SelectedExistsExemplar.InstanceID;
            if (exemplar1.ArticlesInManufacture == ArticlesInManufacture.Instances)
              this._existsExemplars[Math.Abs(exemplar1.ArticleID)].Remove(existsExemplarForm.SelectedExistsExemplar);
          }
        }
      }
    }
    this._exemplars.Add(exemplar1);
    this._exemplarGuids.Add(exemplar1.Guid, exemplar1);
    return exemplar1;
  }

  private void GetTreeNode(
    IUserSession session,
    VirtualExemplar Parent,
    IDBRelationCollection relationColl,
    DBRecordSetParams dbParams,
    IDBRelationCollection rellCollInst,
    List<object> columns,
    int exRellTypeID)
  {
    List<VirtualNode> virtualNodeList = new List<VirtualNode>();
    foreach (DataRow row in (InternalDataCollectionBase) relationColl.ConsistFrom(dbParams, Parent.ArticleID).Rows)
    {
      long int64 = Convert.ToInt64(row[0]);
      string str1 = Convert.ToString(row[4]);
      IDBObjectType objectType = session.GetObjectType(Convert.ToInt32(row[1]));
      string str2 = str1 != string.Empty ? $"{objectType.ObjectInstanceName} '{str1}'" : string.Format(LocalizationHolder.rm.GetString("Pdm_443"), (object) objectType.ObjectInstanceName, (object) int64);
      if (row[2] != null && row[2] != DBNull.Value)
      {
        ArticlesInManufacture int32 = (ArticlesInManufacture) Convert.ToInt32(row[2]);
        IDBObjectType instanceObjectType = PDMHelper.GetInstanceObjectType(session, Convert.ToInt32(row[1]), int32);
        if (instanceObjectType == null)
        {
          this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_438"), (object) str2));
          this._errorPresent = true;
        }
        else
        {
          MeasuredValue quantity1 = CompareValuesHelper.NormalizedValue((object) Convert.ToString(row[3])) != null ? MeasureHelper.ConvertToMeasuredValue(Convert.ToString(row[3])) : (MeasuredValue) null;
          if (quantity1 == null)
          {
            this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_439"), (object) Parent.Name, (object) Parent.ArticleID, (object) str1, (object) int64));
            this._errorPresent = true;
          }
          int num = 1;
          if (quantity1 != null)
          {
            quantity1 = MeasureHelper.ConvertToBaseMeasure(quantity1);
            num = Convert.ToInt32(quantity1.Value);
          }
          switch (int32)
          {
            case ArticlesInManufacture.Parties:
              VirtualExemplar Parent1 = this.AddExemplar(session, int64, instanceObjectType.ObjectType, int32, rellCollInst, columns, exRellTypeID);
              Parent1.Name = str1;
              this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_441"), (object) str2, (object) Parent1.ArticleID));
              this._relations.Add(new VirtualRelation(Parent.Guid, Parent1.Guid, quantity1));
              if (Parent1.ExemplarID == 0L)
              {
                this.GetTreeNode(session, Parent1, relationColl, dbParams, rellCollInst, columns, exRellTypeID);
                continue;
              }
              continue;
            case ArticlesInManufacture.Instances:
              if (Parent.ArticlesInManufacture == ArticlesInManufacture.Parties)
              {
                this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_440"), (object) str1, (object) int64, (object) Parent.Name, (object) Parent.ArticleID));
                this._errorPresent = true;
              }
              MeasuredValue quantity2 = (MeasuredValue) null;
              if (quantity1 != null)
              {
                quantity2 = MeasureHelper.ConvertToMeasuredValue(quantity1.ToString());
                quantity2.Value = 1.0;
                quantity2.Caption = MeasureHelper.ConvertToString(quantity2.Value, quantity2.MeasureID, false);
              }
              for (int index = 0; index < num; ++index)
              {
                VirtualExemplar Parent2 = this.AddExemplar(session, int64, instanceObjectType.ObjectType, int32, rellCollInst, columns, exRellTypeID);
                Parent2.Name = str1;
                this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_441"), (object) str2, (object) Parent2.ArticleID));
                this._relations.Add(new VirtualRelation(Parent.Guid, Parent2.Guid, quantity2));
                if (Parent2.ExemplarID == 0L)
                  this.GetTreeNode(session, Parent2, relationColl, dbParams, rellCollInst, columns, exRellTypeID);
              }
              continue;
            default:
              continue;
          }
        }
      }
      else
      {
        this.AddMessage(string.Format(LocalizationHolder.rm.GetString("Pdm_442"), (object) str2));
        this._errorPresent = true;
      }
    }
  }
}
