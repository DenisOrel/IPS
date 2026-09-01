// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.SerObjectInfoInit
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class SerObjectInfoInit
{
  internal readonly Dictionary<Type, SerObjectInfoCache> _seenBeforeTable = new Dictionary<Type, SerObjectInfoCache>();
  internal int _objectInfoIdCount = 1;
  internal SerStack _oiPool = new SerStack("SerObjectInfo Pool");
}
