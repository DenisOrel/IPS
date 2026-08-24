// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16889
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16889
{
  internal static string ssp_pdm_16890()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 12)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[1] = (byte) 61;
      numArray2[0] = (byte) 173;
      numArray2[2] = (byte) 140;
      numArray2[3] = (byte) 25;
      numArray2[4] = (byte) 120;
      numArray2[5] = (byte) 113;
      numArray2[6] = (byte) 100;
      byte[] numArray3 = new byte[7]
      {
        (byte) 145,
        (byte) 12,
        (byte) 101,
        (byte) 215,
        (byte) 28,
        (byte) 16 /*0x10*/,
        (byte) 155
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7]
    {
      (byte) 129,
      (byte) 209,
      (byte) 175,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 144 /*0x90*/
    };
    numArray5[4] = (byte) 2;
    numArray5[3] = (byte) 136;
    numArray5[5] = (byte) 238;
    byte[] numArray6 = new byte[7]
    {
      (byte) 32 /*0x20*/,
      (byte) 239,
      (byte) 161,
      (byte) 201,
      (byte) 242,
      (byte) 206,
      (byte) 158
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16891()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 100,
        (byte) 40,
        (byte) 231,
        (byte) 98,
        (byte) 218,
        (byte) 226,
        (byte) 17
      };
      byte[] numArray3 = new byte[7]
      {
        (byte) 44,
        (byte) 80 /*0x50*/,
        (byte) 218,
        (byte) 115,
        (byte) 184,
        (byte) 231,
        (byte) 254
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[3] = (byte) 193;
    numArray5[1] = (byte) 131;
    numArray5[2] = (byte) 254;
    numArray5[0] = (byte) 167;
    numArray5[4] = (byte) 138;
    numArray5[6] = (byte) 164;
    numArray5[5] = (byte) 21;
    byte[] numArray6 = new byte[7];
    numArray6[6] = (byte) 94;
    numArray6[5] = (byte) 65;
    numArray6[2] = (byte) 190;
    numArray6[0] = (byte) 175;
    numArray6[4] = (byte) 123;
    numArray6[1] = (byte) 151;
    numArray6[3] = (byte) 56;
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16892()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[1] = (byte) 190;
      numArray2[0] = (byte) 239;
      numArray2[4] = (byte) 205;
      numArray2[3] = (byte) 247;
      numArray2[2] = (byte) 30;
      numArray2[5] = (byte) 118;
      numArray2[6] = (byte) 184;
      byte[] numArray3 = new byte[7]
      {
        (byte) 208 /*0xD0*/,
        (byte) 182,
        (byte) 202,
        (byte) 54,
        (byte) 155,
        (byte) 173,
        (byte) 128 /*0x80*/
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7]
    {
      (byte) 130,
      (byte) 182,
      (byte) 80 /*0x50*/,
      (byte) 22,
      (byte) 44,
      (byte) 6,
      (byte) 242
    };
    byte[] numArray6 = new byte[7];
    numArray6[1] = (byte) 183;
    numArray6[0] = (byte) 45;
    numArray6[4] = (byte) 117;
    numArray6[2] = (byte) 104;
    numArray6[3] = (byte) 32 /*0x20*/;
    numArray6[5] = (byte) 205;
    numArray6[6] = (byte) 0;
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
