// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.ActInfo
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Interfaces;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class ActInfo : Dictionary<string, S4Table>
{
  private int _schemeID;
  private int _ID;
  private WorkflowScheme _scheme;
  public int ListIndex = -1;
  public DeferredObjectData FormData;
  public DeferredObjectData ScriptData;
  public int SenderActivityID = -1;
  public string[] ExecHistory;
  public List<int> MessageIndexes;
  private int _ParentActivityID = -2;
  public ObjectRecord ObjectRecord;
  private Guid _typeGuid = Guid.Empty;
  private int _typeID = -1;
  private ActivityKind _kind;
  public Dictionary<string, object> Data = new Dictionary<string, object>();
  private ParticipantList _participants;
  public List<long> ResetTimerLinks;
  public ConditionList ExpertConditions;
  public List<ExpressionInfo> ExpressionConditions;
  public List<LinkInfo> Links;
  public List<S4Table> Tasks;
  public long SubProcessInfo;

  public ActInfo(WorkflowScheme scheme) => this._scheme = scheme;

  public WorkflowScheme Scheme => this._scheme;

  public int SchemeID
  {
    get
    {
      if (this._schemeID == 0 && this.Data.ContainsKey("schemeid"))
        this._schemeID = Convert.ToInt32(this.Data["schemeid"]);
      return this._schemeID;
    }
  }

  public int ID
  {
    get
    {
      if (this._ID == 0 && this.Data.ContainsKey("activityid"))
        this._ID = Convert.ToInt32(this.Data["activityid"]);
      return this._ID;
    }
  }

  public int ParentActivityID
  {
    get
    {
      if (this._ParentActivityID == -2)
        this._ParentActivityID = !this.Data.ContainsKey("parentactivityid") ? -1 : Convert.ToInt32(this.Data["parentactivityid"]);
      return this._ParentActivityID;
    }
  }

  public ActivityStatus Status => (ActivityStatus) Convert.ToInt32(this.Data["status"]);

  public long ObjectID => this.ObjectRecord != null ? this.ObjectRecord.Object_id : 0L;

  private void InitIfNeed()
  {
    if (!this._typeGuid.Equals(Guid.Empty))
      return;
    if (this.Data.ContainsKey("kind"))
    {
      ActivityInfo byKind = ActivityInfos.FindByKind(wfConsts.IntToActivityKind(Convert.ToInt32(this.Data["kind"])));
      if (byKind != null)
      {
        this._kind = byKind.Kind;
        this._typeGuid = byKind.TypeGuid;
        this._typeID = byKind.Type;
        return;
      }
    }
    this.Data["kind"] = (object) "1";
    this.InitIfNeed();
  }

  public Guid TypeGuid
  {
    get
    {
      this.InitIfNeed();
      return this._typeGuid;
    }
  }

  public int TypeID
  {
    get
    {
      this.InitIfNeed();
      return this._typeID;
    }
  }

  public ActivityKind Kind
  {
    get
    {
      this.InitIfNeed();
      return this._kind;
    }
  }

  public string Name => this.Data["name"].ToString();

  public S4Table VariablesSource
  {
    get
    {
      return !this.Scheme.IsProcess || this.Status == ActivityStatus.OnApproach ? this.Scheme.Table["vars"] : this["vars"];
    }
  }

  private int GetNewVariableID(int oldid, S4Table vars)
  {
    foreach (KeyValuePair<string, object> var in (Dictionary<string, object>) vars)
    {
      if (Convert.ToInt32(var.Key) == oldid)
        return Convert.ToInt32((var.Value as Dictionary<string, object>)["newid"]);
    }
    return 0;
  }

  public int VarIDToNewVarID(int id)
  {
    int newVarId;
    if (id > 0)
    {
      S4Table variablesSource = this.VariablesSource;
      newVarId = this.GetNewVariableID(id, variablesSource);
    }
    else
      newVarId = wfTables.SystemVarIDToNewVarID(id);
    return newVarId;
  }

  public ParticipantList Participants
  {
    get
    {
      if (this._participants == null)
      {
        ParticipantList pl = new ParticipantList();
        if (this.ContainsKey("parts"))
        {
          S4Table s4Table = this["parts"];
          int num = -1;
          foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) s4Table)
          {
            int int32_1 = Convert.ToInt32(keyValuePair.Key);
            if (keyValuePair.Value is Dictionary<string, object>)
            {
              Dictionary<string, object> dictionary = (Dictionary<string, object>) keyValuePair.Value;
              int int32_2 = Convert.ToInt32(dictionary["participantkind"]);
              if (num == -1)
              {
                num = Convert.ToInt32(dictionary["mustparticipate"]);
                pl.EveryOne = Convert.ToBoolean(num);
              }
              switch (int32_2)
              {
                case 0:
                  long newUserId = BasePumpHelper.GetNewUserID(int32_1);
                  if (newUserId != 0L)
                  {
                    pl.AddParticipant(ParticipantKind.User, newUserId);
                    continue;
                  }
                  continue;
                case 1:
                  PumpHelper.AddGroup(pl, int32_1);
                  continue;
                case 2:
                  long newVarId = (long) this.VarIDToNewVarID(int32_1);
                  if (newVarId != 0L)
                  {
                    pl.AddParticipant(ParticipantKind.Variable, newVarId);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        this._participants = pl;
      }
      return this._participants;
    }
  }

  public void SaveParticipantsToStream(Stream stream) => this.Participants.SaveToStream(stream);
}
