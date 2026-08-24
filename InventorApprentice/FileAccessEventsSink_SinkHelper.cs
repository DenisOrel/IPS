// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileAccessEventsSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class FileAccessEventsSink_SinkHelper : FileAccessEventsSink
{
  public FileAccessEventsSink_OnFileResolutionEventHandler m_OnFileResolutionDelegate;
  public FileAccessEventsSink_OnFileDirtyEventHandler m_OnFileDirtyDelegate;
  public int m_dwCookie;

  public override void OnFileResolution(
    [In] string obj0,
    [In] string obj1,
    [In] ref byte[] obj2,
    [In] EventTimingEnum obj3,
    [In] NameValueMap obj4,
    [In] ref string obj5,
    [In] ref HandlingCodeEnum obj6)
  {
    if (this.m_OnFileResolutionDelegate == null)
      return;
    this.m_OnFileResolutionDelegate(obj0, obj1, ref obj2, obj3, obj4, out obj5, out obj6);
  }

  public override void OnFileDirty(
    [In] string obj0,
    [In] string obj1,
    [In] ref byte[] obj2,
    [In] string obj3,
    [In] Document obj4,
    [In] EventTimingEnum obj5,
    [In] NameValueMap obj6,
    [In] ref HandlingCodeEnum obj7)
  {
    if (this.m_OnFileDirtyDelegate == null)
      return;
    this.m_OnFileDirtyDelegate(obj0, obj1, ref obj2, obj3, obj4, obj5, obj6, out obj7);
  }

  internal FileAccessEventsSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnFileResolutionDelegate = (FileAccessEventsSink_OnFileResolutionEventHandler) null;
    this.m_OnFileDirtyDelegate = (FileAccessEventsSink_OnFileDirtyEventHandler) null;
  }
}
