// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.MbsDocument
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using CSharpPlugin.Draftsman.Logical;
using CSharpPlugin.Draftsman.Parameters;
using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace CSharpPlugin;

internal sealed class MbsDocument : 
  FileDocument<IMbsDocument>,
  IMbsDocument,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IFileDocument,
  IDisposable
{
  private string _tempFolder;
  private readonly string _parametersFileName;
  private readonly string _logicalFileName;
  private List<ParameterRecord> _parameters;
  private Root _logicalFile;
  private readonly JsonSerializerSettings _logicalJsonSettings;
  private readonly JsonSerializerSettings _parametersJsonSettings;
  private bool _loaded;
  private int _lastId;

  public MbsDocument(IPSAddInProxy parent, string fileName)
  {
    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
    serializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
    serializerSettings.DateParseHandling = DateParseHandling.None;
    serializerSettings.Converters.Add((JsonConverter) TypeEnumConverter.Singleton);
    serializerSettings.Converters.Add((JsonConverter) new IsoDateTimeConverter()
    {
      DateTimeStyles = DateTimeStyles.AssumeUniversal
    });
    this._logicalJsonSettings = serializerSettings;
    this._parametersJsonSettings = new JsonSerializerSettings()
    {
      MetadataPropertyHandling = MetadataPropertyHandling.Ignore
    };
    // ISSUE: explicit constructor call
    base.\u002Ector((IMbsDocument) null, fileName, parent);
    this.Load();
  }

  private void Load()
  {
    this._tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    Directory.CreateDirectory(this._tempFolder);
    ZipFile.ExtractToDirectory(this.fileName, this._tempFolder);
    this._parameters = JsonConvert.DeserializeObject<List<ParameterRecord>>(File.ReadAllText(Path.Combine(this._tempFolder, this._parametersFileName), Encoding.UTF8), this._parametersJsonSettings);
    string json = File.ReadAllText(Path.Combine(this._tempFolder, this._logicalFileName), Encoding.UTF8);
    this._logicalFile = JsonConvert.DeserializeObject<Root>(json, this._logicalJsonSettings);
    this._lastId = this.LoadLastParameterId(json);
    this._loaded = true;
  }

  private int LoadLastParameterId(string json)
  {
    MatchCollection matchCollection = new Regex("\\\"id\\\"\\s{0,}:\\s{0,}(?<id>\\d{1,})").Matches(json);
    int num = 0;
    foreach (System.Text.RegularExpressions.Match match in matchCollection)
    {
      int result;
      if (int.TryParse(match.Groups["id"].Value, out result) && num < result)
        num = result;
    }
    return num;
  }

  protected override void SaveDocument()
  {
    this.parent.CloseObject(this.fileName);
    string contents1 = JsonConvert.SerializeObject((object) this._parameters, Formatting.Indented, this._parametersJsonSettings);
    File.WriteAllText(Path.Combine(this._tempFolder, this._parametersFileName), contents1);
    string contents2 = JsonConvert.SerializeObject((object) this._logicalFile, Formatting.Indented, this._logicalJsonSettings);
    File.WriteAllText(Path.Combine(this._tempFolder, this._logicalFileName), contents2);
    string str = Path.Combine(Path.GetDirectoryName(this.fileName), this.fileName + ".old");
    FileAttributes attributes = File.GetAttributes(this.fileName);
    File.Copy(this.fileName, str, true);
    this.DeleteFile(this.fileName);
    try
    {
      ZipFile.CreateFromDirectory(this._tempFolder, this.fileName, CompressionLevel.Fastest, false);
    }
    catch
    {
      File.Copy(str, this.fileName);
      File.SetAttributes(this.fileName, attributes);
    }
    finally
    {
      Directory.Delete(this._tempFolder, true);
      this.DeleteFile(str);
    }
    this._loaded = false;
  }

  private void DeleteFile(string deleteFileName)
  {
    File.SetAttributes(deleteFileName, FileAttributes.Normal);
    File.Delete(deleteFileName);
  }

  private void CheckLoad()
  {
    if (this._loaded)
      return;
    this.Load();
  }

  protected override Intermech.AltiumDesigner.Interfaces.Parameter[] GetParameters()
  {
    this.CheckLoad();
    return this._parameters.ConvertAll<Intermech.AltiumDesigner.Interfaces.Parameter>((Converter<ParameterRecord, Intermech.AltiumDesigner.Interfaces.Parameter>) (x => new Intermech.AltiumDesigner.Interfaces.Parameter(x.Name, (object) x.Value, false, typeof (string)))).ToArray();
  }

  public List<MbsModule> Modules
  {
    get
    {
      this.CheckLoad();
      List<MbsModule> modules = new List<MbsModule>();
      if (this._logicalFile.Modules != null && this._logicalFile.Modules.Count > 0)
      {
        foreach (Module module in this._logicalFile.Modules)
        {
          MbsModule mbsModule = new MbsModule()
          {
            Designator = module.Designator,
            SourceProject = module.Source.SourceProject
          };
          if (module.Parameters != null && module.Parameters.Count > 0)
            mbsModule.Parameters = module.Parameters.ConvertAll<Intermech.AltiumDesigner.Interfaces.Parameter>((Converter<CSharpPlugin.Draftsman.Logical.Parameter, Intermech.AltiumDesigner.Interfaces.Parameter>) (x => new Intermech.AltiumDesigner.Interfaces.Parameter(x.Name, (object) x.Value, false, typeof (string))));
          modules.Add(mbsModule);
        }
      }
      return modules;
    }
  }

  protected override void WriteNewParameter(Intermech.AltiumDesigner.Interfaces.Parameter parameter)
  {
    this.CheckLoad();
    this._parameters.Add(new ParameterRecord()
    {
      Name = parameter.Name,
      Value = parameter.Value?.ToString()
    });
    if (this.setParameters)
      return;
    this.SaveDocument();
  }

  protected override void WriteParameterValue(Intermech.AltiumDesigner.Interfaces.Parameter parameter)
  {
    this.CheckLoad();
    this._parameters.Find((Predicate<ParameterRecord>) (x => x.Name.Equals(parameter.Name))).Value = parameter.Value?.ToString();
    if (this.setParameters)
      return;
    this.SaveDocument();
  }

  public void SetActualVariants(List<ActualVariant> actualVariants)
  {
    this.CheckLoad();
    bool flag = false;
    foreach (ActualVariant actualVariant in actualVariants)
    {
      ActualVariant variant = actualVariant;
      Module module = this._logicalFile.Modules.Find((Predicate<Module>) (x => x.Source.SourceProject.Equals(variant.ProjectFile)));
      if (module != null)
      {
        CSharpPlugin.Draftsman.Logical.Parameter parameter = module.Parameters.Find((Predicate<CSharpPlugin.Draftsman.Logical.Parameter>) (x => x.Name.Equals(UseVariantParameter.ParameterName, StringComparison.OrdinalIgnoreCase)));
        if (parameter != null)
        {
          if (!string.Equals(parameter.Value, variant.VariantDescription, StringComparison.OrdinalIgnoreCase))
          {
            parameter.Value = variant.VariantDescription;
            flag = true;
          }
        }
        else
        {
          module.Parameters.Add(new CSharpPlugin.Draftsman.Logical.Parameter()
          {
            Name = UseVariantParameter.ParameterName,
            Value = variant.VariantDescription,
            Id = (long) ++this._lastId
          });
          flag = true;
        }
      }
    }
    if (!flag)
      return;
    this.SaveDocument();
  }
}
