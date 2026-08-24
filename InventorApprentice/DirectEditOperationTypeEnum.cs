// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DirectEditOperationTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("6443DBBF-B024-4D6C-B7DA-43EBF527692B")]
public enum DirectEditOperationTypeEnum
{
  kDirectEditMoveOperationType = 105729, // 0x00019D01
  kDirectEditSizeOperationType = 105730, // 0x00019D02
  kDirectEditRotateOperationType = 105731, // 0x00019D03
  kDirectEditDeleteOperationType = 105732, // 0x00019D04
  kDirectEditUnknownOperationType = 105733, // 0x00019D05
}
