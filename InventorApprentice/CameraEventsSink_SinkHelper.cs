// Decompiled with JetBrains decompiler
// Type: InventorApprentice.CameraEventsSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class CameraEventsSink_SinkHelper : CameraEventsSink
{
  public CameraEventsSink_OnCameraChangeEventHandler m_OnCameraChangeDelegate;
  public int m_dwCookie;

  public override void OnCameraChange([In] View obj0, [In] EventTimingEnum obj1, [In] NameValueMap obj2)
  {
    if (this.m_OnCameraChangeDelegate == null)
      return;
    this.m_OnCameraChangeDelegate(obj0, obj1, obj2);
  }

  internal CameraEventsSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnCameraChangeDelegate = (CameraEventsSink_OnCameraChangeEventHandler) null;
  }
}
