// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.SerObjectInfoCache
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class SerObjectInfoCache
{
  internal readonly string _fullTypeName;
  internal readonly string _assemblyString;
  internal readonly bool _hasTypeForwardedFrom;
  internal MemberInfo[] _memberInfos;
  internal string[] _memberNames;
  internal Type[] _memberTypes;

  internal SerObjectInfoCache(string typeName, string assemblyName, bool hasTypeForwardedFrom)
  {
    this._fullTypeName = typeName;
    this._assemblyString = assemblyName;
    this._hasTypeForwardedFrom = hasTypeForwardedFrom;
  }

  internal SerObjectInfoCache(Type type)
  {
    TypeInformation typeInformation = BinaryFormatter.GetTypeInformation(type);
    this._fullTypeName = typeInformation.FullTypeName;
    this._assemblyString = typeInformation.AssemblyString;
    this._hasTypeForwardedFrom = typeInformation.HasTypeForwardedFrom;
  }
}
