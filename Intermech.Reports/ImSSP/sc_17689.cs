// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17689
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_17689
{
  private static byte[] sspq = new byte[21]
  {
    (byte) 26,
    (byte) 146,
    (byte) 96 /*0x60*/,
    (byte) 107,
    (byte) 2,
    (byte) 46,
    (byte) 222,
    (byte) 117,
    (byte) 96 /*0x60*/,
    (byte) 239,
    (byte) 12,
    (byte) 252,
    (byte) 128 /*0x80*/,
    (byte) 39,
    (byte) 163,
    (byte) 79,
    (byte) 132,
    (byte) 245,
    (byte) 199,
    (byte) 50,
    (byte) 110
  };
  private static byte[] sspr = new byte[21]
  {
    (byte) 139,
    (byte) 21,
    (byte) 218,
    (byte) 239,
    (byte) 127 /*0x7F*/,
    (byte) 148,
    (byte) 189,
    (byte) 236,
    (byte) 191,
    (byte) 67,
    (byte) 24,
    (byte) 75,
    (byte) 134,
    (byte) 70,
    (byte) 224 /*0xE0*/,
    (byte) 220,
    (byte) 113,
    (byte) 82,
    (byte) 71,
    (byte) 13,
    (byte) 99
  };

  internal static string ssp_imclient_17690()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10];
      numArray2[2] = (byte) 16 /*0x10*/;
      numArray2[8] = byte.MaxValue;
      numArray2[3] = (byte) 137;
      numArray2[0] = (byte) 46;
      numArray2[4] = (byte) 39;
      numArray2[5] = (byte) 13;
      numArray2[6] = (byte) 164;
      numArray2[1] = (byte) 65;
      numArray2[7] = (byte) 102;
      numArray2[9] = (byte) 212;
      byte[] numArray3 = new byte[10]
      {
        (byte) 122,
        (byte) 39,
        (byte) 220,
        (byte) 64 /*0x40*/,
        (byte) 141,
        (byte) 37,
        (byte) 205,
        (byte) 5,
        (byte) 156,
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
      (byte) 198,
      (byte) 174,
      (byte) 218,
      (byte) 87,
      (byte) 154,
      (byte) 144 /*0x90*/,
      (byte) 97,
      (byte) 35,
      (byte) 250,
      (byte) 199
    };
    byte[] numArray6 = new byte[10];
    numArray6[0] = (byte) 100;
    numArray6[1] = (byte) 148;
    numArray6[3] = (byte) 115;
    numArray6[5] = (byte) 163;
    numArray6[2] = (byte) 100;
    numArray6[8] = (byte) 214;
    numArray6[4] = (byte) 166;
    numArray6[7] = (byte) 36;
    numArray6[6] = (byte) 230;
    numArray6[9] = (byte) 46;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17691()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 48 /*0x30*/,
        (byte) 174,
        (byte) 147,
        (byte) 82,
        (byte) 139,
        (byte) 180,
        (byte) 60,
        (byte) 4,
        (byte) 212,
        (byte) 65
      };
      byte[] numArray3 = new byte[10];
      numArray3[1] = (byte) 102;
      numArray3[6] = (byte) 232;
      numArray3[2] = (byte) 139;
      numArray3[0] = (byte) 9;
      numArray3[9] = (byte) 248;
      numArray3[5] = (byte) 128 /*0x80*/;
      numArray3[3] = (byte) 125;
      numArray3[7] = (byte) 212;
      numArray3[8] = (byte) 12;
      numArray3[4] = (byte) 45;
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10];
    numArray5[7] = (byte) 76;
    numArray5[1] = (byte) 86;
    numArray5[5] = (byte) 180;
    numArray5[0] = (byte) 135;
    numArray5[4] = (byte) 152;
    numArray5[3] = (byte) 140;
    numArray5[6] = (byte) 80 /*0x50*/;
    numArray5[2] = (byte) 10;
    numArray5[8] = (byte) 131;
    numArray5[9] = (byte) 168;
    byte[] numArray6 = new byte[10]
    {
      (byte) 231,
      (byte) 180,
      (byte) 44,
      (byte) 215,
      (byte) 211,
      (byte) 34,
      (byte) 61,
      (byte) 104,
      (byte) 253,
      (byte) 199
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[21];
    byte[] response = new byte[21];
    Array.Copy((Array) sc_17689.sspq, 0, (Array) numArray7, 0, 21);
    key.Query(true, 348, numArray7, response);
    Array.Copy((Array) sc_17689.sspr, 0, (Array) numArray7, 0, 21);
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
