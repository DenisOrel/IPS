// Decompiled with JetBrains decompiler
// Type: InventorApprentice.MemberManagerErrorsEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("1554BDD7-289C-4A3D-A57C-612AE5D6F5A7")]
public enum MemberManagerErrorsEnum
{
  kMemberManagerNoError = 50410511, // 0x0301340F
  kMemberManagerUnknown = 50410512, // 0x03013410
  kMemberManagerVaultFileStatusFail = 50410513, // 0x03013411
  kMemberManagerVaultCheckoutFail = 50410514, // 0x03013412
  kMemberManagerVaultGetLatestVersionFail = 50410515, // 0x03013413
  kMemberManagerMissingFileWritePermission = 50410516, // 0x03013414
  kMemberManagerDifferentFamily = 50410517, // 0x03013415
  kMemberManagerDifferentMember = 50410518, // 0x03013416
  kMemberManagerMaterialNotFound = 50410519, // 0x03013417
  kMemberManagerLongFilename = 50410520, // 0x03013418
  kMemberManagerFeatureSuppressFail = 50410521, // 0x03013419
  kMemberManagerThreadFeatureNotFound = 50410522, // 0x0301341A
  kMemberManagerThreadCreateFail = 50410523, // 0x0301341B
  kMemberManagerInvalidMemberValue = 50410524, // 0x0301341C
}
