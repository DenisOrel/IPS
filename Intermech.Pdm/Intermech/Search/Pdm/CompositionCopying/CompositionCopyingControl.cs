// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionCopyingControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Search.Pdm.Instances;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public sealed class CompositionCopyingControl : UserControl, ISupportInitialize
{
  private ICurrentUserAndRole _currentUserAndRole;
  private long _objectVersionID;
  private IContainer components;
  private Column _copyColumn;
  private SplitContainer _splitContainer;
  private TableLayoutPanel tableLayoutPanel7;
  private Column column1;
  private CompositionFilterControl _compositionFilterControl;
  private CopyingItemsControl _copyingItemsControl;
  private CompositionCopyingTreeControl _tree;

  public CompositionCopyingControl()
  {
    this.InitializeComponent();
    this.UpdateControls();
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] AllowableForCreateCopyObjectTypes
  {
    get => this._tree.AllowableForCreateCopyObjectTypes;
    set => this._tree.AllowableForCreateCopyObjectTypes = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long ObjectVersionID
  {
    get => this._objectVersionID;
    set
    {
      if (ObjectHelper.IsUnknownObjectVersionID(value))
        throw new ArgumentException();
      if (this._objectVersionID == value)
        return;
      this._objectVersionID = value;
      this._compositionFilterControl.ConsiderInstancesCheckBoxEnabled = this.IsProduct(this.GetObjectType(this._objectVersionID));
      this._tree.ObjectVersionID = this._objectVersionID;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool ConsiderInstances => this._compositionFilterControl.ConsiderInstances;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long[] Instances { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] RelationTypes
  {
    get => this._tree.RelationTypes;
    set => this._tree.RelationTypes = value;
  }

  public void Initialize(
    ICurrentUserAndRole currentUserAndRole,
    IFiltrationService filtrationService,
    INavigatorClientService navigatorClientService)
  {
    if (currentUserAndRole == null)
      throw new ArgumentNullException(nameof (currentUserAndRole));
    if (filtrationService == null)
      throw new ArgumentNullException(nameof (filtrationService));
    if (navigatorClientService == null)
      throw new ArgumentNullException(nameof (navigatorClientService));
    this._currentUserAndRole = currentUserAndRole;
    this._tree.Initialize(filtrationService, navigatorClientService);
  }

  public long[] GetCopies() => this._tree.GetCopies();

  public long[] GetExcluded() => this._tree.GetExcluded();

  public object CreateMemento()
  {
    return (object) new CompositionCopyingControl.CompositionCopyingControlMemento()
    {
      SplitterDistance = ((double) this._splitContainer.SplitterDistance / (double) this._splitContainer.Width),
      TreeState = this._tree.CreateMemento()
    };
  }

  public void SetMemento(object memento)
  {
    CompositionCopyingControl.CompositionCopyingControlMemento copyingControlMemento = memento is CompositionCopyingControl.CompositionCopyingControlMemento ? (CompositionCopyingControl.CompositionCopyingControlMemento) memento : throw new ArgumentException();
    this._splitContainer.SplitterDistance = (int) ((double) this._splitContainer.Width * copyingControlMemento.SplitterDistance);
    this._tree.SetMemento(copyingControlMemento.TreeState);
  }

  public void BeginInit()
  {
  }

  public void EndInit()
  {
  }

  private void CompositionFilterControl_ConsiderInstancesChanged(object sender, EventArgs e)
  {
    if (this.Instances == null)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        this.Instances = ((IInstancesServerService) sessionKeeper.Session.GetCustomService(typeof (IInstancesServerService))).FindInstances(sessionKeeper.Session.SessionGUID, this._objectVersionID);
      if (this.Instances.Length != 0)
        this._tree.Instances = this.Instances;
    }
    this._tree.ConsiderInstances = this._compositionFilterControl.ConsiderInstances;
  }

  private void CopyingItemsControl_SelectButtonClicked(object sender, EventArgs e)
  {
    this._tree.FindNext(this._copyingItemsControl.FindWhat);
  }

  private void CopyingItemsControl_SelectAllButtonClicked(object sender, EventArgs e)
  {
    this._tree.FindAll(this._copyingItemsControl.FindWhat);
  }

  private void CopyingItemsControl_CheckUncheckButtonClicked(object sender, EventArgs e)
  {
    this._tree.InverseSelectedCopies();
  }

  private void Tree_SelectionChanged(object sender, EventArgs e) => this.UpdateControls();

  private void UpdateControls()
  {
    this._copyingItemsControl.CheckUncheckButtonEnabled = this._tree.HasSelectedItems;
  }

  private int GetObjectType(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(objectVersionID).ObjectType;
  }

  private bool IsProduct(int objectType)
  {
    List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(Constants.ProductObjectTypeID);
    return objectType == Constants.ProductObjectTypeID || childrenIdRecursive.Contains(objectType);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._copyColumn = new Column();
    this._splitContainer = new SplitContainer();
    this.tableLayoutPanel7 = new TableLayoutPanel();
    this._compositionFilterControl = new CompositionFilterControl();
    this._copyingItemsControl = new CopyingItemsControl();
    this._tree = new CompositionCopyingTreeControl();
    this.column1 = new Column();
    this._splitContainer.BeginInit();
    this._splitContainer.Panel1.SuspendLayout();
    this._splitContainer.Panel2.SuspendLayout();
    this._splitContainer.SuspendLayout();
    this.tableLayoutPanel7.SuspendLayout();
    this.SuspendLayout();
    this._copyColumn.Caption = "Копия";
    this._copyColumn.Name = "_copyColumn";
    this._splitContainer.Dock = DockStyle.Fill;
    this._splitContainer.Location = new Point(0, 0);
    this._splitContainer.Name = "_splitContainer";
    this._splitContainer.Panel1.Controls.Add((Control) this.tableLayoutPanel7);
    this._splitContainer.Panel2.Controls.Add((Control) this._tree);
    this._splitContainer.Size = new Size(772, 515);
    this._splitContainer.SplitterDistance = 325;
    this._splitContainer.TabIndex = 1;
    this.tableLayoutPanel7.AutoSize = true;
    this.tableLayoutPanel7.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.tableLayoutPanel7.ColumnCount = 1;
    this.tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel7.Controls.Add((Control) this._copyingItemsControl, 0, 0);
    this.tableLayoutPanel7.Controls.Add((Control) this._compositionFilterControl, 0, 1);
    this.tableLayoutPanel7.Dock = DockStyle.Fill;
    this.tableLayoutPanel7.Location = new Point(0, 0);
    this.tableLayoutPanel7.Name = "tableLayoutPanel7";
    this.tableLayoutPanel7.RowCount = 2;
    this.tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel7.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel7.Size = new Size(325, 515);
    this.tableLayoutPanel7.TabIndex = 0;
    this._compositionFilterControl.AutoSize = true;
    this._compositionFilterControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this._compositionFilterControl.Dock = DockStyle.Fill;
    this._compositionFilterControl.Location = new Point(3, 489);
    this._compositionFilterControl.Name = "_compositionFilterControl";
    this._compositionFilterControl.Size = new Size(319, 23);
    this._compositionFilterControl.TabIndex = 2;
    this._compositionFilterControl.ConsiderInstancesChanged += new EventHandler(this.CompositionFilterControl_ConsiderInstancesChanged);
    this._copyingItemsControl.AutoSize = true;
    this._copyingItemsControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this._copyingItemsControl.CheckUncheckButtonEnabled = true;
    this._copyingItemsControl.Dock = DockStyle.Fill;
    this._copyingItemsControl.Location = new Point(3, 3);
    this._copyingItemsControl.Name = "_copyingItemsControl";
    this._copyingItemsControl.Size = new Size(319, 480);
    this._copyingItemsControl.TabIndex = 3;
    this._copyingItemsControl.SelectButtonClicked += new EventHandler(this.CopyingItemsControl_SelectButtonClicked);
    this._copyingItemsControl.SelectAllButtonClicked += new EventHandler(this.CopyingItemsControl_SelectAllButtonClicked);
    this._copyingItemsControl.CheckUncheckButtonClicked += new EventHandler(this.CopyingItemsControl_CheckUncheckButtonClicked);
    this._tree.Dock = DockStyle.Fill;
    this._tree.Location = new Point(0, 0);
    this._tree.Name = "_tree";
    this._tree.Size = new Size(443, 515);
    this._tree.TabIndex = 0;
    this._tree.SelectionChanged += new EventHandler(this.Tree_SelectionChanged);
    this.column1.Caption = (string) null;
    this.column1.Name = "column1";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._splitContainer);
    this.Name = nameof (CompositionCopyingControl);
    this.Size = new Size(772, 515);
    this._splitContainer.Panel1.ResumeLayout(false);
    this._splitContainer.Panel1.PerformLayout();
    this._splitContainer.Panel2.ResumeLayout(false);
    this._splitContainer.EndInit();
    this._splitContainer.ResumeLayout(false);
    this.tableLayoutPanel7.ResumeLayout(false);
    this.tableLayoutPanel7.PerformLayout();
    this.ResumeLayout(false);
  }

  [Serializable]
  private sealed class CompositionCopyingControlMemento
  {
    public double SplitterDistance { get; set; }

    public object TreeState { get; set; }
  }
}
