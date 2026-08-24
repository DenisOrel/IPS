// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.IItemToCreate
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

public interface IItemToCreate : ICustomTypeDescriptor
{
  /// <summary>Локальный идентификатор</summary>
  int LocalID { get; }

  /// <summary>Наименование</summary>
  string Name { get; set; }

  /// <summary>Новый тип атрибута</summary>
  bool IsNew { get; }

  /// <summary>Глобальный идентификатор</summary>
  Guid GUID { get; }

  /// <summary>Системный идентификатор</summary>
  long SystemId { get; }
}
