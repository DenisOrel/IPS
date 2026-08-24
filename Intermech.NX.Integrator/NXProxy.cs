// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXProxy
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Collections;
using Intermech.IO;
using Interop.CADInterface;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXProxy(ICADSystem2 cadInterface, CADSystemProxyBuilder builder) : 
  CADSystemProxy(cadInterface, builder)
{
  protected override bool DoIsActiveDocument(string fullName)
  {
    if (CADInterfaceTracing.ProxyCallTracer.Enabled)
      CADInterfaceTracing.ProxyCallTracer.AddToTrace<string>("NXProxy.IsActiveDocument()", fullName);
    return CollectionUtils.Exists<string>((IEnumerable<string>) this.GetOpenFiles(true), (Predicate<string>) (item => PathUtils.IsSamePath(item, fullName)));
  }

  protected override CADDocumentOpenStatus ValidateDocumentOpenStatus(
    string fullName,
    CADDocumentOpenStatus openStatus)
  {
    if (openStatus == CADDocumentOpenStatus.OpenVisible)
      openStatus = this.IsActiveDocument(fullName) ? CADDocumentOpenStatus.OpenVisible : CADDocumentOpenStatus.OpenInvisible;
    return base.ValidateDocumentOpenStatus(fullName, openStatus);
  }
}
