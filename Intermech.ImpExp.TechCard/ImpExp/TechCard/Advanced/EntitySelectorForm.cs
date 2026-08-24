// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.EntitySelectorForm
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

internal class EntitySelectorForm : TypeSelectorForm
{
  private List<Entity> _entityList;
  private FieldTypes _fieldType;
  private Entity _excludeEntity;
  private IContainer components;

  private Entity GetSelectedItem()
  {
    TreeNode selectedNode = this.tvType.SelectedNode;
    return selectedNode == null ? (Entity) null : selectedNode.Tag as Entity;
  }

  public EntitySelectorForm(
    string caption,
    List<Entity> entityList,
    FieldTypes fieldType,
    Entity excludeEntity)
    : base(new object[3]
    {
      (object) entityList,
      (object) fieldType,
      (object) excludeEntity
    })
  {
    this.InitializeData();
    this.Text = caption;
  }

  public override object SelectedItem => (object) this.GetSelectedItem();

  protected override void SetParams(object[] data, string caption)
  {
    base.SetParams(data, caption);
    if (data == null || data.Length <= 2)
      return;
    this._entityList = (List<Entity>) data[0];
    this._fieldType = (FieldTypes) data[1];
    this._excludeEntity = (Entity) data[2];
  }

  private void InitializeData()
  {
    this.Name = nameof (EntitySelectorForm);
    this.lblInfo.Text = "Выберите понятие";
  }

  protected override void LoadTypesTree()
  {
    this.tvType.BeginUpdate();
    try
    {
      this.tvType.Nodes.Clear();
      if ((IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo)) == null)
        return;
      foreach (Entity entity in this._entityList)
      {
        if ((entity.Settings.PumpMode == EntityPumModes.NewAttr || entity.Settings.Properties.Status != EntityPumpStatus.None && entity.Settings.Properties.Status != EntityPumpStatus.NotPump) && (this._excludeEntity == null || !(entity.Code == this._excludeEntity.Code)) && entity.Settings.Properties.FieldType.Equals((object) this._fieldType))
          this.tvType.Nodes.Add(new TreeNode(entity.ToString())
          {
            Tag = (object) entity
          });
      }
    }
    finally
    {
      this.tvType.EndUpdate();
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent() => this.ResumeLayout(false);
}
