// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Properties.Resources
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Requirement.Properties;

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
      if (Intermech.Requirement.Properties.Resources.resourceMan == null)
        Intermech.Requirement.Properties.Resources.resourceMan = new ResourceManager("Intermech.Requirement.Properties.Resources", typeof (Intermech.Requirement.Properties.Resources).Assembly);
      return Intermech.Requirement.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.Requirement.Properties.Resources.resourceCulture;
    set => Intermech.Requirement.Properties.Resources.resourceCulture = value;
  }

  internal static string AttributeFileNotFound
  {
    get
    {
      return Intermech.Requirement.Properties.Resources.ResourceManager.GetString(nameof (AttributeFileNotFound), Intermech.Requirement.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap check
  {
    get => (Bitmap) Intermech.Requirement.Properties.Resources.ResourceManager.GetObject(nameof (check), Intermech.Requirement.Properties.Resources.resourceCulture);
  }

  internal static string ErrorUser
  {
    get => Intermech.Requirement.Properties.Resources.ResourceManager.GetString(nameof (ErrorUser), Intermech.Requirement.Properties.Resources.resourceCulture);
  }

  internal static string FileNotFound
  {
    get => Intermech.Requirement.Properties.Resources.ResourceManager.GetString(nameof (FileNotFound), Intermech.Requirement.Properties.Resources.resourceCulture);
  }

  internal static string NotCheckout
  {
    get => Intermech.Requirement.Properties.Resources.ResourceManager.GetString(nameof (NotCheckout), Intermech.Requirement.Properties.Resources.resourceCulture);
  }

  internal static string TZCompleted
  {
    get => Intermech.Requirement.Properties.Resources.ResourceManager.GetString(nameof (TZCompleted), Intermech.Requirement.Properties.Resources.resourceCulture);
  }

  internal static string TZNotCompleted
  {
    get => Intermech.Requirement.Properties.Resources.ResourceManager.GetString(nameof (TZNotCompleted), Intermech.Requirement.Properties.Resources.resourceCulture);
  }

  internal static string TZNotCompletedMessage
  {
    get
    {
      return Intermech.Requirement.Properties.Resources.ResourceManager.GetString(nameof (TZNotCompletedMessage), Intermech.Requirement.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap uncheck
  {
    get
    {
      return (Bitmap) Intermech.Requirement.Properties.Resources.ResourceManager.GetObject(nameof (uncheck), Intermech.Requirement.Properties.Resources.resourceCulture);
    }
  }
}
