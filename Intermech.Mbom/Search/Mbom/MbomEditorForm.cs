// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomEditorForm
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Utilities;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class MbomEditorForm : Form
{
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private FlowLayoutPanel flowLayoutPanel1;
  private Button _closeButton;
  private MbomEditorControl _mbomEditorControl;

  public MbomEditorForm() => this.InitializeComponent();

  public long EbomVersionID
  {
    get => this._mbomEditorControl.EbomVersionID;
    set
    {
      this._mbomEditorControl.EbomVersionID = !ObjectHelper.IsUnknownObjectVersionID(value) ? value : throw new ArgumentException();
    }
  }

  public long MbomVersionID
  {
    get => this._mbomEditorControl.MbomVersionID;
    set
    {
      this._mbomEditorControl.MbomVersionID = !ObjectHelper.IsUnknownObjectVersionID(value) ? value : throw new ArgumentException();
    }
  }

  private void MbomEditorForm_Load(object sender, EventArgs e)
  {
    Hashtable hashtable = new Hashtable();
    FormStorage.LoadLayout((Control) this, (IDictionary) hashtable);
    this._mbomEditorControl.SetMemento(new MbomEditorControl.MbomEditorControlMemento()
    {
      EbomNodeColumns = hashtable[(object) "EbomNodeColumns"] as NodeColumnCollection,
      MbomNodeColumns = hashtable[(object) "MbomNodeColumns"] as NodeColumnCollection
    });
  }

  private void MbomEditorForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    Hashtable hashtable = new Hashtable();
    MbomEditorControl.MbomEditorControlMemento memento = this._mbomEditorControl.GetMemento();
    hashtable[(object) "EbomNodeColumns"] = (object) memento.EbomNodeColumns;
    hashtable[(object) "MbomNodeColumns"] = (object) memento.MbomNodeColumns;
    FormStorage.SaveLayout((Control) this, (IDictionary) hashtable);
  }

  private void CloseButton_Click(object sender, EventArgs e) => this.Close();

  private string Serialize(object @object)
  {
    try
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, @object);
        return Convert.ToBase64String(serializationStream.GetBuffer());
      }
    }
    catch (Exception ex)
    {
      this.WriteExceptionToOutputView(ex);
      return (string) null;
    }
  }

  private object Deserialize(string @string)
  {
    try
    {
      using (MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(@string)))
        return new BinaryFormatter().Deserialize((Stream) serializationStream);
    }
    catch (Exception ex)
    {
      this.WriteExceptionToOutputView(ex);
      return (object) null;
    }
  }

  private void WriteExceptionToOutputView(Exception x)
  {
    ServiceLocator.Get<IOutputView>().WriteString("Ошибки", x.Message);
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
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this._closeButton = new Button();
    this._mbomEditorControl = new MbomEditorControl();
    this.tableLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this._mbomEditorControl.BeginInit();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this._mbomEditorControl, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 2;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
    this.tableLayoutPanel1.Size = new Size(487, 368);
    this.tableLayoutPanel1.TabIndex = 0;
    this.flowLayoutPanel1.Controls.Add((Control) this._closeButton);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
    this.flowLayoutPanel1.Location = new Point(3, 331);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(481, 34);
    this.flowLayoutPanel1.TabIndex = 0;
    this._closeButton.Location = new Point(403, 3);
    this._closeButton.Name = "_closeButton";
    this._closeButton.Size = new Size(75, 23);
    this._closeButton.TabIndex = 0;
    this._closeButton.Text = "Закрыть";
    this._closeButton.UseVisualStyleBackColor = true;
    this._closeButton.Click += new EventHandler(this.CloseButton_Click);
    this._mbomEditorControl.Dock = DockStyle.Fill;
    this._mbomEditorControl.Location = new Point(3, 3);
    this._mbomEditorControl.Name = "_mbomEditorControl";
    this._mbomEditorControl.Size = new Size(481, 322);
    this._mbomEditorControl.TabIndex = 1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(487, 368);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (MbomEditorForm);
    this.ShowIcon = false;
    this.Text = "Редактор ТЭСИ";
    this.FormClosing += new FormClosingEventHandler(this.MbomEditorForm_FormClosing);
    this.Load += new EventHandler(this.MbomEditorForm_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.ResumeLayout(false);
    this._mbomEditorControl.EndInit();
    this.ResumeLayout(false);
  }
}
