// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.RelationGuidTypeEditor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

internal class RelationGuidTypeEditor : UITypeEditor
{
  protected bool _anyRelType;

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context != null ? UITypeEditorEditStyle.Modal : base.GetEditStyle(context);
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (context == null || provider == null)
      return base.EditValue(context, provider, value);
    if (provider.GetService(typeof (IWindowsFormsEditorService)) is IWindowsFormsEditorService service1 && value is Guid guid)
    {
      IMetadataInfo service = (IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo));
      using (RelationTypeSelectorForm dialog = new RelationTypeSelectorForm("Выберите тип cвязи", this._anyRelType))
      {
        IRelationTypeItem byGuid = guid != Guid.Empty ? service.RelationTypes.GetByGuid(guid) : (IRelationTypeItem) null;
        if (service1.ShowDialog((Form) dialog).Equals((object) DialogResult.OK))
        {
          IRelationTypeItem byId = service.RelationTypes.GetByID(dialog.RelType);
          return (object) (byId != null ? byId.GUID : Guid.Empty);
        }
      }
    }
    return value;
  }
}
