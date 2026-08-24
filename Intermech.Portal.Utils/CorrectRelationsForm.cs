// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Utils.CorrectRelationsForm
// Assembly: Intermech.Portal.Utils, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 99780CCF-14B7-482E-A297-41CC169803AE
// Assembly location: D:\IPS\Client\Intermech.Portal.Utils.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Portal.Utils;

public class CorrectRelationsForm : Form
{
  private bool _started;
  private int _count;
  private IContainer components;
  private Button button1;
  private Button button2;
  private ProgressBar progressBar1;
  private Label label1;
  private Label label2;
  private ListBox listBox1;
  private Label label3;
  private ContextMenuStrip contextMenuStrip1;
  private ToolStripMenuItem CopyToolStripMenuItem;
  private ToolStripMenuItem SaveToolStripMenuItem;
  private SaveFileDialog saveFileDialog1;

  public CorrectRelationsForm() => this.InitializeComponent();

  private void button1_Click(object sender, EventArgs e)
  {
    if (this._started)
    {
      int num = (int) MessageBox.Show("Процесс корректировки уже запущен!");
    }
    else
    {
      this.progressBar1.Value = 0;
      this.listBox1.Items.Clear();
      new Thread(new ParameterizedThreadStart(this.WorkThreadMethod))
      {
        IsBackground = true,
        Name = "Portal_CorrectRelationsThread"
      }.Start((object) this);
    }
  }

  private void SetCount(int count) => this.progressBar1.Maximum = count;

  private void SetProgress(int step)
  {
    this.progressBar1.Value = step;
    this.label2.Text = $"{step} из {this._count}";
  }

  private void WriteMessage(string message) => this.listBox1.Items.Add((object) message);

