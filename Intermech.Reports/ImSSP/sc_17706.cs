// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17706
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_17706
{
  internal static string ssp_imclient_17707()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10];
      numArray2[2] = (byte) 144 /*0x90*/;
      numArray2[3] = (byte) 24;
      numArray2[5] = (byte) 157;
      numArray2[4] = (byte) 206;
      numArray2[0] = (byte) 30;
      numArray2[6] = (byte) 55;
      numArray2[1] = (byte) 217;
      numArray2[7] = (byte) 99;
      numArray2[8] = (byte) 133;
      numArray2[9] = (byte) 47;
      byte[] numArray3 = new byte[10]
      {
        (byte) 20,
        (byte) 220,
        (byte) 192 /*0xC0*/,
        (byte) 124,
        (byte) 253,
        (byte) 118,
        (byte) 68,
        (byte) 214,
        (byte) 104,
        (byte) 150
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
      (byte) 23,
      (byte) 30,
      (byte) 231,
      byte.MaxValue,
      (byte) 231,
      (byte) 68,
      (byte) 145,
      (byte) 152,
      (byte) 234
    };
    byte[] numArray6 = new byte[10];
    numArray6[6] = (byte) 101;
    numArray6[0] = (byte) 163;
    numArray6[1] = (byte) 149;
    numArray6[3] = (byte) 26;
    numArray6[7] = (byte) 184;
    numArray6[5] = (byte) 125;
    numArray6[2] = (byte) 10;
    numArray6[4] = (byte) 249;
    numArray6[8] = (byte) 24;
    numArray6[9] = (byte) 161;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_17708()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[9];
      byte[] numArray2 = new byte[9]
      {
        (byte) 156,
        (byte) 251,
        (byte) 159,
        (byte) 0,
        (byte) 113,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 0
      };
      numArray2[5] = (byte) 103;
      numArray2[8] = (byte) 34;
      numArray2[7] = (byte) 199;
      numArray2[3] = (byte) 171;
      byte[] numArray3 = new byte[9]
      {
        (byte) 229,
        (byte) 156,
        (byte) 133,
        (byte) 12,
        (byte) 138,
        (byte) 185,
        (byte) 38,
        (byte) 174,
        (byte) 124
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 9);
      for (int index = 0; index < 9; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[9];
    byte[] numArray5 = new byte[9];
    numArray5[3] = (byte) 96 /*0x60*/;
    numArray5[4] = (byte) 246;
    numArray5[1] = (byte) 205;
    numArray5[5] = (byte) 137;
    numArray5[0] = (byte) 146;
    numArray5[2] = (byte) 10;
    numArray5[6] = (byte) 43;
    numArray5[7] = (byte) 202;
    numArray5[8] = (byte) 145;
    byte[] numArray6 = new byte[9]
    {
      (byte) 120,
      (byte) 194,
      (byte) 220,
      (byte) 129,
      (byte) 105,
      (byte) 175,
      (byte) 124,
      (byte) 34,
      (byte) 248
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 9);
    for (int index = 0; index < 9; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
