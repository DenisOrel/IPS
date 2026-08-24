// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.IAttributeCodecCollection
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;

#nullable disable
namespace Intermech.MG.Integrator;

internal interface IAttributeCodecCollection
{
  IAttributeCodec GetDocumentCodec();

  IAttributeCodec GetPartCodec();

  IAttributeCodec GetAssemblyCodec();
}
