// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.FindDialog
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;
using Intermech.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class FindDialog : Form
{
  private MapView map;
  private static List<string> findHistory = new List<string>();
  private MapObject foundNode;
  private IContainer components;
  private ComboBox comboBox1;
  private Label label1;
  private Button findButton;
  private Button button2;
  private CheckBox cbCaseSensitive;

  public FindDialog()
  {
    this.InitializeComponent();
    foreach (object obj in FindDialog.findHistory)
      this.comboBox1.Items.Add(obj);
  }

  public FindDialog(MapView map)
    : this()
  {
    this.map = map;
  }

  private void findButton_Click(object sender, EventArgs e)
  {
    if (this.map == null || this.map.Document == null)
      return;
    string text = this.comboBox1.Text;
    bool flag = this.cbCaseSensitive.Checked;
    if (!text.Equals(string.Empty))
    {
      MapObject node = this.FindNode(this.map.Document, text, true, !flag, this.foundNode);
      if (node != null)
      {
        this.foundNode = node;
        this.SelectObject(node);
      }
      else
      {
        int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_530"), LocalizationHolder.rm.GetString("Pdm_531"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }
    if (FindDialog.findHistory.Contains(text))
      return;
    this.comboBox1.Items.Add((object) text);
    FindDialog.findHistory.Add(text);
  }

  private MapObject FindNode(
    MapDocument doc,
    string s,
    bool prefix,
    bool ignoreCase,
    MapObject startObj)
  {
    MapLayerCollectionObjectEnumerator enumerator = doc.GetEnumerator();
    bool flag1 = this.foundNode == null;
    MapObject node = (MapObject) null;
    while (enumerator.MoveNext())
    {
      if (enumerator.Current is VisNode current && current.Layer.AllowView)
      {
        bool flag2 = current.TopLabel != null && this.IsTextFound(current.TopLabel.Text, s, prefix, ignoreCase) || current.BottomLabel != null && this.IsTextFound(current.BottomLabel.Text, s, prefix, ignoreCase) || current.Obj != null && this.IsTextFound(current.Obj.Caption, s, prefix, ignoreCase) || current.Obj != null && this.IsTextFound(current.Obj.ObjVerId.ToString(), s, prefix, ignoreCase);
        if (flag2 && node == null)
          node = (MapObject) current;
        if (!flag1)
        {
          if (current == startObj)
            flag1 = true;
        }
        else if (flag2)
          return (MapObject) current;
      }
    }
    return node;
  }

  private bool IsTextFound(string text2, string s, bool prefix, bool ignoreCase)
  {
    string str = s;
    if (ignoreCase)
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      str = str.ToUpper(currentCulture);
      text2 = text2.ToUpper(currentCulture);
    }
    return prefix ? text2.Contains(str) : text2 == str;
  }

  private void SelectObject(MapObject seObj)
  {
    this.map.Selection.Clear();
    this.map.Selection.Add(seObj);
    (this.map as VisControl).ScrollToControl(seObj);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FindDialog));
    this.comboBox1 = new ComboBox();
    this.label1 = new Label();
    this.findButton = new Button();
    this.button2 = new Button();
    this.cbCaseSensitive = new CheckBox();
    this.SuspendLayout();
    this.comboBox1.FormattingEnabled = true;
    componentResourceManager.ApplyResources((object) this.comboBox1, "comboBox1");
    this.comboBox1.Name = "comboBox1";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.findButton, "findButton");
    this.findButton.Name = "findButton";
    this.findButton.UseVisualStyleBackColor = true;
    this.findButton.Click += new EventHandler(this.findButton_Click);
    this.button2.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.button2, "button2");
    this.button2.Name = "button2";
    this.button2.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.cbCaseSensitive, "cbCaseSensitive");
    this.cbCaseSensitive.Name = "cbCaseSensitive";
    this.cbCaseSensitive.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.findButton;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button2;
    this.Controls.Add((Control) this.cbCaseSensitive);
    this.Controls.Add((Control) this.button2);
    this.Controls.Add((Control) this.findButton);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.comboBox1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (FindDialog);
    this.ShowInTaskbar = false;
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
