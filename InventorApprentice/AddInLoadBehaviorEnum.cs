// Decompiled with JetBrains decompiler
// Type: InventorApprentice.AddInLoadBehaviorEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5363A2A6-7D80-47B5-A27E-80BA79F51DC3")]
public enum AddInLoadBehaviorEnum
{
  kLoadImmediately = 94721, // 0x00017201
  kLoadWithParts = 94722, // 0x00017202
  kLoadWithAssemblies = 94723, // 0x00017203
  kLoadWithPresentations = 94724, // 0x00017204
  kLoadWithDrawings = 94725, // 0x00017205
  kLoadOnDemand = 94726, // 0x00017206
  kLoadBehaviorUnknown = 94727, // 0x00017207
}
