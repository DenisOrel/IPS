// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16689
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16689
{
  internal static string ssp_pdm_16690()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 0,
        (byte) 121,
        (byte) 0,
        (byte) 0,
        (byte) 173,
        (byte) 0
      };
      numArray2[2] = (byte) 207;
      numArray2[3] = (byte) 253;
      numArray2[0] = (byte) 141;
      numArray2[5] = (byte) 148;
      byte[] numArray3 = new byte[6];
      numArray3[2] = (byte) 33;
      numArray3[0] = (byte) 242;
      numArray3[3] = (byte) 143;
      numArray3[5] = (byte) 14;
      numArray3[4] = (byte) 161;
      numArray3[1] = (byte) 56;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 190,
      (byte) 214,
      (byte) 55,
      (byte) 170,
      byte.MaxValue,
      (byte) 43
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 0,
      (byte) 103,
      (byte) 0,
      (byte) 90,
      (byte) 0,
      (byte) 0
    };
    numArray6[2] = (byte) 4;
    numArray6[4] = (byte) 38;
    numArray6[5] = (byte) 72;
    numArray6[0] = (byte) 48 /*0x30*/;
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16691()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 5,
        (byte) 128 /*0x80*/,
        (byte) 114,
        (byte) 121,
        (byte) 65,
        (byte) 139
      };
      byte[] numArray3 = new byte[6]
      {
        (byte) 225,
        (byte) 135,
        (byte) 208 /*0xD0*/,
        (byte) 160 /*0xA0*/,
        (byte) 90,
        (byte) 69
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
      (byte) 168,
      (byte) 75,
      (byte) 132,
      (byte) 94,
      (byte) 92,
      (byte) 18
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 251,
      (byte) 109,
      (byte) 121,
      (byte) 10,
      (byte) 173,
      (byte) 154
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16692()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 2)
    {
      byte[] numArray1 = new byte[18];
      byte[] numArray2 = new byte[18];
      numArray2[6] = (byte) 142;
      numArray2[1] = (byte) 152;
      numArray2[3] = (byte) 174;
      numArray2[11] = (byte) 180;
      numArray2[4] = (byte) 105;
      numArray2[12] = (byte) 57;
      numArray2[8] = (byte) 2;
      numArray2[5] = (byte) 120;
      numArray2[2] = (byte) 82;
      numArray2[0] = (byte) 162;
      numArray2[10] = (byte) 18;
      numArray2[9] = (byte) 158;
      numArray2[7] = (byte) 121;
      numArray2[13] = (byte) 196;
      numArray2[14] = (byte) 158;
      numArray2[15] = (byte) 236;
      numArray2[16 /*0x10*/] = (byte) 138;
      numArray2[17] = (byte) 143;
      byte[] numArray3 = new byte[18];
      numArray3[4] = (byte) 72;
      numArray3[9] = (byte) 103;
      numArray3[0] = (byte) 238;
      numArray3[3] = (byte) 7;
      numArray3[5] = (byte) 112 /*0x70*/;
      numArray3[1] = (byte) 247;
      numArray3[10] = (byte) 242;
      numArray3[7] = (byte) 1;
      numArray3[8] = (byte) 109;
      numArray3[17] = (byte) 88;
      numArray3[2] = (byte) 62;
      numArray3[14] = (byte) 11;
      numArray3[12] = (byte) 60;
      numArray3[6] = (byte) 59;
      numArray3[13] = (byte) 94;
      numArray3[15] = (byte) 1;
      numArray3[16 /*0x10*/] = (byte) 11;
      numArray3[11] = (byte) 105;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 18);
      for (int index = 0; index < 18; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[18];
    byte[] numArray5 = new byte[18]
    {
      (byte) 69,
      (byte) 87,
      (byte) 137,
      (byte) 121,
      (byte) 214,
      (byte) 65,
      (byte) 90,
      (byte) 44,
      (byte) 207,
      (byte) 61,
      (byte) 6,
      (byte) 102,
      (byte) 184,
      (byte) 57,
      (byte) 184,
      (byte) 134,
      (byte) 42,
      (byte) 77
    };
    byte[] numArray6 = new byte[18]
    {
      (byte) 61,
      (byte) 206,
      (byte) 109,
      (byte) 199,
      (byte) 47,
      (byte) 243,
      (byte) 15,
      (byte) 120,
      (byte) 151,
      (byte) 195,
      (byte) 196,
      (byte) 65,
      (byte) 18,
      (byte) 66,
      (byte) 12,
      (byte) 157,
      (byte) 180,
      (byte) 124
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 18);
    for (int index = 0; index < 18; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
