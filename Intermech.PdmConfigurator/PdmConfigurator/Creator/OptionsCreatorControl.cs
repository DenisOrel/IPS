// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Creator.OptionsCreatorControl
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.PropertyEditors;
using NJFLib.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator.Creator;

public sealed class OptionsCreatorControl : ObjectCreatorControl, IStepCompleteManager
{
  private long _createdObjectID;
  private int _createdObjectTypeID = -1;
  private Dictionary<long, PdmConfiguratorContext> _dict = new Dictionary<long, PdmConfiguratorContext>();
  private ICategoryTypeIconService _categoryTypeIconService;
  private Dictionary<long, Dictionary<long, OptionValueState>> _valuesStates = new Dictionary<long, Dictionary<long, OptionValueState>>();
  private bool _isChanged = true;
  private bool _hasErrors;
  private DescriptorCollection _descriptorCollection;
  private Dictionary<long, ObjectVersionDescription> _dictionary = new Dictionary<long, ObjectVersionDescription>();
  private bool _isCurrentStepComplete = true;
  private ButtonType _buttonType;
  private object _syncObject = new object();
  private SortedDictionary<RelationPath, TraceEntry> _errors = new SortedDictionary<RelationPath, TraceEntry>();
  private Dictionary<string, int> _colWidths = new Dictionary<string, int>();
  private IContainer components;
  private ObjectContextEditor _objectContextEditor;
  private iGrid _compositionGrid;
  private Panel panel1;
  private Intermech.Bars.ToolBar tbTracing;
  private ButtonItem _startTracingButtonItem;
  private ImageList ilTracing;
  private iGrid _tracingGrid;
  private Panel panel2;
  private CollapsibleSplitter collapsibleSplitter1;
  private ButtonItem _stopTracingButtonItem;
  private ImageList ilState;
  private Splitter splitter1;
  private Panel panel3;
  private Intermech.Bars.ToolBar tbComposition;
  private ButtonItem _addButtonItem;
  private ButtonItem _deleteButtonItem;
  private ImageList ilLinked;
  private ButtonItem _showCardButtonItem;

