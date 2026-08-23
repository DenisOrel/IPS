// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.ExpiredAuthFileUsing
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Использование просроченных аутентичных файлов</summary>
public enum ExpiredAuthFileUsing
{
  None,
  /// <summary>Добавлять в УЛ всегда</summary>
  YesForAll,
  /// <summary>Не добавлять в УЛ никогда</summary>
  NoForAll,
}
