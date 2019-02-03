using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using com.FDT.Common;

#region Header
/**	Director
 *  This component allow for sequencial execution of methods using them as UnityEvents.
 *  The utilization of this component automatically creates a DirectorManager to handle their communication.
**/
#endregion
namespace com.FDT.Common
{
	public class Director : MonoBase, IManagerHandled
	{
		#region classes and enums

		[System.Serializable]
		public class DirectorItem:GUIDClass
		{
			public bool active = true;
			public DirectorItemType type;
			public UnityEvent callback;
			public int subItems = 0;
			public Vector2 pos;
			public float waitBefore = 0;
			public float waitAfter = 0;
			public bool waitingBefore = false;
			public bool waitingAfter = false;
			public List<int> completedHashs = new List<int>();
			public List<int> hashsReceived = new List<int>();
			public bool autoComplete = false;
		}

		public enum DirectorItemType
		{
			SINGLE = 0,
			MULTI = 1}

		;

		#endregion

		#region IManagerHandled implementation

		public string HandleName()
		{
			return directorName;
		}

		#endregion

		#region public and serialized variables

		public bool autoStart = false;
		public string directorName = "director";
		public bool running = false;
		public int index = 0;
		public List<DirectorItem> items = new List<DirectorItem>();
		public bool changedItem = false;
		public UnityEvent OnFinish;
		[SerializeField] protected bool editable;

		#endregion

		#region protected variables

		protected int subItems = 0;

		protected DirectorItem currentItem { get { return items[index]; } }

		protected Coroutine directorProcessCoroutine;

		#endregion

		#region Mono

		protected override void OnEnable()
		{
			base.OnEnable();
			DirectorManager.Instance.Register(this);
			directorProcessCoroutine = StartCoroutine(directorProcess());
		}

		protected override void Start()
		{
			base.Start();
			if (autoStart)
				Begin();
		}

		protected override void OnDisableOrDestroyNotQuit()
		{
			StopCoroutine(directorProcessCoroutine);
			DirectorManager.Instance.Unregister(this);
			base.OnDisableOrDestroyNotQuit();
		}

		#endregion

		#region api methods

		public void ItemCompleted()
		{
			if (!currentItem.autoComplete)
			{
				int hash = (directorName.GetHashCode() * 7);
				currentItem.hashsReceived.Add(hash);
			}
		}

		public void ItemCompleted(int hash)
		{
			if (!currentItem.autoComplete)
				currentItem.hashsReceived.Add(hash);
		}

		public void Begin()
		{
			index = -1;
			running = true;
			changedItem = true;
			foreach (DirectorItem i in items)
			{
				i.completedHashs.Clear();
				i.hashsReceived.Clear();
				i.waitingAfter = false;
				i.waitingBefore = false;
			}

			StopCoroutine(directorProcessCoroutine);
			GetNextItem();
			directorProcessCoroutine = StartCoroutine(directorProcess());
		}

		#endregion

		#region protected methods

		protected IEnumerator directorProcess()
		{
			do
			{				
				if (running)
				{
					if (changedItem)
					{
						if (currentItem.waitBefore > 0)
						{
							currentItem.waitingBefore = true;
							yield return new WaitForSeconds(currentItem.waitBefore);
							currentItem.waitingBefore = false;
						}
						currentItem.callback.Invoke();
						if (currentItem.waitAfter > 0)
						{
							currentItem.waitingAfter = true;
							yield return new WaitForSeconds(currentItem.waitAfter);
							currentItem.waitingAfter = false;
						}
						/// aca hacer algo
					}
					if (!currentItem.autoComplete)
					{
						while (currentItem.hashsReceived.Count == 0)
						{
							yield return new WaitForEndOfFrame();
						}
						int hash = currentItem.hashsReceived[0];
						currentItem.hashsReceived.RemoveAt(0);
						TestHash(hash);
					}
					else
					{
						GetNextItem();
					}
				}
				yield return new WaitForEndOfFrame();
			}
			while (true); // never stop
		}

		protected void TestHash(int hash)
		{
			if (running)
			{
				if (currentItem.type == DirectorItemType.MULTI)
				{
					if (!currentItem.completedHashs.Contains(hash))
					{
						currentItem.completedHashs.Add(hash);
						subItems--;
						changedItem = false;
					}
				}
				if (currentItem.type == DirectorItemType.SINGLE || subItems == 0)
				{
					changedItem = true;
					GetNextItem();
				}
			}
		}

		protected void GetNextItem()
		{
			if (items.Count == index + 1)
			{
				running = false;
				index = -1;
				Finished();
				return;
			}
			bool valid = false;
			do
			{
				index++;
				currentItem.completedHashs.Clear();
				valid = currentItem.active;
			}
			while (!valid && index < items.Count - 1);
			if (valid)
			{
				if (currentItem.type == DirectorItemType.MULTI)
				{
					subItems = currentItem.subItems;
				}
				else
				{
					subItems = 0;
				}
			}
			else
			{
				running = false;
				Finished();
			}
		}

		protected void Finished()
		{
			OnFinish.Invoke();
		}

		#endregion
	}
}