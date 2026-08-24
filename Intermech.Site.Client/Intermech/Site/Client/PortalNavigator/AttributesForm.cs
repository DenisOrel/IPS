// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.AttributesForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Blending;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class AttributesForm : Form
{
  private IContainer components;
  private iGCellStyle iGrid1DefaultCellStyle;
  private iGColHdrStyle iGrid1DefaultColHdrStyle;
  private iGCellStyle iGrid1RowTextColCellStyle;
  private iGCellStyle iGrid1Col0CellStyle;
  private iGColHdrStyle iGrid1Col0ColHdrStyle;
  private iGCellStyle iGrid1Col1CellStyle;
  private iGColHdrStyle iGrid1Col1ColHdrStyle;
  private XtraTreeListBlending xtraTreeListBlending1;
  private TreeList treeList1;
  private TreeListColumn treeListColumn1;
  private TreeListColumn treeListColumn2;
  private Panel panel1;
  private Panel panel2;

  public AttributesForm() => this.InitializeComponent();

  public void LoadData(IUserSession session, PublishAttribute[] attrs)
  {
    if (attrs == null)
      throw new ArgumentNullException(nameof (attrs));
    this.treeList1.Nodes.Clear();
    List<PublishAttributeCategory> attributeCategoryList = new List<PublishAttributeCategory>(Enum.GetValues(typeof (PublishAttributeCategory)).Length);
    for (int index = 0; index < attrs.Length; ++index)
    {
      PublishAttribute attr = attrs[index];
      if (!attributeCategoryList.Contains(attr.Category))
        attributeCategoryList.Add(attr.Category);
    }
    for (int index = 0; index < attributeCategoryList.Count; ++index)
      this.treeList1.AppendNode((object) new object[2]
      {
        (object) EnumDescConverter.GetEnumDescription((Enum) attributeCategoryList[index]),
        null
      }, (TreeListNode) null).Tag = (object) attributeCategoryList[index];
    IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    ServicesManager.GetService(typeof (IAttributePropertyDescriberService));
    if (attrs != null && attrs.Length != 0)
    {
      for (int index1 = 0; index1 < attrs.Length; ++index1)
      {
        PublishAttribute attr = attrs[index1];
        string empty = string.Empty;
        Type type = typeof (string);
        DataErrors errors = DataErrors.None;
        object attributeValue = Helper.GetAttributeValue(session, service, attr, ref empty, ref type, ref errors);
        TreeListNode parentNode1 = (TreeListNode) null;
        foreach (TreeListNode node in this.treeList1.Nodes)
        {
          if (node.Tag != null && node.Tag is PublishAttributeCategory && (PublishAttributeCategory) node.Tag == attr.Category)
          {
            parentNode1 = node;
            break;
          }
        }
        TreeListNode parentNode2;
        if (attributeValue is object[])
        {
          parentNode2 = this.treeList1.AppendNode((object) new object[2]
          {
            (object) empty,
            null
          }, parentNode1);
          object[] objArray = attributeValue as object[];
          for (int index2 = 0; index2 < objArray.Length; ++index2)
            this.treeList1.AppendNode((object) new object[2]
            {
              (object) $"[{index2}]",
              objArray[index2] == null ? (object) string.Empty : objArray[index2]
            }, parentNode2);
        }
        else
          parentNode2 = this.treeList1.AppendNode((object) new object[2]
          {
            (object) empty,
            attributeValue
          }, parentNode1);
        parentNode2.Tag = (object) errors;
        parentNode2.Expanded = true;
      }
    }
    foreach (TreeListNode node in this.treeList1.Nodes)
      node.Expanded = true;
  }

  private Type GetTypeConverter(Type dataType) => TypeDescriptor.GetConverter(dataType).GetType();

  internal void SetParent(Control aParent)
  {
    if (aParent == null)
    {
      this.TopLevel = true;
      this.Dock = DockStyle.None;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.Visible = false;
    }
    else
    {
      this.TopLevel = false;
      this.Dock = DockStyle.Fill;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Visible = true;
    }
    this.Parent = aParent;
  }

  private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
  {
    if (e.Node.Tag == null)
      return;
    if (e.Node.Tag is PublishAttributeCategory)
    {
      e.Style = this.treeList1.Styles["RootRowsStyle"];
    }
    else
    {
      object tag;
      if (!((tag = e.Node.Tag) is DataErrors))
        return;
      DataErrors dataErrors = (DataErrors) tag;
      if ((dataErrors & DataErrors.ErrorAttribute) == DataErrors.ErrorAttribute || (dataErrors & DataErrors.ErrorValue) == DataErrors.ErrorValue)
      {
        e.Style = this.treeList1.Styles["ErrorRow"];
      }
      else
      {
        if ((dataErrors & DataErrors.WarningValue) != DataErrors.WarningValue)
          return;
        e.Style = this.treeList1.Styles["WarningRow"];
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AttributesForm));
    this.iGrid1Col0CellStyle = new iGCellStyle(true);
    this.iGrid1Col0ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1Col1CellStyle = new iGCellStyle(true);
    this.iGrid1Col1ColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1DefaultCellStyle = new iGCellStyle(true);
    this.iGrid1DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1RowTextColCellStyle = new iGCellStyle(true);
    this.xtraTreeListBlending1 = new XtraTreeListBlending();
    this.treeList1 = new TreeList();
    this.treeListColumn1 = new TreeListColumn();
    this.treeListColumn2 = new TreeListColumn();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.treeList1.BeginInit();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.treeList1, "treeList1");
    this.treeList1.Columns.AddRange(new TreeListColumn[2]
    {
      this.treeListColumn1,
      this.treeListColumn2
    });
    this.treeList1.Name = "treeList1";
    this.treeList1.Styles.AddReplace("ErrorRow", (object) new ViewStyle("ErrorRow", "", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawEndEllipsis | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseHorzAlignment | StyleOptions.UseImage | StyleOptions.UseWordWrap | StyleOptions.UseVertAlignment, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, Color.LightPink, SystemColors.WindowText));
    this.treeList1.Styles.AddReplace("RootRowsStyle", (object) new ViewStyle("RootRowsStyle", "", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawEndEllipsis | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseHorzAlignment | StyleOptions.UseImage | StyleOptions.UseWordWrap | StyleOptions.UseVertAlignment, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.ControlDark, SystemColors.WindowText));
    this.treeList1.Styles.AddReplace("HorzLine", (object) new ViewStyle("HorzLine", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.ControlDark, SystemColors.ControlDark));
    this.treeList1.Styles.AddReplace("VertLine", (object) new ViewStyle("VertLine", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Near, VertAlignment.Center, (Image) null, SystemColors.ControlDark, SystemColors.ControlDark));
    this.treeList1.Styles.AddReplace("Warningrow", (object) new ViewStyle("Warningrow", "", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawEndEllipsis | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseHorzAlignment | StyleOptions.UseImage | StyleOptions.UseWordWrap | StyleOptions.UseVertAlignment, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, Color.LightYellow, SystemColors.WindowText));
    this.treeList1.CustomDrawNodeCell += new CustomDrawNodeCellEventHandler(this.treeList1_CustomDrawNodeCell);
    componentResourceManager.ApplyResources((object) this.treeListColumn1, "treeListColumn1");
    this.treeListColumn1.Name = "treeListColumn1";
    componentResourceManager.ApplyResources((object) this.treeListColumn2, "treeListColumn2");
    this.treeListColumn2.Name = "treeListColumn2";
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Controls.Add((Control) this.treeList1);
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (AttributesForm);
    this.treeList1.EndInit();
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
