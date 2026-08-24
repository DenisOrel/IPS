// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXMultiCADService
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Data.Metadata;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXMultiCADService(IIntegrator owner) : IntegratorService(owner), IMultiCADSupport
{
  public GlobalId<int> JTDerivedDocumentType
  {
    [DebuggerStepThrough] get
    {
      this.RequireReadyState();
      return NXMultiCADService.InternalCaches.IDCache.JTDerivedDocuments.GID;
    }
  }

  private static class InternalCaches
  {
    private static readonly NXMultiCADService.InternalIDCache idCache = new NXMultiCADService.InternalIDCache(MetadataResolvers.Factory);

    public static NXMultiCADService.InternalIDCache IDCache
    {
      [DebuggerStepThrough] get => NXMultiCADService.InternalCaches.idCache;
    }
  }

  private sealed class InternalIDCache
  {
    public InternalIDCache(MetadataResolverFactory metadataResolvers)
    {
      this.JTDerivedDocuments = metadataResolvers.ObjectTypeResolver(new Guid("CADD94EA-306C-11D8-B4E9-00304F19F545"));
    }

    public ObjectTypeResolver JTDerivedDocuments { get; private set; }
  }
}
