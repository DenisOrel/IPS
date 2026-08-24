// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.VarsPump
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Interfaces;
using Intermech.Workflow;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Workflow;

public static class VarsPump
{
  private static Dictionary<string, VarInfo> VarInfos = new Dictionary<string, VarInfo>();
  private static WorkflowPlugin _plugin = (WorkflowPlugin) null;
  private static Dictionary<string, string> _renamedCache = new Dictionary<string, string>();
  public static VarType LastVarType = VarType.Unknown;
  public static string LastVarName = "";
  internal static VarInfo LastVarInfo = (VarInfo) null;

  internal static void Init(WorkflowPlugin plugin, IUserSession sess)
  {
    VarsPump._plugin = plugin;
    DataTable dataTable = sess.GetAttributesGroup(wfConsts.WorkflowVarsGroupID).Attributes.Select("", (object[]) null);
    StringList stringList = new StringList();
    foreach (DataRow row1 in (InternalDataCollectionBase) dataTable.Rows)
    {
      VarInfo varInfo = new VarInfo(row1);
      IDBAttributeType attributeType = sess.GetAttributeType(varInfo.AttrID, false);
      if (attributeType != null)
      {
        if (attributeType.MultipleValued != MultiValueModes.SingleValue)
        {
          stringList.Clear();
          foreach (DataRow row2 in (InternalDataCollectionBase) attributeType.GetPossibleValues().Rows)
            stringList.Add(row2[1].ToString());
          varInfo.PossibleValues = stringList.Text;
        }
        VarsPump.VarInfos.Add(varInfo.Name.ToUpper(), varInfo);
      }
    }
  }

  public static VarType IntToVarType(int type)
  {
    switch (type)
    {
      case 1:
        type = 2;
        break;
      case 2:
      case 3:
        type = 3;
        break;
      case 6:
        type = 7;
        break;
    }
    return (VarType) type;
  }

  private static int CreateNewVarTypeID(string Name, VarType type, string possibleValues)
  {
    if (VarsPump._plugin.Imdi.AttributeTypes.ExistsByName(Name))
      return -1;
    IUserSession userSession = VarsPump._plugin.Idw.GetUserSession();
    int variableType = VarsHelper.CreateVariableType(userSession, Name, type);
    IDBAttributeType attributeType = userSession.GetAttributeType(variableType);
    if (possibleValues != null)
    {
      StringList stringList = new StringList();
      using (DataTable possibleValues1 = attributeType.GetPossibleValues())
      {
        stringList.Text = possibleValues;
        possibleValues1.Rows.Clear();
        for (int index = 0; index < stringList.Count; ++index)
          possibleValues1.Rows.Add((object) index, (object) stringList[index], (object) "");
        attributeType.SetPossibleValues(possibleValues1);
      }
    }
    VarInfo varInfo = new VarInfo(Name, variableType, type, attributeType.PropertiesStructure.AttributeGuid);
    varInfo.PossibleValues = possibleValues;
    VarsPump.VarInfos.Add(varInfo.Name.ToUpper(), varInfo);
    VarsPump.LastVarType = type;
    VarsPump.LastVarName = Name;
    VarsPump.LastVarInfo = varInfo;
    return variableType;
  }

  public static int GetNewVarTypeID(string Name, int VType)
  {
    return VarsPump.GetNewVarTypeID(Name, VType, 0);
  }

  public static int GetNewVarTypeID(string Name, int VType, string possibleValues)
  {
    return VarsPump.GetNewVarTypeID(Name, VType, 0, possibleValues);
  }

  public static int GetNewVarTypeID(string Name, int VType, int suffix)
  {
    return VarsPump.GetNewVarTypeID(Name, VType, suffix, (string) null);
  }

  public static int GetNewVarTypeID(string Name, int VType, int suffix, string possibleValues)
  {
    if (suffix == 0)
    {
      VarInfo newVarInfo = wfTables.SystemVarNameToNewVarInfo(Name);
      if (newVarInfo != null)
      {
        VarsPump.LastVarType = newVarInfo.Type;
        VarsPump.LastVarName = Name;
        VarsPump.LastVarInfo = newVarInfo;
        return newVarInfo.AttrID;
      }
    }
    string Name1 = Name;
    if (suffix > 0)
      Name1 = $"{Name1}_{suffix.ToString()}";
    else if (VarsPump._renamedCache.ContainsKey(Name))
      Name1 = VarsPump._renamedCache[Name];
    VarType type = VarType.Unknown;
    VarsPump.LastVarInfo = (VarInfo) null;
    if (VType != -1)
      type = VarsPump.IntToVarType(VType);
    string upper = Name1.ToUpper();
    int newVarTypeId;
    if (VarsPump.VarInfos.ContainsKey(upper))
    {
      VarInfo varInfo = VarsPump.VarInfos[upper];
      if ((VType == -1 || varInfo.Type == type) && (possibleValues == null || varInfo.PossibleValues == possibleValues))
      {
        VarsPump.LastVarType = varInfo.Type;
        VarsPump.LastVarName = Name1;
        VarsPump.LastVarInfo = varInfo;
        return varInfo.AttrID;
      }
      newVarTypeId = VarsPump.GetNewVarTypeID(Name, VType, suffix + 1, possibleValues);
    }
    else
    {
      if (VType == -1)
        return 0;
      newVarTypeId = VarsPump.CreateNewVarTypeID(Name1, type, possibleValues);
      if (newVarTypeId == -1)
        newVarTypeId = VarsPump.GetNewVarTypeID(Name, VType, suffix + 1, possibleValues);
    }
    if (suffix == 0 && newVarTypeId > 0 && Name != VarsPump.LastVarName && !VarsPump._renamedCache.ContainsKey(Name))
      VarsPump._renamedCache.Add(Name, VarsPump.LastVarName);
    return newVarTypeId;
  }

  public static void ClearRenamedCache() => VarsPump._renamedCache.Clear();
}
