// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17672
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_17672
{
  private static byte[] sspq = new byte[77]
  {
    (byte) 225,
    (byte) 56,
    (byte) 185,
    (byte) 40,
    (byte) 60,
    (byte) 193,
    (byte) 233,
    (byte) 139,
    (byte) 180,
    (byte) 37,
    (byte) 144 /*0x90*/,
    (byte) 41,
    (byte) 185,
    (byte) 181,
    (byte) 128 /*0x80*/,
    (byte) 25,
    (byte) 77,
    (byte) 13,
    (byte) 203,
    (byte) 11,
    (byte) 79,
    (byte) 253,
    (byte) 4,
    (byte) 136,
    (byte) 56,
    (byte) 69,
    (byte) 123,
    (byte) 212,
    (byte) 66,
    (byte) 91,
    (byte) 143,
    (byte) 153,
    (byte) 118,
    (byte) 132,
    (byte) 103,
    (byte) 175,
    (byte) 40,
    (byte) 69,
    (byte) 136,
    (byte) 13,
    (byte) 54,
    (byte) 110,
    (byte) 64 /*0x40*/,
    (byte) 250,
    (byte) 120,
    (byte) 202,
    (byte) 224 /*0xE0*/,
    (byte) 88,
    (byte) 160 /*0xA0*/,
    (byte) 212,
    (byte) 114,
    (byte) 138,
    (byte) 87,
    (byte) 151,
    (byte) 1,
    (byte) 22,
    (byte) 161,
    (byte) 167,
    (byte) 47,
    (byte) 59,
    (byte) 128 /*0x80*/,
    (byte) 107,
    (byte) 6,
    (byte) 201,
    (byte) 183,
    (byte) 206,
    (byte) 117,
    (byte) 33,
    (byte) 99,
    (byte) 36,
    (byte) 117,
    (byte) 84,
    (byte) 23,
    (byte) 189,
    (byte) 247,
    (byte) 134,
    (byte) 111
  };
  private static byte[] sspr = new byte[77]
  {
    (byte) 254,
    (byte) 135,
    (byte) 105,
    (byte) 91,
    (byte) 133,
    (byte) 74,
    (byte) 187,
    (byte) 227,
    (byte) 57,
    (byte) 64 /*0x40*/,
    (byte) 152,
    (byte) 12,
    (byte) 54,
    (byte) 250,
    (byte) 42,
    (byte) 231,
    (byte) 46,
    (byte) 177,
    (byte) 60,
    (byte) 28,
    (byte) 103,
    (byte) 121,
    (byte) 221,
    (byte) 113,
    (byte) 43,
    (byte) 32 /*0x20*/,
    (byte) 233,
    (byte) 55,
    (byte) 225,
    (byte) 139,
    (byte) 131,
    (byte) 234,
    (byte) 86,
    (byte) 132,
    (byte) 79,
    (byte) 207,
    (byte) 187,
    byte.MaxValue,
    (byte) 29,
    (byte) 194,
    (byte) 45,
    (byte) 173,
    (byte) 139,
    (byte) 54,
    (byte) 7,
    (byte) 162,
    (byte) 76,
    (byte) 50,
    (byte) 29,
    (byte) 165,
    (byte) 156,
    (byte) 2,
    (byte) 98,
    (byte) 177,
    (byte) 94,
    (byte) 195,
    (byte) 13,
    (byte) 146,
    (byte) 243,
    (byte) 176 /*0xB0*/,
    (byte) 113,
    (byte) 205,
    (byte) 63 /*0x3F*/,
    (byte) 24,
    (byte) 109,
    (byte) 121,
    (byte) 34,
    (byte) 193,
    (byte) 51,
    (byte) 242,
    (byte) 93,
    (byte) 65,
    byte.MaxValue,
    (byte) 167,
    (byte) 241,
    (byte) 97,
    (byte) 157
  };

  internal static string ssp_imclient_17673()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 153,
        (byte) 128 /*0x80*/,
        (byte) 49,
        (byte) 224 /*0xE0*/,
        (byte) 38,
        (byte) 190,
        (byte) 62,
        (byte) 184,
        (byte) 94,
        (byte) 157
      };
      byte[] numArray3 = new byte[10]
      {
        (byte) 139,
        (byte) 173,
        (byte) 238,
        (byte) 8,
        (byte) 117,
        (byte) 179,
        (byte) 54,
        (byte) 175,
        (byte) 170,
        (byte) 105
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[16 /*0x10*/];
      byte[] response = new byte[16 /*0x10*/];
      Array.Copy((Array) sc_17672.sspq, 0, (Array) numArray4, 0, 16 /*0x10*/);
      key.Query(true, 348, numArray4, response);
      Array.Copy((Array) sc_17672.sspr, 0, (Array) numArray4, 0, 16 /*0x10*/);
      for (int index = 0; index < numArray4.Length; ++index)
      {
        if ((int) numArray4[index] != (int) response[index])
        {
          key.TagValue = (int) response[index];
          break;
        }
      }
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray5 = new byte[10];
    byte[] numArray6 = new byte[10]
    {
      (byte) 33,
      (byte) 94,
      (byte) 134,
      (byte) 2,
      (byte) 245,
      (byte) 72,
      (byte) 193,
      (byte) 138,
      (byte) 184,
      (byte) 248
    };
    byte[] numArray7 = new byte[10];
    numArray7[5] = (byte) 106;
    numArray7[7] = (byte) 16 /*0x10*/;
    numArray7[2] = (byte) 134;
    numArray7[0] = (byte) 89;
    numArray7[9] = (byte) 170;
    numArray7[3] = (byte) 23;
    numArray7[6] = (byte) 17;
    numArray7[8] = (byte) 1;
    numArray7[4] = (byte) 46;
    numArray7[1] = (byte) 100;
    key.Query(true, 348, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray5[index] ^= numArray7[index];
    byte[] numArray8 = new byte[17];
    byte[] response1 = new byte[17];
    Array.Copy((Array) sc_17672.sspq, 16 /*0x10*/, (Array) numArray8, 0, 17);
    key.Query(true, 348, numArray8, response1);
    Array.Copy((Array) sc_17672.sspr, 16 /*0x10*/, (Array) numArray8, 0, 17);
    for (int index = 0; index < numArray8.Length; ++index)
    {
      if ((int) numArray8[index] != (int) response1[index])
      {
        key.TagValue = (int) response1[index];
        break;
      }
    }
    return Encoding.UTF8.GetString(numArray5);
  }

  internal static string ssp_imclient_17674()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 112 /*0x70*/,
        (byte) 68,
        (byte) 183,
        (byte) 156,
        (byte) 71,
        (byte) 71,
        (byte) 161,
        (byte) 148,
        (byte) 233,
        (byte) 56
      };
      byte[] numArray3 = new byte[10]
      {
        (byte) 206,
        (byte) 111,
        (byte) 245,
        (byte) 27,
        (byte) 218,
        (byte) 222,
        (byte) 94,
        (byte) 162,
        (byte) 25,
        (byte) 52
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
      (byte) 25,
      (byte) 78,
      (byte) 170,
      (byte) 180,
      (byte) 231,
      (byte) 124,
      (byte) 182,
      (byte) 59,
      (byte) 187,
      (byte) 243
    };
    byte[] numArray6 = new byte[10];
    numArray6[2] = (byte) 186;
    numArray6[4] = (byte) 63 /*0x3F*/;
    numArray6[1] = (byte) 54;
    numArray6[3] = (byte) 136;
    numArray6[5] = (byte) 56;
    numArray6[6] = (byte) 133;
    numArray6[0] = (byte) 205;
    numArray6[7] = (byte) 186;
    numArray6[8] = (byte) 103;
    numArray6[9] = (byte) 55;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17675()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 177,
        (byte) 197,
        (byte) 131,
        (byte) 106,
        (byte) 187,
        (byte) 19,
        (byte) 204,
        (byte) 106,
        (byte) 83,
        (byte) 170
      };
      byte[] numArray3 = new byte[10];
      numArray3[5] = (byte) 44;
      numArray3[9] = (byte) 96 /*0x60*/;
      numArray3[2] = (byte) 169;
      numArray3[1] = (byte) 88;
      numArray3[8] = (byte) 76;
      numArray3[4] = (byte) 16 /*0x10*/;
      numArray3[7] = (byte) 71;
      numArray3[3] = (byte) 40;
      numArray3[6] = (byte) 118;
      numArray3[0] = (byte) 65;
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10]
    {
      (byte) 85,
      (byte) 242,
      (byte) 218,
      (byte) 5,
      (byte) 70,
      (byte) 31 /*0x1F*/,
      (byte) 192 /*0xC0*/,
      (byte) 75,
      (byte) 130,
      (byte) 131
    };
    byte[] numArray6 = new byte[10]
    {
      (byte) 208 /*0xD0*/,
      (byte) 53,
      (byte) 245,
      (byte) 135,
      (byte) 86,
      (byte) 250,
      (byte) 152,
      (byte) 162,
      (byte) 73,
      (byte) 210
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17676()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 12)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 75,
        (byte) 127 /*0x7F*/,
        (byte) 168,
        (byte) 38,
        (byte) 21,
        (byte) 193,
        (byte) 83,
        (byte) 143,
        (byte) 41,
        (byte) 34
      };
      byte[] numArray3 = new byte[10];
      numArray3[6] = (byte) 196;
      numArray3[1] = (byte) 48 /*0x30*/;
      numArray3[2] = (byte) 57;
      numArray3[5] = (byte) 205;
      numArray3[4] = (byte) 73;
      numArray3[8] = (byte) 129;
      numArray3[0] = (byte) 5;
      numArray3[3] = (byte) 157;
      numArray3[7] = (byte) 74;
      numArray3[9] = (byte) 112 /*0x70*/;
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10];
    numArray5[9] = (byte) 134;
    numArray5[1] = (byte) 134;
    numArray5[4] = (byte) 94;
    numArray5[5] = (byte) 223;
    numArray5[2] = (byte) 75;
    numArray5[0] = (byte) 13;
    numArray5[6] = (byte) 2;
    numArray5[3] = (byte) 146;
    numArray5[8] = (byte) 163;
    numArray5[7] = (byte) 17;
    byte[] numArray6 = new byte[10]
    {
      (byte) 161,
      (byte) 76,
      (byte) 92,
      (byte) 59,
      (byte) 242,
      (byte) 75,
      (byte) 82,
      (byte) 170,
      (byte) 5,
      (byte) 117
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17677()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 92,
        (byte) 239,
        (byte) 159,
        (byte) 8,
        (byte) 153,
        (byte) 217,
        (byte) 166,
        (byte) 203,
        (byte) 211,
        (byte) 158
      };
      byte[] numArray3 = new byte[10]
      {
        (byte) 251,
        (byte) 158,
        (byte) 240 /*0xF0*/,
        (byte) 72,
        (byte) 16 /*0x10*/,
        (byte) 199,
        (byte) 209,
        (byte) 168,
        (byte) 152,
        (byte) 74
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
      (byte) 46,
      (byte) 245,
      (byte) 175,
      (byte) 39,
      (byte) 16 /*0x10*/,
      (byte) 153,
      (byte) 194,
      (byte) 189,
      (byte) 240 /*0xF0*/,
      (byte) 232
    };
    byte[] numArray6 = new byte[10]
    {
      (byte) 38,
      (byte) 142,
      (byte) 254,
      (byte) 132,
      (byte) 176 /*0xB0*/,
      (byte) 210,
      (byte) 246,
      (byte) 208 /*0xD0*/,
      (byte) 131,
      (byte) 195
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[44];
    byte[] response = new byte[44];
    Array.Copy((Array) sc_17672.sspq, 33, (Array) numArray7, 0, 44);
    key.Query(true, 348, numArray7, response);
    Array.Copy((Array) sc_17672.sspr, 33, (Array) numArray7, 0, 44);
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
