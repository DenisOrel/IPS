// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PluginItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.Interface;

[DefaultProperty("Load")]
[DisplayName("Модуль расширения")]
[System.ComponentModel.Description("Загружаемый модуль расширения")]
[Editor(typeof (PluginItemUIEditor), typeof (UITypeEditor))]
public sealed class PluginItem : ICloneable
{
  private string _fileName;
  private string _description;
  private bool _enable;

  public PluginItem()
  {
    this._fileName = string.Empty;
    this._description = string.Empty;
    this._enable = false;
  }

  public PluginItem(string fileName, string description, bool enable)
  {
    this._fileName = fileName;
    this._description = description;
    this._enable = enable;
  }

  [DisplayName("Имя файла")]
  [System.ComponentModel.Description("Относительный путь и имя файла загружаемого модуля расширения")]
  public string FileName
  {
    get => this._fileName;
    set => this._fileName = value;
  }

  [DisplayName("Описание")]
  [System.ComponentModel.Description("Описание загружаемого модуля расширения")]
  public string Description
  {
    get => this._description;
    set => this._description = value;
  }

  [DisplayName("Флаг загрузки")]
  [System.ComponentModel.Description("Флаг, показывающий загрузку модуля приложением")]
  public bool Enable
  {
    get => this._enable;
    set => this._enable = value;
  }

  public PluginItem Clone() => new PluginItem(this._fileName, this._description, this._enable);

  object ICloneable.Clone() => (object) this.Clone();

  public override bool Equals(object obj)
  {
    if (!(obj is PluginItem pluginItem))
      return base.Equals(obj);
    return pluginItem._description == this._description && pluginItem._fileName == this._fileName && pluginItem._enable == this._enable;
  }

  public override int GetHashCode()
  {
    return this._description.GetHashCode() ^ this._fileName.GetHashCode() ^ this._enable.GetHashCode();
  }
}
