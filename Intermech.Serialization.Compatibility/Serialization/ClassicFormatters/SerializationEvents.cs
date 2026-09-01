// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.SerializationEvents
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class SerializationEvents
{
  private readonly List<MethodInfo> _onSerializingMethods;
  private readonly List<MethodInfo> _onSerializedMethods;
  private readonly List<MethodInfo> _onDeserializingMethods;
  private readonly List<MethodInfo> _onDeserializedMethods;

  internal SerializationEvents(Type t)
  {
    this._onSerializingMethods = this.GetMethodsWithAttribute(typeof (OnSerializingAttribute), t);
    this._onSerializedMethods = this.GetMethodsWithAttribute(typeof (OnSerializedAttribute), t);
    this._onDeserializingMethods = this.GetMethodsWithAttribute(typeof (OnDeserializingAttribute), t);
    this._onDeserializedMethods = this.GetMethodsWithAttribute(typeof (OnDeserializedAttribute), t);
  }

  private List<MethodInfo> GetMethodsWithAttribute(Type attribute, Type t)
  {
    List<MethodInfo> methodsWithAttribute = (List<MethodInfo>) null;
    for (Type type = t; type != (Type) null && type != typeof (object); type = type.BaseType)
    {
      foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (method.IsDefined(attribute, false))
        {
          if (methodsWithAttribute == null)
            methodsWithAttribute = new List<MethodInfo>();
          methodsWithAttribute.Add(method);
        }
      }
    }
    methodsWithAttribute?.Reverse();
    return methodsWithAttribute;
  }

  internal bool HasOnSerializingEvents
  {
    get => this._onSerializingMethods != null || this._onSerializedMethods != null;
  }

  internal void InvokeOnSerializing(object obj, StreamingContext context)
  {
    SerializationEvents.InvokeOnDelegate(obj, context, this._onSerializingMethods);
  }

  internal void InvokeOnDeserializing(object obj, StreamingContext context)
  {
    SerializationEvents.InvokeOnDelegate(obj, context, this._onDeserializingMethods);
  }

  internal void InvokeOnDeserialized(object obj, StreamingContext context)
  {
    SerializationEvents.InvokeOnDelegate(obj, context, this._onDeserializedMethods);
  }

  internal SerializationEventHandler AddOnSerialized(object obj, SerializationEventHandler handler)
  {
    return SerializationEvents.AddOnDelegate(obj, handler, this._onSerializedMethods);
  }

  internal SerializationEventHandler AddOnDeserialized(
    object obj,
    SerializationEventHandler handler)
  {
    return SerializationEvents.AddOnDelegate(obj, handler, this._onDeserializedMethods);
  }

  private static void InvokeOnDelegate(
    object obj,
    StreamingContext context,
    List<MethodInfo> methods)
  {
    SerializationEventHandler serializationEventHandler = SerializationEvents.AddOnDelegate(obj, (SerializationEventHandler) null, methods);
    if (serializationEventHandler == null)
      return;
    serializationEventHandler(context);
  }

  private static SerializationEventHandler AddOnDelegate(
    object obj,
    SerializationEventHandler handler,
    List<MethodInfo> methods)
  {
    if (methods != null)
    {
      foreach (MethodInfo method in methods)
      {
        SerializationEventHandler serializationEventHandler = (SerializationEventHandler) method.CreateDelegate(typeof (SerializationEventHandler), obj);
        handler += serializationEventHandler;
      }
    }
    return handler;
  }
}
