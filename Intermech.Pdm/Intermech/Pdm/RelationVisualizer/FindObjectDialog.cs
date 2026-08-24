// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.FindObjectDialog
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

public class FindObjectDialog : Form
{
  private static List<string> findHistory = new List<string>();
  private MapView map;
  private MapObject foundNode;
  private IContainer components;
  private ComboBox comboBox1;
  private Label label1;
  private Button button1;
  private Button button2;
  private CheckBox checkBox1;

  public FindObjectDialog()
  {
    this.InitializeComponent();
    foreach (object obj in FindObjectDialog.findHistory)
      this.comboBox1.Items.Add(obj);
  }

  public FindObjectDialog(MapView map)
    : this()
  {
    this.map = map;
  }

  private void button1_Click(object sender, EventArgs e)
  {
    if (this.map == null || this.map.Document == null)
      return;
    string text = this.comboBox1.Text;
    bool flag = this.checkBox1.Checked;
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
    if (FindObjectDialog.findHistory.Contains(text))
      return;
    this.comboBox1.Items.Add((object) text);
    FindObjectDialog.findHistory.Add(text);
  }

  private MapObject FindNode(
    MapDocument doc,
    string s,
    bool prefix,
    bool ignorecase,
    MapObject startObj)
  {
    MapLayerCollectionObjectEnumerator enumerator = doc.GetEnumerator();
    bool flag = this.foundNode == null;
    MapObject node1 = (MapObject) null;
    while (enumerator.MoveNext())
    {
      MapObject current = enumerator.Current;
      bool node2 = this.FindNode(current, s, prefix, ignorecase);
      if (node2 && node1 == null)
        node1 = current;
      if (!flag)
      {
        if (current == startObj)
          flag = true;
      }
      else if (node2)
        return current;
    }
    return node1;
  }

  private bool FindNode(MapObject obj, string s, bool prefix, bool ignorecase)
  {
    CultureInfo currentCulture = CultureInfo.CurrentCulture;
    string str1 = s;
    if (ignorecase)
      str1 = str1.ToUpper(currentCulture);
    if (obj is IMapLabeledNode mapLabeledNode)
    {
      string str2 = mapLabeledNode.Text;
      if (ignorecase)
        str2 = str2.ToUpper(currentCulture);
      if (prefix)
        return str2.Contains(str1);
      if (str2 == str1)
        return true;
    }
    return false;
  }

  private void SelectObject(MapObject seObj)
  {
    this.map.Selection.Clear();
    this.map.Selection.Add(seObj);
    (this.map as RelViewControl).ScrollToControl(seObj);
  }

  protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
  {
    return base.ProcessCmdKey(ref msg, keyData);
  }

  protected override bool ProcessDialogKey(Keys keyData) => base.ProcessDialogKey(keyData);

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FindObjectDialog));
    this.comboBox1 = new ComboBox();
    this.label1 = new Label();
    this.button1 = new Button();
    this.button2 = new Button();
    this.checkBox1 = new CheckBox();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.comboBox1, "comboBox1");
    this.comboBox1.FormattingEnabled = true;
    this.comboBox1.Name = "comboBox1";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.button1, "button1");
    this.button1.Name = "button1";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    componentResourceManager.ApplyResources((object) this.button2, "button2");
    this.button2.DialogResult = DialogResult.Cancel;
    this.button2.Name = "button2";
    this.button2.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.checkBox1, "checkBox1");
    this.checkBox1.Name = "checkBox1";
    this.checkBox1.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.button1;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button2;
    this.Controls.Add((Control) this.checkBox1);
    this.Controls.Add((Control) this.button2);
    this.Controls.Add((Control) this.button1);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.comboBox1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (FindObjectDialog);
    this.ShowInTaskbar = false;
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
