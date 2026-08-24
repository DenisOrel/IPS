// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IRelationTypeItemList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>Интерфейс для списка типов связей</summary>
public interface IRelationTypeItemList : 
  ITypeItemList<IRelationTypeItem>,
  IList<IRelationTypeItem>,
  ICollection<IRelationTypeItem>,
  IEnumerable<IRelationTypeItem>,
  IEnumerable,
  IList,
  ICollection
{
  /// <summary>Привязка типа атрибута к типу объекта</summary>
  /// <param name="attrTypeId">Идентификатор атрибута</param>
  /// <param name="relTypeId">Идентификатор типа связи</param>
  /// <param name="requiredMod">Режим добавления параметра</param>
  /// <param name="validationRule">Правило валидации значений, попадающих в этот атрибут
  /// в контексте данного типа объекта.</param>
  /// <param name="computeMode">Вычисляемый параметр или нет:
  /// 0 - обычный;
  /// 1 - вычисляемый с хранением в базе;
  /// 2 - вычисляемый "на лету".</param>
  /// <param name="formula">Формула вычисления значения</param>
  /// <param name="defaultValue">Значение по умолчанию.</param>
  /// <param name="inViewMode">Управляет наличием данного атрибута в общих таблицах представления атрибута на чтение
  /// 0 - не добавлять атрибут в таблицу
  /// 1 - добавлять без индекса
  /// 2 - добавлять с индексом</param>
  /// <param name="isContent">Влияет ли данный атрибут на содержимое объекта</param>
  /// <param name="options">Флаги с настройками
  /// 1 - регистрировать запись атрибута в журнале,
  /// 2 - сохранять персональную историю значений атрибута,
  /// 3 - сохранять общую историю значений атрибута,
  /// 4 - не допускать пустых значений атрибута</param>
  /// <param name="mask">Маска ввода значения атрибута</param>
  /// <param name="masterId">Идентификатор мастер-атрибута. Если  &gt; 0, то это подчиненный атрибут.
  /// (его значение зависит от значения мастер-атрибута).</param>
  /// <param name="sourceId">Ид. атрибута, из которого подчиненный атрибут
  /// выбирает данные у объекта-источника.</param>
  void LinkAttributeTypeToRelationType(
    int attrTypeId,
    int relTypeId,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    string defaultValue,
    short inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId);
}
