// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.IDEWindow
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using Intermech.Mvp.Components;
using Intermech.Mvp.Winforms;
using Intermech.Scripting.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal class IDEWindow : MvpWindow, IIDEView, IView
{
  private EventHandler<CancelEventArgs> viewClosing;
  private ErrorsView errorsView;
  private OutputView outputView;
  private ScriptWindowCollection scriptWindows;
  private IMultiCommand newCommand;
  private IClickCommand openCommand;
  private IClickCommand replaceWithCommand;
  private IClickCommand saveCommand;
  private IClickCommand saveAsCommand;
  private IClickCommand saveCopyCommand;
  private IClickCommand cutCommand;
  private IClickCommand copyCommand;
  private IClickCommand pasteCommand;
  private IClickCommand undoCommand;
  private IClickCommand redoCommand;
  private IClickCommand commentSelectionCommand;
  private IClickCommand uncommentSelectionCommand;
  private IClickCommand formatIndentsCommand;
  private IClickCommand findReplaceCommand;
  private IClickCommand runCommand;
  private IClickCommand stopRunCommand;
  private IMultiCommand editExecutorSettingsCommand;
  private IClickCommand editRunParametersCommand;
  private IClickCommand editIDESettingsCommand;
  private KeyEventHandler cmdKeyDown;
  private IContainer components;
  private StatusStrip ssMainStatusBar;
  private MenuStrip mmMainMenu;
  private ToolStripMenuItem tsmiFile;
  private ToolStripMenuItem tsmiExit;
  private ToolStrip tsMainToolbar;
  private ToolStripMenuItem tsmiNew;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripButton tsbSaveAs;
  private ToolStripDropDownButton tsbNew;
  private ToolStripButton tsbSave;
  private ToolStripButton tsbOpen;
  private DockPanel dpScriptWindow;
  private ToolStripSeparator toolStripSeparator2;
  private ToolStripButton tsbRun;
  private ToolStripStatusLabel tsslLineCaption;
  private ToolStripStatusLabel tsslLine;
  private ToolStripStatusLabel tsslColumnCaption;
  private ToolStripStatusLabel tsslColumn;
  private ToolStripStatusLabel tsslText;
  private ToolStripStatusLabel tsslEncodingCaption;
  private ToolStripStatusLabel tsslEncoding;
  private ToolStripStatusLabel tsslLanguageCaption;
  private ToolStripStatusLabel tsslLanguage;
  private ToolStripMenuItem tsmiSettings;
  private ToolStripMenuItem tsmiEditExecutorSettings;
  private ToolStripMenuItem tsmiOpen;
  private ToolStripMenuItem tsmiSave;
  private ToolStripMenuItem tsmiSaveAs;
  private ToolStripSeparator toolStripSeparator3;
  private ToolStripMenuItem tsmiDebugAndRun;
  private ToolStripMenuItem tsmiRun;
  private ToolStripSeparator toolStripSeparator4;
  private ToolStripMenuItem tsmiStopRun;
  private ToolStripButton tsbStopRun;
  private ToolStripSeparator toolStripSeparator5;
  private ToolStripMenuItem tsmiEditRunParameters;
  private ToolStripButton tsbCommentSelection;
  private ToolStripButton tsbUncommentSelection;
  private ToolStripSeparator toolStripSeparator7;
  private ToolStripMenuItem tsmiEdit;
  private ToolStripMenuItem tsmiCommentSelection;
  private ToolStripMenuItem tsmiUncommentSelection;
  private ToolStripMenuItem tsmiReplaceWith;
  private ToolStripButton tsbReplaceWith;
  private ToolStripSeparator toolStripSeparator6;
  private ToolStripMenuItem tsmiEditIDESettings;
  private ToolStripMenuItem tsmiFormatIndents;
  private ToolStripButton tsbFormatIndents;
  private ToolStripButton tsbUndo;
  private ToolStripSeparator toolStripSeparator8;
  private ToolStripButton tsbRedo;
  private ToolStripMenuItem tsmiUndo;
  private ToolStripMenuItem tsmiRedo;
  private ToolStripSeparator toolStripSeparator9;
  private ToolStripButton tsbCut;
  private ToolStripButton tsbCopy;
  private ToolStripButton tsbPaste;
  private ToolStripSeparator toolStripSeparator11;
  private ToolStripMenuItem tsmiCut;
  private ToolStripMenuItem tsmiCopy;
  private ToolStripMenuItem tsmiPaste;
  private ToolStripSeparator toolStripSeparator10;
  private ToolStripMenuItem tsmiFindReplace;
  private ToolStripButton tsbFindReplace;
  private ToolStripSeparator toolStripSeparator13;
  private ToolStripSeparator toolStripSeparator12;
  private ToolStripButton tsbSaveCopyAs;
  private ToolStripMenuItem tsmiSaveCopyAs;

  public IDEWindow()
  {
    this.InitializeComponent();
    this.errorsView = new ErrorsView();
    this.errorsView.Show(this.dpScriptWindow, DockState.DockBottom);
    this.outputView = new OutputView();
    this.outputView.Show(this.errorsView.Pane, DockAlignment.Right, 0.5);
    this.scriptWindows = new ScriptWindowCollection(this.dpScriptWindow);
    this.newCommand = (IMultiCommand) new MultiCommandGroup(new IMultiCommand[2]
    {
      (IMultiCommand) new ToolStripDropDownMultiCommand((ToolStripDropDownItem) this.tsmiNew),
      (IMultiCommand) new ToolStripDropDownMultiCommand((ToolStripDropDownItem) this.tsbNew)
    });
    this.openCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiOpen),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbOpen)
    });
    this.replaceWithCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiReplaceWith),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbReplaceWith)
    });
    this.saveCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiSave),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbSave)
    });
    this.saveAsCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiSaveAs),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbSaveAs)
    });
    this.saveCopyCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiSaveCopyAs),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbSaveCopyAs)
    });
    this.cutCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiCut),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbCut)
    });
    this.copyCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiCopy),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbCopy)
    });
    this.pasteCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiPaste),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbPaste)
    });
    this.undoCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiUndo),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbUndo)
    });
    this.redoCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiRedo),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbRedo)
    });
    this.commentSelectionCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiCommentSelection),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbCommentSelection)
    });
    this.uncommentSelectionCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiUncommentSelection),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbUncommentSelection)
    });
    this.formatIndentsCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiFormatIndents),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbFormatIndents)
    });
    this.findReplaceCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiFindReplace),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbFindReplace)
    });
    this.runCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiRun),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbRun)
    });
    this.stopRunCommand = (IClickCommand) new ClickCommandGroup(new IClickCommand[2]
    {
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiStopRun),
      (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsbStopRun)
    });
    this.editExecutorSettingsCommand = (IMultiCommand) new ToolStripDropDownMultiCommand((ToolStripDropDownItem) this.tsmiEditExecutorSettings);
    this.editRunParametersCommand = (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiEditRunParameters);
    this.editIDESettingsCommand = (IClickCommand) new ToolStripClickCommand((ToolStripItem) this.tsmiEditIDESettings);
  }

  private string CoerceEmptyString(string value) => !string.IsNullOrEmpty(value) ? value : "-/-";

  void IIDEView.ShowScriptCodeEditorCaretPosition(string line, string column)
  {
    line = this.CoerceEmptyString(line);
    column = this.CoerceEmptyString(column);
    this.tsslLine.Text = line;
    this.tsslColumn.Text = column;
  }

  void IIDEView.ShowScriptLanguage(string languageName)
  {
    languageName = this.CoerceEmptyString(languageName);
    this.tsslLanguage.Text = languageName;
  }

  void IIDEView.ShowScriptEncoding(string encoding)
  {
    encoding = this.CoerceEmptyString(encoding);
    this.tsslEncoding.Text = encoding;
  }

  IScriptWindowCollection IIDEView.ScriptWindows => (IScriptWindowCollection) this.scriptWindows;

  event KeyEventHandler IIDEView.HotkeyPressed
  {
    add
    {
      this.KeyDown += value;
      this.cmdKeyDown += value;
    }
    remove
    {
      this.KeyDown -= value;
      this.cmdKeyDown -= value;
    }
  }

  event EventHandler<CancelEventArgs> IIDEView.ViewClosing
  {
    add => this.viewClosing += value;
    remove => this.viewClosing -= value;
  }

  IMultiCommand IIDEView.NewCommand => this.newCommand;

  IClickCommand IIDEView.OpenCommand => this.openCommand;

  IClickCommand IIDEView.ReplaceWithCommand => this.replaceWithCommand;

  IClickCommand IIDEView.SaveCommand => this.saveCommand;

  IClickCommand IIDEView.SaveAsCommand => this.saveAsCommand;

  IClickCommand IIDEView.SaveCopyCommand => this.saveCopyCommand;

  IClickCommand IIDEView.CutCommand => this.cutCommand;

  IClickCommand IIDEView.CopyCommand => this.copyCommand;

  IClickCommand IIDEView.PasteCommand => this.pasteCommand;

  IClickCommand IIDEView.UndoCommand => this.undoCommand;

  IClickCommand IIDEView.RedoCommand => this.redoCommand;

  IClickCommand IIDEView.CommentSelectionCommand => this.commentSelectionCommand;

  IClickCommand IIDEView.UncommentSelectionCommand => this.uncommentSelectionCommand;

  IClickCommand IIDEView.FormatIndentsCommand => this.formatIndentsCommand;

  IClickCommand IIDEView.FindReplaceCommand => this.findReplaceCommand;

  IClickCommand IIDEView.RunCommand => this.runCommand;

  IClickCommand IIDEView.StopRunCommand => this.stopRunCommand;

  IMultiCommand IIDEView.EditExecutorSettingsCommand => this.editExecutorSettingsCommand;

  IClickCommand IIDEView.EditRunParametersCommand => this.editRunParametersCommand;

  IClickCommand IIDEView.EditIDESettingsCommand => this.editIDESettingsCommand;

  IErrorsView IIDEView.ErrorsView => (IErrorsView) this.errorsView;

  IScriptOutputStream IIDEView.OutputView => (IScriptOutputStream) this.outputView;

  bool IIDEView.MaximizedAtStartup
  {
    get => this.WindowState == FormWindowState.Maximized;
    set
    {
      FormWindowState formWindowState = value ? FormWindowState.Maximized : FormWindowState.Normal;
      if (formWindowState == this.WindowState)
        return;
      this.WindowState = formWindowState;
      if (this.WindowState != FormWindowState.Normal)
        return;
      Size size = Screen.FromControl((Control) this).WorkingArea.Size;
      this.Width = (int) Math.Round((double) size.Width * 0.8);
      this.Height = (int) Math.Round((double) size.Height * 0.8);
      this.CenterToScreen();
    }
  }

  private void tsmiExit_Click(object sender, EventArgs e) => this.Close();

  private void IDEWindow_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this.viewClosing == null)
      return;
    this.viewClosing((object) this, (CancelEventArgs) e);
  }

  private void IDEWindow_FormClosed(object sender, FormClosedEventArgs e)
  {
  }

  protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
  {
    if (this.cmdKeyDown != null)
    {
      KeyEventArgs e = new KeyEventArgs(keyData);
      this.cmdKeyDown((object) this, e);
      if (e.Handled)
        return true;
    }
    return base.ProcessCmdKey(ref msg, keyData);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IDEWindow));
    this.ssMainStatusBar = new StatusStrip();
    this.tsslText = new ToolStripStatusLabel();
    this.tsslLanguageCaption = new ToolStripStatusLabel();
    this.tsslLanguage = new ToolStripStatusLabel();
    this.tsslEncodingCaption = new ToolStripStatusLabel();
    this.tsslEncoding = new ToolStripStatusLabel();
    this.tsslLineCaption = new ToolStripStatusLabel();
    this.tsslLine = new ToolStripStatusLabel();
    this.tsslColumnCaption = new ToolStripStatusLabel();
    this.tsslColumn = new ToolStripStatusLabel();
    this.mmMainMenu = new MenuStrip();
    this.tsmiFile = new ToolStripMenuItem();
    this.tsmiNew = new ToolStripMenuItem();
    this.toolStripSeparator1 = new ToolStripSeparator();
    this.tsmiOpen = new ToolStripMenuItem();
    this.tsmiReplaceWith = new ToolStripMenuItem();
    this.tsmiSave = new ToolStripMenuItem();
    this.tsmiSaveAs = new ToolStripMenuItem();
    this.toolStripSeparator3 = new ToolStripSeparator();
    this.tsmiExit = new ToolStripMenuItem();
    this.tsmiEdit = new ToolStripMenuItem();
    this.tsmiCut = new ToolStripMenuItem();
    this.tsmiCopy = new ToolStripMenuItem();
    this.tsmiPaste = new ToolStripMenuItem();
    this.toolStripSeparator10 = new ToolStripSeparator();
    this.tsmiUndo = new ToolStripMenuItem();
    this.tsmiRedo = new ToolStripMenuItem();
    this.toolStripSeparator9 = new ToolStripSeparator();
    this.tsmiFindReplace = new ToolStripMenuItem();
    this.toolStripSeparator13 = new ToolStripSeparator();
    this.tsmiCommentSelection = new ToolStripMenuItem();
    this.tsmiUncommentSelection = new ToolStripMenuItem();
    this.tsmiFormatIndents = new ToolStripMenuItem();
    this.tsmiDebugAndRun = new ToolStripMenuItem();
    this.tsmiRun = new ToolStripMenuItem();
    this.tsmiStopRun = new ToolStripMenuItem();
    this.toolStripSeparator5 = new ToolStripSeparator();
    this.tsmiEditRunParameters = new ToolStripMenuItem();
    this.tsmiSettings = new ToolStripMenuItem();
    this.tsmiEditIDESettings = new ToolStripMenuItem();
    this.toolStripSeparator6 = new ToolStripSeparator();
    this.tsmiEditExecutorSettings = new ToolStripMenuItem();
    this.tsMainToolbar = new ToolStrip();
    this.tsbNew = new ToolStripDropDownButton();
    this.toolStripSeparator4 = new ToolStripSeparator();
    this.tsbOpen = new ToolStripButton();
    this.tsbReplaceWith = new ToolStripButton();
    this.tsbSave = new ToolStripButton();
    this.tsbSaveAs = new ToolStripButton();
    this.toolStripSeparator2 = new ToolStripSeparator();
    this.tsbCut = new ToolStripButton();
    this.tsbCopy = new ToolStripButton();
    this.tsbPaste = new ToolStripButton();
    this.toolStripSeparator11 = new ToolStripSeparator();
    this.tsbUndo = new ToolStripButton();
    this.tsbRedo = new ToolStripButton();
    this.toolStripSeparator8 = new ToolStripSeparator();
    this.tsbFindReplace = new ToolStripButton();
    this.toolStripSeparator12 = new ToolStripSeparator();
    this.tsbCommentSelection = new ToolStripButton();
    this.tsbUncommentSelection = new ToolStripButton();
    this.tsbFormatIndents = new ToolStripButton();
    this.toolStripSeparator7 = new ToolStripSeparator();
    this.tsbRun = new ToolStripButton();
    this.tsbStopRun = new ToolStripButton();
    this.dpScriptWindow = new DockPanel();
    this.tsbSaveCopyAs = new ToolStripButton();
    this.tsmiSaveCopyAs = new ToolStripMenuItem();
    this.ssMainStatusBar.SuspendLayout();
    this.mmMainMenu.SuspendLayout();
    this.tsMainToolbar.SuspendLayout();
    this.SuspendLayout();
    this.ssMainStatusBar.Items.AddRange(new ToolStripItem[9]
    {
      (ToolStripItem) this.tsslText,
      (ToolStripItem) this.tsslLanguageCaption,
      (ToolStripItem) this.tsslLanguage,
      (ToolStripItem) this.tsslEncodingCaption,
      (ToolStripItem) this.tsslEncoding,
      (ToolStripItem) this.tsslLineCaption,
      (ToolStripItem) this.tsslLine,
      (ToolStripItem) this.tsslColumnCaption,
      (ToolStripItem) this.tsslColumn
    });
    this.ssMainStatusBar.Location = new Point(0, 419);
    this.ssMainStatusBar.Name = "ssMainStatusBar";
    this.ssMainStatusBar.Size = new Size(624, 22);
    this.ssMainStatusBar.TabIndex = 3;
    this.ssMainStatusBar.Text = "statusStrip1";
    this.tsslText.Name = "tsslText";
    this.tsslText.Size = new Size(288, 17);
    this.tsslText.Spring = true;
    this.tsslText.Text = "Готово";
    this.tsslText.TextAlign = ContentAlignment.MiddleLeft;
    this.tsslLanguageCaption.Margin = new Padding(16 /*0x10*/, 3, 0, 2);
    this.tsslLanguageCaption.Name = "tsslLanguageCaption";
    this.tsslLanguageCaption.Size = new Size(37, 17);
    this.tsslLanguageCaption.Text = "Язык:";
    this.tsslLanguage.Name = "tsslLanguage";
    this.tsslLanguage.Size = new Size(22, 17);
    this.tsslLanguage.Text = "-/-";
    this.tsslLanguage.TextAlign = ContentAlignment.MiddleLeft;
    this.tsslEncodingCaption.Margin = new Padding(16 /*0x10*/, 3, 0, 2);
    this.tsslEncodingCaption.Name = "tsslEncodingCaption";
    this.tsslEncodingCaption.Size = new Size(69, 17);
    this.tsslEncodingCaption.Text = "Кодировка:";
    this.tsslEncoding.Name = "tsslEncoding";
    this.tsslEncoding.Size = new Size(22, 17);
    this.tsslEncoding.Text = "-/-";
    this.tsslEncoding.TextAlign = ContentAlignment.MiddleLeft;
    this.tsslLineCaption.Margin = new Padding(16 /*0x10*/, 3, 0, 2);
    this.tsslLineCaption.Name = "tsslLineCaption";
    this.tsslLineCaption.Size = new Size(23, 17);
    this.tsslLineCaption.Text = "Ln:";
    this.tsslLine.AutoSize = false;
    this.tsslLine.Name = "tsslLine";
    this.tsslLine.Size = new Size(32 /*0x20*/, 17);
    this.tsslLine.Text = "-/-";
    this.tsslLine.TextAlign = ContentAlignment.MiddleLeft;
    this.tsslColumnCaption.Margin = new Padding(8, 3, 0, 2);
    this.tsslColumnCaption.Name = "tsslColumnCaption";
    this.tsslColumnCaption.Size = new Size(28, 17);
    this.tsslColumnCaption.Text = "Col:";
    this.tsslColumn.AutoSize = false;
    this.tsslColumn.Name = "tsslColumn";
    this.tsslColumn.Size = new Size(32 /*0x20*/, 17);
    this.tsslColumn.Text = "-/-";
    this.tsslColumn.TextAlign = ContentAlignment.MiddleLeft;
    this.mmMainMenu.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this.tsmiFile,
      (ToolStripItem) this.tsmiEdit,
      (ToolStripItem) this.tsmiDebugAndRun,
      (ToolStripItem) this.tsmiSettings
    });
    this.mmMainMenu.Location = new Point(0, 0);
    this.mmMainMenu.Name = "mmMainMenu";
    this.mmMainMenu.Size = new Size(624, 24);
    this.mmMainMenu.TabIndex = 0;
    this.mmMainMenu.Text = "menuStrip1";
    this.tsmiFile.DropDownItems.AddRange(new ToolStripItem[9]
    {
      (ToolStripItem) this.tsmiNew,
      (ToolStripItem) this.toolStripSeparator1,
      (ToolStripItem) this.tsmiOpen,
      (ToolStripItem) this.tsmiReplaceWith,
      (ToolStripItem) this.tsmiSave,
      (ToolStripItem) this.tsmiSaveAs,
      (ToolStripItem) this.tsmiSaveCopyAs,
      (ToolStripItem) this.toolStripSeparator3,
      (ToolStripItem) this.tsmiExit
    });
    this.tsmiFile.Name = "tsmiFile";
    this.tsmiFile.Size = new Size(48 /*0x30*/, 20);
    this.tsmiFile.Text = "Файл";
    this.tsmiNew.Enabled = false;
    this.tsmiNew.Image = (Image) IDEInternalResources.IR_DocumentNew16;
    this.tsmiNew.Name = "tsmiNew";
    this.tsmiNew.ShortcutKeyDisplayString = "";
    this.tsmiNew.Size = new Size(194, 22);
    this.tsmiNew.Text = "Новый сценарий";
    this.toolStripSeparator1.Name = "toolStripSeparator1";
    this.toolStripSeparator1.Size = new Size(191, 6);
    this.tsmiOpen.Enabled = false;
    this.tsmiOpen.Image = (Image) IDEInternalResources.IR_DocumentOpen16;
    this.tsmiOpen.Name = "tsmiOpen";
    this.tsmiOpen.ShortcutKeys = Keys.O | Keys.Control;
    this.tsmiOpen.Size = new Size(194, 22);
    this.tsmiOpen.Text = "Открыть";
    this.tsmiReplaceWith.Enabled = false;
    this.tsmiReplaceWith.Image = (Image) IDEInternalResources.IR_DocumentReplaceWith16;
    this.tsmiReplaceWith.Name = "tsmiReplaceWith";
    this.tsmiReplaceWith.Size = new Size(194, 22);
    this.tsmiReplaceWith.Text = "Заменить";
    this.tsmiReplaceWith.ToolTipText = "Заменить другим сценарием текущий открытый сценарий";
    this.tsmiSave.Enabled = false;
    this.tsmiSave.Image = (Image) IDEInternalResources.IR_DocumentSave16;
    this.tsmiSave.Name = "tsmiSave";
    this.tsmiSave.ShortcutKeys = Keys.S | Keys.Control;
    this.tsmiSave.Size = new Size(194, 22);
    this.tsmiSave.Text = "Сохранить";
    this.tsmiSaveAs.Enabled = false;
    this.tsmiSaveAs.Image = (Image) IDEInternalResources.IR_DocumentSaveAs16;
    this.tsmiSaveAs.Name = "tsmiSaveAs";
    this.tsmiSaveAs.Size = new Size(194, 22);
    this.tsmiSaveAs.Text = "Сохранить как";
    this.toolStripSeparator3.Name = "toolStripSeparator3";
    this.toolStripSeparator3.Size = new Size(191, 6);
    this.tsmiExit.Image = (Image) IDEInternalResources.IR_AppExit16;
    this.tsmiExit.Name = "tsmiExit";
    this.tsmiExit.ShortcutKeys = Keys.F4 | Keys.Alt;
    this.tsmiExit.Size = new Size(194, 22);
    this.tsmiExit.Text = "Выход";
    this.tsmiExit.Click += new EventHandler(this.tsmiExit_Click);
    this.tsmiEdit.DropDownItems.AddRange(new ToolStripItem[12]
    {
      (ToolStripItem) this.tsmiCut,
      (ToolStripItem) this.tsmiCopy,
      (ToolStripItem) this.tsmiPaste,
      (ToolStripItem) this.toolStripSeparator10,
      (ToolStripItem) this.tsmiUndo,
      (ToolStripItem) this.tsmiRedo,
      (ToolStripItem) this.toolStripSeparator9,
      (ToolStripItem) this.tsmiFindReplace,
      (ToolStripItem) this.toolStripSeparator13,
      (ToolStripItem) this.tsmiCommentSelection,
      (ToolStripItem) this.tsmiUncommentSelection,
      (ToolStripItem) this.tsmiFormatIndents
    });
    this.tsmiEdit.Name = "tsmiEdit";
    this.tsmiEdit.Size = new Size(59, 20);
    this.tsmiEdit.Text = "Правка";
    this.tsmiCut.Enabled = false;
    this.tsmiCut.Image = (Image) IDEInternalResources.IR_Cut16;
    this.tsmiCut.Name = "tsmiCut";
    this.tsmiCut.ShortcutKeys = Keys.X | Keys.Control;
    this.tsmiCut.Size = new Size(209, 22);
    this.tsmiCut.Text = "Вырезать";
    this.tsmiCut.ToolTipText = "Вырезать";
    this.tsmiCopy.Enabled = false;
    this.tsmiCopy.Image = (Image) IDEInternalResources.IR_Copy16;
    this.tsmiCopy.Name = "tsmiCopy";
    this.tsmiCopy.ShortcutKeys = Keys.C | Keys.Control;
    this.tsmiCopy.Size = new Size(209, 22);
    this.tsmiCopy.Text = "Копировать";
    this.tsmiCopy.ToolTipText = "Копировать";
    this.tsmiPaste.Enabled = false;
    this.tsmiPaste.Image = (Image) IDEInternalResources.IR_Paste16;
    this.tsmiPaste.Name = "tsmiPaste";
    this.tsmiPaste.ShortcutKeys = Keys.V | Keys.Control;
    this.tsmiPaste.Size = new Size(209, 22);
    this.tsmiPaste.Text = "Вставить";
    this.tsmiPaste.ToolTipText = "Вставить";
    this.toolStripSeparator10.Name = "toolStripSeparator10";
    this.toolStripSeparator10.Size = new Size(206, 6);
    this.tsmiUndo.Enabled = false;
    this.tsmiUndo.Image = (Image) IDEInternalResources.IR_Undo16;
    this.tsmiUndo.Name = "tsmiUndo";
    this.tsmiUndo.ShortcutKeys = Keys.Z | Keys.Control;
    this.tsmiUndo.Size = new Size(209, 22);
    this.tsmiUndo.Text = "Отменить";
    this.tsmiUndo.ToolTipText = "Отменить";
    this.tsmiRedo.Enabled = false;
    this.tsmiRedo.Image = (Image) IDEInternalResources.IR_Redo16;
    this.tsmiRedo.Name = "tsmiRedo";
    this.tsmiRedo.ShortcutKeys = Keys.R | Keys.Control;
    this.tsmiRedo.Size = new Size(209, 22);
    this.tsmiRedo.Text = "Повторить";
    this.tsmiRedo.ToolTipText = "Повторить";
    this.toolStripSeparator9.Name = "toolStripSeparator9";
    this.toolStripSeparator9.Size = new Size(206, 6);
    this.tsmiFindReplace.Enabled = false;
    this.tsmiFindReplace.Image = (Image) IDEInternalResources.IR_FindReplace16;
    this.tsmiFindReplace.Name = "tsmiFindReplace";
    this.tsmiFindReplace.ShortcutKeyDisplayString = "Ctrl+F";
    this.tsmiFindReplace.Size = new Size(209, 22);
    this.tsmiFindReplace.Text = "Найти/заменить";
    this.toolStripSeparator13.Name = "toolStripSeparator13";
    this.toolStripSeparator13.Size = new Size(206, 6);
    this.tsmiCommentSelection.Enabled = false;
    this.tsmiCommentSelection.Image = (Image) IDEInternalResources.IR_CommentSelection16;
    this.tsmiCommentSelection.Name = "tsmiCommentSelection";
    this.tsmiCommentSelection.Size = new Size(209, 22);
    this.tsmiCommentSelection.Text = "Закомментировать";
    this.tsmiCommentSelection.ToolTipText = "Закомментировать выделенные строки";
    this.tsmiUncommentSelection.Enabled = false;
    this.tsmiUncommentSelection.Image = (Image) IDEInternalResources.IR_UncommentSelection16;
    this.tsmiUncommentSelection.Name = "tsmiUncommentSelection";
    this.tsmiUncommentSelection.Size = new Size(209, 22);
    this.tsmiUncommentSelection.Text = "Раскомментировать";
    this.tsmiUncommentSelection.ToolTipText = "Раскомментировать выделенные строки";
    this.tsmiFormatIndents.Enabled = false;
    this.tsmiFormatIndents.Image = (Image) IDEInternalResources.IR_FormatIndents16;
    this.tsmiFormatIndents.Name = "tsmiFormatIndents";
    this.tsmiFormatIndents.Size = new Size(209, 22);
    this.tsmiFormatIndents.Text = "Форматировать отступы";
    this.tsmiFormatIndents.ToolTipText = "Форматировать отступы";
    this.tsmiDebugAndRun.DropDownItems.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this.tsmiRun,
      (ToolStripItem) this.tsmiStopRun,
      (ToolStripItem) this.toolStripSeparator5,
      (ToolStripItem) this.tsmiEditRunParameters
    });
    this.tsmiDebugAndRun.Name = "tsmiDebugAndRun";
    this.tsmiDebugAndRun.Size = new Size(89, 20);
    this.tsmiDebugAndRun.Text = "Выполнение";
    this.tsmiRun.Enabled = false;
    this.tsmiRun.Image = (Image) IDEInternalResources.IR_Run16;
    this.tsmiRun.Name = "tsmiRun";
    this.tsmiRun.ShortcutKeys = Keys.F5;
    this.tsmiRun.Size = new Size(210, 22);
    this.tsmiRun.Text = "Выполнить";
    this.tsmiStopRun.Enabled = false;
    this.tsmiStopRun.Image = (Image) IDEInternalResources.IR_StopRun16;
    this.tsmiStopRun.Name = "tsmiStopRun";
    this.tsmiStopRun.Size = new Size(210, 22);
    this.tsmiStopRun.Text = "Остановить выполнение";
    this.toolStripSeparator5.Name = "toolStripSeparator5";
    this.toolStripSeparator5.Size = new Size(207, 6);
    this.tsmiEditRunParameters.Enabled = false;
    this.tsmiEditRunParameters.Name = "tsmiEditRunParameters";
    this.tsmiEditRunParameters.Size = new Size(210, 22);
    this.tsmiEditRunParameters.Text = "Параметры запуска...";
    this.tsmiSettings.DropDownItems.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this.tsmiEditIDESettings,
      (ToolStripItem) this.toolStripSeparator6,
      (ToolStripItem) this.tsmiEditExecutorSettings
    });
    this.tsmiSettings.Name = "tsmiSettings";
    this.tsmiSettings.Size = new Size(78, 20);
    this.tsmiSettings.Text = "Настройка";
    this.tsmiEditIDESettings.Enabled = false;
    this.tsmiEditIDESettings.Name = "tsmiEditIDESettings";
    this.tsmiEditIDESettings.Size = new Size(210, 22);
    this.tsmiEditIDESettings.Text = "Общие настройки...";
    this.tsmiEditIDESettings.ToolTipText = "Общие настройки...";
    this.toolStripSeparator6.Name = "toolStripSeparator6";
    this.toolStripSeparator6.Size = new Size(207, 6);
    this.tsmiEditExecutorSettings.Enabled = false;
    this.tsmiEditExecutorSettings.Name = "tsmiEditExecutorSettings";
    this.tsmiEditExecutorSettings.Size = new Size(210, 22);
    this.tsmiEditExecutorSettings.Text = "Исполнители сценариев";
    this.tsMainToolbar.Items.AddRange(new ToolStripItem[23]
    {
      (ToolStripItem) this.tsbNew,
      (ToolStripItem) this.toolStripSeparator4,
      (ToolStripItem) this.tsbOpen,
      (ToolStripItem) this.tsbReplaceWith,
      (ToolStripItem) this.tsbSave,
      (ToolStripItem) this.tsbSaveAs,
      (ToolStripItem) this.tsbSaveCopyAs,
      (ToolStripItem) this.toolStripSeparator2,
      (ToolStripItem) this.tsbCut,
      (ToolStripItem) this.tsbCopy,
      (ToolStripItem) this.tsbPaste,
      (ToolStripItem) this.toolStripSeparator11,
      (ToolStripItem) this.tsbUndo,
      (ToolStripItem) this.tsbRedo,
      (ToolStripItem) this.toolStripSeparator8,
      (ToolStripItem) this.tsbFindReplace,
      (ToolStripItem) this.toolStripSeparator12,
      (ToolStripItem) this.tsbCommentSelection,
      (ToolStripItem) this.tsbUncommentSelection,
      (ToolStripItem) this.tsbFormatIndents,
      (ToolStripItem) this.toolStripSeparator7,
      (ToolStripItem) this.tsbRun,
      (ToolStripItem) this.tsbStopRun
    });
    this.tsMainToolbar.Location = new Point(0, 24);
    this.tsMainToolbar.Name = "tsMainToolbar";
    this.tsMainToolbar.Size = new Size(624, 25);
    this.tsMainToolbar.TabIndex = 1;
    this.tsMainToolbar.Text = "toolStrip1";
    this.tsbNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbNew.Enabled = false;
    this.tsbNew.Image = (Image) IDEInternalResources.IR_DocumentNew16;
    this.tsbNew.ImageTransparentColor = Color.Magenta;
    this.tsbNew.Name = "tsbNew";
    this.tsbNew.Size = new Size(29, 22);
    this.tsbNew.Text = "Новый сценарий";
    this.toolStripSeparator4.Name = "toolStripSeparator4";
    this.toolStripSeparator4.Size = new Size(6, 25);
    this.tsbOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbOpen.Enabled = false;
    this.tsbOpen.Image = (Image) IDEInternalResources.IR_DocumentOpen16;
    this.tsbOpen.ImageTransparentColor = Color.Magenta;
    this.tsbOpen.Name = "tsbOpen";
    this.tsbOpen.Size = new Size(23, 22);
    this.tsbOpen.Text = "Открыть";
    this.tsbReplaceWith.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbReplaceWith.Enabled = false;
    this.tsbReplaceWith.Image = (Image) IDEInternalResources.IR_DocumentReplaceWith16;
    this.tsbReplaceWith.ImageTransparentColor = Color.Magenta;
    this.tsbReplaceWith.Name = "tsbReplaceWith";
    this.tsbReplaceWith.Size = new Size(23, 22);
    this.tsbReplaceWith.Text = "Заменить";
    this.tsbReplaceWith.ToolTipText = "Заменить другим сценарием текущий открытый сценарий";
    this.tsbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbSave.Enabled = false;
    this.tsbSave.Image = (Image) IDEInternalResources.IR_DocumentSave16;
    this.tsbSave.ImageTransparentColor = Color.Magenta;
    this.tsbSave.Name = "tsbSave";
    this.tsbSave.Size = new Size(23, 22);
    this.tsbSave.Text = "Сохранить";
    this.tsbSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbSaveAs.Enabled = false;
    this.tsbSaveAs.Image = (Image) IDEInternalResources.IR_DocumentSaveAs16;
    this.tsbSaveAs.ImageTransparentColor = Color.Magenta;
    this.tsbSaveAs.Name = "tsbSaveAs";
    this.tsbSaveAs.Size = new Size(23, 22);
    this.tsbSaveAs.Text = "Сохранить как";
    this.toolStripSeparator2.Name = "toolStripSeparator2";
    this.toolStripSeparator2.Size = new Size(6, 25);
    this.tsbCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbCut.Enabled = false;
    this.tsbCut.Image = (Image) IDEInternalResources.IR_Cut16;
    this.tsbCut.ImageTransparentColor = Color.Magenta;
    this.tsbCut.Name = "tsbCut";
    this.tsbCut.Size = new Size(23, 22);
    this.tsbCut.Text = "Вырезать";
    this.tsbCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbCopy.Enabled = false;
    this.tsbCopy.Image = (Image) IDEInternalResources.IR_Copy16;
    this.tsbCopy.ImageTransparentColor = Color.Magenta;
    this.tsbCopy.Name = "tsbCopy";
    this.tsbCopy.Size = new Size(23, 22);
    this.tsbCopy.Text = "Копировать";
    this.tsbPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbPaste.Enabled = false;
    this.tsbPaste.Image = (Image) IDEInternalResources.IR_Paste16;
    this.tsbPaste.ImageTransparentColor = Color.Magenta;
    this.tsbPaste.Name = "tsbPaste";
    this.tsbPaste.Size = new Size(23, 22);
    this.tsbPaste.Text = "Вставить";
    this.toolStripSeparator11.Name = "toolStripSeparator11";
    this.toolStripSeparator11.Size = new Size(6, 25);
    this.tsbUndo.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbUndo.Enabled = false;
    this.tsbUndo.Image = (Image) IDEInternalResources.IR_Undo16;
    this.tsbUndo.ImageTransparentColor = Color.Magenta;
    this.tsbUndo.Name = "tsbUndo";
    this.tsbUndo.Size = new Size(23, 22);
    this.tsbUndo.Text = "Отменить";
    this.tsbRedo.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbRedo.Enabled = false;
    this.tsbRedo.Image = (Image) IDEInternalResources.IR_Redo16;
    this.tsbRedo.ImageTransparentColor = Color.Magenta;
    this.tsbRedo.Name = "tsbRedo";
    this.tsbRedo.Size = new Size(23, 22);
    this.tsbRedo.Text = "Повторить";
    this.toolStripSeparator8.Name = "toolStripSeparator8";
    this.toolStripSeparator8.Size = new Size(6, 25);
    this.tsbFindReplace.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbFindReplace.Enabled = false;
    this.tsbFindReplace.Image = (Image) IDEInternalResources.IR_FindReplace16;
    this.tsbFindReplace.ImageTransparentColor = Color.Magenta;
    this.tsbFindReplace.Name = "tsbFindReplace";
    this.tsbFindReplace.Size = new Size(23, 22);
    this.tsbFindReplace.Text = "Найти/заменить";
    this.toolStripSeparator12.Name = "toolStripSeparator12";
    this.toolStripSeparator12.Size = new Size(6, 25);
    this.tsbCommentSelection.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbCommentSelection.Enabled = false;
    this.tsbCommentSelection.Image = (Image) IDEInternalResources.IR_CommentSelection16;
    this.tsbCommentSelection.Name = "tsbCommentSelection";
    this.tsbCommentSelection.Size = new Size(23, 22);
    this.tsbCommentSelection.Text = "Закомментировать";
    this.tsbCommentSelection.ToolTipText = "Закомментировать выделенные строки";
    this.tsbUncommentSelection.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbUncommentSelection.Enabled = false;
    this.tsbUncommentSelection.Image = (Image) IDEInternalResources.IR_UncommentSelection16;
    this.tsbUncommentSelection.Name = "tsbUncommentSelection";
    this.tsbUncommentSelection.Size = new Size(23, 22);
    this.tsbUncommentSelection.Text = "Раскомментировать";
    this.tsbUncommentSelection.ToolTipText = "Раскомментировать выделенные строки";
    this.tsbFormatIndents.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbFormatIndents.Enabled = false;
    this.tsbFormatIndents.Image = (Image) IDEInternalResources.IR_FormatIndents16;
    this.tsbFormatIndents.ImageTransparentColor = Color.Magenta;
    this.tsbFormatIndents.Name = "tsbFormatIndents";
    this.tsbFormatIndents.Size = new Size(23, 22);
    this.tsbFormatIndents.Text = "Форматировать отступы";
    this.toolStripSeparator7.Name = "toolStripSeparator7";
    this.toolStripSeparator7.Size = new Size(6, 25);
    this.tsbRun.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbRun.Enabled = false;
    this.tsbRun.Image = (Image) IDEInternalResources.IR_Run16;
    this.tsbRun.ImageTransparentColor = Color.Magenta;
    this.tsbRun.Name = "tsbRun";
    this.tsbRun.Size = new Size(23, 22);
    this.tsbRun.Text = "Выполнить";
    this.tsbStopRun.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbStopRun.Enabled = false;
    this.tsbStopRun.Image = (Image) IDEInternalResources.IR_StopRun16;
    this.tsbStopRun.ImageTransparentColor = Color.Magenta;
    this.tsbStopRun.Name = "tsbStopRun";
    this.tsbStopRun.Size = new Size(23, 22);
    this.tsbStopRun.Text = "Остановить выполнение";
    this.dpScriptWindow.Dock = DockStyle.Fill;
    this.dpScriptWindow.DocumentStyle = DocumentStyle.DockingWindow;
    this.dpScriptWindow.Location = new Point(0, 49);
    this.dpScriptWindow.Name = "dpScriptWindow";
    this.dpScriptWindow.Size = new Size(624, 370);
    this.dpScriptWindow.TabIndex = 2;
    this.tsbSaveCopyAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbSaveCopyAs.Image = (Image) IDEInternalResources.IR_DocumentSaveAll16;
    this.tsbSaveCopyAs.ImageTransparentColor = Color.Magenta;
    this.tsbSaveCopyAs.Name = "tsbSaveCopyAs";
    this.tsbSaveCopyAs.Size = new Size(23, 22);
    this.tsbSaveCopyAs.Text = "Сохранить копию как";
    this.tsbSaveCopyAs.Enabled = false;
    this.tsmiSaveCopyAs.Name = "tsmiSaveCopyAs";
    this.tsmiSaveCopyAs.Image = (Image) IDEInternalResources.IR_DocumentSaveAll16;
    this.tsmiSaveCopyAs.Size = new Size(194, 22);
    this.tsmiSaveCopyAs.Text = "Сохранить копию как";
    this.tsmiSaveCopyAs.Enabled = false;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(624, 441);
    this.Controls.Add((Control) this.dpScriptWindow);
    this.Controls.Add((Control) this.tsMainToolbar);
    this.Controls.Add((Control) this.ssMainStatusBar);
    this.Controls.Add((Control) this.mmMainMenu);
    this.Icon = IDEInternalResources.IR_IDE;
    this.KeyPreview = true;
    this.MainMenuStrip = this.mmMainMenu;
    this.MinimumSize = new Size(640, 480);
    this.Name = nameof (IDEWindow);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Script pad";
    this.WindowState = FormWindowState.Maximized;
    this.FormClosing += new FormClosingEventHandler(this.IDEWindow_FormClosing);
    this.FormClosed += new FormClosedEventHandler(this.IDEWindow_FormClosed);
    this.ssMainStatusBar.ResumeLayout(false);
    this.ssMainStatusBar.PerformLayout();
    this.mmMainMenu.ResumeLayout(false);
    this.mmMainMenu.PerformLayout();
    this.tsMainToolbar.ResumeLayout(false);
    this.tsMainToolbar.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
