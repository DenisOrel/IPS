// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpSessionParameters
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common;
using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpSessionParameters : ILanguageSessionParameters, ICloneable
{
  private IScriptOutputStream stdout;

  public IScriptOutputStream Stdout
  {
    get => this.stdout;
    set => this.stdout = value;
  }

  public CSharpSessionParameters Clone()
  {
    return new CSharpSessionParameters()
    {
      Stdout = this.Stdout
    };
  }

  object ICloneable.Clone() => (object) this.Clone();
}
