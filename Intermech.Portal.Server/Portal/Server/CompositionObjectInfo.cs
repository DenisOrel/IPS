// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.CompositionObjectInfo
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class CompositionObjectInfo
{
  public long ObjectID { get; private set; }

  public long ID { get; private set; }

  public long ProjID { get; private set; }

  public string ObjectTypeGuid { get; private set; }

  public string ObjectTypeName { get; private set; }

  public string LinkedGuid { get; private set; }

  public CompositionObjectInfo(
    long objectID,
    long id,
    string objectTypeName,
    string objectTypeGuid,
    string linkedGuid)
    : this(objectID, id, objectTypeName, objectTypeGuid, linkedGuid, 0L)
  {
  }

  public CompositionObjectInfo(
    long objectID,
    long id,
    string objectTypeName,
    string objectTypeGuid,
    string linkedGuid,
    long projID)
  {
    this.ObjectID = objectID;
    this.ID = id;
    this.ObjectTypeName = objectTypeName;
    this.ObjectTypeGuid = objectTypeGuid;
    this.LinkedGuid = linkedGuid;
    this.ProjID = projID;
  }

  public void ClearProjID() => this.ProjID = 0L;

  public bool RootLevel => this.ProjID == 0L;
}
