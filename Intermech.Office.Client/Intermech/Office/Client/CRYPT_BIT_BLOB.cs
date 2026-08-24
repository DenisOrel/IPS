// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CRYPT_BIT_BLOB
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Office.Client;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct CRYPT_BIT_BLOB
{
  internal uint cbData;
  internal IntPtr pbData;
  internal uint cUnusedBits;
}
