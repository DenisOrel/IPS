// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ToleranceTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("39807315-0A55-43E4-BC42-A279FE25C46A")]
public enum ToleranceTypeEnum
{
  kDefaultTolerance = 31233, // 0x00007A01
  kOverrideTolerance = 31234, // 0x00007A02
  kSymmetricTolerance = 31235, // 0x00007A03
  kDeviationTolerance = 31236, // 0x00007A04
  kLimitsStackedTolerance = 31237, // 0x00007A05
  kLimitLinearTolerance = 31238, // 0x00007A06
  kMaxTolerance = 31239, // 0x00007A07
  kMinTolerance = 31240, // 0x00007A08
  kLimitsFitsStackedTolerance = 31241, // 0x00007A09
  kLimitsFitsLinearTolerance = 31242, // 0x00007A0A
  kLimitsFitsShowSizeTolerance = 31243, // 0x00007A0B
  kLimitsFitsShowTolerance = 31244, // 0x00007A0C
  kBasicTolerance = 31245, // 0x00007A0D
  kReferenceTolerance = 31246, // 0x00007A0E
}