  public OptionsCreatorControl(CreatedObjectItem createdObject)
    : base(createdObject)
  {
    this.InitializeComponent();
    this._createdObjectID = createdObject.ObjectID;
    this._createdObjectTypeID = createdObject.ObjectTypeID;
    if (this._createdObjectTypeID != MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545"))
      this.collapsibleSplitter1.Visible = this.panel1.Visible = false;
    this._categoryTypeIconService = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    iGCellStyle iGcellStyle = new iGCellStyle(true);
    iGcellStyle.ImageList = this._categoryTypeIconService.ImageList;
    iGcellStyle.ImageAlign = iGContentAlignment.TopLeft;
    iGcellStyle.TextAlign = iGContentAlignment.TopLeft;
    iGcellStyle.ReadOnly = iGBool.True;
    iGCol iGcol1 = this._compositionGrid.Cols.Add(new iGColPattern(64 /*0x40*/, true, true, -1, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_12"), "CAPTION", -1, (object) null, (object) null, -1));
    iGcol1.CellStyle = iGcellStyle;
    iGcol1.AutoWidth(true);
    iGCol iGcol2 = this._compositionGrid.Cols.Add(new iGColPattern(90, true, false, 90, 90, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_13"), "COUNT", -1, (object) null, (object) null, -1));
    iGcol2.CellStyle.TypeFlags = iGCellTypeFlags.HasEllipsisBtn;
    iGcol2.CellStyle.Type = iGCellType.NotSet;
    this._compositionGrid.Cols.Add(new iGColPattern(64 /*0x40*/, false, false, -1, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "REL_ID", -1, (object) null, (object) null, -1));
    this._compositionGrid.Cols.Add(new iGColPattern(64 /*0x40*/, false, false, -1, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJ_ID", -1, (object) null, (object) null, -1));
    this._compositionGrid.Cols.Add(new iGColPattern(64 /*0x40*/, false, false, -1, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJ_TYPE_ID", -1, (object) null, (object) null, -1));
    this.PrepareGridsColumns();
    INamedImageList service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._showCardButtonItem.Image = service.ImageList.Images[service.ImageIndex("imgCard")];
    List<int> childObjectTypes = new List<int>();
    List<int> intList1 = new List<int>();
    foreach (IMSApplicability typeApplicability in MetaDataHelper.GetObjectTypeApplicabilities(this._createdObjectTypeID))
    {
      if (MetaDataHelper.IsPdmConfigurableRelationType(typeApplicability.RelationTypeID) && !childObjectTypes.Contains(typeApplicability.ChildObjectTypeID))
        childObjectTypes.Add(typeApplicability.ChildObjectTypeID);
    }
    List<int> intList2 = new List<int>();
    foreach (int num1 in childObjectTypes)
    {
      if (!MetaDataHelper.GetObjectType(num1).IsLocalType)
      {
        foreach (int num2 in MetaDataHelper.GetObjectTypeChildrenID(num1))
        {
          if (childObjectTypes.Contains(num2))
            intList2.Add(num2);
        }
      }
    }
    foreach (int num in intList2)
      childObjectTypes.Remove(num);
    this._descriptorCollection = new DescriptorCollection();
    for (int index = 0; index < childObjectTypes.Count; ++index)
      this._descriptorCollection.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(childObjectTypes[index]));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(-1);
      relationCollection.ChildObjectTypes = (IList<int>) MetaDataHelper.OptimizeChildObjectTypes((IEnumerable<int>) childObjectTypes);
      DBRecordSetParams paramsSet = new DBRecordSetParams((ConditionStructure[]) null, new object[3]
      {
        (object) -2,
        (object) -20,
        (object) MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545")
      });
      FiltrationHelper.BlockPluginFiltrations(ref paramsSet, (HybridDictionary) null);
      DataTable dataTable = relationCollection.ConsistFrom(paramsSet, this._createdObjectID);
      if (dataTable != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          long int64_1 = Convert.ToInt64(row[0]);
          long int64_2 = Convert.ToInt64(row[1]);
          string str = Convert.ToString(row[2]);
          this._dictionary.Add(int64_2, new ObjectVersionDescription(sessionKeeper.Session.GetObject(int64_1))
          {
            Tag = (object) str
          });
        }
      }
    }
    this._SaveInTransaction = false;
    this.LoadOptionsCache();
  }

  public override bool Refresh(PageRefreshArgs args)
  {
    if (this._dictionary.Keys.Count == 0 || this._createdObjectTypeID != MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545"))
    {
      this._isCurrentStepComplete = true;
      this._startTracingButtonItem.Enabled = false;
    }
    else
    {
      this._isCurrentStepComplete = false;
      this._startTracingButtonItem.Enabled = true;
    }
    this.FillNavigatorTree();
    this._buttonType = ButtonType.None;
    return true;
  }

  public override bool Save(PageSaveArgs args)
  {
    try
    {
      this.SaveCurrentContext();
      if (this.PageIndex >= args.NextPageIndex)
      {
        if (args.NextPageIndex != -1)
          goto label_4;
      }
      this.SaveContexts();
    }
    catch (Exception ex)
    {
      args.errorType = ErrorType.Unknown;
      args.Error = ex;
      return false;
    }
label_4:
    if (args.NextPageIndex == -1)
      this._buttonType = ButtonType.Finish;
    else if (this.PageIndex < args.NextPageIndex)
      this._buttonType = ButtonType.Next;
    else if (this.PageIndex > args.NextPageIndex)
      this._buttonType = ButtonType.Back;
    if (this._createdObjectTypeID == MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545") && !this._isCurrentStepComplete)
      args.errorType = ErrorType.CheckNotCompleted;
    return this._createdObjectTypeID != MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545") || this._isCurrentStepComplete;
  }

  public override void StartErrorCheck()
  {
    if (this._createdObjectTypeID != MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545") || this._isCurrentStepComplete)
      return;
    this._startTracingButtonItem.PerformClick();
  }

  public override int HelpTopicID => 1839;

  public event StepCompletedHandler StepCompletedEvent;

  private void ShowCardButtonItem_Click(object sender, EventArgs e)
  {
    iGRow curRow = this._compositionGrid.CurRow;
    if (curRow == null)
      return;
    long int64 = Convert.ToInt64(curRow.Cells["OBJ_ID"].Value);
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, int64, true);
  }

  private void AddButtonItem_Click(object sender, EventArgs e)
  {
    IDBTypedObjectID[] dbTypedObjectIdArray = this.SelectObjects();
    if (dbTypedObjectIdArray == null || dbTypedObjectIdArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(-1);
      foreach (IDBTypedObjectID dbTypedObjectId in dbTypedObjectIdArray)
      {
        List<IMSApplicability> typeApplicabilities = MetaDataHelper.GetObjectTypeApplicabilities(this._createdObjectTypeID);
        List<int> objectTypeParentsId = MetaDataHelper.GetObjectTypeParentsID(dbTypedObjectId.ObjectType);
        objectTypeParentsId.Insert(0, dbTypedObjectId.ObjectType);
        IMSApplicability imsApplicability = (IMSApplicability) null;
        foreach (int num in objectTypeParentsId)
        {
          int objectTypeID = num;
          imsApplicability = ((IEnumerable<IMSApplicability>) typeApplicabilities.Where<IMSApplicability>((System.Func<IMSApplicability, bool>) (o => o.ChildObjectTypeID == objectTypeID)).ToArray<IMSApplicability>()).FirstOrDefault<IMSApplicability>((System.Func<IMSApplicability, bool>) (o => o.Options.HasFlag((Enum) ApplicabilityOptions.DefaultRelation) && MetaDataHelper.IsPdmConfigurableRelationType(o.RelationTypeID)));
          if (imsApplicability != null)
            break;
        }
        if (imsApplicability == null)
        {
          foreach (int num in objectTypeParentsId)
          {
            int objectTypeID = num;
            imsApplicability = ((IEnumerable<IMSApplicability>) typeApplicabilities.Where<IMSApplicability>((System.Func<IMSApplicability, bool>) (o => o.ChildObjectTypeID == objectTypeID)).ToArray<IMSApplicability>()).FirstOrDefault<IMSApplicability>((System.Func<IMSApplicability, bool>) (o => MetaDataHelper.IsPdmConfigurableRelationType(o.RelationTypeID)));
            if (imsApplicability != null)
              break;
          }
        }
        if (imsApplicability != null)
        {
          relationCollection.RelationTypeID = imsApplicability.RelationTypeID;
          IDBRelation dbRelation = relationCollection.Create(this._createdObjectID, dbTypedObjectId.ObjectID);
          ObjectVersionDescription description = new ObjectVersionDescription(sessionKeeper.Session.GetObject(dbTypedObjectId.ObjectID));
          this._dictionary.Add(dbRelation.RelationID, description);
          this.AddObjectInGrid(dbRelation.RelationID, description);
        }
      }
      this.UpdateControls();
    }
  }

  private void RemoveButtonItem_Click(object sender, EventArgs e)
  {
    iGRow curRow = this._compositionGrid.CurRow;
    if (curRow == null)
      return;
    long int64 = Convert.ToInt64(curRow.Cells["REL_ID"].Value);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      sessionKeeper.Session.GetRelation(int64)?.Delete(0L);
    this._dictionary.Remove(int64);
    if (this._valuesStates.ContainsKey(int64))
      this._valuesStates.Remove(int64);
    if (this._dict.ContainsKey(int64))
      this._dict.Remove(int64);
    this._compositionGrid.Rows.RemoveAt(curRow.Index);
    this._objectContextEditor.ClearKeys();
    this._objectContextEditor.Clear();
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void CompositionGrid_EllipsisBtnClick(object sender, iGEllipsisBtnClickEventArgs e)
  {
    using (MeasureForm measureForm = new MeasureForm())
    {
      measureForm.Text = LocalizationHolder.rm.GetString("PdmConfigurator_18");
      string empty = string.Empty;
      try
      {
        empty = MeasureHelper.ConvertToMeasuredValue(Convert.ToString(this._compositionGrid.Cells[e.RowIndex, "COUNT"].Value)).Value.ToString();
      }
      catch
      {
      }
      long int64 = Convert.ToInt64(this._compositionGrid.Cells[e.RowIndex, "REL_ID"].Value);
      MeasureDescriptor[] array = (MeasureDescriptor[]) MeasureEditor.CollectCountMeasureDescriptors().ToArray(typeof (MeasureDescriptor));
      long measureID = -1;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBRelation relation = sessionKeeper.Session.GetRelation(int64);
        IDBRelationType relationType = sessionKeeper.Session.GetRelationType(relation.RelationType);
        if (relation == null)
          return;
        IDBAttributeType4 attributeById = relationType.Attributes.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"));
        if (attributeById == null || !(attributeById is IDBMeasureAttributeType measureAttributeType))
          return;
        measureID = measureAttributeType.DefaultMeasureID;
      }
      if (measureForm.ExecuteDialog(ref empty, ref measureID, array, (GetDefaultMeasureIDDelegate) null) != DialogResult.OK)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBAttribute attributeByGuid = sessionKeeper.Session.GetRelation(int64).GetAttributeByGuid(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"), false);
        if (attributeByGuid == null)
          return;
        string str = MeasureHelper.ConvertToString(Convert.ToDouble(empty), measureID, false);
        attributeByGuid.Value = (object) str;
        this._compositionGrid.Cells[e.RowIndex, "COUNT"].Value = (object) str;
        this._dictionary[int64].Tag = (object) str;
      }
    }
  }

  private void CompositionGrid_SelectionChanged(object sender, EventArgs e)
  {
    if (this._compositionGrid.SelectedCells != null && this._compositionGrid.SelectedCells.Count != 0)
    {
      iGRow row = this._compositionGrid.SelectedCells[0].Row;
      long int64_1 = Convert.ToInt64(row.Cells["OBJ_ID"].Value);
      int int32 = Convert.ToInt32(row.Cells["OBJ_TYPE_ID"].Value);
      long int64_2 = Convert.ToInt64(row.Cells["REL_ID"].Value);
      if (this._objectContextEditor.Context != null && this._objectContextEditor.IsChanged)
      {
        long fPrjlinkId = this._objectContextEditor.Context.Key.F_PRJLINK_ID;
        long fProjId = this._objectContextEditor.Context.Key.F_PROJ_ID;
        if (this._dict.ContainsKey(fPrjlinkId))
          this._dict[fPrjlinkId] = this._objectContextEditor.Context;
        if (this._valuesStates.ContainsKey(fPrjlinkId))
          this._valuesStates[fPrjlinkId] = this._objectContextEditor.ValuesStates;
        else
          this._valuesStates.Add(fPrjlinkId, this._objectContextEditor.ValuesStates);
      }
      if (!this._dict.ContainsKey(int64_2))
      {
        this.LoadObjectContext(int64_1, int32, int64_2);
      }
      else
      {
        if (!this._valuesStates.ContainsKey(int64_2))
          this._valuesStates.Add(int64_2, new Dictionary<long, OptionValueState>());
        this._objectContextEditor.ValuesStates = this._valuesStates[int64_2];
        this._objectContextEditor.Context = this._dict[int64_2];
      }
    }
    this.UpdateControls();
  }

  private void CompositionGrid_RequestEdit(object sender, iGRequestEditEventArgs e)
  {
    e.DoDefault = false;
  }

  private void ObjectContextEditor_Changed(object sender, EventArgs e)
  {
    if (this._createdObjectTypeID != MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545") || this._objectContextEditor.Context == null || this._objectContextEditor.Context.Key == null || this._objectContextEditor.Context.Key.Empty || !this._objectContextEditor.IsChanged)
      return;
    this.IsChanged = true;
  }

  private void StartTracingButtonItem_Click(object sender, EventArgs e)
  {
    this.SaveCurrentContext();
    this.SaveContexts();
    this._startTracingButtonItem.Enabled = false;
    this._stopTracingButtonItem.Enabled = true;
    SortedDictionary<RelationPath, TraceEntry> errors = TracingProgressForm.Execute(this._dictionary, this._createdObjectID);
    this._startTracingButtonItem.Enabled = true;
    this._stopTracingButtonItem.Enabled = false;
    this.LoadTracingInfo(errors);
  }

  private void StopTracingButtonItem_Click(object sender, EventArgs e)
  {
    this._startTracingButtonItem.Enabled = true;
    this._stopTracingButtonItem.Enabled = false;
  }

  private void TracingGrid_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    this._colWidths[this._tracingGrid.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectColsWidth();
  }

  private void TracingGrid_ColWidthEndChange(object sender, iGColWidthEventArgs e)
  {
    this._colWidths[this._tracingGrid.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectColsWidth();
  }

  private void TracingGrid_Resize(object sender, EventArgs e) => this.CorrectColsWidth();

  private bool IsChanged
  {
    set
    {
      this._isChanged = value;
      this._isCurrentStepComplete = !this._isChanged && !this._hasErrors;
    }
    get => this._isChanged;
  }

  private bool HasErrors
  {
    set
    {
      this._hasErrors = value;
      this._isCurrentStepComplete = !this._isChanged && !this._hasErrors;
    }
    get => this._hasErrors;
  }

  private void SaveCurrentContext()
  {
    if (this._objectContextEditor.Context == null || !this._objectContextEditor.IsChanged)
      return;
    long fPrjlinkId = this._objectContextEditor.Context.Key.F_PRJLINK_ID;
    if (this._dict.ContainsKey(fPrjlinkId))
      this._dict[fPrjlinkId] = this._objectContextEditor.Context;
    long fProjId = this._objectContextEditor.Context.Key.F_PROJ_ID;
    if (this._valuesStates.ContainsKey(fPrjlinkId))
      this._valuesStates[fPrjlinkId] = this._objectContextEditor.ValuesStates;
    else
      this._valuesStates.Add(fPrjlinkId, this._objectContextEditor.ValuesStates);
  }

  private void SaveContexts()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._createdObjectTypeID == MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545"))
      {
        foreach (long key in this._dictionary.Keys)
        {
          ObjectVersionDescription versionDescription = this._dictionary[key];
          IDBObject partObject = sessionKeeper.Session.GetObject(versionDescription.F_OBJECT_ID);
          if (!this._dict.ContainsKey(key))
          {
            IDBRelation relation = sessionKeeper.Session.GetRelation(key, false);
            PdmConfiguratorContext context = this.CreateContext(sessionKeeper.Session, relation, partObject);
            this._dict.Add(key, context);
          }
        }
      }
      foreach (long key in this._dict.Keys)
      {
        IDBRelation relation = sessionKeeper.Session.GetRelation(key, false);
        try
        {
          this._dict[key].Services.AddService(typeof (IUserSession), (object) sessionKeeper.Session);
          this._dict[key].Services.AddService(typeof (object), (object) relation);
          this._dict[key].SaveToObject((IDBAttributable) relation);
        }
        finally
        {
          this._dict[key].Services.RemoveService(typeof (object));
          this._dict[key].Services.RemoveService(typeof (IUserSession));
        }
      }
    }
  }

  private void CheckOrderOptionValue(IUserSession session, PdmConfiguratorContext context)
  {
    foreach (Guid key in context.OptionsValues.Keys)
    {
      if (string.IsNullOrEmpty(context.OptionsValues[key]))
      {
        OptionHolder orLoadOption = PdmConfiguratorCache.CacheFindOrLoadOption(session, key);
        string str = orLoadOption == null ? string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_14"), (object) key.ToString()) : $"\"{orLoadOption.OptionCaption}\"";
        throw new PdmConfiguratorExeption(string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_15"), (object) str));
      }
    }
  }

  private PdmConfiguratorContext CreateContext(
    IUserSession session,
    IDBRelation relation,
    IDBObject partObject)
  {
    PdmConfiguratorContext context = new PdmConfiguratorContext((object) relation);
    RelationPair key = PdmConfiguratorHelper.CreateKey(this._createdObjectID, this._createdObjectTypeID, relation.RelationID, relation.RelationType, partObject.ObjectID, partObject.ObjectType);
    try
    {
      context.Services.AddService(typeof (IUserSession), (object) session);
      context.Services.AddService(typeof (object), (object) relation);
      context.Key = key;
      context.ObjectsOptions.Clear();
      ObjectOptionsHolder objectOptionsHolder = new ObjectOptionsHolder((object) partObject);
      context.ObjectsOptions.Add(objectOptionsHolder);
      context.SyncOptionsList(true);
      context.Key = key;
    }
    finally
    {
      context.Services.RemoveService(typeof (object));
      context.Services.RemoveService(typeof (IUserSession));
    }
    return context;
  }

  private void LoadObjectContext(long objectID, int objectTypeID, long relID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelation relation = sessionKeeper.Session.GetRelation(relID, false);
      IDBObject partObject = sessionKeeper.Session.GetObject(objectID);
      if (relation != null)
      {
        this._objectContextEditor.AccessRights = this._objectContextEditor.CheckAccessRights((IDBAttributable) relation);
        this._objectContextEditor.Services = (IServiceProvider) new ServiceContainer();
        PdmConfiguratorContext context = this.CreateContext(sessionKeeper.Session, relation, partObject);
        if (!this._valuesStates.ContainsKey(relation.RelationID))
          this._valuesStates.Add(relation.RelationID, new Dictionary<long, OptionValueState>());
        this._objectContextEditor.ValuesStates = this._valuesStates[relation.RelationID];
        this._dict[relID] = this._objectContextEditor.Context = context;
      }
      else
      {
        this._objectContextEditor.ClearKeys();
        this._objectContextEditor.Clear();
      }
    }
  }

  private void LoadTracingInfo(SortedDictionary<RelationPath, TraceEntry> errors)
  {
    this._tracingGrid.Rows.Clear();
    this._isCurrentStepComplete = errors.Count == 0;
    if (errors.Count > 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        foreach (RelationPath key in errors.Keys)
        {
          TraceEntry error = errors[key];
          if (error.Flags != PdmConfiguratorResult.Unknown)
          {
            iGRow iGrow = this._tracingGrid.Rows.Add();
            iGrow.Cells["MESSAGE"].Value = error.Message != string.Empty ? (object) error.Message : (object) LocalizationHolder.rm.GetString("PdmConfigurator_5");
            iGrow.Cells["MESSAGE"].ImageIndex = error.Flags != PdmConfiguratorResult.Incompatibles ? (error.Flags != PdmConfiguratorResult.ContextNotFound ? (error.Flags != PdmConfiguratorResult.Exception ? (error.Flags == PdmConfiguratorResult.OptionNotFound || error.Flags == PdmConfiguratorResult.ConflictOptionNotFound || error.Flags == PdmConfiguratorResult.ApplOptionNotFound ? 3 : (error.Flags == PdmConfiguratorResult.OptionValueNotFound || error.Flags == PdmConfiguratorResult.ConflictOptionValueNotFound || error.Flags == PdmConfiguratorResult.ApplOptionValueNotFound ? 4 : (error.Flags != PdmConfiguratorResult.True ? 6 : 5))) : 2) : 1) : 0;
            iGrow.Cells["RELATION_PATH"].Value = (object) key;
            iGrow.Cells["TRACE"].Value = (object) error;
            long fPartId = key.Items[key.Items.Count - 1].F_PART_ID;
            int fObjectType = key.Items[key.Items.Count - 1].F_OBJECT_TYPE;
            iGrow.Cells["RELATION_ID"].Value = (object) key.Items[key.Items.Count - 1].F_PRJLINK_ID;
            iGrow.Cells["OBJECT_TYPE_ICON"].ImageIndex = this._categoryTypeIconService.IndexOf(4, fObjectType);
            QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(fPartId);
            if (!objectInfo.Empty)
            {
              iGrow.Cells["OBJECT_CAPTION"].Value = (object) objectInfo.Caption;
              iGrow.Cells["OBJECT_ID"].Value = (object) objectInfo.ObjectID;
            }
          }
        }
      }
    }
    else
    {
      if (this._buttonType == ButtonType.None || this.StepCompletedEvent == null)
        return;
      this.StepCompletedEvent(this._buttonType);
    }
  }

  private void PrepareGridsColumns()
  {
    iGCellStyle iGcellStyle1 = new iGCellStyle(true);
    iGcellStyle1.ImageAlign = iGContentAlignment.TopLeft;
    iGcellStyle1.TextAlign = iGContentAlignment.TopLeft;
    iGcellStyle1.ReadOnly = iGBool.True;
    iGcellStyle1.ImageList = this.ilState;
    iGCellStyle iGcellStyle2 = new iGCellStyle(true);
    iGcellStyle2.TextAlign = iGContentAlignment.TopLeft;
    iGcellStyle2.ReadOnly = iGBool.True;
    iGCellStyle iGcellStyle3 = new iGCellStyle(true);
    iGcellStyle3.ImageAlign = iGContentAlignment.MiddleLeft;
    iGcellStyle3.ReadOnly = iGBool.True;
    iGcellStyle3.ImageList = this._categoryTypeIconService.ImageList;
    if (this._colWidths.Count == 0)
      this._colWidths = new Dictionary<string, int>()
      {
        {
          "OBJECT_TYPE_ICON",
          32 /*0x20*/
        },
        {
          "OBJECT_ID",
          50
        },
        {
          "RELATION_ID",
          50
        },
        {
          "OBJECT_CAPTION",
          200
        },
        {
          "MESSAGE",
          200
        },
        {
          "RELATION_PATH",
          0
        },
        {
          "TRACE",
          0
        }
      };
    iGCol col1 = this._tracingGrid.Cols["OBJECT_TYPE_ICON"];
    iGCol iGcol1 = this._tracingGrid.Cols["OBJECT_TYPE_ICON"] ?? this._tracingGrid.Cols.Add(new iGColPattern(this._colWidths["OBJECT_TYPE_ICON"], true, true, 32 /*0x20*/, 32 /*0x20*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJECT_TYPE_ICON", -1, (object) null, (object) null, -1));
    iGcol1.Width = this._colWidths["OBJECT_TYPE_ICON"];
    iGcol1.CellStyle = iGcellStyle3;
    iGCol col2 = this._tracingGrid.Cols["OBJECT_ID"];
    iGCol iGcol2 = this._tracingGrid.Cols["OBJECT_ID"] ?? this._tracingGrid.Cols.Add(new iGColPattern(this._colWidths["OBJECT_ID"], true, true, 50, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_1"), "OBJECT_ID", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol2.CellStyle = iGcellStyle2;
    iGcol2.Width = this._colWidths["OBJECT_ID"];
    iGCol col3 = this._tracingGrid.Cols["RELATION_ID"];
    iGCol iGcol3 = this._tracingGrid.Cols["RELATION_ID"] ?? this._tracingGrid.Cols.Add(new iGColPattern(this._colWidths["RELATION_ID"], true, true, 50, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_2"), "RELATION_ID", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol3.CellStyle = iGcellStyle2;
    iGcol3.Width = this._colWidths["RELATION_ID"];
    iGCol col4 = this._tracingGrid.Cols["OBJECT_CAPTION"];
    iGCol iGcol4 = this._tracingGrid.Cols["OBJECT_CAPTION"] ?? this._tracingGrid.Cols.Add(new iGColPattern(this._colWidths["OBJECT_CAPTION"], true, true, 200, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_3"), "OBJECT_CAPTION", -1, (object) null, (object) null, -1));
    iGcol4.Width = this._colWidths["OBJECT_CAPTION"];
    iGcol4.CellStyle = iGcellStyle2;
    iGCol col5 = this._tracingGrid.Cols["MESSAGE"];
    iGCol iGcol5 = this._tracingGrid.Cols["MESSAGE"] ?? this._tracingGrid.Cols.Add(new iGColPattern(this._colWidths["MESSAGE"], true, true, 200, -1, true, false, false, iGSortType.ByValue, iGSortOrder.Ascending, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_4"), "MESSAGE", -1, (object) null, (object) null, -1));
    iGcol5.Width = this._colWidths["MESSAGE"];
    iGcol5.CellStyle = iGcellStyle1;
    iGCol col6 = this._tracingGrid.Cols["RELATION_PATH"];
    (this._tracingGrid.Cols["RELATION_PATH"] ?? this._tracingGrid.Cols.Add(new iGColPattern(this._colWidths["RELATION_PATH"], false, false, 0, 0, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "RELATION_PATH", -1, (object) null, (object) null, -1))).Width = this._colWidths["RELATION_PATH"];
    iGCol col7 = this._tracingGrid.Cols["TRACE"];
    (this._tracingGrid.Cols["TRACE"] ?? this._tracingGrid.Cols.Add(new iGColPattern(this._colWidths["TRACE"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "TRACE", -1, (object) null, (object) null, -1))).Width = this._colWidths["TRACE"];
    this.CorrectColsWidth();
  }

  private void CorrectColsWidth()
  {
    if (this._tracingGrid.AutoResizeCols || this._colWidths.Count == 0)
      return;
    int num = this._tracingGrid.ClientRectangle.Width - 30 - this._colWidths["OBJECT_TYPE_ICON"] - this._colWidths["OBJECT_ID"] - this._colWidths["RELATION_ID"] - this._colWidths["OBJECT_CAPTION"];
    if (this._tracingGrid.Cols.Count == 0)
      return;
    this._tracingGrid.Cols["OBJECT_CAPTION"].Width = this._colWidths["OBJECT_CAPTION"];
    this._tracingGrid.Cols["OBJECT_ID"].Width = this._colWidths["OBJECT_ID"];
    this._tracingGrid.Cols["RELATION_ID"].Width = this._colWidths["RELATION_ID"];
    if (num > 200)
      this._tracingGrid.Cols["MESSAGE"].Width = this._colWidths["MESSAGE"] = num;
    else
      this._tracingGrid.Cols["MESSAGE"].Width = this._colWidths["MESSAGE"];
  }

  private void FillNavigatorTree()
  {
    this._compositionGrid.Rows.Clear();
    foreach (long key in this._dictionary.Keys)
      this.AddObjectInGrid(key, this._dictionary[key]);
    if (this._compositionGrid.Rows.Count > 0)
    {
      this._compositionGrid.CurRow = this._compositionGrid.Rows[0];
    }
    else
    {
      this._objectContextEditor.ClearKeys();
      this._objectContextEditor.Clear();
      this._objectContextEditor.AccessRights = PdmContextAccessRights.FullAccess;
    }
  }

  private void AddObjectInGrid(long relID, ObjectVersionDescription description)
  {
    iGRow iGrow = this._compositionGrid.Rows.Add();
    iGrow.Cells["CAPTION"].ImageIndex = this._categoryTypeIconService.IndexOf(4, description.F_OBJECT_TYPE);
    iGrow.Cells["CAPTION"].Value = !string.IsNullOrEmpty(description.CAPTION) ? (object) description.CAPTION : (object) $"{MetaDataHelper.GetObjectType(description.F_OBJECT_TYPE).ObjectName} [{description.F_OBJECT_ID}]";
    iGrow.Cells["REL_ID"].Value = (object) relID;
    iGrow.Cells["OBJ_ID"].Value = (object) description.F_OBJECT_ID;
    iGrow.Cells["OBJ_TYPE_ID"].Value = (object) description.F_OBJECT_TYPE;
    iGrow.Cells["COUNT"].Value = description.Tag;
    this.IsChanged = true;
  }

  private IDBTypedObjectID[] SelectObjects()
  {
    SelectionOptions options = SelectionOptions.Default | SelectionOptions.ForceFilterObjectsByRule;
    return Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("PdmConfigurator_16"), LocalizationHolder.rm.GetString("PdmConfigurator_17"), (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(Intermech.Navigator.Consts.CategoryCustomNode, 1, LocalizationHolder.rm.GetString("PdmConfigurator_16"), this._descriptorCollection), typeof (IDBTypedObjectID), options) as IDBTypedObjectID[];
  }

  private void UpdateControls()
  {
    this._showCardButtonItem.Enabled = this._deleteButtonItem.Enabled = this._compositionGrid.SelectedCells.Count > 0;
    this._startTracingButtonItem.Enabled = this._compositionGrid.Rows.Count > 0;
    this._isCurrentStepComplete = this._compositionGrid.Rows.Count == 0 || this._isCurrentStepComplete;
  }

  private void LoadOptionsCache()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      (session.GetCustomService(typeof (IPdmConfiguratorService)) as IPdmConfiguratorService).LoadOptions((object) session.SessionGUID);
    }
  }

  public bool IsCompletedEventSubscribed => this.StepCompletedEvent != null;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OptionsCreatorControl));
    this._compositionGrid = new iGrid();
    this.panel1 = new Panel();
    this._tracingGrid = new iGrid();
    this.tbTracing = new Intermech.Bars.ToolBar();
    this.ilTracing = new ImageList(this.components);
    this._startTracingButtonItem = new ButtonItem();
    this._stopTracingButtonItem = new ButtonItem();
    this.panel2 = new Panel();
    this.splitter1 = new Splitter();
    this._objectContextEditor = new ObjectContextEditor();
    this.panel3 = new Panel();
    this.tbComposition = new Intermech.Bars.ToolBar();
    this.ilLinked = new ImageList(this.components);
    this._showCardButtonItem = new ButtonItem();
    this._addButtonItem = new ButtonItem();
    this._deleteButtonItem = new ButtonItem();
    this.collapsibleSplitter1 = new CollapsibleSplitter();
    this.ilState = new ImageList(this.components);
    ((ISupportInitialize) this._compositionGrid).BeginInit();
    this.panel1.SuspendLayout();
    ((ISupportInitialize) this._tracingGrid).BeginInit();
    this.panel2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    this._compositionGrid.AutoResizeCols = true;
    this._compositionGrid.AutoWidthColMode = iGAutoWidthColMode.Cells;
    this._compositionGrid.BackColorEvenRows = Color.WhiteSmoke;
    this._compositionGrid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this._compositionGrid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this._compositionGrid, "_compositionGrid");
    this._compositionGrid.Header.Height = (int) componentResourceManager.GetObject("_compositionGrid.Header.Height");
    this._compositionGrid.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this._compositionGrid.HotTracking = false;
    this._compositionGrid.LayoutObject.Flags = iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._compositionGrid.Name = "_compositionGrid";
    this._compositionGrid.ProcessTab = false;
    this._compositionGrid.RowMode = true;
    this._compositionGrid.RowModeHasCurCell = true;
    this._compositionGrid.SilentValidation = true;
    this._compositionGrid.SingleClickEdit = true;
    this._compositionGrid.EllipsisBtnClick += new iGEllipsisBtnClickEventHandler(this.CompositionGrid_EllipsisBtnClick);
    this._compositionGrid.SelectionChanged += new EventHandler(this.CompositionGrid_SelectionChanged);
    this._compositionGrid.RequestEdit += new iGRequestEditEventHandler(this.CompositionGrid_RequestEdit);
    this.panel1.Controls.Add((Control) this._tracingGrid);
    this.panel1.Controls.Add((Control) this.tbTracing);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this._tracingGrid.BackColorEvenRows = Color.WhiteSmoke;
    this._tracingGrid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height1");
    this._tracingGrid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight1");
    componentResourceManager.ApplyResources((object) this._tracingGrid, "_tracingGrid");
    this._tracingGrid.Header.Height = (int) componentResourceManager.GetObject("_tracingGrid.Header.Height");
    this._tracingGrid.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this._tracingGrid.HotTracking = false;
    this._tracingGrid.LayoutObject.Flags = iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._tracingGrid.Name = "_tracingGrid";
    this._tracingGrid.ReadOnly = true;
    this._tracingGrid.RowMode = true;
    this._tracingGrid.VScrollBar.Visibility = iGScrollBarVisibility.Always;
    this._tracingGrid.ColWidthEndChange += new iGColWidthEventHandler(this.TracingGrid_ColWidthEndChange);
    this._tracingGrid.ColWidthChanging += new iGColWidthEventHandler(this.TracingGrid_ColWidthChanging);
    this._tracingGrid.Resize += new EventHandler(this.TracingGrid_Resize);
    this.tbTracing.FullMenus = true;
    this.tbTracing.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbTracing.Hidden = false;
    this.tbTracing.ImageList = this.ilTracing;
    this.tbTracing.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this._startTracingButtonItem,
      (ToolbarItemBase) this._stopTracingButtonItem
    });
    componentResourceManager.ApplyResources((object) this.tbTracing, "tbTracing");
    this.tbTracing.Name = "tbTracing";
    this.tbTracing.Tag = (object) "";
    this.ilTracing.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilTracing.ImageStream");
    this.ilTracing.TransparentColor = Color.Transparent;
    this.ilTracing.Images.SetKeyName(0, "gear_stop.png");
    this.ilTracing.Images.SetKeyName(1, "gear_run.png");
    this.ilTracing.Images.SetKeyName(2, "standard_add.ico");
    this.ilTracing.Images.SetKeyName(3, "pdm_delete.ico");
    componentResourceManager.ApplyResources((object) this._startTracingButtonItem, "_startTracingButtonItem");
    this._startTracingButtonItem.ImageIndex = 1;
    this._startTracingButtonItem.Click += new EventHandler(this.StartTracingButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._stopTracingButtonItem, "_stopTracingButtonItem");
    this._stopTracingButtonItem.Enabled = false;
    this._stopTracingButtonItem.ImageIndex = 0;
    this._stopTracingButtonItem.Click += new EventHandler(this.StopTracingButtonItem_Click);
    this.panel2.Controls.Add((Control) this.splitter1);
    this.panel2.Controls.Add((Control) this._objectContextEditor);
    this.panel2.Controls.Add((Control) this.panel3);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.splitter1, "splitter1");
    this.splitter1.Name = "splitter1";
    this.splitter1.TabStop = false;
    componentResourceManager.ApplyResources((object) this._objectContextEditor, "_objectContextEditor");
    this._objectContextEditor.IsChanged = false;
    this._objectContextEditor.IsOptionValueStatus = true;
    this._objectContextEditor.Name = "_objectContextEditor";
    this._objectContextEditor.OnChanged += new ObjectContextEditor.ContextChangedEventHandler(this.ObjectContextEditor_Changed);
    this.panel3.Controls.Add((Control) this._compositionGrid);
    this.panel3.Controls.Add((Control) this.tbComposition);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    this.tbComposition.FullMenus = true;
    this.tbComposition.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbComposition.Hidden = false;
    this.tbComposition.ImageList = this.ilLinked;
    this.tbComposition.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this._showCardButtonItem,
      (ToolbarItemBase) this._addButtonItem,
      (ToolbarItemBase) this._deleteButtonItem
    });
    componentResourceManager.ApplyResources((object) this.tbComposition, "tbComposition");
    this.tbComposition.Name = "tbComposition";
    this.tbComposition.Tag = (object) "";
    this.ilLinked.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilLinked.ImageStream");
    this.ilLinked.TransparentColor = Color.Transparent;
    this.ilLinked.Images.SetKeyName(0, "add.png");
    this.ilLinked.Images.SetKeyName(1, "delete.png");
    componentResourceManager.ApplyResources((object) this._showCardButtonItem, "_showCardButtonItem");
    this._showCardButtonItem.Click += new EventHandler(this.ShowCardButtonItem_Click);
    this._addButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addButtonItem, "_addButtonItem");
    this._addButtonItem.ImageIndex = 0;
    this._addButtonItem.Click += new EventHandler(this.AddButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._deleteButtonItem, "_deleteButtonItem");
    this._deleteButtonItem.Enabled = false;
    this._deleteButtonItem.ImageIndex = 1;
    this._deleteButtonItem.Click += new EventHandler(this.RemoveButtonItem_Click);
    this.collapsibleSplitter1.AnimationDelay = 20;
    this.collapsibleSplitter1.AnimationStep = 20;
    this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Etched;
    this.collapsibleSplitter1.ControlToHide = (Control) this.panel1;
    componentResourceManager.ApplyResources((object) this.collapsibleSplitter1, "collapsibleSplitter1");
    this.collapsibleSplitter1.ExpandParentForm = false;
    this.collapsibleSplitter1.Name = "collapsibleSplitter1";
    this.collapsibleSplitter1.TabStop = false;
    this.collapsibleSplitter1.UseAnimations = false;
    this.collapsibleSplitter1.VisualStyle = VisualStyles.Mozilla;
    this.ilState.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilState.ImageStream");
    this.ilState.TransparentColor = Color.Transparent;
    this.ilState.Images.SetKeyName(0, "pcsIncompatibilities.ico");
    this.ilState.Images.SetKeyName(1, "pcsContextNotFound.ico");
    this.ilState.Images.SetKeyName(2, "pcsException.ico");
    this.ilState.Images.SetKeyName(3, "pcsOptionNotFound.ico");
    this.ilState.Images.SetKeyName(4, "pcsOptionValueNotFound.ico");
    this.ilState.Images.SetKeyName(5, "pcsConfigured.ico");
    this.ilState.Images.SetKeyName(6, "pcsNone.ico");
    this.ilState.Images.SetKeyName(7, "gear_information.png");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.collapsibleSplitter1);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (OptionsCreatorControl);
    ((ISupportInitialize) this._compositionGrid).EndInit();
    this.panel1.ResumeLayout(false);
    ((ISupportInitialize) this._tracingGrid).EndInit();
    this.panel2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
