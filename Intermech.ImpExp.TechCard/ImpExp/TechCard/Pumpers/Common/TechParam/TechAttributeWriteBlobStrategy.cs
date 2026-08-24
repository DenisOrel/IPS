// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriteBlobStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftBlob, FieldTypes.ftShortBlob})]
internal class TechAttributeWriteBlobStrategy : TechAttributeWriteStrategy
{
  public override bool Write(
    PumpClass pumper,
    IImportedAttributeList importedList,
    ITechParamAttribute techAttribute,
    out string errorMessage)
  {
    errorMessage = string.Empty;
    if (techAttribute.Value is FileInfo fileInfo)
    {
      try
      {
        importedList.AddAttributeBlob(techAttribute.AttributeType.ID, fileInfo.FullName, fileInfo.Length, "", ArcMethods.NotPacked, techAttribute.Index);
        return true;
      }
      catch (Exception ex)
      {
        errorMessage = $"Невозможно создать атрибут типа Blob по причине: {ex.Message}{Environment.NewLine + ex.StackTrace}";
        if (ex is OutOfMemoryException)
          throw;
      }
    }
    errorMessage = $"Невозможно создать атрибут типа Blob для файла: {techAttribute.Value} отсутствует описание файла с данными";
    return false;
  }
}
