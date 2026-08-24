// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomEditorTreeControl
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class MbomEditorTreeControl : UserControl
{
  private IDescriptor _descriptor;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private MbomNavigatorTreeView _navigatorTreeView;

  public MbomEditorTreeControl()
  {
    this.InitializeComponent();
    this._navigatorTreeView.CommandsProvider = (ICommandsProvider) new MbomEditorTreeControl.MbomEditorTreeCommandsProvider(this, this._navigatorTreeView.CommandsProvider);
    this._navigatorTreeView.Services = (IServiceProvider) ServicesManager.ServiceContainer;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IDescriptor Descriptor
  {
    get => this._descriptor;
    set
    {
      if (this._descriptor == value)
        return;
      this._descriptor = value;
      NodeColumnCollection nodeColumns = this.GetNodeColumns();
      if (nodeColumns == null || nodeColumns.Count == 0)
        this.SetNodeColumns(Intermech.Navigator.Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
      this._navigatorTreeView.Build(this._descriptor);
    }
  }

  public NavigatorTreeNode[] GetSelectedNodes() => this._navigatorTreeView.SelectedNodes;

  public void SetSelectedNodes(NavigatorTreeNode[] nodes)
  {
    foreach (NavigatorTreeNode node in nodes)
      node.Select();
  }

  public event EventHandler SelectionChanged;

  public event EventHandler<MbomEditorTreeControl.GetCellValueEventArgs> GetCellValue;

  public event EventHandler CommandsTableUpdated;

  public event EventHandler<MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs> GetNodeDragDropEffects;

  public event EventHandler<MbomNavigatorTreeView.NodeDropEventArgs> NodeDrop;

  public void AddColumn(Column column)
  {
    if (column == null)
      throw new ArgumentNullException(nameof (column));
    this._navigatorTreeView.Columns.Add(column);
  }

  public bool CanExecuteCommand(string commandName)
  {
    ICommandState commandState = !string.IsNullOrEmpty(commandName) ? ServiceLocator.Get<ICommandManager>().FindCommand(commandName) : throw new ArgumentException();
    this._navigatorTreeView.QueryStatus(commandState);
    return commandState.Enabled;
  }

  public void ExecuteCommand(string commandName)
  {
    if (string.IsNullOrEmpty(commandName))
      throw new ArgumentException();
    this._navigatorTreeView.Execute(commandName);
  }

  public NodeColumnCollection GetNodeColumns()
  {
    return this._navigatorTreeView.ReflectTreeColumsChanges();
  }

  public void SetNodeColumns(NodeColumnCollection nodeColumns)
  {
    if (nodeColumns == null)
      throw new ArgumentNullException(nameof (nodeColumns));
    this._navigatorTreeView.SetColumns(nodeColumns, true);
  }

  public NodeColumnCollection GetSupportedColumns() => this._navigatorTreeView.SupportedColumns;

  public void SetSupportedColumns(NodeColumnCollection nodeColumns)
  {
    this._navigatorTreeView.SupportedColumns = nodeColumns != null ? nodeColumns : throw new ArgumentNullException(nameof (nodeColumns));
  }

  public void Rebuild() => this._navigatorTreeView.RefreshNode(this._navigatorTreeView.RootNode);

  private void NavigatorTreeView_GetCellData(object sender, GetCellDataEventArgs e)
  {
    if (!(e.Row.Item is NavigatorTreeNode navigatorTreeNode))
      return;
    MbomEditorTreeControl.GetCellValueEventArgs e1 = new MbomEditorTreeControl.GetCellValueEventArgs(e.Column, navigatorTreeNode);
    EventHandler<MbomEditorTreeControl.GetCellValueEventArgs> getCellValue = this.GetCellValue;
    if (getCellValue != null)
      getCellValue((object) this, e1);
    e.CellData.Value = e1.CellValue ?? e.CellData.Value;
  }

  private void NavigatorTreeView_SelectionChanged(object sender, EventArgs e)
  {
    EventHandler selectionChanged = this.SelectionChanged;
    if (selectionChanged == null)
      return;
    selectionChanged((object) this, e);
  }

  private void NavigatorTreeView_ShowContextMenu(object sender, MouseEventArgs e)
  {
    if (e.Y <= this._navigatorTreeView.HeaderHeight)
      return;
    this.ContextMenuStrip.Show((Control) this, e.Location);
  }

  private void NavigatorTreeView_CommandsTableUpdated(object sender, EventArgs e)
  {
    EventHandler commandsTableUpdated = this.CommandsTableUpdated;
    if (commandsTableUpdated == null)
      return;
    commandsTableUpdated((object) this, e);
  }

  private void NavigatorTreeView_GetNodeDragDropEffects(
    object sender,
    MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs e)
  {
    EventHandler<MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs> nodeDragDropEffects = this.GetNodeDragDropEffects;
    if (nodeDragDropEffects == null)
      return;
    nodeDragDropEffects((object) this, e);
  }

  private void NavigatorTreeView_NodeDrop(object sender, MbomNavigatorTreeView.NodeDropEventArgs e)
  {
    EventHandler<MbomNavigatorTreeView.NodeDropEventArgs> nodeDrop = this.NodeDrop;
    if (nodeDrop == null)
      return;
    nodeDrop((object) this, e);
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
    this.components = (IContainer) new System.ComponentModel.Container();
    this._navigatorTreeView = new MbomNavigatorTreeView();
    this._navigatorTreeView.BeginInit();
    this.SuspendLayout();
    this._navigatorTreeView.AllowDrop = true;
    this._navigatorTreeView.AllowUserPinnedColumns = false;
    this._navigatorTreeView.DisableCheckedOutColumn = true;
    this._navigatorTreeView.DisableIMContextMenu = true;
    this._navigatorTreeView.DisableKeyDownEvents = true;
    this._navigatorTreeView.DisableKeyUpEvents = true;
    this._navigatorTreeView.Dock = DockStyle.Fill;
    this._navigatorTreeView.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this._navigatorTreeView.ImageList = (ImageList) null;
    this._navigatorTreeView.LineStyle = LineStyle.Dot;
    this._navigatorTreeView.Location = new Point(0, 0);
    this._navigatorTreeView.MultiSelect = true;
    this._navigatorTreeView.Name = "_navigatorTreeView";
    this._navigatorTreeView.RowEvenStyle.WordWrap = false;
    this._navigatorTreeView.RowOddStyle.WordWrap = false;
    this._navigatorTreeView.RowSelectedStyle.WordWrap = false;
    this._navigatorTreeView.RowStyle.BorderColor = SystemColors.Control;
    this._navigatorTreeView.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this._navigatorTreeView.RowStyle.BorderWidth = 1;
    this._navigatorTreeView.RowStyle.WordWrap = false;
    this._navigatorTreeView.SelectBeforeEdit = true;
    this._navigatorTreeView.ShowRootRow = false;
    this._navigatorTreeView.Size = new Size(150, 150);
    this._navigatorTreeView.SuppressErrorMessages = true;
    this._navigatorTreeView.TabIndex = 0;
    this._navigatorTreeView.GetNodeDragDropEffects += new EventHandler<MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs>(this.NavigatorTreeView_GetNodeDragDropEffects);
    this._navigatorTreeView.NodeDrop += new EventHandler<MbomNavigatorTreeView.NodeDropEventArgs>(this.NavigatorTreeView_NodeDrop);
    this._navigatorTreeView.ShowContextMenu += new MouseEventHandler(this.NavigatorTreeView_ShowContextMenu);
    this._navigatorTreeView.CommandsTableUpdated += new EventHandler(this.NavigatorTreeView_CommandsTableUpdated);
    this._navigatorTreeView.GetCellData += new GetCellDataHandler(this.NavigatorTreeView_GetCellData);
    this._navigatorTreeView.SelectionChanged += new EventHandler(this.NavigatorTreeView_SelectionChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._navigatorTreeView);
    this.Name = nameof (MbomEditorTreeControl);
    this._navigatorTreeView.EndInit();
    this.ResumeLayout(false);
  }

  public sealed class GetCellValueEventArgs : EventArgs
  {
    public GetCellValueEventArgs(Column column, NavigatorTreeNode navigatorTreeNode)
    {
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      if (navigatorTreeNode == null)
        throw new ArgumentNullException(nameof (navigatorTreeNode));
      this.Column = column;
      this.NavigatorTreeNode = navigatorTreeNode;
    }

    public object CellValue { get; set; }

    public Column Column { get; private set; }

    public NavigatorTreeNode NavigatorTreeNode { get; private set; }
  }

  public sealed class MbomEditorTreeCommandsProvider : ICommandsProvider
  {
    private MbomEditorTreeControl _mbomEditorTreeControl;
    private ICommandsProvider _commandsProvider;

    public MbomEditorTreeCommandsProvider(
      MbomEditorTreeControl mbomEditorTreeControl,
      ICommandsProvider commandsProvider)
    {
      if (mbomEditorTreeControl == null)
        throw new ArgumentNullException(nameof (mbomEditorTreeControl));
      if (commandsProvider == null)
        throw new ArgumentNullException(nameof (commandsProvider));
      this._mbomEditorTreeControl = mbomEditorTreeControl;
      this._commandsProvider = commandsProvider;
    }

    public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
    {
      CommandsInfo mergedCommands = this._commandsProvider.GetMergedCommands(items, viewServices);
      this.ReplaceSetupColumnsCommand(mergedCommands);
      return mergedCommands;
    }

    public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
    {
      CommandsInfo groupCommands = this._commandsProvider.GetGroupCommands(items, viewServices);
      this.ReplaceSetupColumnsCommand(groupCommands);
      return groupCommands;
    }

    private void ReplaceSetupColumnsCommand(CommandsInfo commandsInfo)
    {
      commandsInfo.Remove("SetupColumns");
      commandsInfo.Add("SetupColumns", new CommandInfo(0, new ClickEventHandler(this.SetupColumns)));
    }

    private void SetupColumns(
      ISelectedItems items,
      IServiceProvider viewServices,
      object additionalInfo)
    {
      NodeColumnCollection nodeColumns = this._mbomEditorTreeControl.GetNodeColumns();
      if (AppearanceTuningForm.Execute(this._mbomEditorTreeControl._navigatorTreeView.GetChildHandler(this._mbomEditorTreeControl._navigatorTreeView.FocusedNode), ContentType.Folders, this._mbomEditorTreeControl.GetSupportedColumns(), nodeColumns) != DialogResult.OK)
        return;
      this._mbomEditorTreeControl.SetNodeColumns(nodeColumns);
    }
  }
}
