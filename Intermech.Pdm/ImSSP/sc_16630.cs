// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16630
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16630
{
  internal static string ssp_pdm_16631()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 0,
        (byte) 229,
        (byte) 0,
        (byte) 0,
        (byte) 125,
        (byte) 0
      };
      numArray2[2] = (byte) 67;
      numArray2[3] = (byte) 88;
      numArray2[0] = (byte) 139;
      numArray2[5] = (byte) 221;
      byte[] numArray3 = new byte[6]
      {
        (byte) 144 /*0x90*/,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 82
      };
      numArray3[2] = (byte) 75;
      numArray3[1] = (byte) 91;
      numArray3[4] = (byte) 48 /*0x30*/;
      numArray3[3] = (byte) 38;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 65,
      (byte) 106,
      (byte) 245,
      (byte) 57,
      (byte) 9,
      (byte) 27
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 146,
      (byte) 30,
      (byte) 0,
      (byte) 31 /*0x1F*/,
      (byte) 33,
      (byte) 0
    };
    numArray6[2] = (byte) 152;
    numArray6[5] = (byte) 23;
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
