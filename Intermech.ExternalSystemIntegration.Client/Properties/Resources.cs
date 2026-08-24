// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.Properties.Resources
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
      if (Intermech.ExternalSystemIntegration.Client.Properties.Resources.resourceMan == null)
        Intermech.ExternalSystemIntegration.Client.Properties.Resources.resourceMan = new ResourceManager("Intermech.ExternalSystemIntegration.Client.Properties.Resources", typeof (Intermech.ExternalSystemIntegration.Client.Properties.Resources).Assembly);
      return Intermech.ExternalSystemIntegration.Client.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.ExternalSystemIntegration.Client.Properties.Resources.resourceCulture;
    set => Intermech.ExternalSystemIntegration.Client.Properties.Resources.resourceCulture = value;
  }
}
