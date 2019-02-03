using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

#region Header
/**	FSM
 *  This component is a Finite State Machine that uses UnityEvents
**/
#endregion
namespace com.FDT.Common
{
	public enum FSMStatus
	{
		NONE = -1,
		DOING_START = 0,
		DOING_UPDATE = 1,
		DOING_END = 2,
		ENDED = 3}

	;

	public class FSM : MonoBase
	{
		#region classes and enums

		[System.Serializable]
		public class FSMUpdateEvent:UnityEvent<float,float,float>
		{
		}

		[System.Serializable]
		public class FSMInitEvent:UnityEvent<FSM>
		{
			
		}

		[System.Serializable]
		public class FSMState
		{
			public int hash;
			public string stateName;
			public UnityEvent stateStart = new UnityEvent();
			public FSMUpdateEvent stateUpdate = new FSMUpdateEvent();
			public UnityEvent stateEnd = new UnityEvent();
			public Action stateStartAction;
			public Action<float,float,float> stateUpdateAction;
			public Action stateEndAction;
		}

		public enum UpdateOptions
		{
			OFF = 0,
			ON_UPDATE = 1,
			ON_LATEUPDATE = 2,
			ON_FIXEDUPDATE = 3}

		;

		#endregion

		#region public and serialized variables

		public string currentStateName;
		public int currentstateHash;
		protected float currentStateInitTime;
		protected float currentStateInitRealtime;
		[SerializeField, HideInInspector] protected FSMStatus currentStatus = FSMStatus.NONE;
		public FSMInitEvent AwakeEvent;
		// called when the GameObject calls Unity's Start event
		public FSMInitEvent InitEvent;
		// called when the first state is setted
		public FSMState currentState { get { return statesByName[currentStateName]; } }

		public List<FSMState> states = new List<FSMState>();
		public Dictionary<int, FSMState> statesByHash = new Dictionary<int, FSMState>();
		public Dictionary<string, FSMState> statesByName = new Dictionary<string, FSMState>();
		public UpdateOptions AutoUpdate = UpdateOptions.OFF;
		public bool initialized = false;
		[SerializeField] protected bool editable;

		#endregion

		#region mono

		protected override void Awake()
		{
			if (!initialized)
				Initialize();
			base.Awake();
		}

		public override string ToString()
		{
			return string.Format("[FSM: currentState={0} status={1}]", currentStateName, currentStatus.ToString());
		}

		#endregion

		#region api methods

		public void IgnoreAndChangeState(string stateName)
		{
			FSMState n = GetState(stateName);

			currentStatus = FSMStatus.DOING_START;
			currentstateHash = n.hash;
			currentStateName = n.stateName;
			currentStateInitRealtime = Time.realtimeSinceStartup;
			currentStateInitTime = Time.time;
			StateStart(n);
			currentStatus = FSMStatus.DOING_UPDATE;

		}

		public FSMState ChangeState(FSMState newState)
		{
			if (!initialized)
				Initialize();
			Debug.Log("requested change state to " + newState.stateName + " / last state " + currentStateName);
			SetState(newState);
			return newState;
		}

		public FSMState ChangeState(string stateName)
		{
			if (!initialized)
				Initialize();
			if (!statesByName.ContainsKey(stateName))
			{
				return null;
			}
			FSMState newState = statesByName[stateName];

			SetState(newState);
			return newState;
		}

		public FSMState AddState(string stateName)
		{
			if (Application.isPlaying && gameObject.activeInHierarchy)
			{
				return AddStateRuntime(stateName);
			}
			else
			{
				return AddStateEditor(stateName);
			}
		}

		public FSMState GetState(string stateName)
		{
			if (Application.isPlaying && gameObject.activeInHierarchy)
			{
				return GetStateRuntime(stateName);
			}
			else
			{
				return GetStateEditor(stateName);
			}
		}

