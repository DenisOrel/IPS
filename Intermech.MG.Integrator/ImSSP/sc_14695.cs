// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14695
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14695
{
  private static byte[] sspq = new byte[53]
  {
    (byte) 142,
    (byte) 54,
    (byte) 112 /*0x70*/,
    (byte) 173,
    (byte) 183,
    (byte) 93,
    (byte) 118,
    (byte) 197,
    (byte) 159,
    (byte) 127 /*0x7F*/,
    (byte) 58,
    (byte) 238,
    (byte) 147,
    (byte) 76,
    (byte) 190,
    (byte) 208 /*0xD0*/,
    (byte) 119,
    (byte) 144 /*0x90*/,
    (byte) 7,
    (byte) 160 /*0xA0*/,
    (byte) 17,
    (byte) 145,
    (byte) 64 /*0x40*/,
    (byte) 221,
    (byte) 130,
    (byte) 233,
    (byte) 197,
    (byte) 6,
    (byte) 165,
    (byte) 143,
    (byte) 127 /*0x7F*/,
    (byte) 7,
    (byte) 90,
    (byte) 163,
    (byte) 34,
    (byte) 157,
    (byte) 78,
    (byte) 184,
    (byte) 62,
    (byte) 199,
    (byte) 237,
    (byte) 82,
    (byte) 153,
    (byte) 158,
    (byte) 217,
    (byte) 64 /*0x40*/,
    (byte) 210,
    (byte) 60,
    (byte) 24,
    (byte) 240 /*0xF0*/,
    (byte) 227,
    (byte) 160 /*0xA0*/,
    (byte) 63 /*0x3F*/
  };
  private static byte[] sspr = new byte[53]
  {
    (byte) 184,
    (byte) 120,
    (byte) 246,
    (byte) 96 /*0x60*/,
    (byte) 44,
    (byte) 33,
    (byte) 153,
    (byte) 182,
    (byte) 45,
    (byte) 198,
    (byte) 196,
    (byte) 187,
    (byte) 184,
    (byte) 30,
    (byte) 243,
    (byte) 57,
    (byte) 88,
    (byte) 185,
    (byte) 121,
    (byte) 187,
    (byte) 215,
    (byte) 44,
    (byte) 161,
    (byte) 203,
    (byte) 129,
    (byte) 119,
    (byte) 238,
    (byte) 235,
    (byte) 239,
    (byte) 89,
    (byte) 133,
    (byte) 180,
    (byte) 11,
    (byte) 24,
    (byte) 235,
    (byte) 101,
    (byte) 50,
    (byte) 109,
    (byte) 186,
    (byte) 147,
    (byte) 172,
    (byte) 133,
    (byte) 188,
    (byte) 33,
    (byte) 57,
    (byte) 189,
    (byte) 136,
    (byte) 244,
    (byte) 55,
    (byte) 44,
    (byte) 225,
    (byte) 186,
    (byte) 186
  };

  internal static string ssp_mentor_14696()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 24,
        (byte) 56,
        (byte) 29,
        (byte) 19,
        (byte) 47,
        (byte) 101,
        (byte) 136,
        (byte) 215,
        (byte) 193,
        (byte) 91
      };
      byte[] numArray3 = new byte[10]
      {
        (byte) 132,
        (byte) 139,
        (byte) 87,
        (byte) 65,
        (byte) 33,
        (byte) 223,
        (byte) 235,
        (byte) 249,
        (byte) 191,
        (byte) 225
      };
      key.Query(true, 346, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[53];
      byte[] response = new byte[53];
      Array.Copy((Array) sc_14695.sspq, 0, (Array) numArray4, 0, 53);
      key.Query(true, 346, numArray4, response);
      Array.Copy((Array) sc_14695.sspr, 0, (Array) numArray4, 0, 53);
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
    byte[] numArray6 = new byte[10];
    numArray6[2] = (byte) 252;
    numArray6[1] = (byte) 66;
    numArray6[6] = (byte) 205;
    numArray6[5] = (byte) 42;
    numArray6[4] = (byte) 147;
    numArray6[0] = (byte) 148;
    numArray6[8] = (byte) 111;
    numArray6[7] = (byte) 151;
    numArray6[3] = (byte) 246;
    numArray6[9] = (byte) 32 /*0x20*/;
    byte[] numArray7 = new byte[10];
    numArray7[5] = (byte) 249;
    numArray7[1] = (byte) 170;
    numArray7[2] = (byte) 131;
    numArray7[6] = (byte) 195;
    numArray7[3] = (byte) 206;
    numArray7[8] = (byte) 157;
    numArray7[0] = (byte) 38;
    numArray7[4] = (byte) 245;
    numArray7[7] = (byte) 117;
    numArray7[9] = (byte) 114;
    key.Query(true, 346, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }
}
