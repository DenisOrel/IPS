// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.RequirementCreatedForm
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using ImSSP;
using Intermech.Requirement.Diff;
using Intermech.Requirement.Properties;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents;
using Telerik.WinForms.Documents.FormatProviders.Rtf;
using Telerik.WinForms.Documents.Model;
using Telerik.WinForms.Documents.TextSearch;
using Telerik.WinForms.Documents.UI.Extensibility;

#nullable disable
namespace Intermech.Requirement;

public class RequirementCreatedForm : Form
{
  private string _docFilePath = string.Empty;
  private Microsoft.Office.Interop.Word.Application _wordapp;
  private List<DeletedNodes> _deletedNodesName = new List<DeletedNodes>();
  private List<AddNodesNames> _addNodesName = new List<AddNodesNames>();
  private List<NodeTreeFromWord> _nodeTreeFromWords = new List<NodeTreeFromWord>();
  private List<NodeTreeFromWord> _newTreeFromWords = new List<NodeTreeFromWord>();
  private List<ChangesNodesRelation> _changesNodesRelations = new List<ChangesNodesRelation>();
  private List<DeletedObjectRelationWithID> _deletedObjectRelationWithID = new List<DeletedObjectRelationWithID>();
  private bool isChanged;
  private int catIndex;
  private int catIndex2;
  private int catIndex3;
  private int catIndex4;
  private int catIndex5;
  private int catIndex6;
  private int catIndex7;
  private int catIndex8;
  private int itter;
  private bool isCancel;
  private IContainer components;
  private Button btnUnCheked;
  private Button btnCheked;
  private Button btnGetTree;
  private TreeViewOverrideDblClick treeView1;
  private Button btnCancel;
  private SplitContainer splitContainer1;
  private ContextMenuStrip contextMenuStrip1;
  private ToolStripMenuItem свернутьВсёToolStripMenuItem;
  private ToolStripMenuItem развернутьВсёToolStripMenuItem;
  private ToolTip toolTip1;
  private GroupBox groupBox1;
  private Label label1;
  private ContextMenuStrip contextMenuStrip2;
  private ToolStripMenuItem setsFlagsOnSelectToolStripMenuItem;
  private ToolStripMenuItem unsetsFlagsOnSelectToolStripMenuItem;
  private Label label5;
  private Label label4;
  public ImageList imgStatusList;
  private Label label6;
  private Label label3;
  private Label label2;
  private RadRichTextEditor radRichTextEditor1;
  private Windows8Theme windows8Theme1;

  public RequirementCreatedForm() => this.InitializeComponent();

