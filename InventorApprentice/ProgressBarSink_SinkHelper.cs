// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProgressBarSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class ProgressBarSink_SinkHelper : ProgressBarSink
{
  public ProgressBarSink_OnCancelEventHandler m_OnCancelDelegate;
  public int m_dwCookie;

  public override void OnCancel()
  {
    if (this.m_OnCancelDelegate == null)
      return;
    this.m_OnCancelDelegate();
  }

  internal ProgressBarSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnCancelDelegate = (ProgressBarSink_OnCancelEventHandler) null;
  }
}
