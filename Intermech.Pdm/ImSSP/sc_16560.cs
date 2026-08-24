// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16560
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16560
{
  internal static string ssp_pdm_16561()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[5] = (byte) 88;
      numArray2[2] = (byte) 27;
      numArray2[3] = (byte) 175;
      numArray2[1] = (byte) 206;
      numArray2[4] = (byte) 171;
      numArray2[0] = (byte) 64 /*0x40*/;
      numArray2[6] = (byte) 90;
      byte[] numArray3 = new byte[7];
      numArray3[5] = (byte) 110;
      numArray3[1] = (byte) 234;
      numArray3[2] = (byte) 46;
      numArray3[6] = (byte) 60;
      numArray3[3] = (byte) 174;
      numArray3[4] = (byte) 164;
      numArray3[0] = byte.MaxValue;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7]
    {
      (byte) 186,
      (byte) 113,
      (byte) 78,
      (byte) 63 /*0x3F*/,
      (byte) 30,
      (byte) 140,
      (byte) 88
    };
    byte[] numArray6 = new byte[7]
    {
      (byte) 211,
      (byte) 251,
      (byte) 150,
      (byte) 251,
      (byte) 116,
      (byte) 113,
      (byte) 164
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
