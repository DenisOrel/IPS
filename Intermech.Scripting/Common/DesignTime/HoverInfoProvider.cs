// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.HoverInfoProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class HoverInfoProvider(ICodeModel codeModel) : CodeModelServiceProvider<IHoverInfoService>(codeModel)
{
  public HoverInfo GetHoverInfo(int offset)
  {
    IHoverInfoService service = this.TryGetService();
    try
    {
      HoverInfo hoverInfo = service.GetHoverInfo(offset);
      this.Errors.Reset();
      return hoverInfo;
    }
    catch
    {
      this.Errors.RegisterError();
      throw;
    }
  }

  public HoverInfo TryGetHoverInfoIfPossible(int offset)
  {
    if (!this.IsSupportedAndAllowed)
      return (HoverInfo) null;
    try
    {
      return this.GetHoverInfo(offset);
    }
    catch (Exception ex)
    {
      this.CodeModelRecoveryAction(ex);
      return (HoverInfo) null;
    }
  }
}
