using UnityEngine;
using System.Collections;
using com.FDT.Common;

public class FSMInit : MonoBehaviour {

	public FSM fsm;
	// Use this for initialization
	void Start () 
	{
		fsm.ChangeState("Look");
	}
}
