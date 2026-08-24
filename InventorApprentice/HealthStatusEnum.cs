// Decompiled with JetBrains decompiler
// Type: InventorApprentice.HealthStatusEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("B9DF2F35-96D9-42D5-9505-14B94CF1D6F6")]
public enum HealthStatusEnum
{
  kUnknownHealth = 11777, // 0x00002E01
  kUpToDateHealth = 11778, // 0x00002E02
  kOutOfDateHealth = 11779, // 0x00002E03
  kDriverLostHealth = 11780, // 0x00002E04
  kInErrorHealth = 11781, // 0x00002E05
  kDeletedHealth = 11782, // 0x00002E06
  kCannotComputeHealth = 11783, // 0x00002E07
  kSuppressedHealth = 11784, // 0x00002E08
  kBeyondStopNodeHealth = 11785, // 0x00002E09
  kInconsistentHealth = 11786, // 0x00002E0A
  kRedundantHealth = 11787, // 0x00002E0B
  kNewlyAddedHealth = 11788, // 0x00002E0C
  kInvalidLimitsHealth = 11789, // 0x00002E0D
  kJointDOFLockedHealth = 11790, // 0x00002E0E
}
