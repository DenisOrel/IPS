// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.AutoSel.AutoSelectNode
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.AutoSelection.Client.AutoSelectionNode;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.AutoSel;

public class AutoSelectNode
{
  private List<long> _anketaRows = new List<long>();
  private AutoSelectionNodeCondList _anketa;
  private int _ctlCondKey;
  private int _ctlKey;
  private int _tableId;
  private Guid _ipsObjectTypeGuid = Guid.Empty;
  private bool _autoSelect;
  private AutoSelectNode.NodeFlags _nodeFlag;
  private string _name = string.Empty;
  private int _order;
  private int _key;
  private AutoSelectNode _parent;
  private List<AutoSelectNode> _childs = new List<AutoSelectNode>();
  private long _forCtl;
  private long _fromCtl;
  private int _orderKey;
  public Guid Procedure = Guid.Empty;

  public AutoSelectNode(
    string name,
    int key,
    AutoSelectNode.NodeFlags flag,
    int order,
    int ctlKey)
  {
    this._name = name;
    this._key = key;
    this._nodeFlag = flag;
    this._order = order;
    this._ctlKey = ctlKey;
  }

  public void AddNode(AutoSelectNode chNode, int chKey)
  {
    foreach (AutoSelectNode child in this.Childs)
      child.AddNode(chNode, chKey);
    if (this._key != chKey)
      return;
    this.Childs.Add(chNode);
    chNode._parent = this;
  }

  public int OrderKey
  {
    get => this._orderKey;
    set => this._orderKey = value;
  }

  public int TableId
  {
    get => this._tableId;
    set => this._tableId = value;
  }

  public bool AutoSelect
  {
    get => this._autoSelect;
    set => this._autoSelect = value;
  }

  public int Order => this._order;

  public long FromCtl
  {
    get => this._fromCtl;
    set => this._fromCtl = value;
  }

  public long ForCtl
  {
    get => this._forCtl;
    set => this._forCtl = value;
  }

  public AutoSelectNode.NodeFlags NodeFlag => this._nodeFlag;

  public int Key => this._key;

  public string Name
  {
    get => this._name;
    set => this._name = value;
  }

  public AutoSelectNode Parent => this._parent;

  public List<AutoSelectNode> Childs
  {
    get => this._childs;
    set
    {
      this._childs = value;
      foreach (AutoSelectNode child in this._childs)
        child._parent = this;
    }
  }

  public List<long> AnketaRows
  {
    get => this._anketaRows;
    set => this._anketaRows = value;
  }

  public AutoSelectionNodeCondList Anketa
  {
    get => this._anketa;
    set => this._anketa = value;
  }

  public int CtlKey
  {
    get => this._ctlKey;
    set => this._ctlKey = value;
  }

  public int CtlCondKey
  {
    get => this._ctlCondKey;
    set => this._ctlCondKey = value;
  }

  public Guid IpsObjectTypeGuid
  {
    get => this._ipsObjectTypeGuid;
    set => this._ipsObjectTypeGuid = value;
  }

  public override string ToString() => this._name != string.Empty ? this._name : base.ToString();

  public enum NodeFlags
  {
    Normal,
    Mandatory,
    Select,
    Folder,
    Dialog,
    Slide,
    Proc,
    Confirm,
    MultiDial,
  }
}
