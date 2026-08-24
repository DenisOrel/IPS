// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.AttrsRelationshipEditor
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

internal class AttrsRelationshipEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context.PropertyDescriptor != null && context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (value != null && value != DBNull.Value && value is AttrsRelationshipPropertyClass)
    {
      AttrsRelationshipEditorForm relationshipEditorForm = new AttrsRelationshipEditorForm((AttrsRelationshipPropertyClass) value);
      if (relationshipEditorForm.ShowDialog() == DialogResult.OK)
        return (object) new AttrsRelationshipPropertyClass(relationshipEditorForm.RelatingAttrId, relationshipEditorForm.RelatedAttrId, relationshipEditorForm.ObjectId);
    }
    else if (context.Instance is AttributeValuesPropertyClass instance)
    {
      if (instance.AttributeValue.Values.Length != 0 && instance.AttributeValue.Values[0] != null)
      {
        AttrsRelationshipEditorForm relationshipEditorForm = new AttrsRelationshipEditorForm(new AttrsRelationshipPropertyClass(new AttrsRelationshipPropertyClass(instance.AttributeValue.Values[0].ToString()).ObjectId));
        if (relationshipEditorForm.ShowDialog() == DialogResult.OK)
          return (object) new AttrsRelationshipPropertyClass(relationshipEditorForm.RelatingAttrId, relationshipEditorForm.RelatedAttrId, relationshipEditorForm.ObjectId);
      }
      else
      {
        AttrsRelationshipEditorForm relationshipEditorForm = new AttrsRelationshipEditorForm();
        if (relationshipEditorForm.ShowDialog() == DialogResult.OK)
          return (object) new AttrsRelationshipPropertyClass(relationshipEditorForm.RelatingAttrId, relationshipEditorForm.RelatedAttrId, relationshipEditorForm.ObjectId);
      }
    }
    return value;
  }
}
