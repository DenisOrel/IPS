// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.BackgroundCompositionReader
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using System;
using System.Threading;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class BackgroundCompositionReader
{
  private Thread _thread;
  private bool _abort;

  public void Start(BackgroundCompositionReaderArgs args)
  {
    this._abort = false;
    this._thread = new Thread(new ParameterizedThreadStart(this.ThreadMethod))
    {
      IsBackground = true,
      Name = $"PDM.CompositionReader_{Guid.NewGuid()}"
    };
    this._thread.Start((object) args);
  }

  public void Stop()
  {
    this._abort = true;
    if (this._thread == null)
      return;
    this._thread.Abort();
    this._thread = (Thread) null;
  }

  private void StateChanged(BackgroundState state)
  {
    CompositionReaderChangeStateDelegate changeStateEvent = this.CompositionReaderChangeStateEvent;
    if (changeStateEvent == null)
      return;
    changeStateEvent((object) this, new CompositionReaderChangeStateEventArgs(state));
  }

  private void StateChanged(BackgroundState state, CompositionItem[] result)
  {
    CompositionReaderChangeStateDelegate changeStateEvent = this.CompositionReaderChangeStateEvent;
    if (changeStateEvent == null)
      return;
    changeStateEvent((object) this, new CompositionReaderChangeStateEventArgs(state, result));
  }

  private void SetError(Exception error)
  {
    CompositionReaderChangeStateDelegate changeStateEvent = this.CompositionReaderChangeStateEvent;
    if (changeStateEvent == null)
      return;
    changeStateEvent((object) this, new CompositionReaderChangeStateEventArgs(error));
  }

  private void ThreadMethod(object args)
  {
    ObjectsCompositionComparer compositionComparer = (ObjectsCompositionComparer) null;
    try
    {
      BackgroundCompositionReaderArgs compositionReaderArgs = (BackgroundCompositionReaderArgs) args;
      this.StateChanged(BackgroundState.Reading);
      CompositionItem compositionItem1 = (CompositionItem) compositionReaderArgs.Item1.Clone();
      CompositionItem compositionItem2 = (CompositionItem) compositionReaderArgs.Item2.Clone();
      compositionComparer = new ObjectsCompositionComparer(compositionReaderArgs.RuleID);
      compositionComparer.Compare(compositionItem1, compositionReaderArgs.Filtration1, compositionItem2, compositionReaderArgs.Filtration2, compositionReaderArgs.Recursive);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        bool checkAttributesExists = ((ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService))).CheckExistsAttributes(compositionReaderArgs.RuleID);
        AttributesCompareFacade attributesCompareFacade = new AttributesCompareFacade(compositionReaderArgs.RuleID);
        attributesCompareFacade.CompareItems(sessionKeeper.Session, compositionItem1, compositionItem2, checkAttributesExists);
        compositionItem1.Handled = true;
        compositionItem2.Handled = true;
        attributesCompareFacade.CompareChildItems(sessionKeeper.Session, compositionItem1, compositionItem2, checkAttributesExists);
      }
      if (this._abort)
        return;
      this.StateChanged(BackgroundState.Fill, new CompositionItem[2]
      {
        compositionItem1,
        compositionItem2
      });
    }
    catch (ThreadAbortException ex)
    {
      compositionComparer?.Abort();
      this.StateChanged(BackgroundState.Empty);
    }
    catch (Exception ex)
    {
      if (!this._abort)
        this.SetError(ex);
      else
        this.StateChanged(BackgroundState.Empty);
    }
  }

  public event CompositionReaderChangeStateDelegate CompositionReaderChangeStateEvent;
}
