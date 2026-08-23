// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SelectGraphs
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Окно выбора граф для подписи</summary>
public class SelectGraphs : Form
{
  private Panel panel1;
  private Panel panel2;
  private Button _bApply;
  private Button _bCancel;
  private CheckedListBox _Box;
  private Button _bUpdate;
  private System.ComponentModel.Container components;
  private List<SelectGraphs.Value2Description> _list = new List<SelectGraphs.Value2Description>();

  /// <summary>событие на изменение пользователем набора граф</summary>
  public event EventHandler OnButtonApllyChange;

  /// <summary>кол-во выбранных граф для подписания</summary>
  public int SelectedGraphs => this._Box.CheckedItems.Count;

  /// <summary>
  /// Создание окна (колонки беруться из атрибута "Графы для подписей")
  /// </summary>
  public SelectGraphs()
  {
    this.InitializeComponent();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1265);
    this._bUpdate_Click((object) null, (EventArgs) null);
  }

  /// <summary>Коллекция выбранных колонок</summary>
  public ICollection SelectedList
  {
    get
    {
      ArrayList selectedList = new ArrayList();
      for (int index = 0; index < this._Box.CheckedItems.Count; ++index)
        selectedList.Add((object) (this._Box.CheckedItems[index] as SelectGraphs.Value2Description).Value);
      return (ICollection) selectedList;
    }
  }

  /// <summary>Загрузка колонок в список для выбора</summary>
  private void LoadValues()
  {
    this._Box.BeginUpdate();
    try
    {
      this._Box.Items.Clear();
      foreach (object obj in this._list)
        this._Box.Items.Add(obj);
    }
    finally
    {
      this._Box.EndUpdate();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="disposing"></param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectGraphs));
    this.panel1 = new Panel();
    this._bUpdate = new Button();
    this.panel2 = new Panel();
    this._bCancel = new Button();
    this._bApply = new Button();
    this._Box = new CheckedListBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Controls.Add((Control) this._bUpdate);
    this.panel1.Controls.Add((Control) this.panel2);
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this._bUpdate, "_bUpdate");
    this._bUpdate.Name = "_bUpdate";
    this._bUpdate.Click += new EventHandler(this._bUpdate_Click);
    this.panel2.Controls.Add((Control) this._bCancel);
    this.panel2.Controls.Add((Control) this._bApply);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this._bCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this._bCancel, "_bCancel");
    this._bCancel.Name = "_bCancel";
    this._bApply.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this._bApply, "_bApply");
    this._bApply.Name = "_bApply";
    this._Box.CheckOnClick = true;
    componentResourceManager.ApplyResources((object) this._Box, "_Box");
    this._Box.Name = "_Box";
    this._Box.SelectedValueChanged += new EventHandler(this._Box_SelectedValueChanged);
    this.AcceptButton = (IButtonControl) this._bApply;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.CancelButton = (IButtonControl) this._bCancel;
    this.Controls.Add((Control) this._Box);
    this.Controls.Add((Control) this.panel1);
    this.HelpButton = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectGraphs);
    this.ShowInTaskbar = false;
    this.Load += new EventHandler(this.SelectGraphs_Load);
    this.Closed += new EventHandler(this.SelectGraphs_Closed);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  /// <summary>
  /// Обновление списка из атрибута "Графы для подписей"
  /// Походу обновляется также кэш возможных колонок
  /// </summary>
  private void _bUpdate_Click(object sender, EventArgs e) => this.ListUpdate();

  private void SelectGraphs_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void SelectGraphs_Closed(object sender, EventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  /// <summary>Для корректного размещения формы</summary>
  /// <param name="parent"></param>
  public void SetParent(Control parent)
  {
    this.TopLevel = false;
    this.Parent = parent;
    this.Dock = DockStyle.Fill;
    this.FormBorderStyle = FormBorderStyle.None;
    this.Visible = true;
    this.panel1.Visible = false;
    this._Box.Dock = DockStyle.Fill;
    this.OnResize(new EventArgs());
    this._bUpdate.Visible = false;
    this._bCancel.Visible = false;
    this._bApply.Visible = false;
  }

  /// <summary>
  /// 
  /// </summary>
  public void ListUpdate()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(SignsHolder.GraphAttrTypeID);
      if (attributeType.MultipleValued.Equals((object) MultiValueModes.SingleValueFromList))
      {
        SignsCache.PossibleGraphs = SignsCache.ParsePossibleGraphs(attributeType.GetPossibleValues());
        this._list.Clear();
        foreach (KeyValuePair<string, string> possibleGraph in SignsCache.PossibleGraphs)
          this._list.Add(new SelectGraphs.Value2Description(possibleGraph.Key, possibleGraph.Value));
        this._list.Sort((Comparison<SelectGraphs.Value2Description>) ((x, y) => x.Description.CompareTo(y.Description)));
      }
      this.LoadValues();
    }
  }

  private void _Box_SelectedValueChanged(object sender, EventArgs e)
  {
    if (this.OnButtonApllyChange == null)
      return;
    this.OnButtonApllyChange(sender, e);
  }

  private class Value2Description
  {
    private string _value = string.Empty;
    private string _description = string.Empty;

    public Value2Description(string value, string description)
    {
      this._value = value;
      this._description = description;
    }

    public string Value => this._value;

    public string Description => this._description;

    public override string ToString() => this._description;
  }
}
