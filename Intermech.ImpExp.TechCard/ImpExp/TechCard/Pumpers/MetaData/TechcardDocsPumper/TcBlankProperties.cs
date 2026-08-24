// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper.TcBlankProperties
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Document.Model;
using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.DocsPump;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.TechCard;
using Intermech.Kernel.Search;
using Intermech.TechCard.Document.Interfaces.Configs.Common;
using Intermech.TechCard.Document.Interfaces.Configs.Interfaces;
using Intermech.TechCard.Document.Interfaces.Configs.Serialization.Save;
using Intermech.TechCard.Document.Interfaces.Configs.Structure;
using Intermech.TechCard.Document.Interfaces.Configs.Structure.FieldContents;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper;

[TaskDescription("Инициализация данных для перекачки - настройки документов TechCard", "Перекачка данных - настройки документов TechCard")]
[TaskType(PumperType.MetaData)]
internal class TcBlankProperties(PluginClass plugin) : PumpClass(plugin)
{
  private string _sqlText = "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20} FROM {21} WHERE {12} > 0";
  private string _blankDir = "";
  public Dictionary<long, Rules> DsList;
  private readonly List<DopTypes> _orders = new List<DopTypes>();
  private long lastObjID;
  protected IImportingData ImportDataMain;

  protected override Guid GUID { get; } = new Guid("{cadd99ae-306c-11d8-b4e9-00304f19f545}");

