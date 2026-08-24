// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

internal class TechAttributeWriter
{
  private readonly PumpClass _pumper;
  private readonly ObjectForAttributeFieldTypeService<TechAttributeWriteStrategy> _writeStrategyCache;

  private void AddWarningMessage(string message)
  {
    if (this._pumper == null || this._pumper.Plugin == null || this._pumper.Plugin.appManager == null || string.IsNullOrEmpty(message))
      return;
    this._pumper.Plugin.appManager.AddNewWarningMessage(message);
  }

  public TechAttributeWriter(
    PumpClass pumper,
    ObjectForAttributeFieldTypeService<TechAttributeWriteStrategy> writeStrategyCache)
  {
    if (writeStrategyCache == null)
      throw new ArgumentNullException(nameof (writeStrategyCache));
    this._pumper = pumper;
    this._writeStrategyCache = writeStrategyCache;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Write(IImportedAttributeList importedList, ITechParamAttribute techAttribute)
  {
    if (importedList == null)
      throw new ArgumentNullException(nameof (importedList));
    if (techAttribute == null || techAttribute.AttributeType == null)
      return;
    TechAttributeWriteStrategy attributeWriteStrategy = this._writeStrategyCache.GetObject((FieldTypes) techAttribute.AttributeType.AttrValueType);
    if (attributeWriteStrategy == null)
      return;
    try
    {
      string errorMessage;
      if (attributeWriteStrategy.Write(this._pumper, importedList, techAttribute, out errorMessage))
        return;
      this.AddWarningMessage(errorMessage);
    }
    catch (InvalidCastException ex)
    {
      this.AddWarningMessage($"Невозможно создать атрибут с идентификатором {techAttribute.AttributeType.ID} по параметру: {((ITechParamBase) techAttribute).ToString()} по причине {ex.Message}");
    }
  }
}
