// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.Compatibility.Binary.BinaryFormatterCompatibilityBinder
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.Compatibility.Binary;

public class BinaryFormatterCompatibilityBinder : SerializationBinder
{
  private readonly StTypeInfoService typeInfoService;

  public BinaryFormatterCompatibilityBinder()
    : this(StTypeInfoService.Default)
  {
  }

  public BinaryFormatterCompatibilityBinder(StTypeInfoService typeInfoService)
  {
    this.typeInfoService = typeInfoService != null ? typeInfoService : throw new ArgumentNullException(nameof (typeInfoService));
  }

  public override void BindToName(
    Type serializedType,
    out string assemblyName,
    out string typeName)
  {
    typeName = (string) null;
    assemblyName = (string) null;
  }

  public override Type BindToType(string assemblyName, string typeName)
  {
    switch (typeName)
    {
      case "System.UnitySerializationHolder":
        assemblyName = "Intermech.Serialization.Compatibility";
        typeName = "Intermech.UnityHolder";
        break;
      case "System.DelegateSerializationHolder":
      case "System.Reflection.MemberInfoSerializationHolder":
        assemblyName = "Intermech.Serialization.Compatibility";
        typeName = "Intermech.Serialization.Compatibility.Binary.NullHolder";
        break;
      case "System.DelegateSerializationHolder+DelegateEntry":
        assemblyName = "Intermech.Serialization.Compatibility";
        typeName = "Intermech.Serialization.Compatibility.Binary.DelegateEntry";
        break;
      case "Intermech.Serialization.Compatibility.Binary.DelegateHolder":
        assemblyName = "Intermech.Serialization.Compatibility";
        typeName = "Intermech.Serialization.Compatibility.Binary.NullHolder";
        break;
    }
    return this.typeInfoService.GetType(this.typeInfoService.GetTypeInfo(typeName, assemblyName));
  }
}
