// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeSettingsForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class OfficeSettingsForm : Form
{
  internal long _Unit;
  [CanBeNull]
  private Dictionary<int, OfficeDocumentTypeSettingsForUnit> _settings;
  [NotNull]
  private readonly Dictionary<OfficeDocumentTypes, FormulaControl> _controls;
  [CanBeNull]
  private Tuple<int, string> _currentType;
  private bool _selfOffice;
  private IContainer components;
  private SplitContainer splitContainer1;
  private TreeView tvObjectTypes;
  private GroupBox groupBox1;
  private TabControl tabControl1;
  private TabPage tpInput;
  private TabPage tpOutput;
  private TabPage tpInternal;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private Panel panel2;
  private Label lNoSelfOffice;

  public OfficeSettingsForm(long unitID, [CanBeNull] IViewState viewState)
  {
    this.InitializeComponent();
    this.tvObjectTypes.ImageList = Holder.IconService.ImageList;
    this._Unit = unitID;
    EventHandler modifyMethod = new EventHandler(this.SetModified);
    this._controls = new Dictionary<OfficeDocumentTypes, FormulaControl>(3);
    FormulaControl formulaControl1 = new FormulaControl(modifyMethod, unitID, OfficeDocumentTypes.Incoming);
    this._controls.Add(OfficeDocumentTypes.Incoming, formulaControl1);
    this.tpInput.Controls.Add((Control) formulaControl1);
    FormulaControl formulaControl2 = new FormulaControl(modifyMethod, unitID, OfficeDocumentTypes.Outgoing);
    this._controls.Add(OfficeDocumentTypes.Outgoing, formulaControl2);
    this.tpOutput.Controls.Add((Control) formulaControl2);
    FormulaControl formulaControl3 = new FormulaControl(modifyMethod, unitID, OfficeDocumentTypes.Internal);
    this._controls.Add(OfficeDocumentTypes.Internal, formulaControl3);
    this.tpInternal.Controls.Add((Control) formulaControl3);
  }

  private void SetModified([NotNull] object sender, [NotNull] EventArgs e)
  {
    if (!this.IsModified)
      this.IsModified = true;
    this.RefreshControls();
  }

  [NotNull]
  private static TreeNode AddNode([NotNull] TreeNodeCollection parent, int typeID, bool checkChild)
  {
    IMSObjectType objectType = MetaDataHelper.GetObjectType(typeID);
    TreeNode treeNode = parent.Add(objectType.ObjectTypeName);
    treeNode.ImageIndex = treeNode.SelectedImageIndex = Holder.IconService.IndexOf(4, objectType.ObjectTypeID);
    treeNode.Tag = (object) objectType.ObjectTypeID;
    if (checkChild && MetaDataHelper.GetObjectTypeChildrenID(objectType.ObjectTypeID).Count > 0)
      treeNode.Nodes.Add(OfficeSettingsForm.EmptyNode);
    return treeNode;
  }

  private void RefreshControls()
  {
    this.bOK.Enabled = this.bCancel.Enabled = this.IsModified;
    this.splitContainer1.Visible = this.bOK.Visible = this.bCancel.Visible = this._selfOffice;
  }

  [NotNull]
  private static TreeNode EmptyNode
  {
    get => new TreeNode(string.Empty) { Tag = (object) -1 };
  }

  private void BuildTree()
  {
    this.tvObjectTypes.Nodes.Clear();
    IMSObjectType objectType = MetaDataHelper.GetObjectType(OfficeConsts.ObjtypeDocumentsID);
    TreeNode treeNode = OfficeSettingsForm.AddNode(this.tvObjectTypes.Nodes, objectType.ObjectTypeID, false);
    foreach (int typeID in MetaDataHelper.GetObjectTypeChildrenID(objectType.ObjectTypeID))
      OfficeSettingsForm.AddNode(treeNode.Nodes, typeID, true);
    treeNode.Expand();
  }

  private void tvObjectTypes_BeforeExpand([CanBeNull] object sender, [NotNull] TreeViewCancelEventArgs e)
  {
    if (e.Node.Nodes.Count != 1 || (int) e.Node.Nodes[0].Tag != -1)
      return;
    e.Node.Nodes.Clear();
    foreach (int typeID in MetaDataHelper.GetObjectTypeChildrenID((int) e.Node.Tag))
      OfficeSettingsForm.AddNode(e.Node.Nodes, typeID, true);
  }

  public void Reload()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._Unit);
      this.BuildTree();
      this._settings = sessionKeeper.Session.GetCustomService<IOfficeDocumentTypeService>().GetTypeSettingsForUnit(this._Unit) ?? new Dictionary<int, OfficeDocumentTypeSettingsForUnit>();
      this._selfOffice = false;
      IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrSelfOfficeID);
      if ((attributeById != null ? (attributeById.AsBoolean ? 1 : 0) : 0) != 0)
        this._selfOffice = true;
      this.lNoSelfOffice.Text = dbObject.NameInMessages + " не имеет собственной канцелярии";
    }
    this.tvObjectTypes.SelectedNode = this.tvObjectTypes.Nodes[0];
    this.SetModified(false);
    this.RefreshControls();
  }

  private void SetModified(bool value)
  {
    this.IsModified = value;
    this._controls[OfficeDocumentTypes.Incoming].Changed = value;
    this._controls[OfficeDocumentTypes.Outgoing].Changed = value;
    this._controls[OfficeDocumentTypes.Internal].Changed = value;
  }

  public void Save()
  {
    if (this._currentType == null)
      return;
    OfficeDocumentTypeSettingsForUnit typeSettingsForUnit = new OfficeDocumentTypeSettingsForUnit(new (OfficeDocumentTypes, RegNumberSettings)[3]
    {
      (OfficeDocumentTypes.Incoming, this._controls[OfficeDocumentTypes.Incoming].Template),
      (OfficeDocumentTypes.Outgoing, this._controls[OfficeDocumentTypes.Outgoing].Template),
      (OfficeDocumentTypes.Internal, this._controls[OfficeDocumentTypes.Internal].Template)
    });
    bool flag = false;
    RegNumberSettings regNumberSettings = new RegNumberSettings();
    if (this._settings != null)
    {
      if (!typeSettingsForUnit.Templates[OfficeDocumentTypes.Incoming].Equals((object) regNumberSettings) || !typeSettingsForUnit.Templates[OfficeDocumentTypes.Outgoing].Equals((object) regNumberSettings) || !typeSettingsForUnit.Templates[OfficeDocumentTypes.Internal].Equals((object) regNumberSettings))
      {
        if (this._settings.ContainsKey(this._currentType.Item1))
          this._settings[this._currentType.Item1] = typeSettingsForUnit;
        else
          this._settings.Add(this._currentType.Item1, typeSettingsForUnit);
      }
      else
      {
        if (this._settings.ContainsKey(this._currentType.Item1))
          this._settings.Remove(this._currentType.Item1);
        flag = true;
      }
      List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(this._currentType.Item1);
      if (childrenIdRecursive.Count > 1 && MessageBox.Show($"Наследовать настройки типа {this._currentType.Item2} дочерним типам?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        foreach (int key in childrenIdRecursive)
        {
          if (key != this._currentType.Item1)
          {
            if (!flag)
            {
              if (this._settings.ContainsKey(key))
                this._settings[key] = typeSettingsForUnit.Clone();
              else
                this._settings.Add(key, typeSettingsForUnit.Clone());
            }
            else if (this._settings.ContainsKey(key))
              this._settings.Remove(key);
          }
        }
      }
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        sessionKeeper.Session.GetCustomService<IOfficeDocumentTypeService>().SetTypeSettingsForUnit(this._Unit, this._settings);
    }
    this.SetModified(false);
    this.RefreshControls();
  }

  public void SetParent([CanBeNull] Control aParent)
  {
    if (aParent == null)
    {
      this.AutoScaleMode = AutoScaleMode.None;
      this.TopLevel = true;
      this.Dock = DockStyle.None;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.Visible = false;
    }
    else
    {
      this.AutoScaleMode = AutoScaleMode.None;
      this.TopLevel = false;
      this.Dock = DockStyle.Fill;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Visible = true;
    }
    this.Parent = aParent;
  }

  public bool IsModified { get; private set; }

  private void tvObjectTypes_AfterSelect([CanBeNull] object sender, [NotNull] TreeViewEventArgs e)
  {
    if (this._settings != null)
    {
      OfficeDocumentTypeSettingsForUnit typeSettingsForUnit;
      if (this._settings.TryGetValue((int) e.Node.Tag, out typeSettingsForUnit))
      {
        foreach (KeyValuePair<OfficeDocumentTypes, RegNumberSettings> template in typeSettingsForUnit.Templates)
          this._controls[template.Key].SetData(template.Value, (int) e.Node.Tag);
      }
      else
        this.SetEmpty();
    }
    this._currentType = new Tuple<int, string>((int) e.Node.Tag, e.Node.Text);
    this.RefreshControls();
  }

  private void tvObjectTypes_BeforeSelect([CanBeNull] object sender, [NotNull] TreeViewCancelEventArgs e)
  {
    if (this._currentType == null || !this.IsModified)
      return;
    if (MessageBox.Show($"Для типа \"{this._currentType.Item2}\" остались несохраненные данные. Сохранить?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      this.Save();
    else
      this.SetModified(false);
  }

  private void SetEmpty()
  {
    this._controls[OfficeDocumentTypes.Incoming].SetData(new RegNumberSettings(), -1);
    this._controls[OfficeDocumentTypes.Outgoing].SetData(new RegNumberSettings(), -1);
    this._controls[OfficeDocumentTypes.Internal].SetData(new RegNumberSettings(), -1);
  }

  private void bOK_Click([CanBeNull] object sender, [NotNull] EventArgs e) => this.Save();

  private void bCancel_Click([CanBeNull] object sender, [NotNull] EventArgs e) => this.Reload();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.splitContainer1 = new SplitContainer();
    this.tvObjectTypes = new TreeView();
    this.groupBox1 = new GroupBox();
    this.tabControl1 = new TabControl();
    this.tpInput = new TabPage();
    this.tpOutput = new TabPage();
    this.tpInternal = new TabPage();
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panel2 = new Panel();
    this.lNoSelfOffice = new Label();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.tabControl1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.tvObjectTypes);
    this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox1);
    this.splitContainer1.Size = new Size(997, 561);
    this.splitContainer1.SplitterDistance = 294;
    this.splitContainer1.TabIndex = 1;
    this.tvObjectTypes.Dock = DockStyle.Fill;
    this.tvObjectTypes.Location = new Point(0, 0);
    this.tvObjectTypes.Name = "tvObjectTypes";
    this.tvObjectTypes.Size = new Size(294, 561);
    this.tvObjectTypes.TabIndex = 0;
    this.tvObjectTypes.BeforeExpand += new TreeViewCancelEventHandler(this.tvObjectTypes_BeforeExpand);
    this.tvObjectTypes.BeforeSelect += new TreeViewCancelEventHandler(this.tvObjectTypes_BeforeSelect);
    this.tvObjectTypes.AfterSelect += new TreeViewEventHandler(this.tvObjectTypes_AfterSelect);
    this.groupBox1.Controls.Add((Control) this.tabControl1);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.ForeColor = SystemColors.HotTrack;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(699, 561);
    this.groupBox1.TabIndex = 3;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Генерация регистрационного номера ";
    this.tabControl1.Controls.Add((Control) this.tpInput);
    this.tabControl1.Controls.Add((Control) this.tpOutput);
    this.tabControl1.Controls.Add((Control) this.tpInternal);
    this.tabControl1.Dock = DockStyle.Fill;
    this.tabControl1.Location = new Point(3, 16 /*0x10*/);
    this.tabControl1.Name = "tabControl1";
    this.tabControl1.SelectedIndex = 0;
    this.tabControl1.Size = new Size(693, 542);
    this.tabControl1.TabIndex = 0;
    this.tpInput.BackColor = SystemColors.Control;
    this.tpInput.Location = new Point(4, 22);
    this.tpInput.Name = "tpInput";
    this.tpInput.Padding = new Padding(3);
    this.tpInput.Size = new Size(685, 516);
    this.tpInput.TabIndex = 0;
    this.tpInput.Text = "Входящие";
    this.tpOutput.BackColor = SystemColors.Control;
    this.tpOutput.Location = new Point(4, 22);
    this.tpOutput.Name = "tpOutput";
    this.tpOutput.Padding = new Padding(3);
    this.tpOutput.Size = new Size(539, 404);
    this.tpOutput.TabIndex = 1;
    this.tpOutput.Text = "Исходящие";
    this.tpInternal.BackColor = SystemColors.Control;
    this.tpInternal.Location = new Point(4, 22);
    this.tpInternal.Name = "tpInternal";
    this.tpInternal.Padding = new Padding(3);
    this.tpInternal.Size = new Size(539, 404);
    this.tpInternal.TabIndex = 2;
    this.tpInternal.Text = "Внутренние";
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 561);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(997, 54);
    this.panel1.TabIndex = 2;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(864, 11);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 2;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bCancel.Click += new EventHandler(this.bCancel_Click);
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.Location = new Point(737, 11);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 1;
    this.bOK.Text = "Применить";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    this.panel2.Controls.Add((Control) this.splitContainer1);
    this.panel2.Controls.Add((Control) this.lNoSelfOffice);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(997, 561);
    this.panel2.TabIndex = 3;
    this.lNoSelfOffice.Dock = DockStyle.Fill;
    this.lNoSelfOffice.Location = new Point(0, 0);
    this.lNoSelfOffice.Name = "lNoSelfOffice";
    this.lNoSelfOffice.Size = new Size(997, 561);
    this.lNoSelfOffice.TabIndex = 2;
    this.lNoSelfOffice.Text = "Подразделение не имеет собственной канцелярии";
    this.lNoSelfOffice.TextAlign = ContentAlignment.MiddleCenter;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(997, 615);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (OfficeSettingsForm);
    this.Text = nameof (OfficeSettingsForm);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.tabControl1.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
