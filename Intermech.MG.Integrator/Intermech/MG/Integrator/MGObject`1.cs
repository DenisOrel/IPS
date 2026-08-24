// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGObject`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGObject<T> : IDisposable
{
  protected List<IDisposable> relatedObjects;

  public MGObject(T instance)
  {
    this.Instance = instance;
    this.relatedObjects = new List<IDisposable>();
  }

  public T Instance { get; protected set; }

  public virtual void Dispose()
  {
    if (this.relatedObjects.Count > 0)
    {
      foreach (IDisposable relatedObject in this.relatedObjects)
        relatedObject.Dispose();
    }
    this.relatedObjects = (List<IDisposable>) null;
    if ((object) this.Instance == null)
      return;
    Marshal.FinalReleaseComObject((object) this.Instance);
  }
}
