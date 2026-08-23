// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18419
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18419
{
  internal static string ssp_signs_18420()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8]
      {
        (byte) 149,
        (byte) 115,
        (byte) 49,
        (byte) 199,
        (byte) 173,
        (byte) 61,
        (byte) 54,
        (byte) 53
      };
      byte[] numArray3 = new byte[8];
      numArray3[0] = (byte) 139;
      numArray3[1] = (byte) 126;
      numArray3[5] = (byte) 44;
      numArray3[3] = (byte) 194;
      numArray3[6] = (byte) 72;
      numArray3[4] = (byte) 95;
      numArray3[2] = (byte) 156;
      numArray3[7] = (byte) 116;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[8];
    byte[] numArray5 = new byte[8]
    {
      (byte) 96 /*0x60*/,
      (byte) 144 /*0x90*/,
      (byte) 3,
      (byte) 201,
      (byte) 7,
      (byte) 156,
      (byte) 202,
      (byte) 229
    };
    byte[] numArray6 = new byte[8]
    {
      (byte) 98,
      (byte) 148,
      (byte) 57,
      (byte) 23,
      (byte) 44,
      (byte) 172,
      (byte) 46,
      (byte) 91
    };
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_signs_18421()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8]
      {
        (byte) 171,
        (byte) 210,
        (byte) 36,
        (byte) 35,
        (byte) 32 /*0x20*/,
        (byte) 185,
        (byte) 21,
        (byte) 177
      };
      byte[] numArray3 = new byte[8];
      numArray3[2] = (byte) 31 /*0x1F*/;
      numArray3[1] = (byte) 45;
      numArray3[3] = (byte) 211;
      numArray3[5] = (byte) 76;
      numArray3[4] = (byte) 159;
      numArray3[0] = (byte) 241;
      numArray3[6] = (byte) 33;
      numArray3[7] = (byte) 137;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[8];
    byte[] numArray5 = new byte[8];
    numArray5[7] = (byte) 41;
    numArray5[1] = (byte) 4;
    numArray5[2] = (byte) 38;
    numArray5[5] = (byte) 146;
    numArray5[4] = (byte) 118;
    numArray5[0] = (byte) 109;
    numArray5[6] = (byte) 170;
    numArray5[3] = (byte) 201;
    byte[] numArray6 = new byte[8]
    {
      (byte) 54,
      (byte) 183,
      (byte) 21,
      (byte) 33,
      (byte) 98,
      (byte) 33,
      (byte) 18,
      (byte) 201
    };
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
