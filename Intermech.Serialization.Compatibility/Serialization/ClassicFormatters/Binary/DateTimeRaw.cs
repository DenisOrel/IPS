// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.DateTimeRaw
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

[StructLayout(LayoutKind.Explicit)]
internal struct DateTimeRaw
{
  [FieldOffset(0)]
  public long LongValue;
  [FieldOffset(0)]
  public DateTime DateTimeValue;
}
