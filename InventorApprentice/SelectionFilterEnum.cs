// Decompiled with JetBrains decompiler
// Type: InventorApprentice.SelectionFilterEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("34D6CCBE-FC1A-4384-98DB-C4565D24391D")]
public enum SelectionFilterEnum
{
  kPartEdgeFilter = 15873, // 0x00003E01
  kPartEdgeCircularFilter = 15874, // 0x00003E02
  kPartEdgeLinearFilter = 15875, // 0x00003E03
  kPartEdgeMidpointFilter = 15876, // 0x00003E04
  kPartFaceFilter = 15877, // 0x00003E05
  kPartFacePlanarFilter = 15878, // 0x00003E06
  kPartFaceCylindricalFilter = 15879, // 0x00003E07
  kPartFaceConicalFilter = 15880, // 0x00003E08
  kPartFaceToroidalFilter = 15881, // 0x00003E09
  kPartFaceSphericalFilter = 15882, // 0x00003E0A
  kPartVertexFilter = 15883, // 0x00003E0B
  kPartFeatureFilter = 15884, // 0x00003E0C
  kPartSurfaceFeatureFilter = 15885, // 0x00003E0D
  kPartDefaultFilter = 15886, // 0x00003E0E
  kPartBodyFilter = 15890, // 0x00003E12
  kPointCloudFilter = 15891, // 0x00003E13
  kPointCloudPointFilter = 15892, // 0x00003E14
  kPointCloudPlaneFilter = 15893, // 0x00003E15
  kSketchDimConstraintFilter = 16128, // 0x00003F00
  kSketchCurveFilter = 16129, // 0x00003F01
  kSketchCurveLinearFilter = 16130, // 0x00003F02
  kSketchCurveCircularFilter = 16131, // 0x00003F03
  kSketchCurveEllipseFilter = 16132, // 0x00003F04
  kSketchCurveSplineFilter = 16133, // 0x00003F05
  kSketchPointFilter = 16134, // 0x00003F06
  kSketchDefaultFilter = 16135, // 0x00003F07
  kSketchObjectFilter = 16136, // 0x00003F08
  kSketchImageFilter = 16137, // 0x00003F09
  kSketchTextBoxFilter = 16138, // 0x00003F0A
  kSketchProfileFilter = 16139, // 0x00003F0B
  kSketchProjectedCutFilter = 16140, // 0x00003F0C
  kSketchBlockDefinitionFilter = 16141, // 0x00003F0D
  kSketchBlockFilter = 16142, // 0x00003F0E
  kWorkAxisFilter = 16384, // 0x00004000
  kWorkPlaneFilter = 16385, // 0x00004001
  kWorkPointFilter = 16386, // 0x00004002
  kUserCoordinateSystemFilter = 16387, // 0x00004003
  kAssemblyOccurrenceFilter = 16640, // 0x00004100
  kAssemblyLeafOccurrenceFilter = 16643, // 0x00004103
  kAssemblyOccurrencePatternFilter = 16644, // 0x00004104
  kAssemblyOccurrencePatternElementFilter = 16645, // 0x00004105
  kAssemblyFeatureFilter = 16646, // 0x00004106
  kDrawingDefaultFilter = 16896, // 0x00004200
  kDrawingSheetFilter = 16897, // 0x00004201
  kDrawingViewFilter = 16898, // 0x00004202
  kDrawingNoteFilter = 16899, // 0x00004203
  kDrawingDimensionFilter = 16900, // 0x00004204
  kDrawingPartsListFilter = 16901, // 0x00004205
  kDrawingHoleTableFilter = 16902, // 0x00004206
  kDrawingHoleTagFilter = 16903, // 0x00004207
  kDrawingRevisionTableFilter = 16904, // 0x00004208
  kDrawingCustomTableFilter = 16905, // 0x00004209
  kDrawingBalloonFilter = 16906, // 0x0000420A
  kDrawingSketchedSymbolFilter = 16907, // 0x0000420B
  kDrawingSketchedSymbolDefinitionFilter = 16908, // 0x0000420C
  kDrawingBorderDefinitionFilter = 16909, // 0x0000420D
  kDrawingTitleBlockDefinitionFilter = 16910, // 0x0000420E
  kDrawingTitleBlockFilter = 16912, // 0x00004210
  kDrawingBorderFilter = 16913, // 0x00004211
  kDrawingCurveSegmentFilter = 16914, // 0x00004212
  kDrawingCenterlineFilter = 16915, // 0x00004213
  kDrawingCentermarkFilter = 16916, // 0x00004214
  kDrawingSheetFormatFilter = 16917, // 0x00004215
  kDrawingFeatureControlFrameFilter = 16918, // 0x00004216
  kDrawingSurfaceTextureSymbolFilter = 16919, // 0x00004217
  kDrawingOriginIndicatorFilter = 16920, // 0x00004218
  kDrawingViewLabelFilter = 16921, // 0x00004219
  kDrawingAutoCADBlockFilter = 16922, // 0x0000421A
  kDrawingAutoCADBlockDefinitionFilter = 16923, // 0x0000421B
  kSketch3DCurveFilter = 17664, // 0x00004500
  kSketch3DCurveLinearFilter = 17665, // 0x00004501
  kSketch3DCurveCircularFilter = 17666, // 0x00004502
  kSketch3DCurveEllipseFilter = 17667, // 0x00004503
  kSketch3DCurveSplineFilter = 17668, // 0x00004504
  kSketch3DPointFilter = 17669, // 0x00004505
  kSketch3DDefaultFilter = 17670, // 0x00004506
  kSketch3DObjectFilter = 17671, // 0x00004507
  kSketch3DDimConstraintFilter = 17672, // 0x00004508
  kSketch3DProfileFilter = 17673, // 0x00004509
  kAllPlanarEntities = 18432, // 0x00004800
  kAllLinearEntities = 18433, // 0x00004801
  kAllPointEntities = 18434, // 0x00004802
  kAllCircularEntities = 18435, // 0x00004803
  kAllCustomGraphicsFilter = 18436, // 0x00004804
  kCustomBrowserNodeFilter = 18437, // 0x00004805
  kFeatureDimensionFilter = 18438, // 0x00004806
  kAllEntitiesFilter = 18439, // 0x00004807
}
