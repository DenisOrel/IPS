// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.RequestSchemeTreeView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Actions;
using Intermech.Bars;
using Intermech.ExternalSystemIntegration.Client.Settings;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class RequestSchemeTreeView : BaseSchemeTreeView
{
  private IContainer components;
  private ButtonItem btnCreateTestFile;
  private ActionList actList;
  private Intermech.Actions.Action actCreateTestFile;

  public RequestSchemeTreeView() => this.InitializeComponent();

  private void actCreateTestFile_Execute(object sender, EventArgs e)
  {
    int num = (int) MessageBox.Show(this.SchemeData);
  }

  protected override void AddElement(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    RequestSchemeItemFrm AShemeItemFrm1 = new RequestSchemeItemFrm();
    base.AddElement(ARow, (BaseSchemeItemFrm) AShemeItemFrm1);
  }

  protected override void AddAttribute(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    RequestSchemeItemFrm AShemeItemFrm1 = new RequestSchemeItemFrm();
    base.AddAttribute(ARow, (BaseSchemeItemFrm) AShemeItemFrm1);
  }

  protected override void ShowNodeProperties(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    RequestSchemeItemFrm AShemeItemFrm1 = new RequestSchemeItemFrm();
    base.ShowNodeProperties(ARow, (BaseSchemeItemFrm) AShemeItemFrm1);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RequestSchemeTreeView));
    this.btnCreateTestFile = new ButtonItem();
    this.actList = new ActionList(this.components);
    this.actCreateTestFile = new Intermech.Actions.Action(this.components);
    this.treeView.BeginInit();
    this.SuspendLayout();
    this.treeView.BackgroundImageMode = ImageDrawMode.Tile;
    this.treeView.BorderStyle = BorderStyle.Fixed3D;
    this.treeView.SelectionMode = Infralution.Controls.VirtualTree.SelectionMode.FullRow;
    this.toolBar.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.btnCreateTestFile
    });
    this.btnDelete.Enabled = false;
    this.btnProperties.Enabled = false;
    this.actList.SetAction((Component) this.btnCreateTestFile, this.actCreateTestFile);
    this.btnCreateTestFile.BeginGroup = true;
    this.btnCreateTestFile.CommandName = "btnCreateTestFile";
    this.btnCreateTestFile.Icon = (Icon) componentResourceManager.GetObject("btnCreateTestFile.Icon");
    this.btnCreateTestFile.Text = "Создать запрос";
    this.btnCreateTestFile.ToolTipText = "Создать запрос";
    this.actList.Actions.AddRange(new Intermech.Actions.Action[1]
    {
      this.actCreateTestFile
    });
    this.actList.ImageList = (ImageList) null;
    this.actList.ShowTextOnToolBar = false;
    this.actList.Tag = (object) null;
    this.actCreateTestFile.Hint = (string) null;
    this.actCreateTestFile.Text = "Создать запрос";
    this.actCreateTestFile.Execute += new EventHandler(this.actCreateTestFile_Execute);
    this.AttributeImage = (Image) componentResourceManager.GetObject("$this.AttributeImage");
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (RequestSchemeTreeView);
    this.NodeImage = (Image) componentResourceManager.GetObject("$this.NodeImage");
    this.treeView.EndInit();
    this.ResumeLayout(false);
  }
}
