// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImbaseAttributeBindingEventArgs
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Привязка атрибута Imbase к атрибуту в новой системе</summary>
public class ImbaseAttributeBindingEventArgs : ImbaseAttributeEventArgs
{
  /// <summary>GUID атрибута к которому привязывается атрибут Imbase</summary>
  private Guid _bindAttrGuid;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="field">Имя поля в БД</param>
  /// <param name="tableName">tableName</param>
  /// <param name="bindAttributeGUID">GUID атрибута к которому привязывается атрибут Imbase</param>
  public ImbaseAttributeBindingEventArgs(string field, string tableName, Guid bindAttributeGUID)
    : base("AttributeBindingChanged", field, tableName)
  {
    this._bindAttrGuid = bindAttributeGUID;
  }

  /// <summary>GUID атрибута к которому привязывается атрибут Imbase</summary>
  public Guid BindAttrGuid => this._bindAttrGuid;
}
