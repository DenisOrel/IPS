// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.AttributesListControlCellDataHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal abstract class AttributesListControlCellDataHandler : IAttributesListControlCellDataHandler
{
  protected abstract object Value { get; }

  protected abstract string Description { get; set; }

  protected abstract int AttributeID { get; }

  protected abstract string AttributeColumnValue { get; }

  public abstract void SetBackColor(GetCellDataEventArgs e);

  protected abstract bool IsDummyItem { get; }

  public void SetDataValue(
    GetCellDataEventArgs e,
    IAttributePropertyDescriberService propertyDescriberService,
    IElementInfo currentElementInfo)
  {
    if (this.IsDummyItem)
      return;
    if (e.Column.Name == "F_ATTRIBUTE_ID")
    {
      e.CellData.Value = (object) this.AttributeColumnValue;
    }
    else
    {
      if (!(e.Column.Name == "F_VALUE"))
        return;
      if (this.Description != null)
      {
        e.CellData.Value = this.Description != string.Empty ? (object) this.Description : this.Value;
      }
      else
      {
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(this.AttributeID);
        object pdValue;
        try
        {
          pdValue = AttributeValuesEditor.GetPDValue(attributeType.MultiValueMode, this.AttributeID, attributeType.FieldType, new object[1]
          {
            this.Value
          }, new object[1]{ (object) this.Description }, 0, currentElementInfo.ElementIdentifier, currentElementInfo.ElementKind, string.Empty, (DataTable) null);
        }
        catch
        {
          pdValue = this.Value;
        }
        this.Description = Convert.ToString(pdValue);
        e.CellData.Value = pdValue;
      }
      switch (MetaDataHelper.GetAttributeType(this.AttributeID).FieldType)
      {
        case FieldTypes.ftString:
        case FieldTypes.ftMemo:
          CellData cellData = e.CellData;
          TextBox textBox = new TextBox();
          textBox.Multiline = true;
          textBox.ScrollBars = ScrollBars.Both;
          textBox.ReadOnly = true;
          cellData.Editor = new CellEditor((Control) textBox)
          {
            DisplayMode = CellEditorDisplayMode.OnEdit
          };
          break;
      }
    }
  }
}
