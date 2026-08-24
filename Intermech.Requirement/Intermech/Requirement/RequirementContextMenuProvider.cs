// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.RequirementContextMenuProvider
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using Intermech.Client.Core;
using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Kernel.Search;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Requirement.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Requirement;

public class RequirementContextMenuProvider : ICommandsProvider
{
  private string _fileFullName = string.Empty;
  private List<NodeTreeFromWord> _list = new List<NodeTreeFromWord>();

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    IViewState service = (IViewState) viewServices.GetService(typeof (IViewState));
    if (service == null || (service.ViewState & ViewStateFlags.ReadOnly) != ViewStateFlags.None || items.Count != 1)
      return CommandsInfo.Empty;
    DataTable dataTable = (DataTable) null;
    CommandsInfo groupCommands = new CommandsInfo();
    groupCommands.Add("CreateObjectTreeReq", new CommandInfo(0, new ClickEventHandler(this.CreateTechnicalRequirement)));
    long projId = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (ICompositionLoadService)) is ICompositionLoadService customService)
      {
        int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(MetaDataHelper.GetObjectTypeID(RequirementConst.TechnicalRequirementGuid));
        List<ColumnDescriptor> columns = new List<ColumnDescriptor>()
        {
          new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
          new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid(RequirementConst.AttrNameRequirementString)), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
        };
        dataTable = customService.LoadComposition((object) sessionKeeper.Session.SessionGUID, projId, defaultRelationTypeId, (IEnumerable<ColumnDescriptor>) columns, string.Empty);
      }
    }
    if (dataTable != null && dataTable.Rows.Count > 0)
      groupCommands.Add("CheckTZforCompleted", new CommandInfo(0, new ClickEventHandler(this.CheckTZForCompleted)));
    return groupCommands;
  }

  private void CheckTZForCompleted(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    long objectId = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICompositionLoadService customService = sessionKeeper.Session.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
      IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
      if (customService == null || service == null)
        return;
      int objectTypeId1 = MetaDataHelper.GetObjectTypeID(RequirementConst.TechnicalRequirementGuid);
      List<int> searchRelationTypes = new List<int>()
      {
        MetaDataHelper.GetDefaultRelationTypeID(objectTypeId1)
      };
      int objectTypeId2 = MetaDataHelper.GetObjectTypeID(RequirementConst.SpecificationGuid);
      IDBObjectTypeID itemData = items.GetItemData(0, typeof (IDBObjectTypeID)) as IDBObjectTypeID;
      List<ObjInfoItem> objects = new List<ObjInfoItem>()
      {
        new ObjInfoItem(objectId, itemData != null ? itemData.Value : objectTypeId2)
      };
      List<int> searchObjectTypes = new List<int>()
      {
        objectTypeId1
      };
      List<ColumnDescriptor> columns = new List<ColumnDescriptor>()
      {
        new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -50),
        new ColumnDescriptor((object) -4)
      };
      DataTable dataTable = customService.LoadComplexCompositions((object) sessionKeeper.Session.SessionGUID, (IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) searchRelationTypes, (IEnumerable<int>) searchObjectTypes, (IEnumerable<ColumnDescriptor>) columns, true, false, (VersionsRule) null, (IEnumerable<ConditionStructure>) null, service.FiltrationServiceOwnerID, (Dictionary<long, HybridDictionary>) null, -1);
      IDBLifecycleStep lifecycleStep = sessionKeeper.Session.GetLifecycleStep(new Guid(RequirementConst.Completed));
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        List<object> notCompletedList = new List<object>();
        bool flag = false;
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          if (Convert.ToInt32(dataTable.Rows[index].ItemArray[2]) != lifecycleStep.LCStep)
          {
            notCompletedList.Add(dataTable.Rows[index].ItemArray[1]);
            flag = true;
          }
        }
        if (flag)
        {
          int num1 = (int) new TZNotCompletedForms(notCompletedList).ShowDialog();
        }
        else
        {
          int num2 = (int) MessageBox.Show(Resources.TZCompleted);
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Не удалось найти технические требования для проверки выполнения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }

  public void SaveChanges_Before(object sender, BeforeObjectCommandArgs e)
  {
    IMSLifeCycleStep imsLifeCycleStep1 = (IMSLifeCycleStep) null;
    IMSLifeCycleStep imsLifeCycleStep2 = (IMSLifeCycleStep) null;
    IMSLifeCycleStep imsLifeCycleStep3 = (IMSLifeCycleStep) null;
    this._fileFullName = string.Empty;
    this._list = new List<NodeTreeFromWord>();
    string text = string.Empty;
    bool flag = false;
    RequirementCreatedForm requirementCreatedForm = new RequirementCreatedForm();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(e.ObjectId);
      if (objectInfo.Empty)
        throw new KernelException($"Объект с идентификатором '{e.ObjectId}' не найден.");
      int objectTypeId = MetaDataHelper.GetObjectTypeID(RequirementConst.SpecificationGuid);
      if (objectInfo.ObjectTypeID == objectTypeId)
      {
        try
        {
          text = this.CheckObjectAndShowRequirementForm(sessionKeeper.Session, e.ObjectId);
        }
        catch (Exception ex)
        {
          text = ex.Message;
        }
        imsLifeCycleStep1 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.NotCompleted));
        imsLifeCycleStep2 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.InWork));
        imsLifeCycleStep3 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.Completed));
      }
      if (text == null)
      {
        if (!string.IsNullOrEmpty(this._fileFullName))
        {
          requirementCreatedForm = new RequirementCreatedForm(this._fileFullName, this._list);
          if (Statics.IconSrv != null)
          {
            Icon iconEx1 = Statics.IconSrv.GetIconEx(8, imsLifeCycleStep1.LevelID);
            Icon iconEx2 = Statics.IconSrv.GetIconEx(8, imsLifeCycleStep2.LevelID);
            Icon iconEx3 = Statics.IconSrv.GetIconEx(8, imsLifeCycleStep3.LevelID);
            requirementCreatedForm.imgStatusList.Images.Add(iconEx1);
            requirementCreatedForm.imgStatusList.Images.Add(iconEx2);
            requirementCreatedForm.imgStatusList.Images.Add(iconEx3);
          }
          flag = true;
        }
        else
        {
          flag = false;
          requirementCreatedForm.Dispose();
        }
      }
      else if (!string.IsNullOrEmpty(text))
      {
        int num = (int) MessageBox.Show(text);
        flag = false;
        requirementCreatedForm.Dispose();
      }
    }
    if (!flag)
      return;
    if (requirementCreatedForm.ShowDialog() == DialogResult.OK && RequirementConst.CheckFormResult)
    {
      this.CreateObjectsTree(this._fileFullName);
    }
    else
    {
      this._list.Clear();
      requirementCreatedForm.Dispose();
    }
  }

  private void CreateTechnicalRequirement(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    this._list = new List<NodeTreeFromWord>();
    this._fileFullName = string.Empty;
    string empty = string.Empty;
    bool flag = false;
    RequirementCreatedForm requirementCreatedForm = new RequirementCreatedForm();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string text;
      try
      {
        long objectId = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
        text = this.CheckObjectAndShowRequirementForm(sessionKeeper.Session, objectId);
      }
      catch (Exception ex)
      {
        text = ex.Message;
      }
      IMSLifeCycleStep lcStep1 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.NotCompleted));
      IMSLifeCycleStep lcStep2 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.InWork));
      IMSLifeCycleStep lcStep3 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.Completed));
      if (text == null)
      {
        if (!string.IsNullOrEmpty(this._fileFullName))
        {
          requirementCreatedForm = new RequirementCreatedForm(this._fileFullName, this._list);
          if (Statics.IconSrv != null)
          {
            Icon iconEx1 = Statics.IconSrv.GetIconEx(8, lcStep1.LevelID);
            Icon iconEx2 = Statics.IconSrv.GetIconEx(8, lcStep2.LevelID);
            Icon iconEx3 = Statics.IconSrv.GetIconEx(8, lcStep3.LevelID);
            requirementCreatedForm.imgStatusList.Images.Add(iconEx1);
            requirementCreatedForm.imgStatusList.Images.Add(iconEx2);
            requirementCreatedForm.imgStatusList.Images.Add(iconEx3);
          }
          flag = true;
        }
        else
        {
          int num = (int) MessageBox.Show(Resources.FileNotFound);
          flag = false;
          requirementCreatedForm.Dispose();
        }
      }
      else if (!string.IsNullOrEmpty(text))
      {
        int num = (int) MessageBox.Show(text);
        flag = false;
        requirementCreatedForm.Dispose();
      }
    }
    if (!flag)
      return;
    if (requirementCreatedForm.ShowDialog() == DialogResult.OK && RequirementConst.CheckFormResult)
    {
      this.CreateObjectsTree(this._fileFullName);
    }
    else
    {
      this._list.Clear();
      requirementCreatedForm.Dispose();
    }
  }

  private string CheckObjectAndShowRequirementForm(IUserSession session, long objectId)
  {
    this._list.Clear();
    IDBObject dbObject = session.GetObject(objectId, true);
    if (dbObject.CheckoutBy == session.UserID)
    {
      RequirementConst.SpecificationID = dbObject.ObjectID;
      ICompositionLoadService customService = session.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
      IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
      List<ColumnDescriptor> columns1 = new List<ColumnDescriptor>();
      List<ColumnDescriptor> columns2 = new List<ColumnDescriptor>();
      int objectTypeId = MetaDataHelper.GetObjectTypeID(RequirementConst.TechnicalRequirementGuid);
      List<int> searchRelationTypes = new List<int>();
      int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(objectTypeId);
      searchRelationTypes.Add(defaultRelationTypeId);
      List<ObjInfoItem> objects = new List<ObjInfoItem>()
      {
        new ObjInfoItem(dbObject.ObjectID, dbObject.TypeID)
      };
      List<int> searchObjectTypes = new List<int>()
      {
        objectTypeId
      };
      columns2.Add(new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      columns2.Add(new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid(RequirementConst.AttrNameRequirementString)), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      DataTable dataTable = customService.LoadComposition((object) session.SessionGUID, dbObject.ObjectID, defaultRelationTypeId, (IEnumerable<ColumnDescriptor>) columns2, service.FiltrationServiceOwnerID);
      columns1.Add(new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid(RequirementConst.AttrNameRequirementString)), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      columns1.Add(new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid(RequirementConst.AttrLinesText)), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      columns1.Add(new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid(RequirementConst.AttrContents)), AttributeSourceTypes.Object, ColumnContents.String, ColumnNameMapping.ID, SortOrders.NONE, 0));
      columns1.Add(new ColumnDescriptor((object) -21));
      columns1.Add(new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid(RequirementConst.AttrIndexRequirementString)), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      columns1.Add(new ColumnDescriptor((object) -4, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      DataTable table = dataTable == null || dataTable.Rows.Count == 0 ? new DataTable("No Composition") : customService.LoadComplexCompositions((object) session.SessionGUID, (IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) searchRelationTypes, (IEnumerable<int>) searchObjectTypes, (IEnumerable<ColumnDescriptor>) columns1, true, false, (VersionsRule) null, (IEnumerable<ConditionStructure>) null, service.FiltrationServiceOwnerID, (Dictionary<long, HybridDictionary>) null, -1);
      long blobId;
      if (table != null && table.Rows.Count > 0)
      {
        string str = $"{table.Columns[3]} ASC";
        DataView rows = new DataView(table) { Sort = str };
        for (int index = 0; index < rows.Count; ++index)
        {
          if (rows[index].Row.ItemArray[3].ToString().StartsWith("-"))
            this._list.Add(this.GenerateObjectsTreeUsingParentID(rows, index));
        }
        IDBAttribute[] attributesByType = dbObject.Attributes.GetAttributesByType(FieldTypes.ftFile);
        if (attributesByType.Length == 0)
          return Resources.AttributeFileNotFound;
        MemoryStream aDestStream = new MemoryStream();
        BlobProcReader blobProcReader = new BlobProcReader(attributesByType[0], 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null);
        blobProcReader.ReadData();
        blobId = blobProcReader.BlobInformation.BlobID;
        PublishedFile publishedFile = ClientContext.FileVault.ViewArea.Publish((IList<DBObjectState>) ClientContext.FileVault.DBObjectsInfo.CreateStateListForObjectTree(objectId, VersionsRuleSources.GetCurrentWindowRule())).ObjectFiles.Find((Predicate<PublishedFile>) (viewFile => viewFile.BlobId == blobId));
        if (publishedFile != null)
          this._fileFullName = publishedFile.FullName;
        return (string) null;
      }
      IDBAttribute[] attributesByType1 = dbObject.Attributes.GetAttributesByType(FieldTypes.ftFile);
      if (attributesByType1.Length == 0)
        return Resources.AttributeFileNotFound;
      MemoryStream aDestStream1 = new MemoryStream();
      BlobProcReader blobProcReader1 = new BlobProcReader(attributesByType1[0], 0, (Stream) aDestStream1, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null);
      blobProcReader1.ReadData();
      blobId = blobProcReader1.BlobInformation.BlobID;
      PublishedFile publishedFile1 = ClientContext.FileVault.ViewArea.Publish((IList<DBObjectState>) ClientContext.FileVault.DBObjectsInfo.CreateStateListForObjectTree(objectId, VersionsRuleSources.GetCurrentWindowRule())).ObjectFiles.Find((Predicate<PublishedFile>) (viewFile => viewFile.BlobId == blobId));
      if (publishedFile1 != null)
        this._fileFullName = publishedFile1.FullName;
      this._list.Clear();
      return (string) null;
    }
    return dbObject.CheckoutBy == 0L ? Resources.NotCheckout : Resources.ErrorUser;
  }

  private NodeTreeFromWord GenerateObjectsTreeUsingParentID(DataView rows, int i)
  {
    IMSLifeCycleStep lcStep1 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.NotCompleted));
    IMSLifeCycleStep lcStep2 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.InWork));
    IMSLifeCycleStep lcStep3 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.Completed));
    int int32 = Convert.ToInt32(rows[i].Row.ItemArray[5]);
    NodeTreeFromWord parent = new NodeTreeFromWord()
    {
      Name = rows[i].Row.ItemArray[0].ToString(),
      Child = new List<NodeTreeFromWord>(),
      TTLines = rows[i].Row.ItemArray[1].ToString(),
      TTDescription = rows[i].Row.ItemArray[2].ToString(),
      TTParentID = rows[i].Row.ItemArray[3].ToString(),
      TTIndexInDocument = rows[i].Row.ItemArray[4].ToString(),
      TTLevelHierarhi = rows[i].Row.ItemArray[1].ToString(),
      TTObjectID = rows[i].Row.ItemArray[6].ToString(),
      Parent = (NodeTreeFromWord) null
    };
    if (lcStep1.LCStepID == int32)
    {
      parent.IconIndex = 0;
      parent.TTLCStep = 0;
    }
    else if (lcStep2.LCStepID == int32)
    {
      parent.IconIndex = 1;
      parent.TTLCStep = 1;
    }
    else if (lcStep3.LCStepID == int32)
    {
      parent.IconIndex = 2;
      parent.TTLCStep = 2;
    }
    List<NodeTreeFromWord> nodeTreeFromWordList = this.ChildObjectsGenerate(rows, i + 1, rows[i].Row.ItemArray[0].ToString(), rows[i].Row.ItemArray[6].ToString(), parent);
    if (nodeTreeFromWordList.Count > 0)
      parent.Child = nodeTreeFromWordList;
    return parent;
  }

  private List<NodeTreeFromWord> ChildObjectsGenerate(
    DataView rows,
    int i,
    string parentName,
    string parentID,
    NodeTreeFromWord parent)
  {
    List<NodeTreeFromWord> nodeTreeFromWordList1 = new List<NodeTreeFromWord>();
    IMSLifeCycleStep lcStep1 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.NotCompleted));
    IMSLifeCycleStep lcStep2 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.InWork));
    IMSLifeCycleStep lcStep3 = MetaDataHelper.GetLCStep(new Guid(RequirementConst.Completed));
    for (int recordIndex = i; recordIndex < rows.Count; ++recordIndex)
    {
      if (rows[recordIndex].Row.ItemArray[3].ToString() == parentID)
      {
        NodeTreeFromWord parent1 = new NodeTreeFromWord()
        {
          Name = rows[recordIndex].Row.ItemArray[0].ToString(),
          Child = new List<NodeTreeFromWord>(),
          TTLines = rows[recordIndex].Row.ItemArray[1].ToString(),
          TTDescription = rows[recordIndex].Row.ItemArray[2].ToString(),
          TTParentID = rows[recordIndex].Row.ItemArray[3].ToString(),
          TTIndexInDocument = rows[recordIndex].Row.ItemArray[4].ToString(),
          TTLevelHierarhi = rows[recordIndex].Row.ItemArray[1].ToString(),
          TTObjectID = rows[recordIndex].Row.ItemArray[6].ToString(),
          Parent = parent
        };
        int int32 = Convert.ToInt32(rows[recordIndex].Row.ItemArray[5]);
        if (lcStep1.LCStepID == int32)
        {
          parent1.IconIndex = 0;
          parent1.TTLCStep = 0;
        }
        else if (lcStep2.LCStepID == int32)
        {
          parent1.IconIndex = 1;
          parent1.TTLCStep = 1;
        }
        else if (lcStep3.LCStepID == int32)
        {
          parent1.IconIndex = 2;
          parent1.TTLCStep = 2;
        }
        List<NodeTreeFromWord> nodeTreeFromWordList2 = this.ChildObjectsGenerate(rows, recordIndex + 1, rows[recordIndex].Row.ItemArray[0].ToString(), rows[recordIndex].Row.ItemArray[6].ToString(), parent1);
        if (nodeTreeFromWordList2.Count > 0)
          parent1.Child = nodeTreeFromWordList2;
        nodeTreeFromWordList1.Add(parent1);
      }
    }
    return nodeTreeFromWordList1;
  }

  private void CreateObjectsTree(string fileName)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      int objectTypeId = MetaDataHelper.GetObjectTypeID(RequirementConst.TechnicalRequirementGuid);
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(objectTypeId);
      int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(objectTypeId);
      IDBTransactions customService = sessionKeeper.Session.GetCustomService(typeof (IDBTransactions)) as IDBTransactions;
      try
      {
        customService?.StartTransaction();
        this.GenerateObjects(fileName, defaultRelationTypeId, objectCollection, sessionKeeper.Session);
        customService?.Commit();
      }
      catch
      {
        customService?.Rollback();
        throw;
      }
    }
  }

  private void GenerateObjects(
    string fileName,
    int relTypeID,
    IDBObjectCollection objects,
    IUserSession session)
  {
    int itterator = 1;
    IDBRelationCollection relationCollection = session.GetRelationCollection(relTypeID);
    INotificationService service = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (!RequirementConst.IsHaveCompisition)
    {
      foreach (NodeTreeFromWord nodeTreeFromWord in RequirementConst.WordTT)
      {
        if (nodeTreeFromWord.IsChecked)
        {
          IDBObject dbObject = objects.Create();
          IDBRelation dbRelation = relationCollection.Create(RequirementConst.SpecificationID, dbObject.ObjectID);
          string str = nodeTreeFromWord.Name.Length > 450 ? nodeTreeFromWord.Name.Substring(0, 450) : nodeTreeFromWord.Name;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrNameRequirementString), false).AsString = str;
          dbObject.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString = str;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrIndexRequirementString), false).AsString = nodeTreeFromWord.TTIndexInDocument;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrRezult), false);
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.OtherFilesGuid), false);
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrContents), false).AsString = nodeTreeFromWord.TTDescription;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.LevelTTGuid), false).AsString = nodeTreeFromWord.TTLevel;
          if (MetaDataHelper.GetObjectType(dbObject.ObjectType).CaptionAttribute <= 0)
            dbObject.Caption = nodeTreeFromWord.Name;
          dbObject.Attributes.FindByGUID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString = $"{Path.GetFileNameWithoutExtension(fileName)} / {itterator:000}";
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrLinesText), false).AsString = nodeTreeFromWord.TTLevelHierarhi;
          dbObject.CommitCreation(true);
          DBObjectsEventArgs e1 = new DBObjectsEventArgs("ObjectsCreated", dbObject.ObjectID);
          DBRelationsEventArgs e2 = new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID);
          service.FireEvent((object) null, (NotificationEventArgs) e2);
          service.FireEvent((object) null, (NotificationEventArgs) e1);
          ++itterator;
          if (nodeTreeFromWord.Child.Count > 0)
            itterator = this.GenerateObjectNextLevel(dbObject.ObjectID, itterator, nodeTreeFromWord.Child, session, objects, relationCollection, service, fileName);
        }
      }
    }
    else
    {
      if (RequirementConst.NodesList.Count > 0)
      {
        try
        {
          foreach (NodeTreeFromWord nodes in RequirementConst.NodesList)
          {
            if (nodes.IsNew)
            {
              if (nodes.IsChecked)
              {
                IDBObject dbObject = objects.Create();
                IDBRelation dbRelation = relationCollection.Create(RequirementConst.SpecificationID, dbObject.ObjectID);
                IDBAttribute dbAttribute = dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrNameRequirementString), false);
                string str1 = nodes.Name.Length > 450 ? nodes.Name.Substring(0, 450) : nodes.Name;
                string str2 = str1;
                dbAttribute.AsString = str2;
                dbObject.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString = str1;
                dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrIndexRequirementString), false).AsString = nodes.TTIndexInDocument;
                dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrRezult), false);
                dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.OtherFilesGuid), false);
                dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrContents), false).AsString = nodes.TTDescription;
                dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.LevelTTGuid), false).AsString = nodes.TTLevel;
                if (MetaDataHelper.GetObjectType(dbObject.ObjectType).CaptionAttribute <= 0)
                  dbObject.Caption = nodes.Name;
                dbObject.Attributes.FindByGUID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString = $"{Path.GetFileNameWithoutExtension(fileName)} / {itterator:000}";
                dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrLinesText), false).AsString = nodes.TTLevelHierarhi;
                dbObject.CommitCreation(true);
                DBObjectsEventArgs e3 = new DBObjectsEventArgs("ObjectsCreated", dbObject.ObjectID);
                DBRelationsEventArgs e4 = new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID);
                service.FireEvent((object) null, (NotificationEventArgs) e4);
                service.FireEvent((object) null, (NotificationEventArgs) e3);
                ++itterator;
                nodes.TTObjectID = dbObject.ObjectID.ToString();
                if (nodes.Child.Count > 0)
                  itterator = this.GenerateObjectNextLevelFromDouble(itterator, nodes.Child, session, objects, relationCollection, service, fileName);
              }
            }
            else
            {
              IDBObject dbObject = session.GetObject(Convert.ToInt64(nodes.TTObjectID));
              IDBRelation relation = session.GetRelation(Convert.ToInt64(nodes.OldNode.TTParentID), dbObject.ID);
              if (relation == null)
                throw new KernelException($"Связь между объектами '{nodes.OldNode.ParentName}[{nodes.OldNode.TTParentID}]' и '{dbObject.Caption}[{dbObject.ID}]' не найдена.");
              relation.ProjID = Convert.ToInt64(nodes.TTParentID ?? nodes.Parent.TTObjectID);
              itterator = this.RenameDBObject(session, nodes, itterator, objects, relationCollection, service, fileName);
              DBRelationsEventArgs e5 = new DBRelationsEventArgs("RelationsRemoved", relation.RelationID);
              DBRelationsEventArgs e6 = new DBRelationsEventArgs("RelationsCreated", relation.RelationID);
              service.FireEvent((object) null, (NotificationEventArgs) e5);
              service.FireEvent((object) null, (NotificationEventArgs) e6);
            }
          }
        }
        catch (Exception ex)
        {
          throw new KernelException(ex.Message, ex.InnerException);
        }
      }
      if (!(session.GetCustomService(typeof (IObjectsDeleteService)) is IObjectsDeleteService customService))
        return;
      DeletingObjects deletingObjects = new DeletingObjects();
      for (int index = 0; index < RequirementConst.DeletedObject.Count; ++index)
      {
        IDBObject dbObject = session.GetObject(Convert.ToInt64(RequirementConst.DeletedObject[index].DeletedTTID));
        deletingObjects.Add(0L, dbObject.ID, dbObject.ObjectID, true);
        DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsRemoved", dbObject.ObjectID);
        service.FireEvent((object) null, (NotificationEventArgs) e);
      }
      customService.Delete(session.SessionGUID, deletingObjects, DeleteObjectsJobMode.AscOnError);
    }
  }

  private int GenerateObjectNextLevelFromDouble(
    int itterator,
    List<NodeTreeFromWord> list,
    IUserSession session,
    IDBObjectCollection objects,
    IDBRelationCollection idbRelationColection,
    INotificationService notificationService,
    string fileName)
  {
    foreach (NodeTreeFromWord nodeTreeFromWord in list)
    {
      if (nodeTreeFromWord.IsNew)
      {
        if (nodeTreeFromWord.IsChecked)
        {
          IDBObject dbObject = objects.Create();
          IDBRelation dbRelation = idbRelationColection.Create(Convert.ToInt64(nodeTreeFromWord.Parent.TTObjectID), dbObject.ObjectID);
          IDBAttribute dbAttribute = dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrNameRequirementString), false);
          string str1 = nodeTreeFromWord.Name.Length > 450 ? nodeTreeFromWord.Name.Substring(0, 450) : nodeTreeFromWord.Name;
          string str2 = str1;
          dbAttribute.AsString = str2;
          dbObject.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString = str1;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrIndexRequirementString), false).AsString = nodeTreeFromWord.TTIndexInDocument;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrRezult), false);
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.OtherFilesGuid), false);
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrContents), false).AsString = nodeTreeFromWord.TTDescription;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.LevelTTGuid), false).AsString = nodeTreeFromWord.TTLevel;
          if (MetaDataHelper.GetObjectType(dbObject.ObjectType).CaptionAttribute <= 0)
            dbObject.Caption = nodeTreeFromWord.Name;
          dbObject.Attributes.FindByGUID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString = $"{Path.GetFileNameWithoutExtension(fileName)} / {itterator:000}";
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrLinesText), false).AsString = nodeTreeFromWord.TTLevelHierarhi;
          dbObject.CommitCreation(true);
          DBObjectsEventArgs e1 = new DBObjectsEventArgs("ObjectsCreated", dbObject.ObjectID);
          DBRelationsEventArgs e2 = new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID);
          notificationService.FireEvent((object) null, (NotificationEventArgs) e2);
          notificationService.FireEvent((object) null, (NotificationEventArgs) e1);
          ++itterator;
          nodeTreeFromWord.TTObjectID = dbObject.ObjectID.ToString();
          if (nodeTreeFromWord.Child.Count > 0)
            itterator = this.GenerateObjectNextLevelFromDouble(itterator, nodeTreeFromWord.Child, session, objects, idbRelationColection, notificationService, fileName);
        }
      }
      else
      {
        IDBObject dbObject = session.GetObject(Convert.ToInt64(nodeTreeFromWord.TTObjectID));
        IDBRelation relation = session.GetRelation(Convert.ToInt64(nodeTreeFromWord.OldNode.TTParentID), dbObject.ID);
        if (relation == null)
          throw new KernelException($"Связь между объектами '{nodeTreeFromWord.OldNode.ParentName}[{nodeTreeFromWord.OldNode.TTParentID}]' и '{dbObject.Caption}[{dbObject.ID}]' не найдена.");
        relation.ProjID = Convert.ToInt64(nodeTreeFromWord.TTParentID ?? nodeTreeFromWord.Parent.TTObjectID);
        itterator = this.RenameDBObject(session, nodeTreeFromWord, itterator, objects, idbRelationColection, notificationService, fileName);
        DBRelationsEventArgs e3 = new DBRelationsEventArgs("RelationsRemoved", relation.RelationID);
        DBRelationsEventArgs e4 = new DBRelationsEventArgs("RelationsCreated", relation.RelationID);
        notificationService.FireEvent((object) null, (NotificationEventArgs) e3);
        notificationService.FireEvent((object) null, (NotificationEventArgs) e4);
      }
    }
    return itterator;
  }

  private int GenerateObjectNextLevel(
    long parent,
    int itterator,
    List<NodeTreeFromWord> nodes,
    IUserSession session,
    IDBObjectCollection objects,
    IDBRelationCollection idbRelationColection,
    INotificationService notificationService,
    string fileName)
  {
    foreach (NodeTreeFromWord node in nodes)
    {
      if (node.IsChecked)
      {
        IDBObject dbObject = objects.Create();
        IDBRelation dbRelation = idbRelationColection.Create(parent, dbObject.ObjectID);
        IDBAttribute dbAttribute = dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrNameRequirementString), false);
        string str1 = node.Name.Length > 450 ? node.Name.Substring(0, 450) : node.Name;
        string str2 = str1;
        dbAttribute.AsString = str2;
        dbObject.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString = str1;
        dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrIndexRequirementString), false).AsString = node.TTIndexInDocument;
        dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrRezult), false);
        dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.OtherFilesGuid), false);
        dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrContents), false).AsString = node.TTDescription;
        dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.LevelTTGuid), false).AsString = node.TTLevel;
        if (MetaDataHelper.GetObjectType(dbObject.ObjectType).CaptionAttribute <= 0)
          dbObject.Caption = node.Name;
        dbObject.Attributes.FindByGUID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString = $"{Path.GetFileNameWithoutExtension(fileName)} / {itterator:000}";
        dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrLinesText), false).AsString = node.TTLevelHierarhi;
        dbObject.CommitCreation(true);
        DBObjectsEventArgs e1 = new DBObjectsEventArgs("ObjectsCreated", dbObject.ObjectID);
        DBRelationsEventArgs e2 = new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID);
        notificationService.FireEvent((object) null, (NotificationEventArgs) e2);
        notificationService.FireEvent((object) null, (NotificationEventArgs) e1);
        ++itterator;
        if (node.Child.Count > 0)
          itterator = this.GenerateObjectNextLevel(dbObject.ObjectID, itterator, node.Child, session, objects, idbRelationColection, notificationService, fileName);
      }
    }
    return itterator;
  }

  private int RenameDBObject(
    IUserSession session,
    NodeTreeFromWord obj,
    int itterator,
    IDBObjectCollection objects,
    IDBRelationCollection idbRelationColection,
    INotificationService notificationService,
    string fileName)
  {
    IDBObject dbObject = session.GetObject(Convert.ToInt64(obj.TTObjectID));
    if (Convert.ToInt64(obj.TTObjectID) > 0L)
      dbObject = dbObject.CheckOut();
    IDBAttribute byGuid = dbObject.Attributes.FindByGUID(new Guid(RequirementConst.AttrNameRequirementString));
    string str1 = obj.Name.Length > 450 ? obj.Name.Substring(0, 450) : obj.Name;
    string str2 = str1;
    byGuid.AsString = str2;
    dbObject.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString = str1;
    dbObject.Attributes.FindByGUID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString = $"{Path.GetFileNameWithoutExtension(fileName)} / {itterator:000}";
    dbObject.Attributes.FindByGUID(new Guid(RequirementConst.AttrContents)).AsString = obj.TTDescription;
    dbObject.Attributes.FindByGUID(new Guid(RequirementConst.AttrLinesText)).AsString = obj.TTLevelHierarhi;
    dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.LevelTTGuid), false).AsString = obj.TTLevel;
    dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrIndexRequirementString), false).AsString = obj.TTIndexInDocument;
    dbObject.SaveChanges();
    if (Convert.ToInt64(obj.TTObjectID) > 0L)
      dbObject.CheckIn();
    ++itterator;
    DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsChanged", dbObject.ObjectID);
    notificationService?.FireEvent((object) null, (NotificationEventArgs) e);
    if (obj.IsHaveChild)
      itterator = this.RenameChild(obj.Child, itterator, session, objects, idbRelationColection, notificationService, fileName);
    return itterator;
  }

  private int RenameChild(
    List<NodeTreeFromWord> childList,
    int itterator,
    IUserSession session,
    IDBObjectCollection objects,
    IDBRelationCollection idbRelationColection,
    INotificationService notificationService,
    string fileName)
  {
    foreach (NodeTreeFromWord child in childList)
    {
      if (child.IsNew)
      {
        if (child.IsChecked)
        {
          IDBObject dbObject = objects.Create();
          IDBRelation dbRelation = idbRelationColection.Create(Convert.ToInt64(child.Parent.TTObjectID), dbObject.ObjectID);
          IDBAttribute dbAttribute = dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrNameRequirementString), false);
          string str1 = child.Name.Length > 450 ? child.Name.Substring(0, 450) : child.Name;
          string str2 = str1;
          dbAttribute.AsString = str2;
          dbObject.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString = str1;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrIndexRequirementString), false).AsString = child.TTIndexInDocument;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrRezult), false);
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.OtherFilesGuid), false);
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrContents), false).AsString = child.TTDescription;
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.LevelTTGuid), false).AsString = child.TTLevel;
          if (MetaDataHelper.GetObjectType(dbObject.ObjectType).CaptionAttribute <= 0)
            dbObject.Caption = child.Name;
          dbObject.Attributes.FindByGUID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString = $"{Path.GetFileNameWithoutExtension(fileName)} / {itterator:000}";
          dbObject.Attributes.AddAttribute(session.IdentHelper.GetAttributeID(RequirementConst.AttrLinesText), false).AsString = child.TTLevelHierarhi;
          dbObject.CommitCreation(true);
          DBObjectsEventArgs e1 = new DBObjectsEventArgs("ObjectsCreated", dbObject.ObjectID);
          DBRelationsEventArgs e2 = new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID);
          notificationService.FireEvent((object) null, (NotificationEventArgs) e2);
          notificationService.FireEvent((object) null, (NotificationEventArgs) e1);
          ++itterator;
          child.TTObjectID = dbObject.ObjectID.ToString();
          if (child.Child.Count > 0)
            itterator = this.GenerateObjectNextLevelFromDouble(itterator, child.Child, session, objects, idbRelationColection, notificationService, fileName);
        }
      }
      else
      {
        IDBObject dbObject = session.GetObject(Convert.ToInt64(child.TTObjectID));
        IDBRelation relation = session.GetRelation(Convert.ToInt64(child.OldNode.TTParentID), dbObject.ID);
        if (relation == null)
          throw new KernelException($"Связь между объектами '{child.OldNode.ParentName}[{child.OldNode.TTParentID}]' и '{dbObject.Caption}[{dbObject.ID}]' не найдена.");
        relation.ProjID = Convert.ToInt64(child.TTParentID ?? child.Parent.TTObjectID);
        itterator = this.RenameDBObject(session, child, itterator, objects, idbRelationColection, notificationService, fileName);
        DBRelationsEventArgs e3 = new DBRelationsEventArgs("RelationsRemoved", relation.RelationID);
        DBRelationsEventArgs e4 = new DBRelationsEventArgs("RelationsCreated", relation.RelationID);
        notificationService.FireEvent((object) null, (NotificationEventArgs) e3);
        notificationService.FireEvent((object) null, (NotificationEventArgs) e4);
      }
    }
    return itterator;
  }
}
