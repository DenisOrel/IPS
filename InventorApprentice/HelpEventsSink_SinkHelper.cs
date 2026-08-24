// Decompiled with JetBrains decompiler
// Type: InventorApprentice.HelpEventsSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class HelpEventsSink_SinkHelper : HelpEventsSink
{
  public HelpEventsSink_OnApplicationHelpEventHandler m_OnApplicationHelpDelegate;
  public int m_dwCookie;

  public override void OnApplicationHelp([In] NameValueMap obj0, [In] ref HandlingCodeEnum obj1)
  {
    if (this.m_OnApplicationHelpDelegate == null)
      return;
    this.m_OnApplicationHelpDelegate(obj0, out obj1);
  }

  internal HelpEventsSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnApplicationHelpDelegate = (HelpEventsSink_OnApplicationHelpEventHandler) null;
  }
}
