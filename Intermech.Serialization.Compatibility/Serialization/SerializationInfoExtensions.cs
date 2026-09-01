// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.SerializationInfoExtensions
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization;

public static class SerializationInfoExtensions
{
  private static readonly Func<SerializationInfo, string, Type, object> reflectionGetValueNoThrow = SerializationInfoExtensions.CreateReflectionGetValueNoThrow();
  private static readonly Action<SerializationInfo, string, object, Type> reflectionUpdateValue = SerializationInfoExtensions.CreateReflectionUpdateValue();

  public static object GetValueOrDefault(
    this SerializationInfo info,
    string name,
    Type type,
    object defaultValue = null)
  {
    if (info == null)
      throw new ArgumentNullException(nameof (info));
    return SerializationInfoExtensions.reflectionGetValueNoThrow(info, name, type) ?? defaultValue;
  }

  public static TValue GetValueOrDefault<TValue>(
    this SerializationInfo info,
    string name,
    TValue defaultValue = null)
  {
    if (info == null)
      throw new ArgumentNullException(nameof (info));
    object obj = SerializationInfoExtensions.reflectionGetValueNoThrow(info, name, typeof (TValue));
    return obj == null ? defaultValue : (TValue) obj;
  }

  public static void AddOrUpdateValue(
    this SerializationInfo info,
    string name,
    object value,
    Type type = null)
  {
    if (info == null)
      throw new ArgumentNullException(nameof (info));
    if (type == (Type) null)
      type = value != null ? value.GetType() : typeof (object);
    SerializationInfoExtensions.reflectionUpdateValue(info, name, value, type);
  }

  private static Func<SerializationInfo, string, Type, object> CreateReflectionGetValueNoThrow()
  {
    MethodInfo method = typeof (SerializationInfo).GetMethod("GetValueNoThrow", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    return ((Expression<Func<SerializationInfo, string, Type, object>>) ((target, name, type) => Expression.Call(target, method, name, type))).Compile();
  }

  private static Action<SerializationInfo, string, object, Type> CreateReflectionUpdateValue()
  {
    MethodInfo method = typeof (SerializationInfo).GetMethod("UpdateValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    return ((Expression<Action<SerializationInfo, string, object, Type>>) ((target, name, value, type) => Expression.Call(target, method, name, value, type))).Compile();
  }
}
