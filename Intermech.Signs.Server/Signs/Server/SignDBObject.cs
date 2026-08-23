// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignDBObject
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.Signs.Server;

public class SignDBObject(UserSession uSession, DataTable objectsTable) : DBObject(uSession, objectsTable), ISignDBObject
{
  private long SignDeleteMode;
  private bool checkGraphsOnCommitCreation = true;

  public override bool CheckAccess(
    ActionType anAction,
    bool aDefaultAccess,
    CheckAccessFlags flags)
  {
    return (this.UserSession.IsSystemSession || anAction != ActionType.EditProperties) && base.CheckAccess(anAction, aDefaultAccess, flags);
  }

  protected override void DoCommitCreation()
  {
    bool flag = false;
    IDBObject dbObject = this.UserSession.GetObject(Convert.ToInt64(this.GetAttributeByID(SignsHolder.SignUpAttrTypeID).Value));
    string str = Convert.ToString(this.GetAttributeByID(SignsHolder.GraphAttrTypeID).Value);
    foreach (object obj in dbObject.GetAttributeByID(SignsHolder.RankAttrTypeID).Values)
    {
      if (obj != null && obj.GetType().Equals(typeof (long)))
      {
        using (MemoryStream sourceStream = new MemoryStream(SignsServerCache.GetSignsSetup((IUserSession) this.UserSession, Convert.ToInt64(obj)) ?? new byte[0]))
        {
          if (sourceStream.Length > 0L)
          {
            Graphs4Type graphs4Type = new Graphs4Type((Stream) sourceStream, SignsServerCache.GetPossibleGraphs());
            List<string> stringList = new List<string>();
            foreach (int objectType in graphs4Type)
            {
              Graphs4TypeStruct graphs4ObjectType = graphs4Type.GetGraphs4ObjectType((IUserSession) this.UserSession, objectType, true);
              stringList.AddRange((IEnumerable<string>) graphs4ObjectType.Graphs);
            }
            if (stringList.Contains(str))
            {
              flag = true;
              break;
            }
          }
        }
      }
    }
    if (!flag && (flag || this.checkGraphsOnCommitCreation))
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("Signs.Server_4"), (object) dbObject.Caption, (object) str));
    base.DoCommitCreation();
  }

  public override int Delete(long DeleteMode)
  {
    this.SignDeleteMode = DeleteMode;
    if ((this.SignDeleteMode & (long) Consts.RelationConstraintMode) == 0L && (this.SignDeleteMode & (long) Consts.PurgeMode) == 0L && !this.IsCreationMode)
      throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_31"), (object) this.ObjectTypeName));
    return base.Delete(DeleteMode);
  }

  protected override void DoNextLCStep(IDBLifecycleStep nextstep)
  {
    if (nextstep.LevelID != this.UserSession.IdentHelper.DeletedID)
      return;
    if ((this.SignDeleteMode & (long) Consts.RelationConstraintMode) == 0L && (this.SignDeleteMode & (long) Consts.PurgeMode) == 0L && !this.IsCreationMode)
      throw new KernelException(string.Format(LocalizationHolder.rm.GetString("Signs.Server_31"), (object) this.ObjectTypeName));
    base.DoNextLCStep(nextstep);
  }

  public override AttributeValues[] GetAttributesValues(GetAttributeValuesModes modes)
  {
    AttributeValues[] attributesValues = base.GetAttributesValues(GetAttributeValuesModes.IncludeGuid | modes);
    foreach (AttributeValues attributeValues in attributesValues)
      attributeValues.ReadOnly = true;
    return attributesValues;
  }

  public bool CheckGraphsOnCommitCreation
  {
    get => this.checkGraphsOnCommitCreation;
    set => this.checkGraphsOnCommitCreation = value;
  }
}
