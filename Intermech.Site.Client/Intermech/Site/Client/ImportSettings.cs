// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImportSettings
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.PropertyEditors;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ImportSettings : TransferSettings<IImportRulesService>
{
  private long _importedObjectOwner;
  private long _baseVersionTemplate;
  private long _defaultImbaseFolder;
  private long _importCompleteTemplate;
  private long _importErrorTemplate;
  private bool _centralizedNSI = true;
  private bool _rewriteArchive = true;
  private bool _renameCoincidenceFileNames;

  public override void OnApply(IImportRulesService service)
  {
    service.DefaultObjectOwner = this._importedObjectOwner;
    service.BaseVersionTemplate = this._baseVersionTemplate;
    service.DefaultImbaseFolder = this._defaultImbaseFolder;
    service.ImportCompleteTemplate = this._importCompleteTemplate;
    service.ImportErrorTemplate = this._importErrorTemplate;
    service.CentralizedNSI = this._centralizedNSI;
    service.RewriteArchive = this._rewriteArchive;
    service.RenameCoincidenceFileNames = this._renameCoincidenceFileNames;
  }

  public override void OnLoad(IImportRulesService service)
  {
    this._importedObjectOwner = service.DefaultObjectOwner;
    this._baseVersionTemplate = service.BaseVersionTemplate;
    this._defaultImbaseFolder = service.DefaultImbaseFolder;
    this._importCompleteTemplate = service.ImportCompleteTemplate;
    this._importErrorTemplate = service.ImportErrorTemplate;
    this._centralizedNSI = service.CentralizedNSI;
    this._rewriteArchive = service.RewriteArchive;
    this._renameCoincidenceFileNames = service.RenameCoincidenceFileNames;
  }

  [DisplayName("Владелец импортированного объекта")]
  [Description("Пользователь, которого назначать владельцем импортируемых из портала объектов в случае, если невозможно определить настоящего владельца.")]
  public UserPropertyClass ImportedObjectOwner
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._importedObjectOwner != 0L ? new UserPropertyClass(this._importedObjectOwner) : (UserPropertyClass) null;
    }
    set => this._importedObjectOwner = value != null ? value.ObjectID : 0L;
  }

  [DisplayName("Процесс согласования базовой версии")]
  [Description("Бизнес процесс для импортированной версии объекта для согласования взаимозаменяемости с имеющейся базовой версией этого объекта.")]
  [DefaultValue(null)]
  public TemplatePropertyClass BaseVersionTemplate
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._baseVersionTemplate != 0L ? new TemplatePropertyClass(this._baseVersionTemplate) : (TemplatePropertyClass) null;
    }
    set => this._baseVersionTemplate = value != null ? value.ObjectID : 0L;
  }

  [CustomDescription("Attribute.Site.Client_20")]
  [CustomDisplayName("Attribute.Site.Client_19")]
  [DefaultValue(null)]
  public ImbaseFolderPropertyClass DefaultImbaseFolder
  {
    get
    {
      this.CheckInited();
      return this._defaultImbaseFolder != 0L ? new ImbaseFolderPropertyClass(this._defaultImbaseFolder) : (ImbaseFolderPropertyClass) null;
    }
    set => this._defaultImbaseFolder = value != null ? value.ObjectID : 0L;
  }

  [DisplayName("Процесс с результатами импорта")]
  [Description("Бизнес процесс об обновлении (создании) объектов в системе в результате импорта.")]
  [DefaultValue(null)]
  public TemplatePropertyClass ImportCompleteTemplate
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._importCompleteTemplate != 0L ? new TemplatePropertyClass(this._importCompleteTemplate) : (TemplatePropertyClass) null;
    }
    set => this._importCompleteTemplate = value != null ? value.ObjectID : 0L;
  }

  [DisplayName("Процесс ошибки импорта")]
  [Description("Бизнес процесс, запускаемый в случае возникновения ошибки импорта.")]
  [DefaultValue(null)]
  public TemplatePropertyClass ImportErrorTemplate
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._importErrorTemplate != 0L ? new TemplatePropertyClass(this._importErrorTemplate) : (TemplatePropertyClass) null;
    }
    set => this._importErrorTemplate = value != null ? value.ObjectID : 0L;
  }

  [DisplayName("Централизованная НСИ")]
  [Description("Централизованная НСИ")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool CentralizedNSI
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._centralizedNSI;
    }
    set => this._centralizedNSI = value;
  }

  [DisplayName("Обновлять атрибут Архив")]
  [Description("При импорте существующих в текущей базе объектов обновлять атрибут Архив у этих объектов")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool RewriteArchive
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._rewriteArchive;
    }
    set => this._rewriteArchive = value;
  }

  [DisplayName("Переименовывать совпадающие имена файлов")]
  [Description("При совпадении имени файла существующего в базе и импортируемого объекта, автоматически переименовывать файл последнего")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool RenameCoincidenceFileNames
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._renameCoincidenceFileNames;
    }
    set => this._renameCoincidenceFileNames = value;
  }
}
