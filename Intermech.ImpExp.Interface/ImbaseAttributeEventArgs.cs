// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImbaseAttributeEventArgs
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces.Client;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Атрибут Imbase, с которыми произошло некоторое событие
/// </summary>
public class ImbaseAttributeEventArgs : NotificationEventArgs
{
  /// <summary>Имя поля в БД</summary>
  private string _attributeField;
  /// <summary>Таблица Imbase, которой принадлежит атрибут</summary>
  private string _tableName;

  /// <summary>
  /// Подготовить список идентификаторов атрибутов, с которыми произошло некоторое событие
  /// </summary>
  /// <param name="eventName">Наименование события</param>
  /// <param name="field">Имя поля в БД</param>
  /// <param name="tableName">tableName</param>
  public ImbaseAttributeEventArgs(string eventName, string field, string tableName)
    : base(eventName)
  {
    this._attributeField = field;
    this._tableName = tableName;
  }

  /// <summary>Имя поля в БД</summary>
  public string Field => this._attributeField;

  /// <summary>Таблица Imbase, которой принадлежит атрибут</summary>
  public string TableName => this._tableName;
}
