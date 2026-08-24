// Decompiled with JetBrains decompiler
// Type: InventorApprentice.InteractionEventsEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("D6164B0C-35CF-41E5-AF48-4C1FAB5C13C1")]
public enum InteractionEventsEnum
{
  kNoInteraction = 0,
  kSelectInteraction = 1,
  kMouseInteraction = 2,
  kKeyboardInteraction = 4,
  kSelectAndKeyboardInteraction = 5,
  kMouseAndKeyboardInteraction = 6,
}