  public override void Pump()
  {
    if (!TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.DocumentSettings))
    {
      this.plugin.appManager.AddInfoMessage("Перекачка настроек документов TechCard отключена в настройках");
      this.PumpCheckPoint("Перекачка данных отключена", 0);
    }
    else
    {
      if (TechCache.isResumeMode || this.IsMetadataPumper)
      {
        SavePoint savePoint = TechCache.SavePoint;
        if (savePoint != null && savePoint.PumpGuid == this.GUID)
          this.AnalyzeData();
      }
      DocumentPlugin.InitDocumentPlugin();
      this.LoadImportData_Main();
      if (this._blankDir == "")
        this.UpdateBlankDir();
      this.DsList = new Dictionary<long, Rules>();
      GroupDocuments groupDocuments = (GroupDocuments) null;
      this.PumpCheckPoint("Перекачка настроек типов документов", 0);
      this._sqlText = string.Format(this._sqlText, (object) "F_KEY", (object) "F_OB", (object) "F_NAME", (object) "BLANK", (object) "DIO", (object) "WMF", (object) "OPCART", (object) "PEREH", (object) "OPER", (object) "LOPER", (object) "OSN", (object) "LOSN", (object) "F_PRODUCTION", (object) "F_AUTO", (object) "F_SORT", (object) "F_DOCS_KIND", (object) "F_LANG", (object) "F_NUM_START", (object) "F_NUM_NEXT", (object) "F_NUM_LENGTH", (object) "F_GROUP", (object) "TC_DOCS");
      IDbCommand command1 = TechcardConsts.ConnectionManager.CreateCommand();
      command1.CommandText = this._sqlText;
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      using (IDataReader dataReader = command1.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        try
        {
          while (dataReader.Read())
          {
            object[] values = new object[dataReader.FieldCount];
            dataReader.GetValues(values);
            if (values[0] != null && values[0] != DBNull.Value && values[1] != null && values[1] != DBNull.Value && values[2] != null && values[2] != DBNull.Value && values[3] != null && values[3] != DBNull.Value)
            {
              int int32_1 = Convert.ToInt32(values[0]);
              string str1 = Convert.ToString(values[1]);
              string str2 = Convert.ToString(values[2]);
              string str3 = Convert.ToString(values[3]);
              string blankFileName = this._blankDir + str3;
              ImDocumentData template;
              try
              {
                if (!TemplateUtils.GetTemplate(blankFileName, out template))
                {
                  this.plugin.appManager.AddWarningMessage("Не удалось получить шаблон для бланка : " + blankFileName);
                  continue;
                }
              }
              catch (Exception ex)
              {
                this.plugin.appManager.AddWarningMessage(ex.Message);
                continue;
              }
              template.Designation = str1;
              Rules imds = new Rules();
              imds.Template = template;
              this.DsList.Add((long) int32_1, imds);
              this.CreatePropsFromTemplate(imds);
              imds.FullName = str2;
              imds.BlankNote = str3;
              imds.ShortName = str1;
              if (values[4] != null && values[4] != DBNull.Value)
              {
                switch (Convert.ToString(values[4]))
                {
                  case "A":
                    imds.Properties.DocumentType = DocumentOwnership.Album;
                    break;
                  case "D":
                    imds.Properties.DocumentType = DocumentOwnership.Process;
                    break;
                  case "I":
                    imds.Properties.DocumentType = DocumentOwnership.Article;
                    break;
                  case "K":
                    imds.Properties.DocumentType = DocumentOwnership.Complect;
                    break;
                  case "L":
                    imds.Properties.DocumentType = DocumentOwnership.OperGroup;
                    break;
                  case "O":
                    imds.Properties.DocumentType = DocumentOwnership.Operation;
                    break;
                  case "T":
                    imds.Properties.DocumentType = DocumentOwnership.InstrumentPosition;
                    break;
                }
              }
              if (values[5] != null && values[5] != DBNull.Value)
              {
                string str4 = Convert.ToString(values[5]);
                imds.Properties.SketchDocument = str4 == "1";
              }
              if (values[6] != null && values[6] != DBNull.Value)
              {
                switch (Convert.ToString(values[6]))
                {
                  case "M":
                    imds.Properties.RouteCard = true;
                    imds.Properties.OperatingCard = false;
                    break;
                  case "O":
                    imds.Properties.RouteCard = false;
                    imds.Properties.OperatingCard = true;
                    break;
                  default:
                    imds.Properties.RouteCard = false;
                    imds.Properties.OperatingCard = false;
                    break;
                }
              }
              if (values[7] != null && values[7] != DBNull.Value)
              {
                switch (Convert.ToString(values[7]))
                {
                  case "1":
                    imds.Properties.StepSetup = StepSetupType.StringsOtpNotAlternate;
                    break;
                  case "2":
                    imds.Properties.StepSetup = StepSetupType.SolidText;
                    break;
                  default:
                    imds.Properties.StepSetup = StepSetupType.StringsOtpAlternate;
                    break;
                }
              }
              if (values[8] != null && values[8] != DBNull.Value)
              {
                string str5 = Convert.ToString(values[8]);
                imds.Properties.OperationalList = str5 == "1";
              }
              if (values[9] != null && values[9] != DBNull.Value)
              {
                string str6 = Convert.ToString(values[9]);
                imds.Properties.EmptyStringBeforeOperation = str6 == "1";
              }
              if (values[10] != null && values[10] != DBNull.Value)
              {
                switch (Convert.ToString(values[10]))
                {
                  case "1":
                    imds.Properties.ToolSetup = ToolSetupType.OnToolType;
                    break;
                  case "2":
                    imds.Properties.ToolSetup = ToolSetupType.SolidText;
                    break;
                  default:
                    imds.Properties.ToolSetup = ToolSetupType.InLine;
                    break;
                }
              }
              if (values[11] != null && values[11] != DBNull.Value)
              {
                string str7 = Convert.ToString(values[11]);
                imds.Properties.ShowToolType = str7 == "1";
              }
              if (values[12] != null && values[12] != DBNull.Value)
              {
                int int32_2 = Convert.ToInt32(values[12]);
                imds.Properties.Production = int32_2;
              }
              if (values[13] != null && values[13] != DBNull.Value)
              {
                DocMask int64 = (DocMask) Convert.ToInt64(values[13]);
                imds.Properties.PickingCard = int64.HasFlag((Enum) DocMask.dmComplCardStructure);
                imds.Properties.NoRepeatTool = int64.HasFlag((Enum) DocMask.dmDontRepeatOsn);
                imds.Properties.DoNotNumberPages = int64.HasFlag((Enum) DocMask.dmNoPageNumbers);
                imds.Properties.PlaceToolIntoEmptyFields = int64.HasFlag((Enum) DocMask.dmPlaceOsnIntoEmptyFlds);
                imds.Properties.NewShopSetup = !int64.HasFlag((Enum) DocMask.dmNewCehFromNewHeadList) ? (!int64.HasFlag((Enum) DocMask.dmNewCehFromNewList) ? NewShopSetupType.OnSelectPage : NewShopSetupType.OnNewPage) : NewShopSetupType.OnCapitalPage;
                imds.Properties.EnterInContents = int64.HasFlag((Enum) DocMask.dmAddToOglav);
                imds.Properties.ForPartDocument = int64.HasFlag((Enum) DocMask.dmDocByDetal);
                imds.Properties.DocumentNotInSet = int64.HasFlag((Enum) DocMask.dmDocOutOfComplect);
                imds.Properties.MaterialSetup = int64.HasFlag((Enum) DocMask.dmVspMatByPlainText) ? MaterialSetupType.SolidText : MaterialSetupType.InLine;
              }
              if (values[14] != null && values[14] != DBNull.Value)
              {
                long int64 = Convert.ToInt64(values[14]);
                imds.Properties.Sorting = int64;
              }
              if (values[15] != null && values[15] != DBNull.Value)
              {
                switch (Convert.ToInt32(values[15]))
                {
                  case -4:
                    imds.Properties.Statement = false;
                    imds.Properties.PickingCard = false;
                    imds.Properties.ShopToolList = false;
                    imds.Properties.Contents = true;
                    break;
                  case -1:
                    imds.Properties.Statement = true;
                    imds.Properties.PickingCard = false;
                    imds.Properties.ShopToolList = false;
                    imds.Properties.Contents = false;
                    break;
                  case 1:
                    imds.Properties.Statement = false;
                    imds.Properties.PickingCard = true;
                    imds.Properties.ShopToolList = false;
                    imds.Properties.Contents = false;
                    break;
                  case 2:
                    imds.Properties.Statement = false;
                    imds.Properties.PickingCard = false;
                    imds.Properties.ShopToolList = true;
                    imds.Properties.Contents = false;
                    break;
                  default:
                    imds.Properties.Statement = false;
                    imds.Properties.PickingCard = false;
                    imds.Properties.ShopToolList = false;
                    imds.Properties.Contents = false;
                    break;
                }
              }
              if (values[16 /*0x10*/] != null && values[16 /*0x10*/] != DBNull.Value)
              {
                int int32_3 = Convert.ToInt32(values[16 /*0x10*/]);
                imds.Properties.Language = (long) int32_3;
              }
              if (values[17] != null && values[17] != DBNull.Value)
              {
                int int32_4 = Convert.ToInt32(values[17]);
                imds.Properties.FirstNumberPageInDocument = int32_4;
              }
              if (values[18] != null && values[18] != DBNull.Value)
              {
                int int32_5 = Convert.ToInt32(values[18]);
                imds.Properties.NumberingInterval = int32_5;
              }
              if (values[19] != null && values[19] != DBNull.Value)
              {
                int int32_6 = Convert.ToInt32(values[19]);
                imds.Properties.CharactersInDocumentNumber = int32_6;
              }
              if (values[20] != null && values[20] != DBNull.Value)
              {
                int int32_7 = Convert.ToInt32(values[20]);
                if (int32_7 > 0)
                {
                  if (groupDocuments == null)
                  {
                    groupDocuments = new GroupDocuments();
                    groupDocuments.Pump(userSession);
                  }
                  QuickObjectInfo documentInfoByTcKey = groupDocuments.GetGroupDocumentInfoByTcKey(int32_7);
                  if (!documentInfoByTcKey.Empty)
                    imds.Properties.DocumentGroup = documentInfoByTcKey.VersionGuid;
                }
              }
            }
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
      string str8 = $"SELECT {"F_DOC_KEY"}, {"F_FLAGS"}, {"F_COMMENT"}, {"F_NUMBER"}, {"F_CONDID"}, {"F_IDENTIFIER"}, {"F_PARENTID"}, {"F_RECORDID"} FROM {"TC_DOCS_SETUP"}";
      IDbCommand command2 = TechcardConsts.ConnectionManager.CreateCommand();
      command2.CommandText = str8;
      using (IDataReader dataReader = command2.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        try
        {
          while (dataReader.Read())
          {
            object[] values = new object[dataReader.FieldCount];
            dataReader.GetValues(values);
            if (values[0] != null && values[0] != DBNull.Value)
            {
              int int32 = Convert.ToInt32(values[0]);
              if (this.DsList.ContainsKey((long) int32))
              {
                Rules ds = this.DsList[(long) int32];
                string str9 = "";
                if (values[5] != null && values[5] != DBNull.Value)
                  str9 = Convert.ToString(values[5]);
                if (!(str9 == ""))
                {
                  string originTechcardTemplate = "";
                  int key1 = 0;
                  Guid objTypeGuid = Guid.Empty;
                  if (values[7] != null && values[7] != DBNull.Value)
                    key1 = Convert.ToInt32(values[7]);
                  TechExpert.TypeConverter.GetAttributeItemByCode(str9, this.plugin, out string _);
                  AttributeSettings attribute1 = FormulaTemplateConverter.EntityToAttribute(str9, this.plugin);
                  if (attribute1 == null)
                  {
                    if (values[2] != null && values[2] != DBNull.Value)
                      originTechcardTemplate = Convert.ToString(values[2]);
                    if (originTechcardTemplate == string.Empty && key1 <= 0)
                    {
                      this.plugin.appManager.AddWarningMessage("Не удалось найти атрибут " + str9);
                      continue;
                    }
                  }
                  ds.Template.FindNode(str9);
                  int num1 = 0;
                  if (values[1] != null && values[1] != DBNull.Value)
                    num1 = Convert.ToInt32(values[1]);
                  int num2 = 0;
                  if (values[3] != null && values[3] != DBNull.Value)
                    num2 = Convert.ToInt32(values[3]);
                  int key2 = 0;
                  if (values[4] != null && values[4] != DBNull.Value)
                    key2 = Convert.ToInt32(values[4]);
                  if (values[6] != null && values[6] != DBNull.Value)
                    Convert.ToString(values[6]);
                  TechTypeList techTypeList = TechPumpData.TechType.TechTypeList;
                  if (techTypeList.ContainsKey(key1))
                    objTypeGuid = techTypeList[key1].TypeSett.ObjType;
                  if (key1 > 0)
                  {
                    if (ds.Properties.FindOrCreateElement(str9, DocumentConfigElementType.Variant) is VariantConfig orCreateElement1)
                    {
                      orCreateElement1.Id = str9;
                      orCreateElement1.OnDetail = (num1 & 2) > 0;
                      orCreateElement1.Number = num2;
                      orCreateElement1.ObjType = MetaDataHelper.GetObjectType(objTypeGuid);
                      if (key2 == 0)
                      {
                        orCreateElement1.Condition = (IFieldContents) null;
                      }
                      else
                      {
                        IFieldContents fieldContents = SetCondition.Set((object) this.GetCondition(key2));
                        orCreateElement1.Condition = fieldContents;
                      }
                    }
                  }
                  else if (string.Compare(str9, "OLE", StringComparison.OrdinalIgnoreCase) == 0)
                  {
                    if (ds.Properties.FindOrCreateElement(str9, DocumentConfigElementType.PictureField) is PictureFieldConfig orCreateElement2)
                    {
                      orCreateElement2.Id = str9;
                      orCreateElement2.SketchType = SketchTypes.Ole;
                      orCreateElement2.SketchField = true;
                    }
                  }
                  else
                  {
                    IFieldContents fieldContents1 = (IFieldContents) null;
                    if (attribute1 == null && !string.IsNullOrEmpty(originTechcardTemplate))
                      fieldContents1 = FormulaTemplateConverter.ConvertTemplate(originTechcardTemplate, this.plugin);
                    if (fieldContents1 == null && (num1 & 1) > 0)
                    {
                      if (ds.Properties.FindOrCreateElement(str9, DocumentConfigElementType.PictureField) is PictureFieldConfig orCreateElement3)
                      {
                        orCreateElement3.Id = str9;
                        orCreateElement3.SketchType = SketchTypes.Dwg;
                        orCreateElement3.SketchField = true;
                      }
                    }
                    else if (ds.Properties.FindOrCreateElement(str9, DocumentConfigElementType.TextField) is TextFieldConfig orCreateElement4)
                    {
                      orCreateElement4.Id = str9;
                      orCreateElement4.Digits = num2;
                      orCreateElement4.NotRepeated = (num1 & 1) > 0;
                      orCreateElement4.CalcOnFill = (num1 & 4) > 0;
                      orCreateElement4.FieldContents = fieldContents1;
                      if (orCreateElement4.FieldContents == null)
                      {
                        AttributeSettings attribute2 = FormulaTemplateConverter.EntityToAttribute(str9, this.plugin);
                        if (attribute2 != null)
                          orCreateElement4.FieldContents = CreateFieldContent.FieldContentsByObject((object) attribute2);
                      }
                      if (key2 == 0)
                      {
                        orCreateElement4.Condition = (IFieldContents) null;
                      }
                      else
                      {
                        IFieldContents fieldContents2 = SetCondition.Set((object) this.GetCondition(key2));
                        orCreateElement4.Condition = fieldContents2;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
      foreach (KeyValuePair<long, Rules> ds in this.DsList)
      {
        Rules rules = ds.Value;
        if (rules?.Template != null)
        {
          IEnumerable<DocumentTreeNode> nodesRecursive = rules.Template.NodesRecursive;
          if (nodesRecursive != null && nodesRecursive.Any<DocumentTreeNode>())
          {
            foreach (DocumentTreeNode documentTreeNode in nodesRecursive)
            {
              if (rules.Properties.FindElement(documentTreeNode.Id) == null && documentTreeNode is ContainerData && rules.Properties.FindOrCreateElement(documentTreeNode.Id, DocumentConfigElementType.PictureField) is PictureFieldConfig orCreateElement)
              {
                orCreateElement.Id = documentTreeNode.Id;
                orCreateElement.SketchType = documentTreeNode.Id.StartsWith("ole", true, CultureInfo.InvariantCulture) ? SketchTypes.Ole : SketchTypes.Dwg;
                orCreateElement.SketchField = true;
              }
            }
            foreach (DocumentTreeNode documentTreeNode in nodesRecursive)
            {
              if (rules.Properties.FindElement(documentTreeNode.Id) == null)
              {
                string attributeValue = documentTreeNode.GetAttributeValue("BLN.ID", true);
                if (!string.IsNullOrEmpty(attributeValue) && !(documentTreeNode.Id == attributeValue))
                {
                  IDocumentConfigElement element = rules.Properties.FindElement(attributeValue);
                  if (element != null && element.Clone() is DocumentConfigElement documentConfigElement)
                  {
                    documentConfigElement.Id = documentTreeNode.Id;
                    rules.Properties.Elements.Add((IDocumentConfigElement) documentConfigElement);
                  }
                }
              }
            }
          }
        }
      }
      this.ReadOrders();
      this.WriteObjects(userSession);
      this.ImportDataMain = (IImportingData) null;
      if (ServicesManager.GetService(typeof (ICache)) is ICache service)
      {
        ImportingCategory[] importingCategoryArray = new ImportingCategory[2]
        {
          ImportingCategory.TechcardDocumentSetup,
          ImportingCategory.TechExpObjStruct
        };
        service.ReleaseCache(importingCategoryArray);
      }
      this.DsList.Clear();
      this.DsList = (Dictionary<long, Rules>) null;
      this.PumpCheckPoint("Перекачка настроек типов документов успешно завершена", 100);
    }
  }

  private void AddOrder(int ownerId, int childId)
  {
    TechTypeList techTypeList = TechPumpData.TechType.TechTypeList;
    foreach (DopTypes order in this._orders)
    {
      if (order.TcId == ownerId && techTypeList.ContainsKey(childId))
      {
        TechTypeInfo techTypeInfo = techTypeList[childId];
        order.Childs.Add(techTypeInfo.TypeSett.ObjType);
        return;
      }
    }
    DopTypes dopTypes = new DopTypes() { TcId = ownerId };
    if (techTypeList.ContainsKey(ownerId) && techTypeList.ContainsKey(childId))
    {
      TechTypeInfo techTypeInfo1 = techTypeList[ownerId];
      dopTypes.ObjectGuid = techTypeInfo1.TypeSett.ObjType;
      TechTypeInfo techTypeInfo2 = techTypeList[childId];
      dopTypes.Childs.Add(techTypeInfo2.TypeSett.ObjType);
    }
    this._orders.Add(dopTypes);
  }

  private void ReadOrders()
  {
    string str = string.Format("SELECT {0}, {1}, {2} FROM {3} ORDER BY {0},{2}", (object) "F_OWN_RECORDID", (object) "F_CHD_RECORDID", (object) "F_ORDER", (object) "TP_DOPREC");
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = str;
    using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
    {
      try
      {
        while (dataReader.Read())
        {
          object[] values = new object[dataReader.FieldCount];
          dataReader.GetValues(values);
          this.AddOrder(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));
        }
      }
      finally
      {
        dataReader.Close();
      }
    }
  }

  private void SetOrders(Rules rules)
  {
    List<VariantConfig> variantConfigList = new List<VariantConfig>();
    DopTypes tp = (DopTypes) null;
    foreach (DopTypes order in this._orders)
    {
      DopTypes dopTypes = order;
      if (MetaDataHelper.IsObjectTypeChildOf(dopTypes.ObjectGuid, TechCardConsts.ObjectTypes.TechProcBaseGUID))
        tp = dopTypes;
      variantConfigList.Clear();
      VariantConfig element = this.FindElement(dopTypes, rules.Properties.Elements, variantConfigList);
      if (element != null && variantConfigList.Count > 0)
      {
        variantConfigList.Sort((Comparison<VariantConfig>) ((x, y) =>
        {
          if (x?.ObjType == null)
            return 1;
          if (y?.ObjType == null)
            return -1;
          return object.Equals((object) x.ObjType, (object) y.ObjType) ? x.Number - y.Number : dopTypes.Childs.IndexOf(x.ObjType.Guid) - dopTypes.Childs.IndexOf(x.ObjType.Guid);
        }));
        element.ChildsList.Clear();
        element.ChildsList.AddRange((IEnumerable<string>) variantConfigList.ConvertAll<string>((Converter<VariantConfig, string>) (config => config.Id)));
      }
    }
    variantConfigList.Clear();
    foreach (IDocumentConfigElement element in rules.Properties.Elements)
    {
      if (element is VariantConfig variantConfig)
        variantConfigList.Add(variantConfig);
    }
    variantConfigList.Sort((Comparison<VariantConfig>) ((x, y) =>
    {
      if (x?.ObjType == null)
        return 1;
      if (y?.ObjType == null)
        return -1;
      if (object.Equals((object) x.ObjType, (object) y.ObjType))
        return x.Number - y.Number;
      if (MetaDataHelper.IsObjectTypeChildOf(x.ObjType.Guid, TechCardConsts.ObjectTypes.TechProcBaseGUID))
        return -1;
      if (MetaDataHelper.IsObjectTypeChildOf(y.ObjType.Guid, TechCardConsts.ObjectTypes.TechProcBaseGUID))
        return 1;
      if (tp == null)
        return 0;
      int num1 = tp.Childs.IndexOf(x.ObjType.Guid);
      int num2 = tp.Childs.IndexOf(y.ObjType.Guid);
      if (num1 < 0 && num2 < 0)
        return 0;
      if (num1 < 0)
        return 1;
      return num2 < 0 ? -1 : num1 - num2;
    }));
    rules.Properties.SetChildList(variantConfigList);
  }

  private VariantConfig FindElement(
    DopTypes dopTypes,
    List<IDocumentConfigElement> elements,
    List<VariantConfig> childs)
  {
    VariantConfig element1 = (VariantConfig) null;
    foreach (IDocumentConfigElement element2 in elements)
    {
      if (element2 is VariantConfig variantConfig && variantConfig.ObjType != null && variantConfig.ObjType.Guid == dopTypes.ObjectGuid)
      {
        if (element1 != null)
        {
          if (element1.Number < variantConfig.Number)
            element1 = variantConfig;
        }
        else
          element1 = variantConfig;
        if (dopTypes.Childs.Contains(variantConfig.ObjType.Guid))
          childs.Add(variantConfig);
      }
    }
    return element1;
  }

  private void WriteObjects(IUserSession userSession)
  {
    foreach (long key in this.DsList.Keys)
    {
      Rules ds = this.DsList[key];
      TemplateUtils.WriteTemplateToDb(userSession, ds);
      this.SetOrders(ds);
      try
      {
        TcBlankProperties.SaveRules(userSession, ds);
        if (this.ImportDataMain.GetValue(ImportingCategory.TechcardDocumentSetup, (object) -1) == null)
          this.ImportDataMain.AddValue(ImportingCategory.TechcardDocumentSetup, (object) -1, key);
        else
          this.ImportDataMain.SetNewKey(ImportingCategory.TechcardDocumentSetup, (object) -1, key);
      }
      catch (Exception ex)
      {
      }
    }
  }

  private static void SaveRules(IUserSession userSession, Rules imdtS)
  {
    IDBObjectCollection objectCollection = userSession.GetObjectCollection(BlankConsts.ObjectType.BlankSetupId);
    imdtS.FullName = "Миграция TechCard - " + imdtS.FullName;
    IpsProductionObj ipsProductionObj;
    if (imdtS.Properties.Production != 0 && TechPumpData.Production.Productions.TryGetValue(imdtS.Properties.Production, out ipsProductionObj))
    {
      Rules rules = imdtS;
      rules.FullName = $"{rules.FullName} ({ipsProductionObj.ProdInfo.Name})";
    }
    ColumnDescriptor[] columns = new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) MetaDataHelper.GetAttributeID((object) "cad0001f-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0)
    };
    ConditionStructure[] conditions = new ConditionStructure[1]
    {
      new ConditionStructure(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"), RelationalOperators.StartString, (object) imdtS.FullName, LogicalOperators.NONE, 0)
    };
    DataTable dataTable = objectCollection.Select(new DBRecordSetParams(conditions, columns));
    string str = imdtS.FullName;
    int num = 0;
    while (dataTable.Select($"[cad0001f-306c-11d8-b4e9-00304f19f545] = '{str}'").Length != 0)
      str = $"{imdtS.FullName}.{(object) num++}";
    imdtS.FullName = str;
    IDBObject dbObject = objectCollection.Create();
    DocumentConfigSerializer.Save(imdtS, dbObject.ObjectID, userSession);
  }

  private void CreatePropsFromTemplate(Rules imds)
  {
    foreach (DocumentTreeNode childNode in DocumentTreeNode.GetChildNodes((DocumentTreeNode) imds.Template))
    {
      if (childNode is TextBoxElement textBoxElement)
      {
        string id = textBoxElement.Id;
        AttributeSettings attribute = FormulaTemplateConverter.EntityToAttribute(id, this.plugin);
        if (attribute != null)
        {
          TextFieldConfig orCreateElement = imds.Properties.FindOrCreateElement(id, DocumentConfigElementType.TextField) as TextFieldConfig;
          orCreateElement.Id = id;
          orCreateElement.Digits = 0;
          orCreateElement.NotRepeated = false;
          orCreateElement.CalcOnFill = false;
          orCreateElement.FieldContents = CreateFieldContent.FieldContentsByObject((object) attribute);
          orCreateElement.Condition = (IFieldContents) null;
        }
      }
    }
  }

  protected virtual void AnalyzeData()
  {
    this.lastObjID = 0L;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    IImportingData cache = service.GetCache(ImportingCategory.TechcardDocumentSetup);
    if (cache == null)
      return;
    try
    {
      this.lastObjID = ImportingDataHelper.Instance.GetNewKey(cache, ImportingCategory.TechcardDocumentSetup, (object) -1, false);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.TechcardDocumentSetup);
    }
  }

  protected virtual void LoadImportData_Main()
  {
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    ImportingCategory[] importingCategoryArray = new ImportingCategory[2]
    {
      ImportingCategory.TechcardDocumentSetup,
      ImportingCategory.TechExpObjStruct
    };
    this.ImportDataMain = service.GetCache(importingCategoryArray);
  }

  private void UpdateBlankDir()
  {
    RegistryKey localMachine = Registry.LocalMachine;
    try
    {
      RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\Intermech\\Techcard\\3.5\\Dirs");
      if (registryKey != null)
        this._blankDir = Convert.ToString(registryKey.GetValue("Blanks_Directory"));
      if (this._blankDir.EndsWith("\\"))
        return;
      this._blankDir += "\\";
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddErrorMessage("Не удалось найти папку бланков в SOFTWARE\\Intermech\\Techcard\\3.5\\Dirs: " + ex.Message);
    }
  }

  public TempFormula GetCondition(int key)
  {
    ITagImportObject tag = this.ImportDataMain.GetTag(ImportingCategory.TechExpObjStruct, (object) key);
    if (tag == null || !(tag is TechObjectTag techObjectTag))
      return (TempFormula) null;
    if (techObjectTag.Object is TempFormula condition)
      return condition;
    this.plugin.appManager.AddWarningMessage($"Условие {key} не найдено в кэше \"{(Enum) ImportingCategory.TechExpObjStruct}\"");
    return (TempFormula) null;
  }
}
