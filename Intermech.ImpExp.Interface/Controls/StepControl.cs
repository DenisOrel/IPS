// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.Controls.StepControl
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface.Controls;

/// <summary>
/// Базовый класс для реализации контролов, которые будуд использоваться для установок
/// </summary>
public class StepControl : UserControl, IStepControl
{
  /// <summary>Лог файл</summary>
  protected ILogFile logFile;
  /// <summary>Контрол участвует в процессе дозакачки</summary>
  protected bool stepRepumpble;
  /// <summary>
  /// Разрешено ли возвращаться на предыдущий шаг
  /// Исходя из этого значения, решается также вопрос об отложенном сохранении
  /// настроек шага, т.е. вызов функции SaveSettings();
  /// если true - значит отложенное, и наоборот ...
  /// </summary>
  protected bool stepPrevAllowed = true;
  /// <summary>Владелец контрола</summary>
  protected object owner;
  private Image _image;
  /// <summary>Required designer variable.</summary>
  private IContainer components;

  /// <summary>Контрол участвует в процессе дозакачки</summary>
  public bool StepRepumpble
  {
    get => this.stepRepumpble;
    set => this.stepRepumpble = value;
  }

  public ILogFile LogFile
  {
    set => this.logFile = value;
  }

  /// <summary>Разрешено ли возвращаться на предыдущий шаг</summary>
  public bool StepPrevAllowed
  {
    get => this.stepPrevAllowed;
    set => this.stepPrevAllowed = value;
  }

  public virtual bool isMetadataSettingsStep => true;

  /// <summary>Конструктор</summary>
  protected StepControl()
  {
    this.CreateHandle();
    this.InitializeComponent();
  }

  /// <summary>Конструктор</summary>
  /// <param name="owner"></param>
  public StepControl(object owner)
    : this()
  {
    this.owner = owner;
  }

  public string Caption => this.getCaption();

  public Image Image => this.getImage();

  protected virtual string getCaption() => "Базовый класс для реализации шагов закачки";

  protected virtual Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgEmpty")];
    return this._image;
  }

  /// <summary>Виртуальный метод для обновления шага</summary>
  public virtual void RefreshControl()
  {
  }

  /// <summary>
  /// Виртуальный метод для сохранения данных на шаге настроек
  /// </summary>
  /// <returns></returns>
  public virtual SaveSettingsResult SaveSettings() => SaveSettingsResult.ssrOk;

  public virtual void Cancel()
  {
  }

  public virtual bool LeaveControl() => true;

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
    this.components = (IContainer) new System.ComponentModel.Container();
    this.AutoScaleMode = AutoScaleMode.Font;
  }
}
