// Decompiled with JetBrains decompiler
// Type: Intermech.UnityHolder
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using Intermech.Serialization;
using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech;

[Serializable]
internal sealed class UnityHolder : ISerializable, IObjectReference
{
  private readonly int unityType;
  private readonly string data;
  private readonly string assemblyName;
  private const int NullUnity = 2;
  private const int MissingUnity = 3;
  private const int RuntimeTypeUnity = 4;
  private const string UnitiTypeField = "UnityType";
  private const string DataField = "Data";
  private const string AssemblyNameField = "AssemblyName";

  internal UnityHolder(SerializationInfo info, StreamingContext context)
  {
    this.unityType = info.GetInt32("UnityType");
    this.data = info.GetString("Data");
    this.assemblyName = info.GetString("AssemblyName");
  }

  void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
  {
    throw new NotSupportedException();
  }

  object IObjectReference.GetRealObject(StreamingContext context)
  {
    switch (this.unityType)
    {
      case 2:
        return (object) DBNull.Value;
      case 3:
        return (object) Missing.Value;
      case 4:
        if (this.data == null || this.data.Length == 0)
          throw this.CreateInsufficientInformationException("Data");
        if (this.assemblyName == null)
          throw this.CreateInsufficientInformationException("AssemblyName");
        StTypeInfoService stTypeInfoService = StTypeInfoService.Default;
        return (object) stTypeInfoService.GetType(stTypeInfoService.GetTypeInfo(this.data, this.assemblyName));
      default:
        throw new ArgumentException($"Invalid unity type {this.unityType}");
    }
  }

  private SerializationException CreateInsufficientInformationException(string fieldName)
  {
    return new SerializationException($"Insufficient deserialization state in field {fieldName}.");
  }

  internal static void GetUnitySerializationInfo(SerializationInfo info, DBNull dbNullValue)
  {
    info.SetType(typeof (UnityHolder));
    info.AddValue("UnityType", 2);
    info.AddValue("Data", (object) null, typeof (string));
    info.AddValue("AssemblyName", (object) null, typeof (string));
  }

  internal static void GetUnitySerializationInfo(SerializationInfo info, Missing missingValue)
  {
    info.SetType(typeof (UnityHolder));
    info.AddValue("UnityType", 3);
    info.AddValue("Data", (object) null, typeof (string));
    info.AddValue("AssemblyName", (object) null, typeof (string));
  }

  internal static void GetUnitySerializationInfo(SerializationInfo info, Type runtimeType)
  {
    StTypeInfo typeInfo = StTypeInfoService.Default.GetTypeInfo(runtimeType);
    info.SetType(typeof (UnityHolder));
    info.AddValue("UnityType", 4);
    info.AddValue("Data", (object) typeInfo.TypeName, typeof (string));
    info.AddValue("AssemblyName", (object) typeInfo.AssemblyName, typeof (string));
  }
}
