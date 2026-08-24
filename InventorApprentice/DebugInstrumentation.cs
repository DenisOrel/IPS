// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentation
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("F6F33557-6984-11D5-8DF3-0010B541CAA8")]
[CoClass(typeof (DebugInstrumentationClass))]
[ComImport]
public interface DebugInstrumentation : DebugInstrumentationObject, DebugInstrumentationSink_Event
{
}
