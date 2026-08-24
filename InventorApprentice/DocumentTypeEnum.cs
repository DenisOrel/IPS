// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DocumentTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("C5198446-8417-11D2-B771-0060B0F159EF")]
public enum DocumentTypeEnum
{
  kUnknownDocumentObject = 12289, // 0x00003001
  kPartDocumentObject = 12290, // 0x00003002
  kAssemblyDocumentObject = 12291, // 0x00003003
  kDrawingDocumentObject = 12292, // 0x00003004
  kPresentationDocumentObject = 12293, // 0x00003005
  kDesignElementDocumentObject = 12294, // 0x00003006
  kForeignModelDocumentObject = 12295, // 0x00003007
  kSATFileDocumentObject = 12296, // 0x00003008
  kNoDocument = 12297, // 0x00003009
}
