// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileDialogEventsSink_EventProvider
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

internal sealed class FileDialogEventsSink_EventProvider : FileDialogEventsSink_Event, IDisposable
{
  private IConnectionPointContainer m_ConnectionPointContainer;
  private ArrayList m_aEventSinkHelpers;
  private IConnectionPoint m_ConnectionPoint;

  private void Init()
  {
    IConnectionPoint ppCP = (IConnectionPoint) null;
    Guid riid = new Guid(new byte[16 /*0x10*/]
    {
      (byte) 37,
      (byte) 137,
      (byte) 7,
      (byte) 191,
      (byte) 193,
      (byte) 154,
      (byte) 94,
      (byte) 72,
      (byte) 150,
      (byte) 56,
      (byte) 77,
      (byte) 232,
      (byte) 124,
      (byte) 171,
      (byte) 188,
      (byte) 183
    });
    this.m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
    this.m_ConnectionPoint = ppCP;
    this.m_aEventSinkHelpers = new ArrayList();
  }

  public override void add_OnOptions([In] FileDialogEventsSink_OnOptionsEventHandler obj0)
  {
    bool lockTaken;
    try
    {
      Monitor.Enter((object) this, ref lockTaken);
      if (this.m_ConnectionPoint == null)
        this.Init();
      FileDialogEventsSink_SinkHelper pUnkSink = new FileDialogEventsSink_SinkHelper();
      int pdwCookie = 0;
      this.m_ConnectionPoint.Advise((object) pUnkSink, out pdwCookie);
      pUnkSink.m_dwCookie = pdwCookie;
      pUnkSink.m_OnOptionsDelegate = obj0;
      this.m_aEventSinkHelpers.Add((object) pUnkSink);
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit((object) this);
    }
  }

  public override void remove_OnOptions([In] FileDialogEventsSink_OnOptionsEventHandler obj0)
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
        FileDialogEventsSink_SinkHelper aEventSinkHelper = (FileDialogEventsSink_SinkHelper) this.m_aEventSinkHelpers[index];
        if (aEventSinkHelper.m_OnOptionsDelegate != null && ((aEventSinkHelper.m_OnOptionsDelegate.Equals((object) obj0) ? 1 : 0) & (int) byte.MaxValue) != 0)
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

  public FileDialogEventsSink_EventProvider([In] object obj0)
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
          this.m_ConnectionPoint.Unadvise(((FileDialogEventsSink_SinkHelper) this.m_aEventSinkHelpers[index]).m_dwCookie);
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
