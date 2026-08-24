// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DocumentLoadStateEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("73FB29A5-FE64-4153-B63E-A3A83690068F")]
public enum DocumentLoadStateEnum
{
  kDocumentUnknownLoadState = 103937, // 0x00019601
  kDocumentExpressLoadState = 103938, // 0x00019602
  [TypeLibVar(64 /*0x40*/)] kDocumentLiteLoadState = 103938, // 0x00019602
  kDocumentFullLoadState = 103939, // 0x00019603
  kDocumentPartialLoadState = 103940, // 0x00019604
}
