// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentationSink_EventProvider
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

internal sealed class DebugInstrumentationSink_EventProvider : 
  DebugInstrumentationSink_Event,
  IDisposable
{
  private IConnectionPointContainer m_ConnectionPointContainer;
  private ArrayList m_aEventSinkHelpers;
  private IConnectionPoint m_ConnectionPoint;

  private void Init()
  {
    IConnectionPoint ppCP = (IConnectionPoint) null;
    Guid riid = new Guid(new byte[16 /*0x10*/]
    {
      (byte) 91,
      (byte) 53,
      (byte) 243,
      (byte) 246,
      (byte) 132,
      (byte) 105,
      (byte) 213,
      (byte) 17,
      (byte) 141,
      (byte) 243,
      (byte) 0,
      (byte) 16 /*0x10*/,
      (byte) 181,
      (byte) 65,
      (byte) 202,
      (byte) 168
    });
    this.m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
    this.m_ConnectionPoint = ppCP;
    this.m_aEventSinkHelpers = new ArrayList();
  }

  public override void add_ObjectCreated(
    [In] DebugInstrumentationSink_ObjectCreatedEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      DebugInstrumentationSink_SinkHelper pUnkSink = new DebugInstrumentationSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_ObjectCreatedDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_ObjectCreated(
    [In] DebugInstrumentationSink_ObjectCreatedEventHandler obj0)
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
        DebugInstrumentationSink_SinkHelper aEventSinkHelper = (DebugInstrumentationSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_ObjectCreatedDelegate != null && ((aEventSinkHelper.m_ObjectCreatedDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public override void add_ObjectDestroyed(
    [In] DebugInstrumentationSink_ObjectDestroyedEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      DebugInstrumentationSink_SinkHelper pUnkSink = new DebugInstrumentationSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_ObjectDestroyedDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_ObjectDestroyed(
    [In] DebugInstrumentationSink_ObjectDestroyedEventHandler obj0)
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
        DebugInstrumentationSink_SinkHelper aEventSinkHelper = (DebugInstrumentationSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_ObjectDestroyedDelegate != null && ((aEventSinkHelper.m_ObjectDestroyedDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public override void add_ObjectAddRefd(
    [In] DebugInstrumentationSink_ObjectAddRefdEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      DebugInstrumentationSink_SinkHelper pUnkSink = new DebugInstrumentationSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_ObjectAddRefdDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_ObjectAddRefd(
    [In] DebugInstrumentationSink_ObjectAddRefdEventHandler obj0)
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
        DebugInstrumentationSink_SinkHelper aEventSinkHelper = (DebugInstrumentationSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_ObjectAddRefdDelegate != null && ((aEventSinkHelper.m_ObjectAddRefdDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public override void add_ObjectReleased(
    [In] DebugInstrumentationSink_ObjectReleasedEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      DebugInstrumentationSink_SinkHelper pUnkSink = new DebugInstrumentationSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_ObjectReleasedDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_ObjectReleased(
    [In] DebugInstrumentationSink_ObjectReleasedEventHandler obj0)
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
        DebugInstrumentationSink_SinkHelper aEventSinkHelper = (DebugInstrumentationSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_ObjectReleasedDelegate != null && ((aEventSinkHelper.m_ObjectReleasedDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public override void add_ObjectQueryInterfaced(
    [In] DebugInstrumentationSink_ObjectQueryInterfacedEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      DebugInstrumentationSink_SinkHelper pUnkSink = new DebugInstrumentationSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_ObjectQueryInterfacedDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_ObjectQueryInterfaced(
    [In] DebugInstrumentationSink_ObjectQueryInterfacedEventHandler obj0)
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
        DebugInstrumentationSink_SinkHelper aEventSinkHelper = (DebugInstrumentationSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_ObjectQueryInterfacedDelegate != null && ((aEventSinkHelper.m_ObjectQueryInterfacedDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public override void add_OnMemberInvoke(
    [In] DebugInstrumentationSink_OnMemberInvokeEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      DebugInstrumentationSink_SinkHelper pUnkSink = new DebugInstrumentationSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_OnMemberInvokeDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_OnMemberInvoke(
    [In] DebugInstrumentationSink_OnMemberInvokeEventHandler obj0)
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
        DebugInstrumentationSink_SinkHelper aEventSinkHelper = (DebugInstrumentationSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_OnMemberInvokeDelegate != null && ((aEventSinkHelper.m_OnMemberInvokeDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public DebugInstrumentationSink_EventProvider([In] object obj0)
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
          this.m_ConnectionPoint.Unadvise(((DebugInstrumentationSink_SinkHelper) this.m_aEventSinkHelpers[index]).m_dwCookie);
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
