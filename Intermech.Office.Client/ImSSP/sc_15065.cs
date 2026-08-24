// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15065
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15065
{
  private static byte[] sspq = new byte[54]
  {
    (byte) 169,
    (byte) 179,
    (byte) 110,
    (byte) 65,
    (byte) 163,
    (byte) 84,
    (byte) 95,
    (byte) 205,
    (byte) 29,
    (byte) 29,
    (byte) 100,
    (byte) 13,
    (byte) 26,
    (byte) 81,
    (byte) 248,
    (byte) 97,
    (byte) 249,
    (byte) 107,
    (byte) 144 /*0x90*/,
    (byte) 66,
    (byte) 166,
    (byte) 114,
    (byte) 162,
    (byte) 211,
    (byte) 61,
    (byte) 115,
    (byte) 128 /*0x80*/,
    (byte) 216,
    (byte) 155,
    (byte) 234,
    (byte) 226,
    (byte) 171,
    (byte) 68,
    (byte) 80 /*0x50*/,
    (byte) 150,
    (byte) 23,
    (byte) 111,
    (byte) 98,
    (byte) 134,
    (byte) 155,
    (byte) 164,
    (byte) 55,
    (byte) 226,
    (byte) 19,
    (byte) 249,
    (byte) 178,
    (byte) 90,
    (byte) 101,
    (byte) 169,
    (byte) 246,
    (byte) 147,
    (byte) 37,
    (byte) 204,
    (byte) 238
  };
  private static byte[] sspr = new byte[54]
  {
    (byte) 78,
    (byte) 80 /*0x50*/,
    (byte) 84,
    (byte) 202,
    (byte) 136,
    (byte) 192 /*0xC0*/,
    (byte) 221,
    (byte) 32 /*0x20*/,
    (byte) 198,
    (byte) 45,
    (byte) 215,
    (byte) 82,
    (byte) 76,
    (byte) 253,
    (byte) 60,
    (byte) 146,
    byte.MaxValue,
    (byte) 43,
    (byte) 48 /*0x30*/,
    (byte) 17,
    (byte) 216,
    (byte) 173,
    (byte) 18,
    (byte) 251,
    (byte) 167,
    (byte) 211,
    (byte) 179,
    (byte) 167,
    (byte) 61,
    (byte) 234,
    (byte) 118,
    (byte) 174,
    (byte) 135,
    (byte) 41,
    (byte) 135,
    (byte) 37,
    (byte) 148,
    (byte) 137,
    (byte) 42,
    (byte) 84,
    (byte) 91,
    (byte) 174,
    (byte) 23,
    (byte) 235,
    (byte) 117,
    (byte) 190,
    (byte) 142,
    (byte) 249,
    (byte) 1,
    (byte) 129,
    (byte) 16 /*0x10*/,
    (byte) 32 /*0x20*/,
    (byte) 242,
    (byte) 19
  };

  internal static string ssp_office_15066()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[12] = (byte) 38;
      numArray2[1] = (byte) 222;
      numArray2[8] = (byte) 72;
      numArray2[3] = (byte) 148;
      numArray2[14] = (byte) 120;
      numArray2[7] = (byte) 104;
      numArray2[6] = (byte) 66;
      numArray2[13] = (byte) 220;
      numArray2[5] = (byte) 202;
      numArray2[4] = (byte) 193;
      numArray2[10] = (byte) 150;
      numArray2[11] = (byte) 42;
      numArray2[15] = (byte) 53;
      numArray2[9] = (byte) 100;
      numArray2[0] = (byte) 122;
      numArray2[2] = (byte) 198;
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 126,
        (byte) 57,
        (byte) 34,
        (byte) 66,
        (byte) 153,
        (byte) 156,
        (byte) 38,
        (byte) 105,
        (byte) 76,
        (byte) 52,
        (byte) 21,
        (byte) 108,
        (byte) 78,
        (byte) 154,
        (byte) 229,
        (byte) 154
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/]
    {
      (byte) 71,
      (byte) 172,
      (byte) 114,
      (byte) 211,
      (byte) 49,
      (byte) 253,
      (byte) 232,
      (byte) 55,
      (byte) 122,
      (byte) 243,
      (byte) 118,
      (byte) 116,
      (byte) 8,
      (byte) 117,
      (byte) 217,
      (byte) 63 /*0x3F*/
    };
    byte[] numArray6 = new byte[16 /*0x10*/];
    numArray6[10] = (byte) 237;
    numArray6[9] = (byte) 111;
    numArray6[2] = (byte) 254;
    numArray6[14] = (byte) 53;
    numArray6[8] = (byte) 41;
    numArray6[5] = (byte) 38;
    numArray6[0] = (byte) 201;
    numArray6[7] = (byte) 81;
    numArray6[15] = (byte) 101;
    numArray6[11] = (byte) 69;
    numArray6[4] = (byte) 233;
    numArray6[1] = (byte) 155;
    numArray6[6] = (byte) 36;
    numArray6[13] = (byte) 133;
    numArray6[12] = (byte) 100;
    numArray6[3] = (byte) 226;
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[54];
    byte[] response = new byte[54];
    Array.Copy((Array) sc_15065.sspq, 0, (Array) numArray7, 0, 54);
    key.Query(true, 349, numArray7, response);
    Array.Copy((Array) sc_15065.sspr, 0, (Array) numArray7, 0, 54);
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
