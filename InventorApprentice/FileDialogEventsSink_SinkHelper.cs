// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileDialogEventsSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class FileDialogEventsSink_SinkHelper : FileDialogEventsSink
{
  public FileDialogEventsSink_OnOptionsEventHandler m_OnOptionsDelegate;
  public int m_dwCookie;

  public override void OnOptions([In] NameValueMap obj0, [In] ref HandlingCodeEnum obj1)
  {
    if (this.m_OnOptionsDelegate == null)
      return;
    this.m_OnOptionsDelegate(obj0, out obj1);
  }

  internal FileDialogEventsSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnOptionsDelegate = (FileDialogEventsSink_OnOptionsEventHandler) null;
  }
}
