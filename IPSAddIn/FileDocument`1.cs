// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.FileDocument`1
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;

#nullable disable
namespace CSharpPlugin;

internal abstract class FileDocument<TDocument> : Parametrable<TDocument>
{
  protected readonly string fileName;
  protected readonly IPSAddInProxy parent;
  private readonly object _syncRoot = new object();
  protected bool setParameters;

  public FileDocument(TDocument document, string fileName, IPSAddInProxy parent)
    : base(document)
  {
    this.fileName = fileName;
    this.parent = parent;
  }

  public string FilePath => this.fileName;

  public override string InternalId => this.fileName;

  public override Parameter[] Parameters
  {
    get => base.Parameters;
    set
    {
      lock (this._syncRoot)
      {
        try
        {
          this.setParameters = true;
          base.Parameters = value;
          this.SaveDocument();
        }
        finally
        {
          this.setParameters = false;
        }
      }
    }
  }

  protected abstract void SaveDocument();
}
