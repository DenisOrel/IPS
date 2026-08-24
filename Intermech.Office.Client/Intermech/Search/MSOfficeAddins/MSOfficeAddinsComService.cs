// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeAddins.MSOfficeAddinsComService
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Files;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Kernel.Search;
using Intermech.MaterialsHandbook;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Descriptos;
using Intermech.Navigator.Interfaces;
using Intermech.Runtime.ComInterop.LocalServer;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.MSOfficeAddins;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
[Guid("9CDE0F8F-6D7C-4649-8392-42D88E31793A")]
[ProgId("Intermech.Search.MSOfficeAddins.IMSOfficeAddinsComService")]
public sealed class MSOfficeAddinsComService : SingleThreadedObject
{
  public void CheckInDocument(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    ObjectCopyCommand checkinCommand = ObjectCommandFactory.CreateCheckinCommand(true);
    checkinCommand.ObjectId = this.GetDocumentVersionID(fileName);
    checkinCommand.UpdateUI = true;
    checkinCommand.Execute();
  }

  public void CheckOutDocument(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    if (!this.IsDocumentCheckedOut(fileName))
      this.CheckOutDocument(this.GetDocumentVersionID(fileName));
    this.ResetFileReadOnlyAttribute(fileName);
  }

  public Tuple<string, string>[] CreateObjectReference(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>();
    long documentVersionId = this.GetDocumentVersionID(fileName);
    int[] parentObjectTypeIds = (int[]) null;
    int[] allObjectTypeIds = (int[]) null;
    this.GetObjectTypesAllowableForAddToDocumentByReference(documentVersionId, out parentObjectTypeIds, out allObjectTypeIds);
    if (parentObjectTypeIds.Length == 0 || allObjectTypeIds.Length == 0)
      throw new Exception($"Ошибка создания ссылки, для объекта типа '{this.GetObjectType(documentVersionId).ObjectTypeName}' не назначено ни одной применяемости по связи '{MSOfficeAddinsConstants.ObjectsAddedByReferenceRelationTypeName}'");
    DescriptorCollection descriptors = new DescriptorCollection();
    ServiceContainer nodesContext = new ServiceContainer();
    nodesContext.AddService(typeof (IObjectTypeNodeFilter), (object) new ObjectTypeNodeFilter(allObjectTypeIds));
    ObjectTypesDescriptor objectTypesDescriptor = new ObjectTypesDescriptor(parentObjectTypeIds, "Допустимые типы объектов");
    descriptors.Add((IDescriptor) objectTypesDescriptor);
    if (ServicesManager.GetService(typeof (IImbaseSelector)) is IImbaseSelector service1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IImbaseServer service = ServiceUtils.GetService<IImbaseServer>((object) sessionKeeper.Session, false);
        if (service != null)
        {
          long[] catalogsForCreateType = service.GetCatalogsForCreateType(sessionKeeper.Session.SessionGUID, (object) parentObjectTypeIds, true);
          descriptors.Add(service1.GetRootDescriptor(catalogsForCreateType != null ? ((IEnumerable<long>) catalogsForCreateType).ToList<long>() : (List<long>) null));
        }
      }
    }
    if (ServicesManager.GetService(typeof (IIMHSelector)) is IIMHSelector service2)
      descriptors.Add(service2.GetMaterialsHandbookDescriptor());
    Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new MSOfficeAddinsComService.ObjectsForAddToDocumentByReferenceAnalyzer(allObjectTypeIds), true);
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects("Intermech Professional Solution", $"Выберите объекты для создания ссылки в объекте {this.GetObjectCaption(documentVersionId)}", (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor("Объекты", descriptors), (IServiceProvider) nodesContext, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect | SelectionOptions.ForceFilterObjectsByRule);
    if (numArray != null && numArray.Length != 0)
    {
      long num = numArray[0];
      if (service1 != null && !ObjectHelper.IsUnknownObjectVersionID(service1.ContextObjectId))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          num = (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) as IImbaseServer).CreateObject(sessionKeeper.Session.SessionGUID, 0L, service1.ContextObjectId, num, true, -1);
          if (ObjectHelper.IsUnknownObjectVersionID(num))
            throw new Exception("Не удалось создать ссылку на объект IPS Search. Объект по выбранной записи IPS IMBase (IPS IMH) не может быть создан.");
        }
      }
      tupleList.Add(new Tuple<string, string>(MSOfficeAddinsHelper.CreateObjectUrl(num), this.GetObjectCaption(num)));
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(num);
        int objectTypeId = MetaDataHelper.GetObjectTypeID(new Guid("cad00170-306c-11d8-b4e9-00304f19f545"));
        List<int> intList = new List<int>();
        intList.Add(objectTypeId);
        intList.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(objectTypeId));
        if (intList.Contains(dbObject.ObjectType))
        {
          int relationTypeId = MetaDataHelper.GetRelationTypeID(new Guid("cad00151-306c-11d8-b4e9-00304f19f545"));
          IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(relationTypeId);
          DBRecordSetParams dbRecordSetParams = new DBRecordSetParams();
          dbRecordSetParams.Columns = new object[3]
          {
            (object) ObligatoryObjectAttributes.F_OBJECT_ID,
            (object) ObligatoryObjectAttributes.CAPTION,
            (object) Constants.CountAttributeTypeID
          };
          // ISSUE: explicit reference operation
          (^ref dbRecordSetParams).Conditions = new ConditionStructure[1]
          {
            new ConditionStructure()
            {
              Attribute = (object) ObligatoryObjectAttributes.F_OBJECT_TYPE,
              RelationalOperator = RelationalOperators.In,
              Value = (object) intList.ToArray(),
              SQL = string.Empty
            }
          };
          DBRecordSetParams paramSet = dbRecordSetParams;
          DataTable dataTable = relationCollection.ConsistFrom(paramSet, num);
          if (dataTable.Rows.Count > 0)
          {
            if (MessageBox.Show($"В состав выбранного объекта типа {MetaDataHelper.GetObjectTypeName(dbObject.ObjectType)} связью {MetaDataHelper.GetRelationTypeName(relationTypeId)} входят другие объекты типа {MetaDataHelper.GetObjectTypeName(objectTypeId)}.{Environment.NewLine}Добавить ссылки на них в документ?", "Добавление ссылки", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
              bool flag = true;
              if (ServicesManager.GetService(typeof (IDBConfigurations)) is IDBConfigurations service3)
                flag = service3.ReadBool("MSOfficeAddins", "Core", "AddCount", true, DBConfigMode.GlobalOnly);
              foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
              {
                long int64Value = DataSetProcessor.GetInt64Value(row, 0, 0L);
                string stringValue = DataSetProcessor.GetStringValue(row, 1, string.Empty);
                MeasuredValue measuredValue = DataSetProcessor.GetMeasuredValue(row, 2, (MeasuredValue) null);
                tupleList.Add(new Tuple<string, string>(MSOfficeAddinsHelper.CreateObjectUrl(int64Value), !flag || measuredValue == null ? stringValue : $"{stringValue} {measuredValue.ToString()}"));
              }
            }
          }
        }
      }
    }
    return tupleList.ToArray();
  }

  public bool IsDocumentCheckedOut(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this.GetDocumentVersionID(fileName));
      return dbObject.ObjectModifyMode == ObjectModifyModes.InBase || dbObject.CheckoutBy == sessionKeeper.Session.UserID;
    }
  }

  public bool IsDocumentRegistered(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    return !ObjectHelper.IsUnknownObjectVersionID(this.GetDocumentVersionID(fileName));
  }

  public void OpenDocumentComposition(string fileName, string objectUrl)
  {
    long num = !string.IsNullOrEmpty(fileName) ? this.GetDocumentVersionID(fileName) : throw new ArgumentException();
    Intermech.Navigator.DBObjects.Descriptor rootDescriptor = new Intermech.Navigator.DBObjects.Descriptor(num);
    NodeIDPath path = (NodeIDPath) null;
    if (!string.IsNullOrEmpty(objectUrl))
    {
      long versionIdFromObjectUrl = MSOfficeAddinsHelper.GetObjectVersionIDFromObjectUrl(objectUrl);
      if (!ObjectHelper.IsUnknownObjectVersionID(versionIdFromObjectUrl))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(versionIdFromObjectUrl, false) ?? sessionKeeper.Session.GetObject(-versionIdFromObjectUrl, false);
          if (dbObject != null)
          {
            IDBRelation relation = sessionKeeper.Session.GetRelation(num, dbObject.ID);
            if (relation != null)
            {
              NodeID NodeID = new NodeID(dbObject.ObjectType, dbObject.ObjectID, dbObject.ID, dbObject.CheckoutBy, relation.RelationID, dbObject.LCStep, dbObject.Caption, relation.TypeID, dbObject.OwnerID, 0L, ObjectFiltrationState.fsVersionNotFound, (long) dbObject.VersionID, dbObject.IsBaseVersion ? 1L : 0L, (string) null, num, relation.GUID, dbObject.ModificationID);
              path = new NodeIDPath((IDescriptor) rootDescriptor);
              path.Add(rootDescriptor.GetRecordNodeID());
              path.Add((INodeID) NodeID);
            }
          }
        }
      }
    }
    this.ShowMainForm();
    Utils.OpenNewWindow((IDescriptor) rootDescriptor, (IServiceProvider) ServicesManager.ServiceContainer, (GetSupportedColumnsEventHandler) null, path);
  }

  public string RegisterDocument(string fileName)
  {
    long documentVersionID = !string.IsNullOrEmpty(fileName) ? ClientContext.FileImporter.ImportFile(fileName) : throw new ArgumentException();
    if (!this.IsDocumentCheckedOut(documentVersionID))
      documentVersionID = this.CheckOutDocument(documentVersionID);
    fileName = this.PublishDocument(documentVersionID);
    this.ResetFileReadOnlyAttribute(fileName);
    return fileName;
  }

  public void SaveDocument(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    ObjectCommand saveChangesCommand = ObjectCommandFactory.CreateSaveChangesCommand(true);
    saveChangesCommand.ObjectId = this.GetDocumentVersionID(fileName);
    saveChangesCommand.UpdateUI = true;
    saveChangesCommand.Execute();
  }

  public string SelectAndPublishDocument(params string[] allowableFilesExtensions)
  {
    if (allowableFilesExtensions == null || allowableFilesExtensions.Length == 0 || ((IEnumerable<string>) allowableFilesExtensions).Any<string>((System.Func<string, bool>) (o => string.IsNullOrEmpty(o))))
      throw new ArgumentException();
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects("Выбор документа", "Выберите документ для открытия в редакторе.", MSOfficeAddinsConstants.DocumentObjectTypeID, SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return (string) null;
    long num = numArray[0];
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeById = sessionKeeper.Session.GetObject(num).GetAttributeByID(Constants.FileAttributeTypeID);
      if (attributeById == null)
        throw new Exception("Выбранный документ не содержит файлов.");
      if (!string.IsNullOrEmpty(attributeById.AsString))
      {
        if (((IEnumerable<string>) allowableFilesExtensions).Contains<string>(Path.GetExtension(attributeById.AsString)))
          goto label_12;
      }
      throw new Exception($"Выбран документ неподходящего типа. Расширение файла документа должно совпадать с одним из списка: {string.Join(", ", allowableFilesExtensions)}.");
    }
label_12:
    if (!this.IsDocumentCheckedOut(num))
      num = this.CheckOutDocument(num);
    string fileName = this.PublishDocument(num);
    this.ResetFileReadOnlyAttribute(fileName);
    return fileName;
  }

  public void ShowDocumentCard(string fileName)
  {
    int num = !string.IsNullOrEmpty(fileName) ? (int) PropertiesWindow.Execute("Свойства (Карточка)", string.Empty, this.GetDocumentVersionID(fileName), true) : throw new ArgumentException();
  }

  public Dictionary<string, Tuple<string, string>> UpdateObjectReferences(
    string fileName,
    string[] objectsUrls)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    if (objectsUrls == null || objectsUrls.Length == 0 || ((IEnumerable<string>) objectsUrls).Any<string>((System.Func<string, bool>) (o => string.IsNullOrEmpty(o))))
      throw new ArgumentException();
    Tuple<string, string>[] array = ((IEnumerable<string>) objectsUrls).Select<string, string[]>((System.Func<string, string[]>) (o => o.Split(new string[1]
    {
      "|||"
    }, StringSplitOptions.None))).Select<string[], Tuple<string, string>>((System.Func<string[], Tuple<string, string>>) (o => new Tuple<string, string>(o[0], o[1]))).ToArray<Tuple<string, string>>();
    Dictionary<string, Tuple<string, string>> dictionary = new Dictionary<string, Tuple<string, string>>();
    this.GetDocumentVersionID(fileName);
    IMSAttribute4RelationType attribute4RelationType = MetaDataHelper.GetAttribute4RelationType(MetaDataHelper.GetRelationTypeID(new Guid("cad00151-306c-11d8-b4e9-00304f19f545")), Constants.CountAttributeTypeID);
    Regex regex = (Regex) null;
    if (attribute4RelationType != null)
    {
      IMSAttributeType countAttribute = MetaDataHelper.GetAttributeType(Constants.CountAttributeTypeID);
      MeasureDescriptor[] source = MeasureHelper.Measures;
      if (!ObjectHelper.IsUnknownObjectVersionID(countAttribute.SizeType))
        source = ((IEnumerable<MeasureDescriptor>) MeasureHelper.Measures).Where<MeasureDescriptor>((System.Func<MeasureDescriptor, bool>) (o => o.PhysicalQuantityID == countAttribute.SizeType)).ToArray<MeasureDescriptor>();
      regex = new Regex($"(-?[0-9]+\\.?[0-9]+ ({string.Join("|", ((IEnumerable<MeasureDescriptor>) source).Select<MeasureDescriptor, string>((System.Func<MeasureDescriptor, string>) (o => o.ShortName)))}))$", RegexOptions.Compiled);
    }
    foreach (Tuple<string, string> tuple1 in array)
    {
      long versionIdFromObjectUrl = MSOfficeAddinsHelper.GetObjectVersionIDFromObjectUrl(tuple1.Item1);
      Tuple<string, string> tuple2 = (Tuple<string, string>) null;
      if (!ObjectHelper.IsUnknownObjectVersionID(versionIdFromObjectUrl))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(versionIdFromObjectUrl, false) ?? sessionKeeper.Session.GetObject(-versionIdFromObjectUrl, false);
          if (dbObject != null)
          {
            if (!dbObject.IsBaseVersion)
              dbObject = sessionKeeper.Session.GetObjectBaseVersionByID(dbObject.ID, false);
            Match match = regex.Match(tuple1.Item2);
            string str = match.Groups.Count > 1 ? $"{dbObject.Caption} {match.Groups[1].Value}" : dbObject.Caption;
            tuple2 = new Tuple<string, string>(MSOfficeAddinsHelper.CreateObjectUrl(dbObject.ObjectID, tuple1.Item1), str);
          }
          else
            tuple2 = new Tuple<string, string>(tuple1.Item1, "Ошибка! Источник ссылки не найден.");
        }
      }
      else
        tuple2 = new Tuple<string, string>(tuple1.Item1, "Ошибка! Источник ссылки не найден.");
      dictionary[tuple1.Item1] = tuple2;
    }
    return dictionary;
  }

  public bool IsDocumentInViewArea(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentException();
    return !string.IsNullOrEmpty(ClientContext.FileVault.ViewArea.AreaPath) && fileName.StartsWith(ClientContext.FileVault.ViewArea.AreaPath);
  }

  private long GetDocumentVersionID(string fileName)
  {
    long documentVersionId = 0;
    FileOrigin fileOrigin = (FileOrigin) null;
    try
    {
      fileOrigin = ClientContext.FileVault.WorkArea.GetFileOrigin(fileName, !Path.IsPathRooted(fileName));
    }
    catch
    {
    }
    if (fileOrigin != null)
    {
      switch (fileOrigin.OriginType)
      {
        case FileOriginType.WorkFile:
          documentVersionId = fileOrigin.WorkObject.ObjectId;
          break;
        case FileOriginType.DetachedFile:
          VersionsRulePackage editorRule = VersionsRuleSources.GetEditorRule();
          if (editorRule != null)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              IDBObject objectByVersionsRule = sessionKeeper.Session.GetObjectByVersionsRule(fileOrigin.Id, editorRule.OwnerId, false);
              if (objectByVersionsRule != null)
              {
                documentVersionId = objectByVersionsRule.ObjectID;
                break;
              }
              break;
            }
          }
          break;
      }
    }
    return documentVersionId;
  }

  private long CheckOutDocument(long documentVersionID)
  {
    ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
    checkoutCommand.ObjectId = documentVersionID;
    checkoutCommand.UpdateUI = true;
    checkoutCommand.Execute();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      documentVersionID = (sessionKeeper.Session.GetObjectActualCopy(documentVersionID, false) ?? sessionKeeper.Session.GetObjectActualCopy(-documentVersionID, false)).ObjectID;
    return documentVersionID;
  }

  private void GetObjectTypesAllowableForAddToDocumentByReference(
    long documentVersionID,
    out int[] parentObjectTypeIds,
    out int[] allObjectTypeIds)
  {
    int objTypeID = -1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      objTypeID = sessionKeeper.Session.GetObject(documentVersionID).ObjectType;
    parentObjectTypeIds = MetaDataHelper.GetObjectTypeApplicabilities(objTypeID).Where<IMSApplicability>((System.Func<IMSApplicability, bool>) (o => o.RelationTypeID == MSOfficeAddinsConstants.ObjectsAddedByReferenceRelationTypeID)).Select<IMSApplicability, int>((System.Func<IMSApplicability, int>) (o => o.ChildObjectTypeID)).Distinct<int>().ToArray<int>();
    List<int> source = new List<int>();
    foreach (int parentTypeID in parentObjectTypeIds)
      source.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(parentTypeID));
    allObjectTypeIds = source.Distinct<int>().ToArray<int>();
  }

  private IMSObjectType GetObjectType(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return MetaDataHelper.GetObjectType(sessionKeeper.Session.GetObject(objectVersionID).TypeID);
  }

  private string GetObjectCaption(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(objectVersionID).Caption;
  }

  private bool IsDocumentCheckedOut(long documentVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(documentVersionID);
      return dbObject.ObjectModifyMode == ObjectModifyModes.InBase || dbObject.CheckoutBy == sessionKeeper.Session.UserID;
    }
  }

  private void ShowMainForm()
  {
    IMainFormUpdate service = (IMainFormUpdate) ServicesManager.GetService(typeof (IMainFormUpdate));
    if (service.MainForm.WindowState == FormWindowState.Minimized)
    {
      service.MainForm.WindowState = FormWindowState.Normal;
    }
    else
    {
      service.MainForm.TopMost = true;
      service.MainForm.Focus();
      service.MainForm.BringToFront();
      service.MainForm.TopMost = false;
    }
  }

  private string PublishDocument(long documentVersionID)
  {
    return ClientContext.FileVault.PublishTree(documentVersionID, ClientContext.FileVault.DBFilesInfo.GetMasterFileName(documentVersionID, false), VersionsRuleSources.GetEditorRule(), (IFileArea) ClientContext.FileVault.WorkArea);
  }

  private void ResetFileReadOnlyAttribute(string fileName)
  {
    FileAttributes attributes = File.GetAttributes(fileName);
    if (!attributes.HasFlag((Enum) FileAttributes.ReadOnly))
      return;
    File.SetAttributes(fileName, attributes ^ FileAttributes.ReadOnly);
  }

  private sealed class ObjectsForAddToDocumentByReferenceAnalyzer : SelectedItemsAnalyzer
  {
    private int[] _allowableObjectTypeIds;

    public ObjectsForAddToDocumentByReferenceAnalyzer(int[] allowableObjectTypeIds)
    {
      this._allowableObjectTypeIds = allowableObjectTypeIds != null && allowableObjectTypeIds.Length != 0 ? allowableObjectTypeIds : throw new ArgumentException();
    }

    public override SelectedItemsAnalyzerResult Analyze(
      ISelectionWindow sender,
      ISelectedItemsHost itemsHost)
    {
      if (sender == null)
        throw new ArgumentNullException();
      if (itemsHost != null)
      {
        IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
        if (SelectedItemsHelper.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeID(itemsHost.SelectedItems, out typedObjectID) && ((IEnumerable<int>) this._allowableObjectTypeIds).Contains<int>(typedObjectID.ObjectType) || itemsHost.SelectedItems != null && itemsHost.SelectedItems.Count > 0 && itemsHost.SelectedItems.GetItemData(0, typeof (IImbaseTableRecordID)) != null || itemsHost.SelectedItems is IMHView.IMHSelectedItems selectedItems && selectedItems.Selectable && selectedItems.Count > 0 && selectedItems.GetItemData(0, (Type) null) is IMHMaterialRecordID itemData && !ObjectHelper.IsUnknownObjectVersionID(itemData.ID))
          return SelectedItemsAnalyzerResult.Enabled;
      }
      return SelectedItemsAnalyzerResult.Disabled;
    }
  }
}
