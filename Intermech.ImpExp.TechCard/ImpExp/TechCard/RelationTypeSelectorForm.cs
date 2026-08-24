// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.RelationTypeSelectorForm
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard;

public class RelationTypeSelectorForm : TypeSelectorForm
{
  private bool _anyRelType = true;

  private void InitializeData() => this.Name = nameof (RelationTypeSelectorForm);

  public override object SelectedItem => (object) this.RelType;

  protected override void SetParams(object[] data, string caption)
  {
    base.SetParams(data, caption);
    if (data == null || data.Length == 0)
      return;
    this._anyRelType = (bool) data[0];
  }

  protected override void LoadTypesTree()
  {
    this.tvType.BeginUpdate();
    try
    {
      this.tvType.Nodes.Clear();
      IMetadataInfo service = (IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo));
      if (service == null)
        return;
      if (this._anyRelType)
      {
        TreeNode node = new TreeNode("Любой тип связи");
        foreach (IRelationTypeItem relationType in (IEnumerable<IRelationTypeItem>) service.RelationTypes)
          node.Nodes.Add(new TreeNode(relationType.Name)
          {
            Tag = (object) relationType
          });
        node.Expand();
        this.tvType.Nodes.Add(node);
      }
      else
      {
        foreach (IRelationTypeItem relationType in (IEnumerable<IRelationTypeItem>) service.RelationTypes)
          this.tvType.Nodes.Add(new TreeNode(relationType.Name)
          {
            Tag = (object) relationType
          });
      }
    }
    finally
    {
      this.tvType.EndUpdate();
    }
  }

  public RelationTypeSelectorForm(string caption, bool anyRelType)
    : base(new object[1]{ (object) anyRelType }, caption)
  {
    this.InitializeData();
  }

  public int RelType => this.tvType.SelectedNode?.Tag is IRelationTypeItem tag ? tag.ID : -1;
}
