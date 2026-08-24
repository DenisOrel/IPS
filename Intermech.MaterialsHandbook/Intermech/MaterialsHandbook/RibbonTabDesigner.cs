// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonTabDesigner
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.ComponentModel.Design;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonTabDesigner : ComponentDesigner
{
  public override DesignerVerbCollection Verbs
  {
    get
    {
      return new DesignerVerbCollection(new DesignerVerb[1]
      {
        new DesignerVerb("Add Panel", new EventHandler(this.AddPanel))
      });
    }
  }

  public RibbonTab Tab => this.Component as RibbonTab;

  public void AddPanel(object sender, EventArgs e)
  {
    if (!(this.GetService(typeof (IDesignerHost)) is IDesignerHost service) || this.Tab == null)
      return;
    DesignerTransaction transaction = service.CreateTransaction(nameof (AddPanel) + this.Component.Site.Name);
    MemberDescriptor property = (MemberDescriptor) TypeDescriptor.GetProperties((object) this.Component)["Panels"];
    this.RaiseComponentChanging(property);
    if (service.CreateComponent(typeof (RibbonPanel)) is RibbonPanel component)
    {
      component.Text = component.Site.Name;
      this.Tab.Panels.Add(component);
      this.Tab.Owner.OnRegionsChanged();
    }
    this.RaiseComponentChanged(property, (object) null, (object) null);
    transaction.Commit();
  }
}
