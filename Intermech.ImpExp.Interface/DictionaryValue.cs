// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DictionaryValue
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Запись элемента кэша</summary>
[Serializable]
public class DictionaryValue
{
  /// <summary>Идентификатор в новой базе</summary>
  public long NewObjectID;
  /// <summary>Заголовок объекта</summary>
  public string Caption;
  /// <summary>Доп. данные (используем по личному разумению)</summary>
  public ITagImportObject Tag;

  /// <summary>Конструктор</summary>
  /// <param name="newObjectID">Идентификатор в новой базе</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <param name="tag">Доп. данные (используем по личному разумению)</param>
  public DictionaryValue(long newObjectID, string caption, ITagImportObject tag)
  {
    this.NewObjectID = newObjectID;
    this.Caption = caption;
    this.Tag = tag;
  }
}
