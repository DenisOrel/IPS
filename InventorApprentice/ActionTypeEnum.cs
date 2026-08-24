// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ActionTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("41EB0DBA-6EDF-4BF0-A404-8BF3FB1601E3")]
public enum ActionTypeEnum
{
  kAllActions = -1, // 0xFFFFFFFF
  kNoAction = 0,
  kDeleteAction = 1,
  kActivationAction = 2,
  kReorderAction = 4,
  [TypeLibVar(64 /*0x40*/)] kRestructureAction = 8,
  kMoveAction = 16, // 0x00000010
}
