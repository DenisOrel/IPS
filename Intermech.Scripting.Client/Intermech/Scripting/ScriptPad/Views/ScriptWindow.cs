// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.ScriptWindow
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;
using WeifenLuo.WinFormsUI.Docking;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal class ScriptWindow : DockContent, IScriptWindow
{
  private static readonly int DefaultDropDownItemsCount = 7;
  private IScriptCodeEditorControl codeEditor;
  private OpenScriptData script;
  private IContainer components;
  private Panel pnCodeEditor;
  private TableLayoutPanel panelNavigation;
  private RadDropDownList comboBoxNavigationMembers;
  private RadDropDownList comboBoxNavigationTypes;
  private Windows8Theme windows8Theme1;

  public ScriptWindow()
  {
    this.InitializeComponent();
    this.InitializeEventHandlers();
  }

  public event EventHandler<NavigateToCodeEventArgs> NavigateToCode;

  public IScriptCodeEditorControl CodeEditor
  {
    [DebuggerStepThrough] get => this.codeEditor;
    set
    {
      if (value != null)
      {
        if (this.codeEditor != null)
          throw new InvalidOperationException("CodeEditor is already set.");
        Control control = (Control) value;
        control.Dock = DockStyle.Fill;
        this.pnCodeEditor.Controls.Add(control);
        this.codeEditor = value;
      }
      else
      {
        if (this.codeEditor == null)
          return;
        Control codeEditor = (Control) this.codeEditor;
        this.pnCodeEditor.Controls.Remove(codeEditor);
        codeEditor.Dispose();
        this.codeEditor = (IScriptCodeEditorControl) null;
      }
    }
  }

  public OpenScriptData Script
  {
    [DebuggerStepThrough] get => this.script;
    set => this.script = value;
  }

  public bool EnableNavigationPanel
  {
    [DebuggerStepThrough] get => this.panelNavigation.Visible;
    set => this.panelNavigation.Visible = value;
  }

  public List<NavigationItem> NavigationTypes
  {
    [DebuggerStepThrough] get => this.GetNavigationItems(this.comboBoxNavigationTypes);
  }

  public List<NavigationItem> NavigationMembers
  {
    [DebuggerStepThrough] get => this.GetNavigationItems(this.comboBoxNavigationMembers);
  }

  public NavigationItem SelectedType
  {
    [DebuggerStepThrough] get => this.GetSelectedNavigationItem(this.comboBoxNavigationTypes);
  }

  public NavigationItem SelectedMember
  {
    [DebuggerStepThrough] get => this.GetSelectedNavigationItem(this.comboBoxNavigationMembers);
  }

  public void UpdateNavigationTypesSelection(NavigationItem typeToSelect)
  {
    if (!this.EnableNavigationPanel)
      throw new InvalidOperationException("Изменение выбранного элемента списка типов сценария невозможно: навигационная панель не отображена.");
    NavigationListItem objB = typeToSelect != null ? new NavigationListItem(typeToSelect) : (NavigationListItem) null;
    if (object.Equals((object) this.comboBoxNavigationTypes.SelectedItem, (object) objB))
      return;
    this.comboBoxNavigationTypes.SelectedItem = (RadListDataItem) objB;
  }

  public void UpdateNavigationMembersSelection(NavigationItem memberToSelect)
  {
    if (!this.EnableNavigationPanel)
      throw new InvalidOperationException("Изменение выбранного элемента списка элементов сценария невозможно: навигационная панель не отображена.");
    NavigationListItem objB = memberToSelect != null ? new NavigationListItem(memberToSelect) : (NavigationListItem) null;
    if (object.Equals((object) this.comboBoxNavigationMembers.SelectedItem, (object) objB))
      return;
    this.comboBoxNavigationMembers.SelectedItem = (RadListDataItem) objB;
  }

  public void UpdateNavigationTypes(IList<NavigationItem> types, NavigationItem typeToSelect)
  {
    if (types == null)
      throw new ArgumentNullException(nameof (types));
    if (!this.EnableNavigationPanel)
      throw new InvalidOperationException("Заполнение списка типов сценария невозможно: навигационная панель не отображена.");
    this.comboBoxNavigationTypes.BeginUpdate();
    try
    {
      this.UpdateNavigationItems(this.comboBoxNavigationTypes, types);
      this.UpdateNavigationTypesSelection(typeToSelect);
    }
    finally
    {
      this.comboBoxNavigationTypes.EndUpdate();
    }
  }

  public void UpdateNavigationMembers(IList<NavigationItem> members, NavigationItem memberToSelect)
  {
    if (members == null)
      throw new ArgumentNullException(nameof (members));
    if (!this.EnableNavigationPanel)
      throw new InvalidOperationException("Заполнение списка элементов сценария невозможно: навигационная панель не отображена.");
    this.comboBoxNavigationMembers.BeginUpdate();
    try
    {
      this.UpdateNavigationItems(this.comboBoxNavigationMembers, members);
      this.UpdateNavigationMembersSelection(memberToSelect);
    }
    finally
    {
      this.comboBoxNavigationMembers.EndUpdate();
    }
  }

  private void InitializeEventHandlers()
  {
    this.comboBoxNavigationTypes.Popup.MouseClick += new MouseEventHandler(this.PanelNavigation_MouseClick);
    this.comboBoxNavigationMembers.Popup.MouseClick += new MouseEventHandler(this.PanelNavigation_MouseClick);
  }

  private void PanelNavigation_MouseClick(object sender, MouseEventArgs e)
  {
    if (!(sender is RadEditorPopupControlBase popupControlBase) || !(popupControlBase.ElementTree.GetElementAtPoint(e.Location) is RadListVisualItem elementAtPoint) || !(elementAtPoint.Data is NavigationListItem data))
      return;
    EventHandler<NavigateToCodeEventArgs> navigateToCode = this.NavigateToCode;
    if (navigateToCode == null)
      return;
    navigateToCode(sender, new NavigateToCodeEventArgs(data.NavigationItem));
  }

  private void UpdateNavigationItems(
    RadDropDownList navigationComboBox,
    IList<NavigationItem> newItems)
  {
    Dictionary<string, NavigationItem> dictionary = new Dictionary<string, NavigationItem>();
    foreach (NavigationItem newItem in (IEnumerable<NavigationItem>) newItems)
    {
      if (!dictionary.ContainsKey(newItem.FullName))
        dictionary.Add(newItem.FullName, newItem);
    }
    List<NavigationListItem> navigationListItemList = new List<NavigationListItem>();
    foreach (NavigationListItem navigationListItem in navigationComboBox.Items.OfType<NavigationListItem>())
    {
      NavigationItem navigationItem1 = navigationListItem.NavigationItem;
      if (dictionary.ContainsKey(navigationItem1.FullName))
      {
        NavigationItem navigationItem2 = dictionary[navigationItem1.FullName];
        navigationListItem.Update(navigationItem2);
        dictionary.Remove(navigationItem1.FullName);
      }
      else
        navigationListItemList.Add(navigationListItem);
    }
    foreach (NavigationListItem navigationListItem in navigationListItemList)
      navigationComboBox.Items.Remove((RadListDataItem) navigationListItem);
    foreach (NavigationItem navigationItem in dictionary.Values)
    {
      NavigationListItem navigationListItem = new NavigationListItem(navigationItem);
      navigationComboBox.Items.Add((RadListDataItem) navigationListItem);
    }
    this.ResetDropDownSettings(navigationComboBox);
  }

  private void ResetDropDownSettings(RadDropDownList navigationComboBox)
  {
    navigationComboBox.ShowImageInEditorArea = false;
    navigationComboBox.ShowImageInEditorArea = true;
    navigationComboBox.DefaultItemsCountInDropDown = navigationComboBox.Items.Count != 0 ? ScriptWindow.DefaultDropDownItemsCount : 1;
  }

  private List<NavigationItem> GetNavigationItems(RadDropDownList navigationComboBox)
  {
    return navigationComboBox.Items.OfType<NavigationListItem>().Select<NavigationListItem, NavigationItem>((Func<NavigationListItem, NavigationItem>) (item => item.NavigationItem)).ToList<NavigationItem>();
  }

  private NavigationItem GetSelectedNavigationItem(RadDropDownList navigationComboBox)
  {
    return !(navigationComboBox?.SelectedItem is NavigationListItem selectedItem) ? (NavigationItem) null : selectedItem.NavigationItem;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.pnCodeEditor = new Panel();
    this.panelNavigation = new TableLayoutPanel();
    this.comboBoxNavigationTypes = new RadDropDownList();
    this.comboBoxNavigationMembers = new RadDropDownList();
    this.windows8Theme1 = new Windows8Theme();
    this.panelNavigation.SuspendLayout();
    this.comboBoxNavigationTypes.BeginInit();
    this.comboBoxNavigationMembers.BeginInit();
    this.SuspendLayout();
    this.pnCodeEditor.Dock = DockStyle.Fill;
    this.pnCodeEditor.Location = new Point(0, 27);
    this.pnCodeEditor.Name = "pnCodeEditor";
    this.pnCodeEditor.Size = new Size(800, 423);
    this.pnCodeEditor.TabIndex = 1;
    this.panelNavigation.ColumnCount = 2;
    this.panelNavigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.panelNavigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.panelNavigation.Controls.Add((Control) this.comboBoxNavigationTypes, 0, 0);
    this.panelNavigation.Controls.Add((Control) this.comboBoxNavigationMembers, 1, 0);
    this.panelNavigation.Dock = DockStyle.Top;
    this.panelNavigation.Location = new Point(0, 0);
    this.panelNavigation.Name = "panelNavigation";
    this.panelNavigation.RowCount = 1;
    this.panelNavigation.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.panelNavigation.Size = new Size(800, 27);
    this.panelNavigation.TabIndex = 3;
    this.panelNavigation.Visible = false;
    this.comboBoxNavigationTypes.AutoSizeItems = true;
    this.comboBoxNavigationTypes.BackColor = SystemColors.Window;
    this.comboBoxNavigationTypes.Dock = DockStyle.Fill;
    this.comboBoxNavigationTypes.DropDownAnimationEnabled = false;
    this.comboBoxNavigationTypes.DropDownStyle = RadDropDownStyle.DropDownList;
    this.comboBoxNavigationTypes.Location = new Point(3, 3);
    this.comboBoxNavigationTypes.Name = "comboBoxNavigationTypes";
    this.comboBoxNavigationTypes.Size = new Size(394, 21);
    this.comboBoxNavigationTypes.SortStyle = SortStyle.Ascending;
    this.comboBoxNavigationTypes.TabIndex = 5;
    this.comboBoxNavigationTypes.ThemeName = "Windows8";
    this.comboBoxNavigationMembers.AutoSizeItems = true;
    this.comboBoxNavigationMembers.BackColor = SystemColors.Window;
    this.comboBoxNavigationMembers.Dock = DockStyle.Fill;
    this.comboBoxNavigationMembers.DropDownAnimationEnabled = false;
    this.comboBoxNavigationMembers.DropDownStyle = RadDropDownStyle.DropDownList;
    this.comboBoxNavigationMembers.Location = new Point(403, 3);
    this.comboBoxNavigationMembers.Name = "comboBoxNavigationMembers";
    this.comboBoxNavigationMembers.Size = new Size(394, 21);
    this.comboBoxNavigationMembers.SortStyle = SortStyle.Ascending;
    this.comboBoxNavigationMembers.TabIndex = 4;
    this.comboBoxNavigationMembers.ThemeName = "Windows8";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(800, 450);
    this.Controls.Add((Control) this.pnCodeEditor);
    this.Controls.Add((Control) this.panelNavigation);
    this.Name = nameof (ScriptWindow);
    this.panelNavigation.ResumeLayout(false);
    this.panelNavigation.PerformLayout();
    this.comboBoxNavigationTypes.EndInit();
    this.comboBoxNavigationMembers.EndInit();
    this.ResumeLayout(false);
  }
}
