// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17179
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_17179
{
  private static byte[] sspq = new byte[35]
  {
    (byte) 144 /*0x90*/,
    (byte) 49,
    (byte) 142,
    (byte) 116,
    (byte) 66,
    (byte) 213,
    (byte) 76,
    (byte) 114,
    (byte) 15,
    (byte) 174,
    (byte) 163,
    (byte) 39,
    (byte) 108,
    (byte) 4,
    (byte) 174,
    (byte) 72,
    (byte) 200,
    (byte) 227,
    (byte) 224 /*0xE0*/,
    (byte) 175,
    (byte) 85,
    (byte) 152,
    (byte) 50,
    (byte) 103,
    (byte) 207,
    (byte) 42,
    (byte) 9,
    (byte) 36,
    (byte) 210,
    (byte) 150,
    (byte) 206,
    (byte) 60,
    (byte) 96 /*0x60*/,
    (byte) 72,
    (byte) 210
  };
  private static byte[] sspr = new byte[35]
  {
    (byte) 228,
    (byte) 75,
    (byte) 239,
    (byte) 134,
    (byte) 185,
    (byte) 251,
    (byte) 149,
    (byte) 47,
    (byte) 249,
    (byte) 157,
    (byte) 238,
    (byte) 21,
    (byte) 215,
    (byte) 2,
    (byte) 25,
    (byte) 194,
    (byte) 108,
    (byte) 135,
    (byte) 16 /*0x10*/,
    (byte) 92,
    (byte) 38,
    (byte) 74,
    (byte) 124,
    (byte) 226,
    (byte) 39,
    (byte) 167,
    (byte) 170,
    (byte) 162,
    (byte) 83,
    (byte) 97,
    (byte) 99,
    (byte) 52,
    (byte) 10,
    (byte) 75,
    (byte) 158
  };

  internal static string ssp_webportal_server_17180()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[12];
      byte[] numArray2 = new byte[12]
      {
        (byte) 82,
        (byte) 70,
        (byte) 5,
        (byte) 168,
        (byte) 221,
        (byte) 99,
        (byte) 106,
        (byte) 238,
        (byte) 189,
        (byte) 52,
        (byte) 129,
        (byte) 206
      };
      byte[] numArray3 = new byte[12]
      {
        (byte) 183,
        (byte) 52,
        (byte) 9,
        (byte) 177,
        (byte) 14,
        (byte) 208 /*0xD0*/,
        (byte) 206,
        (byte) 179,
        (byte) 142,
        (byte) 29,
        (byte) 76,
        (byte) 147
      };
      key.Query(true, 364, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 12);
      for (int index = 0; index < 12; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[14];
      byte[] response = new byte[14];
      Array.Copy((Array) sc_17179.sspq, 0, (Array) numArray4, 0, 14);
      key.Query(true, 364, numArray4, response);
      Array.Copy((Array) sc_17179.sspr, 0, (Array) numArray4, 0, 14);
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
    byte[] numArray5 = new byte[12];
    byte[] numArray6 = new byte[12]
    {
      (byte) 130,
      (byte) 116,
      (byte) 246,
      (byte) 106,
      (byte) 18,
      (byte) 200,
      (byte) 207,
      (byte) 249,
      (byte) 119,
      (byte) 146,
      (byte) 198,
      (byte) 249
    };
    byte[] numArray7 = new byte[12]
    {
      (byte) 148,
      (byte) 193,
      (byte) 73,
      (byte) 167,
      (byte) 211,
      (byte) 110,
      (byte) 104,
      (byte) 201,
      (byte) 168,
      (byte) 24,
      (byte) 114,
      (byte) 221
    };
    key.Query(true, 364, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 12);
    for (int index = 0; index < 12; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }

  internal static string ssp_webportal_server_17181()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[12];
      byte[] numArray2 = new byte[12];
      numArray2[4] = (byte) 53;
      numArray2[11] = (byte) 147;
      numArray2[2] = (byte) 245;
      numArray2[0] = (byte) 89;
      numArray2[9] = (byte) 124;
      numArray2[1] = (byte) 10;
      numArray2[6] = (byte) 107;
      numArray2[3] = (byte) 4;
      numArray2[8] = (byte) 152;
      numArray2[5] = (byte) 88;
      numArray2[10] = (byte) 250;
      numArray2[7] = (byte) 210;
      byte[] numArray3 = new byte[12]
      {
        (byte) 238,
        (byte) 247,
        (byte) 247,
        byte.MaxValue,
        (byte) 253,
        (byte) 163,
        (byte) 181,
        (byte) 22,
        (byte) 126,
        (byte) 33,
        (byte) 132,
        (byte) 98
      };
      key.Query(true, 364, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 12);
      for (int index = 0; index < 12; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[21];
      byte[] response = new byte[21];
      Array.Copy((Array) sc_17179.sspq, 14, (Array) numArray4, 0, 21);
      key.Query(true, 364, numArray4, response);
      Array.Copy((Array) sc_17179.sspr, 14, (Array) numArray4, 0, 21);
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
    byte[] numArray5 = new byte[12];
    byte[] numArray6 = new byte[12]
    {
      (byte) 131,
      (byte) 120,
      (byte) 84,
      (byte) 183,
      (byte) 133,
      (byte) 136,
      (byte) 217,
      (byte) 203,
      (byte) 154,
      (byte) 23,
      (byte) 157,
      (byte) 157
    };
    byte[] numArray7 = new byte[12];
    numArray7[2] = (byte) 0;
    numArray7[11] = (byte) 37;
    numArray7[5] = (byte) 95;
    numArray7[1] = (byte) 7;
    numArray7[7] = (byte) 25;
    numArray7[0] = (byte) 47;
    numArray7[6] = (byte) 24;
    numArray7[8] = (byte) 229;
    numArray7[9] = (byte) 218;
    numArray7[3] = (byte) 223;
    numArray7[10] = (byte) 232;
    numArray7[4] = (byte) 185;
    key.Query(true, 364, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 12);
    for (int index = 0; index < 12; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }
}
