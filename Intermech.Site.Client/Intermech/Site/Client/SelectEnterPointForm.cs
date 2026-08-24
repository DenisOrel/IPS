// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.SelectEnterPointForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal class SelectEnterPointForm : Form
{
  private IContainer components;
  private ComboBox comboBox1;
  private Button bOK;
  private Button bCancel;

  public SelectEnterPointForm() => this.InitializeComponent();

  public char? SelectedSite
  {
    get
    {
      return this.comboBox1.SelectedItem is SelectEnterPointForm.SiteItem selectedItem && !selectedItem.Empty ? new char?(selectedItem.Code) : new char?();
    }
  }

  public void Init(IUserSession session, char? selected = null)
  {
    this.comboBox1.Items.Clear();
    this.comboBox1.Items.Add((object) new SelectEnterPointForm.SiteItem());
    ISitesCacheService customService = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
    int num1 = 0;
    int num2 = 1;
    foreach (SiteInfo site in customService.Sites)
    {
      if (!site.Code.Equals(customService.Info.Code))
      {
        if (selected.HasValue && site.Code.Equals((object) selected))
          num1 = num2;
        this.comboBox1.Items.Add((object) new SelectEnterPointForm.SiteItem(site.Caption, site.Code));
        ++num2;
      }
    }
    this.comboBox1.SelectedIndex = num1;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.comboBox1 = new ComboBox();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.SuspendLayout();
    this.comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
    this.comboBox1.FormattingEnabled = true;
    this.comboBox1.Location = new Point(23, 27);
    this.comboBox1.Name = "comboBox1";
    this.comboBox1.Size = new Size(355, 21);
    this.comboBox1.TabIndex = 0;
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Location = new Point(130, 63 /*0x3F*/);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 1;
    this.bOK.Text = "ОК";
    this.bOK.UseVisualStyleBackColor = true;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(257, 63 /*0x3F*/);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 2;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(403, 106);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.bOK);
    this.Controls.Add((Control) this.comboBox1);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(311, 140);
    this.Name = nameof (SelectEnterPointForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выбор точки ввода";
    this.ResumeLayout(false);
  }

  private class SiteItem
  {
    public string Caption;
    public char Code;
    public bool Empty;

    public SiteItem(string caption, char code)
    {
      this.Caption = caption;
      this.Code = code;
      this.Empty = false;
    }

    public SiteItem() => this.Empty = true;

    public override string ToString()
    {
      return !this.Empty ? $"{this.Code} {this.Caption}" : "Не назначена";
    }
  }
}
