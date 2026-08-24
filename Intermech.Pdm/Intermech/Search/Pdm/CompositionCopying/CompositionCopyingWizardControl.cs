// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionCopyingWizardControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.Interfaces;
using Intermech.Search.CompositionContexts;
using Intermech.Search.GroupAttributesChanging;
using Intermech.Search.Pdm.Instances;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public sealed class CompositionCopyingWizardControl : UserControl
{
  private IFiltrationService _filtrationService;
  private long _objectVersionId;
  private int[] _allowableForCreateCopyObjectTypes = new int[0];
  private bool _excludeFromCompositionForAll;
  private bool _useExistingObjectForAll;
  private Dictionary<long, long> _newObjects = new Dictionary<long, long>();
  private List<long> _newObjectsWithComposition = new List<long>();
  private List<long> _needToUseExistingObjects = new List<long>();
  private List<long> _needToExcludeObjects = new List<long>();
  private ObjectBlank[] _blanksBackup;
  private long[] _excluded;
  private IContainer components;
  private TabControl _tabControl;
  private TabPage _compositionCopyingTabPage;
  private TabPage _groupAttributesChangingTabPage;
  private TableLayoutPanel tableLayoutPanel1;
  private FlowLayoutPanel flowLayoutPanel1;
  private Button _cancelButton;
  private Button _acceptButton;
  private Button _nextButton;
  private CompositionCopyingControl _compositionCopyingControl;
  private GroupAttributesChangingControl _groupAttributesChangingControl;
  private FlowLayoutPanel flowLayoutPanel2;
  private CheckBox _openEditorCheckBox;
  private CheckBox _openInNewNavigatorWindowCheckBox;

  public CompositionCopyingWizardControl()
  {
    this.InitializeComponent();
    this._tabControl.TabPages.Remove(this._groupAttributesChangingTabPage);
    this._acceptButton.Enabled = false;
  }

  public event EventHandler CancelButtonClicked;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long ObjectVersionID
  {
    get => this._objectVersionId;
    set
    {
      if (this._objectVersionId == value)
        return;
      this._objectVersionId = value;
      this._compositionCopyingControl.ObjectVersionID = this._objectVersionId;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] AllowableForCreateCopyObjectTypes
  {
    get => this._allowableForCreateCopyObjectTypes;
    set
    {
      if (value == null || value.Length == 0 || ObjectTypeHelper.IsAnyUnknownObjectTypeID((IEnumerable<int>) value))
        throw new ArgumentException();
      if (this._allowableForCreateCopyObjectTypes == value)
        return;
      this._allowableForCreateCopyObjectTypes = ((IEnumerable<int>) value).Distinct<int>().ToArray<int>();
      this._compositionCopyingControl.AllowableForCreateCopyObjectTypes = this._allowableForCreateCopyObjectTypes;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] RelationTypes
  {
    get => this._compositionCopyingControl.RelationTypes;
    set => this._compositionCopyingControl.RelationTypes = value;
  }

  public void Initialize(
    ICurrentUserAndRole currentUserAndRole,
    IFiltrationService filtrationService,
    INavigatorClientService navigatorClientService)
  {
    if (filtrationService == null)
      throw new ArgumentNullException(nameof (filtrationService));
    if (navigatorClientService == null)
      throw new ArgumentNullException(nameof (navigatorClientService));
    this._filtrationService = filtrationService;
    this._compositionCopyingControl.Initialize(currentUserAndRole, filtrationService, navigatorClientService);
  }

  public object CreateMemento()
  {
    return (object) new CompositionCopyingWizardControl.CompositionCopyingWizardControlMemento()
    {
      CompositionCopyingControlState = this._compositionCopyingControl.CreateMemento(),
      GroupAttributesChangingControlState = this._groupAttributesChangingControl.CreateMemento()
    };
  }

  public void SetMemento(object memento)
  {
    CompositionCopyingWizardControl.CompositionCopyingWizardControlMemento wizardControlMemento = memento is CompositionCopyingWizardControl.CompositionCopyingWizardControlMemento ? (CompositionCopyingWizardControl.CompositionCopyingWizardControlMemento) memento : throw new ArgumentException();
    this._compositionCopyingControl.SetMemento(wizardControlMemento.CompositionCopyingControlState);
    this._groupAttributesChangingControl.SetMemento(wizardControlMemento.GroupAttributesChangingControlState);
  }

  private void NextButton_Click(object sender, EventArgs e)
  {
    ObjectBlank[] list = (ObjectBlank[]) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      list = ((ICompositionCopyingServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionCopyingServerService))).CreateBlanks(sessionKeeper.Session.SessionGUID, this._objectVersionId, this._compositionCopyingControl.GetCopies());
    this._groupAttributesChangingControl.Objects = new BindingList<ObjectBlank>((IList<ObjectBlank>) list);
    this._tabControl.TabPages.Remove(this._compositionCopyingTabPage);
    this._tabControl.TabPages.Add(this._groupAttributesChangingTabPage);
    this._nextButton.Enabled = false;
    this._acceptButton.Enabled = true;
  }

  private void AcceptButton_Click(object sender, EventArgs e)
  {
    this._blanksBackup = this._groupAttributesChangingControl.Objects.Select<ObjectBlank, ObjectBlank>((System.Func<ObjectBlank, ObjectBlank>) (o => o.Clone())).ToArray<ObjectBlank>();
    this._excluded = this._compositionCopyingControl.GetExcluded();
    try
    {
      this.CreateObjectWithCompositionByPrototype(this._objectVersionId);
      if (this._newObjects.ContainsKey(this._objectVersionId) && this._compositionCopyingControl.Instances != null)
      {
        foreach (long instance in this._compositionCopyingControl.Instances)
        {
          long compositionByPrototype = this.CreateObjectWithCompositionByPrototype(instance);
          if (this._newObjects.ContainsKey(instance))
            this.MakeInstance(this._newObjects[this._objectVersionId], compositionByPrototype);
        }
      }
      this._acceptButton.Enabled = false;
      this._cancelButton.Text = "Закрыть";
      if (this._openEditorCheckBox.Checked)
        this.OpenEditorForRootCreatedObject();
      if (!this._openInNewNavigatorWindowCheckBox.Checked)
        return;
      this.OpenRootCreatedObjectInNewNavigatorWindow();
    }
    catch (CompositionCopyingWizardControl.CompositionCopyingAbortException ex)
    {
      long[] array = this._newObjects.Values.ToArray<long>();
      List<ObjectBlank> list = new List<ObjectBlank>();
      foreach (ObjectBlank objectBlank1 in this._blanksBackup)
      {
        ObjectBlank blank = objectBlank1;
        ObjectBlank objectBlank2 = this._groupAttributesChangingControl.Objects.FirstOrDefault<ObjectBlank>((System.Func<ObjectBlank, bool>) (o => o.ObjectVersionID == blank.ObjectVersionID));
        if (objectBlank2 != null)
          list.Add(objectBlank2.Clone());
        else
          list.Add(blank);
      }
      this._groupAttributesChangingControl.Objects = new BindingList<ObjectBlank>((IList<ObjectBlank>) list);
      this._excludeFromCompositionForAll = false;
      this._useExistingObjectForAll = false;
      this._newObjects = new Dictionary<long, long>();
      this._newObjectsWithComposition = new List<long>();
      this._needToUseExistingObjects = new List<long>();
      this._needToExcludeObjects = new List<long>();
      if (array.Length == 0)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        using (NotificationContext.Create(sessionKeeper.Session, (object) this))
          ((ICompositionCopyingServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionCopyingServerService))).RemoveObjects(sessionKeeper.Session.SessionGUID, array);
      }
    }
  }

  private void CancelButton_Click(object sender, EventArgs e)
  {
    EventHandler cancelButtonClicked = this.CancelButtonClicked;
    if (cancelButtonClicked == null)
      return;
    cancelButtonClicked((object) this, EventArgs.Empty);
  }

  private void MakeInstance(long objectVersionID, long needingMakeInstanceVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ((IInstancesServerService) sessionKeeper.Session.GetCustomService(typeof (IInstancesServerService))).MakeInstance(sessionKeeper.Session.SessionGUID, objectVersionID, needingMakeInstanceVersionID);
  }

  private long CreateObjectWithCompositionByPrototype(long prototypeVersionId)
  {
    long objectByPrototype = this.CreateObjectByPrototype(prototypeVersionId);
    if (!ObjectHelper.IsUnknownObjectVersionID(objectByPrototype) && this._newObjects.ContainsKey(prototypeVersionId) && this._newObjects[prototypeVersionId] == objectByPrototype && !this._newObjectsWithComposition.Contains(objectByPrototype))
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ICompositionCopyingServerService customService = (ICompositionCopyingServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionCopyingServerService));
        FindCompositionParams @params = new FindCompositionParams(prototypeVersionId)
        {
          CompositionContexts = this.GetCompositionContexts(),
          FiltrationOwnerID = this._filtrationService.FiltrationServiceOwnerID,
          RecordSetParams = new DBRecordSetParams()
          {
            Columns = new object[3]
            {
              (object) ObligatoryObjectAttributes.F_PRJLINK_ID,
              (object) ObligatoryObjectAttributes.F_OBJECT_ID,
              (object) ObligatoryObjectAttributes.F_OBJECT_TYPE
            }
          },
          RelationTypes = this._compositionCopyingControl.RelationTypes
        };
        DataTable composition1 = customService.FindComposition(sessionKeeper.Session.SessionGUID, @params);
        List<Tuple<long, long>> tupleList = new List<Tuple<long, long>>();
        if (composition1 != null)
        {
          foreach (DataRow row in (InternalDataCollectionBase) composition1.Rows)
          {
            long int64Value = DataSetProcessor.GetInt64Value(row, 0, 0L);
            if (!((IEnumerable<long>) this._excluded).Contains<long>(int64Value) && !((IEnumerable<long>) this._excluded).Contains<long>(-int64Value))
            {
              long compositionByPrototype = this.CreateObjectWithCompositionByPrototype(DataSetProcessor.GetInt64Value(row, 1, 0L));
              if (!ObjectHelper.IsUnknownObjectVersionID(compositionByPrototype))
                tupleList.Add(new Tuple<long, long>(int64Value, compositionByPrototype));
            }
          }
          if (tupleList.Count > 0)
          {
            string[] composition2 = customService.CreateComposition(sessionKeeper.Session.SessionGUID, objectByPrototype, tupleList.ToArray());
            if (composition2.Length != 0)
            {
              int num = (int) MessageBox.Show($"Во время создания состава oбъекта #{objectByPrototype} по прототипу #{prototypeVersionId} произошли следующие ошибки:" + Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine + Environment.NewLine, composition2), "Intrermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
          }
        }
        this._newObjectsWithComposition.Add(objectByPrototype);
      }
    }
    return objectByPrototype;
  }

  private long CreateObjectByPrototype(long prototypeVersionId)
  {
    if (this._needToExcludeObjects.Contains(prototypeVersionId))
      return 0;
    if (this._needToUseExistingObjects.Contains(prototypeVersionId))
      return prototypeVersionId;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session, (object) this))
      {
        ICompositionCopyingServerService customService = (ICompositionCopyingServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionCopyingServerService));
        ObjectBlank objectBlank = this._groupAttributesChangingControl.Objects.FirstOrDefault<ObjectBlank>((System.Func<ObjectBlank, bool>) (o => o.ObjectVersionID == prototypeVersionId));
        if (objectBlank == null)
          return this._newObjects.ContainsKey(prototypeVersionId) ? this._newObjects[prototypeVersionId] : prototypeVersionId;
        Tuple<ObjectBlank, string> tuple = customService.CreateObject(sessionKeeper.Session.SessionGUID, objectBlank.Clone());
        this.ReplaceBlank(objectBlank, tuple.Item1);
        if (string.IsNullOrEmpty(tuple.Item2))
        {
          this._newObjects.Add(prototypeVersionId, tuple.Item1.ObjectVersionID);
          return tuple.Item1.ObjectVersionID;
        }
        if (this._excludeFromCompositionForAll && prototypeVersionId != this._objectVersionId)
          return 0;
        if (this._useExistingObjectForAll && prototypeVersionId != this._objectVersionId)
          return this.FindObjectWithSameDesignation(objectBlank);
        using (ObjectCreationErrorForm creationErrorForm = new ObjectCreationErrorForm())
        {
          creationErrorForm.PrototypeVersionId = objectBlank.ObjectVersionID;
          creationErrorForm.Error = tuple.Item2;
          if (prototypeVersionId == this._objectVersionId)
          {
            creationErrorForm.Error = $"{tuple.Item2}{Environment.NewLine}(Так как объект является корнем создаваемой иерархии, пропустить его создание невозможно.)";
            creationErrorForm.DisabledActions = ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObject | ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromComposition | ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObjectForAll | ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromCompositionForAll;
          }
          int num = (int) creationErrorForm.ShowDialog();
          if (creationErrorForm.Action == ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromComposition)
          {
            this._needToExcludeObjects.Add(prototypeVersionId);
            return 0;
          }
          if (creationErrorForm.Action == ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromCompositionForAll)
          {
            this._excludeFromCompositionForAll = true;
            return 0;
          }
          if (creationErrorForm.Action == ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObject)
          {
            this._needToUseExistingObjects.Add(prototypeVersionId);
            return this.FindObjectWithSameDesignation(objectBlank);
          }
          if (creationErrorForm.Action != ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObjectForAll)
            throw new CompositionCopyingWizardControl.CompositionCopyingAbortException();
          this._useExistingObjectForAll = true;
          return this.FindObjectWithSameDesignation(objectBlank);
        }
      }
    }
  }

  private long FindObjectWithSameDesignation(ObjectBlank blank)
  {
    string attributeValue = blank.GetAttributeValue(Constants.DesignationAttributeTypeID) as string;
    if (string.IsNullOrEmpty(attributeValue))
      return blank.ObjectVersionID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return ((ICompositionCopyingServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionCopyingServerService))).FindObjectWithDesignation(sessionKeeper.Session.SessionGUID, blank.ObjectTypeID, attributeValue);
  }

  private void ReplaceBlank(ObjectBlank oldBlank, ObjectBlank newBlank)
  {
    int index = this._groupAttributesChangingControl.Objects.IndexOf(oldBlank);
    List<ObjectBlank> list = this._groupAttributesChangingControl.Objects.Select<ObjectBlank, ObjectBlank>((System.Func<ObjectBlank, ObjectBlank>) (o => o.Clone())).ToList<ObjectBlank>();
    if (index >= 0)
    {
      list.RemoveAt(index);
      list.Insert(index, newBlank);
    }
    this._groupAttributesChangingControl.Objects = new BindingList<ObjectBlank>((IList<ObjectBlank>) list);
  }

  private CompositionContext[] GetCompositionContexts()
  {
    return this._filtrationService.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] is IEnumerable ? CompositionContextClientHelper.BuildCompositionContextsBasedOnValues(((IEnumerable) this._filtrationService.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"]).Cast<long>()) : CompositionContextClientHelper.CompositionContextsCommon;
  }

  private void OpenEditorForRootCreatedObject()
  {
    ObjectCommands.EditCommand(SelectedItemsHelper.CreateSelectedItemsForObject(this._newObjects[this._objectVersionId]), (IServiceProvider) ServicesManager.ServiceContainer, (object) null);
  }

  private void OpenRootCreatedObjectInNewNavigatorWindow()
  {
    Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(this._newObjects[this._objectVersionId]), (IServiceProvider) ServicesManager.ServiceContainer);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._tabControl = new TabControl();
    this._compositionCopyingTabPage = new TabPage();
    this._compositionCopyingControl = new CompositionCopyingControl();
    this._groupAttributesChangingTabPage = new TabPage();
    this._groupAttributesChangingControl = new GroupAttributesChangingControl();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this._cancelButton = new Button();
    this._acceptButton = new Button();
    this._nextButton = new Button();
    this.flowLayoutPanel2 = new FlowLayoutPanel();
    this._openEditorCheckBox = new CheckBox();
    this._openInNewNavigatorWindowCheckBox = new CheckBox();
    this._tabControl.SuspendLayout();
    this._compositionCopyingTabPage.SuspendLayout();
    this._compositionCopyingControl.BeginInit();
    this._groupAttributesChangingTabPage.SuspendLayout();
    ((ISupportInitialize) this._groupAttributesChangingControl).BeginInit();
    this.tableLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel2.SuspendLayout();
    this.SuspendLayout();
    this._tabControl.Controls.Add((Control) this._compositionCopyingTabPage);
    this._tabControl.Controls.Add((Control) this._groupAttributesChangingTabPage);
    this._tabControl.Dock = DockStyle.Fill;
    this._tabControl.Location = new Point(3, 3);
    this._tabControl.Name = "_tabControl";
    this._tabControl.SelectedIndex = 0;
    this._tabControl.Size = new Size(621, 268);
    this._tabControl.TabIndex = 0;
    this._compositionCopyingTabPage.Controls.Add((Control) this._compositionCopyingControl);
    this._compositionCopyingTabPage.Location = new Point(4, 22);
    this._compositionCopyingTabPage.Name = "_compositionCopyingTabPage";
    this._compositionCopyingTabPage.Padding = new Padding(3);
    this._compositionCopyingTabPage.Size = new Size(613, 242);
    this._compositionCopyingTabPage.TabIndex = 0;
    this._compositionCopyingTabPage.Text = "Выбор копируемых объектов";
    this._compositionCopyingTabPage.UseVisualStyleBackColor = true;
    this._compositionCopyingControl.Dock = DockStyle.Fill;
    this._compositionCopyingControl.Location = new Point(3, 3);
    this._compositionCopyingControl.Name = "_compositionCopyingControl";
    this._compositionCopyingControl.Size = new Size(607, 236);
    this._compositionCopyingControl.TabIndex = 0;
    this._groupAttributesChangingTabPage.Controls.Add((Control) this._groupAttributesChangingControl);
    this._groupAttributesChangingTabPage.Location = new Point(4, 22);
    this._groupAttributesChangingTabPage.Name = "_groupAttributesChangingTabPage";
    this._groupAttributesChangingTabPage.Padding = new Padding(3);
    this._groupAttributesChangingTabPage.Size = new Size(613, 242);
    this._groupAttributesChangingTabPage.TabIndex = 1;
    this._groupAttributesChangingTabPage.Text = "Изменение атрибутов";
    this._groupAttributesChangingTabPage.UseVisualStyleBackColor = true;
    this._groupAttributesChangingControl.Dock = DockStyle.Fill;
    this._groupAttributesChangingControl.Location = new Point(3, 3);
    this._groupAttributesChangingControl.Name = "_groupAttributesChangingControl";
    this._groupAttributesChangingControl.Size = new Size(607, 236);
    this._groupAttributesChangingControl.TabIndex = 0;
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this._tabControl, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel2, 0, 1);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 3;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(627, 338);
    this.tableLayoutPanel1.TabIndex = 1;
    this.flowLayoutPanel1.AutoSize = true;
    this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.flowLayoutPanel1.Controls.Add((Control) this._cancelButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._acceptButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._nextButton);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
    this.flowLayoutPanel1.Location = new Point(3, 306);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(621, 29);
    this.flowLayoutPanel1.TabIndex = 1;
    this._cancelButton.Location = new Point(543, 3);
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.Size = new Size(75, 23);
    this._cancelButton.TabIndex = 1;
    this._cancelButton.Text = "Отмена";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._cancelButton.Click += new EventHandler(this.CancelButton_Click);
    this._acceptButton.Location = new Point(462, 3);
    this._acceptButton.Name = "_acceptButton";
    this._acceptButton.Size = new Size(75, 23);
    this._acceptButton.TabIndex = 1;
    this._acceptButton.Text = "Применить";
    this._acceptButton.UseVisualStyleBackColor = true;
    this._acceptButton.Click += new EventHandler(this.AcceptButton_Click);
    this._nextButton.Location = new Point(381, 3);
    this._nextButton.Name = "_nextButton";
    this._nextButton.Size = new Size(75, 23);
    this._nextButton.TabIndex = 1;
    this._nextButton.Text = "Далее >";
    this._nextButton.UseVisualStyleBackColor = true;
    this._nextButton.Click += new EventHandler(this.NextButton_Click);
    this.flowLayoutPanel2.AutoSize = true;
    this.flowLayoutPanel2.Controls.Add((Control) this._openEditorCheckBox);
    this.flowLayoutPanel2.Controls.Add((Control) this._openInNewNavigatorWindowCheckBox);
    this.flowLayoutPanel2.Dock = DockStyle.Fill;
    this.flowLayoutPanel2.Location = new Point(3, 277);
    this.flowLayoutPanel2.Name = "flowLayoutPanel2";
    this.flowLayoutPanel2.Size = new Size(621, 23);
    this.flowLayoutPanel2.TabIndex = 2;
    this._openEditorCheckBox.AutoSize = true;
    this._openEditorCheckBox.Checked = true;
    this._openEditorCheckBox.CheckState = CheckState.Checked;
    this._openEditorCheckBox.Location = new Point(3, 3);
    this._openEditorCheckBox.Name = "_openEditorCheckBox";
    this._openEditorCheckBox.Size = new Size(204, 17);
    this._openEditorCheckBox.TabIndex = 0;
    this._openEditorCheckBox.Text = "Открыть редактор после создания";
    this._openEditorCheckBox.UseVisualStyleBackColor = true;
    this._openInNewNavigatorWindowCheckBox.AutoSize = true;
    this._openInNewNavigatorWindowCheckBox.Checked = true;
    this._openInNewNavigatorWindowCheckBox.CheckState = CheckState.Checked;
    this._openInNewNavigatorWindowCheckBox.Location = new Point(213, 3);
    this._openInNewNavigatorWindowCheckBox.Name = "_openInNewNavigatorWindowCheckBox";
    this._openInNewNavigatorWindowCheckBox.Size = new Size(240 /*0xF0*/, 17);
    this._openInNewNavigatorWindowCheckBox.TabIndex = 1;
    this._openInNewNavigatorWindowCheckBox.Text = "Открыть созданный объект в Навигаторе";
    this._openInNewNavigatorWindowCheckBox.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (CompositionCopyingWizardControl);
    this.Size = new Size(627, 338);
    this._tabControl.ResumeLayout(false);
    this._compositionCopyingTabPage.ResumeLayout(false);
    this._compositionCopyingControl.EndInit();
    this._groupAttributesChangingTabPage.ResumeLayout(false);
    ((ISupportInitialize) this._groupAttributesChangingControl).EndInit();
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.flowLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel2.ResumeLayout(false);
    this.flowLayoutPanel2.PerformLayout();
    this.ResumeLayout(false);
  }

  [Serializable]
  public sealed class CompositionCopyingWizardControlMemento
  {
    public object CompositionCopyingControlState { get; set; }

    public object GroupAttributesChangingControlState { get; set; }
  }

  private sealed class CompositionCopyingAbortException : Exception
  {
  }
}