  public RequirementCreatedForm(string filePath, List<NodeTreeFromWord> nodeTreeFromWords)
  {
    this.InitializeComponent();
    this.radRichTextEditor1.RichTextBoxElement.FindReplaceDialog = (IFindReplaceDialog) new CustomFindReplaceDialogTelerik();
    this.radRichTextEditor1.HyperlinkToolTipFormatString = "Нажмите {1} для перехода по ссылке. (Адрес ссылки: {0})";
    this._deletedNodesName.Clear();
    this._addNodesName.Clear();
    this._newTreeFromWords.Clear();
    this._changesNodesRelations.Clear();
    this._deletedObjectRelationWithID.Clear();
    this._docFilePath = filePath;
    RequirementConst.CheckFormResult = false;
    try
    {
      this._wordapp = (Microsoft.Office.Interop.Word.Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("000209FF-0000-0000-C000-000000000046")));
    }
    catch (Exception ex)
    {
      throw new KernelException(ex.Message);
    }
    nodeTreeFromWords.SortAllList();
    this._nodeTreeFromWords = nodeTreeFromWords;
    RequirementConst.IsHaveCompisition = this._nodeTreeFromWords.Count > 0;
  }

  private void RequirementCreatedForm_Load(object sender, EventArgs e)
  {
    this.treeView1.ImageList = this.imgStatusList;
    try
    {
      object docFilePath = (object) this._docFilePath;
      object ConfirmConversions = (object) true;
      object obj = (object) false;
      object missing = Type.Missing;
      // ISSUE: reference to a compiler-generated method
      this._wordapp.Documents.Open(ref docFilePath, ref ConfirmConversions, ref obj, ref obj, ref missing, ref missing, ref obj, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref obj, ref missing);
      object paragraphs = (object) this._wordapp.ActiveDocument.Paragraphs;
      // ISSUE: reference to a compiler-generated field
      if (RequirementCreatedForm.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RequirementCreatedForm.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Action<CallSite, RequirementCreatedForm, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "LoadTreeFromWordFiles", (IEnumerable<Type>) null, typeof (RequirementCreatedForm), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      RequirementCreatedForm.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) RequirementCreatedForm.\u003C\u003Eo__20.\u003C\u003Ep__0, this, paragraphs);
      if (this._nodeTreeFromWords.Count > 0)
      {
        this.CheckWordTree(this._newTreeFromWords, this._nodeTreeFromWords);
        if (!this.isChanged)
        {
          int num = (int) MessageBox.Show(sc_17729.ssp_improject_17730());
        }
      }
      this.LoadDataToRichBox(this._wordapp);
    }
    catch (Exception ex)
    {
      object missing = Type.Missing;
      object SaveChanges = (object) false;
      // ISSUE: reference to a compiler-generated method
      this._wordapp.ActiveDocument.Close(ref SaveChanges, ref missing, ref missing);
      // ISSUE: reference to a compiler-generated method
      this._wordapp.Quit(ref SaveChanges, ref missing, ref missing);
      this.Close();
      throw new KernelException(ex.Message);
    }
  }

  private void CheckWordTree(
    List<NodeTreeFromWord> newTreeList,
    List<NodeTreeFromWord> nodeTreeFromWords)
  {
    try
    {
      List<NodeTreeFromWord> nodeTreeFromWordList1 = new List<NodeTreeFromWord>();
      List<NodeTreeFromWord> nodeTreeFromWordList2 = new List<NodeTreeFromWord>((IEnumerable<NodeTreeFromWord>) newTreeList);
      List<NodeTreeFromWord> nodeTreeFromWordList3 = new List<NodeTreeFromWord>((IEnumerable<NodeTreeFromWord>) nodeTreeFromWords);
      Dictionary<NodeTreeFromWord, NodeTreeFromWord> dictionary = new Dictionary<NodeTreeFromWord, NodeTreeFromWord>();
      for (int i = 0; i < newTreeList.Count; i++)
      {
        NodeTreeFromWord nodeTreeFromWord = nodeTreeFromWordList3.FirstOrDefault<NodeTreeFromWord>((Func<NodeTreeFromWord, bool>) (x => x.Name == newTreeList[i].Name));
        if (nodeTreeFromWord != null)
        {
          dictionary.Add(newTreeList[i], nodeTreeFromWord);
          nodeTreeFromWordList2.Remove(newTreeList[i]);
          nodeTreeFromWordList3.Remove(nodeTreeFromWord);
        }
        else
          nodeTreeFromWordList1.Add(newTreeList[i]);
      }
      for (int index1 = 0; index1 < nodeTreeFromWordList1.Count; ++index1)
      {
        string textFromNode = this.GenerateTextFromNode(nodeTreeFromWordList1[index1]);
        for (int index2 = 0; index2 < nodeTreeFromWordList3.Count; ++index2)
        {
          if (this.TextDiff(this.GenerateTextFromNode(nodeTreeFromWordList3[index2]), textFromNode, false))
          {
            dictionary.Add(nodeTreeFromWordList1[index1], nodeTreeFromWordList3[index2]);
            nodeTreeFromWordList3.Remove(nodeTreeFromWordList3[index2]);
            nodeTreeFromWordList2.Remove(nodeTreeFromWordList1[index1]);
            nodeTreeFromWordList1.Remove(nodeTreeFromWordList1[index1]);
            --index1;
            break;
          }
        }
      }
      foreach (KeyValuePair<NodeTreeFromWord, NodeTreeFromWord> keyValuePair in dictionary)
      {
        int index = nodeTreeFromWords.IndexOf(keyValuePair.Value);
        int indexInTree = newTreeList.IndexOf(keyValuePair.Key);
        string textFromNode = this.GenerateTextFromNode(keyValuePair.Key);
        this.DiffNew(this.GenerateTextFromNode(keyValuePair.Value), textFromNode, index, indexInTree);
      }
      for (int index = 0; index < nodeTreeFromWordList2.Count; ++index)
        this.ColorizeAllNodesBeginParent(this.treeView1.Nodes[this._newTreeFromWords.IndexOf(nodeTreeFromWordList2[index])]);
      if (nodeTreeFromWordList3.Count > 0)
        this.CheckOldNodes(nodeTreeFromWordList3);
      for (int index = 0; index < this._addNodesName.Count; ++index)
        this.SearchNodeAndColorize(this._addNodesName[index].TTName, this.treeView1.Nodes[this._addNodesName[index].ParentIndex], this._addNodesName[index].IndexEntry, Color.LightSteelBlue);
      for (int index = 0; index < this._deletedNodesName.Count; ++index)
      {
        NodeTreeFromWord nodeTreeFromWord = this._nodeTreeFromWords[this._deletedNodesName[index].ParentIndex].Find(this._deletedNodesName[index].DeletedTTName, this._deletedNodesName[index].IndexEntry);
        if (nodeTreeFromWord != null)
        {
          nodeTreeFromWord.IsDeleted = true;
          this._deletedObjectRelationWithID.Add(new DeletedObjectRelationWithID()
          {
            DeletedTTID = nodeTreeFromWord.TTObjectID,
            DeletedTTName = this._deletedNodesName[index].DeletedTTName
          });
          if (nodeTreeFromWord.ParentName == "TZ")
          {
            this.treeView1.Nodes.Add(this._deletedNodesName[index].DeletedTTName, this._deletedNodesName[index].DeletedTTName).ForeColor = Color.Red;
            this.treeView1.Nodes[this._deletedNodesName[index].DeletedTTName].NodeFont = new Font(this.treeView1.Font.FontFamily, this.treeView1.Font.Size, FontStyle.Strikeout);
          }
          else
          {
            foreach (TreeNode node in this.treeView1.Nodes)
            {
              if (node.Name.Equals(nodeTreeFromWord.ParentName))
              {
                node.Nodes.Add(nodeTreeFromWord.Name, nodeTreeFromWord.Name).ForeColor = Color.Red;
                node.Nodes[nodeTreeFromWord.Name].NodeFont = new Font(this.treeView1.Font.FontFamily, this.treeView1.Font.Size, FontStyle.Strikeout);
                node.Expand();
                break;
              }
              if (node.Nodes.Count > 0)
              {
                if (this.DelChild(node.Nodes, nodeTreeFromWord.Name, nodeTreeFromWord.ParentName))
                  break;
              }
            }
          }
        }
      }
      for (int index = this._changesNodesRelations.Count - 1; index >= 0; --index)
      {
        this.SearchNodeAndColorize(this._changesNodesRelations[index].New, this.treeView1.Nodes[this._changesNodesRelations[index].IndexNewParentNodes], this._changesNodesRelations[index].IndexEntry, Color.DarkOrange);
        ChangesNodesRelation changesNodesRelation = this._changesNodesRelations.ElementAt<ChangesNodesRelation>(index);
        NodeTreeFromWord nodeTreeFromWord = this.SearchValueInWordTTList(changesNodesRelation.New, this._newTreeFromWords);
        string indexHierarhi = nodeTreeFromWord == null ? string.Empty : nodeTreeFromWord.TTIndexInDocument;
        this._nodeTreeFromWords[changesNodesRelation.IndexParentNodes] = this._nodeTreeFromWords[changesNodesRelation.IndexParentNodes].FindAndReplace(changesNodesRelation.Old, changesNodesRelation.IndexEntry, changesNodesRelation.New, indexHierarhi);
      }
      foreach (NodeTreeFromWord nodeTreeFromWord in this._nodeTreeFromWords)
        this.ReplaceIcon(nodeTreeFromWord);
      this._newTreeFromWords.RebuilList();
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message, ex.InnerException);
    }
  }

  private void ColorizeAllNodesBeginParent(TreeNode treeNode)
  {
    this.isChanged = true;
    treeNode.ForeColor = Color.LightSteelBlue;
    if (treeNode.Nodes.Count <= 0)
      return;
    this.ColorizeAllNodesBeginChild(treeNode.Nodes);
  }

  private void ColorizeAllNodesBeginChild(TreeNodeCollection treeNode)
  {
    foreach (TreeNode treeNode1 in treeNode)
    {
      treeNode1.ForeColor = Color.LightSteelBlue;
      if (treeNode1.Nodes.Count > 0)
        this.ColorizeAllNodesBeginChild(treeNode1.Nodes);
    }
  }

  private void SearchNodeAndColorize(
    string searchText,
    TreeNode startNode,
    int indexEntry,
    Color color)
  {
    int search = 0;
    if (startNode.Text.ToLower().Equals(searchText.ToLower()))
    {
      if (search == indexEntry)
      {
        startNode.ForeColor = color;
        this.isChanged = true;
        return;
      }
      ++search;
    }
    if (startNode.Nodes.Count <= 0)
      return;
    this.SearchNodeAndColorizeChild(searchText, startNode.Nodes, indexEntry, color, ref search);
  }

  private void SearchNodeAndColorizeChild(
    string searchText,
    TreeNodeCollection startNodes,
    int indexEntry,
    Color color,
    ref int search)
  {
    foreach (TreeNode startNode in startNodes)
    {
      if (startNode.Text.ToLower().Equals(searchText.ToLower()))
      {
        if (search == indexEntry)
        {
          startNode.ForeColor = color;
          this.ExpandedNodesToUpper(startNode.Parent, startNode);
          this.isChanged = true;
          ++search;
          break;
        }
        ++search;
      }
      if (startNode.Nodes.Count > 0)
        this.SearchNodeAndColorizeChild(searchText, startNode.Nodes, indexEntry, color, ref search);
    }
  }

  private void ExpandedNodesToUpper(TreeNode parent, TreeNode child)
  {
    if (parent == null)
    {
      child?.Expand();
    }
    else
    {
      this.ExpandedNodesToUpper(parent.Parent, parent);
      parent.Expand();
    }
  }

  private void CheckOldNodes(List<NodeTreeFromWord> oldTreeListAfterDeletedTrue)
  {
    for (int index1 = 0; index1 < oldTreeListAfterDeletedTrue.Count; ++index1)
    {
      NodeTreeFromWord value = this.SearchValueInWordTTList(oldTreeListAfterDeletedTrue[index1].Name, oldTreeListAfterDeletedTrue[index1].TTDescription, this._newTreeFromWords);
      int index2 = this._nodeTreeFromWords.IndexOf(oldTreeListAfterDeletedTrue[index1]);
      NodeTreeFromWord nodeTreeFromWord = this._nodeTreeFromWords[index2].Find(oldTreeListAfterDeletedTrue[index1].Name, 0);
      if (value == null)
      {
        if (nodeTreeFromWord != null)
        {
          nodeTreeFromWord.IsDeleted = true;
          this._deletedNodesName.Add(new DeletedNodes()
          {
            DeletedTTName = oldTreeListAfterDeletedTrue[index1].Name,
            IndexEntry = 0,
            ParentIndex = index2
          });
        }
      }
      else
      {
        AddNodesNames addNodesNames = this._addNodesName.FirstOrDefault<AddNodesNames>((Func<AddNodesNames, bool>) (x => x.TTName == value.Name));
        if (addNodesNames != null)
          this._newTreeFromWords[addNodesNames.ParentIndex].Find(addNodesNames.TTName, addNodesNames.IndexEntry).OldNode = oldTreeListAfterDeletedTrue[index1];
        this._addNodesName.Remove(addNodesNames);
      }
      if (oldTreeListAfterDeletedTrue[index1].IsHaveChild)
        this.CheckThisInWordChild(oldTreeListAfterDeletedTrue[index1].Child, index2);
    }
  }

  private void CheckThisInWordChild(List<NodeTreeFromWord> nodeTreeFromWord, int index)
  {
    for (int index1 = 0; index1 < nodeTreeFromWord.Count; ++index1)
    {
      NodeTreeFromWord value = this.SearchValueInWordTTList(nodeTreeFromWord[index1].Name, nodeTreeFromWord[index1].TTDescription, this._newTreeFromWords);
      NodeTreeFromWord nodeTreeFromWord1 = this._nodeTreeFromWords[index].Find(nodeTreeFromWord[index1].Name, 0);
      if (value == null)
      {
        if (nodeTreeFromWord1 != null)
          nodeTreeFromWord1.IsDeleted = true;
        this._deletedNodesName.Add(new DeletedNodes()
        {
          DeletedTTName = nodeTreeFromWord[index1].Name,
          IndexEntry = 0,
          ParentIndex = index
        });
      }
      else
      {
        AddNodesNames addNodesNames = this._addNodesName.FirstOrDefault<AddNodesNames>((Func<AddNodesNames, bool>) (x => x.TTName == value.Name));
        if (addNodesNames != null)
          this._newTreeFromWords[addNodesNames.ParentIndex].Find(addNodesNames.TTName, addNodesNames.IndexEntry).OldNode = nodeTreeFromWord[index1];
        this._addNodesName.Remove(addNodesNames);
      }
      if (nodeTreeFromWord[index1].IsHaveChild)
        this.CheckThisInWordChild(nodeTreeFromWord[index1].Child, index);
    }
  }

  private NodeTreeFromWord SearchValueInWordTTList(string searchName, List<NodeTreeFromWord> dict)
  {
    foreach (NodeTreeFromWord nodeTreeFromWord1 in dict)
    {
      if (nodeTreeFromWord1.Name == searchName)
        return nodeTreeFromWord1;
      if (nodeTreeFromWord1.Child.Count > 0)
      {
        NodeTreeFromWord nodeTreeFromWord2 = this.SearchValueInWordTTList(searchName, nodeTreeFromWord1.Child);
        if (nodeTreeFromWord2 != null)
          return nodeTreeFromWord2;
      }
    }
    return (NodeTreeFromWord) null;
  }

  private NodeTreeFromWord SearchValueInWordTTList(
    string searchName,
    string searchDescription,
    List<NodeTreeFromWord> dict)
  {
    foreach (NodeTreeFromWord nodeTreeFromWord1 in dict)
    {
      string str = nodeTreeFromWord1.TTDescription.Length > 450 ? nodeTreeFromWord1.TTDescription.Substring(0, 450) : nodeTreeFromWord1.TTDescription;
      if (nodeTreeFromWord1.Name.Equals(searchName) && str.Equals(searchDescription))
        return nodeTreeFromWord1;
      if (nodeTreeFromWord1.Child.Count > 0)
      {
        NodeTreeFromWord nodeTreeFromWord2 = this.SearchValueInWordTTList(searchName, searchDescription, nodeTreeFromWord1.Child);
        if (nodeTreeFromWord2 != null)
          return nodeTreeFromWord2;
      }
    }
    return (NodeTreeFromWord) null;
  }

  private void ReplaceIcon(NodeTreeFromWord node)
  {
    foreach (TreeNode node1 in this.treeView1.Nodes)
    {
      if (node1.Name == node.Name)
      {
        node1.ImageIndex = node.TTLCStep;
        node1.SelectedImageIndex = node.TTLCStep;
        node1.StateImageIndex = node.TTLCStep;
        break;
      }
      if (node1.Nodes.Count > 0 && this.ChildIcon(node, node1.Nodes))
        break;
    }
  }

  private bool ChildIcon(NodeTreeFromWord node, TreeNodeCollection tree)
  {
    foreach (TreeNode treeNode in tree)
    {
      if (treeNode.Name == node.Name)
      {
        treeNode.ImageIndex = node.TTLCStep;
        treeNode.SelectedImageIndex = node.TTLCStep;
        treeNode.StateImageIndex = node.TTLCStep;
        return true;
      }
      if (treeNode.Nodes.Count > 0 && this.ChildIcon(node, treeNode.Nodes))
        return true;
    }
    return false;
  }

  private string GenerateTextFromNode(NodeTreeFromWord nodeTreeFromWord)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (!string.IsNullOrEmpty(nodeTreeFromWord.Name))
      stringBuilder.Append(nodeTreeFromWord.Name + "\r");
    for (int index = 0; index < nodeTreeFromWord.Child.Count; ++index)
      stringBuilder.Append(this.GenerateTextFromNode(nodeTreeFromWord.Child[index]));
    return stringBuilder.ToString();
  }

  private bool TextDiff(string oldText, string newText, bool flag)
  {
    bool flag1 = true;
    DiffList_TextFile source;
    DiffList_TextFile destination;
    try
    {
      source = new DiffList_TextFile(oldText);
      destination = new DiffList_TextFile(newText);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "Ошибка в сборе пунктов");
      return false;
    }
    try
    {
      DiffEngine diffEngine = new DiffEngine();
      diffEngine.ProcessDiff((IDiffList) source, (IDiffList) destination, DiffEngineLevel.Medium);
      ArrayList arrayList = diffEngine.DiffReport();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      foreach (DiffResultSpan diffResultSpan in arrayList)
      {
        switch (diffResultSpan.Status)
        {
          case DiffResultSpanStatus.NoChange:
            for (int index = 0; index < diffResultSpan.Length; ++index)
            {
              if (!flag)
                ++num3;
            }
            continue;
          case DiffResultSpanStatus.Replace:
            for (int index = 0; index < diffResultSpan.Length; ++index)
            {
              if (!flag)
              {
                ++num2;
                if (diffResultSpan.SourceIndex == 0)
                {
                  DiffPiece line = new SideBySideDiffBuilder((IDiffer) new Differ()).BuildDiffModel(oldText, newText).NewText.Lines[diffResultSpan.DestIndex];
                  if ((double) line.SubPieces.Count / 2.0 - 0.3 <= (double) line.SubPieces.Count<DiffPiece>((Func<DiffPiece, bool>) (piec => piec.Type == ChangeType.Unchanged)))
                    flag1 = false;
                }
              }
            }
            continue;
          case DiffResultSpanStatus.DeleteSource:
            for (int index = 0; index < diffResultSpan.Length; ++index)
            {
              if (!flag)
                ++num1;
            }
            continue;
          case DiffResultSpanStatus.AddDestination:
            for (int index = 0; index < diffResultSpan.Length; ++index)
            {
              if (!flag)
                ++num4;
            }
            continue;
          default:
            continue;
        }
      }
      if (!flag)
        return ((num1 != source.Count() - destination.Count() ? 0 : (num2 == destination.Count() ? 1 : 0)) & (flag1 ? 1 : 0)) == 0 && ((num4 != destination.Count() - source.Count() ? 0 : (num2 == source.Count() ? 1 : 0)) & (flag1 ? 1 : 0)) == 0;
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(string.Format("{0}{1}{1}***STACK***{1}{2}", (object) ex.Message, (object) Environment.NewLine, (object) ex.StackTrace), "Ошибка сравнения");
      return false;
    }
    return false;
  }

  private void DiffNew(string oldText, string newText, int index, int indexInTree)
  {
    SideBySideDiffModel sideBySideDiffModel = new SideBySideDiffBuilder((IDiffer) new Differ()).BuildDiffModel(oldText, newText);
    List<DiffPiece> linesOld = sideBySideDiffModel.OldText.Lines;
    List<DiffPiece> linesNew = sideBySideDiffModel.NewText.Lines;
    int num1 = 0;
    int num2 = 0;
    for (int i = 0; i < linesNew.Count; i++)
    {
      switch (linesNew[i].Type)
      {
        case ChangeType.Unchanged:
          int ttEntry1 = 0;
          int ttEntry2 = 0;
          List<DiffPiece> list1 = linesNew.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesNew[i].Text))).ToList<DiffPiece>();
          if (list1.Count > 1)
          {
            for (int index1 = 0; index1 < list1.Count; ++index1)
            {
              DiffPiece diffPiece = list1[index1];
              if (linesNew.IndexOf(diffPiece) == i)
                ttEntry1 = index1;
            }
          }
          List<DiffPiece> list2 = linesOld.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesOld[i].Text))).ToList<DiffPiece>();
          if (list2.Count > 1)
          {
            for (int index2 = 0; index2 < list2.Count; ++index2)
            {
              DiffPiece diffPiece = list2[index2];
              if (linesOld.IndexOf(diffPiece) == i)
                ttEntry2 = index2;
            }
          }
          NodeTreeFromWord nodeTreeFromWord1 = this._newTreeFromWords[indexInTree].Find(linesNew[i].Text, ttEntry1);
          NodeTreeFromWord nodeTreeFromWord2 = this._nodeTreeFromWords[index].Find(linesNew[i].Text, ttEntry2);
          if (nodeTreeFromWord2 != null)
          {
            nodeTreeFromWord1.OldNode = nodeTreeFromWord2;
            break;
          }
          break;
        case ChangeType.Inserted:
          if (i != 0 && linesNew[i - 1].Type == ChangeType.Modified)
          {
            foreach (DiffPiece subPiece in linesNew[i - 1].SubPieces)
            {
              if (subPiece.Type == ChangeType.Unchanged)
                ++num1;
              if (subPiece.Type == ChangeType.Imaginary)
                ++num2;
            }
            if (((double) (linesNew[i - 1].SubPieces.Count - num2) + 0.1) / 2.0 >= (double) num1)
            {
              int num3 = 0;
              List<DiffPiece> list3 = linesNew.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesNew[i - 1].Text))).ToList<DiffPiece>();
              if (list3.Count > 1)
              {
                for (int index3 = 0; index3 < list3.Count; ++index3)
                {
                  if (linesNew.IndexOf(list3[index3]) == i - 1)
                    num3 = index3;
                }
              }
              this._addNodesName.Add(new AddNodesNames()
              {
                TTName = linesNew[i - 1].Text,
                IndexEntry = num3,
                ParentIndex = indexInTree
              });
              ChangesNodesRelation last = this._changesNodesRelations.FindLast((Predicate<ChangesNodesRelation>) (x => x.New == linesNew[i - 1].Text && index == x.IndexParentNodes));
              int ttEntry3 = 0;
              List<DiffPiece> list4 = linesOld.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesOld[i - 1].Text))).ToList<DiffPiece>();
              if (list4.Count > 1)
              {
                for (int index4 = 0; index4 < list4.Count; ++index4)
                {
                  DiffPiece diffPiece = list4[index4];
                  if (linesOld.IndexOf(diffPiece) == i)
                    ttEntry3 = index4;
                }
              }
              int ttEntry4 = 0;
              List<DiffPiece> list5 = linesNew.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesNew[i].Text))).ToList<DiffPiece>();
              if (list4.Count > 1)
              {
                for (int index5 = 0; index5 < list5.Count; ++index5)
                {
                  DiffPiece diffPiece = list5[index5];
                  if (linesNew.IndexOf(diffPiece) == i)
                    ttEntry4 = index5;
                }
              }
              NodeTreeFromWord nodeTreeFromWord3 = this._nodeTreeFromWords[index].Find(linesOld[i - 1].Text, ttEntry3);
              NodeTreeFromWord nodeTreeFromWord4 = this._newTreeFromWords[indexInTree].Find(linesNew[i].Text, ttEntry4);
              if (nodeTreeFromWord3 != null)
              {
                nodeTreeFromWord4.OldNode = nodeTreeFromWord3;
                this._changesNodesRelations.Remove(last);
                this._changesNodesRelations.Add(new ChangesNodesRelation()
                {
                  New = linesNew[i].Text,
                  Old = linesOld[i - 1].Text,
                  IndexParentNodes = index,
                  IndexEntry = ttEntry4,
                  IndexNewParentNodes = indexInTree
                });
                num1 = 0;
                num2 = 0;
                break;
              }
              break;
            }
            int num4 = 0;
            List<DiffPiece> list6 = linesNew.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesNew[i].Text))).ToList<DiffPiece>();
            if (list6.Count > 1)
            {
              for (int index6 = 0; index6 < list6.Count; ++index6)
              {
                if (linesNew.IndexOf(list6[index6]) == i)
                  num4 = index6;
              }
            }
            this._addNodesName.Add(new AddNodesNames()
            {
              TTName = linesNew[i].Text,
              IndexEntry = num4,
              ParentIndex = indexInTree
            });
            break;
          }
          int num5 = 0;
          List<DiffPiece> list7 = linesNew.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesNew[i].Text))).ToList<DiffPiece>();
          if (list7.Count > 1)
          {
            for (int index7 = 0; index7 < list7.Count; ++index7)
            {
              if (linesNew.IndexOf(list7[index7]) == i)
                num5 = index7;
            }
          }
          this._addNodesName.Add(new AddNodesNames()
          {
            TTName = linesNew[i].Text,
            IndexEntry = num5,
            ParentIndex = indexInTree
          });
          break;
        case ChangeType.Imaginary:
          if (i != 0)
          {
            if (linesNew[i - 1].Type == ChangeType.Modified)
            {
              foreach (DiffPiece subPiece in linesNew[i - 1].SubPieces)
              {
                if (subPiece.Type == ChangeType.Unchanged)
                  ++num1;
                if (subPiece.Type == ChangeType.Imaginary)
                  ++num2;
              }
              if (((double) (linesNew[i - 1].SubPieces.Count - num2) + 0.2) / 2.0 > (double) num1)
              {
                ChangesNodesRelation last = this._changesNodesRelations.FindLast((Predicate<ChangesNodesRelation>) (x => x.New == linesNew[i - 1].Text && x.IndexParentNodes == index));
                int num6 = 0;
                List<DiffPiece> list8 = linesOld.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesOld[i - 1].Text))).ToList<DiffPiece>();
                if (list8.Count > 1)
                {
                  for (int index8 = 0; index8 < list8.Count; ++index8)
                  {
                    DiffPiece diffPiece = list8[index8];
                    if (linesOld.IndexOf(diffPiece) == i - 1)
                      num6 = index8;
                  }
                }
                int ttEntry5 = 0;
                List<DiffPiece> list9 = linesNew.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesNew[i - 1].Text))).ToList<DiffPiece>();
                if (list9.Count > 1)
                {
                  for (int index9 = 0; index9 < list9.Count; ++index9)
                  {
                    DiffPiece diffPiece = list9[index9];
                    if (linesNew.IndexOf(diffPiece) == i)
                      ttEntry5 = index9;
                  }
                }
                int ttEntry6 = 0;
                List<DiffPiece> list10 = linesOld.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesOld[i].Text))).ToList<DiffPiece>();
                if (list10.Count > 1)
                {
                  for (int index10 = 0; index10 < list10.Count; ++index10)
                  {
                    DiffPiece diffPiece = list10[index10];
                    if (linesOld.IndexOf(diffPiece) == i)
                      ttEntry6 = index10;
                  }
                }
                this._changesNodesRelations[this._changesNodesRelations.IndexOf(last)].Old = linesOld[i].Text;
                NodeTreeFromWord nodeTreeFromWord5 = this._nodeTreeFromWords[index].Find(linesOld[i].Text, ttEntry6);
                NodeTreeFromWord nodeTreeFromWord6 = this._newTreeFromWords[indexInTree].Find(linesNew[i - 1].Text, ttEntry5);
                if (nodeTreeFromWord5 != null)
                {
                  nodeTreeFromWord6.OldNode = nodeTreeFromWord5;
                  this._deletedNodesName.Add(new DeletedNodes()
                  {
                    IndexEntry = num6,
                    DeletedTTName = linesOld[i - 1].Text,
                    ParentIndex = index
                  });
                  num2 = 0;
                  num1 = 0;
                  break;
                }
                break;
              }
              break;
            }
            int ttEntry7 = 0;
            List<DiffPiece> list11 = linesOld.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesOld[i].Text))).ToList<DiffPiece>();
            if (list11.Count > 1)
            {
              for (int index11 = 0; index11 < list11.Count; ++index11)
              {
                DiffPiece diffPiece = list11[index11];
                if (linesOld.IndexOf(diffPiece) == i)
                  ttEntry7 = index11;
              }
            }
            if (this._nodeTreeFromWords[index].Find(linesOld[i].Text, ttEntry7) != null)
            {
              this._deletedNodesName.Add(new DeletedNodes()
              {
                IndexEntry = ttEntry7,
                DeletedTTName = linesOld[i].Text,
                ParentIndex = index
              });
              break;
            }
            break;
          }
          int ttEntry8 = 0;
          List<DiffPiece> list12 = linesOld.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesOld[i].Text))).ToList<DiffPiece>();
          if (list12.Count > 1)
          {
            for (int index12 = 0; index12 < list12.Count; ++index12)
            {
              DiffPiece diffPiece = list12[index12];
              if (linesOld.IndexOf(diffPiece) == i)
                ttEntry8 = index12;
            }
          }
          if (this._nodeTreeFromWords[index].Find(linesOld[i].Text, ttEntry8) != null)
          {
            this._deletedNodesName.Add(new DeletedNodes()
            {
              IndexEntry = ttEntry8,
              DeletedTTName = linesOld[i].Text,
              ParentIndex = index
            });
            break;
          }
          break;
        case ChangeType.Modified:
          int ttEntry9 = 0;
          List<DiffPiece> list13 = linesOld.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesOld[i].Text))).ToList<DiffPiece>();
          if (list13.Count > 1)
          {
            for (int index13 = 0; index13 < list13.Count; ++index13)
            {
              DiffPiece diffPiece = list13[index13];
              if (linesOld.IndexOf(diffPiece) == i)
                ttEntry9 = index13;
            }
          }
          int ttEntry10 = 0;
          List<DiffPiece> list14 = linesNew.Where<DiffPiece>((Func<DiffPiece, bool>) (x => x.Type != ChangeType.Imaginary && x.Text.Equals(linesNew[i].Text))).ToList<DiffPiece>();
          if (list14.Count > 1)
          {
            for (int index14 = 0; index14 < list14.Count; ++index14)
            {
              DiffPiece diffPiece = list14[index14];
              if (linesNew.IndexOf(diffPiece) == i)
                ttEntry10 = index14;
            }
          }
          NodeTreeFromWord nodeTreeFromWord7 = this._nodeTreeFromWords[index].Find(linesOld[i].Text, ttEntry9);
          NodeTreeFromWord nodeTreeFromWord8 = this._newTreeFromWords[indexInTree].Find(linesNew[i].Text, ttEntry10);
          if (nodeTreeFromWord7 != null)
          {
            nodeTreeFromWord8.OldNode = nodeTreeFromWord7;
            this._changesNodesRelations.Add(new ChangesNodesRelation()
            {
              New = linesNew[i].Text,
              Old = linesOld[i].Text,
              IndexParentNodes = index,
              IndexEntry = ttEntry10,
              IndexNewParentNodes = indexInTree
            });
            break;
          }
          break;
      }
    }
  }

  private bool DelChild(TreeNodeCollection nodeCollection, string deletedNodes, string parentNode)
  {
    foreach (TreeNode node in nodeCollection)
    {
      if (node.Name.Equals(parentNode))
      {
        node.Nodes.Add(deletedNodes, deletedNodes).ForeColor = Color.Red;
        node.Nodes[deletedNodes].NodeFont = new Font(this.treeView1.Font.FontFamily, this.treeView1.Font.Size, FontStyle.Strikeout);
        this.ExpandedNodesToUpper(node.Parent, node);
        return true;
      }
      if (node.Nodes.Count > 0 && this.DelChild(node.Nodes, deletedNodes, parentNode))
        return true;
    }
    return false;
  }

  private void LoadDataToRichBox(Microsoft.Office.Interop.Word.Application wordapp)
  {
    object missing = Type.Missing;
    object obj1 = (object) false;
    object empty1 = (object) string.Empty;
    object empty2 = (object) string.Empty;
    object obj2 = (object) Path.GetFileNameWithoutExtension(wordapp.ActiveDocument.Name);
    string tempPath = Path.GetTempPath();
    Random random = new Random();
    while (File.Exists($"{obj2}.rtf"))
      obj2 = (object) $"{obj2}{random.Next(0, 9)}";
    object FileFormat = (object) WdSaveFormat.wdFormatRTF;
    object FileName = (object) $"{tempPath}{obj2}.rtf";
    // ISSUE: reference to a compiler-generated method
    wordapp.ActiveDocument.SaveAs(ref FileName, ref FileFormat, ref obj1, ref empty1, ref obj1, ref empty2, ref obj1, ref obj1, ref missing, ref obj1, ref obj1, ref missing, ref missing, ref missing, ref missing, ref missing);
    // ISSUE: reference to a compiler-generated method
    wordapp.ActiveDocument.Close(ref obj1, ref missing, ref missing);
    // ISSUE: reference to a compiler-generated method
    wordapp.Quit(ref obj1, ref missing, ref missing);
    using (Stream input = (Stream) File.OpenRead((string) FileName))
      this.radRichTextEditor1.Document = new RtfFormatProvider().Import(input);
  }

  private void LoadTreeFromWordFiles(object paragraphs)
  {
    string level = string.Empty;
    string levelHierarhi = string.Empty;
    string key = string.Empty;
    string indexInDocument = string.Empty;
    StringBuilder sb = new StringBuilder();
    // ISSUE: reference to a compiler-generated field
    if (RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (RequirementCreatedForm), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, int, object> target1 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__1.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, int, object>> p1 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__1;
    // ISSUE: reference to a compiler-generated field
    if (RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof (RequirementCreatedForm), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj1 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__0.Target((CallSite) RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__0, paragraphs);
    object obj2 = target1((CallSite) p1, obj1, 1);
    int num = 1;
    while (true)
    {
      // ISSUE: reference to a compiler-generated field
      if (RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (RequirementCreatedForm), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__2 = CallSite<Func<CallSite, int, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof (RequirementCreatedForm), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__2.Target((CallSite) RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__2, num, obj2);
      if (target2((CallSite) p3, obj3))
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          if (RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Microsoft.Office.Interop.Word.Paragraph>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Microsoft.Office.Interop.Word.Paragraph), typeof (RequirementCreatedForm)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, Microsoft.Office.Interop.Word.Paragraph> target3 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, Microsoft.Office.Interop.Word.Paragraph>> p5 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__5;
          // ISSUE: reference to a compiler-generated field
          if (RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (RequirementCreatedForm), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__4.Target((CallSite) RequirementCreatedForm.\u003C\u003Eo__38.\u003C\u003Ep__4, paragraphs, num);
          // ISSUE: variable of a compiler-generated type
          Microsoft.Office.Interop.Word.Paragraph paragraph = target3((CallSite) p5, obj4);
          // ISSUE: variable of a compiler-generated type
          Microsoft.Office.Interop.Word.Range range = paragraph.Range;
          string rangeText = Regex.Replace(range.Text, "\\p{C}+", string.Empty);
          if (string.IsNullOrEmpty(rangeText))
          {
            if (string.IsNullOrWhiteSpace(rangeText))
              goto label_68;
          }
          if (paragraph.OutlineLevel != WdOutlineLevel.wdOutlineLevelBodyText)
          {
            if (paragraph.OutlineLevel != WdOutlineLevel.wdOutlineLevel1)
            {
              if (this.itter != 0)
              {
                if (key != rangeText)
                {
                  this.AddParagraphToWordList(level, key, sb, levelHierarhi, indexInDocument);
                  string str = range.ListFormat.ListValue != 0 ? range.ListFormat.ListString : string.Empty;
                  key = rangeText;
                  sb.Clear();
                }
                // ISSUE: variable of a compiler-generated type
                WdOutlineLevel outlineLevel = paragraph.OutlineLevel;
                switch (outlineLevel)
                {
                  case WdOutlineLevel.wdOutlineLevel2:
                    if (this.catIndex != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      this.catIndex2 = this.treeView1.Nodes[this.catIndex - 1].Nodes.Count;
                      this.catIndex3 = 0;
                      this.catIndex4 = 0;
                      this.catIndex5 = 0;
                      this.catIndex6 = 0;
                      this.catIndex7 = 0;
                      this.catIndex8 = 0;
                      ++this.itter;
                      level = "2";
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}";
                      break;
                    }
                    break;
                  case WdOutlineLevel.wdOutlineLevel3:
                    if (this.catIndex2 != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child[this.catIndex2 - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      this.catIndex3 = this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes.Count;
                      this.catIndex4 = 0;
                      this.catIndex5 = 0;
                      this.catIndex6 = 0;
                      this.catIndex7 = 0;
                      this.catIndex8 = 0;
                      ++this.itter;
                      level = "3";
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}.{this.catIndex3}";
                      break;
                    }
                    object prop1 = (object) WdBuiltinStyle.wdStyleHeading2;
                    // ISSUE: reference to a compiler-generated method
                    paragraph.set_Style(ref prop1);
                    paragraph.OutlineLevel = WdOutlineLevel.wdOutlineLevel2;
                    --num;
                    break;
                  case WdOutlineLevel.wdOutlineLevel4:
                    if (this.catIndex3 != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child[this.catIndex2 - 1].Child[this.catIndex3 - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      this.catIndex4 = this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes.Count;
                      this.catIndex5 = 0;
                      this.catIndex6 = 0;
                      this.catIndex7 = 0;
                      this.catIndex8 = 0;
                      ++this.itter;
                      level = "4";
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}.{this.catIndex3}.{this.catIndex4}";
                      break;
                    }
                    object prop2 = (object) WdBuiltinStyle.wdStyleHeading3;
                    // ISSUE: reference to a compiler-generated method
                    paragraph.set_Style(ref prop2);
                    paragraph.OutlineLevel = WdOutlineLevel.wdOutlineLevel3;
                    --num;
                    break;
                  case WdOutlineLevel.wdOutlineLevel5:
                    if (this.catIndex4 != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child[this.catIndex2 - 1].Child[this.catIndex3 - 1].Child[this.catIndex4 - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      this.catIndex5 = this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes.Count;
                      this.catIndex6 = 0;
                      this.catIndex7 = 0;
                      this.catIndex8 = 0;
                      ++this.itter;
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}.{this.catIndex3}.{this.catIndex4}.{this.catIndex5}";
                      level = "5";
                      break;
                    }
                    object prop3 = (object) WdBuiltinStyle.wdStyleHeading4;
                    // ISSUE: reference to a compiler-generated method
                    paragraph.set_Style(ref prop3);
                    paragraph.OutlineLevel = WdOutlineLevel.wdOutlineLevel4;
                    --num;
                    break;
                  case WdOutlineLevel.wdOutlineLevel6:
                    if (this.catIndex5 != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child[this.catIndex2 - 1].Child[this.catIndex3 - 1].Child[this.catIndex4 - 1].Child[this.catIndex5 - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      this.catIndex6 = this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes.Count;
                      this.catIndex7 = 0;
                      this.catIndex8 = 0;
                      ++this.itter;
                      level = "6";
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}.{this.catIndex3}.{this.catIndex4}.{this.catIndex5}.{this.catIndex6}";
                      break;
                    }
                    object prop4 = (object) WdBuiltinStyle.wdStyleHeading5;
                    // ISSUE: reference to a compiler-generated method
                    paragraph.set_Style(ref prop4);
                    paragraph.OutlineLevel = WdOutlineLevel.wdOutlineLevel5;
                    --num;
                    break;
                  case WdOutlineLevel.wdOutlineLevel7:
                    if (this.catIndex6 != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child[this.catIndex2 - 1].Child[this.catIndex3 - 1].Child[this.catIndex4 - 1].Child[this.catIndex5 - 1].Child[this.catIndex6 - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes[this.catIndex6 - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      this.catIndex7 = this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes.Count;
                      this.catIndex8 = 0;
                      ++this.itter;
                      level = "7";
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}.{this.catIndex3}.{this.catIndex4}.{this.catIndex5}.{this.catIndex6}.{this.catIndex7}";
                      break;
                    }
                    object prop5 = (object) WdBuiltinStyle.wdStyleHeading6;
                    // ISSUE: reference to a compiler-generated method
                    paragraph.set_Style(ref prop5);
                    paragraph.OutlineLevel = WdOutlineLevel.wdOutlineLevel6;
                    --num;
                    break;
                  case WdOutlineLevel.wdOutlineLevel8:
                    if (this.catIndex7 != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child[this.catIndex2 - 1].Child[this.catIndex3 - 1].Child[this.catIndex4 - 1].Child[this.catIndex5 - 1].Child[this.catIndex6 - 1].Child[this.catIndex7 - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes[this.catIndex6 - 1].Nodes[this.catIndex7 - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      this.catIndex8 = this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes[this.catIndex6 - 1].Nodes[this.catIndex7 - 1].Nodes.Count;
                      ++this.itter;
                      level = "8";
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}.{this.catIndex3}.{this.catIndex4}.{this.catIndex5}.{this.catIndex6}.{this.catIndex7}.{this.catIndex8}";
                      break;
                    }
                    object prop6 = (object) WdBuiltinStyle.wdStyleHeading7;
                    // ISSUE: reference to a compiler-generated method
                    paragraph.set_Style(ref prop6);
                    paragraph.OutlineLevel = WdOutlineLevel.wdOutlineLevel7;
                    --num;
                    break;
                  case WdOutlineLevel.wdOutlineLevel9:
                    if (this.catIndex8 != 0)
                    {
                      if (this._newTreeFromWords[this.catIndex - 1].Child[this.catIndex2 - 1].Child[this.catIndex3 - 1].Child[this.catIndex4 - 1].Child[this.catIndex5 - 1].Child[this.catIndex6 - 1].Child[this.catIndex7 - 1].Child[this.catIndex8 - 1].Child.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                        throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
                      this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes[this.catIndex6 - 1].Nodes[this.catIndex7 - 1].Nodes[this.catIndex8 - 1].Nodes.Add(rangeText, rangeText).Checked = true;
                      ++this.itter;
                      levelHierarhi = $"{this.catIndex}.{this.catIndex2}.{this.catIndex3}.{this.catIndex4}.{this.catIndex5}.{this.catIndex6}.{this.catIndex7}.{this.catIndex8}.{this.treeView1.Nodes[this.catIndex - 1].Nodes[this.catIndex2 - 1].Nodes[this.catIndex3 - 1].Nodes[this.catIndex4 - 1].Nodes[this.catIndex5 - 1].Nodes[this.catIndex6 - 1].Nodes[this.catIndex7 - 1].Nodes[this.catIndex8 - 1].Nodes.Count}";
                      level = "9";
                      break;
                    }
                    object prop7 = (object) WdBuiltinStyle.wdStyleHeading8;
                    // ISSUE: reference to a compiler-generated method
                    paragraph.set_Style(ref prop7);
                    paragraph.OutlineLevel = WdOutlineLevel.wdOutlineLevel8;
                    --num;
                    break;
                }
                indexInDocument = range.ListFormat.ListValue != 0 ? range.ListFormat.ListString : string.Empty;
              }
            }
            else
            {
              if (this._newTreeFromWords.FindAll((Predicate<NodeTreeFromWord>) (x => x.Name.Equals(rangeText))).Count > 0)
                throw new KernelException($"Пункт '{rangeText}' повторяется. На одном уровне вложенностей не может повторяться название пункта. Продолжение невозможно, исправьте техническое задание и повторите операцию.");
              if (!string.IsNullOrEmpty(rangeText) || !string.IsNullOrWhiteSpace(rangeText))
                this.treeView1.Nodes.Add(rangeText, rangeText).Checked = true;
              if (this.itter == 0)
              {
                key = rangeText;
                level = "1";
                indexInDocument = range.ListFormat.ListValue != 0 ? range.ListFormat.ListString : string.Empty;
                levelHierarhi = $"{this.treeView1.Nodes.Count}";
              }
              else
              {
                this.AddParagraphToWordList(level, key, sb, levelHierarhi, indexInDocument);
                indexInDocument = range.ListFormat.ListValue != 0 ? range.ListFormat.ListString : string.Empty;
                key = rangeText;
                sb.Clear();
                level = "1";
                levelHierarhi = $"{this.treeView1.Nodes.Count}";
              }
              this.catIndex = this.treeView1.Nodes.Count;
              this.catIndex2 = 0;
              this.catIndex3 = 0;
              this.catIndex4 = 0;
              this.catIndex5 = 0;
              this.catIndex6 = 0;
              this.catIndex7 = 0;
              this.catIndex8 = 0;
              ++this.itter;
            }
          }
          else if (!string.IsNullOrEmpty(key))
            sb.Append(range.Text);
        }
        catch (Exception ex)
        {
          throw new KernelException(ex.Message);
        }
label_68:
        ++num;
      }
      else
        break;
    }
    this.AddParagraphToWordList(level, key, sb, levelHierarhi, indexInDocument);
  }

  private void AddParagraphToWordList(
    string level,
    string key,
    StringBuilder sb,
    string levelHierarhi,
    string indexInDocument)
  {
    string[] strArray = levelHierarhi.Split(new string[1]
    {
      "."
    }, StringSplitOptions.RemoveEmptyEntries);
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(level))
    {
      case 806133968:
        if (!(level == "5"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1]
        });
        break;
      case 822911587:
        if (!(level == "4"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1]
        });
        break;
      case 839689206:
        if (!(level == "7"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1].Child[Convert.ToInt32(strArray[5]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1].Child[Convert.ToInt32(strArray[5]) - 1]
        });
        break;
      case 856466825:
        if (!(level == "6"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1]
        });
        break;
      case 873244444:
        if (!(level == "1"))
          break;
        this._newTreeFromWords.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument
        });
        break;
      case 906799682:
        if (!(level == "3"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1]
        });
        break;
      case 923577301:
        if (!(level == "2"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1]
        });
        break;
      case 1007465396:
        if (!(level == "9"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1].Child[Convert.ToInt32(strArray[5]) - 1].Child[Convert.ToInt32(strArray[6]) - 1].Child[Convert.ToInt32(strArray[7]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1].Child[Convert.ToInt32(strArray[5]) - 1].Child[Convert.ToInt32(strArray[6]) - 1].Child[Convert.ToInt32(strArray[7]) - 1]
        });
        break;
      case 1024243015:
        if (!(level == "8"))
          break;
        this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1].Child[Convert.ToInt32(strArray[5]) - 1].Child[Convert.ToInt32(strArray[6]) - 1].Child.Add(new NodeTreeFromWord()
        {
          Name = key,
          TTDescription = sb.ToString(),
          TTLevel = level,
          TTLevelHierarhi = levelHierarhi,
          Child = new List<NodeTreeFromWord>(),
          TTIndexInDocument = indexInDocument,
          Parent = this._newTreeFromWords[Convert.ToInt32(strArray[0]) - 1].Child[Convert.ToInt32(strArray[1]) - 1].Child[Convert.ToInt32(strArray[2]) - 1].Child[Convert.ToInt32(strArray[3]) - 1].Child[Convert.ToInt32(strArray[4]) - 1].Child[Convert.ToInt32(strArray[5]) - 1].Child[Convert.ToInt32(strArray[6]) - 1]
        });
        break;
    }
  }

  private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
  {
    DocumentTextSearch documentTextSearch = new DocumentTextSearch(this.radRichTextEditor1.Document);
    List<Telerik.WinForms.Documents.Model.Paragraph> list = this.radRichTextEditor1.Document.EnumerateChildrenOfType<Telerik.WinForms.Documents.Model.Paragraph>().Where<Telerik.WinForms.Documents.Model.Paragraph>((Func<Telerik.WinForms.Documents.Model.Paragraph, bool>) (x => x.Children.Count<DocumentElement>((Func<DocumentElement, bool>) (y => y is BookmarkRangeStart)) > 0 && x.Children.Count<DocumentElement>((Func<DocumentElement, bool>) (y => y is HyperlinkRangeStart)) == 0)).ToList<Telerik.WinForms.Documents.Model.Paragraph>();
    bool flag = false;
    string regex = "\\b" + Regex.Escape(Regex.Replace(e.Node.Text, "\\p{C}+", string.Empty));
    foreach (TextRange textRange in documentTextSearch.FindAll(regex))
    {
      this.radRichTextEditor1.Document.CaretPosition.SetPosition(textRange.StartPosition.Location);
      Telerik.WinForms.Documents.Model.Paragraph currentParagraph = this.radRichTextEditor1.Document.CaretPosition.GetCurrentParagraph();
      foreach (Telerik.WinForms.Documents.Model.Paragraph paragraph in list)
      {
        if (currentParagraph == paragraph)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        break;
    }
    DocumentPosition caretPosition = this.radRichTextEditor1.Document.CaretPosition;
    DocumentPosition position = new DocumentPosition(caretPosition);
    position.MoveToCurrentLineEnd();
    this.radRichTextEditor1.Document.Selection.AddSelectionStart(caretPosition);
    this.radRichTextEditor1.Document.Selection.AddSelectionEnd(position);
  }

  private void btnCollapseAll_Click(object sender, EventArgs e) => this.treeView1.CollapseAll();

  private void btnExpandAll_Click(object sender, EventArgs e) => this.treeView1.ExpandAll();

  private static void CheckedChildNodes(TreeNodeCollection node)
  {
    foreach (TreeNode treeNode in node)
    {
      treeNode.Checked = true;
      if (treeNode.Nodes.Count > 0)
        RequirementCreatedForm.CheckedChildNodes(treeNode.Nodes);
    }
  }

  private static void CanсelCheckedChildNodes(TreeNodeCollection node)
  {
    foreach (TreeNode treeNode in node)
    {
      treeNode.Checked = false;
      if (treeNode.Nodes.Count > 0)
        RequirementCreatedForm.CanсelCheckedChildNodes(treeNode.Nodes);
    }
  }

  private void btnUnCheked_Click(object sender, EventArgs e)
  {
    RequirementCreatedForm.CanсelCheckedChildNodes(this.treeView1.Nodes);
  }

  private void btnCheked_Click(object sender, EventArgs e)
  {
    RequirementCreatedForm.CheckedChildNodes(this.treeView1.Nodes);
  }

  private void btnGetTree_Click(object sender, EventArgs e)
  {
    this.isCancel = false;
    RequirementConst.CheckFormResult = true;
    if (!RequirementConst.IsHaveCompisition)
    {
      RequirementConst.WordTT = this.RemoveUnCkekedItemsFromWordTTList(this.treeView1.Nodes, this._newTreeFromWords);
    }
    else
    {
      RequirementConst.NodesList = this.RemoveUnCkekedItemsFromWordTTList(this.treeView1.Nodes, this._newTreeFromWords);
      RequirementConst.DeletedObject = this.ResolveCheckedItem(this.treeView1.Nodes, this._deletedObjectRelationWithID);
    }
    if (this._deletedObjectRelationWithID.Count > 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("\n");
      foreach (DeletedObjectRelationWithID objectRelationWithId in this._deletedObjectRelationWithID)
      {
        stringBuilder.Append(objectRelationWithId.DeletedTTName);
        stringBuilder.Append("\n");
      }
      if (MessageBox.Show($"Внимание данные элементы будут удалены: {stringBuilder}", "Внимание", MessageBoxButtons.OKCancel) == DialogResult.OK)
        this.Close();
      else
        this.isCancel = true;
    }
    else
      this.Close();
  }

  private List<NodeTreeFromWord> RemoveUnCkekedItemsFromWordTTList(
    TreeNodeCollection nodes,
    List<NodeTreeFromWord> ttDict)
  {
    for (int index = 0; index < ttDict.Count; ++index)
    {
      if (ttDict[index].OldNode != null)
      {
        if (!nodes[index].Checked)
        {
          ttDict[index].IsDeleted = true;
          this._deletedObjectRelationWithID.Add(new DeletedObjectRelationWithID()
          {
            DeletedTTName = ttDict[index].OldNode.Name,
            DeletedTTID = ttDict[index].OldNode.TTObjectID
          });
        }
      }
      else
        ttDict[index].IsChecked = nodes[index].Checked;
      if (ttDict[index].IsHaveChild)
        ttDict[index].Child = this.RemoveUnCkekedItemsFromWordTTList(nodes[index].Nodes, ttDict[index].Child);
    }
    return ttDict;
  }

  private List<DeletedObjectRelationWithID> ResolveCheckedItem(
    TreeNodeCollection node,
    List<DeletedObjectRelationWithID> dict,
    string objID = "",
    string name = "")
  {
    if (string.IsNullOrEmpty(objID))
    {
      foreach (DeletedNodes deletedNodes in this._deletedNodesName)
      {
        NodeTreeFromWord items = this._nodeTreeFromWords[deletedNodes.ParentIndex].Find(deletedNodes.DeletedTTName, deletedNodes.IndexEntry);
        if (items != null)
        {
          foreach (TreeNode treeNode in node)
          {
            if (treeNode.Checked && treeNode.Name.Equals(items.Name) && treeNode.Parent != null && treeNode.Parent.Name.Equals(items.ParentName))
            {
              DeletedObjectRelationWithID objectRelationWithId = dict.Find((Predicate<DeletedObjectRelationWithID>) (x => x.DeletedTTID.Equals(items.TTObjectID)));
              dict.Remove(objectRelationWithId);
            }
            if (treeNode.Nodes.Count > 0)
              this.ResolveCheckedItem(treeNode.Nodes, dict, items.TTObjectID, items.Name);
          }
        }
      }
    }
    else
    {
      foreach (TreeNode treeNode in node)
      {
        if (treeNode.Checked && treeNode.Name.Equals(name))
        {
          DeletedObjectRelationWithID objectRelationWithId = dict.Find((Predicate<DeletedObjectRelationWithID>) (x => x.DeletedTTID.Equals(objID)));
          dict.Remove(objectRelationWithId);
        }
        if (treeNode.Nodes.Count > 0)
          this.ResolveCheckedItem(treeNode.Nodes, dict, objID, name);
      }
    }
    return dict;
  }

  private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
  {
    if (e.Action == TreeViewAction.Unknown)
      return;
    if (e.Node.Nodes.Count > 0)
      RequirementCreatedForm.CheckAllChildNodes(e.Node, e.Node.Checked);
    if (!e.Node.Checked || e.Node.Parent == null)
      return;
    e.Node.Parent.Checked = true;
    this.CheckAllParent(e.Node.Parent);
  }

  private void CheckAllParent(TreeNode node)
  {
    if (node.Parent == null)
      return;
    node.Parent.Checked = true;
    this.CheckAllParent(node.Parent);
  }

  private static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
  {
    foreach (TreeNode node in treeNode.Nodes)
    {
      node.Checked = nodeChecked;
      if (node.Nodes.Count > 0)
        RequirementCreatedForm.CheckAllChildNodes(node, nodeChecked);
    }
  }

  private void richTextBox1_SelectionChanged(object sender, EventArgs e)
  {
    string str = Regex.Replace(this.radRichTextEditor1.Document.Selection.GetSelectedText(), "\\p{C}+", string.Empty);
    if (string.IsNullOrEmpty(str) || str.Length < 4)
    {
      this.setsFlagsOnSelectToolStripMenuItem.Enabled = false;
      this.unsetsFlagsOnSelectToolStripMenuItem.Enabled = false;
    }
    else
    {
      this.setsFlagsOnSelectToolStripMenuItem.Enabled = true;
      this.unsetsFlagsOnSelectToolStripMenuItem.Enabled = true;
    }
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    RequirementConst.CheckFormResult = false;
    this.Close();
  }

  private void setsFlagsOnSelectToolStripMenuItem_Click(object sender, EventArgs e)
  {
    List<string> spansNameList = this.GetSpansNameList();
    RequirementCreatedForm.CanсelCheckedChildNodes(this.treeView1.Nodes);
    for (int index = 0; index < spansNameList.Count; ++index)
    {
      string str = Regex.Replace(spansNameList[index], "\\p{C}+", string.Empty);
      if (this._newTreeFromWords.AnyInList(str))
        this.CheckNodesInTree(str, true);
    }
  }

  private void unsetsFlagsOnSelectToolStripMenuItem_Click(object sender, EventArgs e)
  {
    List<string> spansNameList = this.GetSpansNameList();
    for (int index = 0; index < spansNameList.Count; ++index)
    {
      string str = Regex.Replace(spansNameList[index], "\\p{C}+", string.Empty);
      if (this._newTreeFromWords.AnyInList(str))
        this.CheckNodesInTree(str, false);
    }
  }

  private List<string> GetSpansNameList()
  {
    List<Telerik.WinForms.Documents.Model.Paragraph> list = this.radRichTextEditor1.Document.Selection.GetSelectedParagraphs().Where<Telerik.WinForms.Documents.Model.Paragraph>((Func<Telerik.WinForms.Documents.Model.Paragraph, bool>) (x => x.Children.Count<DocumentElement>((Func<DocumentElement, bool>) (y => y is BookmarkRangeStart)) > 0 && x.Children.Count<DocumentElement>((Func<DocumentElement, bool>) (y => y is HyperlinkRangeStart)) == 0)).ToList<Telerik.WinForms.Documents.Model.Paragraph>();
    List<string> spansNameList = new List<string>();
    foreach (DocumentElement documentElement in list)
    {
      foreach (Span span in documentElement.EnumerateChildrenOfType<Span>())
      {
        string str = span.Text.Trim();
        if (!string.IsNullOrEmpty(str))
          spansNameList.Add(str);
      }
    }
    return spansNameList;
  }

  private void CheckNodesInTree(string chekedItem, bool check)
  {
    foreach (TreeNode node in this.treeView1.Nodes)
    {
      if (node.Name == chekedItem)
      {
        node.Checked = check;
        this.treeView1_AfterCheck((object) null, new TreeViewEventArgs(node, TreeViewAction.ByMouse));
        break;
      }
      if (node.Nodes.Count > 0)
        this.CkeckInChildNodes(node.Nodes, chekedItem, check);
    }
  }

  private void CkeckInChildNodes(TreeNodeCollection node, string chekedItem, bool check)
  {
    foreach (TreeNode node1 in node)
    {
      if (node1.Name == chekedItem)
      {
        node1.Checked = check;
        this.treeView1_AfterCheck((object) null, new TreeViewEventArgs(node1, TreeViewAction.ByMouse));
        break;
      }
      if (node1.Nodes.Count > 0)
        this.CkeckInChildNodes(node1.Nodes, chekedItem, check);
    }
  }

  private void RequirementCreatedForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (RequirementConst.CheckFormResult && this.isCancel)
      e.Cancel = true;
    else
      e.Cancel = false;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.contextMenuStrip2 = new ContextMenuStrip(this.components);
    this.setsFlagsOnSelectToolStripMenuItem = new ToolStripMenuItem();
    this.unsetsFlagsOnSelectToolStripMenuItem = new ToolStripMenuItem();
    this.btnGetTree = new Button();
    this.btnCancel = new Button();
    this.splitContainer1 = new SplitContainer();
    this.treeView1 = new TreeViewOverrideDblClick();
    this.contextMenuStrip1 = new ContextMenuStrip(this.components);
    this.свернутьВсёToolStripMenuItem = new ToolStripMenuItem();
    this.развернутьВсёToolStripMenuItem = new ToolStripMenuItem();
    this.radRichTextEditor1 = new RadRichTextEditor();
    this.imgStatusList = new ImageList(this.components);
    this.toolTip1 = new ToolTip(this.components);
    this.btnUnCheked = new Button();
    this.btnCheked = new Button();
    this.label1 = new Label();
    this.label4 = new Label();
    this.label5 = new Label();
    this.groupBox1 = new GroupBox();
    this.label6 = new Label();
    this.label3 = new Label();
    this.label2 = new Label();
    this.windows8Theme1 = new Windows8Theme();
    this.contextMenuStrip2.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.contextMenuStrip1.SuspendLayout();
    this.radRichTextEditor1.BeginInit();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.contextMenuStrip2.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.setsFlagsOnSelectToolStripMenuItem,
      (ToolStripItem) this.unsetsFlagsOnSelectToolStripMenuItem
    });
    this.contextMenuStrip2.Name = "contextMenuStrip2";
    this.contextMenuStrip2.Size = new Size(381, 48 /*0x30*/);
    this.setsFlagsOnSelectToolStripMenuItem.Name = "setsFlagsOnSelectToolStripMenuItem";
    this.setsFlagsOnSelectToolStripMenuItem.Size = new Size(380, 22);
    this.setsFlagsOnSelectToolStripMenuItem.Text = "Установить флажки на основе выделенного фрагмента";
    this.setsFlagsOnSelectToolStripMenuItem.Click += new EventHandler(this.setsFlagsOnSelectToolStripMenuItem_Click);
    this.unsetsFlagsOnSelectToolStripMenuItem.Name = "unsetsFlagsOnSelectToolStripMenuItem";
    this.unsetsFlagsOnSelectToolStripMenuItem.Size = new Size(380, 22);
    this.unsetsFlagsOnSelectToolStripMenuItem.Text = "Снять флажки у выделенного фрагмента";
    this.unsetsFlagsOnSelectToolStripMenuItem.Click += new EventHandler(this.unsetsFlagsOnSelectToolStripMenuItem_Click);
    this.btnGetTree.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnGetTree.DialogResult = DialogResult.OK;
    this.btnGetTree.Location = new Point(639, 422);
    this.btnGetTree.Name = "btnGetTree";
    this.btnGetTree.Size = new Size(99, 23);
    this.btnGetTree.TabIndex = 5;
    this.btnGetTree.Text = "ОК";
    this.btnGetTree.UseVisualStyleBackColor = true;
    this.btnGetTree.Click += new EventHandler(this.btnGetTree_Click);
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(639, 451);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(99, 23);
    this.btnCancel.TabIndex = 6;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.splitContainer1.Location = new Point(5, 12);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.treeView1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.radRichTextEditor1);
    this.splitContainer1.Size = new Size(628, 468);
    this.splitContainer1.SplitterDistance = 205;
    this.splitContainer1.SplitterWidth = 6;
    this.splitContainer1.TabIndex = 9;
    this.treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.treeView1.CheckBoxes = true;
    this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
    this.treeView1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.treeView1.Indent = 22;
    this.treeView1.ItemHeight = 18;
    this.treeView1.Location = new Point(0, 0);
    this.treeView1.Name = "treeView1";
    this.treeView1.Size = new Size(205, 468);
    this.treeView1.TabIndex = 1;
    this.treeView1.AfterCheck += new TreeViewEventHandler(this.treeView1_AfterCheck);
    this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
    this.contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.свернутьВсёToolStripMenuItem,
      (ToolStripItem) this.развернутьВсёToolStripMenuItem
    });
    this.contextMenuStrip1.Name = "contextMenuStrip1";
    this.contextMenuStrip1.Size = new Size(157, 48 /*0x30*/);
    this.свернутьВсёToolStripMenuItem.Name = "свернутьВсёToolStripMenuItem";
    this.свернутьВсёToolStripMenuItem.Size = new Size(156, 22);
    this.свернутьВсёToolStripMenuItem.Text = "Свернуть всё";
    this.свернутьВсёToolStripMenuItem.Click += new EventHandler(this.btnCollapseAll_Click);
    this.развернутьВсёToolStripMenuItem.Name = "развернутьВсёToolStripMenuItem";
    this.развернутьВсёToolStripMenuItem.Size = new Size(156, 22);
    this.развернутьВсёToolStripMenuItem.Text = "Развернуть всё";
    this.развернутьВсёToolStripMenuItem.Click += new EventHandler(this.btnExpandAll_Click);
    this.radRichTextEditor1.BorderColor = Color.FromArgb(172, 172, 172);
    this.radRichTextEditor1.ContextMenuStrip = this.contextMenuStrip2;
    this.radRichTextEditor1.Dock = DockStyle.Fill;
    this.radRichTextEditor1.IsContextMenuEnabled = false;
    this.radRichTextEditor1.IsReadOnly = true;
    this.radRichTextEditor1.IsSelectionMiniToolBarEnabled = false;
    this.radRichTextEditor1.Location = new Point(0, 0);
    this.radRichTextEditor1.Name = "radRichTextEditor1";
    this.radRichTextEditor1.SelectionFill = Color.FromArgb(128 /*0x80*/, 88, 163, (int) byte.MaxValue);
    this.radRichTextEditor1.Size = new Size(417, 468);
    this.radRichTextEditor1.TabIndex = 3;
    this.radRichTextEditor1.ThemeName = "Windows8";
    this.radRichTextEditor1.SelectionChanged += new EventHandler(this.richTextBox1_SelectionChanged);
    this.imgStatusList.ColorDepth = ColorDepth.Depth16Bit;
    this.imgStatusList.ImageSize = new Size(16 /*0x10*/, 16 /*0x10*/);
    this.imgStatusList.TransparentColor = Color.Transparent;
    this.btnUnCheked.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btnUnCheked.BackColor = Color.Transparent;
    this.btnUnCheked.BackgroundImage = (Image) Resources.uncheck;
    this.btnUnCheked.BackgroundImageLayout = ImageLayout.Center;
    this.btnUnCheked.FlatAppearance.BorderSize = 0;
    this.btnUnCheked.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
    this.btnUnCheked.FlatAppearance.MouseOverBackColor = SystemColors.MenuHighlight;
    this.btnUnCheked.FlatStyle = FlatStyle.Flat;
    this.btnUnCheked.Location = new Point(666, 12);
    this.btnUnCheked.Name = "btnUnCheked";
    this.btnUnCheked.Size = new Size(17, 17);
    this.btnUnCheked.TabIndex = 3;
    this.toolTip1.SetToolTip((Control) this.btnUnCheked, "Снять флажки");
    this.btnUnCheked.UseVisualStyleBackColor = false;
    this.btnUnCheked.Click += new EventHandler(this.btnUnCheked_Click);
    this.btnCheked.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btnCheked.BackgroundImage = (Image) Resources.check;
    this.btnCheked.BackgroundImageLayout = ImageLayout.Center;
    this.btnCheked.FlatAppearance.BorderSize = 0;
    this.btnCheked.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
    this.btnCheked.FlatAppearance.MouseOverBackColor = SystemColors.MenuHighlight;
    this.btnCheked.FlatStyle = FlatStyle.Flat;
    this.btnCheked.Location = new Point(695, 12);
    this.btnCheked.Name = "btnCheked";
    this.btnCheked.Size = new Size(17, 17);
    this.btnCheked.TabIndex = 4;
    this.toolTip1.SetToolTip((Control) this.btnCheked, "Поставить флажки");
    this.btnCheked.UseVisualStyleBackColor = true;
    this.btnCheked.Click += new EventHandler(this.btnCheked_Click);
    this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label1.AutoSize = true;
    this.label1.BackColor = Color.Orange;
    this.label1.Location = new Point(9, 21);
    this.label1.Name = "label1";
    this.label1.Size = new Size(25, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "      ";
    this.toolTip1.SetToolTip((Control) this.label1, "Пункт изменён");
    this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label4.AutoSize = true;
    this.label4.BackColor = Color.Red;
    this.label4.Location = new Point(9, 46);
    this.label4.Name = "label4";
    this.label4.Size = new Size(25, 13);
    this.label4.TabIndex = 0;
    this.label4.Text = "      \r\n";
    this.toolTip1.SetToolTip((Control) this.label4, "Пункт удалён");
    this.label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label5.AutoSize = true;
    this.label5.BackColor = Color.LightSteelBlue;
    this.label5.Location = new Point(9, 71);
    this.label5.Name = "label5";
    this.label5.Size = new Size(25, 13);
    this.label5.TabIndex = 0;
    this.label5.Text = "      \r\n";
    this.toolTip1.SetToolTip((Control) this.label5, "Новый пункт");
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.groupBox1.Controls.Add((Control) this.label6);
    this.groupBox1.Controls.Add((Control) this.label3);
    this.groupBox1.Controls.Add((Control) this.label2);
    this.groupBox1.Controls.Add((Control) this.label5);
    this.groupBox1.Controls.Add((Control) this.label4);
    this.groupBox1.Controls.Add((Control) this.label1);
    this.groupBox1.Location = new Point(639, 35);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(99, 97);
    this.groupBox1.TabIndex = 10;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Легенда";
    this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(42, 71);
    this.label6.Name = "label6";
    this.label6.Size = new Size(41, 13);
    this.label6.TabIndex = 1;
    this.label6.Text = "Новый";
    this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label3.AutoSize = true;
    this.label3.Location = new Point(42, 46);
    this.label3.Name = "label3";
    this.label3.Size = new Size(45, 13);
    this.label3.TabIndex = 1;
    this.label3.Text = "Удален";
    this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.label2.AutoSize = true;
    this.label2.Location = new Point(42, 21);
    this.label2.Name = "label2";
    this.label2.Size = new Size(53, 13);
    this.label2.TabIndex = 1;
    this.label2.Text = "Изменен";
    this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(750, 486);
    this.Controls.Add((Control) this.groupBox1);
    this.Controls.Add((Control) this.btnUnCheked);
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnGetTree);
    this.Controls.Add((Control) this.btnCheked);
    this.MinimumSize = new Size(640, 480);
    this.Name = nameof (RequirementCreatedForm);
    this.Text = "Создание объектов технического задания";
    this.FormClosing += new FormClosingEventHandler(this.RequirementCreatedForm_FormClosing);
    this.Load += new EventHandler(this.RequirementCreatedForm_Load);
    this.contextMenuStrip2.ResumeLayout(false);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.contextMenuStrip1.ResumeLayout(false);
    this.radRichTextEditor1.EndInit();
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.ResumeLayout(false);
  }
}
