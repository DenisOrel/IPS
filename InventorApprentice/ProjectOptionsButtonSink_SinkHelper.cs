// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProjectOptionsButtonSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class ProjectOptionsButtonSink_SinkHelper : ProjectOptionsButtonSink
{
  public ProjectOptionsButtonSink_OnClickEventHandler m_OnClickDelegate;
  public int m_dwCookie;

  public override void OnClick([In] NameValueMap obj0)
  {
    if (this.m_OnClickDelegate == null)
      return;
    this.m_OnClickDelegate(obj0);
  }

  internal ProjectOptionsButtonSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnClickDelegate = (ProjectOptionsButtonSink_OnClickEventHandler) null;
  }
}
