// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonDesigner
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonDesigner : ControlDesigner
{
  public static RibbonDesigner Current;
  private IRibbonElement _selectedElement;

  public static int HiWord(int dwValue) => dwValue >> 16 /*0x10*/ & (int) ushort.MaxValue;

  public static int LoWord(int dwValue) => dwValue & (int) ushort.MaxValue;

  public Ribbon Ribbon => this.Control as Ribbon;

  public IRibbonElement SelectedElement
  {
    get => this._selectedElement;
    set
    {
      this._selectedElement = value;
      if (this.GetService(typeof (ISelectionService)) is ISelectionService service && value != null)
      {
        System.ComponentModel.Component[] components = new System.ComponentModel.Component[1]
        {
          value as System.ComponentModel.Component
        };
        service.SetSelectedComponents((ICollection) components, SelectionTypes.Click);
      }
      if (value is RibbonButton ribbonButton)
        ribbonButton.ShowDropDown();
      this.Ribbon.Refresh();
    }
  }

  public RibbonDesigner() => RibbonDesigner.Current = this;

  ~RibbonDesigner()
  {
    if (RibbonDesigner.Current != this)
      return;
    RibbonDesigner.Current = (RibbonDesigner) null;
  }

  public override DesignerVerbCollection Verbs
  {
    get
    {
      return new DesignerVerbCollection()
      {
        new DesignerVerb("Add Tab", new EventHandler(this.AddTabVerb))
      };
    }
  }

  protected override void OnPaintAdornments(PaintEventArgs pe)
  {
    base.OnPaintAdornments(pe);
    using (Pen pen = new Pen(Color.Black))
    {
      pen.DashStyle = DashStyle.Dot;
      if (!(this.GetService(typeof (ISelectionService)) is ISelectionService service))
        return;
      foreach (IComponent selectedComponent in (IEnumerable) service.GetSelectedComponents())
      {
        if (selectedComponent is RibbonItem ribbonItem)
          pe.Graphics.DrawRectangle(pen, ribbonItem.Bounds);
      }
    }
  }

  protected override void WndProc(ref Message m)
  {
    if (m.HWnd == this.Control.Handle)
    {
      switch (m.Msg)
      {
        case 513:
          return;
        case 514:
        case 517:
          this.HitOn(RibbonDesigner.LoWord((int) m.LParam), RibbonDesigner.HiWord((int) m.LParam));
          return;
        case 516:
          return;
      }
    }
    base.WndProc(ref m);
  }

  private void AddTabVerb(object sender, EventArgs e)
  {
    if (!(this.Control is Ribbon control) || !(this.GetService(typeof (IDesignerHost)) is IDesignerHost service) || !(service.CreateComponent(typeof (RibbonTab)) is RibbonTab component))
      return;
    component.Text = component.Site.Name;
    this.Ribbon.Tabs.Add(component);
    control.Refresh();
  }

  private void HitOn(int x, int y)
  {
    if (this.Ribbon == null)
      return;
    if (this.Ribbon.Tabs.Count == 0 || this.Ribbon.ActiveTab == null)
      this.SelectRibbon();
    if (this.Ribbon.TabHitTest(x, y))
    {
      this.SelectedElement = (IRibbonElement) this.Ribbon.ActiveTab;
    }
    else
    {
      if (this.Ribbon.ActiveTab.TabContentBounds.Contains(x, y))
      {
        if (this.Ribbon.ActiveTab.ScrollLeftBounds.Contains(x, y) && this.Ribbon.ActiveTab.ScrollLeftVisible)
        {
          this.Ribbon.ActiveTab.ScrollLeft();
          this.SelectedElement = (IRibbonElement) this.Ribbon.ActiveTab;
          return;
        }
        if (this.Ribbon.ActiveTab.ScrollRightBounds.Contains(x, y) && this.Ribbon.ActiveTab.ScrollRightVisible)
        {
          this.Ribbon.ActiveTab.ScrollRight();
          this.SelectedElement = (IRibbonElement) this.Ribbon.ActiveTab;
          return;
        }
      }
      if (this.Ribbon.ActiveTab.TabContentBounds.Contains(x, y))
      {
        RibbonPanel ribbonPanel = (RibbonPanel) null;
        foreach (RibbonPanel panel in (List<RibbonPanel>) this.Ribbon.ActiveTab.Panels)
        {
          if (panel.Bounds.Contains(x, y))
          {
            ribbonPanel = panel;
            break;
          }
        }
        if (ribbonPanel != null)
        {
          RibbonItem ribbonItem1 = (RibbonItem) null;
          foreach (RibbonItem ribbonItem2 in (List<RibbonItem>) ribbonPanel.Items)
          {
            if (ribbonItem2.Bounds.Contains(x, y))
            {
              ribbonItem1 = ribbonItem2;
              break;
            }
          }
          if (ribbonItem1 != null && ribbonItem1 is IContainsSelectableRibbonItems)
          {
            RibbonItem ribbonItem3 = (RibbonItem) null;
            foreach (RibbonItem ribbonItem4 in (ribbonItem1 as IContainsSelectableRibbonItems).GetItems())
            {
              if (ribbonItem4.Bounds.Contains(x, y))
              {
                ribbonItem3 = ribbonItem4;
                break;
              }
            }
            this.SelectedElement = (IRibbonElement) (ribbonItem3 ?? ribbonItem1);
          }
          else if (ribbonItem1 != null)
            this.SelectedElement = (IRibbonElement) ribbonItem1;
          else
            this.SelectedElement = (IRibbonElement) ribbonPanel;
        }
        else
          this.SelectedElement = (IRibbonElement) this.Ribbon.ActiveTab;
      }
      else
        this.SelectRibbon();
    }
  }

  private void SelectRibbon()
  {
    if (!(this.GetService(typeof (ISelectionService)) is ISelectionService service))
      return;
    System.ComponentModel.Component[] components = new System.ComponentModel.Component[1]
    {
      (System.ComponentModel.Component) this.Ribbon
    };
    service.SetSelectedComponents((ICollection) components, SelectionTypes.Click);
  }

  public BehaviorService GetBehaviorService() => this.BehaviorService;

  public override void Initialize(IComponent component)
  {
    base.Initialize(component);
    (this.GetService(typeof (IComponentChangeService)) as IComponentChangeService).ComponentRemoved += new ComponentEventHandler(this.OnchangeService_ComponentRemoved);
  }

  public void OnchangeService_ComponentRemoved(object sender, ComponentEventArgs e)
  {
    RibbonTab component1 = e.Component as RibbonTab;
    RibbonPanel component2 = e.Component as RibbonPanel;
    RibbonItem component3 = e.Component as RibbonItem;
    IDesignerHost service = this.GetService(typeof (IDesignerHost)) as IDesignerHost;
    if (component1 != null)
      this.Ribbon.Tabs.Remove(component1);
    else if (component2 != null)
      component2.OwnerTab.Panels.Remove(component2);
    else if (component3 != null && component3.OwnerPanel != null)
      component3.OwnerPanel.Items.Remove(component3);
    this.RemoveRecursive(e.Component as IContainsRibbonComponents, service);
    this.SelectedElement = (IRibbonElement) null;
    this.Ribbon.OnRegionsChanged();
  }

  public void RemoveRecursive(IContainsRibbonComponents item, IDesignerHost service)
  {
    if (item == null || service == null)
      return;
    foreach (System.ComponentModel.Component allChildComponent in item.GetAllChildComponents())
    {
      if (allChildComponent is IContainsRibbonComponents ribbonComponents)
        this.RemoveRecursive(ribbonComponents, service);
      service.DestroyComponent((IComponent) allChildComponent);
    }
  }

  public virtual void CreateItem(Ribbon ribbon, RibbonItemCollection collection, Type t)
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
