// Decompiled with JetBrains decompiler
// Type: Intermech.Navigator.SimpleExcelReports
// Assembly: Intermech.Navigator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FA68CCDA-C8AC-453D-A97D-7A56D5366A1E
// Assembly location: D:\IPS\Client\Intermech.Navigator.dll

using Intermech.Interfaces.Client;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Navigator;

internal class SimpleExcelReports : ISimpleExcelReports
{
  private const string progID = "Excel.Application";

  public object GetExcelInstance() => this.GetExcelInstance((object) null, string.Empty);

  public object GetExcelInstance(object instance)
  {
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, SimpleExcelReports, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, nameof (GetExcelInstance), (IEnumerable<Type>) null, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return SimpleExcelReports.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__2.\u003C\u003Ep__0, this, instance, string.Empty);
  }

  public object GetExcelInstance(object instance, string caption)
  {
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target1 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__1.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p1 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__1;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj1 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__0, instance, (object) null);
    if (target1((CallSite) p1, obj1))
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p3 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Visible", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__2.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__2, instance);
        int num = target2((CallSite) p3, obj2) ? 1 : 0;
        if (!string.IsNullOrEmpty(caption))
        {
          // ISSUE: reference to a compiler-generated field
          if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Caption", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__4.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__4, instance, caption);
        }
      }
      catch
      {
        instance = (object) null;
      }
    }
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target3 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__6.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p6 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__6;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj4 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__5.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__5, instance, (object) null);
    if (target3((CallSite) p6, obj4))
      return instance;
    try
    {
      instance = Marshal.GetActiveObject("Excel.Application");
    }
    catch
    {
    }
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__8 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target4 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__8.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p8 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__8;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj5 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__7.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__7, instance, (object) null);
    if (target4((CallSite) p8, obj5))
    {
      try
      {
        instance = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
        // ISSUE: reference to a compiler-generated field
        if (SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Caption", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__9.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__3.\u003C\u003Ep__9, instance, caption);
      }
      catch
      {
      }
    }
    return instance;
  }

  public void ReleaseExcelInstance(object instance)
  {
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target = SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__1.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p1 = SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__1;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj = SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__0, instance, (object) null);
    if (target((CallSite) p1, obj))
      return;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReleaseComObject", (IEnumerable<Type>) null, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__2.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__4.\u003C\u003Ep__2, typeof (Marshal), instance);
    GC.GetTotalMemory(true);
  }

  public void SetVisible(object instance, bool visible)
  {
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target = SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p1 = SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__1;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj1 = SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__0, instance, (object) null);
    if (!target((CallSite) p1, obj1))
      return;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Visible", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj2 = SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__2.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__5.\u003C\u003Ep__2, instance, visible);
  }

  public object CreateWorkbook(
    object instance,
    string caption,
    string title,
    string author,
    string company)
  {
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Func<CallSite, SimpleExcelReports, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "GetExcelInstance", (IEnumerable<Type>) null, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    instance = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__0, this, instance);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target1 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__2.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p2 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__2;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj1 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__1.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__1, instance, (object) null);
    if (target1((CallSite) p2, obj1))
      throw new ArgumentNullException(nameof (instance));
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Workbooks", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj2 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__3.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__3, instance);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Add", (IEnumerable<Type>) null, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object workbook = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__4.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__4, obj2);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BuiltinDocumentProperties", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj3 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__5.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__5, workbook);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj4 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__6.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__6, obj3, "Title", title);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, string, string, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj5 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__7.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__7, obj3, "Company", company);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__8 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Worksheets", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj6 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__8.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__8, workbook);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__10 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, int, object> target2 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__10.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, int, object>> p10 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__10;
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__9 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Item", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj7 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__9.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__9, obj6);
    object obj8 = target2((CallSite) p10, obj7, 1);
    // ISSUE: reference to a compiler-generated field
    if (SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__11 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Name", typeof (SimpleExcelReports), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj9 = SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__11.Target((CallSite) SimpleExcelReports.\u003C\u003Eo__6.\u003C\u003Ep__11, obj8, caption);
    return workbook;
  }
}
