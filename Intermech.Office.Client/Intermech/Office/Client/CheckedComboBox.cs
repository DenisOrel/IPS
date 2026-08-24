// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CheckedComboBox
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class CheckedComboBox : ComboBox
{
  private IContainer components;
  private CheckedComboBox.Dropdown dropdown;
  private string valueSeparator;

  public string ValueSeparator
  {
    get => this.valueSeparator;
    set => this.valueSeparator = value;
  }

  public bool CheckOnClick
  {
    get => this.dropdown.List.CheckOnClick;
    set => this.dropdown.List.CheckOnClick = value;
  }

  public new string DisplayMember
  {
    get => this.dropdown.List.DisplayMember;
    set => this.dropdown.List.DisplayMember = value;
  }

  public CheckedListBox.ObjectCollection Items => this.dropdown.List.Items;

  public CheckedListBox.CheckedItemCollection CheckedItems => this.dropdown.List.CheckedItems;

  public CheckedListBox.CheckedIndexCollection CheckedIndices => this.dropdown.List.CheckedIndices;

  public bool ValueChanged => this.dropdown.ValueChanged;

  public event ItemCheckEventHandler ItemCheck;

  public CheckedComboBox()
  {
    this.DrawMode = DrawMode.OwnerDrawVariable;
    this.valueSeparator = ", ";
    this.DropDownHeight = 1;
    this.DropDownStyle = ComboBoxStyle.DropDown;
    this.dropdown = new CheckedComboBox.Dropdown(this);
    this.CheckOnClick = true;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  protected override void OnDropDown(EventArgs e)
  {
    base.OnDropDown(e);
    this.DoDropDown();
  }

  private void DoDropDown()
  {
    if (this.dropdown.Visible)
      return;
    Rectangle screen = this.RectangleToScreen(this.ClientRectangle);
    this.dropdown.Location = new Point(screen.X, screen.Y + this.Size.Height);
    int num = this.dropdown.List.Items.Count;
    if (num > this.MaxDropDownItems)
      num = this.MaxDropDownItems;
    else if (num == 0)
      num = 1;
    this.dropdown.Size = new Size(this.Size.Width, this.dropdown.List.ItemHeight * num + 2);
    this.dropdown.Show((IWin32Window) this);
  }

  protected override void OnDropDownClosed(EventArgs e)
  {
    if (!(e is CheckedComboBox.Dropdown.CCBoxEventArgs))
      return;
    base.OnDropDownClosed(e);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (e.KeyCode == Keys.Down)
      this.OnDropDown((EventArgs) null);
    e.Handled = !e.Alt && e.KeyCode != Keys.Tab && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && e.KeyCode != Keys.Home && e.KeyCode != Keys.End;
    base.OnKeyDown(e);
  }

  protected override void OnKeyPress(KeyPressEventArgs e)
  {
    e.Handled = true;
    base.OnKeyPress(e);
  }

  public bool GetItemChecked(int index)
  {
    if (index < 0 || index > this.Items.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "value out of range");
    return this.dropdown.List.GetItemChecked(index);
  }

  public void SetItemChecked(int index, bool isChecked)
  {
    if (index < 0 || index > this.Items.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "value out of range");
    this.dropdown.List.SetItemChecked(index, isChecked);
    this.Text = this.dropdown.GetCheckedItemsStringValue();
  }

  public CheckState GetItemCheckState(int index)
  {
    if (index < 0 || index > this.Items.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "value out of range");
    return this.dropdown.List.GetItemCheckState(index);
  }

  public void SetItemCheckState(int index, CheckState state)
  {
    if (index < 0 || index > this.Items.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "value out of range");
    this.dropdown.List.SetItemCheckState(index, state);
    this.Text = this.dropdown.GetCheckedItemsStringValue();
  }

  internal class Dropdown : Form
  {
    private CheckedComboBox ccbParent;
    private string oldStrValue = "";
    private bool[] checkedStateArr;
    private bool dropdownClosed = true;
    private CheckedComboBox.Dropdown.CustomCheckedListBox cclb;

    public bool ValueChanged
    {
      get
      {
        string text = this.ccbParent.Text;
        return this.oldStrValue.Length > 0 && text.Length > 0 ? this.oldStrValue.CompareTo(text) != 0 : this.oldStrValue.Length != text.Length;
      }
    }

    public CheckedComboBox.Dropdown.CustomCheckedListBox List
    {
      get => this.cclb;
      set => this.cclb = value;
    }

    public Dropdown(CheckedComboBox ccbParent)
    {
      this.ccbParent = ccbParent;
      this.InitializeComponent();
      this.ShowInTaskbar = false;
      this.cclb.ItemCheck += new ItemCheckEventHandler(this.cclb_ItemCheck);
    }

    private void InitializeComponent()
    {
      this.cclb = new CheckedComboBox.Dropdown.CustomCheckedListBox();
      this.SuspendLayout();
      this.cclb.BorderStyle = BorderStyle.None;
      this.cclb.Dock = DockStyle.Fill;
      this.cclb.FormattingEnabled = true;
      this.cclb.Location = new Point(0, 0);
      this.cclb.Name = "cclb";
      this.cclb.Size = new Size(47, 15);
      this.cclb.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Menu;
      this.ClientSize = new Size(47, 16 /*0x10*/);
      this.ControlBox = false;
      this.Controls.Add((Control) this.cclb);
      this.ForeColor = SystemColors.ControlText;
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MinimizeBox = false;
      this.Name = "ccbParent";
      this.StartPosition = FormStartPosition.Manual;
      this.ResumeLayout(false);
    }

    public string GetCheckedItemsStringValue()
    {
      StringBuilder stringBuilder = new StringBuilder("");
      for (int index = 0; index < this.cclb.CheckedItems.Count; ++index)
        stringBuilder.Append(this.cclb.GetItemText(this.cclb.CheckedItems[index])).Append(this.ccbParent.ValueSeparator);
      if (stringBuilder.Length > 0)
        stringBuilder.Remove(stringBuilder.Length - this.ccbParent.ValueSeparator.Length, this.ccbParent.ValueSeparator.Length);
      return stringBuilder.ToString();
    }

    public void CloseDropdown(bool enactChanges)
    {
      if (this.dropdownClosed)
        return;
      if (enactChanges)
      {
        this.ccbParent.SelectedIndex = -1;
        this.ccbParent.Text = this.GetCheckedItemsStringValue();
      }
      else
      {
        for (int index = 0; index < this.cclb.Items.Count; ++index)
          this.cclb.SetItemChecked(index, this.checkedStateArr[index]);
      }
      this.dropdownClosed = true;
      this.ccbParent.Focus();
      this.Hide();
      this.ccbParent.OnDropDownClosed((EventArgs) new CheckedComboBox.Dropdown.CCBoxEventArgs((EventArgs) null, false));
    }

    protected override void OnActivated(EventArgs e)
    {
      base.OnActivated(e);
      this.dropdownClosed = false;
      this.oldStrValue = this.ccbParent.Text;
      this.checkedStateArr = new bool[this.cclb.Items.Count];
      for (int index = 0; index < this.cclb.Items.Count; ++index)
        this.checkedStateArr[index] = this.cclb.GetItemChecked(index);
    }

    protected override void OnDeactivate(EventArgs e)
    {
      base.OnDeactivate(e);
      this.CloseDropdown(!(e is CheckedComboBox.Dropdown.CCBoxEventArgs ccBoxEventArgs) || ccBoxEventArgs.AssignValues);
    }

    private void cclb_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (this.ccbParent.ItemCheck == null)
        return;
      this.ccbParent.ItemCheck(sender, e);
    }

    internal class CCBoxEventArgs : EventArgs
    {
      private bool assignValues;
      private EventArgs e;

      public bool AssignValues
      {
        get => this.assignValues;
        set => this.assignValues = value;
      }

      public EventArgs EventArgs
      {
        get => this.e;
        set => this.e = value;
      }

      public CCBoxEventArgs(EventArgs e, bool assignValues)
      {
        this.e = e;
        this.assignValues = assignValues;
      }
    }

    internal class CustomCheckedListBox : CheckedListBox
    {
      private int curSelIndex = -1;

      public CustomCheckedListBox()
      {
        this.SelectionMode = SelectionMode.One;
        this.HorizontalScrollbar = true;
      }

      protected override void OnKeyDown(KeyEventArgs e)
      {
        if (e.KeyCode == Keys.Return)
        {
          ((Form) this.Parent).OnDeactivate((EventArgs) new CheckedComboBox.Dropdown.CCBoxEventArgs((EventArgs) null, true));
          e.Handled = true;
        }
        else if (e.KeyCode == Keys.Escape)
        {
          ((Form) this.Parent).OnDeactivate((EventArgs) new CheckedComboBox.Dropdown.CCBoxEventArgs((EventArgs) null, false));
          e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete)
        {
          for (int index = 0; index < this.Items.Count; ++index)
            this.SetItemChecked(index, e.Shift);
          e.Handled = true;
        }
        base.OnKeyDown(e);
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
        base.OnMouseMove(e);
        int index = this.IndexFromPoint(e.Location);
        if (index < 0 || index == this.curSelIndex)
          return;
        this.curSelIndex = index;
        this.SetSelected(index, true);
      }
    }
  }
}
