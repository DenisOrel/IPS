// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ResponceSchemeTreeView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Actions;
using Intermech.Bars;
using Intermech.ExternalSystemIntegration.Client.Settings;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class ResponceSchemeTreeView : BaseSchemeTreeView
{
  private IContainer components;
  private ButtonItem btnTestScheme;
  private ActionList actList;
  private Intermech.Actions.Action actTestScheme;

  public ResponceSchemeTreeView() => this.InitializeComponent();

  protected override void AddElement(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    ResponceSchemeItemFrm AShemeItemFrm1 = new ResponceSchemeItemFrm();
    base.AddElement(ARow, (BaseSchemeItemFrm) AShemeItemFrm1);
  }

  protected override void AddAttribute(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    ResponceSchemeItemFrm AShemeItemFrm1 = new ResponceSchemeItemFrm();
    base.AddAttribute(ARow, (BaseSchemeItemFrm) AShemeItemFrm1);
  }

  protected override void ShowNodeProperties(Row ARow, BaseSchemeItemFrm AShemeItemFrm)
  {
    ResponceSchemeItemFrm AShemeItemFrm1 = new ResponceSchemeItemFrm();
    base.ShowNodeProperties(ARow, (BaseSchemeItemFrm) AShemeItemFrm1);
  }

  private void actTestScheme_Execute(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IXMLParser service = ServiceUtils.GetService<IXMLParser>((object) sessionKeeper.Session, true);
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.RestoreDirectory = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (openFileDialog2.ShowDialog() != DialogResult.OK)
        return;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(openFileDialog2.FileName);
      if (!service.CompareNodes(this._SchemeData.OuterXml, xmlDocument.OuterXml))
      {
        int num1 = (int) MessageBox.Show(string.Format(ServiceHolder.rm.GetString("ExtInt_18"), (object) openFileDialog2.FileName, (object) service.CompareErrorMessage), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Dictionary<int, string> attributeFromNodes = service.ExtractAttributeFromNodes(sessionKeeper.Session.SessionGUID, this._SchemeData.OuterXml, xmlDocument.OuterXml);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<int, string> keyValuePair in attributeFromNodes)
        {
          IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(keyValuePair.Key, false);
          if (attributeType != null)
            stringBuilder.AppendLine($"{attributeType.Name} = \"{keyValuePair.Value}\"");
        }
        if (stringBuilder.Length > 0)
        {
          int num2 = (int) MessageBox.Show(string.Format(ServiceHolder.rm.GetString("ExtInt_19"), (object) openFileDialog2.FileName, (object) stringBuilder), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num3 = (int) MessageBox.Show(string.Format(ServiceHolder.rm.GetString("ExtInt_20"), (object) openFileDialog2.FileName), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
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
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ResponceSchemeTreeView));
    this.btnTestScheme = new ButtonItem();
    this.actList = new ActionList(this.components);
    this.actTestScheme = new Intermech.Actions.Action(this.components);
    this.treeView.BeginInit();
    this.SuspendLayout();
    this.treeView.BackgroundImageMode = ImageDrawMode.Tile;
    this.treeView.BorderStyle = BorderStyle.Fixed3D;
    this.treeView.SelectionMode = Infralution.Controls.VirtualTree.SelectionMode.FullRow;
    this.toolBar.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.btnTestScheme
    });
    this.btnDelete.Enabled = false;
    this.btnProperties.Enabled = false;
    this.actList.SetAction((Component) this.btnTestScheme, this.actTestScheme);
    this.btnTestScheme.BeginGroup = true;
    this.btnTestScheme.CommandName = "btnTestConfig";
    this.btnTestScheme.Icon = (Icon) componentResourceManager.GetObject("btnTestScheme.Icon");
    this.btnTestScheme.Text = "Проверить схему";
    this.btnTestScheme.ToolTipText = "Проверить схему";
    this.actList.Actions.AddRange(new Intermech.Actions.Action[1]
    {
      this.actTestScheme
    });
    this.actList.ImageList = (ImageList) null;
    this.actList.ShowTextOnToolBar = false;
    this.actList.Tag = (object) null;
    this.actTestScheme.Hint = (string) null;
    this.actTestScheme.Text = "Проверить схему";
    this.actTestScheme.Execute += new EventHandler(this.actTestScheme_Execute);
    this.AttributeImage = (Image) componentResourceManager.GetObject("$this.AttributeImage");
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (ResponceSchemeTreeView);
    this.NodeImage = (Image) componentResourceManager.GetObject("$this.NodeImage");
    this.treeView.EndInit();
    this.ResumeLayout(false);
  }
}
