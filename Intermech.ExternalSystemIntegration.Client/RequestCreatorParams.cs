// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.RequestCreatorParams
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Interfaces.Client;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

internal class RequestCreatorParams : IObjectCreatorParams
{
  public long SourceObjectID { get; private set; }

  public RequestCreatorParams(long ASourceObjectId) => this.SourceObjectID = ASourceObjectId;

  public bool RawMode => false;
}
