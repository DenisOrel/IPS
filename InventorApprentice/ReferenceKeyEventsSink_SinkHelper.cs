// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferenceKeyEventsSink_SinkHelper
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class ReferenceKeyEventsSink_SinkHelper : ReferenceKeyEventsSink
{
  public ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler m_OnBindKeyToObjectDelegate;
  public int m_dwCookie;

  public override void OnBindKeyToObject(
    [In] ref byte[] obj0,
    [In] object obj1,
    [In] ref object obj2,
    [In] ref SolutionNatureEnum obj3,
    [In] NameValueMap obj4,
    [In] ref HandlingCodeEnum obj5)
  {
    if (this.m_OnBindKeyToObjectDelegate == null)
      return;
    this.m_OnBindKeyToObjectDelegate(ref obj0, obj1, ref obj2, out obj3, obj4, ref obj5);
  }

  internal ReferenceKeyEventsSink_SinkHelper()
  {
    this.m_dwCookie = 0;
    this.m_OnBindKeyToObjectDelegate = (ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler) null;
  }
}
