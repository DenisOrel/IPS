// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16987
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16987
{
  private static byte[] sspq = new byte[39]
  {
    (byte) 54,
    (byte) 67,
    (byte) 204,
    (byte) 206,
    (byte) 39,
    (byte) 138,
    (byte) 85,
    (byte) 134,
    (byte) 254,
    (byte) 38,
    (byte) 133,
    (byte) 149,
    (byte) 238,
    (byte) 204,
    (byte) 126,
    (byte) 249,
    (byte) 154,
    (byte) 92,
    (byte) 239,
    (byte) 37,
    (byte) 30,
    (byte) 140,
    (byte) 37,
    (byte) 184,
    (byte) 193,
    (byte) 40,
    (byte) 78,
    (byte) 253,
    (byte) 76,
    (byte) 195,
    (byte) 138,
    (byte) 96 /*0x60*/,
    (byte) 145,
    (byte) 102,
    (byte) 205,
    (byte) 189,
    (byte) 61,
    (byte) 111,
    (byte) 127 /*0x7F*/
  };
  private static byte[] sspr = new byte[39]
  {
    (byte) 21,
    (byte) 221,
    (byte) 59,
    (byte) 37,
    (byte) 177,
    (byte) 122,
    (byte) 232,
    (byte) 197,
    (byte) 100,
    (byte) 244,
    (byte) 245,
    (byte) 227,
    (byte) 129,
    (byte) 199,
    (byte) 156,
    (byte) 66,
    (byte) 149,
    (byte) 36,
    (byte) 102,
    (byte) 54,
    (byte) 123,
    (byte) 200,
    (byte) 48 /*0x30*/,
    (byte) 140,
    (byte) 119,
    (byte) 209,
    (byte) 241,
    (byte) 169,
    (byte) 93,
    (byte) 137,
    (byte) 44,
    (byte) 135,
    (byte) 229,
    (byte) 156,
    (byte) 34,
    (byte) 49,
    (byte) 119,
    (byte) 55,
    (byte) 247
  };

  internal static string ssp_pdm_16988()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6];
      numArray2[2] = (byte) 149;
      numArray2[0] = (byte) 233;
      numArray2[1] = (byte) 160 /*0xA0*/;
      numArray2[3] = (byte) 165;
      numArray2[5] = (byte) 5;
      numArray2[4] = (byte) 236;
      byte[] numArray3 = new byte[6]
      {
        (byte) 74,
        (byte) 238,
        (byte) 27,
        (byte) 19,
        (byte) 132,
        (byte) 122
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
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 50,
      (byte) 0,
      (byte) 241
    };
    numArray5[2] = (byte) 42;
    numArray5[1] = (byte) 23;
    numArray5[4] = (byte) 29;
    numArray5[0] = (byte) 18;
    byte[] numArray6 = new byte[6]
    {
      (byte) 97,
      (byte) 238,
      (byte) 205,
      (byte) 17,
      (byte) 242,
      (byte) 112 /*0x70*/
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[39];
    byte[] response = new byte[39];
    Array.Copy((Array) sc_16987.sspq, 0, (Array) numArray7, 0, 39);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16987.sspr, 0, (Array) numArray7, 0, 39);
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
