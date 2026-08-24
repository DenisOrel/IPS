// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptTextChangesBuilder
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Класс служит для хранения и обработки изменений в тексте сценария
/// </summary>
public class ScriptTextChangesBuilder
{
  private List<ScriptTextChange> internalList;

  /// <summary>Создает объект.</summary>
  public ScriptTextChangesBuilder()
  {
    this.OptimizedBuild = true;
    this.internalList = new List<ScriptTextChange>(256 /*0x0100*/);
  }

  /// <summary>
  /// Возвращает признак, пуст или заполнен список изменений.
  /// </summary>
  public bool IsEmpty => this.internalList.Count == 0;

  /// <summary>
  /// Задает, требуется ли оптимизировать список изменений (склеивать последовательные).
  /// По умолчанию равен 'true'
  /// </summary>
  public bool OptimizedBuild { get; set; }

  /// <summary>Удаляет все содержащиеся в объекте изменения</summary>
  public void Clear()
  {
    if (this.IsEmpty)
      return;
    this.internalList.Clear();
  }

  /// <summary>
  /// Добавляет изменение <paramref name="textChange" /> к коллекции изменений
  /// </summary>
  /// <param name="textChange">Новое изменение для добавления</param>
  public void Add(ScriptTextChange textChange)
  {
    if (textChange == null)
      throw new ArgumentNullException(nameof (textChange));
    this.internalList.Add(textChange);
  }

  /// <summary>
  /// Формирует окончательный список изменений. В случае, если соответствующим свойством
  /// задано оптимизированное формирование списка, метод склеивает связанные изменения:
  /// последовательные добавления и удаления символов
  /// </summary>
  /// <returns>Окончательный список изменений</returns>
  public List<ScriptTextChange> Build()
  {
    List<ScriptTextChange> scriptTextChangeList1 = new List<ScriptTextChange>();
    List<ScriptTextChange> scriptTextChangeList2 = !this.OptimizedBuild ? this.internalList.ToList<ScriptTextChange>() : this.BuildOptimizedChangesList();
    this.internalList.Clear();
    return scriptTextChangeList2;
  }

  /// <summary>
  /// Оптимизирует список накопленных изменений: склеивает последовательные добавления и удаления символов
  /// </summary>
  /// <returns>Оптимизированный список изменений</returns>
  private List<ScriptTextChange> BuildOptimizedChangesList()
  {
    List<ScriptTextChange> scriptTextChangeList = new List<ScriptTextChange>();
    if (!this.internalList.Any<ScriptTextChange>())
      return scriptTextChangeList;
    int index1;
    for (int index2 = 0; index2 < this.internalList.Count; index2 = index1 - 1 + 1)
    {
      if (index2 == this.internalList.Count - 1)
      {
        scriptTextChangeList.Add(this.internalList[index2]);
        break;
      }
      index1 = index2 + 1;
      ScriptTextChange change = this.internalList[index2];
      for (; index1 < this.internalList.Count; ++index1)
      {
        ScriptTextChange connectedChange = this.internalList[index1];
        (ScriptTextChange, bool) tuple = this.FulfillChange(change, connectedChange);
        change = tuple.Item1;
        if (!tuple.Item2)
          break;
      }
      if (change.InsertedText != "" || change.RemovedLength != 0)
        scriptTextChangeList.Add(change);
    }
    return scriptTextChangeList;
  }

  /// <summary>
  /// Проверяет, был ли в изменении <paramref name="connectedChange" /> вставлен текст сразу за текстом из изменения <paramref name="change" />
  /// </summary>
  /// <param name="change">Текущее изменение в тексте</param>
  /// <param name="connectedChange">Изменение, которое может быть связано с текущим</param>
  /// <returns>true, если изменения последовательны, false иначе</returns>
  private bool CheckConnectedTextInsertion(
    ScriptTextChange change,
    ScriptTextChange connectedChange)
  {
    return connectedChange.Offset == change.Offset + change.InsertedText.Length && connectedChange.RemovedLength == 0;
  }

  /// <summary>
  /// Проверяет, удалилась ли часть текста из изменений в <paramref name="change" /> изменениями в <paramref name="connectedChange" />
  /// </summary>
  /// <param name="change">Текущее изменение в тексте</param>
  /// <param name="connectedChange">Изменение, которое может быть связано с текущим</param>
  /// <returns>true, если изменения последовательны, false иначе</returns>
  private bool CheckConnectedTextDeletion(ScriptTextChange change, ScriptTextChange connectedChange)
  {
    return connectedChange.Offset == change.Offset + change.InsertedText.Length - 1 && change.InsertedText.Length >= connectedChange.RemovedLength && connectedChange.RemovedLength != 0;
  }

  /// <summary>
  /// Проверяет, следует ли изменение <paramref name="connectedChange" /> сразу за <paramref name="change" />.
  /// Если следует, то склеивает данные изменения в одно и возвращает его,
  /// в противном случае возвращает неизмененное <paramref name="change" />
  /// </summary>
  /// <param name="change">Текущее изменение в тексте</param>
  /// <param name="connectedChange">Изменение, которое может быть связано с текущим</param>
  /// <returns>Склеенное изменение и true, если изменения связаны, исходное изменение и false в противном случае</returns>
  private (ScriptTextChange, bool) FulfillChange(
    ScriptTextChange change,
    ScriptTextChange connectedChange)
  {
    bool flag = false;
    if (this.CheckConnectedTextInsertion(change, connectedChange))
    {
      string insertedText = change.InsertedText + connectedChange.InsertedText;
      change = new ScriptTextChange(change.Offset, change.RemovedLength, insertedText);
      flag = true;
    }
    else if (this.CheckConnectedTextDeletion(change, connectedChange))
    {
      int length = change.InsertedText.Length - connectedChange.RemovedLength;
      string insertedText = change.InsertedText.Substring(0, length) + connectedChange.InsertedText;
      change = new ScriptTextChange(change.Offset, change.RemovedLength, insertedText);
      flag = true;
    }
    return (change, flag);
  }
}
