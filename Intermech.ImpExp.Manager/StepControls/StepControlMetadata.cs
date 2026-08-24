// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepControls.StepControlMetadata
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.StepControls;

public class StepControlMetadata : StepControl
{
  private Image _image;
  private IContainer components;
  private GroupBox groupBox1;
  private Label label1;

  public StepControlMetadata(object owner)
    : base(owner)
  {
    this.InitializeComponent();
    this.stepPrevAllowed = false;
  }

  public virtual event EventHandler OnEndSaveMetadata;

  protected override string getCaption() => "Импорт метаданных";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgImportMetadata")];
    return this._image;
  }

  public override SaveSettingsResult SaveSettings()
  {
    if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service1 && !service1.CheckDBVersion(true))
      return SaveSettingsResult.ssrRetry;
    if (service1 != null && service1.MetadataSaveToServer())
    {
      ISavePoint service2 = ServicesManager.GetService(typeof (ISavePoint)) as ISavePoint;
      SavePoint point = service2.GetSavePoint();
      if (point == null)
        point = new SavePoint(TerminateType.SaveMetadata);
      else
        point.OperationTerminateType = TerminateType.SaveMetadata;
      service2.SetSavePoint(point);
      if (this.OnEndSaveMetadata != null)
        this.OnEndSaveMetadata((object) this, new EventArgs());
      (this.owner as WizardForm).AddInfoMessage("Импорт метаданных прошел успешно");
      return SaveSettingsResult.ssrOk;
    }
    string str = "В процессе импорта метаданных возникла ошибка. См.лог файлы.";
    (this.owner as WizardForm).AddErrorMessage(str);
    int num = (int) MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    return SaveSettingsResult.ssrError;
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
    this.groupBox1.Size = new Size(384, 305);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.label1.Dock = DockStyle.Fill;
    this.label1.Location = new Point(3, 16 /*0x10*/);
    this.label1.Name = "label1";
    this.label1.Size = new Size(378, 286);
    this.label1.TabIndex = 0;
    this.label1.Text = "На данном шаге будет произведен импорт метаданных.\r\nДля импорта метаданных в базу нажмите кнопку \"Далее\".";
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (StepControlMetadata);
    this.Size = new Size(384, 305);
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
