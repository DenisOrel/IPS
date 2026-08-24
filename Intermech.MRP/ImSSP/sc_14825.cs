// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14825
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14825
{
  internal static int ssp_mrp_14826(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[2] = (byte) 246;
    sourceArray1[7] = (byte) 40;
    sourceArray1[26] = (byte) 77;
    sourceArray1[15] = (byte) 152;
    sourceArray1[4] = (byte) 210;
    sourceArray1[0] = (byte) 43;
    sourceArray1[6] = (byte) 238;
    sourceArray1[25] = (byte) 241;
    sourceArray1[14] = (byte) 242;
    sourceArray1[9] = (byte) 4;
    sourceArray1[44] = (byte) 238;
    sourceArray1[1] = (byte) 101;
    sourceArray1[16 /*0x10*/] = (byte) 127 /*0x7F*/;
    sourceArray1[13] = (byte) 173;
    sourceArray1[45] = (byte) 246;
    sourceArray1[38] = (byte) 225;
    sourceArray1[40] = (byte) 61;
    sourceArray1[17] = (byte) 20;
    sourceArray1[18] = (byte) 109;
    sourceArray1[19] = (byte) 73;
    sourceArray1[39] = (byte) 205;
    sourceArray1[21] = (byte) 11;
    sourceArray1[11] = (byte) 20;
    sourceArray1[20] = (byte) 221;
    sourceArray1[36] = (byte) 116;
    sourceArray1[46] = (byte) 126;
    sourceArray1[34] = (byte) 18;
    sourceArray1[12] = (byte) 169;
    sourceArray1[28] = (byte) 111;
    sourceArray1[29] = (byte) 149;
    sourceArray1[30] = (byte) 57;
    sourceArray1[3] = (byte) 31 /*0x1F*/;
    sourceArray1[22] = (byte) 112 /*0x70*/;
    sourceArray1[33] = (byte) 185;
    sourceArray1[23] = (byte) 201;
    sourceArray1[10] = (byte) 106;
    sourceArray1[27] = (byte) 50;
    sourceArray1[37] = (byte) 201;
    sourceArray1[5] = (byte) 113;
    sourceArray1[32 /*0x20*/] = (byte) 93;
    sourceArray1[43] = (byte) 246;
    sourceArray1[8] = (byte) 104;
    sourceArray1[41] = (byte) 15;
    sourceArray1[42] = (byte) 117;
    sourceArray1[31 /*0x1F*/] = (byte) 21;
    sourceArray1[35] = (byte) 202;
    sourceArray1[24] = (byte) 190;
    sourceArray1[47] = (byte) 135;
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[8] = (byte) 146;
    sourceArray2[1] = (byte) 102;
    sourceArray2[2] = (byte) 225;
    sourceArray2[12] = (byte) 131;
    sourceArray2[4] = (byte) 113;
    sourceArray2[35] = (byte) 122;
    sourceArray2[25] = (byte) 115;
    sourceArray2[22] = (byte) 89;
    sourceArray2[26] = (byte) 75;
    sourceArray2[41] = (byte) 99;
    sourceArray2[10] = (byte) 66;
    sourceArray2[11] = (byte) 78;
    sourceArray2[44] = (byte) 199;
    sourceArray2[9] = (byte) 165;
    sourceArray2[14] = (byte) 31 /*0x1F*/;
    sourceArray2[15] = (byte) 16 /*0x10*/;
    sourceArray2[20] = (byte) 69;
    sourceArray2[39] = (byte) 210;
    sourceArray2[18] = (byte) 181;
    sourceArray2[19] = (byte) 206;
    sourceArray2[33] = (byte) 149;
    sourceArray2[23] = (byte) 48 /*0x30*/;
    sourceArray2[16 /*0x10*/] = (byte) 244;
    sourceArray2[46] = (byte) 73;
    sourceArray2[24] = (byte) 145;
    sourceArray2[36] = (byte) 245;
    sourceArray2[7] = (byte) 229;
    sourceArray2[27] = (byte) 233;
    sourceArray2[5] = (byte) 199;
    sourceArray2[29] = (byte) 188;
    sourceArray2[28] = (byte) 222;
    sourceArray2[31 /*0x1F*/] = (byte) 70;
    sourceArray2[32 /*0x20*/] = (byte) 133;
    sourceArray2[3] = (byte) 53;
    sourceArray2[34] = (byte) 57;
    sourceArray2[21] = (byte) 151;
    sourceArray2[6] = (byte) 233;
    sourceArray2[37] = (byte) 206;
    sourceArray2[0] = (byte) 31 /*0x1F*/;
    sourceArray2[38] = (byte) 208 /*0xD0*/;
    sourceArray2[40] = (byte) 47;
    sourceArray2[17] = (byte) 43;
    sourceArray2[42] = (byte) 51;
    sourceArray2[43] = (byte) 102;
    sourceArray2[30] = (byte) 233;
    sourceArray2[45] = (byte) 70;
    sourceArray2[13] = (byte) 9;
    sourceArray2[47] = (byte) 147;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 347, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }

  internal static int ssp_mrp_14827(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[17] = (byte) 42;
    sourceArray1[1] = (byte) 132;
    sourceArray1[22] = (byte) 136;
    sourceArray1[3] = (byte) 246;
    sourceArray1[4] = (byte) 214;
    sourceArray1[16 /*0x10*/] = (byte) 147;
    sourceArray1[15] = (byte) 139;
    sourceArray1[10] = (byte) 30;
    sourceArray1[8] = (byte) 68;
    sourceArray1[21] = (byte) 132;
    sourceArray1[41] = (byte) 94;
    sourceArray1[11] = (byte) 107;
    sourceArray1[2] = (byte) 238;
    sourceArray1[14] = (byte) 235;
    sourceArray1[6] = (byte) 100;
    sourceArray1[13] = (byte) 17;
    sourceArray1[44] = (byte) 55;
    sourceArray1[5] = (byte) 176 /*0xB0*/;
    sourceArray1[18] = (byte) 15;
    sourceArray1[19] = (byte) 15;
    sourceArray1[38] = (byte) 121;
    sourceArray1[32 /*0x20*/] = (byte) 123;
    sourceArray1[23] = (byte) 17;
    sourceArray1[12] = (byte) 91;
    sourceArray1[0] = (byte) 74;
    sourceArray1[25] = (byte) 109;
    sourceArray1[26] = (byte) 130;
    sourceArray1[9] = (byte) 248;
    sourceArray1[47] = (byte) 84;
    sourceArray1[29] = (byte) 62;
    sourceArray1[30] = (byte) 247;
    sourceArray1[31 /*0x1F*/] = (byte) 229;
    sourceArray1[37] = (byte) 254;
    sourceArray1[33] = (byte) 161;
    sourceArray1[27] = (byte) 116;
    sourceArray1[35] = (byte) 5;
    sourceArray1[36] = (byte) 138;
    sourceArray1[45] = (byte) 4;
    sourceArray1[34] = (byte) 171;
    sourceArray1[39] = (byte) 145;
    sourceArray1[40] = (byte) 35;
    sourceArray1[46] = (byte) 139;
    sourceArray1[42] = (byte) 149;
    sourceArray1[43] = (byte) 123;
    sourceArray1[7] = (byte) 133;
    sourceArray1[28] = (byte) 36;
    sourceArray1[20] = (byte) 138;
    sourceArray1[24] = (byte) 59;
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 106,
      (byte) 165,
      (byte) 42,
      (byte) 225,
      (byte) 224 /*0xE0*/,
      (byte) 136,
      (byte) 104,
      (byte) 214,
      (byte) 129,
      (byte) 18,
      (byte) 234,
      (byte) 123,
      (byte) 84,
      (byte) 45,
      byte.MaxValue,
      (byte) 160 /*0xA0*/,
      (byte) 22,
      (byte) 208 /*0xD0*/,
      (byte) 184,
      (byte) 222,
      (byte) 118,
      (byte) 123,
      (byte) 155,
      (byte) 163,
      (byte) 3,
      (byte) 164,
      (byte) 157,
      (byte) 111,
      (byte) 146,
      (byte) 8,
      (byte) 173,
      (byte) 59,
      (byte) 171,
      (byte) 8,
      (byte) 138,
      (byte) 206,
      (byte) 237,
      (byte) 234,
      (byte) 124,
      (byte) 8,
      (byte) 28,
      (byte) 0,
      (byte) 9,
      (byte) 98,
      (byte) 170,
      (byte) 159,
      (byte) 115,
      (byte) 246
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 347, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }

  internal static string ssp_mrp_14828()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 20,
        (byte) 209,
        (byte) 239,
        (byte) 148,
        (byte) 87,
        (byte) 65
      };
      byte[] numArray3 = new byte[6]
      {
        byte.MaxValue,
        (byte) 72,
        (byte) 39,
        (byte) 68,
        (byte) 246,
        (byte) 56
      };
      key.Query(true, 347, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 220,
      (byte) 214,
      (byte) 239,
      (byte) 2,
      (byte) 132,
      (byte) 135
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 130,
      (byte) 134,
      (byte) 27,
      (byte) 131,
      (byte) 31 /*0x1F*/,
      (byte) 164
    };
    key.Query(true, 347, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static int ssp_mrp_14829(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 57,
      (byte) 55,
      (byte) 32 /*0x20*/,
      (byte) 127 /*0x7F*/,
      (byte) 221,
      (byte) 83,
      (byte) 81,
      (byte) 160 /*0xA0*/,
      (byte) 26,
      (byte) 184,
      (byte) 45,
      (byte) 96 /*0x60*/,
      (byte) 145,
      (byte) 220,
      (byte) 33,
      (byte) 15,
      (byte) 63 /*0x3F*/,
      (byte) 101,
      (byte) 116,
      (byte) 38,
      (byte) 71,
      (byte) 151,
      (byte) 226,
      (byte) 189,
      (byte) 15,
      (byte) 25,
      (byte) 171,
      (byte) 190,
      (byte) 136,
      (byte) 99,
      (byte) 59,
      (byte) 104,
      (byte) 241,
      (byte) 64 /*0x40*/,
      (byte) 196,
      (byte) 221,
      (byte) 177,
      (byte) 111,
      (byte) 183,
      (byte) 4,
      (byte) 239,
      (byte) 222,
      (byte) 77,
      (byte) 29,
      (byte) 174,
      (byte) 118,
      (byte) 93,
      (byte) 38
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 65,
      (byte) 205,
      (byte) 140,
      (byte) 177,
      (byte) 21,
      (byte) 73,
      (byte) 58,
      (byte) 204,
      (byte) 178,
      (byte) 100,
      (byte) 39,
      (byte) 171,
      (byte) 111,
      (byte) 59,
      (byte) 131,
      (byte) 16 /*0x10*/,
      (byte) 66,
      byte.MaxValue,
      (byte) 225,
      (byte) 109,
      (byte) 81,
      (byte) 74,
      (byte) 33,
      (byte) 25,
      (byte) 230,
      (byte) 12,
      (byte) 100,
      (byte) 247,
      (byte) 131,
      (byte) 182,
      (byte) 200,
      (byte) 88,
      (byte) 140,
      (byte) 53,
      (byte) 41,
      (byte) 64 /*0x40*/,
      (byte) 29,
      (byte) 206,
      (byte) 38,
      (byte) 135,
      (byte) 63 /*0x3F*/,
      (byte) 177,
      (byte) 9,
      (byte) 141,
      (byte) 220,
      (byte) 184,
      (byte) 73,
      (byte) 98
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 347, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
