// Decompiled with JetBrains decompiler
// Type: InventorApprentice.AssetValueTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("3AE65B6E-DC37-4C41-9491-A1D93523B864")]
public enum AssetValueTypeEnum
{
  kAssetValueTypeBoolean = 99329, // 0x00018401
  kAssetValueTypeInteger = 99330, // 0x00018402
  kAssetValueTypeChoice = 99331, // 0x00018403
  kAssetValueTypeFloat = 99332, // 0x00018404
  kAssetValueTypeString = 99333, // 0x00018405
  kAssetValueTypeFilename = 99334, // 0x00018406
  kAssetValueTypeColor = 99335, // 0x00018407
  kAssetValueTextureType = 99336, // 0x00018408
  kAssetValueTypeReference = 99337, // 0x00018409
  kAssetValueUnknownType = 99338, // 0x0001840A
}
