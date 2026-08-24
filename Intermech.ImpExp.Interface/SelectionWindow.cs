// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SelectionWindow
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class SelectionWindow : Form
{
  /// <summary>Глобальный идентификатор пустого нода</summary>
  public static Guid SelectionWindowEmpty = new Guid("{C3CB5DFD-4E07-4C67-8A38-8CEA87C2CA96}");
  private static Dictionary<int, SelectionWindow> _windows;
  private int _currentCategory;
  private SelectionWindowOptions _options;
  private TreeNode _virtualNode;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Panel panelBottom;
  private Button bOK;
  private Panel panelMain;
  private Button bCancel;
  private TreeView treeView;
  private ImageList imageList;

  public Guid SelectedGuid
  {
    get => this.treeView.SelectedNode == null ? Guid.Empty : (Guid) this.treeView.SelectedNode.Tag;
  }

  public string SelectedText
  {
    get => this.treeView.SelectedNode == null ? string.Empty : this.treeView.SelectedNode.Text;
  }

  public SelectionWindow() => this.InitializeComponent();

  public static SelectionWindow ShowWindow(int category)
  {
    return SelectionWindow.ShowWindow(category, SelectionWindowOptions.None);
  }

  public static SelectionWindow ShowWindow(int category, SelectionWindowOptions options)
  {
    if (SelectionWindow._windows == null)
      SelectionWindow._windows = new Dictionary<int, SelectionWindow>(4);
    SelectionWindow selectionWindow = (SelectionWindow) null;
    if (!SelectionWindow._windows.TryGetValue(category, out selectionWindow))
    {
      selectionWindow = new SelectionWindow(category, options);
      SelectionWindow._windows.Add(category, selectionWindow);
    }
    return selectionWindow;
  }

  public SelectionWindow(int category)
    : this(category, SelectionWindowOptions.EmptyInclude)
  {
  }

  public SelectionWindow(int category, SelectionWindowOptions options)
    : this()
  {
    this._currentCategory = category;
    this._options = options;
    this._virtualNode = new TreeNode();
    this._virtualNode.Tag = (object) Guid.Empty;
    IDataWriter service = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
    switch (category)
    {
      case 3:
        this.InitializeAttribute(service.GetUserSession());
        this.Text = "Выбор атрибута";
        break;
      case 4:
        this.InitializeObjectType(service.GetUserSession());
        this.Text = "Выбор типа объектов";
        break;
      case 6:
        this.InitializeRelationTypes(service.GetUserSession());
        this.Text = "Выбор типа связей";
        break;
      case 16 /*0x10*/:
        this.InitializeLCScheme(service.GetUserSession());
        this.Text = "Выбор схемы ЖЦ";
        break;
    }
  }

  private TreeNode AddNode(string name, Guid tag) => this.AddNode(name, tag, 0, false);

  private TreeNode AddNode(string name, Guid tag, int imageIndex, bool addVirtualNode)
  {
    TreeNode treeNode = new TreeNode(name);
    treeNode.Tag = (object) tag;
    treeNode.ImageIndex = imageIndex;
    treeNode.SelectedImageIndex = imageIndex;
    if (addVirtualNode)
      treeNode.Nodes.Add(this._virtualNode.Clone() as TreeNode);
    return treeNode;
  }

  private void InitializeAttribute(IUserSession session)
  {
    if (ServicesManager.GetService(typeof (IAttributeImageList)) is IAttributeImageList service)
      this.treeView.ImageList = service.ImageList;
    DataTable dataTable = session.GetAttributeTypeCollection(-1).Select("F_NAME");
    if (dataTable.Rows.Count <= 0)
      return;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      this.treeView.Nodes.Add(this.AddNode(Convert.ToString(dataTable.Rows[index]["F_NAME"]), new Guid(Convert.ToString(dataTable.Rows[index]["F_GUID"])), service != null ? service.ImageIndex((FieldTypes) Convert.ToInt32(dataTable.Rows[index]["F_ATTRIBUTE_TYPE"])) : 0, false));
  }

  private void InitializeLCScheme(IUserSession session)
  {
    DataTable dataTable = (session.GetLCSchemaCollection() as IDBCollection).Select(string.Empty);
    if (dataTable.Rows.Count <= 0)
      return;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      this.treeView.Nodes.Add(this.AddNode(Convert.ToString(dataTable.Rows[index]["F_NAME"]), new Guid(Convert.ToString(dataTable.Rows[index]["F_GUID"])), 0, false));
  }

  private void InitializeRelationTypes(IUserSession session)
  {
    DataTable dataTable = session.GetRelationTypeCollection().Select(string.Empty);
    if (dataTable.Rows.Count <= 0)
      return;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      int imageIndex = 0;
      if (this.AddIcon(dataTable.Rows[index]["F_ICON"]))
        imageIndex = this.imageList.Images.Count - 1;
      this.treeView.Nodes.Add(this.AddNode(Convert.ToString(dataTable.Rows[index]["F_DESCRIPTION"]), new Guid(Convert.ToString(dataTable.Rows[index]["F_GUID"])), imageIndex, false));
    }
  }

  private bool AddIcon(object bytes)
  {
    if (bytes == DBNull.Value || bytes == null)
      return false;
    MemoryStream memoryStream = (MemoryStream) null;
    try
    {
      memoryStream = new MemoryStream((byte[]) bytes);
      this.imageList.Images.Add(new Icon((Stream) memoryStream));
    }
    catch
    {
      return false;
    }
    finally
    {
      if (memoryStream != null)
      {
        memoryStream.Flush();
        memoryStream.Close();
      }
    }
    return true;
  }

  private TreeNode[] GetObjectTypesLevel(IUserSession session, Guid parentID)
  {
    int parentTypeID = -1;
    if (parentID != Guid.Empty)
      parentTypeID = session.GetObjectType(parentID).ObjectType;
    DataTable dataTable = session.GetObjectTypeCollection(parentTypeID).Select(string.Empty);
    if (dataTable.Rows.Count <= 0)
      return (TreeNode[]) null;
    List<TreeNode> treeNodeList = new List<TreeNode>(dataTable.Rows.Count);
    if (parentTypeID == -1 && (this._options & SelectionWindowOptions.EmptyInclude) == SelectionWindowOptions.EmptyInclude)
      treeNodeList.Add(this.AddNode("Не назначен", SelectionWindow.SelectionWindowEmpty, -1, false));
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      int imageIndex = 0;
      if (this.AddIcon(dataTable.Rows[index]["F_ICON"]))
        imageIndex = this.imageList.Images.Count - 1;
      treeNodeList.Add(this.AddNode(Convert.ToString(dataTable.Rows[index]["F_OBJ_TYPE_NAME"]), new Guid(Convert.ToString(dataTable.Rows[index]["F_GUID"])), imageIndex, true));
    }
    return treeNodeList.ToArray();
  }

  private void InitializeObjectType(IUserSession session)
  {
    this.treeView.Nodes.AddRange(this.GetObjectTypesLevel(session, Guid.Empty));
  }

  private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
  {
    if (e.Node.Nodes[0] == null || !((Guid) e.Node.Nodes[0].Tag == Guid.Empty))
      return;
    e.Node.Nodes.Clear();
    IUserSession userSession = (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).GetUserSession();
    if (this._currentCategory != 4)
      return;
    TreeNode[] objectTypesLevel = this.GetObjectTypesLevel(userSession, (Guid) e.Node.Tag);
    if (objectTypesLevel == null)
      return;
    e.Node.Nodes.AddRange(objectTypesLevel);
  }

  private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.bOK.Enabled = this.treeView.SelectedNode != null && (Guid) this.treeView.SelectedNode.Tag != Guid.Empty;
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectionWindow));
    this.panelBottom = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panelMain = new Panel();
    this.treeView = new TreeView();
    this.imageList = new ImageList(this.components);
    this.panelBottom.SuspendLayout();
    this.panelMain.SuspendLayout();
    this.SuspendLayout();
    this.panelBottom.Controls.Add((Control) this.bCancel);
    this.panelBottom.Controls.Add((Control) this.bOK);
    this.panelBottom.Dock = DockStyle.Bottom;
    this.panelBottom.Location = new Point(0, 221);
    this.panelBottom.Name = "panelBottom";
    this.panelBottom.Size = new Size(363, 46);
    this.panelBottom.TabIndex = 0;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(275, 13);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(75, 23);
    this.bCancel.TabIndex = 1;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Enabled = false;
    this.bOK.Location = new Point(194, 13);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(75, 23);
    this.bOK.TabIndex = 0;
    this.bOK.Text = "OK";
    this.bOK.UseVisualStyleBackColor = true;
    this.panelMain.Controls.Add((Control) this.treeView);
    this.panelMain.Dock = DockStyle.Fill;
    this.panelMain.Location = new Point(0, 0);
    this.panelMain.Name = "panelMain";
    this.panelMain.Size = new Size(363, 221);
    this.panelMain.TabIndex = 1;
    this.treeView.Dock = DockStyle.Fill;
    this.treeView.ImageIndex = 0;
    this.treeView.ImageList = this.imageList;
    this.treeView.Location = new Point(0, 0);
    this.treeView.Name = "treeView";
    this.treeView.SelectedImageIndex = 0;
    this.treeView.Size = new Size(363, 221);
    this.treeView.TabIndex = 0;
    this.treeView.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_BeforeExpand);
    this.treeView.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
    this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
    this.imageList.TransparentColor = Color.Transparent;
    this.imageList.Images.SetKeyName(0, "OutputView.ico");
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(363, 267);
    this.Controls.Add((Control) this.panelMain);
    this.Controls.Add((Control) this.panelBottom);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(195, 105);
    this.Name = nameof (SelectionWindow);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = nameof (SelectionWindow);
    this.panelBottom.ResumeLayout(false);
    this.panelMain.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
