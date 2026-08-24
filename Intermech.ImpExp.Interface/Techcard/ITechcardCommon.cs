// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.Techcard.ITechcardCommon
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.Techcard;

/// <summary>
/// Интерфейс для получения доступа к
/// объектам и спискам закаченных данных Techcard
/// </summary>
public interface ITechcardCommon
{
  /// <summary>
  /// Список правил перекачки понятий
  /// Key   - Код понятия
  /// Value - Guid атрибута, в который будет качаться понятие
  /// (несколько понятий разного типа могут качатся в один и тот же атрибут)
  /// </summary>
  Dictionary<string, Guid> Code2AttributeGuid { get; set; }
}
