// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.Settings.RequestSchemeItemFrm
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client.Settings;

public class RequestSchemeItemFrm : BaseSchemeItemFrm
{
  private IContainer components;

  public RequestSchemeItemFrm() => this.InitializeComponent();

  private void RequestSchemeItemFrm_OnInsertAttribute(object sender, ButtonEditEventArgs args)
  {
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false, new int[1]))
    {
      attributesSelectDlg.LoadAttrDialogForObjectsTypes(MetaDataHelper.GetObjectTypeGuid(Const.RequestObjTypeID));
      if (!attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK) || attributesSelectDlg.SelectedAttributesID.Count <= 0)
        return;
      int attrTypeID = attributesSelectDlg.SelectedAttributesID[0];
      if (attrTypeID == 0)
        return;
      string str = args.edit.Value;
      args.edit.Value = str.Insert(args.edit.CaretPosition, $"[{MetaDataHelper.GetAttributeTypeName(attrTypeID)}]");
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
    this.SuspendLayout();
    this.edName.Size = new Size(314, 20);
    this.edValue.ShowButton = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(359, 177);
    this.Name = nameof (RequestSchemeItemFrm);
    this.Text = "Свойства элемента";
    this.OnInsertAttribute += new BaseSchemeItemFrm.InsertAttributeEventHandler(this.RequestSchemeItemFrm_OnInsertAttribute);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
