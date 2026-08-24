// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16701
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16701
{
  private static byte[] sspq = new byte[15]
  {
    (byte) 221,
    (byte) 120,
    (byte) 57,
    (byte) 158,
    (byte) 125,
    (byte) 136,
    (byte) 112 /*0x70*/,
    (byte) 216,
    (byte) 12,
    (byte) 203,
    (byte) 61,
    (byte) 7,
    (byte) 162,
    (byte) 112 /*0x70*/,
    (byte) 79
  };
  private static byte[] sspr = new byte[15]
  {
    (byte) 213,
    (byte) 136,
    (byte) 254,
    (byte) 127 /*0x7F*/,
    (byte) 22,
    (byte) 141,
    (byte) 225,
    (byte) 194,
    (byte) 66,
    (byte) 196,
    (byte) 243,
    (byte) 73,
    (byte) 170,
    (byte) 202,
    (byte) 246
  };

  internal static string ssp_pdm_16702()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 98,
        (byte) 206,
        (byte) 63 /*0x3F*/,
        (byte) 249,
        (byte) 200,
        (byte) 32 /*0x20*/
      };
      byte[] numArray3 = new byte[6];
      numArray3[4] = (byte) 106;
      numArray3[0] = (byte) 238;
      numArray3[2] = (byte) 31 /*0x1F*/;
      numArray3[3] = (byte) 111;
      numArray3[5] = (byte) 1;
      numArray3[1] = (byte) 211;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 0,
      (byte) 0,
      (byte) 208 /*0xD0*/,
      (byte) 0,
      (byte) 0,
      (byte) 186
    };
    numArray5[1] = (byte) 220;
    numArray5[3] = (byte) 169;
    numArray5[4] = (byte) 14;
    numArray5[0] = (byte) 135;
    byte[] numArray6 = new byte[6]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 94,
      (byte) 22
    };
    numArray6[2] = (byte) 4;
    numArray6[1] = (byte) 241;
    numArray6[0] = (byte) 158;
    numArray6[3] = (byte) 2;
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[15];
    byte[] response = new byte[15];
    Array.Copy((Array) sc_16701.sspq, 0, (Array) numArray7, 0, 15);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16701.sspr, 0, (Array) numArray7, 0, 15);
    for (int index = 0; index < numArray7.Length; ++index)
    {
      if ((int) numArray7[index] != (int) response[index])
      {
        key.TagValue = (int) response[index];
        break;
      }
    }
    return Encoding.UTF8.GetString(numArray4);
  }
}
