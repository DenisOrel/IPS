// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileFormatEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("57016F38-68B4-4C0C-8F12-D121F3232EE6")]
public enum FileFormatEnum
{
  kMicrosoftAccessFormat = 74497, // 0x00012301
  kMicrosoftExcelFormat = 74498, // 0x00012302
  kdBASEIIIFormat = 74499, // 0x00012303
  kdBASEIVFormat = 74500, // 0x00012304
  kTextFileTabDelimitedFormat = 74501, // 0x00012305
  kTextFileCommaDelimitedFormat = 74502, // 0x00012306
  kUnicodeTextFileTabDelimitedFormat = 74503, // 0x00012307
  kUnicodeTextFileCommaDelimitedFormat = 74504, // 0x00012308
}
