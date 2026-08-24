// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriteMemoStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftMemo})]
internal class TechAttributeWriteMemoStrategy : TechAttributeWriteStrategy
{
  public override bool Write(
    PumpClass pumper,
    IImportedAttributeList importedList,
    ITechParamAttribute techAttribute,
    out string errorMessage)
  {
    errorMessage = string.Empty;
    string tmpFileName = TechUtils.File.GetTmpFileName(pumper.PumperGuid);
    try
    {
      string contents = techAttribute.Value.ToString();
      System.IO.File.WriteAllText(tmpFileName, contents);
      importedList.AddAttributeBlob(techAttribute.AttributeType.ID, tmpFileName, (long) contents.Length, "Text", ArcMethods.NotPacked, techAttribute.Index);
      return true;
    }
    catch (Exception ex)
    {
      errorMessage = $"Ошибка создания записи атрибута-текста {((ITechParamBase) techAttribute).ToString()}: {ex.Message}{Environment.NewLine + ex.StackTrace}";
      if (ex is OutOfMemoryException)
        throw;
    }
    return false;
  }
}
