// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.OpenKey
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// Класс для хранения открытого ключа
/// (на самом деле, кроме самого открытого ключа,
/// хранит ещё доп. информацию)
/// </summary>
[DefaultProperty("Key")]
public class OpenKey
{
  /// <summary>тип ключа</summary>
  private OpenKeyType _keyType = OpenKeyType.Extended;
  /// <summary>guid провайлера</summary>
  private Guid _providerGuid = Guid.Empty;
  /// <summary>
  /// имя контейнера,
  /// в котором хранятся ключи
  /// </summary>
  private string _conteinerName = "Intermech_Keys_Container";
  /// <summary>открытый ключ</summary>
  private string _key = string.Empty;

  /// <summary>Конструктор</summary>
  /// <param name="Key">Ключ в формате, зависящим от тип ключа </param>
  public OpenKey(string Key)
  {
    string[] strArray = Key.Split(':');
    this._providerGuid = strArray.Length == 2 || strArray.Length == 3 ? new Guid(strArray[strArray.Length - 2]) : throw new Exception(LocalizationHolder.rm.GetString("Search.Interfaces_10"));
    this._key = strArray[strArray.Length - 1];
    this._keyType = strArray.Length == 3 ? OpenKeyType.Extended : OpenKeyType.Simple;
    if (strArray.Length != 3)
      return;
    this._conteinerName = strArray[0];
  }

  /// <summary>
  /// Конструктор
  /// undone - удалить!!!
  /// </summary>
  /// <param name="providerGuid">Guid криптопровайдера</param>
  /// <param name="key">Открытый ключ</param>
  public OpenKey(Guid providerGuid, string key)
  {
    this._keyType = OpenKeyType.Simple;
    this._providerGuid = providerGuid;
    this._key = key;
    this._conteinerName = "Intermech_Keys_Container";
  }

  /// <summary>Конструктор</summary>
  /// <param name="providerGuid">Guid криптопровайдера</param>
  /// <param name="key">Открытый ключ</param>
  /// <param name="conteinerName">имя контейнера</param>
  public OpenKey(Guid providerGuid, string key, string conteinerName)
  {
    this._providerGuid = providerGuid;
    this._key = key;
    this._conteinerName = conteinerName;
  }

  /// <summary>Guid провайдера</summary>
  [Browsable(false)]
  public Guid ProviderGuid => this._providerGuid;

  /// <summary>Ключ</summary>
  public string Key => this._key;

  /// <summary>
  /// имя контейнера,
  /// в котором хранятся ключи
  /// </summary>
  public string ConteinerName => this._conteinerName;

  /// <summary>тип ключа</summary>
  public OpenKeyType KeyType
  {
    get => this._keyType;
    set => this._keyType = value;
  }

  /// <summary>Преобразование в строку</summary>
  /// <returns>Ключ в формате "guid_провайдера:ключ"</returns>
  public override string ToString()
  {
    string str = $"{this._providerGuid}:{this._key}";
    return this.KeyType != OpenKeyType.Simple ? $"{this._conteinerName}:{str}" : str;
  }
}
