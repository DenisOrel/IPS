// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalSelectionDialog
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using DevExpress.IM.XtraEditors;
using DevExpress.IM.XtraEditors.Controls;
using Intermech.Client.Core;
using Intermech.Client.Core.History;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class PortalSelectionDialog : Form
{
  private long _selObjectID;
  private bool _isModified;
  private IPortalMetadata _portalMetadata;
  private IAttributePropertyDescriberService _attrDescriberService;
  private IContainer components;
  private Panel panel2;
  private Label label4;
  private TreeView tvObjects;
  private Button btnAdd;
  private Button btnDelete;
  private Label label3;
  private ButtonEdit buttonEdit1;
  private Panel panel3;
  private TextBox textBox1;
  private Label label8;
  private Label label5;
  private Label label6;
  private Button btnCancel;
  private Button btnApply;

  public bool IsModified
  {
    get => this._isModified;
    set
    {
      this._isModified = value;
      this.btnApply.Enabled = this.btnCancel.Enabled = this._isModified;
    }
  }

  public PortalSelectionDialog()
  {
    this.InitializeComponent();
    this._attrDescriberService = ServicesManager.GetService(typeof (IAttributePropertyDescriberService)) as IAttributePropertyDescriberService;
    this._portalMetadata = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    this.tvObjects.ImageList = Statics.IconSrv?.ImageList;
    this.tvObjects.Nodes[0].Text = LocalizationHolder.rm.GetString("Site.Client_32");
    this.tvObjects.Nodes[0].ImageIndex = this.tvObjects.Nodes[0].SelectedImageIndex = Statics.IconSrv.IndexOf(Statics.CategoryObjectTypes, 0);
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1736);
  }

  public void SetParent(Control parent) => this.SetParent(parent, false);

  public void SetParent(Control parent, bool visible)
  {
    if (parent == null)
      return;
    this.TopLevel = false;
    this.Dock = DockStyle.Fill;
    this.FormBorderStyle = FormBorderStyle.None;
    this.Visible = true;
    this.Parent = parent;
    this.btnApply.Visible = this.btnCancel.Visible = visible;
  }

  private void AddObjectTypeInTree(int typeID)
  {
    this.tvObjects.BeginUpdate();
    try
    {
      bool flag = false;
      foreach (TreeNode node in this.tvObjects.Nodes[0].Nodes)
      {
        if (((PortalObjectType) node.Tag).ID == typeID)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      PortalObjectType publishObjectType = this._portalMetadata.GetPublishObjectType(typeID);
      TreeNode node1 = new TreeNode(publishObjectType.Name)
      {
        Tag = (object) publishObjectType
      };
      node1.ImageIndex = node1.SelectedImageIndex = Statics.IconSrv.IndexOf(SiteClientConsts.CategoryPublishType, typeID);
      this.tvObjects.Nodes[0].Nodes.Add(node1);
      this.IsModified = true;
    }
    finally
    {
      this.tvObjects.Sort();
      this.tvObjects.EndUpdate();
      this.tvObjects.ExpandAll();
    }
  }

  private void buttonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
  {
    using (ObjectsHistory objectsHistory = new ObjectsHistory((object) this._selObjectID, AttributableElements.Object, (object) MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545")))
    {
      objectsHistory.SelectedValue = (object) this.buttonEdit1.Text.Trim();
      if (objectsHistory.ShowDialog() != DialogResult.OK)
        return;
      this.buttonEdit1.Text = (string) objectsHistory.SelectedValue;
      this.IsModified = true;
    }
  }

  public void SelectionLoad(long selID)
  {
    this._selObjectID = selID;
    this.label4.Text = MetaDataHelper.GetObjectType(PortalConsts.objtypePortalSelections).ObjectName;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject selObject = sessionKeeper.Session.GetObject(this._selObjectID, true);
      IDBObject dbObject = sessionKeeper.Session.GetObject(selObject.OwnerID, false);
      this.label6.Text = dbObject == null ? string.Empty : dbObject.Caption;
      IDBAttribute attributeByGuid = selObject.GetAttributeByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545"), false);
      this.textBox1.Text = attributeByGuid == null ? string.Empty : attributeByGuid.AsString;
      this.buttonEdit1.Text = selObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"), true).AsString;
      this.ObjectsLoad(selObject);
    }
    this.IsModified = false;
  }

  private void ObjectsLoad(IDBObject selObject)
  {
    this.tvObjects.Nodes[0].Nodes.Clear();
    this.tvObjects.Nodes[0].Text = LocalizationHolder.rm.GetString("Site.Client_32");
    this.tvObjects.Nodes[0].ImageIndex = this.tvObjects.Nodes[0].SelectedImageIndex = Statics.IconSrv.IndexOf(Statics.CategoryObjectTypes, 0);
    using (new SessionKeeper())
    {
      IDBAttribute attributeByGuid = selObject.GetAttributeByGuid(PortalConsts.attributePortalObjectTypes);
      if (attributeByGuid != null)
      {
        if (attributeByGuid.ValuesCount > 0)
        {
          for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
          {
            string str = Convert.ToString(attributeByGuid.Values[index]);
            if (GuidHelper.IsGuid(str))
            {
              PortalObjectType publishObjectType = this._portalMetadata.GetPublishObjectType(new Guid(str));
              if (publishObjectType != null)
              {
                TreeNode node = new TreeNode(publishObjectType.Name)
                {
                  Tag = (object) publishObjectType
                };
                node.ImageIndex = node.SelectedImageIndex = Statics.IconSrv.IndexOf(SiteClientConsts.CategoryPublishType, publishObjectType.ID);
                this.tvObjects.Nodes[0].Nodes.Add(node);
              }
            }
          }
          this.tvObjects.Sort();
          this.tvObjects.ExpandAll();
        }
      }
    }
    this.btnDelete.Enabled = this.tvObjects.Nodes[0].Nodes.Count > 0;
  }

  private void AddObjectType()
  {
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(PortalConsts.attributePortalObjectTypes);
    IAttributePropertyDescriber describer = this._attrDescriberService.GetDescriber(attributeTypeId);
    if (describer != null && describer.GetPropDescriptorEditor(attributeTypeId) is UITypeEditor descriptorEditor && descriptorEditor.GetEditStyle() == UITypeEditorEditStyle.Modal)
    {
      object obj = descriptorEditor.EditValue((IServiceProvider) null, (object) null);
      if (obj is PublishTypeAttProxy)
        this.AddObjectTypeInTree(((PublishTypeAttProxy) obj).ID);
    }
    this.IsModified = true;
    this.btnDelete.Enabled = this.tvObjects.Nodes[0].Nodes.Count > 0;
  }

  private void DeleteObject()
  {
    if (this.tvObjects.SelectedNode != null)
    {
      this.tvObjects.BeginUpdate();
      this.tvObjects.SelectedNode.Remove();
      this.tvObjects.Sort();
      this.tvObjects.EndUpdate();
    }
    this.IsModified = true;
    this.btnDelete.Enabled = this.tvObjects.Nodes[0].Nodes.Count > 0;
  }

  private void btnAdd_Click(object sender, EventArgs e) => this.AddObjectType();

  private void btnDelete_Click(object sender, EventArgs e) => this.DeleteObject();

  private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.btnDelete.Enabled = e.Node != this.tvObjects.Nodes[0];
  }

  private void buttonEdit1_TextChanged(object sender, EventArgs e) => this.IsModified = true;

  private void btnCancel_Click(object sender, EventArgs e) => this.SelectionLoad(this._selObjectID);

  private void btnApply_Click(object sender, EventArgs e)
  {
    this.SelectionSave();
    this.IsModified = false;
  }

  public void SelectionSave()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBTransactions customService = (IDBTransactions) sessionKeeper.Session.GetCustomService(typeof (IDBTransactions));
      customService.StartTransaction();
      try
      {
        AttributeValues[] valuesList = new AttributeValues[2]
        {
          new AttributeValues(sessionKeeper.Session.GetAttributeType(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AttributeID, (object) this.buttonEdit1.Text),
          new AttributeValues(sessionKeeper.Session.GetAttributeType(new Guid("cad00021-306c-11d8-b4e9-00304f19f545")).AttributeID, (object) this.textBox1.Text)
        };
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._selObjectID, true);
        dbObject.SetAttributesValues(valuesList);
        IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalConsts.attributePortalObjectTypes);
        attributeByGuid.ClearValues();
        if (this.tvObjects.Nodes[0].Nodes.Count > 0)
        {
          ArrayList arrayList = new ArrayList(this.tvObjects.Nodes[0].Nodes.Count);
          foreach (TreeNode node in this.tvObjects.Nodes[0].Nodes)
            arrayList.Add((object) ((PortalObjectType) node.Tag).GUID);
          attributeByGuid.Values = (object[]) arrayList.ToArray(typeof (object));
        }
        customService.Commit();
      }
      catch
      {
        customService.Rollback();
        throw;
      }
    }
    (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", this._selObjectID));
    this.IsModified = false;
  }

  private void textBox1_TextChanged(object sender, EventArgs e) => this.IsModified = true;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    TreeNode treeNode = new TreeNode("Типы объектов");
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PortalSelectionDialog));
    this.panel2 = new Panel();
    this.label4 = new Label();
    this.tvObjects = new TreeView();
    this.btnAdd = new Button();
    this.btnDelete = new Button();
    this.label3 = new Label();
    this.buttonEdit1 = new ButtonEdit();
    this.panel3 = new Panel();
    this.textBox1 = new TextBox();
    this.label8 = new Label();
    this.label5 = new Label();
    this.label6 = new Label();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.panel2.SuspendLayout();
    this.buttonEdit1.Properties.BeginInit();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    this.panel2.BackColor = Color.FromArgb(180, 179, 178);
    this.panel2.Controls.Add((Control) this.label4);
    this.panel2.Dock = DockStyle.Top;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(830, 41);
    this.panel2.TabIndex = 11;
    this.label4.AutoSize = true;
    this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.label4.Location = new Point(13, 9);
    this.label4.Name = "label4";
    this.label4.Size = new Size(0, 13);
    this.label4.TabIndex = 0;
    this.tvObjects.Location = new Point(149, 135);
    this.tvObjects.Name = "tvObjects";
    treeNode.Name = "Node0";
    treeNode.Text = "Типы объектов";
    this.tvObjects.Nodes.AddRange(new TreeNode[1]
    {
      treeNode
    });
    this.tvObjects.Size = new Size(416, 172);
    this.tvObjects.TabIndex = 14;
    this.tvObjects.AfterSelect += new TreeViewEventHandler(this.tvObjects_AfterSelect);
    this.btnAdd.Image = (Image) Intermech.Site.Client.Properties.Resources.ObjectTypes;
    this.btnAdd.Location = new Point(150, 107);
    this.btnAdd.Name = "btnAdd";
    this.btnAdd.Size = new Size(23, 23);
    this.btnAdd.TabIndex = 16 /*0x10*/;
    this.btnAdd.UseVisualStyleBackColor = true;
    this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
    this.btnDelete.Image = (Image) componentResourceManager.GetObject("btnDelete.Image");
    this.btnDelete.Location = new Point(179, 107);
    this.btnDelete.Name = "btnDelete";
    this.btnDelete.Size = new Size(23, 23);
    this.btnDelete.TabIndex = 17;
    this.btnDelete.UseVisualStyleBackColor = true;
    this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
    this.label3.AutoSize = true;
    this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.label3.Location = new Point(13, 68);
    this.label3.Name = "label3";
    this.label3.Size = new Size(99, 13);
    this.label3.TabIndex = 12;
    this.label3.Text = "Наименование:";
    this.buttonEdit1.EditValue = (object) "";
    this.buttonEdit1.Location = new Point(150, 64 /*0x40*/);
    this.buttonEdit1.Name = "buttonEdit1";
    this.buttonEdit1.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton()
    });
    this.buttonEdit1.Size = new Size(416, 20);
    this.buttonEdit1.TabIndex = 15;
    this.buttonEdit1.TextChanged += new EventHandler(this.buttonEdit1_TextChanged);
    this.buttonEdit1.ButtonClick += new ButtonPressedEventHandler(this.buttonEdit1_ButtonClick);
    this.panel3.BackColor = Color.FromArgb(180, 179, 178);
    this.panel3.Controls.Add((Control) this.textBox1);
    this.panel3.Controls.Add((Control) this.label8);
    this.panel3.Controls.Add((Control) this.label5);
    this.panel3.Controls.Add((Control) this.label6);
    this.panel3.Dock = DockStyle.Bottom;
    this.panel3.Location = new Point(0, 322);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(830, 159);
    this.panel3.TabIndex = 18;
    this.textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
    this.textBox1.BackColor = SystemColors.Window;
    this.textBox1.Location = new Point(14, 80 /*0x50*/);
    this.textBox1.Multiline = true;
    this.textBox1.Name = "textBox1";
    this.textBox1.ScrollBars = ScrollBars.Vertical;
    this.textBox1.Size = new Size(804, 61);
    this.textBox1.TabIndex = 15;
    this.textBox1.WordWrap = false;
    this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
    this.label8.AutoSize = true;
    this.label8.Location = new Point(13, 59);
    this.label8.Name = "label8";
    this.label8.Size = new Size(73, 13);
    this.label8.TabIndex = 14;
    this.label8.Text = "Примечание:";
    this.label5.Anchor = AnchorStyles.Left | AnchorStyles.Right;
    this.label5.AutoSize = true;
    this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.label5.Location = new Point(13, 24);
    this.label5.Name = "label5";
    this.label5.Size = new Size(106, 13);
    this.label5.TabIndex = 11;
    this.label5.Text = "Владелец выборки:";
    this.label6.Anchor = AnchorStyles.Left | AnchorStyles.Right;
    this.label6.AutoSize = true;
    this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.label6.Location = new Point(147, 24);
    this.label6.Name = "label6";
    this.label6.Size = new Size(0, 13);
    this.label6.TabIndex = 12;
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.Location = new Point(697, 280);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(121, 27);
    this.btnCancel.TabIndex = 20;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnApply.Location = new Point(571, 280);
    this.btnApply.Name = "btnApply";
    this.btnApply.Size = new Size(121, 27);
    this.btnApply.TabIndex = 19;
    this.btnApply.Text = "Применить";
    this.btnApply.UseVisualStyleBackColor = true;
    this.btnApply.Click += new EventHandler(this.btnApply_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(830, 481);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnApply);
    this.Controls.Add((Control) this.panel3);
    this.Controls.Add((Control) this.tvObjects);
    this.Controls.Add((Control) this.btnAdd);
    this.Controls.Add((Control) this.btnDelete);
    this.Controls.Add((Control) this.label3);
    this.Controls.Add((Control) this.buttonEdit1);
    this.Controls.Add((Control) this.panel2);
    this.MinimumSize = new Size(790, 515);
    this.Name = nameof (PortalSelectionDialog);
    this.Text = nameof (PortalSelectionDialog);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.buttonEdit1.Properties.EndInit();
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
