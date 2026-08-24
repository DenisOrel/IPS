// Decompiled with JetBrains decompiler
// Type: InventorApprentice.GraphicsSettingTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("3387CA7A-8378-4436-8187-3A2B026D62E5")]
public enum GraphicsSettingTypeEnum
{
  kQualityGraphicsSetting = 91649, // 0x00016601
  kPerformanceGraphicsSetting = 91650, // 0x00016602
  [TypeLibVar(64 /*0x40*/)] kCompatibilityGraphicsSetting = 91651, // 0x00016603
  kConservativeGraphicsSetting = 91652, // 0x00016604
}