		public virtual void DoUpdate(float t)
		{
			if (!initialized)
				Initialize();
			if (statesByHash[currentstateHash] != null)
			{
				StateUpdate(statesByHash[currentstateHash], t, Time.time - currentStateInitTime, Time.realtimeSinceStartup - currentStateInitRealtime);
			}
		}

		#endregion

		#region protected methods

		protected virtual void Initialize()
		{
			if (Application.isPlaying && gameObject.activeInHierarchy)
			{				
				statesByName.Clear();
				statesByHash[-1] = null;
				foreach (FSMState st in states)
				{
					st.hash = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
					statesByName[st.stateName] = st;
					statesByHash[st.hash] = st;
				}
				currentstateHash = -1;
				initialized = true;
				AwakeEvent.Invoke(this);
			}
		}

		protected virtual FSMState AddStateRuntime(string stateName)
		{
			if (!statesByName.ContainsKey(stateName))
			{
				FSMState result = new FSMState();
				result.stateName = stateName;
				states.Add(result);
				statesByName[stateName] = result;
				result.hash = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				statesByHash[result.hash] = result;
				return result;
			}
			return null;
		}

		protected virtual FSMState AddStateEditor(string stateName)
		{
			FSMState result = GetState(stateName);
			if (result == null)
			{
				result = new FSMState();
				result.stateName = stateName;
				result.hash = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				states.Add(result);
			}
			return result;
		}

		protected void SetState(FSMState newState)
		{
			if (currentStatus == FSMStatus.NONE)
				InitEvent.Invoke(this);
			
			if (currentStatus != FSMStatus.DOING_END && statesByHash[currentstateHash] != null)
			{
				currentStatus = FSMStatus.DOING_END;
				FSMState c = statesByHash[currentstateHash];
				currentstateHash = -1;
				currentStateName = null;
				StateEnd(c);
			}

			if (currentStatus != FSMStatus.DOING_START)
			{
				currentStatus = FSMStatus.DOING_START;
				currentstateHash = statesByName[newState.stateName].hash;
				currentStateName = newState.stateName;
				currentStateInitRealtime = Time.realtimeSinceStartup;
				currentStateInitTime = Time.time;
				StateStart(newState);
				currentStatus = FSMStatus.DOING_UPDATE;
			}
		}

		protected virtual FSMState GetStateRuntime(string stateName)
		{
			if (!initialized)
				Initialize();
			if (!statesByName.ContainsKey(stateName))
			{
				return null;
			}
			return statesByName[stateName];
		}

		protected virtual FSMState GetStateEditor(string stateName)
		{
			foreach (FSMState s in states)
			{
				if (s.stateName == stateName)
					return s;
			}
			return null;
		}

		protected void StateEnd(FSMState s)
		{
			s.stateEnd.Invoke();
			if (s.stateEndAction != null)
				s.stateEndAction();
		}

		protected void StateStart(FSMState s)
		{
			s.stateStart.Invoke();
			if (s.stateStartAction != null)
				s.stateStartAction();
		}

		protected void StateUpdate(FSMState s, float delta, float time, float rtime)
		{
			s.stateUpdate.Invoke(delta, time, rtime);
			if (s.stateUpdateAction != null)
				s.stateUpdateAction(delta, time, rtime);
		}

		protected virtual void Update()
		{
			if (currentStatus == FSMStatus.DOING_UPDATE && AutoUpdate == UpdateOptions.ON_UPDATE)
			{
				DoUpdate(Time.deltaTime);
			}
		}

		protected virtual void FixedUpdate()
		{
			if (currentStatus == FSMStatus.DOING_UPDATE && AutoUpdate == UpdateOptions.ON_FIXEDUPDATE)
			{
				DoUpdate(Time.fixedDeltaTime);
			}
		}

		protected virtual void LateUpdate()
		{
			if (currentStatus == FSMStatus.DOING_UPDATE && AutoUpdate == UpdateOptions.ON_LATEUPDATE)
			{
				DoUpdate(Time.deltaTime);
			}
		}

		#endregion
	}
}
