// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepControls.StepControlResult
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.StepControls;

public class StepControlResult : StepControl
{
  private Image _image;
  private IContainer components;
  private GroupBox groupBox1;
  private Label label1;

  public StepControlResult(object owner)
  {
    this.InitializeComponent();
    this.stepPrevAllowed = false;
    this.owner = owner;
  }

  protected override string getCaption() => "Завершение перекачки";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgComplited")];
    return this._image;
  }

  public override void RefreshControl()
  {
    ResultTypes resultType = (this.owner as WizardForm).ResultType;
    if ((resultType & ResultTypes.MetadataTerminate) == ResultTypes.None && (resultType & ResultTypes.Terminate) == ResultTypes.None)
    {
      ISavePoint service = ServicesManager.GetService(typeof (ISavePoint)) as ISavePoint;
      SavePoint savePoint = service.GetSavePoint();
      savePoint.OperationTerminateType = TerminateType.Complete;
      service.SetSavePoint(savePoint);
    }
    this.label1.Text = string.Empty;
    if ((resultType & ResultTypes.MetadataTerminate) == ResultTypes.MetadataTerminate)
      this.label1.Text = "Процесс миграции метаданных был завершен.";
    else if ((resultType & ResultTypes.Terminate) == ResultTypes.Terminate)
      this.label1.Text = "Процесс миграции был прерван.";
    else
      this.label1.Text = "Процесс миграции был завершен.";
    (this.owner as WizardForm).StopTimer();
    base.RefreshControl();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.groupBox1 = new GroupBox();
    this.label1 = new Label();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Controls.Add((Control) this.label1);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(333, 252);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.label1.Dock = DockStyle.Fill;
    this.label1.Location = new Point(3, 16 /*0x10*/);
    this.label1.Name = "label1";
    this.label1.Size = new Size(327, 233);
    this.label1.TabIndex = 0;
    this.label1.Text = "Перекачка успешно завершена";
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (StepControlResult);
    this.Size = new Size(333, 252);
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
