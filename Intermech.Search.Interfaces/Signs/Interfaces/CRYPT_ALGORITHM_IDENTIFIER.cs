// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CRYPT_ALGORITHM_IDENTIFIER
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Описание используемого алгоритма</summary>
public struct CRYPT_ALGORITHM_IDENTIFIER
{
  /// <summary>OID алгоритма</summary>
  [MarshalAs(UnmanagedType.LPStr)]
  public string pszObjId;
  /// <summary>Параметры алгоритма</summary>
  public CRYPTOAPI_BLOB Parameters;
}
