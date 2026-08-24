// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ConfigSettingsForAttributesUITypeEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ConfigSettingsForAttributesUITypeEditor : UITypeEditor
{
  private Guid _g = Guid.Empty;

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (value != null)
    {
      string str = Convert.ToString(value);
      if (GuidHelper.IsGuid(str))
        this._g = new Guid(str);
    }
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.ForbiddenAttrsTypesFilter.AddRange((IEnumerable<FieldTypes>) new FieldTypes[4]
      {
        FieldTypes.ftBlob,
        FieldTypes.ftShortBlob,
        FieldTypes.ftFile,
        FieldTypes.ftSystem
      });
      if (this._g != Guid.Empty)
      {
        int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._g);
        attributesSelectDlg.SelectedAttributeIDOnStartup(attributeTypeId);
      }
      if (attributesSelectDlg.ShowDialog() == DialogResult.OK)
      {
        if (attributesSelectDlg.SelectedAttributesGuid.Count > 0)
          value = (object) attributesSelectDlg.SelectedAttributesGuid[0];
      }
    }
    return value;
  }
}
