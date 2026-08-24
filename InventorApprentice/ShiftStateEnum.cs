// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ShiftStateEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("84094AF2-BC3B-4EEC-BE9F-6C52F372A6A7")]
public enum ShiftStateEnum
{
  kShiftStateNone,
  kShiftStateShift,
  kShiftStateCtrl,
  kShiftStateShiftCtrl,
  kShiftStateAlt,
  kShiftStateShiftAlt,
  kShiftStateCtrlAlt,
  kShiftStateShiftCtrlAlt,
}
