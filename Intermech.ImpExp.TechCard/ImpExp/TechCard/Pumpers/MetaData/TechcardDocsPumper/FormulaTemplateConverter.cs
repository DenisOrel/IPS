// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper.FormulaTemplateConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.TechCard.Document.Interfaces.Configs.Structure;
using Intermech.TechCard.Document.Interfaces.Configs.Structure.FieldContents;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper;

internal static class FormulaTemplateConverter
{
  public static AttributeSettings EntityToAttribute(string techcardEntity, PluginClass plugin)
  {
    AttributeSettings attribute = (AttributeSettings) null;
    IAttributeTypeItem attributeItemByCode = TechExpert.TypeConverter.GetAttributeItemByCode(techcardEntity, plugin, out string _);
    if (attributeItemByCode == null)
      return (AttributeSettings) null;
    Entity entity;
    if (TechPumpData.Entities.EntitiesList.TryGetValue(techcardEntity, out entity))
    {
      if (entity == null)
        return (AttributeSettings) null;
      if (entity.Settings.AttributeBelong == EntitySetting.AttributeBelongs.ToObject || entity.Settings.AttributeBelong == EntitySetting.AttributeBelongs.ToLinkAndObject)
      {
        if (entity.Settings.ObjectType != Guid.Empty)
        {
          attribute = new AttributeSettings(AttributableElements.Object, entity.Settings.ObjectType, attributeItemByCode.GUID);
        }
        else
        {
          TechTypeInfo typeRecByRecordId = TechPumpData.TechType.TechTypeList.GetTypeRecByRecordId(entity.RecordID);
          if (typeRecByRecordId != null)
            attribute = new AttributeSettings(AttributableElements.Object, typeRecByRecordId.TypeSett.ObjType, attributeItemByCode.GUID);
        }
      }
      else
        attribute = new AttributeSettings(AttributableElements.Relation, new Guid("cad0019f-306c-11d8-b4e9-00304f19f545"), attributeItemByCode.GUID);
    }
    else
      attribute = new AttributeSettings(AttributableElements.Object, Guid.Empty, attributeItemByCode.GUID);
    return attribute;
  }

  public static IFieldContents ConvertTemplate(string originTechcardTemplate, PluginClass plugin)
  {
    List<AttributeSettings> attributeSettingsList = new List<AttributeSettings>();
    int num = originTechcardTemplate.IndexOf('[');
    if (num < 0)
      return (IFieldContents) new TemplateFieldContents(originTechcardTemplate, (IEnumerable<AttributeSettings>) attributeSettingsList);
    StringBuilder stringBuilder = new StringBuilder();
    if (num > 0)
      stringBuilder.Append('\'').Append(originTechcardTemplate.Substring(0, num)).Append('\'').Append('+');
    int startIndex = -1;
    while (num >= 0)
    {
      startIndex = originTechcardTemplate.IndexOf(']', num);
      if (startIndex == -1)
      {
        stringBuilder.Append(originTechcardTemplate.Substring(num, originTechcardTemplate.Length - num - 1));
        break;
      }
      string techcardEntity = originTechcardTemplate.Substring(num + 1, startIndex - num - 1);
      AttributeSettings attribute = FormulaTemplateConverter.EntityToAttribute(techcardEntity, plugin);
      if (attribute != null && !attributeSettingsList.Contains<AttributeSettings>((Predicate<AttributeSettings>) (attr => attr.AttributeGuid == attribute.AttributeGuid)))
        attributeSettingsList.Add(attribute);
      if (attribute != null)
        stringBuilder.Append('[').Append((object) attribute).Append(']').Append('+');
      else
        stringBuilder.Append('\'').Append('[').Append(techcardEntity).Append(']').Append('\'').Append('+');
      num = originTechcardTemplate.IndexOf('[', startIndex);
      if (num >= 0 && num - startIndex > 1)
        stringBuilder.Append('\'').Append(originTechcardTemplate.Substring(startIndex + 1, num - startIndex - 1)).Append('\'').Append('+');
    }
    if (startIndex >= 0 && startIndex < originTechcardTemplate.Length - 1)
      stringBuilder.Append('\'').Append(originTechcardTemplate.Substring(startIndex + 1, originTechcardTemplate.Length - startIndex - 1)).Append('\'');
    else if (stringBuilder[stringBuilder.Length - 1] == '+')
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
    return (IFieldContents) new TemplateFieldContents(stringBuilder.ToString(), (IEnumerable<AttributeSettings>) attributeSettingsList);
  }
}
