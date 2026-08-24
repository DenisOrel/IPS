// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.CustomFindReplaceDialogTelerik
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinForms.Documents.UI.Extensibility;
using Telerik.WinForms.RichTextEditor;

#nullable disable
namespace Intermech.Requirement;

public class CustomFindReplaceDialogTelerik : Form, IFindReplaceDialog
{
  private IContainer components;

  public CustomFindReplaceDialogTelerik() => this.InitializeComponent();

  public void Show(
    RadRichTextBox richTextBox,
    Func<string, bool> replaceCallback,
    string textToFind)
  {
  }

  public bool IsOpen { get; }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(375, 113);
    this.Name = nameof (CustomFindReplaceDialogTelerik);
    this.Text = nameof (CustomFindReplaceDialogTelerik);
    this.ResumeLayout(false);
  }

  void IFindReplaceDialog.Close() => this.Close();
}
