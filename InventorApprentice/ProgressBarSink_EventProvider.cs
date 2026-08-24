// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProgressBarSink_EventProvider
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

#nullable disable
namespace InventorApprentice;

internal sealed class ProgressBarSink_EventProvider : ProgressBarSink_Event, IDisposable
{
  private IConnectionPointContainer m_ConnectionPointContainer;
  private ArrayList m_aEventSinkHelpers;
  private IConnectionPoint m_ConnectionPoint;

  private void Init()
  {
    IConnectionPoint ppCP = (IConnectionPoint) null;
    Guid riid = new Guid(new byte[16 /*0x10*/]
    {
      (byte) 8,
      (byte) 198,
      (byte) 34,
      (byte) 99,
      (byte) 44,
      (byte) 249,
      (byte) 187,
      (byte) 76,
      (byte) 156,
      (byte) 23,
      (byte) 177,
      (byte) 102,
      (byte) 29,
      (byte) 166,
      (byte) 65,
      (byte) 171
    });
    this.m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
    this.m_ConnectionPoint = ppCP;
    this.m_aEventSinkHelpers = new ArrayList();
  }

  public override void add_OnCancel([In] ProgressBarSink_OnCancelEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      ProgressBarSink_SinkHelper pUnkSink = new ProgressBarSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_OnCancelDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_OnCancel([In] ProgressBarSink_OnCancelEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_aEventSinkHelpers == null)
        return;
      int count = this.m_aEventSinkHelpers.Count;
      int index = 0;
      if (0 >= count)
        return;
      do
      {
        ProgressBarSink_SinkHelper aEventSinkHelper = (ProgressBarSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_OnCancelDelegate != null && ((aEventSinkHelper.m_OnCancelDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
        {
          this.m_aEventSinkHelpers.RemoveAt(index);
          this.m_ConnectionPoint.Unadvise(aEventSinkHelper.m_dwCookie);
          if (count <= 1)
          {
            Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
            this.m_ConnectionPoint = (IConnectionPoint) null;
            this.m_aEventSinkHelpers = (ArrayList) null;
            return;
          }
          goto label_11;
        }
        ++index;
      }
      while (index < count);
      goto label_12;
label_11:
      return;
label_12:;
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public ProgressBarSink_EventProvider([In] object obj0)
  {
    this.m_ConnectionPointContainer = (IConnectionPointContainer) obj0;
  }

  public override void Finalize()
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        return;
      int count = this.m_aEventSinkHelpers.Count;
      int index = 0;
      if (0 < count)
      {
        do
        {
          this.m_ConnectionPoint.Unadvise(((ProgressBarSink_SinkHelper) this.m_aEventSinkHelpers[index]).m_dwCookie);
          ++index;
        }
        while (index < count);
      }
      Marshal.ReleaseComObject((object) this.m_ConnectionPoint);
    }
    catch (Exception ex)
    {
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void Dispose()
  {
    this.Finalize();
    GC.SuppressFinalize((object) this);
  }
}
