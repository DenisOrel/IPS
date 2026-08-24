// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.PumpHelper
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Workflow;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class PumpHelper : BasePumpHelper
{
  private static CacheCategory _archivesCache = (CacheCategory) null;
  private static Dictionary<long, string> _objCaptions = new Dictionary<long, string>();
  private static CacheCategory _objectGuids = (CacheCategory) null;
  public static WorkflowPlugin Plugin;

  public static void Init(WorkflowPlugin plugin)
  {
    BasePumpHelper.Init((PluginClass) plugin);
    PumpHelper.Plugin = plugin;
    PumpHelper._archivesCache = PumpCache.Category[ImportingCategory.Archives];
    PumpHelper._objectGuids = PumpCache.Category[ImportingCategory.ObjectGUIDs];
  }

  public static void MakeUserLink(IImportedObjectList writer, int oldID, int AttrID)
  {
    DictionaryValue dictionaryValue = BasePumpHelper._usersCache.GetValue((object) oldID);
    if (dictionaryValue != null)
      writer.AddAttributeLink(AttrID, dictionaryValue.NewObjectID, dictionaryValue.Caption);
    else
      BasePumpHelper.AddWarning(BasePumpHelper.WarningType.User, "Пользователь со старым идентификатором \"{0}\" не найден", (long) oldID);
  }

  public static void AddGroup(ParticipantList pl, int oldGroupID)
  {
    if (oldGroupID < 100)
    {
      long newRankId = BasePumpHelper.GetNewRankID(-oldGroupID - 100);
      if (newRankId <= 0L)
        return;
      pl.AddParticipant(ParticipantKind.Rank, newRankId);
    }
    else
    {
      long newGroupId = BasePumpHelper.GetNewGroupID(oldGroupID);
      if (newGroupId <= 0L)
        return;
      pl.AddParticipant(ParticipantKind.Group, newGroupId);
    }
  }

  public static Guid GetNewArchiveGuid(int oldID)
  {
    DictionaryValue dictionaryValue1 = PumpHelper._archivesCache.GetValue((object) oldID);
    if (dictionaryValue1 != null && dictionaryValue1.Tag is Archive)
    {
      DictionaryValue dictionaryValue2 = PumpHelper._objectGuids.GetValue((object) dictionaryValue1.NewObjectID);
      if (dictionaryValue2 != null)
      {
        if (GuidHelper.IsGuid(dictionaryValue2.Caption))
          return new Guid(dictionaryValue2.Caption);
        BasePumpHelper.AddWarning(BasePumpHelper.WarningType.Archive, $"Архив со старым идентификатором \"{{0}}\"  имеет неопределенный GUID =\"{dictionaryValue2.Caption}\" ", (long) oldID);
        return Guid.Empty;
      }
    }
    BasePumpHelper.AddWarning(BasePumpHelper.WarningType.Archive, "Архив со старым идентификатором \"{0}\" не найден", (long) oldID);
    return Guid.Empty;
  }

  public static PeriodInformation TermToPeriodInformation(string value)
  {
    string[] strArray1 = value.Split(':');
    if (strArray1.Length < 2)
      return (PeriodInformation) null;
    string[] strArray2 = strArray1[1].Split('@');
    if (strArray2.Length < 2)
      return (PeriodInformation) null;
    PeriodInformation periodInformation = new PeriodInformation(BasePumpHelper.Session);
    try
    {
      if (strArray1[0] == "2")
      {
        int newVarTypeId = VarsPump.GetNewVarTypeID(strArray2[0], -1);
        periodInformation.VarTypeID = newVarTypeId;
      }
      else
      {
        periodInformation.UnitsCount = Convert.ToInt32(strArray2[0]);
        periodInformation.Units = (TimeUnits) Convert.ToInt32(strArray2[1]);
      }
    }
    catch
    {
    }
    return periodInformation;
  }

  public static TempFormula CreateTempFormula(string expr)
  {
    TempFormula tempFormula = new TempFormula();
    tempFormula.Init();
    Guid empty = Guid.Empty;
    string errorMsg = "";
    expr = expr.Replace('\'', '"');
    Tokenizer tokenizer = new Tokenizer(expr);
    while (!tokenizer.EOS)
    {
      errorMsg = tokenizer.NextToken();
      errorMsg.ToLower();
      bool flag = false;
      if (errorMsg.Length > 0 && (char.IsLetterOrDigit(errorMsg[0]) || errorMsg[0] == '_') && VarsPump.GetNewVarTypeID(errorMsg, -1) > 0)
      {
        Guid attrGuid = VarsPump.LastVarInfo.AttrGuid;
        tempFormula.AddAttributeToken(BasePumpHelper.Session, attrGuid, Guid.Empty);
        flag = true;
      }
      if (!flag)
        tempFormula.AddToken(new Token(errorMsg));
    }
    tempFormula.Compile(out int _, out errorMsg);
    return tempFormula;
  }
}
