// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.CodeFoldingProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class CodeFoldingProvider(ICodeModel codeModel) : 
  CodeModelServiceProvider<ICodeFoldingService>(codeModel)
{
  public IList<FoldingRegionItem> GetFoldingRegions()
  {
    ICodeFoldingService service = this.TryGetService();
    try
    {
      IList<FoldingRegionItem> foldingRegions = service.GetFoldingRegions();
      this.Errors.Reset();
      return foldingRegions;
    }
    catch
    {
      this.Errors.RegisterError();
      throw;
    }
  }

  public IList<FoldingRegionItem> TryGetFoldingRegionsIfPossible()
  {
    if (!this.IsSupportedAndAllowed)
      return (IList<FoldingRegionItem>) null;
    try
    {
      return this.GetFoldingRegions();
    }
    catch (Exception ex)
    {
      this.CodeModelRecoveryAction(ex);
      return (IList<FoldingRegionItem>) null;
    }
  }
}
