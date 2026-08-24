// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileManagerEventsSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class FileManagerEventsSink_SinkHelper : FileManagerEventsSink
{
  public FileManagerEventsSink_OnFileDeleteEventHandler m_OnFileDeleteDelegate;
  public FileManagerEventsSink_OnFileCopyEventHandler m_OnFileCopyDelegate;
  public int m_dwCookie;

  public override void OnFileDelete([In] string obj0, [In] NameValueMap obj1, [In] ref HandlingCodeEnum obj2)
  {
    if (this.m_OnFileDeleteDelegate == null)
      return;
    this.m_OnFileDeleteDelegate(obj0, obj1, out obj2);
  }

  public override void OnFileCopy(
    [In] string obj0,
    [In] string obj1,
    [In] bool obj2,
    [In] NameValueMap obj3,
    [In] ref HandlingCodeEnum obj4)
  {
    if (this.m_OnFileCopyDelegate == null)
      return;
    this.m_OnFileCopyDelegate(obj0, obj1, obj2, obj3, out obj4);
  }

  internal FileManagerEventsSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnFileDeleteDelegate = (FileManagerEventsSink_OnFileDeleteEventHandler) null;
    this.m_OnFileCopyDelegate = (FileManagerEventsSink_OnFileCopyEventHandler) null;
  }
}
