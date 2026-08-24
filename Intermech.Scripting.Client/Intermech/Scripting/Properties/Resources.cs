// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Properties.Resources
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Scripting.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Resources()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (Intermech.Scripting.Properties.Resources.resourceMan == null)
        Intermech.Scripting.Properties.Resources.resourceMan = new ResourceManager("Intermech.Scripting.Properties.Resources", typeof (Intermech.Scripting.Properties.Resources).Assembly);
      return Intermech.Scripting.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.Scripting.Properties.Resources.resourceCulture;
    set => Intermech.Scripting.Properties.Resources.resourceCulture = value;
  }
}
