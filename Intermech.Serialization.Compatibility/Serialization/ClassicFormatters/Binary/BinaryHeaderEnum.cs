// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryHeaderEnum
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal enum BinaryHeaderEnum
{
  SerializedStreamHeader,
  Object,
  ObjectWithMap,
  ObjectWithMapAssemId,
  ObjectWithMapTyped,
  ObjectWithMapTypedAssemId,
  ObjectString,
  Array,
  MemberPrimitiveTyped,
  MemberReference,
  ObjectNull,
  MessageEnd,
  Assembly,
  ObjectNullMultiple256,
  ObjectNullMultiple,
  ArraySinglePrimitive,
  ArraySingleObject,
  ArraySingleString,
  CrossAppDomainMap,
  CrossAppDomainString,
  CrossAppDomainAssembly,
  MethodCall,
  MethodReturn,
}
