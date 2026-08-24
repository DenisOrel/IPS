// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.CodeNavigationProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class CodeNavigationProvider(ICodeModel codeModel) : 
  CodeModelServiceProvider<ICodeNavigationService>(codeModel)
{
  public IList<NavigationItem> GetNavigationItems()
  {
    ICodeNavigationService service = this.TryGetService();
    try
    {
      IList<NavigationItem> navigationItems = service.GetNavigationItems();
      this.Errors.Reset();
      return navigationItems;
    }
    catch
    {
      this.Errors.RegisterError();
      throw;
    }
  }

  public IList<NavigationItem> TryGetNavigationItemsIfPossible()
  {
    if (!this.IsSupportedAndAllowed)
      return (IList<NavigationItem>) null;
    try
    {
      return this.GetNavigationItems();
    }
    catch (Exception ex)
    {
      this.CodeModelRecoveryAction(ex);
      return (IList<NavigationItem>) null;
    }
  }
}
