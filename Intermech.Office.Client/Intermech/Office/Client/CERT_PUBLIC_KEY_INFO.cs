// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CERT_PUBLIC_KEY_INFO
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Signs.Interfaces;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Office.Client;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct CERT_PUBLIC_KEY_INFO
{
  internal CRYPT_ALGORITHM_IDENTIFIER Algorithm;
  internal CRYPT_BIT_BLOB PublicKey;
}
