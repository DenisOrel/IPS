// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonElementWithItemCollectionDesigner
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.ComponentModel.Design;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal abstract class RibbonElementWithItemCollectionDesigner : ComponentDesigner
{
  public abstract RibbonItemCollection Collection { get; }

  public abstract Ribbon Ribbon { get; }

  protected virtual DesignerVerbCollection OnGetVerbs()
  {
    return new DesignerVerbCollection(new DesignerVerb[1]
    {
      new DesignerVerb("Add Button", new EventHandler(this.AddButton))
    });
  }

  public override DesignerVerbCollection Verbs => this.OnGetVerbs();

  protected virtual void AddButton(object sender, EventArgs e)
  {
    this.CreateItem(typeof (RibbonButton));
  }

  private void CreateItem(Type t) => this.CreateItem(this.Ribbon, this.Collection, t);

  protected virtual void CreateItem(Ribbon ribbon, RibbonItemCollection collection, Type t)
  {
    if (!(this.GetService(typeof (IDesignerHost)) is IDesignerHost service) || collection == null || ribbon == null)
      return;
    DesignerTransaction transaction = service.CreateTransaction("AddRibbonItem_" + this.Component.Site.Name);
    MemberDescriptor property = (MemberDescriptor) TypeDescriptor.GetProperties((object) this.Component)["Items"];
    this.RaiseComponentChanging(property);
    RibbonItem component = service.CreateComponent(t) as RibbonItem;
    component.Text = component.Site.Name;
    collection.Add(component);
    ribbon.OnRegionsChanged();
    this.RaiseComponentChanged(property, (object) null, (object) null);
    transaction.Commit();
  }
}
