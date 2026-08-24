// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StartClass
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ApplicationModel;
using Intermech.Controls;
using Intermech.Globalization;
using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Services;
using Intermech.UI;
using Intermech.UI.ExceptionHandling;
using Intermech.UI.Winforms;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal class StartClass
{
  [STAThread]
  private static void Main()
  {
    UICultureHelper.ApplySettingsFromConfigurationFile();
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    StartClass.InitializeSystemServices();
    Application.Run((Form) new WizardForm());
  }

  private static void InitializeSystemServices()
  {
    UIDispatcherService dispatcherService = UIDispatcherService.FromCurrentUIThread();
    ServicesManager.ServiceContainer.AddService(typeof (IUIDispatcherService), (object) dispatcherService);
    ExceptionHandlerService serviceInstance = new ExceptionHandlerService((IUIDispatcherService) dispatcherService, new Func<Exception, DialogResult>(StartClass.ShowUnhandledExceptionDialog));
    ServicesManager.ServiceContainer.AddService(typeof (IExceptionDisplayService), (object) serviceInstance);
    ServicesManager.ServiceContainer.AddService(typeof (IExceptionHandlerService), (object) serviceInstance);
  }

  private static DialogResult ShowUnhandledExceptionDialog(Exception exception)
  {
    using (ExceptionForm exceptionForm = new ExceptionForm())
    {
      exceptionForm.ViewModel.ConfigureCloseCommandAsAbortAction();
      exceptionForm.ViewModel.SaveHandler = (ExceptionSaveHandler) new ImpExpExceptionSaveHandler();
      exceptionForm.ViewModel.Exception = exception;
      return exceptionForm.ShowDialogWithOwner();
    }
  }
}
