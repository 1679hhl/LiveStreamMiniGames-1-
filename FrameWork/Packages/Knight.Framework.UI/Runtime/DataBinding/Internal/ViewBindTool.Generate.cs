using System;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine.EventSystems;
using FancyScrollView;

/// <summary>
/// Auto generate code, not need modify.
/// </summary>
namespace UnityEngine.UI
{
    public static class ViewBindTool
    {
        public static void Set_Transform_position(object rComponent, Vector3 rposition)
        {
            ((Transform)rComponent).position = rposition;
        }
        public static void Set_Transform_localPosition(object rComponent, Vector3 rlocalPosition)
        {
            ((Transform)rComponent).localPosition = rlocalPosition;
        }
        public static void Set_Transform_eulerAngles(object rComponent, Vector3 reulerAngles)
        {
            ((Transform)rComponent).eulerAngles = reulerAngles;
        }
        public static void Set_Transform_localEulerAngles(object rComponent, Vector3 rlocalEulerAngles)
        {
            ((Transform)rComponent).localEulerAngles = rlocalEulerAngles;
        }
        public static void Set_Transform_right(object rComponent, Vector3 rright)
        {
            ((Transform)rComponent).right = rright;
        }
        public static void Set_Transform_up(object rComponent, Vector3 rup)
        {
            ((Transform)rComponent).up = rup;
        }
        public static void Set_Transform_forward(object rComponent, Vector3 rforward)
        {
            ((Transform)rComponent).forward = rforward;
        }
        public static void Set_Transform_localScale(object rComponent, Vector3 rlocalScale)
        {
            ((Transform)rComponent).localScale = rlocalScale;
        }
        public static void Set_Transform_hasChanged(object rComponent, Boolean rhasChanged)
        {
            ((Transform)rComponent).hasChanged = rhasChanged;
        }
        public static void Set_Transform_hierarchyCapacity(object rComponent, Int32 rhierarchyCapacity)
        {
            ((Transform)rComponent).hierarchyCapacity = rhierarchyCapacity;
        }
        public static void Set_Transform_tag(object rComponent, String rtag)
        {
            ((Transform)rComponent).tag = rtag;
        }
        public static void Set_Transform_name(object rComponent, String rname)
        {
            ((Transform)rComponent).name = rname;
        }
        public static void Set_Behaviour_enabled(object rComponent, Boolean renabled)
        {
            ((Behaviour)rComponent).enabled = renabled;
        }
        public static void Set_Behaviour_tag(object rComponent, String rtag)
        {
            ((Behaviour)rComponent).tag = rtag;
        }
        public static void Set_Behaviour_name(object rComponent, String rname)
        {
            ((Behaviour)rComponent).name = rname;
        }
        public static void Set_FlareLayer_enabled(object rComponent, Boolean renabled)
        {
            ((FlareLayer)rComponent).enabled = renabled;
        }
        public static void Set_FlareLayer_tag(object rComponent, String rtag)
        {
            ((FlareLayer)rComponent).tag = rtag;
        }
        public static void Set_FlareLayer_name(object rComponent, String rname)
        {
            ((FlareLayer)rComponent).name = rname;
        }
        public static void Set_Camera_nearClipPlane(object rComponent, Single rnearClipPlane)
        {
            ((Camera)rComponent).nearClipPlane = rnearClipPlane;
        }
        public static void Set_Camera_farClipPlane(object rComponent, Single rfarClipPlane)
        {
            ((Camera)rComponent).farClipPlane = rfarClipPlane;
        }
        public static void Set_Camera_fieldOfView(object rComponent, Single rfieldOfView)
        {
            ((Camera)rComponent).fieldOfView = rfieldOfView;
        }
        public static void Set_Camera_allowHDR(object rComponent, Boolean rallowHDR)
        {
            ((Camera)rComponent).allowHDR = rallowHDR;
        }
        public static void Set_Camera_allowMSAA(object rComponent, Boolean rallowMSAA)
        {
            ((Camera)rComponent).allowMSAA = rallowMSAA;
        }
        public static void Set_Camera_allowDynamicResolution(object rComponent, Boolean rallowDynamicResolution)
        {
            ((Camera)rComponent).allowDynamicResolution = rallowDynamicResolution;
        }
        public static void Set_Camera_forceIntoRenderTexture(object rComponent, Boolean rforceIntoRenderTexture)
        {
            ((Camera)rComponent).forceIntoRenderTexture = rforceIntoRenderTexture;
        }
        public static void Set_Camera_orthographicSize(object rComponent, Single rorthographicSize)
        {
            ((Camera)rComponent).orthographicSize = rorthographicSize;
        }
        public static void Set_Camera_orthographic(object rComponent, Boolean rorthographic)
        {
            ((Camera)rComponent).orthographic = rorthographic;
        }
        public static void Set_Camera_transparencySortAxis(object rComponent, Vector3 rtransparencySortAxis)
        {
            ((Camera)rComponent).transparencySortAxis = rtransparencySortAxis;
        }
        public static void Set_Camera_depth(object rComponent, Single rdepth)
        {
            ((Camera)rComponent).depth = rdepth;
        }
        public static void Set_Camera_aspect(object rComponent, Single raspect)
        {
            ((Camera)rComponent).aspect = raspect;
        }
        public static void Set_Camera_cullingMask(object rComponent, Int32 rcullingMask)
        {
            ((Camera)rComponent).cullingMask = rcullingMask;
        }
        public static void Set_Camera_eventMask(object rComponent, Int32 reventMask)
        {
            ((Camera)rComponent).eventMask = reventMask;
        }
        public static void Set_Camera_layerCullSpherical(object rComponent, Boolean rlayerCullSpherical)
        {
            ((Camera)rComponent).layerCullSpherical = rlayerCullSpherical;
        }
        public static void Set_Camera_useOcclusionCulling(object rComponent, Boolean ruseOcclusionCulling)
        {
            ((Camera)rComponent).useOcclusionCulling = ruseOcclusionCulling;
        }
        public static void Set_Camera_backgroundColor(object rComponent, Color rbackgroundColor)
        {
            ((Camera)rComponent).backgroundColor = rbackgroundColor;
        }
        public static void Set_Camera_clearStencilAfterLightingPass(object rComponent, Boolean rclearStencilAfterLightingPass)
        {
            ((Camera)rComponent).clearStencilAfterLightingPass = rclearStencilAfterLightingPass;
        }
        public static void Set_Camera_usePhysicalProperties(object rComponent, Boolean rusePhysicalProperties)
        {
            ((Camera)rComponent).usePhysicalProperties = rusePhysicalProperties;
        }
        public static void Set_Camera_sensorSize(object rComponent, Vector2 rsensorSize)
        {
            ((Camera)rComponent).sensorSize = rsensorSize;
        }
        public static void Set_Camera_lensShift(object rComponent, Vector2 rlensShift)
        {
            ((Camera)rComponent).lensShift = rlensShift;
        }
        public static void Set_Camera_focalLength(object rComponent, Single rfocalLength)
        {
            ((Camera)rComponent).focalLength = rfocalLength;
        }
        public static void Set_Camera_targetDisplay(object rComponent, Int32 rtargetDisplay)
        {
            ((Camera)rComponent).targetDisplay = rtargetDisplay;
        }
        public static void Set_Camera_useJitteredProjectionMatrixForTransparentRendering(object rComponent, Boolean ruseJitteredProjectionMatrixForTransparentRendering)
        {
            ((Camera)rComponent).useJitteredProjectionMatrixForTransparentRendering = ruseJitteredProjectionMatrixForTransparentRendering;
        }
        public static void Set_Camera_stereoSeparation(object rComponent, Single rstereoSeparation)
        {
            ((Camera)rComponent).stereoSeparation = rstereoSeparation;
        }
        public static void Set_Camera_stereoConvergence(object rComponent, Single rstereoConvergence)
        {
            ((Camera)rComponent).stereoConvergence = rstereoConvergence;
        }
        public static void Set_Camera_enabled(object rComponent, Boolean renabled)
        {
            ((Camera)rComponent).enabled = renabled;
        }
        public static void Set_Camera_tag(object rComponent, String rtag)
        {
            ((Camera)rComponent).tag = rtag;
        }
        public static void Set_Camera_name(object rComponent, String rname)
        {
            ((Camera)rComponent).name = rname;
        }
        public static void Set_Image_preserveAspect(object rComponent, Boolean rpreserveAspect)
        {
            ((Image)rComponent).preserveAspect = rpreserveAspect;
        }
        public static void Set_Image_fillCenter(object rComponent, Boolean rfillCenter)
        {
            ((Image)rComponent).fillCenter = rfillCenter;
        }
        public static void Set_Image_fillAmount(object rComponent, Single rfillAmount)
        {
            ((Image)rComponent).fillAmount = rfillAmount;
        }
        public static void Set_Image_fillClockwise(object rComponent, Boolean rfillClockwise)
        {
            ((Image)rComponent).fillClockwise = rfillClockwise;
        }
        public static void Set_Image_fillOrigin(object rComponent, Int32 rfillOrigin)
        {
            ((Image)rComponent).fillOrigin = rfillOrigin;
        }
        public static void Set_Image_alphaHitTestMinimumThreshold(object rComponent, Single ralphaHitTestMinimumThreshold)
        {
            ((Image)rComponent).alphaHitTestMinimumThreshold = ralphaHitTestMinimumThreshold;
        }
        public static void Set_Image_useSpriteMesh(object rComponent, Boolean ruseSpriteMesh)
        {
            ((Image)rComponent).useSpriteMesh = ruseSpriteMesh;
        }
        public static void Set_Image_pixelsPerUnitMultiplier(object rComponent, Single rpixelsPerUnitMultiplier)
        {
            ((Image)rComponent).pixelsPerUnitMultiplier = rpixelsPerUnitMultiplier;
        }
        public static void Set_Image_maskable(object rComponent, Boolean rmaskable)
        {
            ((Image)rComponent).maskable = rmaskable;
        }
        public static void Set_Image_isMaskingGraphic(object rComponent, Boolean risMaskingGraphic)
        {
            ((Image)rComponent).isMaskingGraphic = risMaskingGraphic;
        }
        public static void Set_Image_color(object rComponent, Color rcolor)
        {
            ((Image)rComponent).color = rcolor;
        }
        public static void Set_Image_raycastTarget(object rComponent, Boolean rraycastTarget)
        {
            ((Image)rComponent).raycastTarget = rraycastTarget;
        }
        public static void Set_Image_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Image)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Image_enabled(object rComponent, Boolean renabled)
        {
            ((Image)rComponent).enabled = renabled;
        }
        public static void Set_Image_tag(object rComponent, String rtag)
        {
            ((Image)rComponent).tag = rtag;
        }
        public static void Set_Image_name(object rComponent, String rname)
        {
            ((Image)rComponent).name = rname;
        }
        public static void Set_RectTransform_anchorMin(object rComponent, Vector2 ranchorMin)
        {
            ((RectTransform)rComponent).anchorMin = ranchorMin;
        }
        public static void Set_RectTransform_anchorMax(object rComponent, Vector2 ranchorMax)
        {
            ((RectTransform)rComponent).anchorMax = ranchorMax;
        }
        public static void Set_RectTransform_anchoredPosition(object rComponent, Vector2 ranchoredPosition)
        {
            ((RectTransform)rComponent).anchoredPosition = ranchoredPosition;
        }
        public static void Set_RectTransform_sizeDelta(object rComponent, Vector2 rsizeDelta)
        {
            ((RectTransform)rComponent).sizeDelta = rsizeDelta;
        }
        public static void Set_RectTransform_pivot(object rComponent, Vector2 rpivot)
        {
            ((RectTransform)rComponent).pivot = rpivot;
        }
        public static void Set_RectTransform_anchoredPosition3D(object rComponent, Vector3 ranchoredPosition3D)
        {
            ((RectTransform)rComponent).anchoredPosition3D = ranchoredPosition3D;
        }
        public static void Set_RectTransform_offsetMin(object rComponent, Vector2 roffsetMin)
        {
            ((RectTransform)rComponent).offsetMin = roffsetMin;
        }
        public static void Set_RectTransform_offsetMax(object rComponent, Vector2 roffsetMax)
        {
            ((RectTransform)rComponent).offsetMax = roffsetMax;
        }
        public static void Set_RectTransform_position(object rComponent, Vector3 rposition)
        {
            ((RectTransform)rComponent).position = rposition;
        }
        public static void Set_RectTransform_localPosition(object rComponent, Vector3 rlocalPosition)
        {
            ((RectTransform)rComponent).localPosition = rlocalPosition;
        }
        public static void Set_RectTransform_eulerAngles(object rComponent, Vector3 reulerAngles)
        {
            ((RectTransform)rComponent).eulerAngles = reulerAngles;
        }
        public static void Set_RectTransform_localEulerAngles(object rComponent, Vector3 rlocalEulerAngles)
        {
            ((RectTransform)rComponent).localEulerAngles = rlocalEulerAngles;
        }
        public static void Set_RectTransform_right(object rComponent, Vector3 rright)
        {
            ((RectTransform)rComponent).right = rright;
        }
        public static void Set_RectTransform_up(object rComponent, Vector3 rup)
        {
            ((RectTransform)rComponent).up = rup;
        }
        public static void Set_RectTransform_forward(object rComponent, Vector3 rforward)
        {
            ((RectTransform)rComponent).forward = rforward;
        }
        public static void Set_RectTransform_localScale(object rComponent, Vector3 rlocalScale)
        {
            ((RectTransform)rComponent).localScale = rlocalScale;
        }
        public static void Set_RectTransform_hasChanged(object rComponent, Boolean rhasChanged)
        {
            ((RectTransform)rComponent).hasChanged = rhasChanged;
        }
        public static void Set_RectTransform_hierarchyCapacity(object rComponent, Int32 rhierarchyCapacity)
        {
            ((RectTransform)rComponent).hierarchyCapacity = rhierarchyCapacity;
        }
        public static void Set_RectTransform_tag(object rComponent, String rtag)
        {
            ((RectTransform)rComponent).tag = rtag;
        }
        public static void Set_RectTransform_name(object rComponent, String rname)
        {
            ((RectTransform)rComponent).name = rname;
        }
        public static void Set_Text_text(object rComponent, String rtext)
        {
            ((Text)rComponent).text = rtext;
        }
        public static void Set_Text_supportRichText(object rComponent, Boolean rsupportRichText)
        {
            ((Text)rComponent).supportRichText = rsupportRichText;
        }
        public static void Set_Text_resizeTextForBestFit(object rComponent, Boolean rresizeTextForBestFit)
        {
            ((Text)rComponent).resizeTextForBestFit = rresizeTextForBestFit;
        }
        public static void Set_Text_resizeTextMinSize(object rComponent, Int32 rresizeTextMinSize)
        {
            ((Text)rComponent).resizeTextMinSize = rresizeTextMinSize;
        }
        public static void Set_Text_resizeTextMaxSize(object rComponent, Int32 rresizeTextMaxSize)
        {
            ((Text)rComponent).resizeTextMaxSize = rresizeTextMaxSize;
        }
        public static void Set_Text_alignByGeometry(object rComponent, Boolean ralignByGeometry)
        {
            ((Text)rComponent).alignByGeometry = ralignByGeometry;
        }
        public static void Set_Text_fontSize(object rComponent, Int32 rfontSize)
        {
            ((Text)rComponent).fontSize = rfontSize;
        }
        public static void Set_Text_lineSpacing(object rComponent, Single rlineSpacing)
        {
            ((Text)rComponent).lineSpacing = rlineSpacing;
        }
        public static void Set_Text_maskable(object rComponent, Boolean rmaskable)
        {
            ((Text)rComponent).maskable = rmaskable;
        }
        public static void Set_Text_isMaskingGraphic(object rComponent, Boolean risMaskingGraphic)
        {
            ((Text)rComponent).isMaskingGraphic = risMaskingGraphic;
        }
        public static void Set_Text_color(object rComponent, Color rcolor)
        {
            ((Text)rComponent).color = rcolor;
        }
        public static void Set_Text_raycastTarget(object rComponent, Boolean rraycastTarget)
        {
            ((Text)rComponent).raycastTarget = rraycastTarget;
        }
        public static void Set_Text_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Text)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Text_enabled(object rComponent, Boolean renabled)
        {
            ((Text)rComponent).enabled = renabled;
        }
        public static void Set_Text_tag(object rComponent, String rtag)
        {
            ((Text)rComponent).tag = rtag;
        }
        public static void Set_Text_name(object rComponent, String rname)
        {
            ((Text)rComponent).name = rname;
        }
        public static void Set_GameObjectActive_IsActive(object rComponent, Boolean rIsActive)
        {
            ((GameObjectActive)rComponent).IsActive = rIsActive;
        }
        public static void Set_GameObjectActive_IsDeActive(object rComponent, Boolean rIsDeActive)
        {
            ((GameObjectActive)rComponent).IsDeActive = rIsDeActive;
        }
        public static void Set_GameObjectActive_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((GameObjectActive)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_GameObjectActive_enabled(object rComponent, Boolean renabled)
        {
            ((GameObjectActive)rComponent).enabled = renabled;
        }
        public static void Set_GameObjectActive_tag(object rComponent, String rtag)
        {
            ((GameObjectActive)rComponent).tag = rtag;
        }
        public static void Set_GameObjectActive_name(object rComponent, String rname)
        {
            ((GameObjectActive)rComponent).name = rname;
        }
        public static void Set_Button_interactable(object rComponent, Boolean rinteractable)
        {
            ((Button)rComponent).interactable = rinteractable;
        }
        public static void Set_Button_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Button)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Button_enabled(object rComponent, Boolean renabled)
        {
            ((Button)rComponent).enabled = renabled;
        }
        public static void Set_Button_tag(object rComponent, String rtag)
        {
            ((Button)rComponent).tag = rtag;
        }
        public static void Set_Button_name(object rComponent, String rname)
        {
            ((Button)rComponent).name = rname;
        }
        public static void Set_EmptyGraphics_color(object rComponent, Color rcolor)
        {
            ((EmptyGraphics)rComponent).color = rcolor;
        }
        public static void Set_EmptyGraphics_raycastTarget(object rComponent, Boolean rraycastTarget)
        {
            ((EmptyGraphics)rComponent).raycastTarget = rraycastTarget;
        }
        public static void Set_EmptyGraphics_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((EmptyGraphics)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_EmptyGraphics_enabled(object rComponent, Boolean renabled)
        {
            ((EmptyGraphics)rComponent).enabled = renabled;
        }
        public static void Set_EmptyGraphics_tag(object rComponent, String rtag)
        {
            ((EmptyGraphics)rComponent).tag = rtag;
        }
        public static void Set_EmptyGraphics_name(object rComponent, String rname)
        {
            ((EmptyGraphics)rComponent).name = rname;
        }
        public static void Set_WText_IsUseMultiLang(object rComponent, Boolean rIsUseMultiLang)
        {
            ((WText)rComponent).IsUseMultiLang = rIsUseMultiLang;
        }
        public static void Set_WText_text(object rComponent, String rtext)
        {
            ((WText)rComponent).text = rtext;
        }
        public static void Set_WText_FontColor(object rComponent, String rFontColor)
        {
            ((WText)rComponent).FontColor = rFontColor;
        }
        public static void Set_WText_supportRichText(object rComponent, Boolean rsupportRichText)
        {
            ((WText)rComponent).supportRichText = rsupportRichText;
        }
        public static void Set_WText_resizeTextForBestFit(object rComponent, Boolean rresizeTextForBestFit)
        {
            ((WText)rComponent).resizeTextForBestFit = rresizeTextForBestFit;
        }
        public static void Set_WText_resizeTextMinSize(object rComponent, Int32 rresizeTextMinSize)
        {
            ((WText)rComponent).resizeTextMinSize = rresizeTextMinSize;
        }
        public static void Set_WText_resizeTextMaxSize(object rComponent, Int32 rresizeTextMaxSize)
        {
            ((WText)rComponent).resizeTextMaxSize = rresizeTextMaxSize;
        }
        public static void Set_WText_alignByGeometry(object rComponent, Boolean ralignByGeometry)
        {
            ((WText)rComponent).alignByGeometry = ralignByGeometry;
        }
        public static void Set_WText_fontSize(object rComponent, Int32 rfontSize)
        {
            ((WText)rComponent).fontSize = rfontSize;
        }
        public static void Set_WText_lineSpacing(object rComponent, Single rlineSpacing)
        {
            ((WText)rComponent).lineSpacing = rlineSpacing;
        }
        public static void Set_WText_maskable(object rComponent, Boolean rmaskable)
        {
            ((WText)rComponent).maskable = rmaskable;
        }
        public static void Set_WText_isMaskingGraphic(object rComponent, Boolean risMaskingGraphic)
        {
            ((WText)rComponent).isMaskingGraphic = risMaskingGraphic;
        }
        public static void Set_WText_color(object rComponent, Color rcolor)
        {
            ((WText)rComponent).color = rcolor;
        }
        public static void Set_WText_raycastTarget(object rComponent, Boolean rraycastTarget)
        {
            ((WText)rComponent).raycastTarget = rraycastTarget;
        }
        public static void Set_WText_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((WText)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_WText_enabled(object rComponent, Boolean renabled)
        {
            ((WText)rComponent).enabled = renabled;
        }
        public static void Set_WText_tag(object rComponent, String rtag)
        {
            ((WText)rComponent).tag = rtag;
        }
        public static void Set_WText_name(object rComponent, String rname)
        {
            ((WText)rComponent).name = rname;
        }
        public static void Set_ContentSizeFitter_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ContentSizeFitter)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ContentSizeFitter_enabled(object rComponent, Boolean renabled)
        {
            ((ContentSizeFitter)rComponent).enabled = renabled;
        }
        public static void Set_ContentSizeFitter_tag(object rComponent, String rtag)
        {
            ((ContentSizeFitter)rComponent).tag = rtag;
        }
        public static void Set_ContentSizeFitter_name(object rComponent, String rname)
        {
            ((ContentSizeFitter)rComponent).name = rname;
        }
        public static void Set_LayoutElement_ignoreLayout(object rComponent, Boolean rignoreLayout)
        {
            ((LayoutElement)rComponent).ignoreLayout = rignoreLayout;
        }
        public static void Set_LayoutElement_minWidth(object rComponent, Single rminWidth)
        {
            ((LayoutElement)rComponent).minWidth = rminWidth;
        }
        public static void Set_LayoutElement_minHeight(object rComponent, Single rminHeight)
        {
            ((LayoutElement)rComponent).minHeight = rminHeight;
        }
        public static void Set_LayoutElement_preferredWidth(object rComponent, Single rpreferredWidth)
        {
            ((LayoutElement)rComponent).preferredWidth = rpreferredWidth;
        }
        public static void Set_LayoutElement_preferredHeight(object rComponent, Single rpreferredHeight)
        {
            ((LayoutElement)rComponent).preferredHeight = rpreferredHeight;
        }
        public static void Set_LayoutElement_flexibleWidth(object rComponent, Single rflexibleWidth)
        {
            ((LayoutElement)rComponent).flexibleWidth = rflexibleWidth;
        }
        public static void Set_LayoutElement_flexibleHeight(object rComponent, Single rflexibleHeight)
        {
            ((LayoutElement)rComponent).flexibleHeight = rflexibleHeight;
        }
        public static void Set_LayoutElement_layoutPriority(object rComponent, Int32 rlayoutPriority)
        {
            ((LayoutElement)rComponent).layoutPriority = rlayoutPriority;
        }
        public static void Set_LayoutElement_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((LayoutElement)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_LayoutElement_enabled(object rComponent, Boolean renabled)
        {
            ((LayoutElement)rComponent).enabled = renabled;
        }
        public static void Set_LayoutElement_tag(object rComponent, String rtag)
        {
            ((LayoutElement)rComponent).tag = rtag;
        }
        public static void Set_LayoutElement_name(object rComponent, String rname)
        {
            ((LayoutElement)rComponent).name = rname;
        }
        public static void Set_RawImage_maskable(object rComponent, Boolean rmaskable)
        {
            ((RawImage)rComponent).maskable = rmaskable;
        }
        public static void Set_RawImage_isMaskingGraphic(object rComponent, Boolean risMaskingGraphic)
        {
            ((RawImage)rComponent).isMaskingGraphic = risMaskingGraphic;
        }
        public static void Set_RawImage_color(object rComponent, Color rcolor)
        {
            ((RawImage)rComponent).color = rcolor;
        }
        public static void Set_RawImage_raycastTarget(object rComponent, Boolean rraycastTarget)
        {
            ((RawImage)rComponent).raycastTarget = rraycastTarget;
        }
        public static void Set_RawImage_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((RawImage)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_RawImage_enabled(object rComponent, Boolean renabled)
        {
            ((RawImage)rComponent).enabled = renabled;
        }
        public static void Set_RawImage_tag(object rComponent, String rtag)
        {
            ((RawImage)rComponent).tag = rtag;
        }
        public static void Set_RawImage_name(object rComponent, String rname)
        {
            ((RawImage)rComponent).name = rname;
        }
        public static void Set_ScrollRect_horizontal(object rComponent, Boolean rhorizontal)
        {
            ((ScrollRect)rComponent).horizontal = rhorizontal;
        }
        public static void Set_ScrollRect_vertical(object rComponent, Boolean rvertical)
        {
            ((ScrollRect)rComponent).vertical = rvertical;
        }
        public static void Set_ScrollRect_elasticity(object rComponent, Single relasticity)
        {
            ((ScrollRect)rComponent).elasticity = relasticity;
        }
        public static void Set_ScrollRect_inertia(object rComponent, Boolean rinertia)
        {
            ((ScrollRect)rComponent).inertia = rinertia;
        }
        public static void Set_ScrollRect_decelerationRate(object rComponent, Single rdecelerationRate)
        {
            ((ScrollRect)rComponent).decelerationRate = rdecelerationRate;
        }
        public static void Set_ScrollRect_scrollSensitivity(object rComponent, Single rscrollSensitivity)
        {
            ((ScrollRect)rComponent).scrollSensitivity = rscrollSensitivity;
        }
        public static void Set_ScrollRect_horizontalScrollbarSpacing(object rComponent, Single rhorizontalScrollbarSpacing)
        {
            ((ScrollRect)rComponent).horizontalScrollbarSpacing = rhorizontalScrollbarSpacing;
        }
        public static void Set_ScrollRect_verticalScrollbarSpacing(object rComponent, Single rverticalScrollbarSpacing)
        {
            ((ScrollRect)rComponent).verticalScrollbarSpacing = rverticalScrollbarSpacing;
        }
        public static void Set_ScrollRect_velocity(object rComponent, Vector2 rvelocity)
        {
            ((ScrollRect)rComponent).velocity = rvelocity;
        }
        public static void Set_ScrollRect_normalizedPosition(object rComponent, Vector2 rnormalizedPosition)
        {
            ((ScrollRect)rComponent).normalizedPosition = rnormalizedPosition;
        }
        public static void Set_ScrollRect_horizontalNormalizedPosition(object rComponent, Single rhorizontalNormalizedPosition)
        {
            ((ScrollRect)rComponent).horizontalNormalizedPosition = rhorizontalNormalizedPosition;
        }
        public static void Set_ScrollRect_verticalNormalizedPosition(object rComponent, Single rverticalNormalizedPosition)
        {
            ((ScrollRect)rComponent).verticalNormalizedPosition = rverticalNormalizedPosition;
        }
        public static void Set_ScrollRect_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ScrollRect)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ScrollRect_enabled(object rComponent, Boolean renabled)
        {
            ((ScrollRect)rComponent).enabled = renabled;
        }
        public static void Set_ScrollRect_tag(object rComponent, String rtag)
        {
            ((ScrollRect)rComponent).tag = rtag;
        }
        public static void Set_ScrollRect_name(object rComponent, String rname)
        {
            ((ScrollRect)rComponent).name = rname;
        }
        public static void Set_Mask_showMaskGraphic(object rComponent, Boolean rshowMaskGraphic)
        {
            ((Mask)rComponent).showMaskGraphic = rshowMaskGraphic;
        }
        public static void Set_Mask_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Mask)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Mask_enabled(object rComponent, Boolean renabled)
        {
            ((Mask)rComponent).enabled = renabled;
        }
        public static void Set_Mask_tag(object rComponent, String rtag)
        {
            ((Mask)rComponent).tag = rtag;
        }
        public static void Set_Mask_name(object rComponent, String rname)
        {
            ((Mask)rComponent).name = rname;
        }
        public static void Set_HorizontalLayoutGroup_spacing(object rComponent, Single rspacing)
        {
            ((HorizontalLayoutGroup)rComponent).spacing = rspacing;
        }
        public static void Set_HorizontalLayoutGroup_childForceExpandWidth(object rComponent, Boolean rchildForceExpandWidth)
        {
            ((HorizontalLayoutGroup)rComponent).childForceExpandWidth = rchildForceExpandWidth;
        }
        public static void Set_HorizontalLayoutGroup_childForceExpandHeight(object rComponent, Boolean rchildForceExpandHeight)
        {
            ((HorizontalLayoutGroup)rComponent).childForceExpandHeight = rchildForceExpandHeight;
        }
        public static void Set_HorizontalLayoutGroup_childControlWidth(object rComponent, Boolean rchildControlWidth)
        {
            ((HorizontalLayoutGroup)rComponent).childControlWidth = rchildControlWidth;
        }
        public static void Set_HorizontalLayoutGroup_childControlHeight(object rComponent, Boolean rchildControlHeight)
        {
            ((HorizontalLayoutGroup)rComponent).childControlHeight = rchildControlHeight;
        }
        public static void Set_HorizontalLayoutGroup_childScaleWidth(object rComponent, Boolean rchildScaleWidth)
        {
            ((HorizontalLayoutGroup)rComponent).childScaleWidth = rchildScaleWidth;
        }
        public static void Set_HorizontalLayoutGroup_childScaleHeight(object rComponent, Boolean rchildScaleHeight)
        {
            ((HorizontalLayoutGroup)rComponent).childScaleHeight = rchildScaleHeight;
        }
        public static void Set_HorizontalLayoutGroup_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((HorizontalLayoutGroup)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_HorizontalLayoutGroup_enabled(object rComponent, Boolean renabled)
        {
            ((HorizontalLayoutGroup)rComponent).enabled = renabled;
        }
        public static void Set_HorizontalLayoutGroup_tag(object rComponent, String rtag)
        {
            ((HorizontalLayoutGroup)rComponent).tag = rtag;
        }
        public static void Set_HorizontalLayoutGroup_name(object rComponent, String rname)
        {
            ((HorizontalLayoutGroup)rComponent).name = rname;
        }
        public static void Set_CanvasGroup_alpha(object rComponent, Single ralpha)
        {
            ((CanvasGroup)rComponent).alpha = ralpha;
        }
        public static void Set_CanvasGroup_interactable(object rComponent, Boolean rinteractable)
        {
            ((CanvasGroup)rComponent).interactable = rinteractable;
        }
        public static void Set_CanvasGroup_blocksRaycasts(object rComponent, Boolean rblocksRaycasts)
        {
            ((CanvasGroup)rComponent).blocksRaycasts = rblocksRaycasts;
        }
        public static void Set_CanvasGroup_ignoreParentGroups(object rComponent, Boolean rignoreParentGroups)
        {
            ((CanvasGroup)rComponent).ignoreParentGroups = rignoreParentGroups;
        }
        public static void Set_CanvasGroup_enabled(object rComponent, Boolean renabled)
        {
            ((CanvasGroup)rComponent).enabled = renabled;
        }
        public static void Set_CanvasGroup_tag(object rComponent, String rtag)
        {
            ((CanvasGroup)rComponent).tag = rtag;
        }
        public static void Set_CanvasGroup_name(object rComponent, String rname)
        {
            ((CanvasGroup)rComponent).name = rname;
        }
        public static void Set_GraphicRaycaster_ignoreReversedGraphics(object rComponent, Boolean rignoreReversedGraphics)
        {
            ((GraphicRaycaster)rComponent).ignoreReversedGraphics = rignoreReversedGraphics;
        }
        public static void Set_GraphicRaycaster_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((GraphicRaycaster)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_GraphicRaycaster_enabled(object rComponent, Boolean renabled)
        {
            ((GraphicRaycaster)rComponent).enabled = renabled;
        }
        public static void Set_GraphicRaycaster_tag(object rComponent, String rtag)
        {
            ((GraphicRaycaster)rComponent).tag = rtag;
        }
        public static void Set_GraphicRaycaster_name(object rComponent, String rname)
        {
            ((GraphicRaycaster)rComponent).name = rname;
        }
        public static void Set_Canvas_scaleFactor(object rComponent, Single rscaleFactor)
        {
            ((Canvas)rComponent).scaleFactor = rscaleFactor;
        }
        public static void Set_Canvas_referencePixelsPerUnit(object rComponent, Single rreferencePixelsPerUnit)
        {
            ((Canvas)rComponent).referencePixelsPerUnit = rreferencePixelsPerUnit;
        }
        public static void Set_Canvas_overridePixelPerfect(object rComponent, Boolean roverridePixelPerfect)
        {
            ((Canvas)rComponent).overridePixelPerfect = roverridePixelPerfect;
        }
        public static void Set_Canvas_pixelPerfect(object rComponent, Boolean rpixelPerfect)
        {
            ((Canvas)rComponent).pixelPerfect = rpixelPerfect;
        }
        public static void Set_Canvas_planeDistance(object rComponent, Single rplaneDistance)
        {
            ((Canvas)rComponent).planeDistance = rplaneDistance;
        }
        public static void Set_Canvas_overrideSorting(object rComponent, Boolean roverrideSorting)
        {
            ((Canvas)rComponent).overrideSorting = roverrideSorting;
        }
        public static void Set_Canvas_sortingOrder(object rComponent, Int32 rsortingOrder)
        {
            ((Canvas)rComponent).sortingOrder = rsortingOrder;
        }
        public static void Set_Canvas_targetDisplay(object rComponent, Int32 rtargetDisplay)
        {
            ((Canvas)rComponent).targetDisplay = rtargetDisplay;
        }
        public static void Set_Canvas_sortingLayerID(object rComponent, Int32 rsortingLayerID)
        {
            ((Canvas)rComponent).sortingLayerID = rsortingLayerID;
        }
        public static void Set_Canvas_sortingLayerName(object rComponent, String rsortingLayerName)
        {
            ((Canvas)rComponent).sortingLayerName = rsortingLayerName;
        }
        public static void Set_Canvas_normalizedSortingGridSize(object rComponent, Single rnormalizedSortingGridSize)
        {
            ((Canvas)rComponent).normalizedSortingGridSize = rnormalizedSortingGridSize;
        }
        public static void Set_Canvas_enabled(object rComponent, Boolean renabled)
        {
            ((Canvas)rComponent).enabled = renabled;
        }
        public static void Set_Canvas_tag(object rComponent, String rtag)
        {
            ((Canvas)rComponent).tag = rtag;
        }
        public static void Set_Canvas_name(object rComponent, String rname)
        {
            ((Canvas)rComponent).name = rname;
        }
        public static void Set_GridLayoutGroup_cellSize(object rComponent, Vector2 rcellSize)
        {
            ((GridLayoutGroup)rComponent).cellSize = rcellSize;
        }
        public static void Set_GridLayoutGroup_spacing(object rComponent, Vector2 rspacing)
        {
            ((GridLayoutGroup)rComponent).spacing = rspacing;
        }
        public static void Set_GridLayoutGroup_constraintCount(object rComponent, Int32 rconstraintCount)
        {
            ((GridLayoutGroup)rComponent).constraintCount = rconstraintCount;
        }
        public static void Set_GridLayoutGroup_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((GridLayoutGroup)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_GridLayoutGroup_enabled(object rComponent, Boolean renabled)
        {
            ((GridLayoutGroup)rComponent).enabled = renabled;
        }
        public static void Set_GridLayoutGroup_tag(object rComponent, String rtag)
        {
            ((GridLayoutGroup)rComponent).tag = rtag;
        }
        public static void Set_GridLayoutGroup_name(object rComponent, String rname)
        {
            ((GridLayoutGroup)rComponent).name = rname;
        }
        public static void Set_UIArrayPool_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((UIArrayPool)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_UIArrayPool_enabled(object rComponent, Boolean renabled)
        {
            ((UIArrayPool)rComponent).enabled = renabled;
        }
        public static void Set_UIArrayPool_tag(object rComponent, String rtag)
        {
            ((UIArrayPool)rComponent).tag = rtag;
        }
        public static void Set_UIArrayPool_name(object rComponent, String rname)
        {
            ((UIArrayPool)rComponent).name = rname;
        }
        public static void Set_DataBindingRetrive_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((DataBindingRetrive)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_DataBindingRetrive_enabled(object rComponent, Boolean renabled)
        {
            ((DataBindingRetrive)rComponent).enabled = renabled;
        }
        public static void Set_DataBindingRetrive_tag(object rComponent, String rtag)
        {
            ((DataBindingRetrive)rComponent).tag = rtag;
        }
        public static void Set_DataBindingRetrive_name(object rComponent, String rname)
        {
            ((DataBindingRetrive)rComponent).name = rname;
        }
        public static void Set_VerticalLayoutGroup_spacing(object rComponent, Single rspacing)
        {
            ((VerticalLayoutGroup)rComponent).spacing = rspacing;
        }
        public static void Set_VerticalLayoutGroup_childForceExpandWidth(object rComponent, Boolean rchildForceExpandWidth)
        {
            ((VerticalLayoutGroup)rComponent).childForceExpandWidth = rchildForceExpandWidth;
        }
        public static void Set_VerticalLayoutGroup_childForceExpandHeight(object rComponent, Boolean rchildForceExpandHeight)
        {
            ((VerticalLayoutGroup)rComponent).childForceExpandHeight = rchildForceExpandHeight;
        }
        public static void Set_VerticalLayoutGroup_childControlWidth(object rComponent, Boolean rchildControlWidth)
        {
            ((VerticalLayoutGroup)rComponent).childControlWidth = rchildControlWidth;
        }
        public static void Set_VerticalLayoutGroup_childControlHeight(object rComponent, Boolean rchildControlHeight)
        {
            ((VerticalLayoutGroup)rComponent).childControlHeight = rchildControlHeight;
        }
        public static void Set_VerticalLayoutGroup_childScaleWidth(object rComponent, Boolean rchildScaleWidth)
        {
            ((VerticalLayoutGroup)rComponent).childScaleWidth = rchildScaleWidth;
        }
        public static void Set_VerticalLayoutGroup_childScaleHeight(object rComponent, Boolean rchildScaleHeight)
        {
            ((VerticalLayoutGroup)rComponent).childScaleHeight = rchildScaleHeight;
        }
        public static void Set_VerticalLayoutGroup_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((VerticalLayoutGroup)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_VerticalLayoutGroup_enabled(object rComponent, Boolean renabled)
        {
            ((VerticalLayoutGroup)rComponent).enabled = renabled;
        }
        public static void Set_VerticalLayoutGroup_tag(object rComponent, String rtag)
        {
            ((VerticalLayoutGroup)rComponent).tag = rtag;
        }
        public static void Set_VerticalLayoutGroup_name(object rComponent, String rname)
        {
            ((VerticalLayoutGroup)rComponent).name = rname;
        }
        public static void Set_ScreenAdapt_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ScreenAdapt)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ScreenAdapt_enabled(object rComponent, Boolean renabled)
        {
            ((ScreenAdapt)rComponent).enabled = renabled;
        }
        public static void Set_ScreenAdapt_tag(object rComponent, String rtag)
        {
            ((ScreenAdapt)rComponent).tag = rtag;
        }
        public static void Set_ScreenAdapt_name(object rComponent, String rname)
        {
            ((ScreenAdapt)rComponent).name = rname;
        }
        public static void Set_GrayGraphics_DarkNessValue(object rComponent, Single rDarkNessValue)
        {
            ((GrayGraphics)rComponent).DarkNessValue = rDarkNessValue;
        }
        public static void Set_GrayGraphics_GrayFactor(object rComponent, Single rGrayFactor)
        {
            ((GrayGraphics)rComponent).GrayFactor = rGrayFactor;
        }
        public static void Set_GrayGraphics_IsGray(object rComponent, Boolean rIsGray)
        {
            ((GrayGraphics)rComponent).IsGray = rIsGray;
        }
        public static void Set_GrayGraphics_IsGrayDisable(object rComponent, Boolean rIsGrayDisable)
        {
            ((GrayGraphics)rComponent).IsGrayDisable = rIsGrayDisable;
        }
        public static void Set_GrayGraphics_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((GrayGraphics)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_GrayGraphics_enabled(object rComponent, Boolean renabled)
        {
            ((GrayGraphics)rComponent).enabled = renabled;
        }
        public static void Set_GrayGraphics_tag(object rComponent, String rtag)
        {
            ((GrayGraphics)rComponent).tag = rtag;
        }
        public static void Set_GrayGraphics_name(object rComponent, String rname)
        {
            ((GrayGraphics)rComponent).name = rname;
        }
        public static void Set_ImageReplace_IsUseMultiLang(object rComponent, Boolean rIsUseMultiLang)
        {
            ((ImageReplace)rComponent).IsUseMultiLang = rIsUseMultiLang;
        }
        public static void Set_ImageReplace_ImgColor(object rComponent, String rImgColor)
        {
            ((ImageReplace)rComponent).ImgColor = rImgColor;
        }
        public static void Set_ImageReplace_SpriteName(object rComponent, String rSpriteName)
        {
            ((ImageReplace)rComponent).SpriteName = rSpriteName;
        }
        public static void Set_ImageReplace_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ImageReplace)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ImageReplace_enabled(object rComponent, Boolean renabled)
        {
            ((ImageReplace)rComponent).enabled = renabled;
        }
        public static void Set_ImageReplace_tag(object rComponent, String rtag)
        {
            ((ImageReplace)rComponent).tag = rtag;
        }
        public static void Set_ImageReplace_name(object rComponent, String rname)
        {
            ((ImageReplace)rComponent).name = rname;
        }
        public static void Set_LoopVerticalScrollRect_IsPlayCellAnimation(object rComponent, Boolean rIsPlayCellAnimation)
        {
            ((LoopVerticalScrollRect)rComponent).IsPlayCellAnimation = rIsPlayCellAnimation;
        }
        public static void Set_LoopVerticalScrollRect_AnimationTotalDelayTime(object rComponent, Single rAnimationTotalDelayTime)
        {
            ((LoopVerticalScrollRect)rComponent).AnimationTotalDelayTime = rAnimationTotalDelayTime;
        }
        public static void Set_LoopVerticalScrollRect_UpdateEnable(object rComponent, Boolean rUpdateEnable)
        {
            ((LoopVerticalScrollRect)rComponent).UpdateEnable = rUpdateEnable;
        }
        public static void Set_LoopVerticalScrollRect_horizontal(object rComponent, Boolean rhorizontal)
        {
            ((LoopVerticalScrollRect)rComponent).horizontal = rhorizontal;
        }
        public static void Set_LoopVerticalScrollRect_vertical(object rComponent, Boolean rvertical)
        {
            ((LoopVerticalScrollRect)rComponent).vertical = rvertical;
        }
        public static void Set_LoopVerticalScrollRect_elasticity(object rComponent, Single relasticity)
        {
            ((LoopVerticalScrollRect)rComponent).elasticity = relasticity;
        }
        public static void Set_LoopVerticalScrollRect_inertia(object rComponent, Boolean rinertia)
        {
            ((LoopVerticalScrollRect)rComponent).inertia = rinertia;
        }
        public static void Set_LoopVerticalScrollRect_decelerationRate(object rComponent, Single rdecelerationRate)
        {
            ((LoopVerticalScrollRect)rComponent).decelerationRate = rdecelerationRate;
        }
        public static void Set_LoopVerticalScrollRect_scrollSensitivity(object rComponent, Single rscrollSensitivity)
        {
            ((LoopVerticalScrollRect)rComponent).scrollSensitivity = rscrollSensitivity;
        }
        public static void Set_LoopVerticalScrollRect_isNeedHideRefresh(object rComponent, Boolean risNeedHideRefresh)
        {
            ((LoopVerticalScrollRect)rComponent).isNeedHideRefresh = risNeedHideRefresh;
        }
        public static void Set_LoopVerticalScrollRect_RefillCellIndex(object rComponent, Int32 rRefillCellIndex)
        {
            ((LoopVerticalScrollRect)rComponent).RefillCellIndex = rRefillCellIndex;
        }
        public static void Set_LoopVerticalScrollRect_horizontalScrollbarSpacing(object rComponent, Single rhorizontalScrollbarSpacing)
        {
            ((LoopVerticalScrollRect)rComponent).horizontalScrollbarSpacing = rhorizontalScrollbarSpacing;
        }
        public static void Set_LoopVerticalScrollRect_verticalScrollbarSpacing(object rComponent, Single rverticalScrollbarSpacing)
        {
            ((LoopVerticalScrollRect)rComponent).verticalScrollbarSpacing = rverticalScrollbarSpacing;
        }
        public static void Set_LoopVerticalScrollRect_velocity(object rComponent, Vector2 rvelocity)
        {
            ((LoopVerticalScrollRect)rComponent).velocity = rvelocity;
        }
        public static void Set_LoopVerticalScrollRect_normalizedPosition(object rComponent, Vector2 rnormalizedPosition)
        {
            ((LoopVerticalScrollRect)rComponent).normalizedPosition = rnormalizedPosition;
        }
        public static void Set_LoopVerticalScrollRect_horizontalNormalizedPosition(object rComponent, Single rhorizontalNormalizedPosition)
        {
            ((LoopVerticalScrollRect)rComponent).horizontalNormalizedPosition = rhorizontalNormalizedPosition;
        }
        public static void Set_LoopVerticalScrollRect_verticalNormalizedPosition(object rComponent, Single rverticalNormalizedPosition)
        {
            ((LoopVerticalScrollRect)rComponent).verticalNormalizedPosition = rverticalNormalizedPosition;
        }
        public static void Set_LoopVerticalScrollRect_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((LoopVerticalScrollRect)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_LoopVerticalScrollRect_enabled(object rComponent, Boolean renabled)
        {
            ((LoopVerticalScrollRect)rComponent).enabled = renabled;
        }
        public static void Set_LoopVerticalScrollRect_tag(object rComponent, String rtag)
        {
            ((LoopVerticalScrollRect)rComponent).tag = rtag;
        }
        public static void Set_LoopVerticalScrollRect_name(object rComponent, String rname)
        {
            ((LoopVerticalScrollRect)rComponent).name = rname;
        }
        public static void Set_LoopHorizontalScrollRect_IsPlayCellAnimation(object rComponent, Boolean rIsPlayCellAnimation)
        {
            ((LoopHorizontalScrollRect)rComponent).IsPlayCellAnimation = rIsPlayCellAnimation;
        }
        public static void Set_LoopHorizontalScrollRect_AnimationTotalDelayTime(object rComponent, Single rAnimationTotalDelayTime)
        {
            ((LoopHorizontalScrollRect)rComponent).AnimationTotalDelayTime = rAnimationTotalDelayTime;
        }
        public static void Set_LoopHorizontalScrollRect_UpdateEnable(object rComponent, Boolean rUpdateEnable)
        {
            ((LoopHorizontalScrollRect)rComponent).UpdateEnable = rUpdateEnable;
        }
        public static void Set_LoopHorizontalScrollRect_horizontal(object rComponent, Boolean rhorizontal)
        {
            ((LoopHorizontalScrollRect)rComponent).horizontal = rhorizontal;
        }
        public static void Set_LoopHorizontalScrollRect_vertical(object rComponent, Boolean rvertical)
        {
            ((LoopHorizontalScrollRect)rComponent).vertical = rvertical;
        }
        public static void Set_LoopHorizontalScrollRect_elasticity(object rComponent, Single relasticity)
        {
            ((LoopHorizontalScrollRect)rComponent).elasticity = relasticity;
        }
        public static void Set_LoopHorizontalScrollRect_inertia(object rComponent, Boolean rinertia)
        {
            ((LoopHorizontalScrollRect)rComponent).inertia = rinertia;
        }
        public static void Set_LoopHorizontalScrollRect_decelerationRate(object rComponent, Single rdecelerationRate)
        {
            ((LoopHorizontalScrollRect)rComponent).decelerationRate = rdecelerationRate;
        }
        public static void Set_LoopHorizontalScrollRect_scrollSensitivity(object rComponent, Single rscrollSensitivity)
        {
            ((LoopHorizontalScrollRect)rComponent).scrollSensitivity = rscrollSensitivity;
        }
        public static void Set_LoopHorizontalScrollRect_isNeedHideRefresh(object rComponent, Boolean risNeedHideRefresh)
        {
            ((LoopHorizontalScrollRect)rComponent).isNeedHideRefresh = risNeedHideRefresh;
        }
        public static void Set_LoopHorizontalScrollRect_RefillCellIndex(object rComponent, Int32 rRefillCellIndex)
        {
            ((LoopHorizontalScrollRect)rComponent).RefillCellIndex = rRefillCellIndex;
        }
        public static void Set_LoopHorizontalScrollRect_horizontalScrollbarSpacing(object rComponent, Single rhorizontalScrollbarSpacing)
        {
            ((LoopHorizontalScrollRect)rComponent).horizontalScrollbarSpacing = rhorizontalScrollbarSpacing;
        }
        public static void Set_LoopHorizontalScrollRect_verticalScrollbarSpacing(object rComponent, Single rverticalScrollbarSpacing)
        {
            ((LoopHorizontalScrollRect)rComponent).verticalScrollbarSpacing = rverticalScrollbarSpacing;
        }
        public static void Set_LoopHorizontalScrollRect_velocity(object rComponent, Vector2 rvelocity)
        {
            ((LoopHorizontalScrollRect)rComponent).velocity = rvelocity;
        }
        public static void Set_LoopHorizontalScrollRect_normalizedPosition(object rComponent, Vector2 rnormalizedPosition)
        {
            ((LoopHorizontalScrollRect)rComponent).normalizedPosition = rnormalizedPosition;
        }
        public static void Set_LoopHorizontalScrollRect_horizontalNormalizedPosition(object rComponent, Single rhorizontalNormalizedPosition)
        {
            ((LoopHorizontalScrollRect)rComponent).horizontalNormalizedPosition = rhorizontalNormalizedPosition;
        }
        public static void Set_LoopHorizontalScrollRect_verticalNormalizedPosition(object rComponent, Single rverticalNormalizedPosition)
        {
            ((LoopHorizontalScrollRect)rComponent).verticalNormalizedPosition = rverticalNormalizedPosition;
        }
        public static void Set_LoopHorizontalScrollRect_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((LoopHorizontalScrollRect)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_LoopHorizontalScrollRect_enabled(object rComponent, Boolean renabled)
        {
            ((LoopHorizontalScrollRect)rComponent).enabled = renabled;
        }
        public static void Set_LoopHorizontalScrollRect_tag(object rComponent, String rtag)
        {
            ((LoopHorizontalScrollRect)rComponent).tag = rtag;
        }
        public static void Set_LoopHorizontalScrollRect_name(object rComponent, String rname)
        {
            ((LoopHorizontalScrollRect)rComponent).name = rname;
        }
        public static void Set_ViewModelDataSourceArray_ParentIndex(object rComponent, Int32 rParentIndex)
        {
            ((ViewModelDataSourceArray)rComponent).ParentIndex = rParentIndex;
        }
        public static void Set_ViewModelDataSourceArray_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ViewModelDataSourceArray)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ViewModelDataSourceArray_enabled(object rComponent, Boolean renabled)
        {
            ((ViewModelDataSourceArray)rComponent).enabled = renabled;
        }
        public static void Set_ViewModelDataSourceArray_tag(object rComponent, String rtag)
        {
            ((ViewModelDataSourceArray)rComponent).tag = rtag;
        }
        public static void Set_ViewModelDataSourceArray_name(object rComponent, String rname)
        {
            ((ViewModelDataSourceArray)rComponent).name = rname;
        }
        public static void Set_ViewModelDataSourceList_ParentIndex(object rComponent, Int32 rParentIndex)
        {
            ((ViewModelDataSourceList)rComponent).ParentIndex = rParentIndex;
        }
        public static void Set_ViewModelDataSourceList_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ViewModelDataSourceList)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ViewModelDataSourceList_enabled(object rComponent, Boolean renabled)
        {
            ((ViewModelDataSourceList)rComponent).enabled = renabled;
        }
        public static void Set_ViewModelDataSourceList_tag(object rComponent, String rtag)
        {
            ((ViewModelDataSourceList)rComponent).tag = rtag;
        }
        public static void Set_ViewModelDataSourceList_name(object rComponent, String rname)
        {
            ((ViewModelDataSourceList)rComponent).name = rname;
        }
        public static void Set_CanvasScaler_referencePixelsPerUnit(object rComponent, Single rreferencePixelsPerUnit)
        {
            ((CanvasScaler)rComponent).referencePixelsPerUnit = rreferencePixelsPerUnit;
        }
        public static void Set_CanvasScaler_scaleFactor(object rComponent, Single rscaleFactor)
        {
            ((CanvasScaler)rComponent).scaleFactor = rscaleFactor;
        }
        public static void Set_CanvasScaler_referenceResolution(object rComponent, Vector2 rreferenceResolution)
        {
            ((CanvasScaler)rComponent).referenceResolution = rreferenceResolution;
        }
        public static void Set_CanvasScaler_matchWidthOrHeight(object rComponent, Single rmatchWidthOrHeight)
        {
            ((CanvasScaler)rComponent).matchWidthOrHeight = rmatchWidthOrHeight;
        }
        public static void Set_CanvasScaler_fallbackScreenDPI(object rComponent, Single rfallbackScreenDPI)
        {
            ((CanvasScaler)rComponent).fallbackScreenDPI = rfallbackScreenDPI;
        }
        public static void Set_CanvasScaler_defaultSpriteDPI(object rComponent, Single rdefaultSpriteDPI)
        {
            ((CanvasScaler)rComponent).defaultSpriteDPI = rdefaultSpriteDPI;
        }
        public static void Set_CanvasScaler_dynamicPixelsPerUnit(object rComponent, Single rdynamicPixelsPerUnit)
        {
            ((CanvasScaler)rComponent).dynamicPixelsPerUnit = rdynamicPixelsPerUnit;
        }
        public static void Set_CanvasScaler_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((CanvasScaler)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_CanvasScaler_enabled(object rComponent, Boolean renabled)
        {
            ((CanvasScaler)rComponent).enabled = renabled;
        }
        public static void Set_CanvasScaler_tag(object rComponent, String rtag)
        {
            ((CanvasScaler)rComponent).tag = rtag;
        }
        public static void Set_CanvasScaler_name(object rComponent, String rname)
        {
            ((CanvasScaler)rComponent).name = rname;
        }
        public static void Set_EventSystem_sendNavigationEvents(object rComponent, Boolean rsendNavigationEvents)
        {
            ((EventSystem)rComponent).sendNavigationEvents = rsendNavigationEvents;
        }
        public static void Set_EventSystem_pixelDragThreshold(object rComponent, Int32 rpixelDragThreshold)
        {
            ((EventSystem)rComponent).pixelDragThreshold = rpixelDragThreshold;
        }
        public static void Set_EventSystem_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((EventSystem)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_EventSystem_enabled(object rComponent, Boolean renabled)
        {
            ((EventSystem)rComponent).enabled = renabled;
        }
        public static void Set_EventSystem_tag(object rComponent, String rtag)
        {
            ((EventSystem)rComponent).tag = rtag;
        }
        public static void Set_EventSystem_name(object rComponent, String rname)
        {
            ((EventSystem)rComponent).name = rname;
        }
        public static void Set_UIRoot_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((UIRoot)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_UIRoot_enabled(object rComponent, Boolean renabled)
        {
            ((UIRoot)rComponent).enabled = renabled;
        }
        public static void Set_UIRoot_tag(object rComponent, String rtag)
        {
            ((UIRoot)rComponent).tag = rtag;
        }
        public static void Set_UIRoot_name(object rComponent, String rname)
        {
            ((UIRoot)rComponent).name = rname;
        }
        public static void Set_MeshFilter_tag(object rComponent, String rtag)
        {
            ((MeshFilter)rComponent).tag = rtag;
        }
        public static void Set_MeshFilter_name(object rComponent, String rname)
        {
            ((MeshFilter)rComponent).name = rname;
        }
        public static void Set_TextOutlineCircle_effectColor(object rComponent, Color reffectColor)
        {
            ((TextOutlineCircle)rComponent).effectColor = reffectColor;
        }
        public static void Set_TextOutlineCircle_effectDistance(object rComponent, Vector2 reffectDistance)
        {
            ((TextOutlineCircle)rComponent).effectDistance = reffectDistance;
        }
        public static void Set_TextOutlineCircle_useGraphicAlpha(object rComponent, Boolean ruseGraphicAlpha)
        {
            ((TextOutlineCircle)rComponent).useGraphicAlpha = ruseGraphicAlpha;
        }
        public static void Set_TextOutlineCircle_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((TextOutlineCircle)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_TextOutlineCircle_enabled(object rComponent, Boolean renabled)
        {
            ((TextOutlineCircle)rComponent).enabled = renabled;
        }
        public static void Set_TextOutlineCircle_tag(object rComponent, String rtag)
        {
            ((TextOutlineCircle)rComponent).tag = rtag;
        }
        public static void Set_TextOutlineCircle_name(object rComponent, String rname)
        {
            ((TextOutlineCircle)rComponent).name = rname;
        }
        public static void Set_RawImageReplace_SpriteName(object rComponent, String rSpriteName)
        {
            ((RawImageReplace)rComponent).SpriteName = rSpriteName;
        }
        public static void Set_RawImageReplace_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((RawImageReplace)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_RawImageReplace_enabled(object rComponent, Boolean renabled)
        {
            ((RawImageReplace)rComponent).enabled = renabled;
        }
        public static void Set_RawImageReplace_tag(object rComponent, String rtag)
        {
            ((RawImageReplace)rComponent).tag = rtag;
        }
        public static void Set_RawImageReplace_name(object rComponent, String rname)
        {
            ((RawImageReplace)rComponent).name = rname;
        }
        public static void Set_AnchorSafeArea_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((AnchorSafeArea)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_AnchorSafeArea_enabled(object rComponent, Boolean renabled)
        {
            ((AnchorSafeArea)rComponent).enabled = renabled;
        }
        public static void Set_AnchorSafeArea_tag(object rComponent, String rtag)
        {
            ((AnchorSafeArea)rComponent).tag = rtag;
        }
        public static void Set_AnchorSafeArea_name(object rComponent, String rname)
        {
            ((AnchorSafeArea)rComponent).name = rname;
        }
        public static void Set_GlobalMessageBox_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((GlobalMessageBox)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_GlobalMessageBox_enabled(object rComponent, Boolean renabled)
        {
            ((GlobalMessageBox)rComponent).enabled = renabled;
        }
        public static void Set_GlobalMessageBox_tag(object rComponent, String rtag)
        {
            ((GlobalMessageBox)rComponent).tag = rtag;
        }
        public static void Set_GlobalMessageBox_name(object rComponent, String rname)
        {
            ((GlobalMessageBox)rComponent).name = rname;
        }
        public static void Set_UIWait_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((UIWait)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_UIWait_enabled(object rComponent, Boolean renabled)
        {
            ((UIWait)rComponent).enabled = renabled;
        }
        public static void Set_UIWait_tag(object rComponent, String rtag)
        {
            ((UIWait)rComponent).tag = rtag;
        }
        public static void Set_UIWait_name(object rComponent, String rname)
        {
            ((UIWait)rComponent).name = rname;
        }
        public static void Set_Toast_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Toast)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Toast_enabled(object rComponent, Boolean renabled)
        {
            ((Toast)rComponent).enabled = renabled;
        }
        public static void Set_Toast_tag(object rComponent, String rtag)
        {
            ((Toast)rComponent).tag = rtag;
        }
        public static void Set_Toast_name(object rComponent, String rname)
        {
            ((Toast)rComponent).name = rname;
        }
        public static void Set_RectTransformSizeFollow_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((RectTransformSizeFollow)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_RectTransformSizeFollow_enabled(object rComponent, Boolean renabled)
        {
            ((RectTransformSizeFollow)rComponent).enabled = renabled;
        }
        public static void Set_RectTransformSizeFollow_tag(object rComponent, String rtag)
        {
            ((RectTransformSizeFollow)rComponent).tag = rtag;
        }
        public static void Set_RectTransformSizeFollow_name(object rComponent, String rname)
        {
            ((RectTransformSizeFollow)rComponent).name = rname;
        }
        public static void Set_EventSystemTransmiter_color(object rComponent, Color rcolor)
        {
            ((EventSystemTransmiter)rComponent).color = rcolor;
        }
        public static void Set_EventSystemTransmiter_raycastTarget(object rComponent, Boolean rraycastTarget)
        {
            ((EventSystemTransmiter)rComponent).raycastTarget = rraycastTarget;
        }
        public static void Set_EventSystemTransmiter_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((EventSystemTransmiter)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_EventSystemTransmiter_enabled(object rComponent, Boolean renabled)
        {
            ((EventSystemTransmiter)rComponent).enabled = renabled;
        }
        public static void Set_EventSystemTransmiter_tag(object rComponent, String rtag)
        {
            ((EventSystemTransmiter)rComponent).tag = rtag;
        }
        public static void Set_EventSystemTransmiter_name(object rComponent, String rname)
        {
            ((EventSystemTransmiter)rComponent).name = rname;
        }
        public static void Set_Slider_minValue(object rComponent, Single rminValue)
        {
            ((Slider)rComponent).minValue = rminValue;
        }
        public static void Set_Slider_maxValue(object rComponent, Single rmaxValue)
        {
            ((Slider)rComponent).maxValue = rmaxValue;
        }
        public static void Set_Slider_wholeNumbers(object rComponent, Boolean rwholeNumbers)
        {
            ((Slider)rComponent).wholeNumbers = rwholeNumbers;
        }
        public static void Set_Slider_value(object rComponent, Single rvalue)
        {
            ((Slider)rComponent).value = rvalue;
        }
        public static void Set_Slider_normalizedValue(object rComponent, Single rnormalizedValue)
        {
            ((Slider)rComponent).normalizedValue = rnormalizedValue;
        }
        public static void Set_Slider_interactable(object rComponent, Boolean rinteractable)
        {
            ((Slider)rComponent).interactable = rinteractable;
        }
        public static void Set_Slider_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Slider)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Slider_enabled(object rComponent, Boolean renabled)
        {
            ((Slider)rComponent).enabled = renabled;
        }
        public static void Set_Slider_tag(object rComponent, String rtag)
        {
            ((Slider)rComponent).tag = rtag;
        }
        public static void Set_Slider_name(object rComponent, String rname)
        {
            ((Slider)rComponent).name = rname;
        }
        public static void Set_Toggle_isOn(object rComponent, Boolean risOn)
        {
            ((Toggle)rComponent).isOn = risOn;
        }
        public static void Set_Toggle_interactable(object rComponent, Boolean rinteractable)
        {
            ((Toggle)rComponent).interactable = rinteractable;
        }
        public static void Set_Toggle_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Toggle)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Toggle_enabled(object rComponent, Boolean renabled)
        {
            ((Toggle)rComponent).enabled = renabled;
        }
        public static void Set_Toggle_tag(object rComponent, String rtag)
        {
            ((Toggle)rComponent).tag = rtag;
        }
        public static void Set_Toggle_name(object rComponent, String rname)
        {
            ((Toggle)rComponent).name = rname;
        }
        public static void Set_TabButton_isOn(object rComponent, Boolean risOn)
        {
            ((TabButton)rComponent).isOn = risOn;
        }
        public static void Set_TabButton_interactable(object rComponent, Boolean rinteractable)
        {
            ((TabButton)rComponent).interactable = rinteractable;
        }
        public static void Set_TabButton_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((TabButton)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_TabButton_enabled(object rComponent, Boolean renabled)
        {
            ((TabButton)rComponent).enabled = renabled;
        }
        public static void Set_TabButton_tag(object rComponent, String rtag)
        {
            ((TabButton)rComponent).tag = rtag;
        }
        public static void Set_TabButton_name(object rComponent, String rname)
        {
            ((TabButton)rComponent).name = rname;
        }
        public static void Set_TextSpace_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((TextSpace)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_TextSpace_enabled(object rComponent, Boolean renabled)
        {
            ((TextSpace)rComponent).enabled = renabled;
        }
        public static void Set_TextSpace_tag(object rComponent, String rtag)
        {
            ((TextSpace)rComponent).tag = rtag;
        }
        public static void Set_TextSpace_name(object rComponent, String rname)
        {
            ((TextSpace)rComponent).name = rname;
        }
        public static void Set_RectMask2D_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((RectMask2D)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_RectMask2D_enabled(object rComponent, Boolean renabled)
        {
            ((RectMask2D)rComponent).enabled = renabled;
        }
        public static void Set_RectMask2D_tag(object rComponent, String rtag)
        {
            ((RectMask2D)rComponent).tag = rtag;
        }
        public static void Set_RectMask2D_name(object rComponent, String rname)
        {
            ((RectMask2D)rComponent).name = rname;
        }
        public static void Set_LetterSpacing_spacing(object rComponent, Single rspacing)
        {
            ((LetterSpacing)rComponent).spacing = rspacing;
        }
        public static void Set_LetterSpacing_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((LetterSpacing)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_LetterSpacing_enabled(object rComponent, Boolean renabled)
        {
            ((LetterSpacing)rComponent).enabled = renabled;
        }
        public static void Set_LetterSpacing_tag(object rComponent, String rtag)
        {
            ((LetterSpacing)rComponent).tag = rtag;
        }
        public static void Set_LetterSpacing_name(object rComponent, String rname)
        {
            ((LetterSpacing)rComponent).name = rname;
        }
        public static void Set_Outline_effectColor(object rComponent, Color reffectColor)
        {
            ((Outline)rComponent).effectColor = reffectColor;
        }
        public static void Set_Outline_effectDistance(object rComponent, Vector2 reffectDistance)
        {
            ((Outline)rComponent).effectDistance = reffectDistance;
        }
        public static void Set_Outline_useGraphicAlpha(object rComponent, Boolean ruseGraphicAlpha)
        {
            ((Outline)rComponent).useGraphicAlpha = ruseGraphicAlpha;
        }
        public static void Set_Outline_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Outline)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Outline_enabled(object rComponent, Boolean renabled)
        {
            ((Outline)rComponent).enabled = renabled;
        }
        public static void Set_Outline_tag(object rComponent, String rtag)
        {
            ((Outline)rComponent).tag = rtag;
        }
        public static void Set_Outline_name(object rComponent, String rname)
        {
            ((Outline)rComponent).name = rname;
        }
        public static void Set_TextOutline8_effectColor(object rComponent, Color reffectColor)
        {
            ((TextOutline8)rComponent).effectColor = reffectColor;
        }
        public static void Set_TextOutline8_effectDistance(object rComponent, Vector2 reffectDistance)
        {
            ((TextOutline8)rComponent).effectDistance = reffectDistance;
        }
        public static void Set_TextOutline8_useGraphicAlpha(object rComponent, Boolean ruseGraphicAlpha)
        {
            ((TextOutline8)rComponent).useGraphicAlpha = ruseGraphicAlpha;
        }
        public static void Set_TextOutline8_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((TextOutline8)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_TextOutline8_enabled(object rComponent, Boolean renabled)
        {
            ((TextOutline8)rComponent).enabled = renabled;
        }
        public static void Set_TextOutline8_tag(object rComponent, String rtag)
        {
            ((TextOutline8)rComponent).tag = rtag;
        }
        public static void Set_TextOutline8_name(object rComponent, String rname)
        {
            ((TextOutline8)rComponent).name = rname;
        }
        public static void Set_SoftMaskable_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((SoftMaskable)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_SoftMaskable_enabled(object rComponent, Boolean renabled)
        {
            ((SoftMaskable)rComponent).enabled = renabled;
        }
        public static void Set_SoftMaskable_tag(object rComponent, String rtag)
        {
            ((SoftMaskable)rComponent).tag = rtag;
        }
        public static void Set_SoftMaskable_name(object rComponent, String rname)
        {
            ((SoftMaskable)rComponent).name = rname;
        }
        public static void Set_NoRegularImageAlpha_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((NoRegularImageAlpha)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_NoRegularImageAlpha_enabled(object rComponent, Boolean renabled)
        {
            ((NoRegularImageAlpha)rComponent).enabled = renabled;
        }
        public static void Set_NoRegularImageAlpha_tag(object rComponent, String rtag)
        {
            ((NoRegularImageAlpha)rComponent).tag = rtag;
        }
        public static void Set_NoRegularImageAlpha_name(object rComponent, String rname)
        {
            ((NoRegularImageAlpha)rComponent).name = rname;
        }
        public static void Set_EventTrigger_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((EventTrigger)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_EventTrigger_enabled(object rComponent, Boolean renabled)
        {
            ((EventTrigger)rComponent).enabled = renabled;
        }
        public static void Set_EventTrigger_tag(object rComponent, String rtag)
        {
            ((EventTrigger)rComponent).tag = rtag;
        }
        public static void Set_EventTrigger_name(object rComponent, String rname)
        {
            ((EventTrigger)rComponent).name = rname;
        }
        public static void Set_AnimationCell_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((AnimationCell)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_AnimationCell_enabled(object rComponent, Boolean renabled)
        {
            ((AnimationCell)rComponent).enabled = renabled;
        }
        public static void Set_AnimationCell_tag(object rComponent, String rtag)
        {
            ((AnimationCell)rComponent).tag = rtag;
        }
        public static void Set_AnimationCell_name(object rComponent, String rname)
        {
            ((AnimationCell)rComponent).name = rname;
        }
        public static void Set_SoftMask_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((SoftMask)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_SoftMask_enabled(object rComponent, Boolean renabled)
        {
            ((SoftMask)rComponent).enabled = renabled;
        }
        public static void Set_SoftMask_tag(object rComponent, String rtag)
        {
            ((SoftMask)rComponent).tag = rtag;
        }
        public static void Set_SoftMask_name(object rComponent, String rname)
        {
            ((SoftMask)rComponent).name = rname;
        }
        public static void Set_PressButton_interactable(object rComponent, Boolean rinteractable)
        {
            ((PressButton)rComponent).interactable = rinteractable;
        }
        public static void Set_PressButton_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((PressButton)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_PressButton_enabled(object rComponent, Boolean renabled)
        {
            ((PressButton)rComponent).enabled = renabled;
        }
        public static void Set_PressButton_tag(object rComponent, String rtag)
        {
            ((PressButton)rComponent).tag = rtag;
        }
        public static void Set_PressButton_name(object rComponent, String rname)
        {
            ((PressButton)rComponent).name = rname;
        }
        public static void Set_ScreenAdapt_UnSafeArea_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ScreenAdapt_UnSafeArea)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ScreenAdapt_UnSafeArea_enabled(object rComponent, Boolean renabled)
        {
            ((ScreenAdapt_UnSafeArea)rComponent).enabled = renabled;
        }
        public static void Set_ScreenAdapt_UnSafeArea_tag(object rComponent, String rtag)
        {
            ((ScreenAdapt_UnSafeArea)rComponent).tag = rtag;
        }
        public static void Set_ScreenAdapt_UnSafeArea_name(object rComponent, String rname)
        {
            ((ScreenAdapt_UnSafeArea)rComponent).name = rname;
        }
        public static void Set_DragScrollRect_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((DragScrollRect)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_DragScrollRect_enabled(object rComponent, Boolean renabled)
        {
            ((DragScrollRect)rComponent).enabled = renabled;
        }
        public static void Set_DragScrollRect_tag(object rComponent, String rtag)
        {
            ((DragScrollRect)rComponent).tag = rtag;
        }
        public static void Set_DragScrollRect_name(object rComponent, String rname)
        {
            ((DragScrollRect)rComponent).name = rname;
        }
        public static void Set_Scrollbar_value(object rComponent, Single rvalue)
        {
            ((Scrollbar)rComponent).value = rvalue;
        }
        public static void Set_Scrollbar_size(object rComponent, Single rsize)
        {
            ((Scrollbar)rComponent).size = rsize;
        }
        public static void Set_Scrollbar_numberOfSteps(object rComponent, Int32 rnumberOfSteps)
        {
            ((Scrollbar)rComponent).numberOfSteps = rnumberOfSteps;
        }
        public static void Set_Scrollbar_interactable(object rComponent, Boolean rinteractable)
        {
            ((Scrollbar)rComponent).interactable = rinteractable;
        }
        public static void Set_Scrollbar_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Scrollbar)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Scrollbar_enabled(object rComponent, Boolean renabled)
        {
            ((Scrollbar)rComponent).enabled = renabled;
        }
        public static void Set_Scrollbar_tag(object rComponent, String rtag)
        {
            ((Scrollbar)rComponent).tag = rtag;
        }
        public static void Set_Scrollbar_name(object rComponent, String rname)
        {
            ((Scrollbar)rComponent).name = rname;
        }
        public static void Set_ViewModelDataSourceFancyList_ParentIndex(object rComponent, Int32 rParentIndex)
        {
            ((ViewModelDataSourceFancyList)rComponent).ParentIndex = rParentIndex;
        }
        public static void Set_ViewModelDataSourceFancyList_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((ViewModelDataSourceFancyList)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_ViewModelDataSourceFancyList_enabled(object rComponent, Boolean renabled)
        {
            ((ViewModelDataSourceFancyList)rComponent).enabled = renabled;
        }
        public static void Set_ViewModelDataSourceFancyList_tag(object rComponent, String rtag)
        {
            ((ViewModelDataSourceFancyList)rComponent).tag = rtag;
        }
        public static void Set_ViewModelDataSourceFancyList_name(object rComponent, String rname)
        {
            ((ViewModelDataSourceFancyList)rComponent).name = rname;
        }
        public static void Set_FancyBindingScrollView_CurrentSelectIndex(object rComponent, Int32 rCurrentSelectIndex)
        {
            ((FancyBindingScrollView)rComponent).CurrentSelectIndex = rCurrentSelectIndex;
        }
        public static void Set_FancyBindingScrollView_ScrollDuration(object rComponent, Single rScrollDuration)
        {
            ((FancyBindingScrollView)rComponent).ScrollDuration = rScrollDuration;
        }
        public static void Set_FancyBindingScrollView_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((FancyBindingScrollView)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_FancyBindingScrollView_enabled(object rComponent, Boolean renabled)
        {
            ((FancyBindingScrollView)rComponent).enabled = renabled;
        }
        public static void Set_FancyBindingScrollView_tag(object rComponent, String rtag)
        {
            ((FancyBindingScrollView)rComponent).tag = rtag;
        }
        public static void Set_FancyBindingScrollView_name(object rComponent, String rname)
        {
            ((FancyBindingScrollView)rComponent).name = rname;
        }
        public static void Set_AspectRatioFitter_aspectRatio(object rComponent, Single raspectRatio)
        {
            ((AspectRatioFitter)rComponent).aspectRatio = raspectRatio;
        }
        public static void Set_AspectRatioFitter_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((AspectRatioFitter)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_AspectRatioFitter_enabled(object rComponent, Boolean renabled)
        {
            ((AspectRatioFitter)rComponent).enabled = renabled;
        }
        public static void Set_AspectRatioFitter_tag(object rComponent, String rtag)
        {
            ((AspectRatioFitter)rComponent).tag = rtag;
        }
        public static void Set_AspectRatioFitter_name(object rComponent, String rname)
        {
            ((AspectRatioFitter)rComponent).name = rname;
        }
        public static void Set_UIMaterialModifier_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((UIMaterialModifier)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_UIMaterialModifier_enabled(object rComponent, Boolean renabled)
        {
            ((UIMaterialModifier)rComponent).enabled = renabled;
        }
        public static void Set_UIMaterialModifier_tag(object rComponent, String rtag)
        {
            ((UIMaterialModifier)rComponent).tag = rtag;
        }
        public static void Set_UIMaterialModifier_name(object rComponent, String rname)
        {
            ((UIMaterialModifier)rComponent).name = rname;
        }
        public static void Set_Scroller_Elasticity(object rComponent, Single rElasticity)
        {
            ((Scroller)rComponent).Elasticity = rElasticity;
        }
        public static void Set_Scroller_ScrollSensitivity(object rComponent, Single rScrollSensitivity)
        {
            ((Scroller)rComponent).ScrollSensitivity = rScrollSensitivity;
        }
        public static void Set_Scroller_Inertia(object rComponent, Boolean rInertia)
        {
            ((Scroller)rComponent).Inertia = rInertia;
        }
        public static void Set_Scroller_DecelerationRate(object rComponent, Single rDecelerationRate)
        {
            ((Scroller)rComponent).DecelerationRate = rDecelerationRate;
        }
        public static void Set_Scroller_SnapEnabled(object rComponent, Boolean rSnapEnabled)
        {
            ((Scroller)rComponent).SnapEnabled = rSnapEnabled;
        }
        public static void Set_Scroller_Draggable(object rComponent, Boolean rDraggable)
        {
            ((Scroller)rComponent).Draggable = rDraggable;
        }
        public static void Set_Scroller_Position(object rComponent, Single rPosition)
        {
            ((Scroller)rComponent).Position = rPosition;
        }
        public static void Set_Scroller_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Scroller)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Scroller_enabled(object rComponent, Boolean renabled)
        {
            ((Scroller)rComponent).enabled = renabled;
        }
        public static void Set_Scroller_tag(object rComponent, String rtag)
        {
            ((Scroller)rComponent).tag = rtag;
        }
        public static void Set_Scroller_name(object rComponent, String rname)
        {
            ((Scroller)rComponent).name = rname;
        }
        public static void Set_FancyBindingCell_Index(object rComponent, Int32 rIndex)
        {
            ((FancyBindingCell)rComponent).Index = rIndex;
        }
        public static void Set_FancyBindingCell_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((FancyBindingCell)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_FancyBindingCell_enabled(object rComponent, Boolean renabled)
        {
            ((FancyBindingCell)rComponent).enabled = renabled;
        }
        public static void Set_FancyBindingCell_tag(object rComponent, String rtag)
        {
            ((FancyBindingCell)rComponent).tag = rtag;
        }
        public static void Set_FancyBindingCell_name(object rComponent, String rname)
        {
            ((FancyBindingCell)rComponent).name = rname;
        }
        public static void Set_TextAlignmentAdapter_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((TextAlignmentAdapter)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_TextAlignmentAdapter_enabled(object rComponent, Boolean renabled)
        {
            ((TextAlignmentAdapter)rComponent).enabled = renabled;
        }
        public static void Set_TextAlignmentAdapter_tag(object rComponent, String rtag)
        {
            ((TextAlignmentAdapter)rComponent).tag = rtag;
        }
        public static void Set_TextAlignmentAdapter_name(object rComponent, String rname)
        {
            ((TextAlignmentAdapter)rComponent).name = rname;
        }
        public static void Set_InputField_shouldHideMobileInput(object rComponent, Boolean rshouldHideMobileInput)
        {
            ((InputField)rComponent).shouldHideMobileInput = rshouldHideMobileInput;
        }
        public static void Set_InputField_text(object rComponent, String rtext)
        {
            ((InputField)rComponent).text = rtext;
        }
        public static void Set_InputField_caretBlinkRate(object rComponent, Single rcaretBlinkRate)
        {
            ((InputField)rComponent).caretBlinkRate = rcaretBlinkRate;
        }
        public static void Set_InputField_caretWidth(object rComponent, Int32 rcaretWidth)
        {
            ((InputField)rComponent).caretWidth = rcaretWidth;
        }
        public static void Set_InputField_caretColor(object rComponent, Color rcaretColor)
        {
            ((InputField)rComponent).caretColor = rcaretColor;
        }
        public static void Set_InputField_customCaretColor(object rComponent, Boolean rcustomCaretColor)
        {
            ((InputField)rComponent).customCaretColor = rcustomCaretColor;
        }
        public static void Set_InputField_selectionColor(object rComponent, Color rselectionColor)
        {
            ((InputField)rComponent).selectionColor = rselectionColor;
        }
        public static void Set_InputField_characterLimit(object rComponent, Int32 rcharacterLimit)
        {
            ((InputField)rComponent).characterLimit = rcharacterLimit;
        }
        public static void Set_InputField_readOnly(object rComponent, Boolean rreadOnly)
        {
            ((InputField)rComponent).readOnly = rreadOnly;
        }
        public static void Set_InputField_caretPosition(object rComponent, Int32 rcaretPosition)
        {
            ((InputField)rComponent).caretPosition = rcaretPosition;
        }
        public static void Set_InputField_selectionAnchorPosition(object rComponent, Int32 rselectionAnchorPosition)
        {
            ((InputField)rComponent).selectionAnchorPosition = rselectionAnchorPosition;
        }
        public static void Set_InputField_selectionFocusPosition(object rComponent, Int32 rselectionFocusPosition)
        {
            ((InputField)rComponent).selectionFocusPosition = rselectionFocusPosition;
        }
        public static void Set_InputField_interactable(object rComponent, Boolean rinteractable)
        {
            ((InputField)rComponent).interactable = rinteractable;
        }
        public static void Set_InputField_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((InputField)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_InputField_enabled(object rComponent, Boolean renabled)
        {
            ((InputField)rComponent).enabled = renabled;
        }
        public static void Set_InputField_tag(object rComponent, String rtag)
        {
            ((InputField)rComponent).tag = rtag;
        }
        public static void Set_InputField_name(object rComponent, String rname)
        {
            ((InputField)rComponent).name = rname;
        }
        public static void Set_Book_CurrentPage(object rComponent, Int32 rCurrentPage)
        {
            ((Book)rComponent).CurrentPage = rCurrentPage;
        }
        public static void Set_Book_NextPageID(object rComponent, Int32 rNextPageID)
        {
            ((Book)rComponent).NextPageID = rNextPageID;
        }
        public static void Set_Book_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Book)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Book_enabled(object rComponent, Boolean renabled)
        {
            ((Book)rComponent).enabled = renabled;
        }
        public static void Set_Book_tag(object rComponent, String rtag)
        {
            ((Book)rComponent).tag = rtag;
        }
        public static void Set_Book_name(object rComponent, String rname)
        {
            ((Book)rComponent).name = rname;
        }
        public static void Set_AutoFlip_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((AutoFlip)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_AutoFlip_enabled(object rComponent, Boolean renabled)
        {
            ((AutoFlip)rComponent).enabled = renabled;
        }
        public static void Set_AutoFlip_tag(object rComponent, String rtag)
        {
            ((AutoFlip)rComponent).tag = rtag;
        }
        public static void Set_AutoFlip_name(object rComponent, String rname)
        {
            ((AutoFlip)rComponent).name = rname;
        }
        public static void Set_SetAsLastSibling_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((SetAsLastSibling)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_SetAsLastSibling_enabled(object rComponent, Boolean renabled)
        {
            ((SetAsLastSibling)rComponent).enabled = renabled;
        }
        public static void Set_SetAsLastSibling_tag(object rComponent, String rtag)
        {
            ((SetAsLastSibling)rComponent).tag = rtag;
        }
        public static void Set_SetAsLastSibling_name(object rComponent, String rname)
        {
            ((SetAsLastSibling)rComponent).name = rname;
        }
        public static void Set_ReflectionProbe_size(object rComponent, Vector3 rsize)
        {
            ((ReflectionProbe)rComponent).size = rsize;
        }
        public static void Set_ReflectionProbe_center(object rComponent, Vector3 rcenter)
        {
            ((ReflectionProbe)rComponent).center = rcenter;
        }
        public static void Set_ReflectionProbe_nearClipPlane(object rComponent, Single rnearClipPlane)
        {
            ((ReflectionProbe)rComponent).nearClipPlane = rnearClipPlane;
        }
        public static void Set_ReflectionProbe_farClipPlane(object rComponent, Single rfarClipPlane)
        {
            ((ReflectionProbe)rComponent).farClipPlane = rfarClipPlane;
        }
        public static void Set_ReflectionProbe_intensity(object rComponent, Single rintensity)
        {
            ((ReflectionProbe)rComponent).intensity = rintensity;
        }
        public static void Set_ReflectionProbe_hdr(object rComponent, Boolean rhdr)
        {
            ((ReflectionProbe)rComponent).hdr = rhdr;
        }
        public static void Set_ReflectionProbe_shadowDistance(object rComponent, Single rshadowDistance)
        {
            ((ReflectionProbe)rComponent).shadowDistance = rshadowDistance;
        }
        public static void Set_ReflectionProbe_resolution(object rComponent, Int32 rresolution)
        {
            ((ReflectionProbe)rComponent).resolution = rresolution;
        }
        public static void Set_ReflectionProbe_cullingMask(object rComponent, Int32 rcullingMask)
        {
            ((ReflectionProbe)rComponent).cullingMask = rcullingMask;
        }
        public static void Set_ReflectionProbe_backgroundColor(object rComponent, Color rbackgroundColor)
        {
            ((ReflectionProbe)rComponent).backgroundColor = rbackgroundColor;
        }
        public static void Set_ReflectionProbe_blendDistance(object rComponent, Single rblendDistance)
        {
            ((ReflectionProbe)rComponent).blendDistance = rblendDistance;
        }
        public static void Set_ReflectionProbe_boxProjection(object rComponent, Boolean rboxProjection)
        {
            ((ReflectionProbe)rComponent).boxProjection = rboxProjection;
        }
        public static void Set_ReflectionProbe_importance(object rComponent, Int32 rimportance)
        {
            ((ReflectionProbe)rComponent).importance = rimportance;
        }
        public static void Set_ReflectionProbe_enabled(object rComponent, Boolean renabled)
        {
            ((ReflectionProbe)rComponent).enabled = renabled;
        }
        public static void Set_ReflectionProbe_tag(object rComponent, String rtag)
        {
            ((ReflectionProbe)rComponent).tag = rtag;
        }
        public static void Set_ReflectionProbe_name(object rComponent, String rname)
        {
            ((ReflectionProbe)rComponent).name = rname;
        }
        public static void Set_TrailRenderer_time(object rComponent, Single rtime)
        {
            ((TrailRenderer)rComponent).time = rtime;
        }
        public static void Set_TrailRenderer_startWidth(object rComponent, Single rstartWidth)
        {
            ((TrailRenderer)rComponent).startWidth = rstartWidth;
        }
        public static void Set_TrailRenderer_endWidth(object rComponent, Single rendWidth)
        {
            ((TrailRenderer)rComponent).endWidth = rendWidth;
        }
        public static void Set_TrailRenderer_widthMultiplier(object rComponent, Single rwidthMultiplier)
        {
            ((TrailRenderer)rComponent).widthMultiplier = rwidthMultiplier;
        }
        public static void Set_TrailRenderer_autodestruct(object rComponent, Boolean rautodestruct)
        {
            ((TrailRenderer)rComponent).autodestruct = rautodestruct;
        }
        public static void Set_TrailRenderer_emitting(object rComponent, Boolean remitting)
        {
            ((TrailRenderer)rComponent).emitting = remitting;
        }
        public static void Set_TrailRenderer_numCornerVertices(object rComponent, Int32 rnumCornerVertices)
        {
            ((TrailRenderer)rComponent).numCornerVertices = rnumCornerVertices;
        }
        public static void Set_TrailRenderer_numCapVertices(object rComponent, Int32 rnumCapVertices)
        {
            ((TrailRenderer)rComponent).numCapVertices = rnumCapVertices;
        }
        public static void Set_TrailRenderer_minVertexDistance(object rComponent, Single rminVertexDistance)
        {
            ((TrailRenderer)rComponent).minVertexDistance = rminVertexDistance;
        }
        public static void Set_TrailRenderer_startColor(object rComponent, Color rstartColor)
        {
            ((TrailRenderer)rComponent).startColor = rstartColor;
        }
        public static void Set_TrailRenderer_endColor(object rComponent, Color rendColor)
        {
            ((TrailRenderer)rComponent).endColor = rendColor;
        }
        public static void Set_TrailRenderer_shadowBias(object rComponent, Single rshadowBias)
        {
            ((TrailRenderer)rComponent).shadowBias = rshadowBias;
        }
        public static void Set_TrailRenderer_generateLightingData(object rComponent, Boolean rgenerateLightingData)
        {
            ((TrailRenderer)rComponent).generateLightingData = rgenerateLightingData;
        }
        public static void Set_TrailRenderer_enabled(object rComponent, Boolean renabled)
        {
            ((TrailRenderer)rComponent).enabled = renabled;
        }
        public static void Set_TrailRenderer_receiveShadows(object rComponent, Boolean rreceiveShadows)
        {
            ((TrailRenderer)rComponent).receiveShadows = rreceiveShadows;
        }
        public static void Set_TrailRenderer_forceRenderingOff(object rComponent, Boolean rforceRenderingOff)
        {
            ((TrailRenderer)rComponent).forceRenderingOff = rforceRenderingOff;
        }
        public static void Set_TrailRenderer_rendererPriority(object rComponent, Int32 rrendererPriority)
        {
            ((TrailRenderer)rComponent).rendererPriority = rrendererPriority;
        }
        public static void Set_TrailRenderer_sortingLayerName(object rComponent, String rsortingLayerName)
        {
            ((TrailRenderer)rComponent).sortingLayerName = rsortingLayerName;
        }
        public static void Set_TrailRenderer_sortingLayerID(object rComponent, Int32 rsortingLayerID)
        {
            ((TrailRenderer)rComponent).sortingLayerID = rsortingLayerID;
        }
        public static void Set_TrailRenderer_sortingOrder(object rComponent, Int32 rsortingOrder)
        {
            ((TrailRenderer)rComponent).sortingOrder = rsortingOrder;
        }
        public static void Set_TrailRenderer_allowOcclusionWhenDynamic(object rComponent, Boolean rallowOcclusionWhenDynamic)
        {
            ((TrailRenderer)rComponent).allowOcclusionWhenDynamic = rallowOcclusionWhenDynamic;
        }
        public static void Set_TrailRenderer_lightmapIndex(object rComponent, Int32 rlightmapIndex)
        {
            ((TrailRenderer)rComponent).lightmapIndex = rlightmapIndex;
        }
        public static void Set_TrailRenderer_realtimeLightmapIndex(object rComponent, Int32 rrealtimeLightmapIndex)
        {
            ((TrailRenderer)rComponent).realtimeLightmapIndex = rrealtimeLightmapIndex;
        }
        public static void Set_TrailRenderer_tag(object rComponent, String rtag)
        {
            ((TrailRenderer)rComponent).tag = rtag;
        }
        public static void Set_TrailRenderer_name(object rComponent, String rname)
        {
            ((TrailRenderer)rComponent).name = rname;
        }
        public static void Set_SkinnedMeshRenderer_updateWhenOffscreen(object rComponent, Boolean rupdateWhenOffscreen)
        {
            ((SkinnedMeshRenderer)rComponent).updateWhenOffscreen = rupdateWhenOffscreen;
        }
        public static void Set_SkinnedMeshRenderer_forceMatrixRecalculationPerRender(object rComponent, Boolean rforceMatrixRecalculationPerRender)
        {
            ((SkinnedMeshRenderer)rComponent).forceMatrixRecalculationPerRender = rforceMatrixRecalculationPerRender;
        }
        public static void Set_SkinnedMeshRenderer_skinnedMotionVectors(object rComponent, Boolean rskinnedMotionVectors)
        {
            ((SkinnedMeshRenderer)rComponent).skinnedMotionVectors = rskinnedMotionVectors;
        }
        public static void Set_SkinnedMeshRenderer_enabled(object rComponent, Boolean renabled)
        {
            ((SkinnedMeshRenderer)rComponent).enabled = renabled;
        }
        public static void Set_SkinnedMeshRenderer_receiveShadows(object rComponent, Boolean rreceiveShadows)
        {
            ((SkinnedMeshRenderer)rComponent).receiveShadows = rreceiveShadows;
        }
        public static void Set_SkinnedMeshRenderer_forceRenderingOff(object rComponent, Boolean rforceRenderingOff)
        {
            ((SkinnedMeshRenderer)rComponent).forceRenderingOff = rforceRenderingOff;
        }
        public static void Set_SkinnedMeshRenderer_rendererPriority(object rComponent, Int32 rrendererPriority)
        {
            ((SkinnedMeshRenderer)rComponent).rendererPriority = rrendererPriority;
        }
        public static void Set_SkinnedMeshRenderer_sortingLayerName(object rComponent, String rsortingLayerName)
        {
            ((SkinnedMeshRenderer)rComponent).sortingLayerName = rsortingLayerName;
        }
        public static void Set_SkinnedMeshRenderer_sortingLayerID(object rComponent, Int32 rsortingLayerID)
        {
            ((SkinnedMeshRenderer)rComponent).sortingLayerID = rsortingLayerID;
        }
        public static void Set_SkinnedMeshRenderer_sortingOrder(object rComponent, Int32 rsortingOrder)
        {
            ((SkinnedMeshRenderer)rComponent).sortingOrder = rsortingOrder;
        }
        public static void Set_SkinnedMeshRenderer_allowOcclusionWhenDynamic(object rComponent, Boolean rallowOcclusionWhenDynamic)
        {
            ((SkinnedMeshRenderer)rComponent).allowOcclusionWhenDynamic = rallowOcclusionWhenDynamic;
        }
        public static void Set_SkinnedMeshRenderer_lightmapIndex(object rComponent, Int32 rlightmapIndex)
        {
            ((SkinnedMeshRenderer)rComponent).lightmapIndex = rlightmapIndex;
        }
        public static void Set_SkinnedMeshRenderer_realtimeLightmapIndex(object rComponent, Int32 rrealtimeLightmapIndex)
        {
            ((SkinnedMeshRenderer)rComponent).realtimeLightmapIndex = rrealtimeLightmapIndex;
        }
        public static void Set_SkinnedMeshRenderer_tag(object rComponent, String rtag)
        {
            ((SkinnedMeshRenderer)rComponent).tag = rtag;
        }
        public static void Set_SkinnedMeshRenderer_name(object rComponent, String rname)
        {
            ((SkinnedMeshRenderer)rComponent).name = rname;
        }
        public static void Set_Shadow_effectColor(object rComponent, Color reffectColor)
        {
            ((Shadow)rComponent).effectColor = reffectColor;
        }
        public static void Set_Shadow_effectDistance(object rComponent, Vector2 reffectDistance)
        {
            ((Shadow)rComponent).effectDistance = reffectDistance;
        }
        public static void Set_Shadow_useGraphicAlpha(object rComponent, Boolean ruseGraphicAlpha)
        {
            ((Shadow)rComponent).useGraphicAlpha = ruseGraphicAlpha;
        }
        public static void Set_Shadow_useGUILayout(object rComponent, Boolean ruseGUILayout)
        {
            ((Shadow)rComponent).useGUILayout = ruseGUILayout;
        }
        public static void Set_Shadow_enabled(object rComponent, Boolean renabled)
        {
            ((Shadow)rComponent).enabled = renabled;
        }
        public static void Set_Shadow_tag(object rComponent, String rtag)
        {
            ((Shadow)rComponent).tag = rtag;
        }
        public static void Set_Shadow_name(object rComponent, String rname)
        {
            ((Shadow)rComponent).name = rname;
        }

        public static Dictionary<string, Action<object, Vector3>> mAllFieldSetFuncDict_Vector3 = new Dictionary<string, Action<object, Vector3>>()
        {
            {"Set_Transform_position" ,Set_Transform_position},
            {"Set_Transform_localPosition" ,Set_Transform_localPosition},
            {"Set_Transform_eulerAngles" ,Set_Transform_eulerAngles},
            {"Set_Transform_localEulerAngles" ,Set_Transform_localEulerAngles},
            {"Set_Transform_right" ,Set_Transform_right},
            {"Set_Transform_up" ,Set_Transform_up},
            {"Set_Transform_forward" ,Set_Transform_forward},
            {"Set_Transform_localScale" ,Set_Transform_localScale},
            {"Set_Camera_transparencySortAxis" ,Set_Camera_transparencySortAxis},
            {"Set_RectTransform_anchoredPosition3D" ,Set_RectTransform_anchoredPosition3D},
            {"Set_RectTransform_position" ,Set_RectTransform_position},
            {"Set_RectTransform_localPosition" ,Set_RectTransform_localPosition},
            {"Set_RectTransform_eulerAngles" ,Set_RectTransform_eulerAngles},
            {"Set_RectTransform_localEulerAngles" ,Set_RectTransform_localEulerAngles},
            {"Set_RectTransform_right" ,Set_RectTransform_right},
            {"Set_RectTransform_up" ,Set_RectTransform_up},
            {"Set_RectTransform_forward" ,Set_RectTransform_forward},
            {"Set_RectTransform_localScale" ,Set_RectTransform_localScale},
            {"Set_ReflectionProbe_size" ,Set_ReflectionProbe_size},
            {"Set_ReflectionProbe_center" ,Set_ReflectionProbe_center},
        };
        public static Dictionary<string, Action<object, Boolean>> mAllFieldSetFuncDict_Boolean = new Dictionary<string, Action<object, Boolean>>()
        {
            {"Set_Transform_hasChanged" ,Set_Transform_hasChanged},
            {"Set_Behaviour_enabled" ,Set_Behaviour_enabled},
            {"Set_FlareLayer_enabled" ,Set_FlareLayer_enabled},
            {"Set_Camera_allowHDR" ,Set_Camera_allowHDR},
            {"Set_Camera_allowMSAA" ,Set_Camera_allowMSAA},
            {"Set_Camera_allowDynamicResolution" ,Set_Camera_allowDynamicResolution},
            {"Set_Camera_forceIntoRenderTexture" ,Set_Camera_forceIntoRenderTexture},
            {"Set_Camera_orthographic" ,Set_Camera_orthographic},
            {"Set_Camera_layerCullSpherical" ,Set_Camera_layerCullSpherical},
            {"Set_Camera_useOcclusionCulling" ,Set_Camera_useOcclusionCulling},
            {"Set_Camera_clearStencilAfterLightingPass" ,Set_Camera_clearStencilAfterLightingPass},
            {"Set_Camera_usePhysicalProperties" ,Set_Camera_usePhysicalProperties},
            {"Set_Camera_useJitteredProjectionMatrixForTransparentRendering" ,Set_Camera_useJitteredProjectionMatrixForTransparentRendering},
            {"Set_Camera_enabled" ,Set_Camera_enabled},
            {"Set_Image_preserveAspect" ,Set_Image_preserveAspect},
            {"Set_Image_fillCenter" ,Set_Image_fillCenter},
            {"Set_Image_fillClockwise" ,Set_Image_fillClockwise},
            {"Set_Image_useSpriteMesh" ,Set_Image_useSpriteMesh},
            {"Set_Image_maskable" ,Set_Image_maskable},
            {"Set_Image_isMaskingGraphic" ,Set_Image_isMaskingGraphic},
            {"Set_Image_raycastTarget" ,Set_Image_raycastTarget},
            {"Set_Image_useGUILayout" ,Set_Image_useGUILayout},
            {"Set_Image_enabled" ,Set_Image_enabled},
            {"Set_RectTransform_hasChanged" ,Set_RectTransform_hasChanged},
            {"Set_Text_supportRichText" ,Set_Text_supportRichText},
            {"Set_Text_resizeTextForBestFit" ,Set_Text_resizeTextForBestFit},
            {"Set_Text_alignByGeometry" ,Set_Text_alignByGeometry},
            {"Set_Text_maskable" ,Set_Text_maskable},
            {"Set_Text_isMaskingGraphic" ,Set_Text_isMaskingGraphic},
            {"Set_Text_raycastTarget" ,Set_Text_raycastTarget},
            {"Set_Text_useGUILayout" ,Set_Text_useGUILayout},
            {"Set_Text_enabled" ,Set_Text_enabled},
            {"Set_GameObjectActive_IsActive" ,Set_GameObjectActive_IsActive},
            {"Set_GameObjectActive_IsDeActive" ,Set_GameObjectActive_IsDeActive},
            {"Set_GameObjectActive_useGUILayout" ,Set_GameObjectActive_useGUILayout},
            {"Set_GameObjectActive_enabled" ,Set_GameObjectActive_enabled},
            {"Set_Button_interactable" ,Set_Button_interactable},
            {"Set_Button_useGUILayout" ,Set_Button_useGUILayout},
            {"Set_Button_enabled" ,Set_Button_enabled},
            {"Set_EmptyGraphics_raycastTarget" ,Set_EmptyGraphics_raycastTarget},
            {"Set_EmptyGraphics_useGUILayout" ,Set_EmptyGraphics_useGUILayout},
            {"Set_EmptyGraphics_enabled" ,Set_EmptyGraphics_enabled},
            {"Set_WText_IsUseMultiLang" ,Set_WText_IsUseMultiLang},
            {"Set_WText_supportRichText" ,Set_WText_supportRichText},
            {"Set_WText_resizeTextForBestFit" ,Set_WText_resizeTextForBestFit},
            {"Set_WText_alignByGeometry" ,Set_WText_alignByGeometry},
            {"Set_WText_maskable" ,Set_WText_maskable},
            {"Set_WText_isMaskingGraphic" ,Set_WText_isMaskingGraphic},
            {"Set_WText_raycastTarget" ,Set_WText_raycastTarget},
            {"Set_WText_useGUILayout" ,Set_WText_useGUILayout},
            {"Set_WText_enabled" ,Set_WText_enabled},
            {"Set_ContentSizeFitter_useGUILayout" ,Set_ContentSizeFitter_useGUILayout},
            {"Set_ContentSizeFitter_enabled" ,Set_ContentSizeFitter_enabled},
            {"Set_LayoutElement_ignoreLayout" ,Set_LayoutElement_ignoreLayout},
            {"Set_LayoutElement_useGUILayout" ,Set_LayoutElement_useGUILayout},
            {"Set_LayoutElement_enabled" ,Set_LayoutElement_enabled},
            {"Set_RawImage_maskable" ,Set_RawImage_maskable},
            {"Set_RawImage_isMaskingGraphic" ,Set_RawImage_isMaskingGraphic},
            {"Set_RawImage_raycastTarget" ,Set_RawImage_raycastTarget},
            {"Set_RawImage_useGUILayout" ,Set_RawImage_useGUILayout},
            {"Set_RawImage_enabled" ,Set_RawImage_enabled},
            {"Set_ScrollRect_horizontal" ,Set_ScrollRect_horizontal},
            {"Set_ScrollRect_vertical" ,Set_ScrollRect_vertical},
            {"Set_ScrollRect_inertia" ,Set_ScrollRect_inertia},
            {"Set_ScrollRect_useGUILayout" ,Set_ScrollRect_useGUILayout},
            {"Set_ScrollRect_enabled" ,Set_ScrollRect_enabled},
            {"Set_Mask_showMaskGraphic" ,Set_Mask_showMaskGraphic},
            {"Set_Mask_useGUILayout" ,Set_Mask_useGUILayout},
            {"Set_Mask_enabled" ,Set_Mask_enabled},
            {"Set_HorizontalLayoutGroup_childForceExpandWidth" ,Set_HorizontalLayoutGroup_childForceExpandWidth},
            {"Set_HorizontalLayoutGroup_childForceExpandHeight" ,Set_HorizontalLayoutGroup_childForceExpandHeight},
            {"Set_HorizontalLayoutGroup_childControlWidth" ,Set_HorizontalLayoutGroup_childControlWidth},
            {"Set_HorizontalLayoutGroup_childControlHeight" ,Set_HorizontalLayoutGroup_childControlHeight},
            {"Set_HorizontalLayoutGroup_childScaleWidth" ,Set_HorizontalLayoutGroup_childScaleWidth},
            {"Set_HorizontalLayoutGroup_childScaleHeight" ,Set_HorizontalLayoutGroup_childScaleHeight},
            {"Set_HorizontalLayoutGroup_useGUILayout" ,Set_HorizontalLayoutGroup_useGUILayout},
            {"Set_HorizontalLayoutGroup_enabled" ,Set_HorizontalLayoutGroup_enabled},
            {"Set_CanvasGroup_interactable" ,Set_CanvasGroup_interactable},
            {"Set_CanvasGroup_blocksRaycasts" ,Set_CanvasGroup_blocksRaycasts},
            {"Set_CanvasGroup_ignoreParentGroups" ,Set_CanvasGroup_ignoreParentGroups},
            {"Set_CanvasGroup_enabled" ,Set_CanvasGroup_enabled},
            {"Set_GraphicRaycaster_ignoreReversedGraphics" ,Set_GraphicRaycaster_ignoreReversedGraphics},
            {"Set_GraphicRaycaster_useGUILayout" ,Set_GraphicRaycaster_useGUILayout},
            {"Set_GraphicRaycaster_enabled" ,Set_GraphicRaycaster_enabled},
            {"Set_Canvas_overridePixelPerfect" ,Set_Canvas_overridePixelPerfect},
            {"Set_Canvas_pixelPerfect" ,Set_Canvas_pixelPerfect},
            {"Set_Canvas_overrideSorting" ,Set_Canvas_overrideSorting},
            {"Set_Canvas_enabled" ,Set_Canvas_enabled},
            {"Set_GridLayoutGroup_useGUILayout" ,Set_GridLayoutGroup_useGUILayout},
            {"Set_GridLayoutGroup_enabled" ,Set_GridLayoutGroup_enabled},
            {"Set_UIArrayPool_useGUILayout" ,Set_UIArrayPool_useGUILayout},
            {"Set_UIArrayPool_enabled" ,Set_UIArrayPool_enabled},
            {"Set_DataBindingRetrive_useGUILayout" ,Set_DataBindingRetrive_useGUILayout},
            {"Set_DataBindingRetrive_enabled" ,Set_DataBindingRetrive_enabled},
            {"Set_VerticalLayoutGroup_childForceExpandWidth" ,Set_VerticalLayoutGroup_childForceExpandWidth},
            {"Set_VerticalLayoutGroup_childForceExpandHeight" ,Set_VerticalLayoutGroup_childForceExpandHeight},
            {"Set_VerticalLayoutGroup_childControlWidth" ,Set_VerticalLayoutGroup_childControlWidth},
            {"Set_VerticalLayoutGroup_childControlHeight" ,Set_VerticalLayoutGroup_childControlHeight},
            {"Set_VerticalLayoutGroup_childScaleWidth" ,Set_VerticalLayoutGroup_childScaleWidth},
            {"Set_VerticalLayoutGroup_childScaleHeight" ,Set_VerticalLayoutGroup_childScaleHeight},
            {"Set_VerticalLayoutGroup_useGUILayout" ,Set_VerticalLayoutGroup_useGUILayout},
            {"Set_VerticalLayoutGroup_enabled" ,Set_VerticalLayoutGroup_enabled},
            {"Set_ScreenAdapt_useGUILayout" ,Set_ScreenAdapt_useGUILayout},
            {"Set_ScreenAdapt_enabled" ,Set_ScreenAdapt_enabled},
            {"Set_GrayGraphics_IsGray" ,Set_GrayGraphics_IsGray},
            {"Set_GrayGraphics_IsGrayDisable" ,Set_GrayGraphics_IsGrayDisable},
            {"Set_GrayGraphics_useGUILayout" ,Set_GrayGraphics_useGUILayout},
            {"Set_GrayGraphics_enabled" ,Set_GrayGraphics_enabled},
            {"Set_ImageReplace_IsUseMultiLang" ,Set_ImageReplace_IsUseMultiLang},
            {"Set_ImageReplace_useGUILayout" ,Set_ImageReplace_useGUILayout},
            {"Set_ImageReplace_enabled" ,Set_ImageReplace_enabled},
            {"Set_LoopVerticalScrollRect_IsPlayCellAnimation" ,Set_LoopVerticalScrollRect_IsPlayCellAnimation},
            {"Set_LoopVerticalScrollRect_UpdateEnable" ,Set_LoopVerticalScrollRect_UpdateEnable},
            {"Set_LoopVerticalScrollRect_horizontal" ,Set_LoopVerticalScrollRect_horizontal},
            {"Set_LoopVerticalScrollRect_vertical" ,Set_LoopVerticalScrollRect_vertical},
            {"Set_LoopVerticalScrollRect_inertia" ,Set_LoopVerticalScrollRect_inertia},
            {"Set_LoopVerticalScrollRect_isNeedHideRefresh" ,Set_LoopVerticalScrollRect_isNeedHideRefresh},
            {"Set_LoopVerticalScrollRect_useGUILayout" ,Set_LoopVerticalScrollRect_useGUILayout},
            {"Set_LoopVerticalScrollRect_enabled" ,Set_LoopVerticalScrollRect_enabled},
            {"Set_LoopHorizontalScrollRect_IsPlayCellAnimation" ,Set_LoopHorizontalScrollRect_IsPlayCellAnimation},
            {"Set_LoopHorizontalScrollRect_UpdateEnable" ,Set_LoopHorizontalScrollRect_UpdateEnable},
            {"Set_LoopHorizontalScrollRect_horizontal" ,Set_LoopHorizontalScrollRect_horizontal},
            {"Set_LoopHorizontalScrollRect_vertical" ,Set_LoopHorizontalScrollRect_vertical},
            {"Set_LoopHorizontalScrollRect_inertia" ,Set_LoopHorizontalScrollRect_inertia},
            {"Set_LoopHorizontalScrollRect_isNeedHideRefresh" ,Set_LoopHorizontalScrollRect_isNeedHideRefresh},
            {"Set_LoopHorizontalScrollRect_useGUILayout" ,Set_LoopHorizontalScrollRect_useGUILayout},
            {"Set_LoopHorizontalScrollRect_enabled" ,Set_LoopHorizontalScrollRect_enabled},
            {"Set_ViewModelDataSourceArray_useGUILayout" ,Set_ViewModelDataSourceArray_useGUILayout},
            {"Set_ViewModelDataSourceArray_enabled" ,Set_ViewModelDataSourceArray_enabled},
            {"Set_ViewModelDataSourceList_useGUILayout" ,Set_ViewModelDataSourceList_useGUILayout},
            {"Set_ViewModelDataSourceList_enabled" ,Set_ViewModelDataSourceList_enabled},
            {"Set_CanvasScaler_useGUILayout" ,Set_CanvasScaler_useGUILayout},
            {"Set_CanvasScaler_enabled" ,Set_CanvasScaler_enabled},
            {"Set_EventSystem_sendNavigationEvents" ,Set_EventSystem_sendNavigationEvents},
            {"Set_EventSystem_useGUILayout" ,Set_EventSystem_useGUILayout},
            {"Set_EventSystem_enabled" ,Set_EventSystem_enabled},
            {"Set_UIRoot_useGUILayout" ,Set_UIRoot_useGUILayout},
            {"Set_UIRoot_enabled" ,Set_UIRoot_enabled},
            {"Set_TextOutlineCircle_useGraphicAlpha" ,Set_TextOutlineCircle_useGraphicAlpha},
            {"Set_TextOutlineCircle_useGUILayout" ,Set_TextOutlineCircle_useGUILayout},
            {"Set_TextOutlineCircle_enabled" ,Set_TextOutlineCircle_enabled},
            {"Set_RawImageReplace_useGUILayout" ,Set_RawImageReplace_useGUILayout},
            {"Set_RawImageReplace_enabled" ,Set_RawImageReplace_enabled},
            {"Set_AnchorSafeArea_useGUILayout" ,Set_AnchorSafeArea_useGUILayout},
            {"Set_AnchorSafeArea_enabled" ,Set_AnchorSafeArea_enabled},
            {"Set_GlobalMessageBox_useGUILayout" ,Set_GlobalMessageBox_useGUILayout},
            {"Set_GlobalMessageBox_enabled" ,Set_GlobalMessageBox_enabled},
            {"Set_UIWait_useGUILayout" ,Set_UIWait_useGUILayout},
            {"Set_UIWait_enabled" ,Set_UIWait_enabled},
            {"Set_Toast_useGUILayout" ,Set_Toast_useGUILayout},
            {"Set_Toast_enabled" ,Set_Toast_enabled},
            {"Set_RectTransformSizeFollow_useGUILayout" ,Set_RectTransformSizeFollow_useGUILayout},
            {"Set_RectTransformSizeFollow_enabled" ,Set_RectTransformSizeFollow_enabled},
            {"Set_EventSystemTransmiter_raycastTarget" ,Set_EventSystemTransmiter_raycastTarget},
            {"Set_EventSystemTransmiter_useGUILayout" ,Set_EventSystemTransmiter_useGUILayout},
            {"Set_EventSystemTransmiter_enabled" ,Set_EventSystemTransmiter_enabled},
            {"Set_Slider_wholeNumbers" ,Set_Slider_wholeNumbers},
            {"Set_Slider_interactable" ,Set_Slider_interactable},
            {"Set_Slider_useGUILayout" ,Set_Slider_useGUILayout},
            {"Set_Slider_enabled" ,Set_Slider_enabled},
            {"Set_Toggle_isOn" ,Set_Toggle_isOn},
            {"Set_Toggle_interactable" ,Set_Toggle_interactable},
            {"Set_Toggle_useGUILayout" ,Set_Toggle_useGUILayout},
            {"Set_Toggle_enabled" ,Set_Toggle_enabled},
            {"Set_TabButton_isOn" ,Set_TabButton_isOn},
            {"Set_TabButton_interactable" ,Set_TabButton_interactable},
            {"Set_TabButton_useGUILayout" ,Set_TabButton_useGUILayout},
            {"Set_TabButton_enabled" ,Set_TabButton_enabled},
            {"Set_TextSpace_useGUILayout" ,Set_TextSpace_useGUILayout},
            {"Set_TextSpace_enabled" ,Set_TextSpace_enabled},
            {"Set_RectMask2D_useGUILayout" ,Set_RectMask2D_useGUILayout},
            {"Set_RectMask2D_enabled" ,Set_RectMask2D_enabled},
            {"Set_LetterSpacing_useGUILayout" ,Set_LetterSpacing_useGUILayout},
            {"Set_LetterSpacing_enabled" ,Set_LetterSpacing_enabled},
            {"Set_Outline_useGraphicAlpha" ,Set_Outline_useGraphicAlpha},
            {"Set_Outline_useGUILayout" ,Set_Outline_useGUILayout},
            {"Set_Outline_enabled" ,Set_Outline_enabled},
            {"Set_TextOutline8_useGraphicAlpha" ,Set_TextOutline8_useGraphicAlpha},
            {"Set_TextOutline8_useGUILayout" ,Set_TextOutline8_useGUILayout},
            {"Set_TextOutline8_enabled" ,Set_TextOutline8_enabled},
            {"Set_SoftMaskable_useGUILayout" ,Set_SoftMaskable_useGUILayout},
            {"Set_SoftMaskable_enabled" ,Set_SoftMaskable_enabled},
            {"Set_NoRegularImageAlpha_useGUILayout" ,Set_NoRegularImageAlpha_useGUILayout},
            {"Set_NoRegularImageAlpha_enabled" ,Set_NoRegularImageAlpha_enabled},
            {"Set_EventTrigger_useGUILayout" ,Set_EventTrigger_useGUILayout},
            {"Set_EventTrigger_enabled" ,Set_EventTrigger_enabled},
            {"Set_AnimationCell_useGUILayout" ,Set_AnimationCell_useGUILayout},
            {"Set_AnimationCell_enabled" ,Set_AnimationCell_enabled},
            {"Set_SoftMask_useGUILayout" ,Set_SoftMask_useGUILayout},
            {"Set_SoftMask_enabled" ,Set_SoftMask_enabled},
            {"Set_PressButton_interactable" ,Set_PressButton_interactable},
            {"Set_PressButton_useGUILayout" ,Set_PressButton_useGUILayout},
            {"Set_PressButton_enabled" ,Set_PressButton_enabled},
            {"Set_ScreenAdapt_UnSafeArea_useGUILayout" ,Set_ScreenAdapt_UnSafeArea_useGUILayout},
            {"Set_ScreenAdapt_UnSafeArea_enabled" ,Set_ScreenAdapt_UnSafeArea_enabled},
            {"Set_DragScrollRect_useGUILayout" ,Set_DragScrollRect_useGUILayout},
            {"Set_DragScrollRect_enabled" ,Set_DragScrollRect_enabled},
            {"Set_Scrollbar_interactable" ,Set_Scrollbar_interactable},
            {"Set_Scrollbar_useGUILayout" ,Set_Scrollbar_useGUILayout},
            {"Set_Scrollbar_enabled" ,Set_Scrollbar_enabled},
            {"Set_ViewModelDataSourceFancyList_useGUILayout" ,Set_ViewModelDataSourceFancyList_useGUILayout},
            {"Set_ViewModelDataSourceFancyList_enabled" ,Set_ViewModelDataSourceFancyList_enabled},
            {"Set_FancyBindingScrollView_useGUILayout" ,Set_FancyBindingScrollView_useGUILayout},
            {"Set_FancyBindingScrollView_enabled" ,Set_FancyBindingScrollView_enabled},
            {"Set_AspectRatioFitter_useGUILayout" ,Set_AspectRatioFitter_useGUILayout},
            {"Set_AspectRatioFitter_enabled" ,Set_AspectRatioFitter_enabled},
            {"Set_UIMaterialModifier_useGUILayout" ,Set_UIMaterialModifier_useGUILayout},
            {"Set_UIMaterialModifier_enabled" ,Set_UIMaterialModifier_enabled},
            {"Set_Scroller_Inertia" ,Set_Scroller_Inertia},
            {"Set_Scroller_SnapEnabled" ,Set_Scroller_SnapEnabled},
            {"Set_Scroller_Draggable" ,Set_Scroller_Draggable},
            {"Set_Scroller_useGUILayout" ,Set_Scroller_useGUILayout},
            {"Set_Scroller_enabled" ,Set_Scroller_enabled},
            {"Set_FancyBindingCell_useGUILayout" ,Set_FancyBindingCell_useGUILayout},
            {"Set_FancyBindingCell_enabled" ,Set_FancyBindingCell_enabled},
            {"Set_TextAlignmentAdapter_useGUILayout" ,Set_TextAlignmentAdapter_useGUILayout},
            {"Set_TextAlignmentAdapter_enabled" ,Set_TextAlignmentAdapter_enabled},
            {"Set_InputField_shouldHideMobileInput" ,Set_InputField_shouldHideMobileInput},
            {"Set_InputField_customCaretColor" ,Set_InputField_customCaretColor},
            {"Set_InputField_readOnly" ,Set_InputField_readOnly},
            {"Set_InputField_interactable" ,Set_InputField_interactable},
            {"Set_InputField_useGUILayout" ,Set_InputField_useGUILayout},
            {"Set_InputField_enabled" ,Set_InputField_enabled},
            {"Set_Book_useGUILayout" ,Set_Book_useGUILayout},
            {"Set_Book_enabled" ,Set_Book_enabled},
            {"Set_AutoFlip_useGUILayout" ,Set_AutoFlip_useGUILayout},
            {"Set_AutoFlip_enabled" ,Set_AutoFlip_enabled},
            {"Set_SetAsLastSibling_useGUILayout" ,Set_SetAsLastSibling_useGUILayout},
            {"Set_SetAsLastSibling_enabled" ,Set_SetAsLastSibling_enabled},
            {"Set_ReflectionProbe_hdr" ,Set_ReflectionProbe_hdr},
            {"Set_ReflectionProbe_boxProjection" ,Set_ReflectionProbe_boxProjection},
            {"Set_ReflectionProbe_enabled" ,Set_ReflectionProbe_enabled},
            {"Set_TrailRenderer_autodestruct" ,Set_TrailRenderer_autodestruct},
            {"Set_TrailRenderer_emitting" ,Set_TrailRenderer_emitting},
            {"Set_TrailRenderer_generateLightingData" ,Set_TrailRenderer_generateLightingData},
            {"Set_TrailRenderer_enabled" ,Set_TrailRenderer_enabled},
            {"Set_TrailRenderer_receiveShadows" ,Set_TrailRenderer_receiveShadows},
            {"Set_TrailRenderer_forceRenderingOff" ,Set_TrailRenderer_forceRenderingOff},
            {"Set_TrailRenderer_allowOcclusionWhenDynamic" ,Set_TrailRenderer_allowOcclusionWhenDynamic},
            {"Set_SkinnedMeshRenderer_updateWhenOffscreen" ,Set_SkinnedMeshRenderer_updateWhenOffscreen},
            {"Set_SkinnedMeshRenderer_forceMatrixRecalculationPerRender" ,Set_SkinnedMeshRenderer_forceMatrixRecalculationPerRender},
            {"Set_SkinnedMeshRenderer_skinnedMotionVectors" ,Set_SkinnedMeshRenderer_skinnedMotionVectors},
            {"Set_SkinnedMeshRenderer_enabled" ,Set_SkinnedMeshRenderer_enabled},
            {"Set_SkinnedMeshRenderer_receiveShadows" ,Set_SkinnedMeshRenderer_receiveShadows},
            {"Set_SkinnedMeshRenderer_forceRenderingOff" ,Set_SkinnedMeshRenderer_forceRenderingOff},
            {"Set_SkinnedMeshRenderer_allowOcclusionWhenDynamic" ,Set_SkinnedMeshRenderer_allowOcclusionWhenDynamic},
            {"Set_Shadow_useGraphicAlpha" ,Set_Shadow_useGraphicAlpha},
            {"Set_Shadow_useGUILayout" ,Set_Shadow_useGUILayout},
            {"Set_Shadow_enabled" ,Set_Shadow_enabled},
        };
        public static Dictionary<string, Action<object, Int32>> mAllFieldSetFuncDict_Int32 = new Dictionary<string, Action<object, Int32>>()
        {
            {"Set_Transform_hierarchyCapacity" ,Set_Transform_hierarchyCapacity},
            {"Set_Camera_cullingMask" ,Set_Camera_cullingMask},
            {"Set_Camera_eventMask" ,Set_Camera_eventMask},
            {"Set_Camera_targetDisplay" ,Set_Camera_targetDisplay},
            {"Set_Image_fillOrigin" ,Set_Image_fillOrigin},
            {"Set_RectTransform_hierarchyCapacity" ,Set_RectTransform_hierarchyCapacity},
            {"Set_Text_resizeTextMinSize" ,Set_Text_resizeTextMinSize},
            {"Set_Text_resizeTextMaxSize" ,Set_Text_resizeTextMaxSize},
            {"Set_Text_fontSize" ,Set_Text_fontSize},
            {"Set_WText_resizeTextMinSize" ,Set_WText_resizeTextMinSize},
            {"Set_WText_resizeTextMaxSize" ,Set_WText_resizeTextMaxSize},
            {"Set_WText_fontSize" ,Set_WText_fontSize},
            {"Set_LayoutElement_layoutPriority" ,Set_LayoutElement_layoutPriority},
            {"Set_Canvas_sortingOrder" ,Set_Canvas_sortingOrder},
            {"Set_Canvas_targetDisplay" ,Set_Canvas_targetDisplay},
            {"Set_Canvas_sortingLayerID" ,Set_Canvas_sortingLayerID},
            {"Set_GridLayoutGroup_constraintCount" ,Set_GridLayoutGroup_constraintCount},
            {"Set_LoopVerticalScrollRect_RefillCellIndex" ,Set_LoopVerticalScrollRect_RefillCellIndex},
            {"Set_LoopHorizontalScrollRect_RefillCellIndex" ,Set_LoopHorizontalScrollRect_RefillCellIndex},
            {"Set_ViewModelDataSourceArray_ParentIndex" ,Set_ViewModelDataSourceArray_ParentIndex},
            {"Set_ViewModelDataSourceList_ParentIndex" ,Set_ViewModelDataSourceList_ParentIndex},
            {"Set_EventSystem_pixelDragThreshold" ,Set_EventSystem_pixelDragThreshold},
            {"Set_Scrollbar_numberOfSteps" ,Set_Scrollbar_numberOfSteps},
            {"Set_ViewModelDataSourceFancyList_ParentIndex" ,Set_ViewModelDataSourceFancyList_ParentIndex},
            {"Set_FancyBindingScrollView_CurrentSelectIndex" ,Set_FancyBindingScrollView_CurrentSelectIndex},
            {"Set_FancyBindingCell_Index" ,Set_FancyBindingCell_Index},
            {"Set_InputField_caretWidth" ,Set_InputField_caretWidth},
            {"Set_InputField_characterLimit" ,Set_InputField_characterLimit},
            {"Set_InputField_caretPosition" ,Set_InputField_caretPosition},
            {"Set_InputField_selectionAnchorPosition" ,Set_InputField_selectionAnchorPosition},
            {"Set_InputField_selectionFocusPosition" ,Set_InputField_selectionFocusPosition},
            {"Set_Book_CurrentPage" ,Set_Book_CurrentPage},
            {"Set_Book_NextPageID" ,Set_Book_NextPageID},
            {"Set_ReflectionProbe_resolution" ,Set_ReflectionProbe_resolution},
            {"Set_ReflectionProbe_cullingMask" ,Set_ReflectionProbe_cullingMask},
            {"Set_ReflectionProbe_importance" ,Set_ReflectionProbe_importance},
            {"Set_TrailRenderer_numCornerVertices" ,Set_TrailRenderer_numCornerVertices},
            {"Set_TrailRenderer_numCapVertices" ,Set_TrailRenderer_numCapVertices},
            {"Set_TrailRenderer_rendererPriority" ,Set_TrailRenderer_rendererPriority},
            {"Set_TrailRenderer_sortingLayerID" ,Set_TrailRenderer_sortingLayerID},
            {"Set_TrailRenderer_sortingOrder" ,Set_TrailRenderer_sortingOrder},
            {"Set_TrailRenderer_lightmapIndex" ,Set_TrailRenderer_lightmapIndex},
            {"Set_TrailRenderer_realtimeLightmapIndex" ,Set_TrailRenderer_realtimeLightmapIndex},
            {"Set_SkinnedMeshRenderer_rendererPriority" ,Set_SkinnedMeshRenderer_rendererPriority},
            {"Set_SkinnedMeshRenderer_sortingLayerID" ,Set_SkinnedMeshRenderer_sortingLayerID},
            {"Set_SkinnedMeshRenderer_sortingOrder" ,Set_SkinnedMeshRenderer_sortingOrder},
            {"Set_SkinnedMeshRenderer_lightmapIndex" ,Set_SkinnedMeshRenderer_lightmapIndex},
            {"Set_SkinnedMeshRenderer_realtimeLightmapIndex" ,Set_SkinnedMeshRenderer_realtimeLightmapIndex},
        };
        public static Dictionary<string, Action<object, Int64>> mAllFieldSetFuncDict_Int64 = new Dictionary<string, Action<object, Int64>>()
        {
        };
        public static Dictionary<string, Action<object, String>> mAllFieldSetFuncDict_String = new Dictionary<string, Action<object, String>>()
        {
            {"Set_Transform_tag" ,Set_Transform_tag},
            {"Set_Transform_name" ,Set_Transform_name},
            {"Set_Behaviour_tag" ,Set_Behaviour_tag},
            {"Set_Behaviour_name" ,Set_Behaviour_name},
            {"Set_FlareLayer_tag" ,Set_FlareLayer_tag},
            {"Set_FlareLayer_name" ,Set_FlareLayer_name},
            {"Set_Camera_tag" ,Set_Camera_tag},
            {"Set_Camera_name" ,Set_Camera_name},
            {"Set_Image_tag" ,Set_Image_tag},
            {"Set_Image_name" ,Set_Image_name},
            {"Set_RectTransform_tag" ,Set_RectTransform_tag},
            {"Set_RectTransform_name" ,Set_RectTransform_name},
            {"Set_Text_text" ,Set_Text_text},
            {"Set_Text_tag" ,Set_Text_tag},
            {"Set_Text_name" ,Set_Text_name},
            {"Set_GameObjectActive_tag" ,Set_GameObjectActive_tag},
            {"Set_GameObjectActive_name" ,Set_GameObjectActive_name},
            {"Set_Button_tag" ,Set_Button_tag},
            {"Set_Button_name" ,Set_Button_name},
            {"Set_EmptyGraphics_tag" ,Set_EmptyGraphics_tag},
            {"Set_EmptyGraphics_name" ,Set_EmptyGraphics_name},
            {"Set_WText_text" ,Set_WText_text},
            {"Set_WText_FontColor" ,Set_WText_FontColor},
            {"Set_WText_tag" ,Set_WText_tag},
            {"Set_WText_name" ,Set_WText_name},
            {"Set_ContentSizeFitter_tag" ,Set_ContentSizeFitter_tag},
            {"Set_ContentSizeFitter_name" ,Set_ContentSizeFitter_name},
            {"Set_LayoutElement_tag" ,Set_LayoutElement_tag},
            {"Set_LayoutElement_name" ,Set_LayoutElement_name},
            {"Set_RawImage_tag" ,Set_RawImage_tag},
            {"Set_RawImage_name" ,Set_RawImage_name},
            {"Set_ScrollRect_tag" ,Set_ScrollRect_tag},
            {"Set_ScrollRect_name" ,Set_ScrollRect_name},
            {"Set_Mask_tag" ,Set_Mask_tag},
            {"Set_Mask_name" ,Set_Mask_name},
            {"Set_HorizontalLayoutGroup_tag" ,Set_HorizontalLayoutGroup_tag},
            {"Set_HorizontalLayoutGroup_name" ,Set_HorizontalLayoutGroup_name},
            {"Set_CanvasGroup_tag" ,Set_CanvasGroup_tag},
            {"Set_CanvasGroup_name" ,Set_CanvasGroup_name},
            {"Set_GraphicRaycaster_tag" ,Set_GraphicRaycaster_tag},
            {"Set_GraphicRaycaster_name" ,Set_GraphicRaycaster_name},
            {"Set_Canvas_sortingLayerName" ,Set_Canvas_sortingLayerName},
            {"Set_Canvas_tag" ,Set_Canvas_tag},
            {"Set_Canvas_name" ,Set_Canvas_name},
            {"Set_GridLayoutGroup_tag" ,Set_GridLayoutGroup_tag},
            {"Set_GridLayoutGroup_name" ,Set_GridLayoutGroup_name},
            {"Set_UIArrayPool_tag" ,Set_UIArrayPool_tag},
            {"Set_UIArrayPool_name" ,Set_UIArrayPool_name},
            {"Set_DataBindingRetrive_tag" ,Set_DataBindingRetrive_tag},
            {"Set_DataBindingRetrive_name" ,Set_DataBindingRetrive_name},
            {"Set_VerticalLayoutGroup_tag" ,Set_VerticalLayoutGroup_tag},
            {"Set_VerticalLayoutGroup_name" ,Set_VerticalLayoutGroup_name},
            {"Set_ScreenAdapt_tag" ,Set_ScreenAdapt_tag},
            {"Set_ScreenAdapt_name" ,Set_ScreenAdapt_name},
            {"Set_GrayGraphics_tag" ,Set_GrayGraphics_tag},
            {"Set_GrayGraphics_name" ,Set_GrayGraphics_name},
            {"Set_ImageReplace_ImgColor" ,Set_ImageReplace_ImgColor},
            {"Set_ImageReplace_SpriteName" ,Set_ImageReplace_SpriteName},
            {"Set_ImageReplace_tag" ,Set_ImageReplace_tag},
            {"Set_ImageReplace_name" ,Set_ImageReplace_name},
            {"Set_LoopVerticalScrollRect_tag" ,Set_LoopVerticalScrollRect_tag},
            {"Set_LoopVerticalScrollRect_name" ,Set_LoopVerticalScrollRect_name},
            {"Set_LoopHorizontalScrollRect_tag" ,Set_LoopHorizontalScrollRect_tag},
            {"Set_LoopHorizontalScrollRect_name" ,Set_LoopHorizontalScrollRect_name},
            {"Set_ViewModelDataSourceArray_tag" ,Set_ViewModelDataSourceArray_tag},
            {"Set_ViewModelDataSourceArray_name" ,Set_ViewModelDataSourceArray_name},
            {"Set_ViewModelDataSourceList_tag" ,Set_ViewModelDataSourceList_tag},
            {"Set_ViewModelDataSourceList_name" ,Set_ViewModelDataSourceList_name},
            {"Set_CanvasScaler_tag" ,Set_CanvasScaler_tag},
            {"Set_CanvasScaler_name" ,Set_CanvasScaler_name},
            {"Set_EventSystem_tag" ,Set_EventSystem_tag},
            {"Set_EventSystem_name" ,Set_EventSystem_name},
            {"Set_UIRoot_tag" ,Set_UIRoot_tag},
            {"Set_UIRoot_name" ,Set_UIRoot_name},
            {"Set_MeshFilter_tag" ,Set_MeshFilter_tag},
            {"Set_MeshFilter_name" ,Set_MeshFilter_name},
            {"Set_TextOutlineCircle_tag" ,Set_TextOutlineCircle_tag},
            {"Set_TextOutlineCircle_name" ,Set_TextOutlineCircle_name},
            {"Set_RawImageReplace_SpriteName" ,Set_RawImageReplace_SpriteName},
            {"Set_RawImageReplace_tag" ,Set_RawImageReplace_tag},
            {"Set_RawImageReplace_name" ,Set_RawImageReplace_name},
            {"Set_AnchorSafeArea_tag" ,Set_AnchorSafeArea_tag},
            {"Set_AnchorSafeArea_name" ,Set_AnchorSafeArea_name},
            {"Set_GlobalMessageBox_tag" ,Set_GlobalMessageBox_tag},
            {"Set_GlobalMessageBox_name" ,Set_GlobalMessageBox_name},
            {"Set_UIWait_tag" ,Set_UIWait_tag},
            {"Set_UIWait_name" ,Set_UIWait_name},
            {"Set_Toast_tag" ,Set_Toast_tag},
            {"Set_Toast_name" ,Set_Toast_name},
            {"Set_RectTransformSizeFollow_tag" ,Set_RectTransformSizeFollow_tag},
            {"Set_RectTransformSizeFollow_name" ,Set_RectTransformSizeFollow_name},
            {"Set_EventSystemTransmiter_tag" ,Set_EventSystemTransmiter_tag},
            {"Set_EventSystemTransmiter_name" ,Set_EventSystemTransmiter_name},
            {"Set_Slider_tag" ,Set_Slider_tag},
            {"Set_Slider_name" ,Set_Slider_name},
            {"Set_Toggle_tag" ,Set_Toggle_tag},
            {"Set_Toggle_name" ,Set_Toggle_name},
            {"Set_TabButton_tag" ,Set_TabButton_tag},
            {"Set_TabButton_name" ,Set_TabButton_name},
            {"Set_TextSpace_tag" ,Set_TextSpace_tag},
            {"Set_TextSpace_name" ,Set_TextSpace_name},
            {"Set_RectMask2D_tag" ,Set_RectMask2D_tag},
            {"Set_RectMask2D_name" ,Set_RectMask2D_name},
            {"Set_LetterSpacing_tag" ,Set_LetterSpacing_tag},
            {"Set_LetterSpacing_name" ,Set_LetterSpacing_name},
            {"Set_Outline_tag" ,Set_Outline_tag},
            {"Set_Outline_name" ,Set_Outline_name},
            {"Set_TextOutline8_tag" ,Set_TextOutline8_tag},
            {"Set_TextOutline8_name" ,Set_TextOutline8_name},
            {"Set_SoftMaskable_tag" ,Set_SoftMaskable_tag},
            {"Set_SoftMaskable_name" ,Set_SoftMaskable_name},
            {"Set_NoRegularImageAlpha_tag" ,Set_NoRegularImageAlpha_tag},
            {"Set_NoRegularImageAlpha_name" ,Set_NoRegularImageAlpha_name},
            {"Set_EventTrigger_tag" ,Set_EventTrigger_tag},
            {"Set_EventTrigger_name" ,Set_EventTrigger_name},
            {"Set_AnimationCell_tag" ,Set_AnimationCell_tag},
            {"Set_AnimationCell_name" ,Set_AnimationCell_name},
            {"Set_SoftMask_tag" ,Set_SoftMask_tag},
            {"Set_SoftMask_name" ,Set_SoftMask_name},
            {"Set_PressButton_tag" ,Set_PressButton_tag},
            {"Set_PressButton_name" ,Set_PressButton_name},
            {"Set_ScreenAdapt_UnSafeArea_tag" ,Set_ScreenAdapt_UnSafeArea_tag},
            {"Set_ScreenAdapt_UnSafeArea_name" ,Set_ScreenAdapt_UnSafeArea_name},
            {"Set_DragScrollRect_tag" ,Set_DragScrollRect_tag},
            {"Set_DragScrollRect_name" ,Set_DragScrollRect_name},
            {"Set_Scrollbar_tag" ,Set_Scrollbar_tag},
            {"Set_Scrollbar_name" ,Set_Scrollbar_name},
            {"Set_ViewModelDataSourceFancyList_tag" ,Set_ViewModelDataSourceFancyList_tag},
            {"Set_ViewModelDataSourceFancyList_name" ,Set_ViewModelDataSourceFancyList_name},
            {"Set_FancyBindingScrollView_tag" ,Set_FancyBindingScrollView_tag},
            {"Set_FancyBindingScrollView_name" ,Set_FancyBindingScrollView_name},
            {"Set_AspectRatioFitter_tag" ,Set_AspectRatioFitter_tag},
            {"Set_AspectRatioFitter_name" ,Set_AspectRatioFitter_name},
            {"Set_UIMaterialModifier_tag" ,Set_UIMaterialModifier_tag},
            {"Set_UIMaterialModifier_name" ,Set_UIMaterialModifier_name},
            {"Set_Scroller_tag" ,Set_Scroller_tag},
            {"Set_Scroller_name" ,Set_Scroller_name},
            {"Set_FancyBindingCell_tag" ,Set_FancyBindingCell_tag},
            {"Set_FancyBindingCell_name" ,Set_FancyBindingCell_name},
            {"Set_TextAlignmentAdapter_tag" ,Set_TextAlignmentAdapter_tag},
            {"Set_TextAlignmentAdapter_name" ,Set_TextAlignmentAdapter_name},
            {"Set_InputField_text" ,Set_InputField_text},
            {"Set_InputField_tag" ,Set_InputField_tag},
            {"Set_InputField_name" ,Set_InputField_name},
            {"Set_Book_tag" ,Set_Book_tag},
            {"Set_Book_name" ,Set_Book_name},
            {"Set_AutoFlip_tag" ,Set_AutoFlip_tag},
            {"Set_AutoFlip_name" ,Set_AutoFlip_name},
            {"Set_SetAsLastSibling_tag" ,Set_SetAsLastSibling_tag},
            {"Set_SetAsLastSibling_name" ,Set_SetAsLastSibling_name},
            {"Set_ReflectionProbe_tag" ,Set_ReflectionProbe_tag},
            {"Set_ReflectionProbe_name" ,Set_ReflectionProbe_name},
            {"Set_TrailRenderer_sortingLayerName" ,Set_TrailRenderer_sortingLayerName},
            {"Set_TrailRenderer_tag" ,Set_TrailRenderer_tag},
            {"Set_TrailRenderer_name" ,Set_TrailRenderer_name},
            {"Set_SkinnedMeshRenderer_sortingLayerName" ,Set_SkinnedMeshRenderer_sortingLayerName},
            {"Set_SkinnedMeshRenderer_tag" ,Set_SkinnedMeshRenderer_tag},
            {"Set_SkinnedMeshRenderer_name" ,Set_SkinnedMeshRenderer_name},
            {"Set_Shadow_tag" ,Set_Shadow_tag},
            {"Set_Shadow_name" ,Set_Shadow_name},
        };
        public static Dictionary<string, Action<object, Single>> mAllFieldSetFuncDict_Single = new Dictionary<string, Action<object, Single>>()
        {
            {"Set_Camera_nearClipPlane" ,Set_Camera_nearClipPlane},
            {"Set_Camera_farClipPlane" ,Set_Camera_farClipPlane},
            {"Set_Camera_fieldOfView" ,Set_Camera_fieldOfView},
            {"Set_Camera_orthographicSize" ,Set_Camera_orthographicSize},
            {"Set_Camera_depth" ,Set_Camera_depth},
            {"Set_Camera_aspect" ,Set_Camera_aspect},
            {"Set_Camera_focalLength" ,Set_Camera_focalLength},
            {"Set_Camera_stereoSeparation" ,Set_Camera_stereoSeparation},
            {"Set_Camera_stereoConvergence" ,Set_Camera_stereoConvergence},
            {"Set_Image_fillAmount" ,Set_Image_fillAmount},
            {"Set_Image_alphaHitTestMinimumThreshold" ,Set_Image_alphaHitTestMinimumThreshold},
            {"Set_Image_pixelsPerUnitMultiplier" ,Set_Image_pixelsPerUnitMultiplier},
            {"Set_Text_lineSpacing" ,Set_Text_lineSpacing},
            {"Set_WText_lineSpacing" ,Set_WText_lineSpacing},
            {"Set_LayoutElement_minWidth" ,Set_LayoutElement_minWidth},
            {"Set_LayoutElement_minHeight" ,Set_LayoutElement_minHeight},
            {"Set_LayoutElement_preferredWidth" ,Set_LayoutElement_preferredWidth},
            {"Set_LayoutElement_preferredHeight" ,Set_LayoutElement_preferredHeight},
            {"Set_LayoutElement_flexibleWidth" ,Set_LayoutElement_flexibleWidth},
            {"Set_LayoutElement_flexibleHeight" ,Set_LayoutElement_flexibleHeight},
            {"Set_ScrollRect_elasticity" ,Set_ScrollRect_elasticity},
            {"Set_ScrollRect_decelerationRate" ,Set_ScrollRect_decelerationRate},
            {"Set_ScrollRect_scrollSensitivity" ,Set_ScrollRect_scrollSensitivity},
            {"Set_ScrollRect_horizontalScrollbarSpacing" ,Set_ScrollRect_horizontalScrollbarSpacing},
            {"Set_ScrollRect_verticalScrollbarSpacing" ,Set_ScrollRect_verticalScrollbarSpacing},
            {"Set_ScrollRect_horizontalNormalizedPosition" ,Set_ScrollRect_horizontalNormalizedPosition},
            {"Set_ScrollRect_verticalNormalizedPosition" ,Set_ScrollRect_verticalNormalizedPosition},
            {"Set_HorizontalLayoutGroup_spacing" ,Set_HorizontalLayoutGroup_spacing},
            {"Set_CanvasGroup_alpha" ,Set_CanvasGroup_alpha},
            {"Set_Canvas_scaleFactor" ,Set_Canvas_scaleFactor},
            {"Set_Canvas_referencePixelsPerUnit" ,Set_Canvas_referencePixelsPerUnit},
            {"Set_Canvas_planeDistance" ,Set_Canvas_planeDistance},
            {"Set_Canvas_normalizedSortingGridSize" ,Set_Canvas_normalizedSortingGridSize},
            {"Set_VerticalLayoutGroup_spacing" ,Set_VerticalLayoutGroup_spacing},
            {"Set_GrayGraphics_DarkNessValue" ,Set_GrayGraphics_DarkNessValue},
            {"Set_GrayGraphics_GrayFactor" ,Set_GrayGraphics_GrayFactor},
            {"Set_LoopVerticalScrollRect_AnimationTotalDelayTime" ,Set_LoopVerticalScrollRect_AnimationTotalDelayTime},
            {"Set_LoopVerticalScrollRect_elasticity" ,Set_LoopVerticalScrollRect_elasticity},
            {"Set_LoopVerticalScrollRect_decelerationRate" ,Set_LoopVerticalScrollRect_decelerationRate},
            {"Set_LoopVerticalScrollRect_scrollSensitivity" ,Set_LoopVerticalScrollRect_scrollSensitivity},
            {"Set_LoopVerticalScrollRect_horizontalScrollbarSpacing" ,Set_LoopVerticalScrollRect_horizontalScrollbarSpacing},
            {"Set_LoopVerticalScrollRect_verticalScrollbarSpacing" ,Set_LoopVerticalScrollRect_verticalScrollbarSpacing},
            {"Set_LoopVerticalScrollRect_horizontalNormalizedPosition" ,Set_LoopVerticalScrollRect_horizontalNormalizedPosition},
            {"Set_LoopVerticalScrollRect_verticalNormalizedPosition" ,Set_LoopVerticalScrollRect_verticalNormalizedPosition},
            {"Set_LoopHorizontalScrollRect_AnimationTotalDelayTime" ,Set_LoopHorizontalScrollRect_AnimationTotalDelayTime},
            {"Set_LoopHorizontalScrollRect_elasticity" ,Set_LoopHorizontalScrollRect_elasticity},
            {"Set_LoopHorizontalScrollRect_decelerationRate" ,Set_LoopHorizontalScrollRect_decelerationRate},
            {"Set_LoopHorizontalScrollRect_scrollSensitivity" ,Set_LoopHorizontalScrollRect_scrollSensitivity},
            {"Set_LoopHorizontalScrollRect_horizontalScrollbarSpacing" ,Set_LoopHorizontalScrollRect_horizontalScrollbarSpacing},
            {"Set_LoopHorizontalScrollRect_verticalScrollbarSpacing" ,Set_LoopHorizontalScrollRect_verticalScrollbarSpacing},
            {"Set_LoopHorizontalScrollRect_horizontalNormalizedPosition" ,Set_LoopHorizontalScrollRect_horizontalNormalizedPosition},
            {"Set_LoopHorizontalScrollRect_verticalNormalizedPosition" ,Set_LoopHorizontalScrollRect_verticalNormalizedPosition},
            {"Set_CanvasScaler_referencePixelsPerUnit" ,Set_CanvasScaler_referencePixelsPerUnit},
            {"Set_CanvasScaler_scaleFactor" ,Set_CanvasScaler_scaleFactor},
            {"Set_CanvasScaler_matchWidthOrHeight" ,Set_CanvasScaler_matchWidthOrHeight},
            {"Set_CanvasScaler_fallbackScreenDPI" ,Set_CanvasScaler_fallbackScreenDPI},
            {"Set_CanvasScaler_defaultSpriteDPI" ,Set_CanvasScaler_defaultSpriteDPI},
            {"Set_CanvasScaler_dynamicPixelsPerUnit" ,Set_CanvasScaler_dynamicPixelsPerUnit},
            {"Set_Slider_minValue" ,Set_Slider_minValue},
            {"Set_Slider_maxValue" ,Set_Slider_maxValue},
            {"Set_Slider_value" ,Set_Slider_value},
            {"Set_Slider_normalizedValue" ,Set_Slider_normalizedValue},
            {"Set_LetterSpacing_spacing" ,Set_LetterSpacing_spacing},
            {"Set_Scrollbar_value" ,Set_Scrollbar_value},
            {"Set_Scrollbar_size" ,Set_Scrollbar_size},
            {"Set_FancyBindingScrollView_ScrollDuration" ,Set_FancyBindingScrollView_ScrollDuration},
            {"Set_AspectRatioFitter_aspectRatio" ,Set_AspectRatioFitter_aspectRatio},
            {"Set_Scroller_Elasticity" ,Set_Scroller_Elasticity},
            {"Set_Scroller_ScrollSensitivity" ,Set_Scroller_ScrollSensitivity},
            {"Set_Scroller_DecelerationRate" ,Set_Scroller_DecelerationRate},
            {"Set_Scroller_Position" ,Set_Scroller_Position},
            {"Set_InputField_caretBlinkRate" ,Set_InputField_caretBlinkRate},
            {"Set_ReflectionProbe_nearClipPlane" ,Set_ReflectionProbe_nearClipPlane},
            {"Set_ReflectionProbe_farClipPlane" ,Set_ReflectionProbe_farClipPlane},
            {"Set_ReflectionProbe_intensity" ,Set_ReflectionProbe_intensity},
            {"Set_ReflectionProbe_shadowDistance" ,Set_ReflectionProbe_shadowDistance},
            {"Set_ReflectionProbe_blendDistance" ,Set_ReflectionProbe_blendDistance},
            {"Set_TrailRenderer_time" ,Set_TrailRenderer_time},
            {"Set_TrailRenderer_startWidth" ,Set_TrailRenderer_startWidth},
            {"Set_TrailRenderer_endWidth" ,Set_TrailRenderer_endWidth},
            {"Set_TrailRenderer_widthMultiplier" ,Set_TrailRenderer_widthMultiplier},
            {"Set_TrailRenderer_minVertexDistance" ,Set_TrailRenderer_minVertexDistance},
            {"Set_TrailRenderer_shadowBias" ,Set_TrailRenderer_shadowBias},
        };
        public static Dictionary<string, Action<object, Color>> mAllFieldSetFuncDict_Color = new Dictionary<string, Action<object, Color>>()
        {
            {"Set_Camera_backgroundColor" ,Set_Camera_backgroundColor},
            {"Set_Image_color" ,Set_Image_color},
            {"Set_Text_color" ,Set_Text_color},
            {"Set_EmptyGraphics_color" ,Set_EmptyGraphics_color},
            {"Set_WText_color" ,Set_WText_color},
            {"Set_RawImage_color" ,Set_RawImage_color},
            {"Set_TextOutlineCircle_effectColor" ,Set_TextOutlineCircle_effectColor},
            {"Set_EventSystemTransmiter_color" ,Set_EventSystemTransmiter_color},
            {"Set_Outline_effectColor" ,Set_Outline_effectColor},
            {"Set_TextOutline8_effectColor" ,Set_TextOutline8_effectColor},
            {"Set_InputField_caretColor" ,Set_InputField_caretColor},
            {"Set_InputField_selectionColor" ,Set_InputField_selectionColor},
            {"Set_ReflectionProbe_backgroundColor" ,Set_ReflectionProbe_backgroundColor},
            {"Set_TrailRenderer_startColor" ,Set_TrailRenderer_startColor},
            {"Set_TrailRenderer_endColor" ,Set_TrailRenderer_endColor},
            {"Set_Shadow_effectColor" ,Set_Shadow_effectColor},
        };
        public static Dictionary<string, Action<object, Vector2>> mAllFieldSetFuncDict_Vector2 = new Dictionary<string, Action<object, Vector2>>()
        {
            {"Set_Camera_sensorSize" ,Set_Camera_sensorSize},
            {"Set_Camera_lensShift" ,Set_Camera_lensShift},
            {"Set_RectTransform_anchorMin" ,Set_RectTransform_anchorMin},
            {"Set_RectTransform_anchorMax" ,Set_RectTransform_anchorMax},
            {"Set_RectTransform_anchoredPosition" ,Set_RectTransform_anchoredPosition},
            {"Set_RectTransform_sizeDelta" ,Set_RectTransform_sizeDelta},
            {"Set_RectTransform_pivot" ,Set_RectTransform_pivot},
            {"Set_RectTransform_offsetMin" ,Set_RectTransform_offsetMin},
            {"Set_RectTransform_offsetMax" ,Set_RectTransform_offsetMax},
            {"Set_ScrollRect_velocity" ,Set_ScrollRect_velocity},
            {"Set_ScrollRect_normalizedPosition" ,Set_ScrollRect_normalizedPosition},
            {"Set_GridLayoutGroup_cellSize" ,Set_GridLayoutGroup_cellSize},
            {"Set_GridLayoutGroup_spacing" ,Set_GridLayoutGroup_spacing},
            {"Set_LoopVerticalScrollRect_velocity" ,Set_LoopVerticalScrollRect_velocity},
            {"Set_LoopVerticalScrollRect_normalizedPosition" ,Set_LoopVerticalScrollRect_normalizedPosition},
            {"Set_LoopHorizontalScrollRect_velocity" ,Set_LoopHorizontalScrollRect_velocity},
            {"Set_LoopHorizontalScrollRect_normalizedPosition" ,Set_LoopHorizontalScrollRect_normalizedPosition},
            {"Set_CanvasScaler_referenceResolution" ,Set_CanvasScaler_referenceResolution},
            {"Set_TextOutlineCircle_effectDistance" ,Set_TextOutlineCircle_effectDistance},
            {"Set_Outline_effectDistance" ,Set_Outline_effectDistance},
            {"Set_TextOutline8_effectDistance" ,Set_TextOutline8_effectDistance},
            {"Set_Shadow_effectDistance" ,Set_Shadow_effectDistance},
        };
        public static bool SetValue(string rFuncName, object rTarget, Vector3 value)
        {
            if (!mAllFieldSetFuncDict_Vector3.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }
        public static bool SetValue(string rFuncName, object rTarget, Boolean value)
        {
            if (!mAllFieldSetFuncDict_Boolean.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }
        public static bool SetValue(string rFuncName, object rTarget, Int32 value)
        {
            if (!mAllFieldSetFuncDict_Int32.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }

        public static bool SetValue(string rFuncName, object rTarget, Int64 value)
        {
            if (!mAllFieldSetFuncDict_Int64.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }
        public static bool SetValue(string rFuncName, object rTarget, String value)
        {
            if (!mAllFieldSetFuncDict_String.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }
        public static bool SetValue(string rFuncName, object rTarget, Single value)
        {
            if (!mAllFieldSetFuncDict_Single.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }
        public static bool SetValue(string rFuncName, object rTarget, Color value)
        {
            if (!mAllFieldSetFuncDict_Color.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }
        public static bool SetValue(string rFuncName, object rTarget, Vector2 value)
        {
            if (!mAllFieldSetFuncDict_Vector2.TryGetValue(rFuncName, out var rFunc))
            {
                LogManager.LogError($"找不到SetValue方法: FuncName = {rFuncName},请告知程序前往ViewBindTool_Hotfix.cs文件中添加方法");
                return false;
            }
            rFunc.Invoke(rTarget, value);
            return true;
        }
    }
}

