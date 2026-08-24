// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGDocumentEmbedAttributesDriver
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGDocumentEmbedAttributesDriver(IIntegrator integrator) : 
  DocumentEmbedAttributesDriver(integrator)
{
  protected override ICollection<StringKey> DoGetEmbeddableAttributes(
    long documentId,
    int documentType)
  {
    return documentType == MetaDataHelper.GetObjectTypeID(MGConsts.ObjTypeExPCBProject) ? (ICollection<StringKey>) new List<StringKey>(0) : base.DoGetEmbeddableAttributes(documentId, documentType);
  }
}
