// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17682
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_17682
{
  private static byte[] sspq = new byte[44]
  {
    (byte) 22,
    (byte) 235,
    (byte) 156,
    (byte) 73,
    (byte) 219,
    (byte) 7,
    (byte) 59,
    (byte) 216,
    (byte) 46,
    (byte) 230,
    (byte) 51,
    (byte) 18,
    (byte) 75,
    (byte) 212,
    (byte) 69,
    (byte) 67,
    (byte) 205,
    (byte) 202,
    (byte) 178,
    (byte) 71,
    (byte) 94,
    (byte) 12,
    (byte) 56,
    (byte) 67,
    (byte) 111,
    (byte) 189,
    (byte) 179,
    (byte) 47,
    (byte) 205,
    (byte) 221,
    (byte) 64 /*0x40*/,
    (byte) 157,
    (byte) 9,
    (byte) 136,
    (byte) 82,
    (byte) 245,
    (byte) 213,
    (byte) 64 /*0x40*/,
    (byte) 232,
    (byte) 189,
    (byte) 79,
    (byte) 59,
    (byte) 17,
    (byte) 113
  };
  private static byte[] sspr = new byte[44]
  {
    (byte) 239,
    (byte) 31 /*0x1F*/,
    (byte) 40,
    (byte) 133,
    (byte) 137,
    (byte) 27,
    (byte) 142,
    (byte) 55,
    (byte) 16 /*0x10*/,
    (byte) 25,
    (byte) 227,
    (byte) 66,
    (byte) 138,
    (byte) 156,
    (byte) 78,
    (byte) 119,
    (byte) 159,
    (byte) 114,
    (byte) 253,
    (byte) 60,
    (byte) 31 /*0x1F*/,
    (byte) 152,
    (byte) 237,
    (byte) 18,
    (byte) 120,
    (byte) 220,
    (byte) 155,
    (byte) 87,
    (byte) 251,
    (byte) 14,
    (byte) 82,
    (byte) 13,
    (byte) 143,
    (byte) 201,
    (byte) 47,
    (byte) 180,
    (byte) 212,
    (byte) 204,
    (byte) 174,
    (byte) 68,
    (byte) 68,
    (byte) 112 /*0x70*/,
    (byte) 248,
    (byte) 190
  };

  internal static string ssp_imclient_17683()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10];
      numArray2[8] = (byte) 152;
      numArray2[2] = (byte) 137;
      numArray2[1] = (byte) 131;
      numArray2[9] = (byte) 159;
      numArray2[4] = (byte) 190;
      numArray2[5] = (byte) 126;
      numArray2[0] = (byte) 126;
      numArray2[7] = (byte) 231;
      numArray2[6] = (byte) 168;
      numArray2[3] = (byte) 216;
      byte[] numArray3 = new byte[10];
      numArray3[1] = (byte) 29;
      numArray3[7] = (byte) 111;
      numArray3[2] = (byte) 134;
      numArray3[3] = (byte) 70;
      numArray3[0] = (byte) 156;
      numArray3[5] = (byte) 217;
      numArray3[9] = (byte) 123;
      numArray3[6] = (byte) 91;
      numArray3[8] = (byte) 85;
      numArray3[4] = (byte) 144 /*0x90*/;
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10];
    numArray5[4] = (byte) 207;
    numArray5[3] = (byte) 85;
    numArray5[1] = (byte) 230;
    numArray5[2] = (byte) 229;
    numArray5[0] = (byte) 122;
    numArray5[5] = (byte) 170;
    numArray5[6] = (byte) 244;
    numArray5[7] = (byte) 181;
    numArray5[8] = (byte) 199;
    numArray5[9] = (byte) 242;
    byte[] numArray6 = new byte[10]
    {
      (byte) 33,
      (byte) 131,
      (byte) 87,
      (byte) 5,
      (byte) 219,
      (byte) 229,
      (byte) 113,
      (byte) 214,
      (byte) 147,
      (byte) 52
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17684()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 4)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 187,
        (byte) 28,
        (byte) 248,
        (byte) 23,
        (byte) 111,
        (byte) 245,
        (byte) 229,
        (byte) 209,
        (byte) 158,
        (byte) 121
      };
      byte[] numArray3 = new byte[10]
      {
        (byte) 184,
        (byte) 201,
        (byte) 44,
        (byte) 105,
        (byte) 83,
        (byte) 180,
        (byte) 19,
        (byte) 230,
        (byte) 134,
        (byte) 230
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10]
    {
      (byte) 17,
      (byte) 222,
      (byte) 198,
      (byte) 198,
      (byte) 244,
      (byte) 52,
      (byte) 27,
      (byte) 207,
      (byte) 180,
      (byte) 134
    };
    byte[] numArray6 = new byte[10]
    {
      (byte) 130,
      (byte) 171,
      (byte) 124,
      (byte) 206,
      (byte) 16 /*0x10*/,
      (byte) 137,
      (byte) 162,
      (byte) 1,
      (byte) 165,
      (byte) 10
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17685()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10];
      numArray2[8] = (byte) 178;
      numArray2[6] = (byte) 178;
      numArray2[9] = (byte) 178;
      numArray2[3] = (byte) 154;
      numArray2[4] = (byte) 14;
      numArray2[5] = (byte) 117;
      numArray2[2] = (byte) 109;
      numArray2[7] = (byte) 153;
      numArray2[1] = (byte) 190;
      numArray2[0] = (byte) 195;
      byte[] numArray3 = new byte[10];
      numArray3[6] = (byte) 222;
      numArray3[7] = (byte) 185;
      numArray3[2] = (byte) 123;
      numArray3[0] = (byte) 184;
      numArray3[4] = (byte) 139;
      numArray3[5] = (byte) 40;
      numArray3[1] = (byte) 187;
      numArray3[9] = (byte) 4;
      numArray3[8] = (byte) 250;
      numArray3[3] = (byte) 222;
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10];
    numArray5[2] = (byte) 18;
    numArray5[0] = (byte) 187;
    numArray5[9] = (byte) 43;
    numArray5[3] = (byte) 49;
    numArray5[1] = (byte) 33;
    numArray5[4] = (byte) 223;
    numArray5[6] = (byte) 19;
    numArray5[7] = (byte) 178;
    numArray5[8] = (byte) 120;
    numArray5[5] = (byte) 3;
    byte[] numArray6 = new byte[10];
    numArray6[2] = (byte) 68;
    numArray6[5] = (byte) 179;
    numArray6[0] = (byte) 176 /*0xB0*/;
    numArray6[3] = (byte) 30;
    numArray6[9] = (byte) 133;
    numArray6[4] = (byte) 234;
    numArray6[6] = (byte) 14;
    numArray6[7] = (byte) 0;
    numArray6[1] = (byte) 66;
    numArray6[8] = (byte) 212;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[20];
    byte[] response = new byte[20];
    Array.Copy((Array) sc_17682.sspq, 0, (Array) numArray7, 0, 20);
    key.Query(true, 348, numArray7, response);
    Array.Copy((Array) sc_17682.sspr, 0, (Array) numArray7, 0, 20);
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

  internal static string ssp_imclient_17686()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10];
      numArray2[0] = (byte) 39;
      numArray2[8] = (byte) 229;
      numArray2[1] = (byte) 111;
      numArray2[6] = (byte) 241;
      numArray2[4] = (byte) 62;
      numArray2[2] = (byte) 156;
      numArray2[3] = (byte) 192 /*0xC0*/;
      numArray2[7] = (byte) 100;
      numArray2[5] = (byte) 45;
      numArray2[9] = (byte) 14;
      byte[] numArray3 = new byte[10];
      numArray3[1] = (byte) 37;
      numArray3[9] = (byte) 12;
      numArray3[8] = (byte) 231;
      numArray3[4] = (byte) 72;
      numArray3[6] = (byte) 140;
      numArray3[0] = (byte) 19;
      numArray3[2] = (byte) 223;
      numArray3[7] = (byte) 164;
      numArray3[3] = (byte) 147;
      numArray3[5] = (byte) 234;
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10]
    {
      (byte) 178,
      (byte) 239,
      (byte) 124,
      byte.MaxValue,
      (byte) 136,
      (byte) 157,
      (byte) 218,
      (byte) 73,
      (byte) 116,
      (byte) 72
    };
    byte[] numArray6 = new byte[10];
    numArray6[2] = (byte) 146;
    numArray6[7] = (byte) 184;
    numArray6[8] = (byte) 5;
    numArray6[1] = (byte) 90;
    numArray6[4] = (byte) 50;
    numArray6[0] = (byte) 60;
    numArray6[3] = (byte) 77;
    numArray6[6] = (byte) 82;
    numArray6[5] = (byte) 65;
    numArray6[9] = (byte) 77;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17687()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 11)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10];
      numArray2[0] = (byte) 89;
      numArray2[9] = (byte) 135;
      numArray2[8] = (byte) 1;
      numArray2[2] = (byte) 232;
      numArray2[4] = (byte) 22;
      numArray2[5] = (byte) 88;
      numArray2[6] = (byte) 32 /*0x20*/;
      numArray2[7] = (byte) 187;
      numArray2[3] = (byte) 42;
      numArray2[1] = (byte) 98;
      byte[] numArray3 = new byte[10]
      {
        (byte) 72,
        (byte) 182,
        (byte) 150,
        (byte) 207,
        (byte) 75,
        (byte) 146,
        (byte) 92,
        (byte) 45,
        (byte) 73,
        (byte) 211
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10];
    numArray5[0] = (byte) 154;
    numArray5[1] = (byte) 122;
    numArray5[3] = (byte) 76;
    numArray5[2] = (byte) 184;
    numArray5[4] = (byte) 231;
    numArray5[5] = (byte) 120;
    numArray5[6] = (byte) 146;
    numArray5[7] = (byte) 201;
    numArray5[8] = (byte) 42;
    numArray5[9] = (byte) 45;
    byte[] numArray6 = new byte[10]
    {
      (byte) 234,
      (byte) 35,
      (byte) 99,
      (byte) 9,
      (byte) 150,
      (byte) 81,
      (byte) 229,
      (byte) 89,
      (byte) 51,
      (byte) 85
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17688()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[1];
      byte[] numArray2 = new byte[1]{ (byte) 58 };
      byte[] numArray3 = new byte[1]{ (byte) 132 };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 1);
      for (int index = 0; index < 1; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[1];
    byte[] numArray5 = new byte[1]{ (byte) 164 };
    byte[] numArray6 = new byte[1]{ (byte) 114 };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 1);
    for (int index = 0; index < 1; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[24];
    byte[] response = new byte[24];
    Array.Copy((Array) sc_17682.sspq, 20, (Array) numArray7, 0, 24);
    key.Query(true, 348, numArray7, response);
    Array.Copy((Array) sc_17682.sspr, 20, (Array) numArray7, 0, 24);
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
