// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16981
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16981
{
  internal static string ssp_pdm_16982()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 233,
        (byte) 244,
        (byte) 134,
        (byte) 118,
        (byte) 184,
        (byte) 200
      };
      byte[] numArray3 = new byte[6]
      {
        (byte) 0,
        (byte) 245,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 177
      };
      numArray3[3] = (byte) 108;
      numArray3[2] = (byte) 140;
      numArray3[4] = (byte) 182;
      numArray3[0] = (byte) 54;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 127 /*0x7F*/,
      (byte) 3,
      (byte) 215,
      (byte) 3,
      (byte) 2,
      (byte) 61
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 251,
      (byte) 175,
      (byte) 129,
      (byte) 141,
      (byte) 186,
      (byte) 62
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
