// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16685
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16685
{
  internal static string ssp_pdm_16686()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6];
      numArray2[2] = (byte) 33;
      numArray2[1] = (byte) 87;
      numArray2[3] = (byte) 59;
      numArray2[4] = (byte) 143;
      numArray2[0] = (byte) 216;
      numArray2[5] = (byte) 122;
      byte[] numArray3 = new byte[6]
      {
        (byte) 186,
        (byte) 8,
        (byte) 180,
        (byte) 14,
        (byte) 109,
        (byte) 167
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 16 /*0x10*/,
      (byte) 150,
      (byte) 135,
      (byte) 74,
      (byte) 122,
      (byte) 78
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 51,
      (byte) 18,
      (byte) 227,
      (byte) 168,
      (byte) 33,
      (byte) 226
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
