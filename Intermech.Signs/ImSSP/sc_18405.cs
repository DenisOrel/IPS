// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18405
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18405
{
  private static byte[] sspq = new byte[46]
  {
    (byte) 14,
    (byte) 5,
    (byte) 60,
    (byte) 215,
    (byte) 211,
    (byte) 239,
    (byte) 166,
    (byte) 84,
    (byte) 93,
    (byte) 36,
    (byte) 146,
    (byte) 230,
    (byte) 37,
    (byte) 200,
    (byte) 162,
    (byte) 59,
    (byte) 80 /*0x50*/,
    (byte) 34,
    (byte) 35,
    (byte) 96 /*0x60*/,
    (byte) 144 /*0x90*/,
    (byte) 96 /*0x60*/,
    (byte) 250,
    (byte) 77,
    (byte) 32 /*0x20*/,
    (byte) 240 /*0xF0*/,
    (byte) 110,
    (byte) 201,
    (byte) 18,
    (byte) 214,
    (byte) 191,
    (byte) 38,
    (byte) 106,
    (byte) 91,
    (byte) 65,
    (byte) 30,
    (byte) 102,
    (byte) 88,
    (byte) 111,
    (byte) 123,
    (byte) 211,
    (byte) 199,
    (byte) 251,
    (byte) 157,
    (byte) 31 /*0x1F*/,
    (byte) 111
  };
  private static byte[] sspr = new byte[46]
  {
    (byte) 64 /*0x40*/,
    (byte) 67,
    (byte) 16 /*0x10*/,
    (byte) 166,
    (byte) 241,
    (byte) 4,
    (byte) 23,
    (byte) 221,
    (byte) 157,
    (byte) 73,
    (byte) 147,
    (byte) 116,
    (byte) 198,
    (byte) 30,
    (byte) 56,
    (byte) 97,
    (byte) 136,
    (byte) 128 /*0x80*/,
    (byte) 2,
    (byte) 142,
    (byte) 237,
    (byte) 99,
    (byte) 129,
    (byte) 1,
    (byte) 251,
    (byte) 187,
    (byte) 249,
    (byte) 218,
    (byte) 232,
    (byte) 52,
    (byte) 143,
    (byte) 193,
    (byte) 131,
    (byte) 78,
    (byte) 182,
    (byte) 91,
    (byte) 253,
    (byte) 206,
    (byte) 82,
    (byte) 103,
    (byte) 120,
    (byte) 225,
    (byte) 13,
    (byte) 71,
    (byte) 102,
    (byte) 13
  };

  internal static string ssp_signs_18406()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[0] = (byte) 99;
      numArray2[2] = (byte) 84;
      numArray2[1] = (byte) 162;
      numArray2[3] = (byte) 222;
      numArray2[4] = (byte) 226;
      numArray2[6] = (byte) 170;
      numArray2[5] = (byte) 68;
      byte[] numArray3 = new byte[7];
      numArray3[1] = (byte) 105;
      numArray3[3] = (byte) 0;
      numArray3[2] = (byte) 82;
      numArray3[0] = (byte) 92;
      numArray3[4] = (byte) 178;
      numArray3[5] = (byte) 5;
      numArray3[6] = (byte) 220;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[2] = (byte) 93;
    numArray5[1] = (byte) 80 /*0x50*/;
    numArray5[0] = (byte) 186;
    numArray5[5] = (byte) 93;
    numArray5[4] = (byte) 58;
    numArray5[3] = (byte) 132;
    numArray5[6] = (byte) 18;
    byte[] numArray6 = new byte[7];
    numArray6[1] = (byte) 116;
    numArray6[0] = (byte) 95;
    numArray6[5] = (byte) 14;
    numArray6[2] = (byte) 239;
    numArray6[4] = (byte) 98;
    numArray6[3] = (byte) 146;
    numArray6[6] = (byte) 200;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[34];
    byte[] response = new byte[34];
    Array.Copy((Array) sc_18405.sspq, 0, (Array) numArray7, 0, 34);
    key.Query(true, 353, numArray7, response);
    Array.Copy((Array) sc_18405.sspr, 0, (Array) numArray7, 0, 34);
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

  internal static string ssp_signs_18407()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[4] = (byte) 138;
      numArray2[1] = (byte) 214;
      numArray2[2] = (byte) 231;
      numArray2[3] = (byte) 2;
      numArray2[6] = (byte) 33;
      numArray2[5] = (byte) 122;
      numArray2[0] = (byte) 88;
      byte[] numArray3 = new byte[7]
      {
        (byte) 65,
        (byte) 55,
        (byte) 9,
        (byte) 90,
        (byte) 43,
        (byte) 220,
        (byte) 176 /*0xB0*/
      };
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[2] = (byte) 153;
    numArray5[1] = (byte) 146;
    numArray5[3] = (byte) 174;
    numArray5[5] = (byte) 239;
    numArray5[6] = (byte) 183;
    numArray5[0] = (byte) 6;
    numArray5[4] = (byte) 10;
    byte[] numArray6 = new byte[7];
    numArray6[5] = (byte) 242;
    numArray6[1] = (byte) 39;
    numArray6[2] = (byte) 47;
    numArray6[3] = (byte) 148;
    numArray6[6] = (byte) 225;
    numArray6[0] = (byte) 68;
    numArray6[4] = (byte) 29;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_signs_18408()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 31 /*0x1F*/,
        (byte) 209,
        (byte) 188,
        (byte) 130,
        (byte) 225,
        (byte) 120,
        (byte) 17
      };
      byte[] numArray3 = new byte[7]
      {
        (byte) 4,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 82,
        (byte) 0,
        (byte) 37
      };
      numArray3[3] = (byte) 147;
      numArray3[1] = (byte) 149;
      numArray3[5] = (byte) 168;
      numArray3[2] = (byte) 91;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7]
    {
      (byte) 9,
      (byte) 157,
      (byte) 223,
      (byte) 195,
      (byte) 118,
      (byte) 14,
      (byte) 231
    };
    byte[] numArray6 = new byte[7];
    numArray6[2] = (byte) 177;
    numArray6[5] = (byte) 102;
    numArray6[3] = (byte) 138;
    numArray6[1] = (byte) 65;
    numArray6[4] = (byte) 189;
    numArray6[6] = (byte) 151;
    numArray6[0] = (byte) 181;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_signs_18409()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8];
      numArray2[2] = (byte) 96 /*0x60*/;
      numArray2[1] = (byte) 250;
      numArray2[5] = (byte) 48 /*0x30*/;
      numArray2[3] = (byte) 159;
      numArray2[6] = (byte) 129;
      numArray2[4] = (byte) 100;
      numArray2[7] = (byte) 124;
      numArray2[0] = (byte) 29;
      byte[] numArray3 = new byte[8];
      numArray3[4] = (byte) 77;
      numArray3[1] = (byte) 125;
      numArray3[2] = (byte) 87;
      numArray3[3] = (byte) 42;
      numArray3[7] = (byte) 250;
      numArray3[0] = (byte) 160 /*0xA0*/;
      numArray3[6] = (byte) 66;
      numArray3[5] = (byte) 164;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[8];
    byte[] numArray5 = new byte[8];
    numArray5[5] = (byte) 228;
    numArray5[3] = (byte) 44;
    numArray5[1] = (byte) 48 /*0x30*/;
    numArray5[2] = (byte) 177;
    numArray5[4] = (byte) 151;
    numArray5[7] = (byte) 201;
    numArray5[6] = (byte) 97;
    numArray5[0] = (byte) 158;
    byte[] numArray6 = new byte[8]
    {
      (byte) 125,
      (byte) 245,
      (byte) 249,
      (byte) 179,
      (byte) 150,
      (byte) 33,
      (byte) 30,
      (byte) 189
    };
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_signs_18410()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8]
      {
        (byte) 29,
        (byte) 47,
        (byte) 181,
        (byte) 222,
        (byte) 100,
        (byte) 155,
        (byte) 70,
        (byte) 66
      };
      byte[] numArray3 = new byte[8]
      {
        (byte) 134,
        (byte) 189,
        (byte) 149,
        (byte) 139,
        (byte) 186,
        (byte) 92,
        (byte) 77,
        (byte) 71
      };
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[8];
    byte[] numArray5 = new byte[8]
    {
      (byte) 175,
      (byte) 243,
      (byte) 186,
      (byte) 227,
      (byte) 76,
      (byte) 154,
      (byte) 119,
      (byte) 112 /*0x70*/
    };
    byte[] numArray6 = new byte[8];
    numArray6[6] = (byte) 20;
    numArray6[5] = (byte) 180;
    numArray6[2] = (byte) 209;
    numArray6[3] = (byte) 151;
    numArray6[1] = (byte) 210;
    numArray6[4] = (byte) 171;
    numArray6[0] = (byte) 252;
    numArray6[7] = (byte) 83;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[12];
    byte[] response = new byte[12];
    Array.Copy((Array) sc_18405.sspq, 34, (Array) numArray7, 0, 12);
    key.Query(true, 353, numArray7, response);
    Array.Copy((Array) sc_18405.sspr, 34, (Array) numArray7, 0, 12);
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

  internal static string ssp_signs_18411()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 11)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8]
      {
        (byte) 60,
        (byte) 187,
        (byte) 69,
        (byte) 245,
        (byte) 94,
        (byte) 173,
        (byte) 38,
        (byte) 203
      };
      byte[] numArray3 = new byte[8]
      {
        (byte) 134,
        (byte) 114,
        (byte) 60,
        (byte) 252,
        (byte) 215,
        (byte) 0,
        (byte) 0,
        (byte) 193
      };
      numArray3[6] = (byte) 48 /*0x30*/;
      numArray3[5] = (byte) 55;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[8];
    byte[] numArray5 = new byte[8]
    {
      (byte) 89,
      (byte) 148,
      (byte) 64 /*0x40*/,
      (byte) 144 /*0x90*/,
      (byte) 140,
      (byte) 97,
      (byte) 115,
      (byte) 208 /*0xD0*/
    };
    byte[] numArray6 = new byte[8];
    numArray6[5] = (byte) 19;
    numArray6[3] = (byte) 236;
    numArray6[2] = (byte) 159;
    numArray6[4] = (byte) 11;
    numArray6[0] = (byte) 2;
    numArray6[1] = (byte) 149;
    numArray6[6] = (byte) 103;
    numArray6[7] = (byte) 169;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_signs_18412()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 12)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8]
      {
        (byte) 1,
        (byte) 218,
        (byte) 197,
        (byte) 22,
        (byte) 137,
        (byte) 246,
        (byte) 238,
        (byte) 66
      };
      byte[] numArray3 = new byte[8];
      numArray3[0] = (byte) 209;
      numArray3[4] = (byte) 129;
      numArray3[2] = (byte) 47;
      numArray3[3] = (byte) 183;
      numArray3[1] = (byte) 49;
      numArray3[6] = (byte) 89;
      numArray3[5] = (byte) 127 /*0x7F*/;
      numArray3[7] = (byte) 231;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[8];
    byte[] numArray5 = new byte[8]
    {
      (byte) 147,
      (byte) 234,
      (byte) 220,
      (byte) 95,
      (byte) 27,
      (byte) 181,
      (byte) 248,
      (byte) 243
    };
    byte[] numArray6 = new byte[8];
    numArray6[6] = (byte) 184;
    numArray6[0] = (byte) 165;
    numArray6[5] = (byte) 0;
    numArray6[3] = (byte) 225;
    numArray6[2] = (byte) 197;
    numArray6[4] = (byte) 129;
    numArray6[1] = (byte) 134;
    numArray6[7] = (byte) 125;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
