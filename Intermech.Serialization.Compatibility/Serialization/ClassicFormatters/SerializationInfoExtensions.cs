// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.SerializationInfoExtensions
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal static class SerializationInfoExtensions
{
  private static readonly Action<SerializationInfo, string, object, Type> s_updateValue = (Action<SerializationInfo, string, object, Type>) typeof (SerializationInfo).GetMethod("UpdateValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).CreateDelegate(typeof (Action<SerializationInfo, string, object, Type>));

  public static void UpdateValue(this SerializationInfo si, string name, object value, Type type)
  {
    SerializationInfoExtensions.s_updateValue(si, name, value, type);
  }
}
