// Decompiled with JetBrains decompiler
// Type: InventorApprentice.BOMStructureEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("7C5A0BCF-3BCF-4B9E-97FF-CC90751155E0")]
public enum BOMStructureEnum
{
  kDefaultBOMStructure = 51969, // 0x0000CB01
  kNormalBOMStructure = 51970, // 0x0000CB02
  kPhantomBOMStructure = 51971, // 0x0000CB03
  kReferenceBOMStructure = 51972, // 0x0000CB04
  kPurchasedBOMStructure = 51973, // 0x0000CB05
  kInseparableBOMStructure = 51974, // 0x0000CB06
  kVariesBOMStructure = 51975, // 0x0000CB07
}
