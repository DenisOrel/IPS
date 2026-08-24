// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14789
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14789
{
  private static byte[] sspq = new byte[40]
  {
    (byte) 80 /*0x50*/,
    (byte) 132,
    (byte) 2,
    (byte) 206,
    (byte) 254,
    (byte) 196,
    (byte) 36,
    (byte) 107,
    (byte) 188,
    (byte) 143,
    (byte) 208 /*0xD0*/,
    (byte) 143,
    (byte) 28,
    (byte) 75,
    (byte) 173,
    (byte) 205,
    (byte) 225,
    (byte) 199,
    (byte) 83,
    (byte) 186,
    (byte) 143,
    (byte) 213,
    (byte) 114,
    (byte) 240 /*0xF0*/,
    (byte) 122,
    (byte) 93,
    (byte) 167,
    (byte) 65,
    (byte) 145,
    (byte) 11,
    (byte) 161,
    (byte) 102,
    (byte) 98,
    (byte) 201,
    (byte) 75,
    (byte) 42,
    (byte) 251,
    (byte) 64 /*0x40*/,
    (byte) 148,
    (byte) 162
  };
  private static byte[] sspr = new byte[40]
  {
    (byte) 221,
    (byte) 23,
    (byte) 58,
    (byte) 39,
    (byte) 98,
    (byte) 217,
    (byte) 137,
    (byte) 90,
    (byte) 70,
    (byte) 90,
    (byte) 104,
    (byte) 127 /*0x7F*/,
    (byte) 149,
    (byte) 29,
    (byte) 73,
    (byte) 163,
    (byte) 123,
    (byte) 41,
    (byte) 142,
    (byte) 117,
    (byte) 48 /*0x30*/,
    (byte) 79,
    (byte) 83,
    (byte) 69,
    (byte) 45,
    (byte) 126,
    (byte) 119,
    (byte) 97,
    (byte) 182,
    (byte) 13,
    (byte) 68,
    (byte) 183,
    (byte) 20,
    (byte) 36,
    (byte) 63 /*0x3F*/,
    (byte) 137,
    (byte) 169,
    (byte) 146,
    (byte) 82,
    (byte) 109
  };

  internal static string ssp_mrp_14790()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 12)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 0,
        (byte) 0,
        (byte) 68,
        (byte) 0,
        (byte) 0,
        (byte) 40
      };
      numArray2[1] = (byte) 17;
      numArray2[3] = (byte) 185;
      numArray2[4] = (byte) 104;
      numArray2[0] = (byte) 35;
      byte[] numArray3 = new byte[6]
      {
        (byte) 3,
        (byte) 32 /*0x20*/,
        (byte) 14,
        (byte) 185,
        (byte) 4,
        (byte) 133
      };
      key.Query(true, 347, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 85,
      (byte) 145,
      (byte) 210,
      (byte) 61,
      (byte) 0,
      (byte) 51
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 147,
      (byte) 205,
      (byte) 191,
      (byte) 11,
      (byte) 28,
      (byte) 166
    };
    key.Query(true, 347, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static int ssp_mrp_14791(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray1 = new byte[4];
    byte[] response1 = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 25,
      (byte) 182,
      (byte) 218,
      (byte) 154,
      (byte) 4,
      (byte) 19,
      (byte) 214,
      (byte) 176 /*0xB0*/,
      (byte) 73,
      (byte) 40,
      (byte) 231,
      (byte) 92,
      (byte) 202,
      (byte) 241,
      (byte) 90,
      (byte) 186,
      (byte) 17,
      (byte) 193,
      (byte) 87,
      (byte) 133,
      (byte) 103,
      (byte) 74,
      (byte) 183,
      (byte) 21,
      (byte) 118,
      (byte) 153,
      (byte) 87,
      (byte) 71,
      (byte) 224 /*0xE0*/,
      (byte) 186,
      (byte) 12,
      (byte) 156,
      (byte) 174,
      (byte) 253,
      (byte) 15,
      (byte) 147,
      (byte) 103,
      (byte) 86,
      (byte) 49,
      (byte) 130,
      (byte) 18,
      (byte) 119,
      (byte) 207,
      (byte) 89,
      (byte) 109,
      (byte) 52,
      (byte) 7,
      (byte) 76
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 223,
      (byte) 83,
      (byte) 198,
      (byte) 62,
      (byte) 60,
      (byte) 109,
      (byte) 189,
      (byte) 229,
      (byte) 125,
      (byte) 118,
      (byte) 33,
      (byte) 37,
      (byte) 7,
      (byte) 239,
      (byte) 60,
      (byte) 214,
      (byte) 139,
      (byte) 22,
      (byte) 60,
      (byte) 99,
      (byte) 179,
      (byte) 116,
      (byte) 50,
      (byte) 221,
      (byte) 236,
      (byte) 118,
      (byte) 156,
      (byte) 18,
      (byte) 190,
      (byte) 220,
      (byte) 192 /*0xC0*/,
      (byte) 148,
      (byte) 36,
      (byte) 181,
      (byte) 94,
      (byte) 26,
      (byte) 46,
      (byte) 137,
      (byte) 52,
      (byte) 195,
      (byte) 64 /*0x40*/,
      (byte) 20,
      (byte) 12,
      (byte) 206,
      (byte) 86,
      (byte) 35,
      (byte) 44,
      (byte) 33
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    key.Query(true, 347, numArray1, response1);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    byte[] numArray2 = new byte[40];
    byte[] response2 = new byte[40];
    Array.Copy((Array) sc_14789.sspq, 0, (Array) numArray2, 0, 40);
    key.Query(true, 347, numArray2, response2);
    Array.Copy((Array) sc_14789.sspr, 0, (Array) numArray2, 0, 40);
    for (int index = 0; index < numArray2.Length; ++index)
    {
      if ((int) numArray2[index] != (int) response2[index])
      {
        key.TagValue = (int) response2[index];
        break;
      }
    }
    return BitConverter.ToInt32(response1, 0) ^ BitConverter.ToInt32(numArray1, 0) ^ k;
  }
}
