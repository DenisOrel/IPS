// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IDEChildPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal abstract class IDEChildPresenter
{
  private IDEPresenter idePresenter;
  private IIDEView ideView;
  private bool isAttachedToView;

  protected IDEChildPresenter(IDEPresenter idePresenter)
  {
    this.idePresenter = idePresenter != null ? idePresenter : throw new ArgumentNullException(nameof (idePresenter));
  }

  protected IDEPresenter IDEPresenter
  {
    [DebuggerStepThrough] get => this.idePresenter;
  }

  protected IIDEView IDEView
  {
    [DebuggerStepThrough] get => this.ideView;
  }

  public void AttachView()
  {
    if (this.isAttachedToView)
      throw new InvalidOperationException("The child presenter is already attached.");
    this.DoValidate();
    this.OnAttachView();
    this.isAttachedToView = true;
  }

  public void DetachView(bool fullDetach)
  {
    if (!this.isAttachedToView)
      return;
    this.OnDetachView(fullDetach);
    this.isAttachedToView = false;
  }

  public bool IsAttachedToView
  {
    [DebuggerStepThrough] get => this.isAttachedToView;
  }

  protected virtual void DoValidate()
  {
    if (this.idePresenter.View == null)
      throw new InvalidOperationException("The IDE presenter must have a view.");
  }

  protected virtual void OnAttachView() => this.ideView = this.idePresenter.View;

  protected virtual void OnDetachView(bool fullDetach) => this.ideView = (IIDEView) null;

  protected void PostToViewThread(Action method)
  {
    if (method == null)
      throw new ArgumentNullException(nameof (method));
    if (!this.IDEPresenter.IsAttachedToView)
      return;
    SynchronizationContext synchronizationContext = this.IDEPresenter.SynchronizationContext;
    if (synchronizationContext == null)
      return;
    SendOrPostCallback d = (SendOrPostCallback) (arg =>
    {
      if (!this.IDEPresenter.IsAttachedToView)
        return;
      method();
    });
    synchronizationContext.Post(d, (object) null);
  }
}
