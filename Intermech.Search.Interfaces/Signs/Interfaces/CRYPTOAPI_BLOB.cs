// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CRYPTOAPI_BLOB
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>structure is used for an arbitrary array of bytes.</summary>
public struct CRYPTOAPI_BLOB
{
  /// <summary>
  /// The count of bytes in the buffer pointed to by pbData.
  /// </summary>
  public int cbData;
  /// <summary>A pointer to a block of data bytes.</summary>
  public IntPtr pbData;
}
