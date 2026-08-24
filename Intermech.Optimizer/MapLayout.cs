// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayout
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace Intermech.Map.Layout;

public abstract class MapLayout : Component
{
  private MapLayoutProgressEventArgs myEventArgs;
  private MapDocument myGoDoc;
  private MapLayoutNetwork myMapLayoutNetwork;
  internal static Assembly myVersionAssembly;
  internal static string myVersionName = "";

  public event MapLayoutProgressEventHandler Progress;

  static MapLayout() => MapLayout.myVersionAssembly = (Assembly) null;

  protected MapLayout()
  {
    this.myGoDoc = (MapDocument) null;
    this.myMapLayoutNetwork = (MapLayoutNetwork) null;
    this.myEventArgs = new MapLayoutProgressEventArgs(0.0f, "");
  }

  protected virtual void OnProgress(MapLayoutProgressEventArgs evt)
  {
    if (this.Progress == null)
      return;
    this.Progress((object) this, evt);
  }

  public virtual void PerformLayout()
  {
  }

  public void RaiseProgress(float done) => this.RaiseProgress(done, "");

  public virtual void RaiseProgress(float done, string msg)
  {
    this.myEventArgs.Progress = done;
    this.myEventArgs.Message = msg;
    this.OnProgress(this.myEventArgs);
  }

  [Browsable(false)]
  public virtual MapDocument Document
  {
    get => this.myGoDoc;
    set
    {
      if (this.myGoDoc == value)
        return;
      this.myGoDoc = value;
      this.Network = (MapLayoutNetwork) null;
    }
  }

  [Browsable(false)]
  public virtual MapLayoutNetwork Network
  {
    get => this.myMapLayoutNetwork;
    set => this.myMapLayoutNetwork = value;
  }

  public static float Version => 2.2f;

  public static string VersionName
  {
    get => "2.2.1.0";
    set
    {
      MapLayout.myVersionName = value;
      MapLayout.myVersionAssembly = Assembly.GetCallingAssembly();
    }
  }
}