  private void WorkThreadMethod(object obj)
  {
    CorrectRelationsForm correctRelationsForm = (CorrectRelationsForm) obj;
    Guid guid = new Guid("cad01493-306c-11d8-b4e9-00304f19f545");
    try
    {
      this._started = true;
      this._count = 0;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        DataTable dataTable = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PortalConsts.reltypePublish)).Select(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(guid, RelationalOperators.NotEmpty, (object) null, LogicalOperators.NONE, 0)
        }, new object[1]{ (object) -20 }));
        this._count = dataTable.Rows.Count;
        correctRelationsForm.Invoke((Delegate) new CorrectRelationsForm.SetCountDelegate(this.SetCount), (object) this._count);
        IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(PortalConsts.objtypePublishObjects);
        for (int index = 0; index < this._count; ++index)
        {
          correctRelationsForm.Invoke((Delegate) new CorrectRelationsForm.SetProgressDelegate(this.SetProgress), (object) (index + 1));
          string message = string.Empty;
          RelationsFileCorrector.Correct(sessionKeeper.Session, objectCollection, Convert.ToInt64(dataTable.Rows[index][0]), guid, out message);
          if (message != string.Empty)
            correctRelationsForm.Invoke((Delegate) new CorrectRelationsForm.WriteMessageDelegate(this.WriteMessage), (object) message);
        }
      }
      int num = (int) MessageBox.Show($"Корректировка окончена. Обработано {this._count} связей", "Корректировка связей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message);
    }
    finally
    {
      this._started = false;
    }
  }

  private void button2_Click(object sender, EventArgs e)
  {
    if (this._started)
    {
      int num = (int) MessageBox.Show("Нельзя закрывать форму пока идет процесс корректировки!");
    }
    else
      this.Close();
  }

  private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
      return;
    StreamWriter text = File.CreateText(this.saveFileDialog1.FileName);
    try
    {
      text.Write(this.GetMessages());
    }
    finally
    {
      text.Flush();
      text.Close();
    }
  }

  private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
  {
    Clipboard.SetData(DataFormats.StringFormat, (object) this.GetMessages());
  }

  private string GetMessages()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < this.listBox1.Items.Count; ++index)
      stringBuilder.AppendLine(this.listBox1.Items[index].ToString());
    return stringBuilder.ToString();
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
    this.button1 = new Button();
    this.button2 = new Button();
    this.progressBar1 = new ProgressBar();
    this.label1 = new Label();
    this.label2 = new Label();
    this.listBox1 = new ListBox();
    this.contextMenuStrip1 = new ContextMenuStrip(this.components);
    this.CopyToolStripMenuItem = new ToolStripMenuItem();
    this.SaveToolStripMenuItem = new ToolStripMenuItem();
    this.label3 = new Label();
    this.saveFileDialog1 = new SaveFileDialog();
    this.contextMenuStrip1.SuspendLayout();
    this.SuspendLayout();
    this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.button1.Location = new Point(602, 91);
    this.button1.Name = "button1";
    this.button1.Size = new Size(121, 27);
    this.button1.TabIndex = 0;
    this.button1.Text = "Старт";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.button2.DialogResult = DialogResult.Cancel;
    this.button2.Location = new Point(729, 91);
    this.button2.Name = "button2";
    this.button2.Size = new Size(121, 27);
    this.button2.TabIndex = 1;
    this.button2.Text = "Закрыть";
    this.button2.UseVisualStyleBackColor = true;
    this.button2.Click += new EventHandler(this.button2_Click);
    this.progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.progressBar1.Location = new Point(30, 49);
    this.progressBar1.Maximum = 999999999;
    this.progressBar1.Name = "progressBar1";
    this.progressBar1.Size = new Size(820, 23);
    this.progressBar1.Step = 1;
    this.progressBar1.TabIndex = 2;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(27, 33);
    this.label1.Name = "label1";
    this.label1.Size = new Size(109, 13);
    this.label1.TabIndex = 3;
    this.label1.Text = "Ход корректировки:";
    this.label2.Location = new Point(142, 33);
    this.label2.Name = "label2";
    this.label2.Size = new Size(392, 13);
    this.label2.TabIndex = 4;
    this.listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Location = new Point(30, 145);
    this.listBox1.Name = "listBox1";
    this.listBox1.Size = new Size(820, 264);
    this.listBox1.TabIndex = 5;
    this.contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.CopyToolStripMenuItem,
      (ToolStripItem) this.SaveToolStripMenuItem
    });
    this.contextMenuStrip1.Name = "contextMenuStrip1";
    this.contextMenuStrip1.Size = new Size(140, 48 /*0x30*/);
    this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
    this.CopyToolStripMenuItem.Size = new Size(139, 22);
    this.CopyToolStripMenuItem.Text = "Копировать";
    this.CopyToolStripMenuItem.Click += new EventHandler(this.CopyToolStripMenuItem_Click);
    this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
    this.SaveToolStripMenuItem.Size = new Size(139, 22);
    this.SaveToolStripMenuItem.Text = "Сохранить";
    this.SaveToolStripMenuItem.Click += new EventHandler(this.SaveToolStripMenuItem_Click);
    this.label3.AutoSize = true;
    this.label3.Location = new Point(27, 129);
    this.label3.Name = "label3";
    this.label3.Size = new Size(50, 13);
    this.label3.TabIndex = 6;
    this.label3.Text = "Ошибки:";
    this.saveFileDialog1.DefaultExt = "txt";
    this.saveFileDialog1.Filter = "Текстовый файл|*.txt|Все файлы|*.*";
    this.saveFileDialog1.RestoreDirectory = true;
    this.saveFileDialog1.SupportMultiDottedExtensions = true;
    this.AcceptButton = (IButtonControl) this.button1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button2;
    this.ClientSize = new Size(868, 443);
    this.Controls.Add((Control) this.label3);
    this.Controls.Add((Control) this.listBox1);
    this.Controls.Add((Control) this.label2);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.progressBar1);
    this.Controls.Add((Control) this.button2);
    this.Controls.Add((Control) this.button1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (CorrectRelationsForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Корректировка связей";
    this.contextMenuStrip1.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void SetCountDelegate(int count);

  private delegate void SetProgressDelegate(int step);

  private delegate void WriteMessageDelegate(string message);
}
