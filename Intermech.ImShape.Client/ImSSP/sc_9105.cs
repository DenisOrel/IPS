// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_9105
// Assembly: Intermech.ImShape.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EAEE73DE-1C1F-4401-8BB6-D181BFA32870
// Assembly location: D:\IPS\Client\Intermech.ImShape.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_9105
{
  internal static string ssp_imbase_9106()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[2];
      byte[] numArray2 = new byte[2]
      {
        (byte) 235,
        (byte) 47
      };
      byte[] numArray3 = new byte[2]
      {
        (byte) 138,
        (byte) 192 /*0xC0*/
      };
      key.Query(true, 343, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 2);
      for (int index = 0; index < 2; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[2];
    byte[] numArray5 = new byte[2]
    {
      (byte) 224 /*0xE0*/,
      (byte) 5
    };
    byte[] numArray6 = new byte[2]{ (byte) 159, (byte) 10 };
    key.Query(true, 343, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 2);
    for (int index = 0; index < 2; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
