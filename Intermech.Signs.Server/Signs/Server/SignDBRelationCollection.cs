// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignDBRelationCollection
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Server;

public class SignDBRelationCollection(UserSession uSession, int relationType) : DBRelationCollection(uSession, relationType)
{
  public override IDBRelation Create(
    DateTime beginDate,
    long projectID,
    long partID,
    long prjlinkID,
    long partObjectID,
    IDBRelation prototype,
    Guid relationGUID,
    AttributeValues[] vals)
  {
    IDBObject objectByVersionsRule = this.UserSession.GetObjectByVersionsRule(partID, this.FiltrationOwnerID, true);
    if (objectByVersionsRule.IsCreationMode)
      return base.Create(beginDate, projectID, partID, prjlinkID, partObjectID, prototype, relationGUID, vals);
    if (objectByVersionsRule.ObjectType.Equals(SignsHolder.SignObjectTypeID))
      return SignsServerStartup.Server.Sign(new SignCollection()
      {
        UserID = this.UserSession.UserID,
        RankID = Convert.ToInt64(objectByVersionsRule.GetAttributeByID(SignsHolder.RankAttrTypeID).Value),
        ListOfGraphs = {
          Convert.ToString(objectByVersionsRule.GetAttributeByID(SignsHolder.GraphAttrTypeID).Value)
        },
        ListOfIDs = {
          projectID
        }
      }, this.UserSession.SessionGUID, SignsHolder.SignObjectTypeID, out Dictionary<long, List<long>> _);
    if (objectByVersionsRule.ObjectType.Equals(SignsHolder.CryptoSignObjectTypeID))
      throw new ArgumentException(LocalizationHolder.rm.GetString("Signs.Server_5"));
    throw new ArgumentException(LocalizationHolder.rm.GetString("Signs.Server_6"));
  }
}
