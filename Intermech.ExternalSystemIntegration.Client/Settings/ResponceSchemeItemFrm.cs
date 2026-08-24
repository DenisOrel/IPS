// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.Settings.ResponceSchemeItemFrm
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client.Settings;

public class ResponceSchemeItemFrm : BaseSchemeItemFrm
{
  private IContainer components;

  public ResponceSchemeItemFrm() => this.InitializeComponent();

  private void ResponceSchemeItemFrm_OnInsertAttribute(object sender, ButtonEditEventArgs args)
  {
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false, new int[1]))
    {
      attributesSelectDlg.LoadAttrDialogForObjectsTypes(MetaDataHelper.GetObjectTypeGuid(Const.ResponceObjTypeID));
      if (!attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK) || attributesSelectDlg.SelectedAttributesID.Count <= 0)
        return;
      int attrTypeID = attributesSelectDlg.SelectedAttributesID[0];
      if (attrTypeID <= 0)
        return;
      string str = args.edit.Value;
      args.edit.Value = str.Insert(args.edit.CaretPosition, $"[{MetaDataHelper.GetAttributeTypeName(attrTypeID)}]");
    }
  }

  protected override void buttonOK_Click(object sender, EventArgs e)
  {
    string str = this.edValue.Value;
    int num1 = str.IndexOf('[');
    int num2 = str.IndexOf(']');
    if (num1 > -1 && num2 > num1 && num2 < str.Length - 1)
    {
      int num3 = (int) MessageBox.Show($"Значением элемента может быть атрибут или текст!{Environment.NewLine} Использовать одновременно атрибут и текст запрещено", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
      base.buttonOK_Click(sender, e);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.edName.Size = new Size(314, 20);
    this.edValue.ShowButton = true;
    this.chbCData.Visible = false;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(359, 178);
    this.Name = nameof (ResponceSchemeItemFrm);
    this.Text = "Свойства элемента";
    this.OnInsertAttribute += new BaseSchemeItemFrm.InsertAttributeEventHandler(this.ResponceSchemeItemFrm_OnInsertAttribute);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
