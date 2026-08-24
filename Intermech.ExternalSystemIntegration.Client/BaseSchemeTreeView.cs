// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.BaseSchemeTreeView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Infralution.Controls.VirtualTree;
using Intermech.Actions;
using Intermech.Bars;
using Intermech.ExternalSystemIntegration.Client.Settings;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class BaseSchemeTreeView : UserControl
{
  protected XmlDocument _SchemeData = new XmlDocument();
  protected bool _ReadOnly;
  private IContainer components;
  private Column colName;
  private Column colValue;
  private ToolStripMenuItem miProperties;
  private Intermech.Actions.Action actAddElement;
  private Intermech.Actions.Action actAddAttribute;
  private Intermech.Actions.Action actDelete;
  private Intermech.Actions.Action actProperties;
  private Intermech.Actions.Action actLoadFromFile;
  private ToolStripMenuItem miAddElement;
  private ToolStripMenuItem miAddAttribute;
  private ToolStripMenuItem miDelete;
  private ToolStripSeparator toolStripSeparator3;
  protected Intermech.VirtualTreeView.VirtualTreeView treeView;
  private Intermech.Actions.Action actExpandAll;
  private Intermech.Actions.Action actCollapseAll;
  private ToolStripSeparator toolStripSeparator5;
  private ToolStripMenuItem miExpandAll;
  private ToolStripMenuItem miCollapseAll;
  protected Intermech.Bars.ToolBar toolBar;
  protected ButtonItem btnAddElement;
  protected ButtonItem btnAddAttribute;
  protected ButtonItem btnDelete;
  protected ButtonItem btnProperties;
  private ButtonItem btnExpandAll;
  private ButtonItem btnCollapseAll;
  private ButtonItem btnLoadFromFile;
  private ContextMenuStrip contMenuStrip;
  private ImageList imageList;
  private ActionList actions;
  private ButtonItem btnSaveToFile;
  private Intermech.Actions.Action actSaveToFile;

  public event EventHandler DataChanged;

  public Image NodeImage { get; set; }

  public Image AttributeImage { get; set; }

  [DefaultValue(false)]
  public bool ReadOnly
  {
    get => this._ReadOnly;
    set => this._ReadOnly = value;
  }

  public BaseSchemeTreeView() => this.InitializeComponent();

  public string SchemeData => this._SchemeData.InnerXml;

  public void Activate(string ADataSource)
  {
    if (ADataSource.Length > 0)
      this._SchemeData.LoadXml(ADataSource);
    this.treeView.DataSource = (object) this._SchemeData;
  }

  protected virtual void AddElement(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    if (ARow != null && AShemeItemFrm != null)
    {
      if (!(ARow.Item is XmlNode))
        return;
      XmlNode xmlNode = ARow.Item as XmlNode;
      if (xmlNode.NodeType != XmlNodeType.Element || AShemeItemFrm.ShowDialog() != DialogResult.OK)
        return;
      XmlNode node1 = this._SchemeData.CreateNode(XmlNodeType.Element, AShemeItemFrm.NodeName, xmlNode.NamespaceURI);
      xmlNode.AppendChild(node1);
      if (AShemeItemFrm.NodeValue.Length != 0)
      {
        XmlNode node2 = this._SchemeData.CreateNode(XmlNodeType.Text, "", node1.NamespaceURI);
        node2.Value = AShemeItemFrm.NodeValue;
        node1.AppendChild(node2);
      }
      this.treeView.UpdateRows(true);
      ARow.Expand();
      this.RaiseDataChangedEvent();
    }
    else
    {
      if (this.treeView.RootRow.ChildItems.Count != 0 || AShemeItemFrm == null || AShemeItemFrm.ShowDialog() != DialogResult.OK)
        return;
      XmlNode node3 = this._SchemeData.CreateNode(XmlNodeType.Element, AShemeItemFrm.NodeName, this._SchemeData.NamespaceURI);
      this._SchemeData.AppendChild(node3);
      if (AShemeItemFrm.NodeValue.Length != 0)
      {
        XmlNode node4 = this._SchemeData.CreateNode(XmlNodeType.Text, "", node3.NamespaceURI);
        node4.Value = AShemeItemFrm.NodeValue;
        node3.AppendChild(node4);
      }
      this.treeView.UpdateRows(true);
      Row row = this.treeView.FindRow((object) node3);
      if (row != null)
        row.Selected = true;
      this.RaiseDataChangedEvent();
    }
  }

  protected virtual void AddAttribute(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    if (ARow == null || AShemeItemFrm == null || !(ARow.Item is XmlNode))
      return;
    XmlNode xmlNode = ARow.Item as XmlNode;
    if (xmlNode.NodeType != XmlNodeType.Element || AShemeItemFrm.ShowDialog() != DialogResult.OK)
      return;
    XmlAttribute attribute = this._SchemeData.CreateAttribute(AShemeItemFrm.NodeName);
    if (AShemeItemFrm.NodeValue.Length != 0)
      attribute.Value = AShemeItemFrm.NodeValue;
    xmlNode.Attributes.Append(attribute);
    this.treeView.UpdateRows(true);
    if (this.treeView.SelectedRow != null)
      this.treeView.SelectedRow.Selected = false;
    ARow.Expand();
    Row row = this.treeView.FindRow((object) attribute);
    if (row != null)
      row.Selected = true;
    this.RaiseDataChangedEvent();
  }

  private void RaiseDataChangedEvent()
  {
    EventHandler dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged((object) this, new EventArgs());
  }

  protected virtual void ShowNodeProperties(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    if (!(ARow.Item is XmlNode))
      return;
    XmlNode node = ARow.Item as XmlNode;
    if (node.NodeType == XmlNodeType.Element)
    {
      string str = "";
      if (node.ChildNodes.OfType<XmlCharacterData>().Any<XmlCharacterData>())
      {
        if (node.ChildNodes.OfType<XmlCharacterData>().First<XmlCharacterData>() is XmlCDataSection)
          AShemeItemFrm.CDATA = true;
        else if (node.ChildNodes.OfType<XmlCharacterData>().First<XmlCharacterData>() is XmlText)
          AShemeItemFrm.CDATA = false;
        str = node.ChildNodes.OfType<XmlCharacterData>().First<XmlCharacterData>().Value;
      }
      AShemeItemFrm.NodeName = node.Name;
      AShemeItemFrm.NodeValue = str;
      if (AShemeItemFrm.ShowDialog() != DialogResult.OK)
        return;
      if (node.HasChildNodes && node.ChildNodes.OfType<XmlCharacterData>().Any<XmlCharacterData>())
        node.ChildNodes.OfType<XmlCharacterData>().ToList<XmlCharacterData>().ForEach((Action<XmlCharacterData>) (x => node.RemoveChild((XmlNode) x)));
      if (AShemeItemFrm.NodeValue.Length != 0)
      {
        if (AShemeItemFrm.CDATA)
        {
          XmlNode node1 = this._SchemeData.CreateNode(XmlNodeType.CDATA, "", node.NamespaceURI);
          node1.Value = AShemeItemFrm.NodeValue;
          node.AppendChild(node1);
        }
        else
        {
          XmlNode node2 = this._SchemeData.CreateNode(XmlNodeType.Text, "", node.NamespaceURI);
          node2.Value = AShemeItemFrm.NodeValue;
          node.AppendChild(node2);
        }
      }
      if (node.Name != AShemeItemFrm.NodeName)
        node = (XmlNode) this.RenameElement(node as XmlElement, AShemeItemFrm.NodeName);
      this.treeView.UpdateRows(true);
      Row row = this.treeView.FindRow((object) node);
      if (row != null)
      {
        row.Selected = true;
        row.Expand();
      }
      this.RaiseDataChangedEvent();
    }
    else
    {
      if (node.NodeType != XmlNodeType.Attribute)
        return;
      XmlAttribute e = node as XmlAttribute;
      AShemeItemFrm.NodeName = e.Name;
      AShemeItemFrm.NodeValue = e.Value;
      if (AShemeItemFrm.ShowDialog() != DialogResult.OK)
        return;
      if (e.Value != AShemeItemFrm.NodeValue)
        e.Value = AShemeItemFrm.NodeValue;
      if (e.Name != AShemeItemFrm.NodeName)
        e = this.RenameAttribute(e, AShemeItemFrm.NodeName);
      this.treeView.UpdateRows(true);
      Row row = this.treeView.FindRow((object) e);
      if (row != null)
      {
        row.Selected = true;
        row.Expand();
      }
      this.RaiseDataChangedEvent();
    }
  }

  protected virtual void DeleteNode(Row ARow)
  {
    if (!(ARow.Item is XmlNode))
      return;
    XmlNode oldChild = ARow.Item as XmlNode;
    if (oldChild.NodeType == XmlNodeType.Element)
    {
      oldChild.ParentNode.RemoveChild(oldChild);
      this.RaiseDataChangedEvent();
    }
    else if (oldChild.NodeType == XmlNodeType.Attribute)
    {
      XmlAttribute node = oldChild as XmlAttribute;
      node.OwnerElement.Attributes.Remove(node);
      this.RaiseDataChangedEvent();
    }
    this.treeView.UpdateRows(true);
  }

  protected virtual void LoadFromFile()
  {
    if (MessageBox.Show("Загрузить схему трансформации из файла? Текущая схема будет удалена", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
      return;
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.RestoreDirectory = true;
    openFileDialog.Title = "Укажите xml файл";
    openFileDialog.DefaultExt = "xml";
    openFileDialog.Filter = "XML files (*.xml)|*.xml";
    if (openFileDialog.ShowDialog() != DialogResult.OK || !(openFileDialog.FileName != ""))
      return;
    this._SchemeData.Load(openFileDialog.FileName);
    this.treeView.UpdateRows(true);
    this.RaiseDataChangedEvent();
  }

  protected virtual void SaveToFile()
  {
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.Title = "Укажите имя файла";
    saveFileDialog.DefaultExt = "xml";
    saveFileDialog.Filter = "XML files (*.xml)|*.xml";
    saveFileDialog.RestoreDirectory = true;
    if (saveFileDialog.ShowDialog() != DialogResult.OK || saveFileDialog.FileName.Equals(string.Empty))
      return;
    this._SchemeData.Save(saveFileDialog.FileName);
  }

  private void treeView_GetCellData(object sender, GetCellDataEventArgs e)
  {
    XmlNode xmlNode = e.Row.Item as XmlNode;
    if (xmlNode.NodeType == XmlNodeType.Element)
    {
      if (e.Column == this.colName)
      {
        e.CellData.Value = (object) xmlNode.Name;
      }
      else
      {
        if (e.Column != this.colValue)
          return;
        if (xmlNode.ChildNodes.OfType<XmlText>().Any<XmlText>())
        {
          e.CellData.Value = (object) xmlNode.ChildNodes.OfType<XmlText>().First<XmlText>().Value;
        }
        else
        {
          if (!xmlNode.ChildNodes.OfType<XmlCDataSection>().Any<XmlCDataSection>())
            return;
          e.CellData.Value = (object) xmlNode.ChildNodes.OfType<XmlCDataSection>().First<XmlCDataSection>().Value;
        }
      }
    }
    else
    {
      if (xmlNode.NodeType != XmlNodeType.Attribute)
        return;
      if (e.Column == this.colName)
      {
        e.CellData.Value = (object) xmlNode.Name;
      }
      else
      {
        if (e.Column != this.colValue)
          return;
        e.CellData.Value = (object) xmlNode.Value;
      }
    }
  }

  private void treeView_GetRowData(object sender, GetRowDataEventArgs e)
  {
    XmlNode xmlNode = e.Row.Item as XmlNode;
    if (xmlNode.NodeType == XmlNodeType.Element)
    {
      if (this.NodeImage == null)
        return;
      e.RowData.Image = this.NodeImage;
    }
    else
    {
      if (xmlNode.NodeType != XmlNodeType.Attribute || this.AttributeImage == null)
        return;
      e.RowData.Image = this.AttributeImage;
    }
  }

  private void treeView_GetChildren(object sender, GetChildrenEventArgs e)
  {
    XmlNode xmlNode = e.Row.Item as XmlNode;
    ArrayList arrayList = new ArrayList();
    if (xmlNode.NodeType == XmlNodeType.Element || xmlNode.NodeType == XmlNodeType.Document)
    {
      if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
      {
        foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode.Attributes)
          arrayList.Add((object) attribute);
      }
      foreach (XmlNode childNode in xmlNode.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element || childNode.NodeType == XmlNodeType.Attribute)
          arrayList.Add((object) childNode);
      }
    }
    e.Children = (IList) arrayList;
  }

  private void treeView_GetParent(object sender, GetParentEventArgs e)
  {
    if (e.Item is XmlElement)
    {
      XmlElement xmlElement = (XmlElement) e.Item;
      e.Parent = (object) xmlElement.ParentNode;
    }
    else
    {
      if (!(e.Item is XmlAttribute))
        return;
      XmlAttribute xmlAttribute = (XmlAttribute) e.Item;
      e.Parent = (object) xmlAttribute.OwnerElement;
    }
  }

  private XmlElement RenameElement(XmlElement e, string newName)
  {
    XmlElement element = e.OwnerDocument.CreateElement(newName);
    while (e.HasChildNodes)
      element.AppendChild(e.FirstChild);
    XmlAttributeCollection attributes = e.Attributes;
    while (attributes.Count > 0)
      element.Attributes.Append(attributes[0]);
    e.ParentNode.ReplaceChild((XmlNode) element, (XmlNode) e);
    return element;
  }

  private XmlAttribute RenameAttribute(XmlAttribute e, string newName)
  {
    XmlAttribute attribute = e.OwnerDocument.CreateAttribute(newName);
    attribute.Value = e.Value;
    XmlElement ownerElement = e.OwnerElement;
    ownerElement.Attributes.InsertBefore(attribute, e);
    ownerElement.Attributes.Remove(e);
    return attribute;
  }

  private void actAddElement_Execute(object sender, EventArgs e)
  {
    this.AddElement(this.treeView.SelectedRow, (BaseSchemeItemFrm) null);
  }

  private void actAddAttribute_Execute(object sender, EventArgs e)
  {
    if (this.treeView.SelectedRow == null)
      return;
    this.AddAttribute(this.treeView.SelectedRow, (BaseSchemeItemFrm) null);
  }

  private void actDelete_Execute(object sender, EventArgs e)
  {
    if (this.treeView.SelectedRow == null)
      return;
    this.DeleteNode(this.treeView.SelectedRow);
  }

  private void actProperties_Execute(object sender, EventArgs e)
  {
    if (this.treeView.SelectedRow == null)
      return;
    this.ShowNodeProperties(this.treeView.SelectedRow, (BaseSchemeItemFrm) null);
  }

  private void actLoadFromFile_Execute(object sender, EventArgs e) => this.LoadFromFile();

  private void actSaveToFile_Execute(object sender, EventArgs e) => this.SaveToFile();

  private void actCollapseAll_Execute(object sender, EventArgs e)
  {
    this.treeView.RootRow.CollapseChildren(true);
  }

  private void actExpandAll_Execute(object sender, EventArgs e)
  {
    this.treeView.RootRow.ExpandChildren(true);
  }

  private void actAddAttribute_Update(object sender, EventArgs e)
  {
    if (this.treeView == null || this.treeView.SelectedItem == null)
      return;
    this.actAddAttribute.Enabled = this.treeView.SelectedRow != null && this.treeView.SelectedItem is XmlElement;
  }

  private void actDelete_Update(object sender, EventArgs e)
  {
    if (this.treeView == null)
      return;
    this.actDelete.Enabled = this.treeView.SelectedRow != null;
  }

  private void actProperties_Update(object sender, EventArgs e)
  {
    if (this.treeView == null)
      return;
    this.actProperties.Enabled = this.treeView.SelectedRow != null;
  }

  private void actAddElement_Update(object sender, EventArgs e)
  {
    if (this.treeView == null || this.treeView.RootRow == null || this.treeView.RootRow.ChildItems == null)
      return;
    this.actAddElement.Enabled = this.treeView.RootRow.ChildItems.Count <= 0 || this.treeView.SelectedItem is XmlElement;
  }

  private void actExpandAll_Update(object sender, EventArgs e)
  {
    if (this.treeView == null || this.treeView.RootRow == null || this.treeView.RootRow.ChildItems == null)
      return;
    this.actExpandAll.Enabled = this.treeView.RootRow.ChildItems.Count > 0;
  }

  private void actCollapseAll_Update(object sender, EventArgs e)
  {
    if (this.treeView == null || this.treeView.RootRow == null || this.treeView.RootRow.ChildItems == null)
      return;
    this.actCollapseAll.Enabled = this.treeView.RootRow.ChildItems.Count > 0;
  }

  private void treeView_GetAllowRowDrag(object sender, GetAllowRowDragEventArgs e)
  {
    if (!(e.Row.Item is XmlElement) && !(e.Row.Item is XmlAttribute))
      return;
    e.AllowDrag = true;
  }

  private void treeView_DragEnter(object sender, DragEventArgs e) => e.Effect = e.AllowedEffect;

  private void treeView_GetAllowedRowDropLocations(
    object sender,
    GetAllowedRowDropLocationsEventArgs e)
  {
    e.AllowedDropLocations = RowDropLocation.OnRow;
  }

  private void treeView_GetRowDropEffect(object sender, GetRowDropEffectEventArgs e)
  {
    if (!(e.Row.Item is XmlElement))
      return;
    if (((Infralution.Controls.VirtualTree.VirtualTree) sender).SelectedItem != e.Row.Item)
    {
      if (((Infralution.Controls.VirtualTree.VirtualTree) sender).SelectedItem is XmlElement)
      {
        if (((XmlNode) ((Infralution.Controls.VirtualTree.VirtualTree) sender).SelectedItem).SelectNodes("descendant::node()").OfType<XmlElement>().Any<XmlElement>((Func<XmlElement, bool>) (x => x == e.Row.Item)))
          e.DropEffect = DragDropEffects.None;
        else if (Control.ModifierKeys == Keys.Control)
          e.DropEffect = DragDropEffects.Copy;
        else
          e.DropEffect = DragDropEffects.Move;
      }
      else
      {
        if (!(((Infralution.Controls.VirtualTree.VirtualTree) sender).SelectedItem is XmlAttribute))
          return;
        if ((e.Row.Item as XmlElement).Attributes.OfType<XmlAttribute>().Any<XmlAttribute>((Func<XmlAttribute, bool>) (x => x.Name == ((XmlNode) ((Infralution.Controls.VirtualTree.VirtualTree) sender).SelectedItem).Name)))
          e.DropEffect = DragDropEffects.None;
        else if (Control.ModifierKeys == Keys.Control)
          e.DropEffect = DragDropEffects.Copy;
        else
          e.DropEffect = DragDropEffects.Move;
      }
    }
    else
      e.DropEffect = DragDropEffects.None;
  }

  private void treeView_RowDrop(object sender, RowDropEventArgs e)
  {
    if (e.DropLocation != RowDropLocation.OnRow || !(e.Row.Item is XmlElement))
      return;
    XmlElement target = (XmlElement) e.Row.Item;
    XmlNode xmlNode = (XmlNode) null;
    if ((e.DropEffect & DragDropEffects.Move) == DragDropEffects.Move)
      xmlNode = this.MoveElements(target, (XmlNode) ((Infralution.Controls.VirtualTree.VirtualTree) sender).SelectedItem);
    else if ((e.DropEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
      xmlNode = this.CopyElements(target, (XmlNode) ((Infralution.Controls.VirtualTree.VirtualTree) sender).SelectedItem);
    this.treeView.UpdateRows(true);
    if (this.treeView.SelectedRow != null)
      this.treeView.SelectedRow.Selected = false;
    if (xmlNode == null)
      return;
    Row row = this.treeView.FindRow((object) xmlNode);
    if (row == null)
      return;
    row.Selected = true;
    row.ParentRow?.Expand();
  }

  private XmlNode MoveElements(XmlElement target, XmlNode dragged)
  {
    XmlNode xmlNode = (XmlNode) null;
    if (target != null)
    {
      switch (dragged)
      {
        case XmlElement _:
          dragged.ParentNode.RemoveChild(dragged);
          xmlNode = target.AppendChild(dragged);
          break;
        case XmlAttribute _:
          XmlAttribute node = (XmlAttribute) dragged;
          node.OwnerElement.Attributes.Remove(node);
          xmlNode = (XmlNode) target.Attributes.Append(node);
          break;
      }
    }
    return xmlNode;
  }

  private XmlNode CopyElements(XmlElement target, XmlNode dragged)
  {
    XmlNode xmlNode = (XmlNode) null;
    if (target != null)
    {
      switch (dragged)
      {
        case XmlElement _:
          XmlElement newChild = (XmlElement) dragged.Clone();
          xmlNode = target.AppendChild((XmlNode) newChild);
          break;
        case XmlAttribute _:
          XmlAttribute node = (XmlAttribute) dragged.Clone();
          xmlNode = (XmlNode) target.Attributes.Append(node);
          break;
      }
    }
    return xmlNode;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BaseSchemeTreeView));
    this.treeView = new Intermech.VirtualTreeView.VirtualTreeView();
    this.colName = new Column();
    this.colValue = new Column();
    this.contMenuStrip = new ContextMenuStrip(this.components);
    this.miAddElement = new ToolStripMenuItem();
    this.miAddAttribute = new ToolStripMenuItem();
    this.miDelete = new ToolStripMenuItem();
    this.toolStripSeparator3 = new ToolStripSeparator();
    this.miProperties = new ToolStripMenuItem();
    this.toolStripSeparator5 = new ToolStripSeparator();
    this.miExpandAll = new ToolStripMenuItem();
    this.miCollapseAll = new ToolStripMenuItem();
    this.imageList = new ImageList(this.components);
    this.actions = new ActionList(this.components);
    this.actAddElement = new Intermech.Actions.Action(this.components);
    this.actAddAttribute = new Intermech.Actions.Action(this.components);
    this.actDelete = new Intermech.Actions.Action(this.components);
    this.actProperties = new Intermech.Actions.Action(this.components);
    this.actExpandAll = new Intermech.Actions.Action(this.components);
    this.actCollapseAll = new Intermech.Actions.Action(this.components);
    this.actLoadFromFile = new Intermech.Actions.Action(this.components);
    this.actSaveToFile = new Intermech.Actions.Action(this.components);
    this.btnAddElement = new ButtonItem();
    this.btnAddAttribute = new ButtonItem();
    this.btnDelete = new ButtonItem();
    this.btnProperties = new ButtonItem();
    this.btnExpandAll = new ButtonItem();
    this.btnCollapseAll = new ButtonItem();
    this.btnLoadFromFile = new ButtonItem();
    this.toolBar = new Intermech.Bars.ToolBar();
    this.btnSaveToFile = new ButtonItem();
    this.treeView.BeginInit();
    this.contMenuStrip.SuspendLayout();
    this.SuspendLayout();
    this.treeView.AllowDrop = true;
    this.treeView.Columns.Add(this.colName);
    this.treeView.Columns.Add(this.colValue);
    this.treeView.ContextMenuStrip = this.contMenuStrip;
    this.treeView.DisableHeaderContextMenu = false;
    this.treeView.Dock = DockStyle.Fill;
    this.treeView.ImageList = (ImageList) null;
    this.treeView.LineStyle = LineStyle.Dot;
    this.treeView.Location = new Point(0, 24);
    this.treeView.Name = "treeView";
    this.treeView.ShowRootRow = false;
    this.treeView.Size = new Size(542, 273);
    this.treeView.TabIndex = 1;
    this.treeView.GetAllowedRowDropLocations += new GetAllowedRowDropLocationsHandler(this.treeView_GetAllowedRowDropLocations);
    this.treeView.GetAllowRowDrag += new GetAllowRowDragHandler(this.treeView_GetAllowRowDrag);
    this.treeView.GetCellData += new GetCellDataHandler(this.treeView_GetCellData);
    this.treeView.GetChildren += new GetChildrenHandler(this.treeView_GetChildren);
    this.treeView.GetParent += new GetParentHandler(this.treeView_GetParent);
    this.treeView.GetRowData += new GetRowDataHandler(this.treeView_GetRowData);
    this.treeView.GetRowDropEffect += new GetRowDropEffectHandler(this.treeView_GetRowDropEffect);
    this.treeView.RowDrop += new RowDropHandler(this.treeView_RowDrop);
    this.treeView.DragEnter += new DragEventHandler(this.treeView_DragEnter);
    this.colName.Caption = "Наименование";
    this.colName.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this.colName.Name = "colName";
    this.colName.Width = 250;
    this.colValue.Caption = "Значение";
    this.colValue.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this.colValue.Name = "colValue";
    this.colValue.Width = 250;
    this.contMenuStrip.Items.AddRange(new ToolStripItem[8]
    {
      (ToolStripItem) this.miAddElement,
      (ToolStripItem) this.miAddAttribute,
      (ToolStripItem) this.miDelete,
      (ToolStripItem) this.toolStripSeparator3,
      (ToolStripItem) this.miProperties,
      (ToolStripItem) this.toolStripSeparator5,
      (ToolStripItem) this.miExpandAll,
      (ToolStripItem) this.miCollapseAll
    });
    this.contMenuStrip.Name = "contMenuStrip";
    this.contMenuStrip.Size = new Size(176 /*0xB0*/, 148);
    this.contMenuStrip.Text = "Свойства";
    this.actions.SetAction((Component) this.miAddElement, this.actAddElement);
    this.miAddElement.Enabled = false;
    this.miAddElement.Image = (Image) componentResourceManager.GetObject("miAddElement.Image");
    this.miAddElement.Name = "miAddElement";
    this.miAddElement.Size = new Size(175, 22);
    this.miAddElement.Text = "Добавить элемент";
    this.actions.SetAction((Component) this.miAddAttribute, this.actAddAttribute);
    this.miAddAttribute.Enabled = false;
    this.miAddAttribute.Image = (Image) componentResourceManager.GetObject("miAddAttribute.Image");
    this.miAddAttribute.Name = "miAddAttribute";
    this.miAddAttribute.Size = new Size(175, 22);
    this.miAddAttribute.Text = "Добавить атрибут";
    this.actions.SetAction((Component) this.miDelete, this.actDelete);
    this.miDelete.Image = (Image) componentResourceManager.GetObject("miDelete.Image");
    this.miDelete.Name = "miDelete";
    this.miDelete.Size = new Size(175, 22);
    this.miDelete.Text = "Удалить";
    this.toolStripSeparator3.Name = "toolStripSeparator3";
    this.toolStripSeparator3.Size = new Size(172, 6);
    this.actions.SetAction((Component) this.miProperties, this.actProperties);
    this.miProperties.Image = (Image) componentResourceManager.GetObject("miProperties.Image");
    this.miProperties.Name = "miProperties";
    this.miProperties.Size = new Size(175, 22);
    this.miProperties.Text = "Свойства";
    this.toolStripSeparator5.Name = "toolStripSeparator5";
    this.toolStripSeparator5.Size = new Size(172, 6);
    this.actions.SetAction((Component) this.miExpandAll, this.actExpandAll);
    this.miExpandAll.Image = (Image) componentResourceManager.GetObject("miExpandAll.Image");
    this.miExpandAll.Name = "miExpandAll";
    this.miExpandAll.Size = new Size(175, 22);
    this.miExpandAll.Text = "Развернуть все";
    this.actions.SetAction((Component) this.miCollapseAll, this.actCollapseAll);
    this.miCollapseAll.Image = (Image) componentResourceManager.GetObject("miCollapseAll.Image");
    this.miCollapseAll.Name = "miCollapseAll";
    this.miCollapseAll.Size = new Size(175, 22);
    this.miCollapseAll.Text = "Свернуть все";
    this.imageList.ColorDepth = ColorDepth.Depth8Bit;
    this.imageList.ImageSize = new Size(16 /*0x10*/, 16 /*0x10*/);
    this.imageList.TransparentColor = Color.Transparent;
    this.actions.Actions.AddRange(new Intermech.Actions.Action[8]
    {
      this.actAddElement,
      this.actAddAttribute,
      this.actDelete,
      this.actProperties,
      this.actLoadFromFile,
      this.actExpandAll,
      this.actCollapseAll,
      this.actSaveToFile
    });
    this.actions.ImageList = (ImageList) null;
    this.actions.ShowTextOnToolBar = false;
    this.actions.Tag = (object) null;
    this.actAddElement.Enabled = false;
    this.actAddElement.Hint = (string) null;
    this.actAddElement.Text = "Добавить элемент";
    this.actAddElement.Execute += new EventHandler(this.actAddElement_Execute);
    this.actAddElement.Update += new EventHandler(this.actAddElement_Update);
    this.actAddAttribute.Enabled = false;
    this.actAddAttribute.Hint = (string) null;
    this.actAddAttribute.Text = "Добавить атрибут";
    this.actAddAttribute.Execute += new EventHandler(this.actAddAttribute_Execute);
    this.actAddAttribute.Update += new EventHandler(this.actAddAttribute_Update);
    this.actDelete.Hint = (string) null;
    this.actDelete.Text = "Удалить";
    this.actDelete.Execute += new EventHandler(this.actDelete_Execute);
    this.actDelete.Update += new EventHandler(this.actDelete_Update);
    this.actProperties.Hint = (string) null;
    this.actProperties.Text = "Свойства";
    this.actProperties.Execute += new EventHandler(this.actProperties_Execute);
    this.actProperties.Update += new EventHandler(this.actProperties_Update);
    this.actExpandAll.Hint = (string) null;
    this.actExpandAll.Text = "Развернуть все";
    this.actExpandAll.Execute += new EventHandler(this.actExpandAll_Execute);
    this.actExpandAll.Update += new EventHandler(this.actExpandAll_Update);
    this.actCollapseAll.Hint = (string) null;
    this.actCollapseAll.Text = "Свернуть все";
    this.actCollapseAll.Execute += new EventHandler(this.actCollapseAll_Execute);
    this.actCollapseAll.Update += new EventHandler(this.actCollapseAll_Update);
    this.actLoadFromFile.Hint = (string) null;
    this.actLoadFromFile.Text = "Загрузить из файла";
    this.actLoadFromFile.Execute += new EventHandler(this.actLoadFromFile_Execute);
    this.actSaveToFile.Hint = (string) null;
    this.actSaveToFile.Text = "Сохранить в файл";
    this.actSaveToFile.Execute += new EventHandler(this.actSaveToFile_Execute);
    this.actions.SetAction((Component) this.btnAddElement, this.actAddElement);
    this.btnAddElement.CommandName = "btnAddElement";
    this.btnAddElement.Enabled = false;
    this.btnAddElement.Icon = (Icon) componentResourceManager.GetObject("btnAddElement.Icon");
    this.btnAddElement.Text = "Добавить элемент";
    this.btnAddElement.ToolTipText = "Добавить элемент";
    this.actions.SetAction((Component) this.btnAddAttribute, this.actAddAttribute);
    this.btnAddAttribute.CommandName = "btnAddAttribute";
    this.btnAddAttribute.Enabled = false;
    this.btnAddAttribute.Icon = (Icon) componentResourceManager.GetObject("btnAddAttribute.Icon");
    this.btnAddAttribute.Text = "Добавить атрибут";
    this.btnAddAttribute.ToolTipText = "Добавить атрибут";
    this.actions.SetAction((Component) this.btnDelete, this.actDelete);
    this.btnDelete.CommandName = "btnDelete";
    this.btnDelete.Icon = (Icon) componentResourceManager.GetObject("btnDelete.Icon");
    this.btnDelete.Text = "Удалить";
    this.btnDelete.ToolTipText = "Удалить";
    this.actions.SetAction((Component) this.btnProperties, this.actProperties);
    this.btnProperties.BeginGroup = true;
    this.btnProperties.CommandName = "btnProperties";
    this.btnProperties.Icon = (Icon) componentResourceManager.GetObject("btnProperties.Icon");
    this.btnProperties.Text = "Свойства";
    this.btnProperties.ToolTipText = "Свойства";
    this.actions.SetAction((Component) this.btnExpandAll, this.actExpandAll);
    this.btnExpandAll.BeginGroup = true;
    this.btnExpandAll.CommandName = "btnExpandAll";
    this.btnExpandAll.Icon = (Icon) componentResourceManager.GetObject("btnExpandAll.Icon");
    this.btnExpandAll.Text = "Развернуть все";
    this.btnExpandAll.ToolTipText = "Развернуть все";
    this.actions.SetAction((Component) this.btnCollapseAll, this.actCollapseAll);
    this.btnCollapseAll.CommandName = "btnCollapseAll";
    this.btnCollapseAll.Icon = (Icon) componentResourceManager.GetObject("btnCollapseAll.Icon");
    this.btnCollapseAll.Text = "Свернуть все";
    this.btnCollapseAll.ToolTipText = "Свернуть все";
    this.actions.SetAction((Component) this.btnLoadFromFile, this.actLoadFromFile);
    this.btnLoadFromFile.BeginGroup = true;
    this.btnLoadFromFile.CommandName = "btnLoadFromFile";
    this.btnLoadFromFile.Icon = (Icon) componentResourceManager.GetObject("btnLoadFromFile.Icon");
    this.btnLoadFromFile.Text = "Загрузить из файла";
    this.btnLoadFromFile.ToolTipText = "Загрузить из файла";
    this.toolBar.FullMenus = true;
    this.toolBar.Guid = new Guid("80c68f22-19d6-4ed5-a0c5-1f1186321a6c");
    this.toolBar.Hidden = false;
    this.toolBar.Items.AddRange(new ToolbarItemBase[8]
    {
      (ToolbarItemBase) this.btnAddElement,
      (ToolbarItemBase) this.btnAddAttribute,
      (ToolbarItemBase) this.btnDelete,
      (ToolbarItemBase) this.btnProperties,
      (ToolbarItemBase) this.btnExpandAll,
      (ToolbarItemBase) this.btnCollapseAll,
      (ToolbarItemBase) this.btnLoadFromFile,
      (ToolbarItemBase) this.btnSaveToFile
    });
    this.toolBar.Location = new Point(0, 0);
    this.toolBar.Name = "toolBar";
    this.toolBar.Size = new Size(542, 24);
    this.toolBar.TabIndex = 2;
    this.toolBar.Text = "toolBar1";
    this.actions.SetAction((Component) this.btnSaveToFile, this.actSaveToFile);
    this.btnSaveToFile.CommandName = "btnSaveToFile";
    this.btnSaveToFile.Icon = (Icon) componentResourceManager.GetObject("btnSaveToFile.Icon");
    this.btnSaveToFile.Text = "Сохранить в файл";
    this.btnSaveToFile.ToolTipText = "Сохранить в файл";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.treeView);
    this.Controls.Add((Control) this.toolBar);
    this.Name = nameof (BaseSchemeTreeView);
    this.Size = new Size(542, 297);
    this.treeView.EndInit();
    this.contMenuStrip.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public delegate void TransfSchemeTreeViewEventHandler(object sender, NodeEventArgs e);
}
