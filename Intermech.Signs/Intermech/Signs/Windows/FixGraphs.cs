// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.FixGraphs
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Workflow;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Windows;

public class FixGraphs : Form
{
  private IContainer components;
  private Button button1;
  private ListBox listBox1;

  public FixGraphs() => this.InitializeComponent();

  private void Add2Trace(string message)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.AddToTrace(message, Consts.traceAlways, "ConvertSignGraphs.log");
      this.listBox1.Items.Add((object) message);
      Application.DoEvents();
    }
  }

  private void FixGraphs_Shown(object sender, EventArgs e)
  {
    this.button1.Enabled = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      try
      {
        long num = sessionKeeper.Session.Configurations.ReadInteger("KERNEL", "SIGNS", "ConvertSignGraphs", 0L, DBConfigMode.GlobalOnly);
        ISignsService customService = sessionKeeper.Session.GetCustomService(typeof (ISignsService)) as ISignsService;
        string message;
        if (num < 1L)
        {
          this.Add2Trace("Выполняется замена значений граф для подписей в метаданных и объектах...");
          IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(SignConsts.GraphAttrTypeGuid);
          DataTable possibleValues = attributeType.GetPossibleValues();
          DataTable valuesTable = possibleValues.Copy();
          valuesTable.Columns.Add("F_OID", typeof (int));
          Dictionary<string, string> dictionary = new Dictionary<string, string>(possibleValues.Rows.Count);
          for (int index = 0; index < valuesTable.Rows.Count; ++index)
          {
            dictionary.Add(valuesTable.Rows[index]["F_STRING_VALUE"].ToString(), valuesTable.Rows[index]["F_DESCRIPTION"].ToString());
            valuesTable.Rows[index]["F_STRING_VALUE"] = valuesTable.Rows[index]["F_DESCRIPTION"];
            valuesTable.Rows[index]["F_OID"] = valuesTable.Rows[index]["F_INLIST_ID"];
          }
          attributeType.SetPossibleValues(valuesTable);
          this.Add2Trace("Исправление настроек подписей в шаблонах процессов и выполняемых процессах...");
          this.Add2Trace((sessionKeeper.Session.GetCustomService(typeof (IApproveGraphValueReplaceService)) as IApproveGraphValueReplaceService).ReplaceGraphsInApproveExecutedProcessAndAllSchemes(dictionary, sessionKeeper.Session.SessionGUID));
          this.Add2Trace("Исправление настроек подписей для шагов ЖЦ и уровней продвижения...");
          this.Add2Trace(customService.PatchSignGraphsForLCStepsAndLCLevels(dictionary, sessionKeeper.Session.SessionGUID));
          this.Add2Trace("Исправление настроек подписей для должностей...");
          this.Add2Trace(customService.PatchSignGraphsForRanks(dictionary, sessionKeeper.Session.SessionGUID));
          this.Add2Trace("Исправление настроек подписей для архивов...");
          message = customService.PatchSignGraphsForAllArchives(dictionary, sessionKeeper.Session.SessionGUID);
          sessionKeeper.Session.Configurations.WriteInteger("KERNEL", "SIGNS", "ConvertSignGraphs", 1L, 0L);
          this.Add2Trace(message);
        }
        if (num >= 2L)
          return;
        this.Add2Trace("Выполняется пересчет контрольных сумм подписей...");
        customService.UpdateSignsHashes(sessionKeeper.Session.SessionGUID, out message);
        sessionKeeper.Session.Configurations.WriteInteger("KERNEL", "SIGNS", "ConvertSignGraphs", 2L, 0L);
        this.Add2Trace(message);
        this.Add2Trace("Преобразование граф для подписей завершено.");
      }
      finally
      {
        this.button1.Enabled = true;
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
    this.button1 = new Button();
    this.listBox1 = new ListBox();
    this.SuspendLayout();
    this.button1.DialogResult = DialogResult.OK;
    this.button1.Location = new Point(570, 420);
    this.button1.Name = "button1";
    this.button1.Size = new Size(98, 23);
    this.button1.TabIndex = 1;
    this.button1.Text = "Закрыть";
    this.button1.UseVisualStyleBackColor = true;
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Location = new Point(12, 12);
    this.listBox1.Name = "listBox1";
    this.listBox1.Size = new Size(680, 394);
    this.listBox1.TabIndex = 2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(704, 455);
    this.Controls.Add((Control) this.listBox1);
    this.Controls.Add((Control) this.button1);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (FixGraphs);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Преобразование значений граф для подписей";
    this.Shown += new EventHandler(this.FixGraphs_Shown);
    this.ResumeLayout(false);
  }
}
