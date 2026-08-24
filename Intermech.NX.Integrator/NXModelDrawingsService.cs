// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXModelDrawingsService
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface.ModelDrawings;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXModelDrawingsService(IIntegrator owner) : SuffixBasedModelDrawingsService(owner, NXConsts.AnyFileExtension, NXConsts.AnyFileExtension)
{
  private bool useDefaultSuffixOnly;

  public bool UseDefaultSuffixOnly
  {
    [DebuggerStepThrough] get
    {
      lock (this.Integrator.SyncRoot)
        return this.useDefaultSuffixOnly;
    }
    [DebuggerStepThrough] set
    {
      lock (this.Integrator.SyncRoot)
      {
        this.RequireNotInitialized();
        this.useDefaultSuffixOnly = value;
      }
    }
  }

  public NXModelDrawingsService CloneUninitialized()
  {
    NXModelDrawingsService modelDrawingsService = new NXModelDrawingsService(this.Integrator);
    modelDrawingsService.UseDefaultSuffixOnly = this.UseDefaultSuffixOnly;
    modelDrawingsService.SettingsProvider = this.SettingsProvider;
    modelDrawingsService.OutputService = this.OutputService;
    modelDrawingsService.LicenseService = this.LicenseService;
    return modelDrawingsService;
  }

  protected override ICollection<string> DoGeneratePossibleSuffixesFromSettings(
    ICollection<string> drawingSuffixes)
  {
    if (this.UseDefaultSuffixOnly)
      drawingSuffixes = (ICollection<string>) drawingSuffixes.Take<string>(1).ToArray<string>();
    List<string> suffixesFromSettings = new List<string>(drawingSuffixes.Count * 10);
    suffixesFromSettings.AddRange((IEnumerable<string>) drawingSuffixes);
    foreach (string drawingSuffix in (IEnumerable<string>) drawingSuffixes)
    {
      if (!char.IsDigit(drawingSuffix[drawingSuffix.Length - 1]))
      {
        for (int index = 1; index <= 10; ++index)
        {
          string str = drawingSuffix + index.ToString();
          if (!suffixesFromSettings.Contains(str))
            suffixesFromSettings.Add(str);
        }
      }
    }
    return (ICollection<string>) suffixesFromSettings;
  }
}
