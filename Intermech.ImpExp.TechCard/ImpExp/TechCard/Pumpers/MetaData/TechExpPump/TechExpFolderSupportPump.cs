// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.TechExpFolderSupportPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.TablesPump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump;

internal abstract class TechExpFolderSupportPump(PluginClass plugin) : TechExpBasePump(plugin)
{
  protected readonly IDictionary<TechExpKey, TechExpFolderObject> _techFolderCache = (IDictionary<TechExpKey, TechExpFolderObject>) new Dictionary<TechExpKey, TechExpFolderObject>();

  protected virtual bool PumpRootFolderObject(
    TechExpObject techExpObject,
    out TechExpFolderObject techExpFolder)
  {
    techExpFolder = new TechExpFolderObject(TechExpKeyConverter.ConvertTo((long) techExpObject.Key, -1L), (object) null)
    {
      Name = "." + techExpObject.Name
    };
    return this.PumpFolderObject(techExpFolder);
  }

  protected bool PumpFolderObject(TechExpFolderObject techExpFolder)
  {
    if (techExpFolder == null)
      return false;
    IDBObject dbObject = this.plugin.Idw.GetUserSession().GetObjectCollection(TechExpTablesConst.DBFolderObjectTypeID).Create();
    if (techExpFolder.Condition != null && techExpFolder.Condition.Data != null && techExpFolder.Condition.Data.Count != 0 && techExpFolder.IpsCondition != null && dbObject is IExpertFormulable expertFormulable)
      expertFormulable.UpdateObject(techExpFolder.IpsCondition);
    AttributeValues[] valuesList = new AttributeValues[1]
    {
      new AttributeValues(this._atNaimAttrTypeId, (object) techExpFolder.Name)
    };
    dbObject.SetAttributesValues(valuesList);
    if (dbObject.IsCreationMode)
      dbObject.CommitCreation(true);
    techExpFolder.ImportedObjectInfo = new QuickObjectInfo(dbObject.ObjectID, string.Empty, dbObject.ObjectType, dbObject.ObjectGUID, dbObject.ID);
    return true;
  }

  protected bool ConvertFolderCondition(
    TechExpObject expertObject,
    TechExpFolderObject techExpFolder)
  {
    if (expertObject == null)
      throw new ArgumentNullException(nameof (expertObject));
    if (techExpFolder == null)
      throw new ArgumentNullException(nameof (techExpFolder));
    if (techExpFolder.Condition == null || techExpFolder.Condition.Data == null || techExpFolder.Condition.Data.Count == 0)
      return false;
    TempFormula ipsFormulaData = (TempFormula) null;
    try
    {
      this.ConvertExpertData(techExpFolder.Condition.ResType, techExpFolder.Condition.Data, techExpFolder.Condition.ID, out ipsFormulaData);
      techExpFolder.IpsCondition = ipsFormulaData;
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case TokenConvertException _:
        case CommonDataTypeCheckFailException _:
        case CommonDataTypeConvertException _:
        case EntitySettNotExistException _:
        case FormulaConvertException _:
          this.plugin.appManager.AddWarningMessage(ex.Message);
          break;
        case FormulaCompileException _:
          string message = ex.Message;
          this.plugin.appManager.AddWarningMessage($"Ошибка компиляции формул. Файл \"{expertObject.Name}\", группа \"{techExpFolder.Name}\" Сообщение: {message}");
          break;
        default:
          throw;
      }
    }
    return techExpFolder.IpsCondition != null;
  }
}
