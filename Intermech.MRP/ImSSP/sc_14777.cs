// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14777
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14777
{
  internal static string ssp_mrp_14778()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 205,
        (byte) 1,
        (byte) 52,
        (byte) 124,
        (byte) 42,
        (byte) 0
      };
      byte[] numArray3 = new byte[6];
      numArray3[3] = (byte) 75;
      numArray3[2] = (byte) 181;
      numArray3[0] = (byte) 76;
      numArray3[4] = (byte) 26;
      numArray3[1] = (byte) 20;
      numArray3[5] = (byte) 94;
      key.Query(true, 347, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 241,
      (byte) 145,
      (byte) 170,
      (byte) 155,
      (byte) 35,
      (byte) 217
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 199,
      (byte) 118,
      (byte) 43,
      (byte) 106,
      (byte) 224 /*0xE0*/,
      (byte) 120
    };
    key.Query(true, 347, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
