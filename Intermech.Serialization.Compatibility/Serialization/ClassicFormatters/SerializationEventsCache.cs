// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.SerializationEventsCache
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Concurrent;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal static class SerializationEventsCache
{
  private static readonly ConcurrentDictionary<Type, SerializationEvents> s_cache = new ConcurrentDictionary<Type, SerializationEvents>();

  internal static SerializationEvents GetSerializationEventsForType(Type t)
  {
    return SerializationEventsCache.s_cache.GetOrAdd(t, (Func<Type, SerializationEvents>) (type => SerializationEventsCache.CreateSerializationEvents(type)));
  }

  private static SerializationEvents CreateSerializationEvents(Type t)
  {
    return new SerializationEvents(t);
  }
}
