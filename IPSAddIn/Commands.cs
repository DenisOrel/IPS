// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Commands
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using DXP;
using Intermech.Runtime.ComInterop;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace CSharpPlugin;

public static class Commands
{
  public static void ProjectImportCommand(IServerDocumentView argView, ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__0.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__0.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ImportProject", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__0.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__0.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedProjectFileName);
  }

  public static void ProjectPropertiesViewCommand(
    IServerDocumentView argView,
    ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ViewDocumentProperties", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__1.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedProjectFileName);
  }

  public static void ProjectSaveChangesCommand(
    IServerDocumentView argView,
    ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SaveChanges", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__2.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedProjectFileName);
  }

  public static void CreateSpecificationCommand(
    IServerDocumentView argView,
    ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CreateSpecification", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__3.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedProjectFileName);
  }

  public static void CreateElementListCommand(IServerDocumentView argView, ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CreateElementList", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__4.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedProjectFileName);
  }

  public static void ProjectExtendedSaveCommand(
    IServerDocumentView argView,
    ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ExtendedSave", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__5.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedProjectFileName);
  }

  public static void OpenIPSProjectCommand(IServerDocumentView argContext, ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenProject", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__6.\u003C\u003Ep__0, application);
    }), Commands.FocusedProjectFileName);
  }

  public static void DocumentPropertiesViewCommand(
    IServerDocumentView argContext,
    ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ViewDocumentProperties", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__7.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedDocumentFileName);
  }

  public static void DocumentSaveChangesCommand(
    IServerDocumentView argContext,
    ref string argParameters)
  {
    Commands.COMMethod((Commands.UnsafeCOMMethodHandler) ((fileName, application) =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SaveChanges", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__8.\u003C\u003Ep__0, application, fileName);
    }), Commands.FocusedDocumentFileName);
  }

  private static string FocusedProjectFileName
  {
    get => GlobalVars.DXPWorkSpace.DM_FocusedProject().DM_ProjectFullPath();
  }

  private static string FocusedDocumentFileName
  {
    get => GlobalVars.DXPWorkSpace.DM_FocusedDocument().DM_FullPath();
  }

  private static void COMMethod(Commands.UnsafeCOMMethodHandler method, string fileName)
  {
    Commands.ExecuteComMethod((Action<object>) (application =>
    {
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Action<CallSite, Commands.UnsafeCOMMethodHandler, string, object>>.Create(Binder.Invoke(CSharpBinderFlags.ResultDiscarded, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__14.\u003C\u003Ep__0, method, fileName, application);
    }));
  }

  internal static void ExecuteComMethod(Action<object> action)
  {
    try
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      ProgIdProvider progIdProvider = new ProgIdProvider("IPS.ADIntegratorAPI", false);
      object obj1 = progIdProvider.TryGetRunningInstance() ?? progIdProvider.CreateInstance();
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Action<CallSite, Action<object>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Invoke", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__15.\u003C\u003Ep__0, action, obj1);
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__15.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__15.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__15.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__15.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p2 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ErrorCode", typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__1.Target((CallSite) Commands.\u003C\u003Eo__15.\u003C\u003Ep__1, obj1);
      object obj3 = target2((CallSite) p2, obj2, 0);
      if (!target1((CallSite) p3, obj3))
        return;
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__15.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__15.\u003C\u003Ep__5 = CallSite<Action<CallSite, Type, object, string, Commands.MessageIconIdxs>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertMessagesIntoMessagePanel", (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Type, object, string, Commands.MessageIconIdxs> target3 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Type, object, string, Commands.MessageIconIdxs>> p5 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__5;
      Type type = typeof (Commands);
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__15.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__15.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ErrorMessage", typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Commands.\u003C\u003Eo__15.\u003C\u003Ep__4.Target((CallSite) Commands.\u003C\u003Eo__15.\u003C\u003Ep__4, obj1);
      target3((CallSite) p5, type, obj4, "Ошибка", Commands.MessageIconIdxs.Error);
    }
    catch (Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(ex.Message);
      stringBuilder.AppendLine(ex.StackTrace);
      Commands.InsertMessagesIntoMessagePanel(stringBuilder.ToString(), "Ошибка", Commands.MessageIconIdxs.Error);
    }
  }

  internal static void ThrowClientException(Exception exception)
  {
    Commands.ExecuteComMethod((Action<object>) (app =>
    {
      // ISSUE: reference to a compiler-generated field
      if (Commands.\u003C\u003Eo__16.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Commands.\u003C\u003Eo__16.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, Exception>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (ThrowClientException), (IEnumerable<Type>) null, typeof (Commands), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Commands.\u003C\u003Eo__16.\u003C\u003Ep__0.Target((CallSite) Commands.\u003C\u003Eo__16.\u003C\u003Ep__0, app, exception);
    }));
  }

  private static void InsertMessagesIntoMessagePanel(
    string message,
    string caption,
    Commands.MessageIconIdxs iconIdx)
  {
    IDXPWorkSpace dxpWorkSpace = GlobalVars.DXPWorkSpace;
    if (dxpWorkSpace == null)
      return;
    IDXPMessagesManager dxpMessagesManager = dxpWorkSpace.DM_MessagesManager();
    if (dxpMessagesManager == null)
      return;
    dxpMessagesManager.ClearMessages();
    dxpMessagesManager.BeginUpdate();
    dxpMessagesManager.AddMessage(caption, message, Consts.ModuleName, string.Empty, string.Empty, string.Empty, (int) iconIdx, false, string.Empty, string.Empty);
    dxpMessagesManager.EndUpdate();
    dxpWorkSpace.DM_ShowMessageView();
  }

  private delegate void UnsafeCOMMethodHandler(string fileName, object application);

  private enum MessageIconIdxs
  {
    OK = 3,
    Error = 4,
  }
}
