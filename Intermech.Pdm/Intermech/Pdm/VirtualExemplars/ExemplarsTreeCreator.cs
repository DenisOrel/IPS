// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.ExemplarsTreeCreator
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Pdm.VirtualExemplars.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class ExemplarsTreeCreator
{
  private List<NotificationEventArgs> _notifAfterCreateTree;
  private long _rootArticleID;
  private CreateExemplarForm _treeForm;
  private VirtualGraph _graph;
  public bool TreePresent;
  private string OwnerID = "cad001e2-306c-11d8-b4e9-00304f19f545";

  public List<NotificationEventArgs> NotifAfterCreateTree => this._notifAfterCreateTree;

  public void RollbackTree()
  {
    if (this._graph == null)
      return;
    this._graph.RollbackTree();
  }

  public ExemplarsTreeCreator(long rootArticleID) => this._rootArticleID = rootArticleID;

  public bool CheckTree(SessionKeeper keeper)
  {
    using (ContextAndRuleForm contextAndRuleForm = new ContextAndRuleForm())
    {
      if (contextAndRuleForm.ShowDialog() != DialogResult.OK)
        return false;
    }
    bool flag = false;
    IDBRelationCollection relationCollection = keeper.Session.GetRelationCollection(keeper.Session.IdentHelper.DocRelationTypeID);
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[1]
    {
      (object) -2
    });
    if (relationCollection.ConsistFrom(paramSet, this._rootArticleID).Rows.Count == 0)
    {
      if (keeper.Session.GetRelationCollection(keeper.Session.IdentHelper.SPRelationTypeID).ConsistFrom(paramSet, this._rootArticleID).Rows.Count > 0)
        flag = true;
    }
    else
      flag = true;
    if (flag)
      this.OwnerID = (ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService).FiltrationServiceOwnerID;
    this._treeForm = new CreateExemplarForm(LocalizationHolder.rm.GetString("Pdm_427"), LocalizationHolder.rm.GetString("Pdm_428"));
    Thread thread = new Thread(new ThreadStart(this.CheckTreeMethod));
    thread.IsBackground = true;
    thread.Name = "ArticlesTreeCheck";
    thread.Start();
    DialogResult dialogResult = this._treeForm.ShowDialog();
    thread.Abort();
    thread.Join();
    return dialogResult == DialogResult.OK;
  }

  private void CreateExemplar(
    IUserSession session,
    VirtualExemplar exemplar,
    int attrArticleID,
    int attrActivePartyID,
    int attributeVersionInRelationID,
    IDBRelationCollection docRellColl,
    DBRecordSetParams dbParams)
  {
    IDBObject dbObject1 = session.GetObjectCollection(exemplar.ExemplarObjectType).Create(exemplar.ArticleID);
    dbObject1.Attributes.AddAttribute(attrArticleID, false, new object[1]
    {
      (object) Math.Abs(exemplar.ArticleID)
    });
    if (exemplar.ArticlesInManufacture == ArticlesInManufacture.Parties && exemplar.SetActive)
      dbObject1.Attributes.AddAttribute(attrActivePartyID, false, new object[1]
      {
        (object) true
      });
    dbObject1.CommitCreation(true);
    IDBObject dbObject2 = dbObject1.CheckOut(false);
    exemplar.ExemplarID = dbObject2.ObjectID;
    IDBObjectType objectType = session.GetObjectType(exemplar.ExemplarObjectType);
    exemplar.ObjectsName = objectType.ObjectInstanceName;
    this._notifAfterCreateTree.Add((NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", exemplar.ExemplarID));
    foreach (DataRow row in (InternalDataCollectionBase) docRellColl.ConsistFrom(dbParams, exemplar.ArticleID).Rows)
    {
      IDBRelation dbRelation = docRellColl.Create(new NewRelationProperties(Convert.ToInt64(row[0]), dbObject2.ObjectID, Convert.ToInt64(row[1])));
      dbRelation.Attributes.AddAttribute(attributeVersionInRelationID, false, new object[1]
      {
        (object) Math.Abs(Convert.ToInt64(row[2]))
      });
      this._notifAfterCreateTree.Add((NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, dbRelation.RelationType));
    }
  }

  public void CreateTree(IUserSession session, long rootExemplarID)
  {
    this._notifAfterCreateTree = new List<NotificationEventArgs>();
    IDBRelationCollection relationCollection1 = session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PDMHelper.relationTypeInstances));
    IDBAttributeType attributeType1 = session.GetAttributeType(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    IDBAttributeType attributeType2 = session.GetAttributeType(new Guid("cad00622-306c-11d8-b4e9-00304f19f545"));
    IDBAttributeType attributeType3 = session.GetAttributeType(PDMHelper.attributeActiveParty);
    IDBAttributeType attributeType4 = session.GetAttributeType(new Guid("cad001c2-306c-11d8-b4e9-00304f19f545"));
    IDBRelationType relationType = session.GetRelationType(new Guid("cad00154-306c-11d8-b4e9-00304f19f545"));
    IDBRelationCollection relationCollection2 = session.GetRelationCollection(relationType.RelationType, this.OwnerID);
    DBRecordSetParams dbParams = new DBRecordSetParams((ConditionStructure[]) null, new object[3]
    {
      (object) -20,
      (object) -22,
      (object) -2
    });
    this._graph.Exemplars[0].ExemplarID = rootExemplarID;
    foreach (VirtualRelation relation in this._graph.Relations)
    {
      VirtualExemplar exemplar1 = this._graph.GetExemplar(relation.ParentExemplar);
      if (exemplar1.ExemplarID == 0L)
        this.CreateExemplar(session, exemplar1, attributeType2.AttributeID, attributeType3.AttributeID, attributeType4.AttributeID, relationCollection2, dbParams);
      VirtualExemplar exemplar2 = this._graph.GetExemplar(relation.ChildExemplar);
      if (exemplar2.ExemplarID == 0L)
        this.CreateExemplar(session, exemplar2, attributeType2.AttributeID, attributeType3.AttributeID, attributeType4.AttributeID, relationCollection2, dbParams);
      IDBRelation dbRelation = relationCollection1.Create(exemplar1.ExemplarID, exemplar2.ExemplarID);
      dbRelation.Attributes.AddAttribute(attributeType1.AttributeID, false, new object[1]
      {
        (object) relation.Quantity
      });
      this._notifAfterCreateTree.Add((NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, dbRelation.RelationType));
    }
  }

  private void _treeForm_CancelCreateEvent()
  {
  }

  private void _graph_AddStringEvent(string LineText) => this._treeForm.AddString(LineText);

  private void CheckTreeMethod()
  {
    while (!this._treeForm.Visible)
      Thread.Sleep(3);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._graph = new VirtualGraph();
      this._graph.AddStringEvent += new AddStringHandler(this._graph_AddStringEvent);
      this._graph.GetTree(sessionKeeper.Session, this._rootArticleID, this.OwnerID);
    }
    if (!this._graph.ErrorPresent)
      this._treeForm.OkEnable();
    if (this._graph.Relations.Count <= 0)
      return;
    this.TreePresent = true;
  }
}
