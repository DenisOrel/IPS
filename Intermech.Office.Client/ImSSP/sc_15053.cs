// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15053
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15053
{
  private static byte[] sspq = new byte[22]
  {
    (byte) 229,
    (byte) 164,
    (byte) 108,
    (byte) 46,
    (byte) 70,
    (byte) 88,
    (byte) 171,
    (byte) 247,
    (byte) 16 /*0x10*/,
    (byte) 97,
    (byte) 13,
    (byte) 230,
    (byte) 188,
    (byte) 64 /*0x40*/,
    (byte) 43,
    (byte) 194,
    (byte) 21,
    (byte) 166,
    (byte) 206,
    (byte) 218,
    (byte) 178,
    (byte) 228
  };
  private static byte[] sspr = new byte[22]
  {
    (byte) 160 /*0xA0*/,
    (byte) 224 /*0xE0*/,
    (byte) 34,
    (byte) 75,
    (byte) 140,
    (byte) 195,
    (byte) 133,
    (byte) 72,
    (byte) 124,
    (byte) 159,
    (byte) 69,
    (byte) 164,
    (byte) 187,
    (byte) 245,
    (byte) 108,
    (byte) 240 /*0xF0*/,
    (byte) 203,
    (byte) 146,
    (byte) 105,
    (byte) 134,
    (byte) 7,
    (byte) 225
  };

  internal static string ssp_office_15054()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[22];
      byte[] numArray2 = new byte[22]
      {
        (byte) 78,
        byte.MaxValue,
        (byte) 243,
        (byte) 92,
        (byte) 92,
        (byte) 198,
        (byte) 150,
        (byte) 161,
        (byte) 233,
        (byte) 7,
        (byte) 12,
        (byte) 14,
        (byte) 30,
        (byte) 25,
        (byte) 241,
        (byte) 228,
        (byte) 176 /*0xB0*/,
        (byte) 65,
        (byte) 137,
        (byte) 88,
        (byte) 143,
        (byte) 83
      };
      byte[] numArray3 = new byte[22]
      {
        (byte) 238,
        (byte) 162,
        (byte) 238,
        (byte) 102,
        (byte) 33,
        (byte) 26,
        (byte) 192 /*0xC0*/,
        (byte) 74,
        (byte) 69,
        (byte) 94,
        (byte) 51,
        (byte) 13,
        (byte) 5,
        (byte) 191,
        (byte) 163,
        (byte) 165,
        (byte) 100,
        (byte) 71,
        (byte) 178,
        (byte) 222,
        (byte) 211,
        (byte) 213
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 22);
      for (int index = 0; index < 22; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[22];
    byte[] numArray5 = new byte[22];
    numArray5[9] = (byte) 15;
    numArray5[1] = (byte) 7;
    numArray5[2] = (byte) 19;
    numArray5[6] = (byte) 4;
    numArray5[3] = (byte) 210;
    numArray5[16 /*0x10*/] = (byte) 35;
    numArray5[19] = (byte) 175;
    numArray5[7] = (byte) 245;
    numArray5[8] = (byte) 187;
    numArray5[5] = (byte) 245;
    numArray5[10] = (byte) 207;
    numArray5[17] = (byte) 37;
    numArray5[12] = (byte) 20;
    numArray5[0] = (byte) 54;
    numArray5[14] = (byte) 163;
    numArray5[15] = (byte) 179;
    numArray5[4] = (byte) 125;
    numArray5[11] = (byte) 186;
    numArray5[18] = (byte) 144 /*0x90*/;
    numArray5[13] = (byte) 217;
    numArray5[20] = (byte) 118;
    numArray5[21] = (byte) 62;
    byte[] numArray6 = new byte[22]
    {
      (byte) 242,
      (byte) 191,
      (byte) 71,
      (byte) 161,
      (byte) 46,
      (byte) 97,
      (byte) 60,
      (byte) 118,
      (byte) 127 /*0x7F*/,
      (byte) 107,
      (byte) 191,
      (byte) 54,
      (byte) 17,
      (byte) 174,
      (byte) 249,
      (byte) 122,
      (byte) 178,
      (byte) 212,
      (byte) 193,
      (byte) 73,
      (byte) 61,
      (byte) 229
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 22);
    for (int index = 0; index < 22; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[22];
    byte[] response = new byte[22];
    Array.Copy((Array) sc_15053.sspq, 0, (Array) numArray7, 0, 22);
    key.Query(true, 349, numArray7, response);
    Array.Copy((Array) sc_15053.sspr, 0, (Array) numArray7, 0, 22);
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
