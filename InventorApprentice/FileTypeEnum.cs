// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("97596346-841E-4972-A127-668AAE78D119")]
public enum FileTypeEnum
{
  kUnknownFileType = 56321, // 0x0000DC01
  kPartFileType = 56322, // 0x0000DC02
  kAssemblyFileType = 56323, // 0x0000DC03
  kDrawingFileType = 56324, // 0x0000DC04
  kPresentationFileType = 56325, // 0x0000DC05
  kDesignElementFileType = 56326, // 0x0000DC06
  kForeignFileType = 56327, // 0x0000DC07
}
