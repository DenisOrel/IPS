// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFileAccessEvents_EventProvider
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

internal sealed class IRxFileAccessEvents_EventProvider : IRxFileAccessEvents_Event, IDisposable
{
  private IConnectionPointContainer m_ConnectionPointContainer;
  private ArrayList m_aEventSinkHelpers;
  private IConnectionPoint m_ConnectionPoint;

  private void Init()
  {
    IConnectionPoint ppCP = (IConnectionPoint) null;
    Guid riid = new Guid(new byte[16 /*0x10*/]
    {
      (byte) 24,
      (byte) 163,
      (byte) 228,
      (byte) 50,
      (byte) 232,
      (byte) 197,
      (byte) 210,
      (byte) 17,
      (byte) 183,
      (byte) 127 /*0x7F*/,
      (byte) 0,
      (byte) 96 /*0x60*/,
      (byte) 176 /*0xB0*/,
      (byte) 241,
      (byte) 89,
      (byte) 239
    });
    this.m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
    this.m_ConnectionPoint = ppCP;
    this.m_aEventSinkHelpers = new ArrayList();
  }

  public override void add_OnFileResolution(
    [In] IRxFileAccessEvents_OnFileResolutionEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      IRxFileAccessEvents_SinkHelper pUnkSink = new IRxFileAccessEvents_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_OnFileResolutionDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_OnFileResolution(
    [In] IRxFileAccessEvents_OnFileResolutionEventHandler obj0)
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
        IRxFileAccessEvents_SinkHelper aEventSinkHelper = (IRxFileAccessEvents_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_OnFileResolutionDelegate != null && ((aEventSinkHelper.m_OnFileResolutionDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public override void add_OnFileDirty([In] IRxFileAccessEvents_OnFileDirtyEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      IRxFileAccessEvents_SinkHelper pUnkSink = new IRxFileAccessEvents_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_OnFileDirtyDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_OnFileDirty([In] IRxFileAccessEvents_OnFileDirtyEventHandler obj0)
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
        IRxFileAccessEvents_SinkHelper aEventSinkHelper = (IRxFileAccessEvents_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_OnFileDirtyDelegate != null && ((aEventSinkHelper.m_OnFileDirtyDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public IRxFileAccessEvents_EventProvider([In] object obj0)
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
          this.m_ConnectionPoint.Unadvise(((IRxFileAccessEvents_SinkHelper) this.m_aEventSinkHelpers[index]).m_dwCookie);
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
