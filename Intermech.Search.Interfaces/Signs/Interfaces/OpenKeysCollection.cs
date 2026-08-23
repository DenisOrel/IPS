// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.OpenKeysCollection
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.Collections;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Класс для хранения списка открытых ключей</summary>
public class OpenKeysCollection : ArrayList
{
  /// <summary>Конструктор по умолчанию</summary>
  public OpenKeysCollection()
  {
  }

  /// <summary>Конструктор - создает список из массива ключей</summary>
  /// <param name="keys">Открытые ключи</param>
  public OpenKeysCollection(OpenKey[] keys) => this.AddRange((ICollection) keys);

  /// <summary>Массив значений</summary>
  public OpenKey[] Values => this.ToArray(typeof (OpenKey)) as OpenKey[];
}
