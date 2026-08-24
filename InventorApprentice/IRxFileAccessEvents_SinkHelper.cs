// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFileAccessEvents_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class IRxFileAccessEvents_SinkHelper : IRxFileAccessEvents
{
  public IRxFileAccessEvents_OnFileResolutionEventHandler m_OnFileResolutionDelegate;
  public IRxFileAccessEvents_OnFileDirtyEventHandler m_OnFileDirtyDelegate;
  public int m_dwCookie;

  public override void OnFileResolution(
    [In] string obj0,
    [In] string obj1,
    [In] ref byte[] obj2,
    [In] ref string obj3,
    [In] ref HandlingCodeEnum obj4)
  {
    if (this.m_OnFileResolutionDelegate == null)
      return;
    this.m_OnFileResolutionDelegate(obj0, obj1, ref obj2, out obj3, out obj4);
  }

  public override void OnFileDirty(
    [In] string obj0,
    [In] string obj1,
    [In] ref byte[] obj2,
    [In] string obj3,
    [In] Document obj4,
    [In] ref HandlingCodeEnum obj5)
  {
    if (this.m_OnFileDirtyDelegate == null)
      return;
    this.m_OnFileDirtyDelegate(obj0, obj1, ref obj2, obj3, obj4, out obj5);
  }

  internal IRxFileAccessEvents_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnFileResolutionDelegate = (IRxFileAccessEvents_OnFileResolutionEventHandler) null;
    this.m_OnFileDirtyDelegate = (IRxFileAccessEvents_OnFileDirtyEventHandler) null;
  }
}
