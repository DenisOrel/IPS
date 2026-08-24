// Decompiled with JetBrains decompiler
// Type: InventorApprentice.RigidBodyJointTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("3F5D0302-506A-4CA7-9A1C-0BBFBC7684AA")]
[TypeLibType(16 /*0x10*/)]
public enum RigidBodyJointTypeEnum
{
  kAngleVectorVectorJoint = 68097, // 0x00010A01
  kMatePointPointJoint = 68098, // 0x00010A02
  kMatePointLineJoint = 68099, // 0x00010A03
  kMateLineLineJoint = 68100, // 0x00010A04
  kMateCylinderCylinderJoint = 68101, // 0x00010A05
  kMateSphereSphereJoint = 68102, // 0x00010A06
  kMateSphereConeJoint = 68103, // 0x00010A07
  kMateConeConeJoint = 68104, // 0x00010A08
  kMatePointCurveJoint = 68105, // 0x00010A09
  kMatePointCircleJoint = 68106, // 0x00010A0A
  kMatePointSurfaceJoint = 68107, // 0x00010A0B
  kMateCurveCurveJoint = 68108, // 0x00010A0C
  kConCentricCircleCircleJoint = 68109, // 0x00010A0D
  kDistancePointPointJoint = 68110, // 0x00010A0E
  kDistancePointLineJoint = 68111, // 0x00010A0F
  kDistancePointPlaneJoint = 68112, // 0x00010A10
  kDistanceLineLineJoint = 68113, // 0x00010A11
  kDistanceLinePlaneJoint = 68114, // 0x00010A12
  kDistancePlanePlaneJoint = 68115, // 0x00010A13
  kEqualDistancePointPlaneJoint = 68116, // 0x00010A14
  kEqualDistancePlanePlaneJoint = 68117, // 0x00010A15
  kTangentPlaneCylinderJoint = 68118, // 0x00010A16
  kTangentCylinderCylinderJoint = 68119, // 0x00010A17
  kTangentCylinderSphereJoint = 68120, // 0x00010A18
  kTangentSphereSphereJoint = 68121, // 0x00010A19
  kTangentPlaneConeJoint = 68122, // 0x00010A1A
  kTangentCylinderConeJoint = 68123, // 0x00010A1B
  kTangentSphereConeJoint = 68124, // 0x00010A1C
  kTangentConeConeJoint = 68125, // 0x00010A1D
  kTangentPlaneCircleJoint = 68126, // 0x00010A1E
  kTangentCircleCylinderJoint = 68127, // 0x00010A1F
  kTangentLineCylinderJoint = 68128, // 0x00010A20
  kTangentCircleCircleJoint = 68129, // 0x00010A21
  kTangentLineSphereJoint = 68130, // 0x00010A22
  kTangentLineCircleJoint = 68131, // 0x00010A23
  kTangentCurveSurfaceJoint = 68132, // 0x00010A24
  kTangentSurfaceSurfaceJoint = 68133, // 0x00010A25
  kPerpendicularCurveSurfaceJoint = 68134, // 0x00010A26
  kTranslationalJoint = 68135, // 0x00010A27
  kRevoluteJoint = 68136, // 0x00010A28
  kTransitionalJoint = 68137, // 0x00010A29
  kGearJoint = 68138, // 0x00010A2A
  kWireframeWireframeJoint = 68139, // 0x00010A2B
  kWeldJoint = 68140, // 0x00010A2C
  kUniversalJoint = 68141, // 0x00010A2D
  kSymmetricPointPointPlaneJoint = 68142, // 0x00010A2E
  kSymmetricVectorVectorPlaneJoint = 68143, // 0x00010A2F
  kSymmetricPlanePlanePlaneJoint = 68144, // 0x00010A30
  kSymmetricLineLinePlaneJoint = 68145, // 0x00010A31
  kSymmetricPointPointLineJoint = 68146, // 0x00010A32
  kSymmetricLineLineLineJoint = 68147, // 0x00010A33
  kSymmetricCircleCircleLineJoint = 68148, // 0x00010A34
  kSymmetricEllipseEllipseLineJoint = 68149, // 0x00010A35
  kUnknownJoint = 68150, // 0x00010A36
}
