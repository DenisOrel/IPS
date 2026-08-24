// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.wfTables
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Interfaces;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Workflow;

public static class wfTables
{
  public static readonly string Schemes = "WorkflowSchemesTable";
  public static readonly string Activities = "ActivitiesTable";
  public static readonly string ActivityLinks = "ActivityLinksTable";
  public static readonly string ActivitiesData = "ActivitiesDataTable";
  public static readonly string Variables = "VariablesTable";
  public static readonly string Forms = "ActivityFormsTable";
  public static readonly string Attachments = "AttachmentsTable";
  public static readonly string Participants = "ParticipantsTable";
  public static readonly string ActivityNotifications = "ActivityNotificationsTable";
  public static readonly string ActivityMessages = "ActivityMessagesTable";
  public static readonly string ActivityMsgRecipients = "ActivityMsgRecipientsTable";
  public static readonly string UserMail = "UserTasksTable";
  public static readonly string UserMessages = "UserMessagesTable";
  public static readonly string ActMessages = "MessagesTable";
  public static string[] SchemeID_Linked = new string[9]
  {
    wfTables.ActivitiesData,
    wfTables.Variables,
    wfTables.Forms,
    wfTables.Attachments,
    wfTables.Participants,
    wfTables.ActivityNotifications,
    wfTables.ActivityMessages,
    wfTables.ActivityMsgRecipients,
    wfTables.ActMessages
  };
  public static string[] SchemeID_Linked_Alias = new string[9]
  {
    "data",
    "vars",
    "form",
    "att",
    "parts",
    "notifs",
    "messages",
    "msgrecips",
    "amessage"
  };
  public static string[] SchemeID_Linked_InternalIDs = new string[9]
  {
    "",
    "VarID",
    "",
    "AttachmentID",
    "ParticipantID",
    "",
    "Kind",
    "MsgKind",
    ""
  };
  public static List<string> SysVarNames = new List<string>((IEnumerable<string>) new string[13]
  {
    "SYS_STEPNAME",
    "SYS_STEPTEXT",
    "SYS_STARTER",
    "SYS_STRONGSIGN",
    "SYS_EXECSIDE",
    "SYS_PID",
    "SYS_SENDER",
    "SYS_DEFAULTFORM",
    "SYS_CHIEF",
    "SYS_BLOCKWORKCOPY",
    "SYS_USERS",
    "SYS_SUBPROCSTATUS",
    "SYS_MAKEACTUAL"
  });
  internal static VarInfo[] SysVarIDs;
  public static readonly string AttrSearchUserIDName = "SEARCH_ID_USER";
  public static int AttrSearchUserID;
  public static readonly string AttrSearchGroupIDName = "SEARCH_ID_USER_GROUP";
  public static int AttrSearchGroupID;
  public static readonly string AttrSearchArchiveIDName = "SEARCH_ID_ARCHIVE";
  public static int AttrSearchArchiveID;

  public static int SystemVarIDToNewVarID(int id)
  {
    if (id < 0)
    {
      id = -id - 1;
      if (id < wfTables.SysVarIDs.Length)
      {
        VarInfo sysVarId = wfTables.SysVarIDs[id];
        if (sysVarId != null)
          return sysVarId.AttrID;
      }
    }
    return 0;
  }

  internal static VarInfo SystemVarNameToNewVarInfo(string name)
  {
    if (name.StartsWith("SYS_"))
    {
      int index = wfTables.SysVarNames.IndexOf(name);
      if (index != -1)
        return wfTables.SysVarIDs[index];
    }
    return (VarInfo) null;
  }

  public static void Init(IUserSession sess)
  {
    wfTables.AttrSearchUserID = sess.GetAttributeType(wfTables.AttrSearchUserIDName).AttributeID;
    wfTables.AttrSearchGroupID = sess.GetAttributeType(wfTables.AttrSearchGroupIDName).AttributeID;
    wfTables.AttrSearchArchiveID = sess.GetAttributeType(wfTables.AttrSearchArchiveIDName).AttributeID;
    wfTables.SysVarIDs = new VarInfo[wfTables.SysVarNames.Count];
    Array.Clear((Array) wfTables.SysVarIDs, 0, wfTables.SysVarIDs.Length);
    Dictionary<int, string> dictionary = new Dictionary<int, string>();
    dictionary.Add(wfConsts.SysVarStarterID, "SYS_STARTER");
    dictionary.Add(wfConsts.SysVarSenderID, "SYS_SENDER");
    foreach (DataRow row in (InternalDataCollectionBase) sess.GetAttributesGroup(wfConsts.WorkflowSysVarsGroupID).Attributes.Select("", (object[]) null).Rows)
    {
      int int32 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
      string str = "";
      if (dictionary.TryGetValue(int32, out str))
      {
        int index = wfTables.SysVarNames.IndexOf(str);
        if (index != -1)
          wfTables.SysVarIDs[index] = new VarInfo(row);
      }
    }
  }
}
