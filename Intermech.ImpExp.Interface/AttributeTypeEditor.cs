// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.AttributeTypeEditor
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Summary description for ObjectTypeEditor.</summary>
public class AttributeTypeEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    IAttributeTypeToCreateList service = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    if (value is AttributeTypeAttProxy && ((AttributeTypeAttProxy) value).AttributeType != Guid.Empty)
      service.SelectDialog.SelectedItemGUID = ((AttributeTypeAttProxy) value).AttributeType;
    if (service.SelectDialog.ShowDialog() != DialogResult.OK || !(service.SelectDialog.SelectedItemGUID != ((AttributeTypeAttProxy) value).AttributeType))
      return value;
    IAttributeTypeToCreate byGuid1 = service.GetByGuid(service.SelectDialog.SelectedItemGUID);
    IAttributeTypeToCreate byGuid2 = service.GetByGuid(((AttributeTypeAttProxy) value).AttributeType);
    if (byGuid1.FieldType != byGuid2.FieldType)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      List<FieldTypes> convertList = new List<FieldTypes>();
      RelationalOperators[] enabledOperators = (RelationalOperators[]) null;
      bool computableAttribute = false;
      AttributeCacheHelper.GetAttributeTypeValues(byGuid2.FieldType, -1, ref empty1, ref empty3, ref convertList, ref enabledOperators, ref computableAttribute, ref empty2);
      if (!convertList.Contains(byGuid1.FieldType))
      {
        int num = (int) MessageBox.Show("Недопустимое преобразование типов данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return value;
      }
    }
    return byGuid1.FieldType == FieldTypes.ftString && byGuid1.Size < byGuid2.Size && MessageBox.Show("Возможна потеря данных в связи с разными длинами значений атрибутов. Продолжить ?", "Преобразование типов данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.OK ? value : (object) new AttributeTypeAttProxy(service.SelectDialog.SelectedItemGUID, service.SelectDialog.SelectedItemName);
  }
}
