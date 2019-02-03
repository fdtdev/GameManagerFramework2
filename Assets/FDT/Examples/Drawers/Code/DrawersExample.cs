using UnityEngine;
using System.Collections;
using com.FDT.Common;

public class DrawersExample : MonoBehaviour {

	public enum EnumAliasExample
	{
		[EnumAlias("alias: NONE")]
		NONE=0, 
		[EnumAlias("alias: ONE")]
		ONE=1, 
		[EnumAlias("alias: TWO")]
		TWO=2, 
		[EnumAlias("alias: THREE")]
		THREE=3
	}
	
	public enum EnumFlagsExample
	{
		NONE=0, ONE=1, TWO=2, FOUR=4, EIGHT=8, SIXTEEN=16
	}

	[PreviewTexture(label="variable: PreviewTexture", tooltip="tooltip: PreviewTexture")]
	public Object PreviewTexture;

	[BHeader("DRAWERS DEMO", "property and decorator drawers examples"), MHeader("PopUpBased: "), AllSceneName(label="variable: AllSceneName", tooltip="tooltip: AllSceneName")]
	public string AllSceneName;
	[SceneName(noPath=true, label="variable: SceneName", tooltip="tooltip: SceneName")]
	public string SceneName;

	[EnumAlias(label="variable: EnumAlias", tooltip="tooltip: EnumAlias")]
	public EnumAliasExample EnumAlias;
	[EnumFlags(label="variable: EnumFlags", tooltip="tooltip: EnumFlags")]
	public EnumFlagsExample EnumFlags;

	[Layer(label="variable: LayerAttribute", tooltip="tooltip: LayerAttribute")]
	public int LayerAttribute;

	[PopupAttribute("option 1", "option 2", "option 3",label="variable: PopupAttribute", tooltip="tooltip: PopupAttribute")]
	public string PopupAttribute;

	[Tag(label="variable: Tag", tooltip="tooltip: Tag")]
	public string Tag;

	[MHeader("Ranges:"), MinMaxRange(5.0f, 20.0f, label="variable: MinMaxRangeFloat", tooltip="tooltip: MinMaxRangeFloat")]
	public Vector2 MinMaxRangeFloat;
	[MinMaxRange(5, 20, label="variable: MinMaxRangeInt", tooltip="tooltip: MinMaxRangeInt")]
	public Vector2 MinMaxRangeInt;

	[MinMaxRange("RMinMaxRangeFloatMin", "RMinMaxRangeFloatMax", label="variable: RMinMaxRangeFloat", tooltip="tooltip: RMinMaxRangeFloat")]
	public Vector2 RMinMaxRangeFloat;
	[MinMaxRange("RMinMaxRangeIntMin", "RMinMaxRangeIntMax", label="variable: RMinMaxRangeInt", tooltip="tooltip: RMinMaxRangeInt")]
	public Vector2 RMinMaxRangeInt;

	public float RMinMaxRangeFloatMin=5.0f;
	public float RMinMaxRangeFloatMax=20.0f;
	public int RMinMaxRangeIntMin=5;
	public int RMinMaxRangeIntMax=20;



	[CustomRange(5.0f, 20.0f, label="variable: FloatCustomRange", tooltip="tooltip: FloatCustomRange")]
	public float FloatCustomRange;
	[CustomRange(5, 20, label="variable: IntCustomRange", tooltip="tooltip: IntCustomRange")]
	public int IntCustomRange;

	[CustomRange("RFloatRangeIntMin","RFloatRangeIntMax", label="variable: FloatCustomRangeReflected", tooltip="tooltip: FloatCustomRangeReflected")]
	public float FloatCustomRangeReflected;
	[CustomRange("RIntRangeIntMin", "RIntRangeIntMax", label="variable: IntCustomRangeReflected", tooltip="tooltip: IntCustomRangeReflected")]
	public int IntCustomRangeReflected;

	public float RFloatRangeIntMin=5.0f;
	public float RFloatRangeIntMax=20.0f;
	public int RIntRangeIntMin=5;
	public int RIntRangeIntMax=20;

	[NotMoreThan(50.0f, label="variable: FloatLessThanFifty", tooltip="tooltip: FloatLessThanFifty")]
	public float FloatLessThanFifty;

	[NotLessThan(50.0f, label="variable: FloatMoreThanFifty", tooltip="tooltip: FloatMoreThanFifty")]
	public float FloatMoreThanFifty;


	[NotMoreThan(50, label="variable: IntLessThanFifty", tooltip="tooltip: IntLessThanFifty")]
	public int IntLessThanFifty;

	[NotLessThan(50, label="variable: IntMoreThanFifty", tooltip="tooltip: IntMoreThanFifty")]
	public int IntMoreThanFifty;

	[MHeader("Others"), Observe("callback1", "callback2",label="variable: Observe", tooltip="tooltip: Observe")]
	public string Observe;

	[RedNull(label="variable: RedNull", tooltip="tooltip: RedNull")]
	public GameObject RedNull;


	[LabelAlias(label="variable: LabelAlias", tooltip="tooltip: LabelAlias")]
	public string LabelAlias;

	[HelpBox("HelpBox message", HelpBoxType.Info), Button("method button", "buttonMethod", label="variable: Button", tooltip="tooltip: Button")]
	public bool Button;

	[ToggleBoolean(label="variable: ToggleBoolean", tooltip="tooltip: ToggleBoolean")]
	public bool ToggleBoolean;

	public void buttonMethod()
	{
		Debug.Log("buttonMethod");
	}
	public void callback1()
	{
		Debug.Log("called callback1");
	}
	public void callback2()
	{
		Debug.Log("called callback2");
	}

}
