// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.GtcPropDescriptorHolder
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Client.Core;
using Intermech.GTC.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.PropertyEditors;
using Intermech.PropertyEditors.AttrProcessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class GtcPropDescriptorHolder : 
  PropDescriptorHolder,
  IPossibleValuesHolder,
  IElementInfoEx,
  IElementInfo,
  IGtcObjectPropDescriptorHolder
{
  private AttributableElements _attributableElement;
  private GtcPropertyGrid _propertyGrid;
  private Type[] _tabTypes;
  private ArrayList _loadedTabs = new ArrayList();
  private Hashtable _visibleAttributeIdsByTab = new Hashtable();
  private bool _lockTypeChange;
  private List<int> _lockedAttributeIds = new List<int>();
  private ArrayList _deletedAttributeIds = new ArrayList();
  private GetAttributeValuesModes _attributeValuesModes = ClientConsts.GetAttributeValuesModes;
  private ArrayList _originalAttributeValuesList = new ArrayList();
  private ArrayList _attributeValuesList = new ArrayList();
  private Dictionary<int, string> _attributeCategoriesDictionary = new Dictionary<int, string>();
  internal ArrayList PropertyDescriptorList = new ArrayList();
  private int elementType;
  private long cachedId;
  private AttributableElements cachedKind;
  private int cachedType;
  private List<int> cachedLockedAttrsList;

  public long Id { get; private set; }

  public AttributableElements AttributableElement => this._attributableElement;

  public bool AnyAttributes { get; private set; }

  public List<int> LockedAttributes => this._lockedAttributeIds;

  public ArrayList AttributeValuesList => this._attributeValuesList;

  private PropDescriptor AttributeValuesToPropDescriptor(AttributeValues aAttributeValues)
  {
    int id = 0;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string category = string.Empty;
    Type type = (Type) null;
    TypeConverter typeConverter = (TypeConverter) null;
    object editor = (object) null;
    bool ro = true;
    bool reset = false;
    string empty3 = string.Empty;
    bool disableManualEdit = false;
    if (!AttributeValuesEditor.GetPDAttributes((object) this, aAttributeValues, ref id, ref empty1, ref empty2, ref category, ref type, ref typeConverter, ref editor, ref ro, ref reset, ref empty3, ref disableManualEdit))
      return (PropDescriptor) null;
    PropDescriptor propDescriptor = (PropDescriptor) null;
    if (!ro && (this._attributableElement == AttributableElements.Object && id == -7 || this._attributableElement == AttributableElements.Relation && id == -23))
      ro = this._lockTypeChange;
    if (!ro && this._lockedAttributeIds.IndexOf(id) != -1)
      ro = true;
    string str = ServiceHolder.Rm.GetString("GTC_6");
    if (!this._attributeCategoriesDictionary.TryGetValue(id, out category))
      category = str;
    if (type != (Type) null)
    {
      if (ListPropDescriptor.IsList(aAttributeValues))
      {
        propDescriptor = (PropDescriptor) new ListPropDescriptor(id, (object) this, empty1, (object) new AttributeValuesPropertyClass(aAttributeValues), typeof (AttributeValuesPropertyClass), (TypeConverter) new ObjectGridExpandableObjectConverter(), (object) null, category, empty2, ro, true, false, empty3, disableManualEdit);
      }
      else
      {
        DataTable possibleValues = (DataTable) null;
        if (MultiValueModesHelper.IsValuedFromList(aAttributeValues.MultipleValued))
          possibleValues = ClientCommons.GetPossibleValues(aAttributeValues.AttributeID);
        propDescriptor = (PropDescriptor) new SimplePropDescriptor(id, (object) this, empty1, AttributeValuesEditor.GetPDValue(aAttributeValues, 0, this.Id, this._attributableElement, empty3, possibleValues), type, typeConverter, editor, category, empty2, ro, true, reset, empty3, disableManualEdit, new AttributeValuesPropertyClass(aAttributeValues));
      }
    }
    return propDescriptor;
  }

  private PropDescriptor GetPropDescriptorById(int aPropId)
  {
    return this.PropertyDescriptorList.Cast<object>().Where<object>((System.Func<object, bool>) (t => ((PropDescriptor) t).PropID == aPropId)).Cast<PropDescriptor>().FirstOrDefault<PropDescriptor>();
  }

  private bool AttributeExists(ArrayList attributeValuesList, int attributeId)
  {
    return attributeValuesList.Cast<object>().Any<object>((System.Func<object, bool>) (t => ((AttributeValues) t).AttributeID == attributeId));
  }

  private ArrayList CollectAttributeValuesList()
  {
    ArrayList arrayList1 = new ArrayList();
    for (int index = 0; index < this._attributeValuesList.Count; ++index)
      arrayList1.Add((object) false);
    ArrayList arrayList2 = new ArrayList();
    for (int index = 0; index < this.PropertyDescriptorList.Count; ++index)
    {
      if (this.PropertyDescriptorList[index] is SimplePropDescriptor && ((PropDescriptor) this.PropertyDescriptorList[index]).ValueChanged || this.PropertyDescriptorList[index] is ListPropDescriptor && ((PropDescriptor) this.PropertyDescriptorList[index]).ValueChanged)
      {
        bool flag1 = this.PropertyDescriptorList[index].GetType() == typeof (ListPropDescriptor);
        PropDescriptor propertyDescriptor = (PropDescriptor) this.PropertyDescriptorList[index];
        int attributeValueListIndex = this.GetAttributeValueListIndex(propertyDescriptor.PropID);
        if (attributeValueListIndex != -1)
        {
          bool flag2 = false;
          arrayList2.Clear();
          if (flag1)
          {
            if (((PropDescriptor) this.PropertyDescriptorList[index]).ValueChanged)
            {
              for (int lPropID = 0; lPropID < ((ListPropDescriptor) propertyDescriptor).PdcList.Count; ++lPropID)
              {
                SimplePropDescriptor listItemByPropId = (SimplePropDescriptor) ((ListPropDescriptor) propertyDescriptor).GetPdcListItemByPropID(lPropID);
                if (listItemByPropId != null)
                  arrayList2.Add(AttributeValuesEditor.GetAVValue((PropDescriptor) listItemByPropId, (AttributeValues) this._attributeValuesList[attributeValueListIndex], (object) this));
              }
              flag2 = true;
            }
          }
          else if (((PropDescriptor) this.PropertyDescriptorList[index]).ValueChanged)
          {
            arrayList2.Add(AttributeValuesEditor.GetAVValue(propertyDescriptor, (AttributeValues) this._attributeValuesList[attributeValueListIndex], (object) this));
            flag2 = true;
          }
          if (flag2)
          {
            AttributeValues attributeValues = (AttributeValues) this._attributeValuesList[attributeValueListIndex];
            attributeValues.Values = arrayList2.ToArray();
            this._attributeValuesList[attributeValueListIndex] = (object) attributeValues;
            arrayList1[attributeValueListIndex] = (object) true;
          }
        }
      }
    }
    return arrayList1;
  }

  public bool CheckIfDeleted(int attributeType)
  {
    return this._deletedAttributeIds.IndexOf((object) attributeType) != -1;
  }

  public DataTable GetPossibleAttributes(bool byType, bool byVisible)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBCollection dbCollection = (IDBCollection) null;
      if (byType)
      {
        if (this._attributableElement == AttributableElements.Object)
          dbCollection = byVisible ? (IDBCollection) sessionKeeper.Session.GetObjectType(this.ElementType).VisibleAttributes : (IDBCollection) sessionKeeper.Session.GetObjectType(this.ElementType).Attributes;
        if (this._attributableElement == AttributableElements.Relation)
          dbCollection = byVisible ? (IDBCollection) sessionKeeper.Session.GetRelationType(this.ElementType).VisibleAttributes : (IDBCollection) sessionKeeper.Session.GetRelationType(this.ElementType).Attributes;
      }
      else
        dbCollection = (IDBCollection) sessionKeeper.Session.GetAttributeTypeCollection(-1, byVisible);
      DataTable possibleAttributes = dbCollection.Select("F_ATTRIBUTE_ID");
      if (this._lockedAttributeIds.Count > 0)
      {
        int index1 = this._lockedAttributeIds.Count - 1;
        for (int index2 = possibleAttributes.Rows.Count - 1; index2 >= 0; --index2)
        {
          int int32 = Convert.ToInt32(possibleAttributes.Rows[index2]["F_ATTRIBUTE_ID"]);
          if (int32 <= this._lockedAttributeIds[index1])
          {
            if (int32 == this._lockedAttributeIds[index1])
              possibleAttributes.Rows.RemoveAt(index2);
            --index1;
            if (index1 < 0)
              break;
          }
        }
      }
      return possibleAttributes;
    }
  }

  public IDBAttributable GetAttributable(IUserSession session)
  {
    return ClientCommons.GetAttributable(this.Id, this._attributableElement, session);
  }

  public IDBAttributableTypeInfo GetAttributableType(IUserSession session)
  {
    return ClientCommons.GetAttributableType(this.ElementType, this._attributableElement);
  }

  public static int GetAttributeValueListIndex(ArrayList list, int aAttributeId)
  {
    int attributeValueListIndex = -1;
    for (int index = 0; index < list.Count; ++index)
    {
      if (((AttributeValues) list[index]).AttributeID == aAttributeId)
      {
        attributeValueListIndex = index;
        break;
      }
    }
    return attributeValueListIndex;
  }

  public int GetAttributeValueListIndex(int aAttributeId)
  {
    return GtcPropDescriptorHolder.GetAttributeValueListIndex(this._attributeValuesList, aAttributeId);
  }

  public static AttributeValues GetAttributeValueListItem(ArrayList list, int aAttributeId)
  {
    int attributeValueListIndex = GtcPropDescriptorHolder.GetAttributeValueListIndex(list, aAttributeId);
    return attributeValueListIndex >= 0 ? (AttributeValues) list[attributeValueListIndex] : (AttributeValues) null;
  }

  public AttributeValues GetAttributeValueListItem(int aAttributeId)
  {
    return GtcPropDescriptorHolder.GetAttributeValueListItem(this._attributeValuesList, aAttributeId);
  }

  private ArrayList CloneAttributeValueList(ArrayList list)
  {
    if (list == null)
      return (ArrayList) null;
    ArrayList arrayList = new ArrayList();
    for (int index = 0; index < list.Count; ++index)
      arrayList.Add(((AttributeValues) list[index]).Clone());
    return arrayList;
  }

  public int GetPdcGeneralListIndex(int aAttributeId)
  {
    int generalListIndex = -1;
    for (int index = 0; index < this.PropertyDescriptorList.Count; ++index)
    {
      if (((PropDescriptor) this.PropertyDescriptorList[index]).PropID == aAttributeId)
      {
        generalListIndex = index;
        break;
      }
    }
    return generalListIndex;
  }

  public bool AssignData(
    long id,
    AttributableElements aAttributableElement,
    GetAttributeValuesModes aAttributeValuesModes,
    GtcPropertyGrid propertyGrid,
    bool lockTypeChange,
    Type[] tabTypes)
  {
    this._lockTypeChange = lockTypeChange;
    this._tabTypes = tabTypes;
    this._visibleAttributeIdsByTab.Clear();
    this._deletedAttributeIds.Clear();
    this._loadedTabs.Clear();
    this._originalAttributeValuesList.Clear();
    this._attributeValuesList.Clear();
    this.PropertyDescriptorList.Clear();
    if (id != -1L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (ClientCommons.GetAttributable(id, aAttributableElement, sessionKeeper.Session) == null)
          return false;
        if (aAttributableElement == AttributableElements.Object)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(id);
          if (dbObject != null)
            this._attributeCategoriesDictionary = AttributeCategoriesHelper.GetAttributeCategoriesDictionary(dbObject);
        }
      }
      this.Id = id;
      this._attributableElement = aAttributableElement;
      this._attributeValuesModes = aAttributeValuesModes | ClientConsts.GetAttributeValuesModesMinimum;
      this.elementType = ClientCommons.GetElementType(this.Id, this._attributableElement);
      this.AnyAttributes = ClientCommons.GetAnyAttributesFlag(this.ElementType, this._attributableElement);
      this._lockedAttributeIds.Clear();
      if (ServicesManager.GetService(typeof (IAttributesLockService)) is IAttributesLockService service)
      {
        this._lockedAttributeIds.AddRange((IEnumerable<int>) service.GetLockedAttributes(this._attributableElement, this.Id, this.ElementType));
        this._lockedAttributeIds.Sort();
      }
      this.DropPropertyDescriptorCollection();
      this._propertyGrid = propertyGrid;
      if (this._propertyGrid == null || this._propertyGrid.IsDisposed)
        return false;
      this._propertyGrid.SelectedObject = (object) this;
    }
    else
    {
      this.Id = 0L;
      this._attributableElement = aAttributableElement;
      this._attributeValuesModes = aAttributeValuesModes;
      this.elementType = 0;
      this.AnyAttributes = false;
      this.DropPropertyDescriptorCollection();
      if (this._propertyGrid != null)
        this._propertyGrid.SelectedObject = (object) null;
      this._propertyGrid = (GtcPropertyGrid) null;
    }
    return true;
  }

  public bool SaveData(out ArrayList origList, out ArrayList fireList)
  {
    origList = (ArrayList) null;
    fireList = (ArrayList) null;
    if (this.PropertyDescriptorList.Count == 0 && this._deletedAttributeIds.Count == 0)
      return true;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttributable attributable = this.GetAttributable(sessionKeeper.Session);
      if (attributable == null)
        return false;
      ArrayList arrayList = this.CollectAttributeValuesList();
      ArrayList list = (ArrayList) this._attributeValuesList.Clone();
      int index1 = 0;
      while (index1 < list.Count)
      {
        if (!(bool) arrayList[index1])
        {
          list.RemoveAt(index1);
          arrayList.RemoveAt(index1);
        }
        else
          ++index1;
      }
      for (int index2 = 0; index2 < this._deletedAttributeIds.Count; ++index2)
      {
        AttributeValues attributeValues = new AttributeValues((int) this._deletedAttributeIds[index2], FieldTypes.ftUnknown, MultiValueModes.SingleValue, ComputeValueModes.NotComputableValue)
        {
          Values = new object[1]
          {
            (object) DeleteModesEnum.None
          }
        };
        list.Add((object) attributeValues);
        arrayList.Add((object) true);
      }
      int aType;
      if (ClientCommons.GetAttributable(this.Id, this._attributableElement, out aType, sessionKeeper.Session) != null && this._attributableElement == AttributableElements.Object && sessionKeeper.Session.GetCustomService(typeof (IDocumentTypeSettingsService)) is IDocumentTypeSettingsService customService && customService.InheritedFromDocuments(sessionKeeper.Session.SessionGUID, aType))
      {
        int attributeId = sessionKeeper.Session.IdentHelper.GetAttributeID("cad0001f-306c-11d8-b4e9-00304f19f545");
        int attributeValueListIndex = GtcPropDescriptorHolder.GetAttributeValueListIndex(list, attributeId);
        AttributeValues attributeValues = attributeValueListIndex != -1 ? (AttributeValues) list[attributeValueListIndex] : (AttributeValues) null;
        if (attributeValues != null)
        {
          DocumentTypeSettings settings = customService.GetSettings(sessionKeeper.Session.SessionGUID, aType);
          if (settings.DocumentTypeCodeInDesignation && settings.DocumentTypeCode != string.Empty)
          {
            string designation = Convert.ToString(attributeValues.Values[0]);
            attributeValues.Values[0] = (object) DocumentsHelper.AppendDocCode(sessionKeeper.Session, designation, settings.DocumentTypeCode);
          }
        }
      }
      if (list.Count > 0)
      {
        AttributeValues[] array = (AttributeValues[]) list.ToArray(typeof (AttributeValues));
        AttributeValues[] attributeValuesArray = attributable.SetAttributesValues(array, false, true, true, this._attributeValuesModes);
        for (int index3 = 0; index3 < this.PropertyDescriptorList.Count; ++index3)
        {
          if (this.PropertyDescriptorList[index3] is SimplePropDescriptor propertyDescriptor)
          {
            if (propertyDescriptor.ValueChanged)
              propertyDescriptor.ValueChanged = false;
            else if (this.PropertyDescriptorList[index3] is ListPropDescriptor && ((PropDescriptor) this.PropertyDescriptorList[index3]).ValueChanged)
              ((PropDescriptor) this.PropertyDescriptorList[index3]).ResetValueChanged((object) this);
          }
        }
        if (attributeValuesArray != null)
        {
          bool flag = false;
          for (int index4 = 0; index4 < attributeValuesArray.Length; ++index4)
          {
            AttributeValues aAttributeValues = attributeValuesArray[index4];
            PropDescriptor propDescriptor = this.AttributeValuesToPropDescriptor(aAttributeValues);
            if (propDescriptor != null)
            {
              int attributeValueListIndex = this.GetAttributeValueListIndex(aAttributeValues.AttributeID);
              if (attributeValueListIndex != -1)
                this._attributeValuesList[attributeValueListIndex] = (object) aAttributeValues;
              PropDescriptor propDescriptorById = this.GetPropDescriptorById(propDescriptor.PropID);
              if (propDescriptorById != null)
                this.PropertyDescriptorList.Remove((object) propDescriptorById);
              this.PropertyDescriptorList.Add((object) propDescriptor);
              flag = true;
            }
            int attributeValueListIndex1 = GtcPropDescriptorHolder.GetAttributeValueListIndex(list, aAttributeValues.AttributeID);
            if (attributeValueListIndex1 == -1)
              list.Add(aAttributeValues.Clone());
            else
              list[attributeValueListIndex1] = aAttributeValues.Clone();
            int attributeValueListIndex2 = GtcPropDescriptorHolder.GetAttributeValueListIndex(this._attributeValuesList, aAttributeValues.AttributeID);
            if (attributeValueListIndex2 == -1)
              this._attributeValuesList.Add(aAttributeValues.Clone());
            else
              this._attributeValuesList[attributeValueListIndex2] = aAttributeValues.Clone();
          }
          if (flag)
          {
            this.DropPropertyDescriptorCollection();
            this._propertyGrid.SelectedObject = (object) this;
          }
        }
      }
      origList = this._originalAttributeValuesList;
      fireList = this.CloneAttributeValueList(list);
      this._originalAttributeValuesList = this.CloneAttributeValueList(this._attributeValuesList);
      this._deletedAttributeIds.Clear();
    }
    return true;
  }

  public bool AddProperty(AttributeValues[] aAttributeValues, out bool directWriteOccured)
  {
    return this.AddProperty(aAttributeValues, out directWriteOccured, false, false);
  }

  public bool AddProperty(
    AttributeValues[] aAttributeValues,
    out bool directWriteOccured,
    bool masterProcess,
    bool masterProcessEdit)
  {
    bool flag1 = false;
    bool flag2 = false;
    directWriteOccured = false;
    if (!masterProcess)
    {
      for (int index1 = 0; index1 < aAttributeValues.Length; ++index1)
      {
        if (this.AttributeExists(this._attributeValuesList, aAttributeValues[index1].AttributeID))
        {
          int num = (int) MessageBox.Show(string.Format(ServiceHolder.Rm.GetString("GTC_16"), (object) aAttributeValues[index1].AttributeName));
        }
        else
        {
          if (aAttributeValues[index1].AttributeType == FieldTypes.ftAutoInc)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              IDBAttributable attributable = this.GetAttributable(sessionKeeper.Session);
              if (attributable != null)
              {
                IDBAttribute dbAttribute = attributable.Attributes.AddAttribute(aAttributeValues[index1].AttributeID, true);
                if (dbAttribute != null)
                  aAttributeValues[index1].Values = (object[]) dbAttribute.Values.Clone();
                else
                  continue;
              }
              else
                continue;
            }
            directWriteOccured = true;
          }
          PropDescriptor propDescriptor = this.AttributeValuesToPropDescriptor(aAttributeValues[index1]);
          if (propDescriptor != null)
          {
            if (propDescriptor is ListPropDescriptor)
            {
              TypeConverter typeConverter = (TypeConverter) null;
              if (propDescriptor.Converter is ObjectGridExpandableObjectConverter)
                typeConverter = propDescriptor.Converter;
              else if (propDescriptor.Converter is TypeConvertorWrapper && ((TypeConvertorWrapper) propDescriptor.Converter).WrappedTypeConverter is ObjectGridExpandableObjectConverter)
                typeConverter = ((TypeConvertorWrapper) propDescriptor.Converter).WrappedTypeConverter;
              typeConverter?.GetProperties((ITypeDescriptorContext) null, (object) new BugFixObject(new object[2]
              {
                (object) this,
                (object) propDescriptor
              }));
            }
            if (aAttributeValues[index1].AttributeType == FieldTypes.ftAutoInc)
            {
              propDescriptor.ValueChanged = false;
            }
            else
            {
              propDescriptor.ValueChanged = true;
              flag1 = true;
            }
            int index2 = this._deletedAttributeIds.IndexOf((object) propDescriptor.PropID);
            if (index2 != -1)
              this._deletedAttributeIds.RemoveAt(index2);
            for (int index3 = 0; index3 < this._propertyGrid.PropertyTabs.Count; ++index3)
            {
              IObjectPropertyGridTab propertyTab = (IObjectPropertyGridTab) this._propertyGrid.PropertyTabs[index3];
              if (propertyTab != null)
              {
                ArrayList arrayList = (ArrayList) this._visibleAttributeIdsByTab[(object) propertyTab.TabGuid] ?? new ArrayList();
                arrayList.Add((object) aAttributeValues[index1].AttributeID);
                this._visibleAttributeIdsByTab[(object) propertyTab.TabGuid] = (object) arrayList;
              }
            }
            this.DropPropertyDescriptorCollection();
            this._attributeValuesList.Add((object) aAttributeValues[index1]);
            this.PropertyDescriptorList.Add((object) propDescriptor);
            flag2 = true;
          }
        }
      }
    }
    else
    {
      this.CollectAttributeValuesList();
      AttributeProcessor attributeProcessor = new AttributeProcessor();
      attributeProcessor.MemLoad(this.Id, this._attributableElement, this._attributeValuesModes, this.ElementType, this.AnyAttributes, new Intermech.PropertyEditors.AttrProcessor.AttributeValuesList((IEnumerable<AttributeValues>) this._attributeValuesList.ToArray(typeof (AttributeValues))));
      for (int index = 0; index < aAttributeValues.Length; ++index)
      {
        AttributeValues aAttributeValue = aAttributeValues[index];
        if (aAttributeValue.AttributeType == FieldTypes.ftObjectLink && (aAttributeValue.MultipleValued == MultiValueModes.SingleValue || aAttributeValue.MultipleValued == MultiValueModes.SingleValueFromList))
        {
          AttributeValues byAttributeId = attributeProcessor.ActualAttributeValues.FindByAttributeID(aAttributeValue.AttributeID);
          if (byAttributeId == null || byAttributeId.Values == null || byAttributeId.Values.Length == 0)
            ++index;
          else
            attributeProcessor.AssignMasterAttributePrim(byAttributeId.AttributeID, byAttributeId.Values[0], attributeProcessor.ActualAttributeValues, false, out Intermech.PropertyEditors.AttrProcessor.AttributeValuesList _);
        }
      }
      for (int index4 = 0; index4 < attributeProcessor.ActualAttributeValues.Count; ++index4)
      {
        AttributeValues attributeValueListItem = GtcPropDescriptorHolder.GetAttributeValueListItem(this._attributeValuesList, attributeProcessor.ActualAttributeValues[index4].AttributeID);
        if (attributeValueListItem == null)
        {
          if (attributeProcessor.ActualAttributeValues[index4].AttributeType == FieldTypes.ftAutoInc)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              IDBAttributable attributable = this.GetAttributable(sessionKeeper.Session);
              if (attributable != null)
              {
                IDBAttribute dbAttribute = attributable.Attributes.AddAttribute(attributeProcessor.ActualAttributeValues[index4].AttributeID, true);
                if (dbAttribute != null)
                  attributeProcessor.ActualAttributeValues[index4].Values = (object[]) dbAttribute.Values.Clone();
                else
                  continue;
              }
              else
                continue;
            }
            directWriteOccured = true;
          }
          PropDescriptor propDescriptor = this.AttributeValuesToPropDescriptor(attributeProcessor.ActualAttributeValues[index4]);
          if (propDescriptor != null)
          {
            if (propDescriptor is ListPropDescriptor)
            {
              TypeConverter typeConverter = (TypeConverter) null;
              if (propDescriptor.Converter is ObjectGridExpandableObjectConverter)
                typeConverter = propDescriptor.Converter;
              else if (propDescriptor.Converter is TypeConvertorWrapper && ((TypeConvertorWrapper) propDescriptor.Converter).WrappedTypeConverter is ObjectGridExpandableObjectConverter)
                typeConverter = ((TypeConvertorWrapper) propDescriptor.Converter).WrappedTypeConverter;
              typeConverter?.GetProperties((ITypeDescriptorContext) null, (object) new BugFixObject(new object[2]
              {
                (object) this,
                (object) propDescriptor
              }));
            }
            if (attributeProcessor.ActualAttributeValues[index4].AttributeType == FieldTypes.ftAutoInc)
            {
              propDescriptor.ValueChanged = false;
            }
            else
            {
              propDescriptor.ValueChanged = true;
              flag1 = true;
            }
            int index5 = this._deletedAttributeIds.IndexOf((object) propDescriptor.PropID);
            if (index5 != -1)
              this._deletedAttributeIds.RemoveAt(index5);
            for (int index6 = 0; index6 < this._propertyGrid.PropertyTabs.Count; ++index6)
            {
              IObjectPropertyGridTab propertyTab = (IObjectPropertyGridTab) this._propertyGrid.PropertyTabs[index6];
              if (propertyTab != null)
              {
                ArrayList arrayList = (ArrayList) this._visibleAttributeIdsByTab[(object) propertyTab.TabGuid] ?? new ArrayList();
                arrayList.Add((object) attributeProcessor.ActualAttributeValues[index4].AttributeID);
                this._visibleAttributeIdsByTab[(object) propertyTab.TabGuid] = (object) arrayList;
              }
            }
            this.DropPropertyDescriptorCollection();
            this._attributeValuesList.Add((object) attributeProcessor.ActualAttributeValues[index4]);
            this.PropertyDescriptorList.Add((object) propDescriptor);
            flag2 = true;
          }
        }
        else if (!attributeProcessor.ActualAttributeValues[index4].Equals(attributeValueListItem))
        {
          if (attributeProcessor.ActualAttributeValues[index4].AttributeType == FieldTypes.ftAutoInc)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              IDBAttributable attributable = this.GetAttributable(sessionKeeper.Session);
              if (attributable != null)
              {
                attributable.SetAttributesValues(new AttributeValues[1]
                {
                  attributeProcessor.ActualAttributeValues[index4]
                });
                IDBAttribute attributeById = attributable.GetAttributeByID(attributeProcessor.ActualAttributeValues[index4].AttributeID);
                if (attributeById != null)
                  attributeProcessor.ActualAttributeValues[index4].Values = (object[]) attributeById.Values.Clone();
                else
                  continue;
              }
              else
                continue;
            }
            directWriteOccured = true;
          }
          AttributeValues actualAttributeValue = attributeProcessor.ActualAttributeValues[index4];
          PropDescriptor propDescriptor = this.AttributeValuesToPropDescriptor(actualAttributeValue);
          if (propDescriptor != null)
          {
            if (attributeProcessor.ActualAttributeValues[index4].AttributeType == FieldTypes.ftAutoInc)
            {
              propDescriptor.ValueChanged = false;
            }
            else
            {
              propDescriptor.ValueChanged = true;
              flag1 = true;
            }
            int attributeValueListIndex = this.GetAttributeValueListIndex(actualAttributeValue.AttributeID);
            if (attributeValueListIndex != -1)
              this._attributeValuesList[attributeValueListIndex] = (object) actualAttributeValue;
            PropDescriptor propDescriptorById = this.GetPropDescriptorById(propDescriptor.PropID);
            if (propDescriptorById != null)
              this.PropertyDescriptorList.Remove((object) propDescriptorById);
            this.PropertyDescriptorList.Add((object) propDescriptor);
            this.DropPropertyDescriptorCollection();
            flag2 = true;
          }
        }
      }
    }
    if (flag2)
      this._propertyGrid.SelectedObject = (object) this;
    return flag1;
  }

  public bool AddListProperty(ListPropDescriptor aListPropDescriptor)
  {
    bool flag = false;
    AttributeValuesPropertyClass valuesPropertyClass = (AttributeValuesPropertyClass) aListPropDescriptor.GetValue((object) this);
    if (valuesPropertyClass != null)
    {
      AttributeValues attributeValue = valuesPropertyClass.AttributeValue;
      if (attributeValue.MultipleValued == MultiValueModes.MultiValues || attributeValue.MultipleValued == MultiValueModes.MultiValuesFromList)
      {
        int id = 0;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        Type type = (Type) null;
        TypeConverter typeConverter = (TypeConverter) null;
        object editor = (object) null;
        bool ro = true;
        bool reset = false;
        string empty4 = string.Empty;
        bool disableManualEdit = false;
        if (AttributeValuesEditor.GetPDAttributes((object) this, attributeValue, ref id, ref empty1, ref empty2, ref empty3, ref type, ref typeConverter, ref editor, ref ro, ref reset, ref empty4, ref disableManualEdit))
        {
          if (!ro && this._lockedAttributeIds.IndexOf(id) != -1)
            ro = true;
          SimplePropDescriptor simplePropDescriptor = new SimplePropDescriptor(attributeValue.Values.Length, (object) this, $"[{attributeValue.Values.Length.ToString(ClientConsts.MultiValueEnumerateFormat)}]", (object) null, type, typeConverter, editor, empty3, empty2, ro, true, reset, empty4, disableManualEdit, (AttributeValuesPropertyClass) null)
          {
            ParentListPropDescriptor = aListPropDescriptor
          };
          aListPropDescriptor.PdcList.Add((PropertyDescriptor) simplePropDescriptor);
          simplePropDescriptor.ParentListPropDescriptor.ValueChanged = true;
          ArrayList arrayList = new ArrayList();
          arrayList.AddRange((ICollection) attributeValue.Values);
          arrayList.Add((object) null);
          attributeValue.Values = (object[]) arrayList.ToArray(typeof (object));
          for (int index = 0; index < aListPropDescriptor.PdcList.Count; ++index)
          {
            ((PropDescriptor) aListPropDescriptor.PdcList[index]).SetPropID(index);
            ((PropDescriptor) aListPropDescriptor.PdcList[index]).SetName($"[{index.ToString(ClientConsts.MultiValueEnumerateFormat)}]");
          }
          aListPropDescriptor.SetValue((object) this, (object) new AttributeValuesPropertyClass(attributeValue));
          this._propertyGrid.SelectedObject = (object) this;
          flag = true;
        }
      }
    }
    return flag;
  }

  public bool DeleteProperty(PropDescriptor aRemovedDescriptor, out bool directWriteOccured)
  {
    bool flag1 = false;
    directWriteOccured = false;
    switch (aRemovedDescriptor)
    {
      case ListPropDescriptor _:
      case SimplePropDescriptor _ when ((SimplePropDescriptor) aRemovedDescriptor).ParentListPropDescriptor == null:
        int attributeValueListIndex = this.GetAttributeValueListIndex(aRemovedDescriptor.PropID);
        if (attributeValueListIndex != -1)
        {
          AttributeValues attributeValues = (AttributeValues) this._attributeValuesList[attributeValueListIndex];
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            IDBAttributableTypeInfo attributableType = this.GetAttributableType(sessionKeeper.Session);
            if (attributableType == null)
              return false;
            IDBAttributeTypeInfo4 attributeById = attributableType.Attributes.GetAttributeByID(attributeValues.AttributeID);
            if (attributeById != null)
            {
              if (attributeById.Required == RequiredModes.AutoRequired)
              {
                int num = (int) MessageBox.Show(ServiceHolder.Rm.GetString("GTC_15"));
                return false;
              }
            }
          }
          if (MessageBox.Show(MessageDialogs.msgReallyDelete, MessageDialogs.msgConfirmDelete, MessageBoxButtons.YesNo) != DialogResult.Yes)
            return false;
          bool flag2 = attributeValues.AttributeType == FieldTypes.ftAutoInc;
          if (flag2)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              IDBAttributable attributable = this.GetAttributable(sessionKeeper.Session);
              if (attributable == null)
                return false;
              IDBAttribute attributeById = attributable.GetAttributeByID(attributeValues.AttributeID);
              if (attributeById == null)
                return false;
              attributeById.Delete(0L);
            }
            directWriteOccured = true;
          }
          if (this._deletedAttributeIds.IndexOf((object) aRemovedDescriptor.PropID) == -1)
            this._deletedAttributeIds.Add((object) aRemovedDescriptor.PropID);
          for (int index1 = 0; index1 < this._propertyGrid.PropertyTabs.Count; ++index1)
          {
            IObjectPropertyGridTab propertyTab = (IObjectPropertyGridTab) this._propertyGrid.PropertyTabs[index1];
            if (propertyTab != null)
            {
              ArrayList arrayList = (ArrayList) this._visibleAttributeIdsByTab[(object) propertyTab.TabGuid];
              if (arrayList != null)
              {
                int index2 = arrayList.IndexOf((object) aRemovedDescriptor.PropID);
                if (index2 != -1)
                  arrayList.RemoveAt(index2);
                if (arrayList.Count == 0 && this._loadedTabs.IndexOf((object) propertyTab.TabGuid) == -1)
                  this._visibleAttributeIdsByTab[(object) propertyTab.TabGuid] = (object) null;
              }
            }
          }
          this._attributeValuesList.RemoveAt(attributeValueListIndex);
          PropDescriptor propDescriptorById = this.GetPropDescriptorById(aRemovedDescriptor.PropID);
          if (propDescriptorById != null)
          {
            this.PropertyDescriptorList.Remove((object) propDescriptorById);
            this.DropPropertyDescriptorCollection();
          }
          this._propertyGrid.SelectedObject = (object) this;
          if (!flag2)
          {
            flag1 = true;
            break;
          }
          break;
        }
        break;
      case SimplePropDescriptor _ when ((SimplePropDescriptor) aRemovedDescriptor).ParentListPropDescriptor != null:
        ListPropDescriptor listPropDescriptor = ((SimplePropDescriptor) aRemovedDescriptor).ParentListPropDescriptor;
        AttributeValuesPropertyClass valuesPropertyClass = (AttributeValuesPropertyClass) listPropDescriptor.GetValue((object) this);
        if (valuesPropertyClass != null)
        {
          AttributeValues attributeValue = valuesPropertyClass.AttributeValue;
          if (attributeValue.Values.Length > 1)
          {
            int propId = aRemovedDescriptor.PropID;
            ArrayList arrayList = new ArrayList();
            arrayList.AddRange((ICollection) attributeValue.Values);
            arrayList.RemoveAt(propId);
            attributeValue.Values = (object[]) arrayList.ToArray(typeof (object));
            listPropDescriptor.SetValue((object) this, (object) new AttributeValuesPropertyClass(attributeValue));
            listPropDescriptor.PdcList = PropDescriptorHolder.RemovePDCItem(listPropDescriptor.PdcList, propId);
            for (int index = 0; index < listPropDescriptor.PdcList.Count; ++index)
            {
              ((PropDescriptor) listPropDescriptor.PdcList[index]).SetPropID(index);
              ((PropDescriptor) listPropDescriptor.PdcList[index]).SetName($"[{index.ToString(ClientConsts.MultiValueEnumerateFormat)}]");
            }
            listPropDescriptor.ValueChanged = true;
            this._propertyGrid.SelectedObject = (object) this;
            flag1 = true;
            break;
          }
          break;
        }
        break;
    }
    return flag1;
  }

  protected override AttributeCollection ExtendAttributes(AttributeCollection attributes)
  {
    ArrayList arrayList = new ArrayList((ICollection) attributes);
    if (this._tabTypes != null)
    {
      Attribute attribute = (Attribute) new PropertyTabAttribute4GtcPropertyGrid(this._tabTypes);
      arrayList.Add((object) attribute);
    }
    return new AttributeCollection((Attribute[]) arrayList.ToArray(typeof (Attribute)));
  }

  public override void CreateProperties(PropertyDescriptorCollection pdc)
  {
    pdc.Clear();
    PropertyDescriptorCollection descriptorCollection = this.ExtendPropDescriptorCollectionbyMode((object) this._propertyGrid.PropertyTabByGuid(GtcPropertiesTabCustom.PropertyTabGuid), this._attributeValuesModes, true);
    for (int index = 0; index < descriptorCollection.Count; ++index)
      pdc.Add(descriptorCollection[index]);
  }

  public PropertyDescriptorCollection ExtendPropDescriptorCollectionbyMode(
    object component,
    GetAttributeValuesModes avm,
    bool hideIfNotInMode)
  {
    if (!(component is IObjectPropertyGridTab objectPropertyGridTab))
      return (PropertyDescriptorCollection) null;
    ArrayList arrayList1 = new ArrayList();
    if (this._loadedTabs.IndexOf((object) objectPropertyGridTab.TabGuid) == -1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBAttributable attributable = this.GetAttributable(sessionKeeper.Session);
        if (attributable != null)
        {
          ArrayList arrayList2 = (ArrayList) this._visibleAttributeIdsByTab[(object) objectPropertyGridTab.TabGuid];
          if (arrayList2 != null)
          {
            for (int index = 0; index < arrayList2.Count; ++index)
            {
              int generalListIndex = this.GetPdcGeneralListIndex((int) arrayList2[index]);
              if (generalListIndex != -1)
                arrayList1.Add(this.PropertyDescriptorList[generalListIndex]);
            }
          }
          else
            arrayList2 = new ArrayList();
          if (this._loadedTabs.IndexOf((object) GtcPropertiesTabCustom.PropertyTabGuid) == -1 && (avm & GetAttributeValuesModes.IncludeOnlyInvisible) != GetAttributeValuesModes.None)
            avm &= ~GetAttributeValuesModes.IncludeOnlyInvisible;
          AttributeValues[] attributesValues = attributable.GetAttributesValues(avm);
          if (!objectPropertyGridTab.TabGuid.Equals(GtcPropertiesTabCustom.PropertyTabGuid) && this._loadedTabs.IndexOf((object) GtcPropertiesTabCustom.PropertyTabGuid) != -1 && (avm & GetAttributeValuesModes.IncludeOnlyInvisible) != GetAttributeValuesModes.None)
          {
            ArrayList arrayList3 = (ArrayList) this._visibleAttributeIdsByTab[(object) GtcPropertiesTabCustom.PropertyTabGuid];
            for (int index = 0; index < arrayList3.Count; ++index)
            {
              if (this._deletedAttributeIds.IndexOf((object) (int) arrayList3[index]) == -1)
              {
                int generalListIndex = this.GetPdcGeneralListIndex((int) arrayList3[index]);
                if (generalListIndex != -1)
                  arrayList1.Add(this.PropertyDescriptorList[generalListIndex]);
                arrayList2.Add((object) (int) arrayList3[index]);
              }
            }
          }
          for (int index = 0; index < attributesValues.Length; ++index)
          {
            if (arrayList2.IndexOf((object) attributesValues[index].AttributeID) == -1 && this._deletedAttributeIds.IndexOf((object) attributesValues[index].AttributeID) == -1)
            {
              if (this.GetAttributeValueListIndex(attributesValues[index].AttributeID) == -1)
              {
                this._attributeValuesList.Add((object) attributesValues[index]);
                this._originalAttributeValuesList.Add(attributesValues[index].Clone());
                PropDescriptor propDescriptor = this.AttributeValuesToPropDescriptor(attributesValues[index]);
                if (propDescriptor != null)
                {
                  this.PropertyDescriptorList.Add((object) propDescriptor);
                  arrayList1.Add((object) propDescriptor);
                }
              }
              else
              {
                int generalListIndex = this.GetPdcGeneralListIndex(attributesValues[index].AttributeID);
                if (generalListIndex != -1)
                  arrayList1.Add(this.PropertyDescriptorList[generalListIndex]);
              }
              arrayList2.Add((object) attributesValues[index].AttributeID);
            }
          }
          this._visibleAttributeIdsByTab[(object) objectPropertyGridTab.TabGuid] = (object) arrayList2;
          this._loadedTabs.Add((object) objectPropertyGridTab.TabGuid);
        }
      }
    }
    else
    {
      ArrayList arrayList4 = (ArrayList) this._visibleAttributeIdsByTab[(object) objectPropertyGridTab.TabGuid];
      for (int index = 0; index < arrayList4.Count; ++index)
      {
        if (this._deletedAttributeIds.IndexOf((object) (int) arrayList4[index]) == -1)
        {
          int generalListIndex = this.GetPdcGeneralListIndex((int) arrayList4[index]);
          if (generalListIndex != -1)
            arrayList1.Add(this.PropertyDescriptorList[generalListIndex]);
        }
      }
    }
    return new PropertyDescriptorCollection((PropertyDescriptor[]) arrayList1.ToArray(typeof (PropDescriptor)));
  }

  public GetAttributeValuesModes AttributeValuesModes => this._attributeValuesModes;

  public GtcPropertyGrid PropertyGrid => this._propertyGrid;

  public DataTable GetPossibleValues(ITypeDescriptorContext context)
  {
    GridItem selectedGridItem = this.PropertyGrid.SelectedGridItem;
    return selectedGridItem == null || !(selectedGridItem.PropertyDescriptor is SimplePropDescriptor) ? (DataTable) null : ClientCommons.GetPossibleValues(((SimplePropDescriptor) selectedGridItem.PropertyDescriptor).AttributeValuePropertyClass.AttributeValue.AttributeID);
  }

  public long ElementIdentifier => this.Id;

  public AttributableElements ElementKind => this._attributableElement;

  public int ElementType => this.elementType;

  public bool CheckAttributeLock(int attrId)
  {
    bool flag = false;
    if ((this.cachedLockedAttrsList == null || this.cachedLockedAttrsList != null && (this.cachedId != this.ElementIdentifier || this.cachedKind != this.ElementKind || this.cachedType != this.ElementType)) && ServicesManager.ServiceContainer.GetService(typeof (IAttributesLockService)) is IAttributesLockService service)
    {
      this.cachedLockedAttrsList = new List<int>((IEnumerable<int>) service.GetLockedAttributes(this.ElementKind, this.ElementIdentifier, this.ElementType));
      this.cachedId = this.ElementIdentifier;
      this.cachedKind = this.ElementKind;
      this.cachedType = this.ElementType;
    }
    if (this.cachedLockedAttrsList != null)
      flag = this.cachedLockedAttrsList.IndexOf(attrId) != -1;
    return flag;
  }

  [SpecialName]
  PropertyDescriptorCollection IGtcObjectPropDescriptorHolder.get_PropDescriptorCollection()
  {
    return this.PropDescriptorCollection;
  }
}
