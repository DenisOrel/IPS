// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15101
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15101
{
  private static byte[] sspq = new byte[26]
  {
    (byte) 125,
    (byte) 212,
    byte.MaxValue,
    (byte) 229,
    (byte) 173,
    (byte) 112 /*0x70*/,
    (byte) 102,
    (byte) 187,
    (byte) 1,
    (byte) 169,
    (byte) 157,
    (byte) 127 /*0x7F*/,
    (byte) 156,
    (byte) 113,
    (byte) 103,
    (byte) 36,
    (byte) 254,
    (byte) 249,
    (byte) 244,
    (byte) 159,
    (byte) 187,
    (byte) 223,
    (byte) 91,
    (byte) 73,
    (byte) 70,
    (byte) 23
  };
  private static byte[] sspr = new byte[26]
  {
    (byte) 43,
    (byte) 123,
    (byte) 155,
    (byte) 5,
    (byte) 68,
    (byte) 16 /*0x10*/,
    (byte) 27,
    (byte) 200,
    (byte) 245,
    (byte) 10,
    (byte) 81,
    (byte) 190,
    (byte) 188,
    (byte) 61,
    (byte) 90,
    (byte) 46,
    (byte) 233,
    (byte) 236,
    (byte) 52,
    (byte) 171,
    (byte) 16 /*0x10*/,
    (byte) 194,
    (byte) 94,
    (byte) 214,
    (byte) 223,
    (byte) 155
  };

  internal static string ssp_office_15102()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[7] = (byte) 148;
      numArray2[10] = (byte) 247;
      numArray2[2] = (byte) 58;
      numArray2[11] = (byte) 0;
      numArray2[0] = (byte) 134;
      numArray2[6] = (byte) 165;
      numArray2[13] = (byte) 97;
      numArray2[4] = (byte) 44;
      numArray2[8] = (byte) 192 /*0xC0*/;
      numArray2[3] = (byte) 69;
      numArray2[5] = (byte) 151;
      numArray2[9] = (byte) 97;
      numArray2[1] = (byte) 24;
      numArray2[12] = (byte) 47;
      numArray2[14] = (byte) 173;
      numArray2[15] = (byte) 89;
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[15] = (byte) 213;
      numArray3[2] = (byte) 225;
      numArray3[8] = (byte) 110;
      numArray3[3] = (byte) 5;
      numArray3[12] = (byte) 50;
      numArray3[5] = (byte) 227;
      numArray3[11] = (byte) 103;
      numArray3[0] = (byte) 37;
      numArray3[1] = (byte) 251;
      numArray3[9] = (byte) 218;
      numArray3[10] = (byte) 125;
      numArray3[4] = (byte) 252;
      numArray3[7] = (byte) 7;
      numArray3[13] = (byte) 204;
      numArray3[14] = (byte) 110;
      numArray3[6] = (byte) 189;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[5] = (byte) 83;
    numArray5[9] = (byte) 91;
    numArray5[2] = (byte) 124;
    numArray5[3] = (byte) 163;
    numArray5[10] = (byte) 112 /*0x70*/;
    numArray5[6] = (byte) 106;
    numArray5[15] = (byte) 51;
    numArray5[7] = (byte) 31 /*0x1F*/;
    numArray5[14] = (byte) 245;
    numArray5[4] = (byte) 33;
    numArray5[12] = (byte) 26;
    numArray5[11] = (byte) 3;
    numArray5[8] = (byte) 181;
    numArray5[13] = (byte) 167;
    numArray5[1] = (byte) 190;
    numArray5[0] = (byte) 213;
    byte[] numArray6 = new byte[16 /*0x10*/];
    numArray6[2] = (byte) 51;
    numArray6[0] = (byte) 1;
    numArray6[5] = (byte) 43;
    numArray6[1] = (byte) 34;
    numArray6[15] = (byte) 216;
    numArray6[10] = (byte) 139;
    numArray6[6] = (byte) 60;
    numArray6[7] = (byte) 141;
    numArray6[8] = (byte) 95;
    numArray6[9] = (byte) 39;
    numArray6[4] = (byte) 182;
    numArray6[11] = (byte) 14;
    numArray6[12] = (byte) 190;
    numArray6[14] = (byte) 93;
    numArray6[3] = (byte) 53;
    numArray6[13] = (byte) 55;
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15103()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 2)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[2] = (byte) 80 /*0x50*/;
      numArray2[1] = (byte) 148;
      numArray2[4] = (byte) 199;
      numArray2[10] = (byte) 109;
      numArray2[11] = (byte) 163;
      numArray2[9] = (byte) 26;
      numArray2[6] = (byte) 137;
      numArray2[7] = (byte) 36;
      numArray2[0] = (byte) 198;
      numArray2[13] = (byte) 121;
      numArray2[5] = (byte) 14;
      numArray2[15] = (byte) 180;
      numArray2[12] = (byte) 213;
      numArray2[8] = (byte) 51;
      numArray2[14] = (byte) 5;
      numArray2[3] = (byte) 101;
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[10] = (byte) 137;
      numArray3[3] = (byte) 61;
      numArray3[2] = (byte) 161;
      numArray3[12] = (byte) 16 /*0x10*/;
      numArray3[15] = (byte) 5;
      numArray3[14] = (byte) 253;
      numArray3[6] = (byte) 224 /*0xE0*/;
      numArray3[8] = (byte) 13;
      numArray3[4] = (byte) 129;
      numArray3[13] = (byte) 11;
      numArray3[7] = (byte) 41;
      numArray3[11] = (byte) 154;
      numArray3[9] = (byte) 24;
      numArray3[1] = (byte) 214;
      numArray3[0] = (byte) 188;
      numArray3[5] = (byte) 98;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[6] = (byte) 103;
    numArray5[1] = (byte) 92;
    numArray5[10] = (byte) 16 /*0x10*/;
    numArray5[15] = (byte) 4;
    numArray5[4] = (byte) 48 /*0x30*/;
    numArray5[5] = (byte) 131;
    numArray5[7] = (byte) 14;
    numArray5[14] = (byte) 252;
    numArray5[12] = (byte) 16 /*0x10*/;
    numArray5[9] = (byte) 134;
    numArray5[0] = (byte) 215;
    numArray5[11] = (byte) 115;
    numArray5[2] = (byte) 32 /*0x20*/;
    numArray5[3] = (byte) 250;
    numArray5[13] = (byte) 84;
    numArray5[8] = (byte) 21;
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 45,
      (byte) 87,
      (byte) 117,
      (byte) 94,
      (byte) 197,
      (byte) 86,
      (byte) 118,
      (byte) 229,
      (byte) 102,
      (byte) 189,
      (byte) 0,
      (byte) 142,
      (byte) 43,
      (byte) 254,
      (byte) 26,
      (byte) 63 /*0x3F*/
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[26];
    byte[] response = new byte[26];
    Array.Copy((Array) sc_15101.sspq, 0, (Array) numArray7, 0, 26);
    key.Query(true, 349, numArray7, response);
    Array.Copy((Array) sc_15101.sspr, 0, (Array) numArray7, 0, 26);
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

  internal static string ssp_office_15104()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 11)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 208 /*0xD0*/,
        (byte) 109,
        (byte) 51,
        (byte) 92,
        (byte) 39,
        (byte) 101,
        (byte) 231,
        (byte) 135,
        (byte) 50,
        (byte) 73,
        (byte) 183,
        (byte) 242,
        (byte) 212,
        (byte) 209,
        (byte) 230,
        (byte) 63 /*0x3F*/
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 160 /*0xA0*/,
        (byte) 179,
        (byte) 87,
        (byte) 221,
        (byte) 201,
        (byte) 42,
        (byte) 29,
        (byte) 186,
        (byte) 125,
        (byte) 241,
        (byte) 113,
        (byte) 125,
        (byte) 236,
        (byte) 42,
        (byte) 136,
        (byte) 245
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[8] = (byte) 89;
    numArray5[1] = (byte) 110;
    numArray5[15] = (byte) 149;
    numArray5[3] = (byte) 244;
    numArray5[12] = (byte) 158;
    numArray5[0] = (byte) 180;
    numArray5[2] = (byte) 221;
    numArray5[7] = (byte) 79;
    numArray5[6] = (byte) 245;
    numArray5[9] = (byte) 157;
    numArray5[10] = (byte) 120;
    numArray5[11] = (byte) 181;
    numArray5[5] = (byte) 29;
    numArray5[13] = (byte) 150;
    numArray5[14] = (byte) 66;
    numArray5[4] = (byte) 222;
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 177,
      (byte) 144 /*0x90*/,
      (byte) 166,
      (byte) 36,
      (byte) 102,
      (byte) 111,
      (byte) 168,
      (byte) 157,
      (byte) 57,
      (byte) 164,
      (byte) 186,
      (byte) 73,
      (byte) 119,
      (byte) 140,
      (byte) 147,
      (byte) 22
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
