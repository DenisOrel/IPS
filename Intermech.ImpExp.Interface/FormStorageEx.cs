// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.FormStorageEx
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces.Client;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Класс загрузки/сохранения информации в раздел "FormStorage"
/// </summary>
public class FormStorageEx
{
  private SortedDictionary<string, string> _dict = new SortedDictionary<string, string>();
  private string _name;

  /// <summary>Конструктор</summary>
  /// <param name="name">имя секции</param>
  public FormStorageEx(string name) => this._name = name;

  /// <summary>Ашчэ конструктор</summary>
  /// <param name="data">Control для сохранения (формируется только имя)</param>
  public FormStorageEx(Control data) => this._name = $"Form_{data.Name}";

  /// <summary>Добавление дополнительного атрибута</summary>
  /// <param name="name">имя атрибута</param>
  /// <param name="value">значение</param>
  public void AddAttribute(string name, string value) => this._dict[name] = value;

  /// <summary>Проверка на наличие атрибута</summary>
  /// <param name="name">имя атрибута</param>
  /// <returns>true если есть</returns>
  public bool HasAttribute(string name) => this._dict.ContainsKey(name);

  /// <summary>Получение значения атрибута по имени</summary>
  /// <param name="name">имя атрибута</param>
  /// <returns></returns>
  public string GetAttribute(string name) => this._dict[name];

  /// <summary>Убрать атрибут из списка</summary>
  /// <param name="name">имя атрибута</param>
  public void RemoveAttribute(string name) => this._dict.Remove(name);

  /// <summary>Очистить весь список атрибутов</summary>
  public void Clear() => this._dict.Clear();

  /// <summary>Загрузить данные в класс</summary>
  public void Load()
  {
    this._dict.Clear();
    IConfiguration configuration = (ServicesManager.GetService(typeof (IConfiguration)) as IConfiguration).Open($"FormStorage\\{this._name}");
    if (configuration == null)
      return;
    foreach (XmlAttribute attribute in (XmlNamedNodeMap) configuration.Node.Attributes)
      this._dict[attribute.Name] = attribute.Value;
  }

  /// <summary>
  /// Загрузить данные для контрола
  /// (устанавливаются атрибуты "Позиция" и "Размер")
  /// </summary>
  /// <param name="data">Control для установки</param>
  public void Load(Control data)
  {
    this.Load();
    if (this._dict.ContainsKey("Location"))
      data.Location = (Point) new PointConverter().ConvertFromInvariantString(this._dict["Location"]);
    if (!this._dict.ContainsKey("Size"))
      return;
    data.Size = (Size) new SizeConverter().ConvertFromInvariantString(this._dict["Size"]);
  }

  /// <summary>Сохранение атрибутов в xml</summary>
  public void Save()
  {
    IConfiguration configuration = (ServicesManager.GetService(typeof (IConfiguration)) as IConfiguration).Open($"FormStorage\\{this._name}", true);
    if (configuration == null)
      return;
    foreach (KeyValuePair<string, string> keyValuePair in this._dict)
      configuration.SetAttribute(keyValuePair.Key, keyValuePair.Value);
  }

  /// <summary>
  /// Сохранение атрибутов в xml
  /// (устанавливаются атрибуты "Позиция" и "Размер")
  /// </summary>
  /// <param name="data">Control для установки</param>
  public void Save(Control data)
  {
    this._dict["Location"] = new PointConverter().ConvertToInvariantString((object) data.Location);
    this._dict["Size"] = new SizeConverter().ConvertToInvariantString((object) data.Size);
    this.Save();
  }

  /// <summary>
  /// Загрузка данных и установка атрибутов "Позиция" и "Размер"
  /// </summary>
  /// <param name="data">Control для установки</param>
  public static void LoadSettings(Control data)
  {
    IConfiguration configuration = (ServicesManager.GetService(typeof (IConfiguration)) as IConfiguration).Open($"FormStorage\\Form_{data.Name}");
    if (configuration == null)
      return;
    if (configuration.HasAttribute("Location"))
      data.Location = (Point) new PointConverter().ConvertFromInvariantString(configuration.GetAttribute("Location"));
    if (!configuration.HasAttribute("Size"))
      return;
    data.Size = (Size) new SizeConverter().ConvertFromInvariantString(configuration.GetAttribute("Size"));
  }

  /// <summary>
  /// Сохранение данных и установка атрибутов "Позиция" и "Размер"
  /// </summary>
  /// <param name="data">Control для установки</param>
  public static void SaveSettings(Control data)
  {
    IConfiguration configuration = (ServicesManager.GetService(typeof (IConfiguration)) as IConfiguration).Open($"FormStorage\\Form_{data.Name}", true);
    if (configuration == null)
      return;
    configuration.SetAttribute("Location", new PointConverter().ConvertToInvariantString((object) data.Location));
    configuration.SetAttribute("Size", new SizeConverter().ConvertToInvariantString((object) data.Size));
  }

  public static void AddAttribute(Control data, string name, string value)
  {
    (ServicesManager.GetService(typeof (IConfiguration)) as IConfiguration).Open($"FormStorage\\Form_{data.Name}", true)?.SetAttribute(name, value);
  }

  public static string GetAttribute(Control data, string name)
  {
    IConfiguration configuration = (ServicesManager.GetService(typeof (IConfiguration)) as IConfiguration).Open($"FormStorage\\Form_{data.Name}");
    return configuration != null && configuration.HasAttribute(name) ? configuration.GetAttribute(name) : string.Empty;
  }
}
