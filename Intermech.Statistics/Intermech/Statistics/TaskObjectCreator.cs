// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.TaskObjectCreator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class TaskObjectCreator : Form
{
  private int _objectTypeID;
  public bool OpenConfig;
  public long ObjectID = -1;
  private IContainer components;
  private CheckBox configOpen;
  private Button cancelBtn;
  private Button okBtn;
  private Label label1;
  private TextBox nameTb;

  public TaskObjectCreator(int objectTypeID)
  {
    this.InitializeComponent();
    this._objectTypeID = objectTypeID;
  }

  private void okBtn_Click(object sender, EventArgs e)
  {
    if (this.nameTb.Text.Length > 0)
    {
      this.CreateObject();
      this.OpenConfig = this.configOpen.Checked;
      this.DialogResult = DialogResult.OK;
    }
    else
    {
      int num = (int) MessageBox.Show("Поле 'Наименование' не может быть пустым.");
    }
  }

  private void cancelBtn_Click(object sender, EventArgs e) => this.Close();

  private void CreateObject()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(this._objectTypeID);
      if (objectCollection == null)
        return;
      IDBTransactions customService = sessionKeeper.Session.GetCustomService(typeof (IDBTransactions)) as IDBTransactions;
      try
      {
        customService?.StartTransaction();
        IDBObject dbObject1 = objectCollection.Create();
        if (dbObject1 == null)
        {
          customService?.Rollback();
        }
        else
        {
          long objectId = dbObject1.ObjectID;
          AttributeValues[] valuesList = new AttributeValues[1];
          int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
          valuesList[0] = new AttributeValues(attributeTypeId, (object) this.nameTb.Text);
          dbObject1.SetAttributesValues(valuesList);
          IDBObject dbObject2 = sessionKeeper.Session.GetObject(objectId);
          if (dbObject2 != null)
          {
            if (dbObject2.IsCreationMode)
              dbObject2.CommitCreation(true);
            objectId = dbObject2.ObjectID;
          }
          this.ObjectID = objectId;
          customService?.Commit();
        }
      }
      catch
      {
        customService?.Rollback();
        throw;
      }
    }
  }

  private void TaskObjectCreator_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this.DialogResult == DialogResult.OK && this.DialogResult == DialogResult.Cancel)
      return;
    e.Cancel = false;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.configOpen = new CheckBox();
    this.cancelBtn = new Button();
    this.okBtn = new Button();
    this.label1 = new Label();
    this.nameTb = new TextBox();
    this.SuspendLayout();
    this.configOpen.AutoSize = true;
    this.configOpen.Checked = true;
    this.configOpen.CheckState = CheckState.Checked;
    this.configOpen.Location = new Point(15, 40);
    this.configOpen.Name = "configOpen";
    this.configOpen.Size = new Size(182, 17);
    this.configOpen.TabIndex = 2;
    this.configOpen.Text = "Открыть конфигуратор задачи";
    this.configOpen.UseVisualStyleBackColor = true;
    this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cancelBtn.DialogResult = DialogResult.Cancel;
    this.cancelBtn.Location = new Point(297, 36);
    this.cancelBtn.Name = "cancelBtn";
    this.cancelBtn.Size = new Size(75, 23);
    this.cancelBtn.TabIndex = 4;
    this.cancelBtn.Text = "Отмена";
    this.cancelBtn.UseVisualStyleBackColor = true;
    this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
    this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.okBtn.Location = new Point(216, 36);
    this.okBtn.Name = "okBtn";
    this.okBtn.Size = new Size(75, 23);
    this.okBtn.TabIndex = 3;
    this.okBtn.Text = "ОК";
    this.okBtn.UseVisualStyleBackColor = true;
    this.okBtn.Click += new EventHandler(this.okBtn_Click);
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 14);
    this.label1.Name = "label1";
    this.label1.Size = new Size(89, 13);
    this.label1.TabIndex = 8;
    this.label1.Text = "Наименование: ";
    this.nameTb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.nameTb.Location = new Point(108, 11);
    this.nameTb.Name = "nameTb";
    this.nameTb.Size = new Size(264, 20);
    this.nameTb.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.cancelBtn;
    this.ClientSize = new Size(384, 71);
    this.Controls.Add((Control) this.configOpen);
    this.Controls.Add((Control) this.cancelBtn);
    this.Controls.Add((Control) this.okBtn);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.nameTb);
    this.MaximumSize = new Size(400, 110);
    this.MinimumSize = new Size(400, 110);
    this.Name = nameof (TaskObjectCreator);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Создание задачи сбора статистики";
    this.FormClosing += new FormClosingEventHandler(this.TaskObjectCreator_FormClosing);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
