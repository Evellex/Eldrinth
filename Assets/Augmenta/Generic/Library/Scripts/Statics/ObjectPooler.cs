using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	public static class ObjectPooler
	{
		private static Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

		static ObjectPooler()
		{
			UnityEvents.On_LevelWasLoaded += OnLevelLoadCalled;
		}

		public static void InitialisePool(GameObject template, int poolSize, bool dontDepositOnLoad = false)
		{
			if (template == null)
			{
				Console.PrintError("Parameter \"template\" is null!");
				return;
			}
			if (poolSize <= 0)
				Console.PrintWarning("Parameter \"poolSize\" is negative or zero! (ObjectPooler.InitialisePool)");
			ObjectPool targetPool = FindPool(template);
			if (targetPool == null)
			{
				Object.DontDestroyOnLoad(template);
				targetPool = new ObjectPool(template, poolSize, dontDepositOnLoad);
				pools.Add(template, targetPool);
			}
			else if (targetPool.CurrentPoolSize() < poolSize)
				targetPool.SetNewTargetPoolSize(poolSize);
		}

		public static GameObject Withdraw(GameObject template)
		{
			if (template == null)
			{
				Console.PrintError("Parameter \"template\" is null!");
				return null;
			}
			return Withdraw(template, Vector3.zero, Quaternion.identity);
		}

		public static GameObject Withdraw(GameObject template, Vector3 position, Quaternion rotation)
		{
			if (template == null)
			{
				Console.PrintError("Parameter \"template\" is null!");
				return null;
			}
			ObjectPool targetPool = FindPool(template);
			if (targetPool == null)
				return (Object.Instantiate(template, position, rotation) as GameObject);
			return targetPool.WithdrawObject(position, rotation);
		}

		public static void Deposit(GameObject target)
		{
			if (target == null)
			{
				Console.PrintWarning("Parameter \"target\" is null!");
				return;
			}
			ObjectPool targetPool = FindOwnerPool(target);
			if (targetPool != null)
				targetPool.DepositObject(target);
			else
				Object.Destroy(target);
		}

		public static void Deposit(GameObject target, float delay)
		{
			Invoker.Invoke(() => { if (target) Deposit(target); }, delay);
		}

		private static void OnLevelLoadCalled(int levelIndex)
		{
			foreach (ObjectPool pool in pools.Values)
				pool.OnLevelLoaded();
		}

		private static ObjectPool FindPool(GameObject template)
		{
			if (pools.ContainsKey(template))
				return pools[template];
			return null;
		}

		private static ObjectPool FindOwnerPool(GameObject target)
		{
			foreach (ObjectPool pool in pools.Values)
			{
				if (pool.ContainsObject(target))
					return pool;
			}
			return null;
		}

		private class ObjectPool
		{
			private GameObject template;
			private Transform objectFolder;
			private int currentPoolSize = 0;
			private bool dontDepositOnLoad = false;
			private List<GameObject> depositedObjects = new List<GameObject>();
			private List<GameObject> withdrawnObjects = new List<GameObject>();

			public ObjectPool(GameObject template, int poolSize, bool dontDepositOnLoad)
			{
				this.dontDepositOnLoad = dontDepositOnLoad;
				currentPoolSize = poolSize;
				this.template = template;
				objectFolder = new GameObject("Object Pool Folder (" + template.name + ")").transform;
				Object.DontDestroyOnLoad(objectFolder.gameObject);
				for (int i = 0; i < poolSize; ++i)
					AddNewPooledObject();
			}

			private ObjectPool()
			{
			}

			public GameObject Template
			{
				get { return template; }
			}

			public void OnLevelLoaded()
			{
				if (!dontDepositOnLoad)
					DepositAll();
			}

			public void DepositAll()
			{
				for (; withdrawnObjects.Count > 0;)
					DepositObject(withdrawnObjects[0]);
			}

			public GameObject WithdrawObject()
			{
				return WithdrawObject(template.transform.position, template.transform.rotation);
			}

			public GameObject WithdrawObject(Vector3 position, Quaternion rotation)
			{
				GameObject returnObject = WithdrawInternal();
				returnObject.transform.position = position;
				returnObject.transform.rotation = rotation;
				returnObject.transform.parent = null;
				returnObject.SetActive(true);
				CallOnWithdrawEvent(returnObject);
				return returnObject;
			}

			public void DepositObject(GameObject target)
			{
				if (withdrawnObjects.Contains(target))
				{
					if (target == null)
						CleanPool();
					else
					{
						withdrawnObjects.Remove(target);
						depositedObjects.Add(target);
						target.SetActive(false);
						target.transform.parent = objectFolder;
						CallOnDepositEvent(target);
					}
				}
			}

			public bool ContainsObject(GameObject target)
			{
				if (depositedObjects.Contains(target))
					return true;
				if (withdrawnObjects.Contains(target))
					return true;
				return false;
			}

			public int CurrentPoolSize()
			{
				return currentPoolSize;
			}

			public void SetNewTargetPoolSize(int newSize)
			{
				if (currentPoolSize < newSize)
				{
					for (; currentPoolSize < newSize;)
						AddNewPooledObject();
				}
				else if (currentPoolSize > newSize)
				{
					for (; currentPoolSize > newSize;)
						RemoveDepositedObject();
				}
			}

			private void AddNewPooledObject()
			{
				GameObject newObject = Object.Instantiate(template);
				newObject.name = newObject.name.Replace("(Clone)", "(Pooled)");
				newObject.SetActive(false);
				newObject.transform.parent = objectFolder;
				CallOnDepositEvent(newObject);
				depositedObjects.Add(newObject);
				Object.DontDestroyOnLoad(newObject);
				++currentPoolSize;
			}

			private void CallOnDepositEvent(GameObject target)
			{
				IPoolEvent[] list;
				list = target.GetInterfacesInChildren<IPoolEvent>(true);
				if (list != null) foreach (IPoolEvent t in list) { t.OnDeposit(); }
			}

			private void CallOnWithdrawEvent(GameObject target)
			{
				IPoolEvent[] list;
				list = target.GetInterfacesInChildren<IPoolEvent>(true);
				if (list != null) foreach (IPoolEvent t in list) { t.OnWithdraw(); }
			}

			private void CleanPool()
			{
				int countRemoved = depositedObjects.RemoveAll(x => x == null);
				countRemoved += withdrawnObjects.RemoveAll(x => x == null);
				currentPoolSize -= countRemoved;
				if (countRemoved > 0)
					Console.PrintError("An Object in the \"" + template.name + "\" ObjectPool has been destroyed incorrectly!");
			}

			private void RemoveDepositedObject()
			{
				GameObject targetObject = depositedObjects[0];
				depositedObjects.RemoveAt(0);
				Object.Destroy(targetObject);
				--currentPoolSize;
			}

			private GameObject WithdrawInternal()
			{
				GameObject returnObject = null;
				if (depositedObjects.Count == 0)
					AddNewPooledObject();
				returnObject = depositedObjects[0];
				if (returnObject == null)
				{
					CleanPool();
					return WithdrawInternal();
				}
				else
				{
					withdrawnObjects.Add(returnObject);
					depositedObjects.Remove(returnObject);
					return returnObject;
				}
			}
		}
	}
}