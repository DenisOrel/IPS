// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.DockPanelWindowCollection`2
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp.Components;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal abstract class DockPanelWindowCollection<TWindowControl, TWindow> : 
  IWindowCollection<TWindow>
  where TWindowControl : DockContent, TWindow
{
  private readonly DockPanel dockPanel;
  private bool closeableWindows;

  public DockPanelWindowCollection(DockPanel dockPanel)
  {
    this.dockPanel = dockPanel != null ? dockPanel : throw new ArgumentNullException(nameof (dockPanel));
    this.dockPanel.ActiveDocumentChanged += new EventHandler(this.OnActiveDockDocumentChanged);
  }

  public bool CloseableWindows
  {
    [DebuggerStepThrough] get => this.closeableWindows;
    set
    {
      if (this.closeableWindows == value)
        return;
      this.closeableWindows = value;
      this.ChangeCloseableStateForAllWindows(value);
    }
  }

  private void ChangeCloseableStateForAllWindows(bool newValue)
  {
    foreach (IDockContent document in this.dockPanel.Documents)
    {
      if (document is TWindowControl windowControl)
        this.ChangeCloseableState(windowControl, newValue);
    }
  }

  private void ChangeCloseableState(TWindowControl windowControl, bool newValue)
  {
    if (newValue)
    {
      windowControl.CloseButton = true;
      windowControl.CloseButtonVisible = true;
      windowControl.FormClosing += new FormClosingEventHandler(this.OnWindowClosing);
      windowControl.FormClosed += new FormClosedEventHandler(this.OnWindowClosed);
    }
    else
    {
      windowControl.CloseButton = false;
      windowControl.CloseButtonVisible = false;
      windowControl.FormClosing -= new FormClosingEventHandler(this.OnWindowClosing);
      windowControl.FormClosed -= new FormClosedEventHandler(this.OnWindowClosed);
    }
  }

  public TWindow AddWindow()
  {
    TWindowControl windowControl = this.DoCreateWindowControl();
    windowControl.DockAreas = DockAreas.Document;
    this.ChangeCloseableState(windowControl, this.closeableWindows);
    windowControl.Show(this.dockPanel, DockState.Document);
    return (TWindow) windowControl;
  }

  public void RemoveWindow(TWindow window)
  {
    if ((object) window == null)
      throw new ArgumentNullException(nameof (window));
    ((TWindowControl) (object) window).Close();
  }

  public TWindow ActiveWindow
  {
    get
    {
      IDockContent activeDocument = this.dockPanel.ActiveDocument;
      return activeDocument != null && activeDocument is TWindowControl ? (TWindow) activeDocument : default (TWindow);
    }
    set
    {
      if ((object) value == null)
        throw new ArgumentNullException(nameof (value));
      ((TWindowControl) (object) value).Activate();
    }
  }

  private void OnActiveDockDocumentChanged(object sender, EventArgs e)
  {
    if (this.ActiveWindowChanged == null)
      return;
    this.ActiveWindowChanged((object) this, EventArgs.Empty);
  }

  private void OnWindowClosing(object sender, FormClosingEventArgs e)
  {
    EventHandler windowClosing = this.WindowClosing;
    if (windowClosing == null)
      return;
    windowClosing(sender, EventArgs.Empty);
  }

  private void OnWindowClosed(object sender, FormClosedEventArgs e)
  {
    EventHandler windowClosed = this.WindowClosed;
    if (windowClosed == null)
      return;
    windowClosed(sender, EventArgs.Empty);
  }

  public event EventHandler ActiveWindowChanged;

  public event EventHandler WindowClosing;

  public event EventHandler WindowClosed;

  protected abstract TWindowControl DoCreateWindowControl();
}
