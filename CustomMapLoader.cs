using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CosmeticRoom;
using CustomMapSupport;
using GorillaExtensions;
using GorillaLocomotion.Swimming;
using GorillaNetworking;
using GorillaNetworking.Store;
using GorillaTag.Rendering;
using GorillaTagScripts.CustomMapSupport;
using GorillaTagScripts.ModIO;
using GorillaTagScripts.VirtualStumpCustomMaps;
using GT_CustomMapSupportRuntime;
using ModIO;
using Newtonsoft.Json;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x0200080F RID: 2063
public class CustomMapLoader : MonoBehaviour
{
	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x0600339E RID: 13214 RVA: 0x0010C00A File Offset: 0x0010A20A
	// (set) Token: 0x0600339D RID: 13213 RVA: 0x0010BFF3 File Offset: 0x0010A1F3
	public static string LoadedMapLevelName
	{
		get
		{
			return CustomMapLoader.loadedMapLevelName;
		}
		set
		{
			CustomMapLoader.loadedMapLevelName = value.Replace(" ", "");
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x0600339F RID: 13215 RVA: 0x0010C011 File Offset: 0x0010A211
	public static long LoadedMapModId
	{
		get
		{
			return CustomMapLoader.loadedMapModId;
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x060033A0 RID: 13216 RVA: 0x0010C018 File Offset: 0x0010A218
	public static long LoadedMapModFileId
	{
		get
		{
			return CustomMapLoader.loadedMapModFileId;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x060033A1 RID: 13217 RVA: 0x0010C01F File Offset: 0x0010A21F
	public static MapDescriptor LoadedMapDescriptor
	{
		get
		{
			return CustomMapLoader.loadedMapDescriptor;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x060033A2 RID: 13218 RVA: 0x0010C026 File Offset: 0x0010A226
	public static long LoadingMapModId
	{
		get
		{
			return CustomMapLoader.attemptedLoadID;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x060033A3 RID: 13219 RVA: 0x0010C02D File Offset: 0x0010A22D
	public static bool IsLoading
	{
		get
		{
			return CustomMapLoader.isLoading;
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x060033A5 RID: 13221 RVA: 0x0010C03C File Offset: 0x0010A23C
	// (set) Token: 0x060033A4 RID: 13220 RVA: 0x0010C034 File Offset: 0x0010A234
	public static bool CanLoadEntities
	{
		[CompilerGenerated]
		get
		{
			return CustomMapLoader.<CanLoadEntities>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			CustomMapLoader.<CanLoadEntities>k__BackingField = value;
		}
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x0010C043 File Offset: 0x0010A243
	public static bool IsCustomScene(string sceneName)
	{
		return CustomMapLoader.loadedSceneNames.Contains(sceneName);
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x0010C050 File Offset: 0x0010A250
	private void Awake()
	{
		if (CustomMapLoader.instance == null)
		{
			CustomMapLoader.instance = this;
			CustomMapLoader.hasInstance = true;
			return;
		}
		if (CustomMapLoader.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x0010C08C File Offset: 0x0010A28C
	private void Start()
	{
		byte[] array = new byte[]
		{
			Convert.ToByte(68),
			Convert.ToByte(111),
			Convert.ToByte(110),
			Convert.ToByte(116),
			Convert.ToByte(68),
			Convert.ToByte(101),
			Convert.ToByte(115),
			Convert.ToByte(116),
			Convert.ToByte(114),
			Convert.ToByte(111),
			Convert.ToByte(121),
			Convert.ToByte(79),
			Convert.ToByte(110),
			Convert.ToByte(76),
			Convert.ToByte(111),
			Convert.ToByte(97),
			Convert.ToByte(100)
		};
		this.dontDestroyOnLoadSceneName = Encoding.ASCII.GetString(array);
		if (this.networkTrigger != null)
		{
			this.networkTrigger.SetActive(false);
		}
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x0010C180 File Offset: 0x0010A380
	public static void LoadMap(long mapModId, string mapFilePath, Action<bool> onLoadComplete, Action<MapLoadStatus, int, string> progressCallback, Action<string> onSceneLoaded)
	{
		if (!CustomMapLoader.hasInstance)
		{
			return;
		}
		if (CustomMapLoader.isLoading)
		{
			return;
		}
		if (CustomMapLoader.isUnloading)
		{
			if (onLoadComplete != null)
			{
				onLoadComplete(false);
			}
			return;
		}
		if (CustomMapLoader.IsModLoaded(mapModId))
		{
			if (onLoadComplete != null)
			{
				onLoadComplete(true);
			}
			return;
		}
		GorillaNetworkJoinTrigger.DisableTriggerJoins();
		CustomMapLoader.CanLoadEntities = false;
		CustomMapLoader.modLoadProgressCallback = progressCallback;
		CustomMapLoader.modLoadedCallback = onLoadComplete;
		CustomMapLoader.sceneLoadedCallback = onSceneLoaded;
		CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadAssetBundle(mapModId, mapFilePath, new Action<bool, bool>(CustomMapLoader.OnAssetBundleLoaded)));
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x0010C200 File Offset: 0x0010A400
	public static void ResetToInitialZone(Action<string> onSceneLoaded, Action<string> onSceneUnloaded)
	{
		int[] array = new int[] { CustomMapLoader.initialSceneIndex };
		List<int> list = CustomMapLoader.loadedSceneIndexes;
		list.Remove(CustomMapLoader.initialSceneIndex);
		if (CustomMapLoader.sceneLoadingCoroutine != null)
		{
			CustomMapLoader.LoadZoneRequest loadZoneRequest = new CustomMapLoader.LoadZoneRequest
			{
				sceneIndexesToLoad = array,
				sceneIndexesToUnload = list.ToArray(),
				onSceneLoadedCallback = onSceneLoaded,
				onSceneUnloadedCallback = onSceneUnloaded
			};
			CustomMapLoader.queuedLoadZoneRequests.Add(loadZoneRequest);
			return;
		}
		CustomMapLoader.sceneLoadedCallback = onSceneLoaded;
		CustomMapLoader.sceneUnloadedCallback = onSceneUnloaded;
		CustomMapLoader.sceneLoadingCoroutine = CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadZoneCoroutine(array, list.ToArray()));
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x0010C29C File Offset: 0x0010A49C
	public static void LoadZoneTriggered(int[] loadSceneIndexes, int[] unloadSceneIndexes, Action<string> onSceneLoaded, Action<string> onSceneUnloaded)
	{
		string text = "";
		for (int i = 0; i < loadSceneIndexes.Length; i++)
		{
			text += loadSceneIndexes[i].ToString();
			if (i != loadSceneIndexes.Length - 1)
			{
				text += ", ";
			}
		}
		string text2 = "";
		for (int j = 0; j < unloadSceneIndexes.Length; j++)
		{
			text2 += unloadSceneIndexes[j].ToString();
			if (j != unloadSceneIndexes.Length - 1)
			{
				text2 += ", ";
			}
		}
		if (CustomMapLoader.sceneLoadingCoroutine != null)
		{
			CustomMapLoader.LoadZoneRequest loadZoneRequest = new CustomMapLoader.LoadZoneRequest
			{
				sceneIndexesToLoad = loadSceneIndexes,
				sceneIndexesToUnload = unloadSceneIndexes,
				onSceneLoadedCallback = onSceneLoaded,
				onSceneUnloadedCallback = onSceneUnloaded
			};
			CustomMapLoader.queuedLoadZoneRequests.Add(loadZoneRequest);
			return;
		}
		CustomMapLoader.sceneLoadedCallback = onSceneLoaded;
		CustomMapLoader.sceneUnloadedCallback = onSceneUnloaded;
		CustomMapLoader.sceneLoadingCoroutine = CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadZoneCoroutine(loadSceneIndexes, unloadSceneIndexes));
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x0010C383 File Offset: 0x0010A583
	private static IEnumerator LoadZoneCoroutine(int[] loadScenes, int[] unloadScenes)
	{
		if (!unloadScenes.IsNullOrEmpty<int>())
		{
			yield return CustomMapLoader.UnloadScenesCoroutine(unloadScenes);
		}
		if (!loadScenes.IsNullOrEmpty<int>())
		{
			yield return CustomMapLoader.LoadScenesCoroutine(loadScenes);
		}
		if (CustomMapLoader.sceneLoadingCoroutine != null)
		{
			CustomMapLoader.instance.StopCoroutine(CustomMapLoader.sceneLoadingCoroutine);
			CustomMapLoader.sceneLoadingCoroutine = null;
		}
		if (CustomMapLoader.queuedLoadZoneRequests.Count > 0)
		{
			CustomMapLoader.LoadZoneRequest loadZoneRequest = CustomMapLoader.queuedLoadZoneRequests[0];
			CustomMapLoader.queuedLoadZoneRequests.RemoveAt(0);
			CustomMapLoader.LoadZoneTriggered(loadZoneRequest.sceneIndexesToLoad, loadZoneRequest.sceneIndexesToUnload, loadZoneRequest.onSceneLoadedCallback, loadZoneRequest.onSceneUnloadedCallback);
		}
		yield break;
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x0010C399 File Offset: 0x0010A599
	private static IEnumerator LoadScenesCoroutine(int[] sceneIndexes)
	{
		int num;
		for (int i = 0; i < sceneIndexes.Length; i = num + 1)
		{
			if (!CustomMapLoader.loadedSceneFilePaths.Contains(CustomMapLoader.assetBundleSceneFilePaths[sceneIndexes[i]]))
			{
				yield return CustomMapLoader.LoadSceneFromAssetBundle(sceneIndexes[i], false, new Action<bool, bool, string>(CustomMapLoader.OnIncrementalLoadComplete));
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x0010C3A8 File Offset: 0x0010A5A8
	private static IEnumerator UnloadScenesCoroutine(int[] sceneIndexes)
	{
		int num;
		for (int i = 0; i < sceneIndexes.Length; i = num + 1)
		{
			yield return CustomMapLoader.UnloadSceneCoroutine(sceneIndexes[i], null);
			num = i;
		}
		yield break;
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x0010C3B7 File Offset: 0x0010A5B7
	private static IEnumerator LoadAssetBundle(long mapModID, string packageInfoFilePath, Action<bool, bool> OnLoadComplete)
	{
		if (CustomMapLoader.isLoading)
		{
			if (OnLoadComplete != null)
			{
				OnLoadComplete(false, false);
			}
			yield break;
		}
		yield return CustomMapLoader.CloseDoorAndUnloadModCoroutine();
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(-1);
			OnLoadComplete(false, true);
			yield break;
		}
		CustomMapLoader.isLoading = true;
		CustomMapLoader.attemptedLoadID = mapModID;
		Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
		if (action != null)
		{
			action(MapLoadStatus.Loading, 1, "GRABBING LIGHTMAP DATA");
		}
		CustomMapLoader.lightmaps = new LightmapData[LightmapSettings.lightmaps.Length];
		if (CustomMapLoader.lightmapsToKeep.Count > 0)
		{
			CustomMapLoader.lightmapsToKeep.Clear();
		}
		CustomMapLoader.lightmapsToKeep = new List<Texture2D>(LightmapSettings.lightmaps.Length * 2);
		for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
		{
			CustomMapLoader.lightmaps[i] = LightmapSettings.lightmaps[i];
			if (LightmapSettings.lightmaps[i].lightmapColor != null)
			{
				CustomMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapColor);
			}
			if (LightmapSettings.lightmaps[i].lightmapDir != null)
			{
				CustomMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapDir);
			}
		}
		Action<MapLoadStatus, int, string> action2 = CustomMapLoader.modLoadProgressCallback;
		if (action2 != null)
		{
			action2(MapLoadStatus.Loading, 2, "LOADING PACKAGE INFO");
		}
		MapPackageInfo packageInfo;
		try
		{
			packageInfo = CustomMapLoader.GetPackageInfo(packageInfoFilePath);
		}
		catch (Exception ex)
		{
			Action<MapLoadStatus, int, string> action3 = CustomMapLoader.modLoadProgressCallback;
			if (action3 != null)
			{
				action3(MapLoadStatus.Error, 0, ex.Message);
			}
			yield break;
		}
		if (packageInfo == null)
		{
			Action<MapLoadStatus, int, string> action4 = CustomMapLoader.modLoadProgressCallback;
			if (action4 != null)
			{
				action4(MapLoadStatus.Error, 0, "FAILED TO READ FILE AT " + packageInfoFilePath);
			}
			OnLoadComplete(false, false);
			yield break;
		}
		CustomMapLoader.initialSceneName = packageInfo.initialScene;
		Action<MapLoadStatus, int, string> action5 = CustomMapLoader.modLoadProgressCallback;
		if (action5 != null)
		{
			action5(MapLoadStatus.Loading, 3, "PACKAGE INFO LOADED");
		}
		string text = Path.GetDirectoryName(packageInfoFilePath) + "/" + packageInfo.pcFileName;
		Action<MapLoadStatus, int, string> action6 = CustomMapLoader.modLoadProgressCallback;
		if (action6 != null)
		{
			action6(MapLoadStatus.Loading, 12, "LOADING MAP ASSET BUNDLE");
		}
		AssetBundleCreateRequest loadBundleRequest = AssetBundle.LoadFromFileAsync(text);
		yield return loadBundleRequest;
		CustomMapLoader.mapBundle = loadBundleRequest.assetBundle;
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(-1);
			OnLoadComplete(false, true);
			yield break;
		}
		if (CustomMapLoader.mapBundle == null)
		{
			Action<MapLoadStatus, int, string> action7 = CustomMapLoader.modLoadProgressCallback;
			if (action7 != null)
			{
				action7(MapLoadStatus.Error, 0, "CUSTOM MAP ASSET BUNDLE FAILED TO LOAD");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		if (!CustomMapLoader.mapBundle.isStreamedSceneAssetBundle)
		{
			CustomMapLoader.mapBundle.Unload(true);
			Action<MapLoadStatus, int, string> action8 = CustomMapLoader.modLoadProgressCallback;
			if (action8 != null)
			{
				action8(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		Action<MapLoadStatus, int, string> action9 = CustomMapLoader.modLoadProgressCallback;
		if (action9 != null)
		{
			action9(MapLoadStatus.Loading, 20, "MAP ASSET BUNDLE LOADED");
		}
		CustomMapLoader.mapBundle.GetAllAssetNames();
		CustomMapLoader.assetBundleSceneFilePaths = CustomMapLoader.mapBundle.GetAllScenePaths();
		if (CustomMapLoader.assetBundleSceneFilePaths.Length == 0)
		{
			CustomMapLoader.mapBundle.Unload(true);
			Action<MapLoadStatus, int, string> action10 = CustomMapLoader.modLoadProgressCallback;
			if (action10 != null)
			{
				action10(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		foreach (string text2 in CustomMapLoader.assetBundleSceneFilePaths)
		{
			if (text2.Equals(CustomMapLoader.instance.dontDestroyOnLoadSceneName, StringComparison.OrdinalIgnoreCase))
			{
				CustomMapLoader.mapBundle.Unload(true);
				Action<MapLoadStatus, int, string> action11 = CustomMapLoader.modLoadProgressCallback;
				if (action11 != null)
				{
					action11(MapLoadStatus.Error, 0, "Map name is " + text2 + " this is an invalid name");
				}
				OnLoadComplete(false, false);
				yield break;
			}
		}
		OnLoadComplete(true, false);
		yield break;
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x0010C3D4 File Offset: 0x0010A5D4
	private static void RequestAbortModLoad(Action callback = null)
	{
		CustomMapLoader.abortModLoadCallback = callback;
		CustomMapLoader.shouldAbortSceneLoad = true;
		CustomMapLoader.shouldUnloadMod = true;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x0010C3E8 File Offset: 0x0010A5E8
	private static IEnumerator AbortSceneLoad(int sceneIndex)
	{
		if (sceneIndex == -1)
		{
			CustomMapLoader.shouldUnloadMod = true;
		}
		CustomMapLoader.isLoading = false;
		if (CustomMapLoader.shouldUnloadMod)
		{
			if (CustomMapLoader.sceneLoadingCoroutine != null)
			{
				CustomMapLoader.instance.StopCoroutine(CustomMapLoader.sceneLoadingCoroutine);
				CustomMapLoader.sceneLoadingCoroutine = null;
			}
			yield return CustomMapLoader.UnloadAllScenesCoroutine();
			if (CustomMapLoader.mapBundle != null)
			{
				CustomMapLoader.mapBundle.Unload(true);
			}
			CustomMapLoader.mapBundle = null;
			Action action = CustomMapLoader.abortModLoadCallback;
			if (action != null)
			{
				action();
			}
		}
		else
		{
			yield return CustomMapLoader.UnloadSceneCoroutine(sceneIndex, null);
		}
		CustomMapLoader.abortModLoadCallback = null;
		CustomMapLoader.shouldAbortSceneLoad = false;
		CustomMapLoader.shouldUnloadMod = false;
		yield break;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x0010C3F8 File Offset: 0x0010A5F8
	private static int GetSceneIndex(string sceneName)
	{
		int num = -1;
		if (CustomMapLoader.assetBundleSceneFilePaths.Length == 1)
		{
			return 0;
		}
		for (int i = 0; i < CustomMapLoader.assetBundleSceneFilePaths.Length; i++)
		{
			string sceneNameFromFilePath = CustomMapLoader.GetSceneNameFromFilePath(CustomMapLoader.assetBundleSceneFilePaths[i]);
			if (sceneNameFromFilePath != null && sceneNameFromFilePath.Equals(sceneName))
			{
				num = i;
				break;
			}
		}
		return num;
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x0010C447 File Offset: 0x0010A647
	private static IEnumerator LoadSceneFromAssetBundle(int sceneIndex, bool useProgressCallback, Action<bool, bool, string> OnLoadComplete)
	{
		LoadSceneParameters loadSceneParameters = new LoadSceneParameters
		{
			loadSceneMode = LoadSceneMode.Additive,
			localPhysicsMode = LocalPhysicsMode.None
		};
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			yield break;
		}
		CustomMapLoader.runningAsyncLoad = true;
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 30, "LOADING MAP SCENE");
			}
		}
		CustomMapLoader.attemptedSceneToLoad = CustomMapLoader.assetBundleSceneFilePaths[sceneIndex];
		string sceneName = CustomMapLoader.GetSceneNameFromFilePath(CustomMapLoader.attemptedSceneToLoad);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(CustomMapLoader.attemptedSceneToLoad, loadSceneParameters);
		yield return asyncOperation;
		CustomMapLoader.runningAsyncLoad = false;
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action2 = CustomMapLoader.modLoadProgressCallback;
			if (action2 != null)
			{
				action2(MapLoadStatus.Loading, 50, "SANITIZING MAP");
			}
		}
		GameObject[] rootGameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
		List<MapDescriptor> list = new List<MapDescriptor>();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			list.AddRange(rootGameObjects[i].GetComponentsInChildren<MapDescriptor>());
		}
		MapDescriptor mapDescriptor = null;
		bool flag = false;
		foreach (MapDescriptor mapDescriptor2 in list)
		{
			if (!mapDescriptor2.IsNull())
			{
				if (!mapDescriptor.IsNull())
				{
					flag = true;
					break;
				}
				mapDescriptor = mapDescriptor2;
			}
		}
		if (flag)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action3 = CustomMapLoader.modLoadProgressCallback;
				if (action3 != null)
				{
					action3(MapLoadStatus.Error, 0, "MAP CONTAINS MULTIPLE MAP DESCRIPTOR OBJECTS");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		if (mapDescriptor.IsNull())
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action4 = CustomMapLoader.modLoadProgressCallback;
				if (action4 != null)
				{
					action4(MapLoadStatus.Error, 0, "MAP SCENE DOES NOT CONTAIN A MAP DESCRIPTOR");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		GameObject gameObject = mapDescriptor.gameObject;
		if (!CustomMapLoader.SanitizeObject(gameObject, gameObject))
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action5 = CustomMapLoader.modLoadProgressCallback;
				if (action5 != null)
				{
					action5(MapLoadStatus.Error, 0, "MAP DESCRIPTOR HAS UNAPPROVED COMPONENTS ON IT");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		CustomMapLoader.totalObjectsInLoadingScene = 0;
		for (int j = 0; j < rootGameObjects.Length; j++)
		{
			CustomMapLoader.SanitizeObjectRecursive(rootGameObjects[j], gameObject);
		}
		CustomMapLoader.CheckVirtualStumpOverlap(sceneName);
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action6 = CustomMapLoader.modLoadProgressCallback;
			if (action6 != null)
			{
				action6(MapLoadStatus.Loading, 70, "MAP SCENE LOADED");
			}
		}
		CustomMapLoader.leafGliderIndex = 0;
		yield return CustomMapLoader.ProcessAndInstantiateMap(gameObject, useProgressCallback);
		yield return null;
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			if (CustomMapLoader.cachedExceptionMessage.Length > 0 && useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action7 = CustomMapLoader.modLoadProgressCallback;
				if (action7 != null)
				{
					action7(MapLoadStatus.Error, 0, CustomMapLoader.cachedExceptionMessage);
				}
			}
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action8 = CustomMapLoader.modLoadProgressCallback;
			if (action8 != null)
			{
				action8(MapLoadStatus.Loading, 99, "FINALIZING MAP");
			}
		}
		CustomMapLoader.loadedSceneFilePaths.AddIfNew(CustomMapLoader.attemptedSceneToLoad);
		CustomMapLoader.loadedSceneNames.AddIfNew(sceneName);
		CustomMapLoader.loadedSceneIndexes.AddIfNew(sceneIndex);
		OnLoadComplete(true, false, sceneName);
		yield break;
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x0010C464 File Offset: 0x0010A664
	public static void CloseDoorAndUnloadMod(Action unloadFinishedCallback = null)
	{
		if (!CustomMapLoader.IsModLoaded(0L) && !CustomMapLoader.isLoading)
		{
			return;
		}
		CustomMapLoader.unloadModCallback = unloadFinishedCallback;
		if (CustomMapLoader.isLoading)
		{
			CustomMapLoader.RequestAbortModLoad(null);
			return;
		}
		if (CustomMapLoader.instance.accessDoor != null)
		{
			CustomMapLoader.instance.accessDoor.CloseDoor();
		}
		CustomMapLoader.shouldUnloadMod = true;
		if (CustomMapLoader.mapBundle != null)
		{
			CustomMapLoader.mapBundle.Unload(true);
		}
		CustomMapLoader.mapBundle = null;
		CustomMapTelemetry.EndMapTracking();
		CMSSerializer.ResetSyncedMapObjects();
		CustomMapLoader.instance.StartCoroutine(CustomMapLoader.UnloadAllScenesCoroutine());
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x0010C4FB File Offset: 0x0010A6FB
	private static IEnumerator CloseDoorAndUnloadModCoroutine()
	{
		if (!CustomMapLoader.IsModLoaded(0L) || CustomMapLoader.IsLoading)
		{
			yield break;
		}
		if (CustomMapLoader.instance.accessDoor != null)
		{
			CustomMapLoader.instance.accessDoor.CloseDoor();
		}
		CustomMapLoader.shouldUnloadMod = true;
		if (CustomMapLoader.mapBundle != null)
		{
			CustomMapLoader.mapBundle.Unload(true);
		}
		CustomMapLoader.mapBundle = null;
		CustomMapTelemetry.EndMapTracking();
		CMSSerializer.ResetSyncedMapObjects();
		yield return CustomMapLoader.UnloadAllScenesCoroutine();
		yield break;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x0010C503 File Offset: 0x0010A703
	private static IEnumerator UnloadAllScenesCoroutine()
	{
		CustomMapLoader.isLoading = false;
		CustomMapLoader.isUnloading = true;
		CustomMapLoader.CanLoadEntities = false;
		ZoneShaderSettings.ActivateDefaultSettings();
		CustomMapLoader.RemoveCustomMapATM();
		if (!CustomMapLoader.assetBundleSceneFilePaths.IsNullOrEmpty<string>())
		{
			int num;
			for (int sceneIndex = 0; sceneIndex < CustomMapLoader.assetBundleSceneFilePaths.Length; sceneIndex = num + 1)
			{
				yield return CustomMapLoader.UnloadSceneCoroutine(sceneIndex, null);
				num = sceneIndex;
			}
		}
		CustomMapLoader.loadedMapDescriptor = null;
		CustomMapLoader.loadedSceneFilePaths.Clear();
		CustomMapLoader.loadedSceneNames.Clear();
		CustomMapLoader.loadedSceneIndexes.Clear();
		for (int i = 0; i < CustomMapLoader.instance.leafGliders.Length; i++)
		{
			CustomMapLoader.instance.leafGliders[i].CustomMapUnload();
			CustomMapLoader.instance.leafGliders[i].enabled = false;
			CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].transform.GetChild(0).gameObject.SetActive(false);
		}
		GorillaNetworkJoinTrigger.EnableTriggerJoins();
		LightmapSettings.lightmaps = CustomMapLoader.lightmaps;
		CustomMapLoader.UnloadLightmaps();
		Resources.UnloadUnusedAssets();
		CustomMapLoader.isUnloading = false;
		if (CustomMapLoader.shouldUnloadMod)
		{
			yield return CustomMapLoader.ResetLightmaps();
			CustomMapLoader.assetBundleSceneFilePaths = new string[] { "" };
			CustomMapLoader.loadedMapModId = 0L;
			CustomMapLoader.initialSceneIndex = 0;
			CustomMapLoader.initialSceneName = "";
			CustomMapLoader.maxPlayersForMap = 10;
			Action action = CustomMapLoader.unloadModCallback;
			if (action != null)
			{
				action();
			}
			CustomMapLoader.unloadModCallback = null;
			CustomMapLoader.shouldUnloadMod = false;
			CustomMapLoader.usingDynamicLighting = false;
			GameLightingManager.instance.SetCustomDynamicLightingEnabled(false);
			GameLightingManager.instance.SetAmbientLightDynamic(Color.black);
		}
		yield break;
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x0010C50B File Offset: 0x0010A70B
	private static IEnumerator UnloadSceneCoroutine(int sceneIndex, Action OnUnloadComplete = null)
	{
		if (!CustomMapLoader.hasInstance)
		{
			yield break;
		}
		if (sceneIndex < 0 || sceneIndex >= CustomMapLoader.assetBundleSceneFilePaths.Length)
		{
			Debug.LogError(string.Format("[CustomMapLoader::UnloadSceneCoroutine] SceneIndex of {0} is invalid! ", sceneIndex) + string.Format("The currently loaded AssetBundle contains {0} scenes.", CustomMapLoader.assetBundleSceneFilePaths.Length));
			yield break;
		}
		while (CustomMapLoader.runningAsyncLoad)
		{
			yield return null;
		}
		UnloadSceneOptions unloadSceneOptions = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
		string scenePathWithExtension = CustomMapLoader.assetBundleSceneFilePaths[sceneIndex];
		string[] array = scenePathWithExtension.Split(".", StringSplitOptions.None);
		string text = "";
		string sceneName = "";
		if (!array.IsNullOrEmpty<string>())
		{
			text = array[0];
			if (text.Length > 0)
			{
				sceneName = Path.GetFileName(text);
			}
		}
		Scene sceneByName = SceneManager.GetSceneByName(text);
		if (sceneByName.IsValid())
		{
			if (CustomMapLoader.customMapATM.IsNotNull() && CustomMapLoader.customMapATM.gameObject.scene.Equals(sceneByName))
			{
				CustomMapLoader.RemoveCustomMapATM();
			}
			CustomMapLoader.RemoveUnloadingStorePrefabs(sceneByName);
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenePathWithExtension, unloadSceneOptions);
			yield return asyncOperation;
			CustomMapLoader.loadedSceneFilePaths.Remove(scenePathWithExtension);
			CustomMapLoader.loadedSceneNames.Remove(sceneName);
			CustomMapLoader.loadedSceneIndexes.Remove(sceneIndex);
			Action<string> action = CustomMapLoader.sceneUnloadedCallback;
			if (action != null)
			{
				action(sceneName);
			}
			if (OnUnloadComplete != null)
			{
				OnUnloadComplete();
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x0010C524 File Offset: 0x0010A724
	private static void RemoveUnloadingStorePrefabs(Scene unloadingScene)
	{
		for (int i = CustomMapLoader.storeDisplayStands.Count - 1; i >= 0; i--)
		{
			if (CustomMapLoader.storeDisplayStands[i].IsNull())
			{
				CustomMapLoader.storeDisplayStands.RemoveAt(i);
			}
			else if (CustomMapLoader.storeDisplayStands[i].scene.Equals(unloadingScene))
			{
				DynamicCosmeticStand componentInChildren = CustomMapLoader.storeDisplayStands[i].GetComponentInChildren<DynamicCosmeticStand>();
				if (componentInChildren.IsNotNull())
				{
					StoreController.instance.RemoveStandFromPlayFabIDDictionary(componentInChildren);
				}
				CustomMapLoader.storeDisplayStands.RemoveAt(i);
			}
		}
		for (int i = CustomMapLoader.storeCheckouts.Count - 1; i >= 0; i--)
		{
			if (CustomMapLoader.storeCheckouts[i].IsNull())
			{
				CustomMapLoader.storeCheckouts.RemoveAt(i);
			}
			else
			{
				ItemCheckout componentInChildren2 = CustomMapLoader.storeCheckouts[i].GetComponentInChildren<ItemCheckout>();
				if (componentInChildren2.IsNotNull() && componentInChildren2.IsFromScene(unloadingScene))
				{
					componentInChildren2.RemoveFromCustomMap(CustomMapLoader.instance.compositeTryOnArea);
					CosmeticsController.instance.RemoveItemCheckout(componentInChildren2);
					Object.Destroy(CustomMapLoader.storeCheckouts[i]);
					CustomMapLoader.storeCheckouts.RemoveAt(i);
				}
			}
		}
		for (int i = CustomMapLoader.storeTryOnConsoles.Count - 1; i >= 0; i--)
		{
			if (CustomMapLoader.storeTryOnConsoles[i].IsNull())
			{
				CustomMapLoader.storeTryOnConsoles.RemoveAt(i);
			}
			else if (CustomMapLoader.storeTryOnConsoles[i].scene.Equals(unloadingScene))
			{
				FittingRoom componentInChildren3 = CustomMapLoader.storeTryOnConsoles[i].GetComponentInChildren<FittingRoom>();
				if (componentInChildren3.IsNotNull())
				{
					CosmeticsController.instance.RemoveFittingRoom(componentInChildren3);
				}
				CustomMapLoader.storeTryOnConsoles.RemoveAt(i);
			}
		}
		for (int i = CustomMapLoader.storeTryOnAreas.Count - 1; i >= 0; i--)
		{
			if (CustomMapLoader.storeTryOnAreas[i].IsNull())
			{
				CustomMapLoader.storeTryOnAreas.RemoveAt(i);
			}
			else
			{
				CMSTryOnArea component = CustomMapLoader.storeTryOnAreas[i].GetComponent<CMSTryOnArea>();
				if (component.IsNotNull() && component.IsFromScene(unloadingScene))
				{
					component.RemoveFromCustomMap(CustomMapLoader.instance.compositeTryOnArea);
					Object.Destroy(CustomMapLoader.storeTryOnAreas[i]);
					CustomMapLoader.storeTryOnAreas.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x0010C771 File Offset: 0x0010A971
	private static void RemoveCustomMapATM()
	{
		if (ATM_Manager.instance.IsNotNull())
		{
			ATM_Manager.instance.RemoveATM(CustomMapLoader.customMapATM);
			ATM_Manager.instance.ResetTemporaryCreatorCode();
			CustomMapLoader.customMapATM = null;
		}
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x0010C7A4 File Offset: 0x0010A9A4
	private static IEnumerator ResetLightmaps()
	{
		CustomMapLoader.instance.dayNightManager.RequestRepopulateLightmaps();
		LoadSceneParameters loadSceneParameters = new LoadSceneParameters
		{
			loadSceneMode = LoadSceneMode.Additive,
			localPhysicsMode = LocalPhysicsMode.None
		};
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(10, loadSceneParameters);
		yield return asyncOperation;
		asyncOperation = SceneManager.UnloadSceneAsync(10);
		yield return asyncOperation;
		yield break;
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x0010C7AC File Offset: 0x0010A9AC
	private static void LoadLightmaps(Texture2D[] colorMaps, Texture2D[] dirMaps)
	{
		if (colorMaps.Length == 0)
		{
			return;
		}
		CustomMapLoader.UnloadLightmaps();
		List<LightmapData> list = new List<LightmapData>(LightmapSettings.lightmaps);
		for (int i = 0; i < colorMaps.Length; i++)
		{
			bool flag = false;
			LightmapData lightmapData = new LightmapData();
			if (colorMaps[i] != null)
			{
				lightmapData.lightmapColor = colorMaps[i];
				flag = true;
				if (i < dirMaps.Length && dirMaps[i] != null)
				{
					lightmapData.lightmapDir = dirMaps[i];
				}
			}
			if (flag)
			{
				list.Add(lightmapData);
			}
		}
		LightmapSettings.lightmaps = list.ToArray();
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x0010C82C File Offset: 0x0010AA2C
	private static void UnloadLightmaps()
	{
		foreach (LightmapData lightmapData in LightmapSettings.lightmaps)
		{
			if (lightmapData.lightmapColor != null && !CustomMapLoader.lightmapsToKeep.Contains(lightmapData.lightmapColor))
			{
				Resources.UnloadAsset(lightmapData.lightmapColor);
			}
			if (lightmapData.lightmapDir != null && !CustomMapLoader.lightmapsToKeep.Contains(lightmapData.lightmapDir))
			{
				Resources.UnloadAsset(lightmapData.lightmapDir);
			}
		}
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x0010C8A8 File Offset: 0x0010AAA8
	private static bool SanitizeObject(GameObject gameObject, GameObject mapRoot)
	{
		if (gameObject == null)
		{
			Debug.LogError("CustomMapLoader::SanitizeObject gameobject null");
			return false;
		}
		if (!CustomMapLoader.APPROVED_LAYERS.Contains(gameObject.layer))
		{
			gameObject.layer = 0;
		}
		foreach (Component component in gameObject.GetComponents<Component>())
		{
			if (component == null)
			{
				Object.DestroyImmediate(gameObject, true);
				return false;
			}
			bool flag = true;
			foreach (Type type in CustomMapLoader.componentAllowlist)
			{
				if (component.GetType() == type)
				{
					if (type == typeof(Camera))
					{
						Camera camera = (Camera)component;
						if (camera.IsNotNull() && camera.targetTexture.IsNull())
						{
							break;
						}
					}
					flag = false;
					break;
				}
			}
			if (flag)
			{
				foreach (string text in CustomMapLoader.componentTypeStringAllowList)
				{
					if (component.GetType().ToString().Contains(text))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				Object.DestroyImmediate(gameObject, true);
				return false;
			}
		}
		if (gameObject.transform.parent.IsNull() && gameObject.transform != mapRoot.transform)
		{
			gameObject.transform.SetParent(mapRoot.transform);
		}
		return true;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x0010CA38 File Offset: 0x0010AC38
	private static void SanitizeObjectRecursive(GameObject rootObject, GameObject mapRoot)
	{
		if (!CustomMapLoader.SanitizeObject(rootObject, mapRoot))
		{
			return;
		}
		CustomMapLoader.totalObjectsInLoadingScene++;
		for (int i = 0; i < rootObject.transform.childCount; i++)
		{
			GameObject gameObject = rootObject.transform.GetChild(i).gameObject;
			if (gameObject.IsNotNull())
			{
				CustomMapLoader.SanitizeObjectRecursive(gameObject, mapRoot);
			}
		}
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x0010CA92 File Offset: 0x0010AC92
	private static IEnumerator ProcessAndInstantiateMap(GameObject map, bool useProgressCallback)
	{
		if (map.IsNull() || !CustomMapLoader.hasInstance)
		{
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 73, "PROCESSING ROOT MAP OBJECT");
			}
		}
		CustomMapLoader.loadedMapDescriptor = map.GetComponent<MapDescriptor>();
		if (CustomMapLoader.loadedMapDescriptor.IsNull())
		{
			yield break;
		}
		if (CustomMapLoader.loadedMapDescriptor.IsInitialScene && CustomMapLoader.loadedMapDescriptor.UseUberShaderDynamicLighting)
		{
			GameLightingManager.instance.SetCustomDynamicLightingEnabled(true);
			GameLightingManager.instance.SetAmbientLightDynamic(CustomMapLoader.loadedMapDescriptor.UberShaderAmbientDynamicLight);
			CustomMapLoader.usingDynamicLighting = true;
		}
		CustomMapLoader.objectsProcessedForLoadingScene = 0;
		CustomMapLoader.objectsProcessedThisFrame = 0;
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action2 = CustomMapLoader.modLoadProgressCallback;
			if (action2 != null)
			{
				action2(MapLoadStatus.Loading, 75, "PROCESSING CHILD OBJECTS");
			}
		}
		CustomMapLoader.initializePhaseTwoComponents.Clear();
		CustomMapLoader.agentsToCreate.Clear();
		yield return CustomMapLoader.ProcessChildObjects(map, useProgressCallback);
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action3 = CustomMapLoader.modLoadProgressCallback;
			if (action3 != null)
			{
				action3(MapLoadStatus.Loading, 95, "PROCESSING COMPLETE");
			}
		}
		yield return null;
		CustomMapLoader.InitializeComponentsPhaseTwo();
		CustomMapLoader.placeholderReplacements.Clear();
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action4 = CustomMapLoader.modLoadProgressCallback;
			if (action4 != null)
			{
				action4(MapLoadStatus.Loading, 97, "PROCESSING COMPLETE");
			}
		}
		if (CustomMapLoader.loadedMapDescriptor.IsInitialScene)
		{
			CustomMapLoader.maxPlayersForMap = (byte)Math.Clamp(CustomMapLoader.loadedMapDescriptor.MaxPlayers, 1, 10);
			VirtualStumpReturnWatch.SetWatchProperties(CustomMapLoader.loadedMapDescriptor.GetReturnToVStumpWatchProps());
		}
		yield break;
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x0010CAA8 File Offset: 0x0010ACA8
	private static IEnumerator ProcessChildObjects(GameObject parent, bool useProgressCallback)
	{
		if (parent == null || CustomMapLoader.placeholderReplacements.Contains(parent))
		{
			yield break;
		}
		int num3;
		for (int i = 0; i < parent.transform.childCount; i = num3 + 1)
		{
			Transform child = parent.transform.GetChild(i);
			if (!(child == null))
			{
				GameObject gameObject = child.gameObject;
				if (!(gameObject == null) && !CustomMapLoader.placeholderReplacements.Contains(gameObject))
				{
					try
					{
						CustomMapLoader.SetupCollisions(gameObject);
						CustomMapLoader.ReplaceDataOnlyScripts(gameObject);
						CustomMapLoader.ReplacePlaceholders(gameObject);
						CustomMapLoader.SetupDynamicLight(gameObject);
						CustomMapLoader.StoreAIAgent(gameObject);
						CustomMapLoader.InitializeComponentsPhaseOne(gameObject);
					}
					catch (Exception ex)
					{
						CustomMapLoader.shouldAbortSceneLoad = true;
						CustomMapLoader.cachedExceptionMessage = ex.Message;
						yield break;
					}
					if (gameObject.transform.childCount > 0)
					{
						yield return CustomMapLoader.ProcessChildObjects(gameObject, useProgressCallback);
						if (CustomMapLoader.shouldAbortSceneLoad)
						{
							yield break;
						}
					}
					if (CustomMapLoader.shouldAbortSceneLoad)
					{
						yield break;
					}
					CustomMapLoader.objectsProcessedForLoadingScene++;
					CustomMapLoader.objectsProcessedThisFrame++;
					if (CustomMapLoader.objectsProcessedThisFrame >= CustomMapLoader.numObjectsToProcessPerFrame)
					{
						CustomMapLoader.objectsProcessedThisFrame = 0;
						if (useProgressCallback)
						{
							float num = (float)CustomMapLoader.objectsProcessedForLoadingScene / (float)CustomMapLoader.totalObjectsInLoadingScene;
							int num2 = Mathf.FloorToInt(20f * num);
							Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
							if (action != null)
							{
								action(MapLoadStatus.Loading, 75 + num2, "PROCESSING CHILD OBJECTS");
							}
						}
						yield return null;
					}
				}
			}
			num3 = i;
		}
		yield break;
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x0010CAC0 File Offset: 0x0010ACC0
	private static void CheckVirtualStumpOverlap(string sceneName)
	{
		Vector3 vector = new Vector3(5.15f, 0.72f, 5.15f);
		Vector3 vector2 = new Vector3(0f, 0.73f, 0f);
		float num = vector.x * 0.5f + 2f;
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		gameObject.transform.position = CustomMapLoader.instance.virtualStumpMesh.transform.position + vector2;
		gameObject.transform.localScale = vector;
		Collider[] array = Physics.OverlapSphere(gameObject.transform.position, num);
		if (array == null || array.Length == 0)
		{
			Object.DestroyImmediate(gameObject);
			return;
		}
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.convex = true;
		foreach (Collider collider in array)
		{
			Vector3 vector3;
			float num2;
			if (!(collider == null) && !(collider.gameObject == gameObject) && !(collider.gameObject.scene.name != sceneName) && Physics.ComputePenetration(meshCollider, gameObject.transform.position, gameObject.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out vector3, out num2) && !collider.isTrigger)
			{
				GTDev.Log<string>("[CustomMapLoader::CheckVirtualStumpOverlap] Gameobject " + collider.name + " has a collider overlapping with the virtual stump. Collider will be removed", null);
				Object.DestroyImmediate(collider);
			}
		}
		Object.DestroyImmediate(gameObject);
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x0010CC48 File Offset: 0x0010AE48
	private static void SetupCollisions(GameObject gameObject)
	{
		if (gameObject == null || CustomMapLoader.placeholderReplacements.Contains(gameObject))
		{
			return;
		}
		Collider[] components = gameObject.GetComponents<Collider>();
		if (components == null)
		{
			return;
		}
		bool flag = true;
		bool flag2 = false;
		foreach (Collider collider in components)
		{
			if (!(collider == null))
			{
				if (collider.isTrigger)
				{
					flag2 = true;
					if (gameObject.layer != UnityLayer.GorillaInteractable.ToLayerIndex())
					{
						gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
						break;
					}
				}
				else
				{
					flag = false;
					if (!flag2 && gameObject.layer == UnityLayer.Default.ToLayerIndex())
					{
						gameObject.layer = UnityLayer.GorillaObject.ToLayerIndex();
					}
				}
			}
		}
		if (!flag)
		{
			SurfaceOverrideSettings component = gameObject.GetComponent<SurfaceOverrideSettings>();
			GorillaSurfaceOverride gorillaSurfaceOverride = gameObject.AddComponent<GorillaSurfaceOverride>();
			if (component == null)
			{
				gorillaSurfaceOverride.overrideIndex = 0;
				return;
			}
			gorillaSurfaceOverride.overrideIndex = (int)component.soundOverride;
			gorillaSurfaceOverride.extraVelMultiplier = component.extraVelMultiplier;
			gorillaSurfaceOverride.extraVelMaxMultiplier = component.extraVelMaxMultiplier;
			gorillaSurfaceOverride.slidePercentageOverride = component.slidePercentage;
			gorillaSurfaceOverride.disablePushBackEffect = component.disablePushBackEffect;
			Object.Destroy(component);
		}
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x0010CD5C File Offset: 0x0010AF5C
	private static void ReplaceDataOnlyScripts(GameObject gameObject)
	{
		MapBoundarySettings component = gameObject.GetComponent<MapBoundarySettings>();
		if (component != null)
		{
			CMSMapBoundary cmsmapBoundary = gameObject.AddComponent<CMSMapBoundary>();
			if (cmsmapBoundary != null)
			{
				cmsmapBoundary.CopyTriggerSettings(component);
			}
			Object.Destroy(component);
		}
		TagZoneSettings component2 = gameObject.GetComponent<TagZoneSettings>();
		if (component2 != null)
		{
			CMSTagZone cmstagZone = gameObject.AddComponent<CMSTagZone>();
			if (cmstagZone != null)
			{
				cmstagZone.CopyTriggerSettings(component2);
			}
			Object.Destroy(component2);
		}
		TeleporterSettings component3 = gameObject.GetComponent<TeleporterSettings>();
		if (component3 != null)
		{
			CMSTeleporter cmsteleporter = gameObject.AddComponent<CMSTeleporter>();
			if (cmsteleporter != null)
			{
				cmsteleporter.CopyTriggerSettings(component3);
			}
			Object.Destroy(component3);
		}
		ObjectActivationTriggerSettings component4 = gameObject.GetComponent<ObjectActivationTriggerSettings>();
		if (component4 != null)
		{
			CMSObjectActivationTrigger cmsobjectActivationTrigger = gameObject.AddComponent<CMSObjectActivationTrigger>();
			if (cmsobjectActivationTrigger != null)
			{
				cmsobjectActivationTrigger.CopyTriggerSettings(component4);
			}
			Object.Destroy(component4);
		}
		LoadZoneSettings component5 = gameObject.GetComponent<LoadZoneSettings>();
		if (component5 != null)
		{
			CMSLoadingZone cmsloadingZone = gameObject.AddComponent<CMSLoadingZone>();
			if (cmsloadingZone != null)
			{
				cmsloadingZone.SetupLoadingZone(component5, in CustomMapLoader.assetBundleSceneFilePaths);
			}
			Object.Destroy(component5);
		}
		CMSZoneShaderSettings component6 = gameObject.GetComponent<CMSZoneShaderSettings>();
		if (component6.IsNotNull())
		{
			ZoneShaderSettings zoneShaderSettings = gameObject.AddComponent<ZoneShaderSettings>();
			zoneShaderSettings.CopySettings(component6, false);
			if (component6.isDefaultValues)
			{
				CustomMapManager.SetDefaultZoneShaderSettings(zoneShaderSettings, component6.GetProperties());
			}
			CustomMapManager.AddZoneShaderSettings(zoneShaderSettings);
			Object.Destroy(component6);
		}
		ZoneShaderTriggerSettings component7 = gameObject.GetComponent<ZoneShaderTriggerSettings>();
		if (component7.IsNotNull())
		{
			gameObject.AddComponent<CMSZoneShaderSettingsTrigger>().CopySettings(component7);
			Object.Destroy(component7);
		}
		HandHoldSettings component8 = gameObject.GetComponent<HandHoldSettings>();
		if (component8.IsNotNull())
		{
			gameObject.AddComponent<HandHold>().CopyProperties(component8);
			Object.Destroy(component8);
		}
		CustomMapEjectButtonSettings component9 = gameObject.GetComponent<CustomMapEjectButtonSettings>();
		if (component9.IsNotNull())
		{
			CustomMapEjectButton customMapEjectButton = gameObject.AddComponent<CustomMapEjectButton>();
			customMapEjectButton.gameObject.layer = UnityLayer.GorillaInteractable.ToLayerIndex();
			customMapEjectButton.CopySettings(component9);
			Object.Destroy(component9);
		}
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x0010CF2C File Offset: 0x0010B12C
	private static void StoreAIAgent(GameObject agentGameObject)
	{
		if (agentGameObject.IsNull() || CustomMapsGameManager.instance.IsNull())
		{
			return;
		}
		AIAgent component = agentGameObject.GetComponent<AIAgent>();
		if (component.IsNull() || component.isTemplate)
		{
			return;
		}
		CustomMapLoader.agentsToCreate.Add(component);
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x0010CF74 File Offset: 0x0010B174
	private static void SetupDynamicLight(GameObject dynamicLightGameObject)
	{
		if (dynamicLightGameObject.IsNull())
		{
			return;
		}
		UberShaderDynamicLight component = dynamicLightGameObject.GetComponent<UberShaderDynamicLight>();
		if (component.IsNull())
		{
			return;
		}
		if (component.dynamicLight.IsNull())
		{
			return;
		}
		GameObject gameObject = new GameObject(dynamicLightGameObject.name + "GameLight");
		GameLight gameLight = gameObject.AddComponent<GameLight>();
		gameLight.light = component.dynamicLight;
		GameLightingManager.instance.AddGameLight(gameLight, false);
		gameObject.transform.SetParent(dynamicLightGameObject.transform.parent);
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x0010CFF4 File Offset: 0x0010B1F4
	private static void ReplacePlaceholders(GameObject placeholderGameObject)
	{
		if (placeholderGameObject.IsNull())
		{
			return;
		}
		GTObjectPlaceholder component = placeholderGameObject.GetComponent<GTObjectPlaceholder>();
		if (component.IsNull())
		{
			return;
		}
		switch (component.PlaceholderObject)
		{
		case GTObject.LeafGlider:
			if (CustomMapLoader.leafGliderIndex < CustomMapLoader.instance.leafGliders.Length)
			{
				CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].enabled = true;
				CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].CustomMapLoad(component.transform, component.maxDistanceBeforeRespawn);
				CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].transform.GetChild(0).gameObject.SetActive(true);
				CustomMapLoader.leafGliderIndex++;
				return;
			}
			break;
		case GTObject.GliderWindVolume:
		{
			List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
			if (component.useDefaultPlaceholder || list.Count == 0)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(CustomMapLoader.instance.gliderWindVolume, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
				if (gameObject != null)
				{
					CustomMapLoader.placeholderReplacements.Add(gameObject);
					gameObject.transform.localScale = placeholderGameObject.transform.localScale;
					placeholderGameObject.transform.localScale = Vector3.one;
					gameObject.transform.SetParent(placeholderGameObject.transform);
					GliderWindVolume component2 = gameObject.GetComponent<GliderWindVolume>();
					if (component2 == null)
					{
						return;
					}
					component2.SetProperties(component.maxSpeed, component.maxAccel, component.SpeedVSAccelCurve, component.localWindDirection);
					return;
				}
			}
			else
			{
				placeholderGameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
				GliderWindVolume gliderWindVolume = placeholderGameObject.AddComponent<GliderWindVolume>();
				if (gliderWindVolume.IsNotNull())
				{
					gliderWindVolume.SetProperties(component.maxSpeed, component.maxAccel, component.SpeedVSAccelCurve, component.localWindDirection);
					return;
				}
			}
			break;
		}
		case GTObject.WaterVolume:
		{
			List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
			if (component.useDefaultPlaceholder || list.Count == 0)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(CustomMapLoader.instance.waterVolumePrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
				if (gameObject2 != null)
				{
					CustomMapLoader.placeholderReplacements.Add(gameObject2);
					gameObject2.layer = UnityLayer.Water.ToLayerIndex();
					gameObject2.transform.localScale = placeholderGameObject.transform.localScale;
					placeholderGameObject.transform.localScale = Vector3.one;
					gameObject2.transform.SetParent(placeholderGameObject.transform);
					MeshRenderer component3 = gameObject2.GetComponent<MeshRenderer>();
					if (component3.IsNull())
					{
						return;
					}
					if (!component.useWaterMesh)
					{
						component3.enabled = false;
						return;
					}
					component3.enabled = true;
					WaterSurfaceMaterialController component4 = gameObject2.GetComponent<WaterSurfaceMaterialController>();
					if (component4.IsNull())
					{
						return;
					}
					component4.ScrollX = component.scrollTextureX;
					component4.ScrollY = component.scrollTextureY;
					component4.Scale = component.scaleTexture;
					return;
				}
			}
			else
			{
				placeholderGameObject.layer = UnityLayer.Water.ToLayerIndex();
				WaterVolume waterVolume = placeholderGameObject.AddComponent<WaterVolume>();
				if (waterVolume.IsNotNull())
				{
					WaterParameters waterParameters = null;
					CMSZoneShaderSettings.EZoneLiquidType liquidType = component.liquidType;
					if (liquidType != CMSZoneShaderSettings.EZoneLiquidType.Water)
					{
						if (liquidType == CMSZoneShaderSettings.EZoneLiquidType.Lava)
						{
							waterParameters = CustomMapLoader.instance.defaultLavaParameters;
						}
					}
					else
					{
						waterParameters = CustomMapLoader.instance.defaultWaterParameters;
					}
					waterVolume.SetPropertiesFromPlaceholder(component.GetWaterVolumeProperties(), list, waterParameters);
					waterVolume.RefreshColliders();
					return;
				}
			}
			break;
		}
		case GTObject.ForceVolume:
		{
			List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
			if (component.useDefaultPlaceholder || list.Count == 0)
			{
				GameObject gameObject3 = Object.Instantiate<GameObject>(CustomMapLoader.instance.forceVolumePrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
				if (gameObject3.IsNotNull())
				{
					CustomMapLoader.placeholderReplacements.Add(gameObject3);
					gameObject3.transform.localScale = placeholderGameObject.transform.localScale;
					placeholderGameObject.transform.localScale = Vector3.one;
					gameObject3.transform.SetParent(placeholderGameObject.transform);
					ForceVolume component5 = gameObject3.GetComponent<ForceVolume>();
					if (component5.IsNull())
					{
						return;
					}
					component5.SetPropertiesFromPlaceholder(component.GetForceVolumeProperties(), null, null);
					return;
				}
			}
			else
			{
				ForceVolume forceVolume = placeholderGameObject.AddComponent<ForceVolume>();
				if (forceVolume.IsNotNull())
				{
					AudioSource audioSource = placeholderGameObject.GetComponent<AudioSource>();
					if (audioSource.IsNull())
					{
						audioSource = placeholderGameObject.AddComponent<AudioSource>();
						audioSource.spatialize = true;
						audioSource.playOnAwake = false;
						audioSource.priority = 128;
						audioSource.volume = 0.522f;
						audioSource.pitch = 1f;
						audioSource.panStereo = 0f;
						audioSource.spatialBlend = 1f;
						audioSource.reverbZoneMix = 1f;
						audioSource.dopplerLevel = 1f;
						audioSource.spread = 0f;
						audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
						audioSource.minDistance = 8.2f;
						audioSource.maxDistance = 43.94f;
						audioSource.enabled = true;
					}
					audioSource.outputAudioMixerGroup = CustomMapLoader.instance.masterAudioMixer;
					for (int i = list.Count - 1; i >= 0; i--)
					{
						if (i == 0)
						{
							list[i].isTrigger = true;
						}
						else
						{
							Object.Destroy(list[i]);
						}
					}
					placeholderGameObject.layer = UnityLayer.GorillaBoundary.ToLayerIndex();
					forceVolume.SetPropertiesFromPlaceholder(component.GetForceVolumeProperties(), audioSource, component.GetComponent<Collider>());
					return;
				}
				Debug.LogError("[CustomMapLoader::ReplacePlaceholders] Failed to add ForceVolume component to Placeholder!");
				return;
			}
			break;
		}
		case GTObject.ATM:
		{
			if (CustomMapLoader.customMapATM.IsNotNull())
			{
				Debug.LogError("[CustomMapLoader::ReplacePlaceholders] Map already contains an ATM, maps are only allowed 1 ATM! Removing placeholder and not instantiating...");
				return;
			}
			GameObject gameObject4 = CustomMapLoader.instance.atmPrefab;
			if (component.useCustomMesh)
			{
				gameObject4 = CustomMapLoader.instance.atmNoShellPrefab;
			}
			if (gameObject4.IsNull())
			{
				return;
			}
			GameObject gameObject5 = Object.Instantiate<GameObject>(gameObject4, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
			if (gameObject5.IsNotNull())
			{
				CustomMapLoader.placeholderReplacements.Add(gameObject5);
				gameObject5.transform.SetParent(placeholderGameObject.transform);
				ATM_UI componentInChildren = gameObject5.GetComponentInChildren<ATM_UI>();
				if (componentInChildren.IsNotNull() && ATM_Manager.instance.IsNotNull())
				{
					CustomMapLoader.customMapATM = componentInChildren;
					ATM_Manager.instance.AddATM(componentInChildren);
					if (!component.defaultCreatorCode.IsNullOrEmpty())
					{
						ATM_Manager.instance.SetTemporaryCreatorCode(component.defaultCreatorCode, true, null);
						return;
					}
				}
			}
			break;
		}
		case GTObject.HoverboardArea:
			if (component.AddComponent<HoverboardAreaTrigger>().IsNotNull())
			{
				component.gameObject.layer = UnityLayer.GorillaBoundary.ToLayerIndex();
				List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
				if (list.Count != 0)
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (j == 0)
						{
							list[j].isTrigger = true;
						}
						else
						{
							Object.Destroy(list[j]);
						}
					}
					return;
				}
				BoxCollider boxCollider = component.AddComponent<BoxCollider>();
				if (boxCollider.IsNotNull())
				{
					boxCollider.isTrigger = true;
					return;
				}
			}
			break;
		case GTObject.HoverboardDispenser:
		{
			if (CustomMapLoader.instance.hoverboardDispenserPrefab.IsNull())
			{
				Debug.LogError("[CustomMapLoader::ReplacePlaceholders] hoverboardDispenserPrefab is NULL!");
				return;
			}
			GameObject gameObject6 = Object.Instantiate<GameObject>(CustomMapLoader.instance.hoverboardDispenserPrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
			if (gameObject6.IsNotNull())
			{
				CustomMapLoader.placeholderReplacements.Add(gameObject6);
				gameObject6.transform.SetParent(placeholderGameObject.transform);
				return;
			}
			break;
		}
		case GTObject.RopeSwing:
		{
			GameObject gameObject7 = Object.Instantiate<GameObject>(CustomMapLoader.instance.ropeSwingPrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
			if (gameObject7.IsNull())
			{
				return;
			}
			gameObject7.transform.SetParent(placeholderGameObject.transform);
			CustomMapsGorillaRopeSwing component6 = gameObject7.GetComponent<CustomMapsGorillaRopeSwing>();
			if (component6.IsNull())
			{
				Object.DestroyImmediate(gameObject7);
				return;
			}
			int num = Math.Clamp(component.ropeLength, 3, 31);
			component6.SetRopeLength(num);
			CustomMapLoader.placeholderReplacements.Add(gameObject7);
			return;
		}
		case GTObject.ZipLine:
		{
			GameObject gameObject8 = Object.Instantiate<GameObject>(CustomMapLoader.instance.ziplinePrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
			if (gameObject8.IsNull())
			{
				return;
			}
			gameObject8.transform.SetParent(placeholderGameObject.transform);
			CustomMapsGorillaZipline component7 = gameObject8.GetComponent<CustomMapsGorillaZipline>();
			if (component7.IsNull())
			{
				Object.DestroyImmediate(gameObject8);
				return;
			}
			if (!component7.GenerateZipline(component.spline))
			{
				Object.DestroyImmediate(gameObject8);
				return;
			}
			CustomMapLoader.placeholderReplacements.Add(gameObject8);
			return;
		}
		case GTObject.Store_DisplayStand:
		{
			if (CustomMapLoader.instance.storeDisplayStandPrefab.IsNull())
			{
				return;
			}
			if (CustomMapLoader.storeDisplayStands.Count >= Constants.storeDisplayStandLimit)
			{
				Object.Destroy(component);
				return;
			}
			GameObject gameObject9 = Object.Instantiate<GameObject>(CustomMapLoader.instance.storeDisplayStandPrefab, placeholderGameObject.transform);
			if (gameObject9.IsNull())
			{
				return;
			}
			DynamicCosmeticStand component8 = gameObject9.GetComponent<DynamicCosmeticStand>();
			if (component8.IsNull())
			{
				Object.DestroyImmediate(gameObject9);
				return;
			}
			component8.InitializeForCustomMapCosmeticItem(component.CosmeticItem);
			CustomMapLoader.storeDisplayStands.Add(gameObject9);
			CustomMapLoader.placeholderReplacements.Add(gameObject9);
			return;
		}
		case GTObject.Store_TryOnArea:
		{
			if (CustomMapLoader.instance.storeTryOnAreaPrefab.IsNull() || CustomMapLoader.instance.compositeTryOnArea.IsNull())
			{
				return;
			}
			if (CustomMapLoader.storeTryOnAreas.Count >= Constants.storeTryOnAreaLimit)
			{
				Object.Destroy(component);
				return;
			}
			GameObject gameObject10 = Object.Instantiate<GameObject>(CustomMapLoader.instance.storeTryOnAreaPrefab, placeholderGameObject.transform);
			gameObject10.transform.SetParent(CustomMapLoader.instance.compositeTryOnArea.transform);
			CMSTryOnArea component9 = gameObject10.GetComponent<CMSTryOnArea>();
			if (component9.IsNull() || component9.tryOnAreaCollider.IsNull())
			{
				Object.DestroyImmediate(gameObject10);
				return;
			}
			BoxCollider tryOnAreaCollider = component9.tryOnAreaCollider;
			Vector3 zero = Vector3.zero;
			zero.x = tryOnAreaCollider.size.x * tryOnAreaCollider.transform.lossyScale.x;
			zero.y = tryOnAreaCollider.size.y * tryOnAreaCollider.transform.lossyScale.y;
			zero.z = tryOnAreaCollider.size.z * tryOnAreaCollider.transform.lossyScale.z;
			if (zero.x * zero.y * zero.z > Constants.storeTryOnAreaVolumeLimit)
			{
				Object.DestroyImmediate(gameObject10);
				return;
			}
			component9.InitializeForCustomMap(CustomMapLoader.instance.compositeTryOnArea, placeholderGameObject.scene);
			CustomMapLoader.storeTryOnAreas.Add(gameObject10);
			CustomMapLoader.placeholderReplacements.Add(gameObject10);
			break;
		}
		case GTObject.Store_Checkout:
		{
			if (CustomMapLoader.instance.storeCheckoutCounterPrefab.IsNull())
			{
				return;
			}
			if (CustomMapLoader.storeCheckouts.Count >= Constants.storeCheckoutCounterLimit)
			{
				Object.Destroy(component);
				return;
			}
			GameObject gameObject11 = Object.Instantiate<GameObject>(CustomMapLoader.instance.storeCheckoutCounterPrefab, placeholderGameObject.transform);
			gameObject11.transform.SetParent(CustomMapLoader.instance.compositeTryOnArea.transform);
			if (gameObject11.IsNull())
			{
				return;
			}
			ItemCheckout componentInChildren2 = gameObject11.GetComponentInChildren<ItemCheckout>();
			if (componentInChildren2.IsNull())
			{
				Object.DestroyImmediate(gameObject11);
				return;
			}
			componentInChildren2.InitializeForCustomMap(CustomMapLoader.instance.compositeTryOnArea, placeholderGameObject.scene, component.useCustomMesh);
			CustomMapLoader.storeCheckouts.Add(gameObject11);
			CustomMapLoader.placeholderReplacements.Add(gameObject11);
			return;
		}
		case GTObject.Store_TryOnConsole:
		{
			if (CustomMapLoader.instance.storeTryOnConsolePrefab.IsNull())
			{
				return;
			}
			if (CustomMapLoader.storeTryOnConsoles.Count >= Constants.storeTryOnConsoleLimit)
			{
				Object.Destroy(component);
				return;
			}
			GameObject gameObject12 = Object.Instantiate<GameObject>(CustomMapLoader.instance.storeTryOnConsolePrefab, placeholderGameObject.transform);
			if (gameObject12.IsNull())
			{
				return;
			}
			FittingRoom componentInChildren3 = gameObject12.GetComponentInChildren<FittingRoom>();
			if (componentInChildren3.IsNull())
			{
				Object.DestroyImmediate(gameObject12);
				return;
			}
			componentInChildren3.InitializeForCustomMap(component.useCustomMesh);
			CustomMapLoader.storeTryOnConsoles.Add(gameObject12);
			CustomMapLoader.placeholderReplacements.Add(gameObject12);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x000023F5 File Offset: 0x000005F5
	private static void InitializeComponentsPhaseOne(GameObject childGameObject)
	{
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x0010DBAC File Offset: 0x0010BDAC
	private static void InitializeComponentsPhaseTwo()
	{
		for (int i = 0; i < CustomMapLoader.initializePhaseTwoComponents.Count; i++)
		{
		}
		CustomMapLoader.initializePhaseTwoComponents.Clear();
		if (CustomMapLoader.agentsToCreate.Count > 0)
		{
			for (int j = 0; j < CustomMapLoader.agentsToCreate.Count; j++)
			{
				CustomMapLoader.agentsToCreate[j].gameObject.SetActive(false);
			}
			CustomMapsGameManager.SetAgentsToCreate(CustomMapLoader.agentsToCreate);
		}
		CustomMapLoader.CanLoadEntities = true;
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x0010DC20 File Offset: 0x0010BE20
	public static bool OpenDoorToMap()
	{
		if (!CustomMapLoader.hasInstance)
		{
			return false;
		}
		if (CustomMapLoader.instance.accessDoor != null)
		{
			CustomMapLoader.instance.accessDoor.OpenDoor();
			return true;
		}
		return false;
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x0010DC54 File Offset: 0x0010BE54
	private static void OnAssetBundleLoaded(bool loadSucceeded, bool loadAborted)
	{
		if (loadAborted)
		{
			return;
		}
		if (loadSucceeded)
		{
			CustomMapLoader.loadedMapModId = CustomMapLoader.attemptedLoadID;
			SubscribedMod subscribedMod;
			if (ModIOManager.GetSubscribedModProfile(new ModId(CustomMapLoader.loadedMapModId), out subscribedMod))
			{
				CustomMapLoader.loadedMapModFileId = subscribedMod.modProfile.modfile.id;
			}
			else
			{
				CustomMapLoader.loadedMapModFileId = 0L;
			}
			if (CustomMapLoader.initialSceneName != string.Empty)
			{
				CustomMapLoader.initialSceneIndex = CustomMapLoader.GetSceneIndex(CustomMapLoader.initialSceneName);
			}
			if (CustomMapLoader.initialSceneIndex == -1 && CustomMapLoader.mapBundle != null)
			{
				CustomMapLoader.mapBundle.Unload(true);
				CustomMapLoader.mapBundle = null;
				Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
				if (action != null)
				{
					action(MapLoadStatus.Error, 0, "ASSET BUNDLE CONTAINS MULTIPLE SCENES, BUT NONE ARE SET AS INITIAL SCENE.");
				}
				CustomMapLoader.OnLoadComplete(false, true, "");
			}
			CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadSceneFromAssetBundle(CustomMapLoader.initialSceneIndex, true, new Action<bool, bool, string>(CustomMapLoader.OnLoadComplete)));
		}
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x0010DD33 File Offset: 0x0010BF33
	private static void OnIncrementalLoadComplete(bool loadSucceeded, bool loadAborted, string loadedScene)
	{
		if (loadSucceeded)
		{
			CustomMapLoader.sceneLoadedCallback(loadedScene);
			return;
		}
		CustomMapLoader.instance.StopAllCoroutines();
		CustomMapLoader.isLoading = false;
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x0010DD58 File Offset: 0x0010BF58
	private static void OnLoadComplete(bool loadSucceeded, bool loadAborted, string loadedScene)
	{
		CustomMapLoader.isLoading = false;
		GorillaNetworkJoinTrigger.EnableTriggerJoins();
		if (loadSucceeded)
		{
			Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 100, "Load Complete");
			}
			if (CustomMapLoader.instance.networkTrigger != null)
			{
				CustomMapLoader.instance.networkTrigger.SetActive(true);
			}
			if (CustomMapsTerminal.IsDriver)
			{
			}
		}
		else
		{
			CustomMapLoader.loadedMapDescriptor = null;
		}
		if (!loadAborted)
		{
			Action<bool> action2 = CustomMapLoader.modLoadedCallback;
			if (action2 != null)
			{
				action2(loadSucceeded);
			}
			if (loadSucceeded)
			{
				Action<string> action3 = CustomMapLoader.sceneLoadedCallback;
				if (action3 == null)
				{
					return;
				}
				action3(loadedScene);
			}
		}
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x0010DDE7 File Offset: 0x0010BFE7
	private static string GetSceneNameFromFilePath(string filePath)
	{
		string[] array = filePath.Split("/", StringSplitOptions.None);
		return array[array.Length - 1].Split(".", StringSplitOptions.None)[0];
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x0010DE08 File Offset: 0x0010C008
	public static MapPackageInfo GetPackageInfo(string packageInfoFilePath)
	{
		MapPackageInfo mapPackageInfo;
		using (StreamReader streamReader = new StreamReader(File.OpenRead(packageInfoFilePath), Encoding.Default))
		{
			mapPackageInfo = JsonConvert.DeserializeObject<MapPackageInfo>(streamReader.ReadToEnd());
		}
		return mapPackageInfo;
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x0010DE50 File Offset: 0x0010C050
	public static Transform GetCustomMapsDefaultSpawnLocation()
	{
		if (CustomMapLoader.hasInstance)
		{
			return CustomMapLoader.instance.CustomMapsDefaultSpawnLocation;
		}
		return null;
	}

	// Token: 0x060033D0 RID: 13264 RVA: 0x0010DE67 File Offset: 0x0010C067
	public static bool IsModLoaded(long mapModId = 0L)
	{
		if (mapModId != 0L)
		{
			return !CustomMapLoader.IsLoading && CustomMapLoader.LoadedMapModId == mapModId;
		}
		return !CustomMapLoader.IsLoading && CustomMapLoader.LoadedMapModId != 0L;
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x0010DE90 File Offset: 0x0010C090
	public static byte GetRoomSizeForCurrentlyLoadedMap()
	{
		if (CustomMapLoader.IsModLoaded(0L))
		{
			return CustomMapLoader.maxPlayersForMap;
		}
		return 10;
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x0010DEA3 File Offset: 0x0010C0A3
	public CustomMapLoader()
	{
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x0010DEB8 File Offset: 0x0010C0B8
	// Note: this type is marked as 'beforefieldinit'.
	static CustomMapLoader()
	{
	}

	// Token: 0x04004079 RID: 16505
	[OnEnterPlay_SetNull]
	private static volatile CustomMapLoader instance;

	// Token: 0x0400407A RID: 16506
	[OnEnterPlay_Set(false)]
	private static bool hasInstance;

	// Token: 0x0400407B RID: 16507
	public Transform CustomMapsDefaultSpawnLocation;

	// Token: 0x0400407C RID: 16508
	public CustomMapAccessDoor accessDoor;

	// Token: 0x0400407D RID: 16509
	public GameObject networkTrigger;

	// Token: 0x0400407E RID: 16510
	[SerializeField]
	private BetterDayNightManager dayNightManager;

	// Token: 0x0400407F RID: 16511
	[SerializeField]
	private GameObject placeholderParent;

	// Token: 0x04004080 RID: 16512
	[SerializeField]
	private GliderHoldable[] leafGliders;

	// Token: 0x04004081 RID: 16513
	[SerializeField]
	private GameObject leafGlider;

	// Token: 0x04004082 RID: 16514
	[SerializeField]
	private GameObject gliderWindVolume;

	// Token: 0x04004083 RID: 16515
	[FormerlySerializedAs("waterVolume")]
	[SerializeField]
	private GameObject waterVolumePrefab;

	// Token: 0x04004084 RID: 16516
	[SerializeField]
	private WaterParameters defaultWaterParameters;

	// Token: 0x04004085 RID: 16517
	[SerializeField]
	private WaterParameters defaultLavaParameters;

	// Token: 0x04004086 RID: 16518
	[FormerlySerializedAs("forceVolume")]
	[SerializeField]
	private GameObject forceVolumePrefab;

	// Token: 0x04004087 RID: 16519
	[SerializeField]
	private GameObject atmPrefab;

	// Token: 0x04004088 RID: 16520
	[SerializeField]
	private GameObject atmNoShellPrefab;

	// Token: 0x04004089 RID: 16521
	[SerializeField]
	private GameObject storeDisplayStandPrefab;

	// Token: 0x0400408A RID: 16522
	[SerializeField]
	private GameObject storeCheckoutCounterPrefab;

	// Token: 0x0400408B RID: 16523
	[SerializeField]
	private GameObject storeTryOnConsolePrefab;

	// Token: 0x0400408C RID: 16524
	[SerializeField]
	private GameObject storeTryOnAreaPrefab;

	// Token: 0x0400408D RID: 16525
	[SerializeField]
	private GameObject hoverboardDispenserPrefab;

	// Token: 0x0400408E RID: 16526
	[SerializeField]
	private GameObject ropeSwingPrefab;

	// Token: 0x0400408F RID: 16527
	[SerializeField]
	private GameObject ziplinePrefab;

	// Token: 0x04004090 RID: 16528
	[SerializeField]
	private GameObject zoneShaderSettingsTrigger;

	// Token: 0x04004091 RID: 16529
	[SerializeField]
	private AudioMixerGroup masterAudioMixer;

	// Token: 0x04004092 RID: 16530
	[SerializeField]
	private ZoneShaderSettings customMapZoneShaderSettings;

	// Token: 0x04004093 RID: 16531
	[SerializeField]
	private CompositeTriggerEvents compositeTryOnArea;

	// Token: 0x04004094 RID: 16532
	[SerializeField]
	private GameObject virtualStumpMesh;

	// Token: 0x04004095 RID: 16533
	private static readonly int numObjectsToProcessPerFrame = 5;

	// Token: 0x04004096 RID: 16534
	private static readonly List<int> APPROVED_LAYERS = new List<int>
	{
		0, 1, 2, 4, 5, 9, 11, 18, 22, 27,
		30
	};

	// Token: 0x04004097 RID: 16535
	private static bool isLoading;

	// Token: 0x04004098 RID: 16536
	private static bool isUnloading;

	// Token: 0x04004099 RID: 16537
	private static bool runningAsyncLoad = false;

	// Token: 0x0400409A RID: 16538
	private static long attemptedLoadID = 0L;

	// Token: 0x0400409B RID: 16539
	private static string attemptedSceneToLoad;

	// Token: 0x0400409C RID: 16540
	private static bool shouldUnloadMod = false;

	// Token: 0x0400409D RID: 16541
	private static AssetBundle mapBundle;

	// Token: 0x0400409E RID: 16542
	private static string initialSceneName = string.Empty;

	// Token: 0x0400409F RID: 16543
	private static int initialSceneIndex = 0;

	// Token: 0x040040A0 RID: 16544
	private static byte maxPlayersForMap = 10;

	// Token: 0x040040A1 RID: 16545
	private static string loadedMapLevelName;

	// Token: 0x040040A2 RID: 16546
	private static long loadedMapModId;

	// Token: 0x040040A3 RID: 16547
	private static long loadedMapModFileId;

	// Token: 0x040040A4 RID: 16548
	private static MapDescriptor loadedMapDescriptor;

	// Token: 0x040040A5 RID: 16549
	private static Action<MapLoadStatus, int, string> modLoadProgressCallback;

	// Token: 0x040040A6 RID: 16550
	private static Action<bool> modLoadedCallback;

	// Token: 0x040040A7 RID: 16551
	private static Coroutine sceneLoadingCoroutine;

	// Token: 0x040040A8 RID: 16552
	private static Action<string> sceneLoadedCallback;

	// Token: 0x040040A9 RID: 16553
	private static Action<string> sceneUnloadedCallback;

	// Token: 0x040040AA RID: 16554
	private static List<CustomMapLoader.LoadZoneRequest> queuedLoadZoneRequests = new List<CustomMapLoader.LoadZoneRequest>();

	// Token: 0x040040AB RID: 16555
	private static string[] assetBundleSceneFilePaths;

	// Token: 0x040040AC RID: 16556
	private static List<string> loadedSceneFilePaths = new List<string>();

	// Token: 0x040040AD RID: 16557
	private static List<string> loadedSceneNames = new List<string>();

	// Token: 0x040040AE RID: 16558
	private static List<int> loadedSceneIndexes = new List<int>();

	// Token: 0x040040AF RID: 16559
	private static int leafGliderIndex;

	// Token: 0x040040B0 RID: 16560
	private static bool usingDynamicLighting = false;

	// Token: 0x040040B1 RID: 16561
	private static int totalObjectsInLoadingScene = 0;

	// Token: 0x040040B2 RID: 16562
	private static int objectsProcessedForLoadingScene = 0;

	// Token: 0x040040B3 RID: 16563
	private static int objectsProcessedThisFrame = 0;

	// Token: 0x040040B4 RID: 16564
	private static List<Component> initializePhaseTwoComponents = new List<Component>();

	// Token: 0x040040B5 RID: 16565
	private static List<AIAgent> agentsToCreate = new List<AIAgent>(64);

	// Token: 0x040040B6 RID: 16566
	private static bool shouldAbortSceneLoad = false;

	// Token: 0x040040B7 RID: 16567
	private static Action abortModLoadCallback;

	// Token: 0x040040B8 RID: 16568
	private static Action unloadModCallback;

	// Token: 0x040040B9 RID: 16569
	private static string cachedExceptionMessage = "";

	// Token: 0x040040BA RID: 16570
	private static LightmapData[] lightmaps;

	// Token: 0x040040BB RID: 16571
	private static List<Texture2D> lightmapsToKeep = new List<Texture2D>();

	// Token: 0x040040BC RID: 16572
	private static List<GameObject> placeholderReplacements = new List<GameObject>();

	// Token: 0x040040BD RID: 16573
	private static ATM_UI customMapATM = null;

	// Token: 0x040040BE RID: 16574
	private static List<GameObject> storeCheckouts = new List<GameObject>();

	// Token: 0x040040BF RID: 16575
	private static List<GameObject> storeDisplayStands = new List<GameObject>();

	// Token: 0x040040C0 RID: 16576
	private static List<GameObject> storeTryOnConsoles = new List<GameObject>();

	// Token: 0x040040C1 RID: 16577
	private static List<GameObject> storeTryOnAreas = new List<GameObject>();

	// Token: 0x040040C2 RID: 16578
	private string dontDestroyOnLoadSceneName = "";

	// Token: 0x040040C3 RID: 16579
	private static List<Type> componentAllowlist = new List<Type>
	{
		typeof(MeshRenderer),
		typeof(Transform),
		typeof(MeshFilter),
		typeof(MeshRenderer),
		typeof(Collider),
		typeof(BoxCollider),
		typeof(SphereCollider),
		typeof(CapsuleCollider),
		typeof(MeshCollider),
		typeof(Light),
		typeof(ReflectionProbe),
		typeof(AudioSource),
		typeof(Animator),
		typeof(SkinnedMeshRenderer),
		typeof(TextMesh),
		typeof(ParticleSystem),
		typeof(ParticleSystemRenderer),
		typeof(RectTransform),
		typeof(SpriteRenderer),
		typeof(BillboardRenderer),
		typeof(Canvas),
		typeof(CanvasRenderer),
		typeof(CanvasScaler),
		typeof(GraphicRaycaster),
		typeof(Rigidbody),
		typeof(TrailRenderer),
		typeof(LineRenderer),
		typeof(LensFlareComponentSRP),
		typeof(Camera),
		typeof(UniversalAdditionalCameraData),
		typeof(NavMeshAgent),
		typeof(NavMesh),
		typeof(NavMeshObstacle),
		typeof(NavMeshLink),
		typeof(NavMeshModifierVolume),
		typeof(NavMeshModifier),
		typeof(NavMeshSurface),
		typeof(HingeJoint),
		typeof(ConstantForce),
		typeof(MapDescriptor),
		typeof(AccessDoorPlaceholder),
		typeof(MapOrientationPoint),
		typeof(SurfaceOverrideSettings),
		typeof(TeleporterSettings),
		typeof(TagZoneSettings),
		typeof(MapBoundarySettings),
		typeof(ObjectActivationTriggerSettings),
		typeof(LoadZoneSettings),
		typeof(GTObjectPlaceholder),
		typeof(CMSZoneShaderSettings),
		typeof(ZoneShaderTriggerSettings),
		typeof(MultiPartFire),
		typeof(HandHoldSettings),
		typeof(CustomMapEjectButtonSettings),
		typeof(global::CustomMapSupport.BezierSpline),
		typeof(UberShaderDynamicLight),
		typeof(AIAgent),
		typeof(AISpawnManager),
		typeof(AISpawnPoint),
		typeof(ProBuilderMesh),
		typeof(TMP_Text),
		typeof(TextMeshPro),
		typeof(TextMeshProUGUI),
		typeof(UniversalAdditionalLightData),
		typeof(BakerySkyLight),
		typeof(BakeryDirectLight),
		typeof(BakeryPointLight),
		typeof(ftLightmapsStorage)
	};

	// Token: 0x040040C4 RID: 16580
	private static readonly List<string> componentTypeStringAllowList = new List<string> { "UnityEngine.Halo" };

	// Token: 0x040040C5 RID: 16581
	private static Type[] badComponents = new Type[]
	{
		typeof(EventTrigger),
		typeof(UIBehaviour),
		typeof(GorillaPressableButton),
		typeof(GorillaPressableDelayButton),
		typeof(Camera),
		typeof(AudioListener),
		typeof(VideoPlayer)
	};

	// Token: 0x040040C6 RID: 16582
	[CompilerGenerated]
	private static bool <CanLoadEntities>k__BackingField;

	// Token: 0x02000810 RID: 2064
	private struct LoadZoneRequest
	{
		// Token: 0x040040C7 RID: 16583
		public int[] sceneIndexesToLoad;

		// Token: 0x040040C8 RID: 16584
		public int[] sceneIndexesToUnload;

		// Token: 0x040040C9 RID: 16585
		public Action<string> onSceneLoadedCallback;

		// Token: 0x040040CA RID: 16586
		public Action<string> onSceneUnloadedCallback;
	}

	// Token: 0x02000811 RID: 2065
	[CompilerGenerated]
	private sealed class <AbortSceneLoad>d__106 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033D4 RID: 13268 RVA: 0x0010E4BF File Offset: 0x0010C6BF
		[DebuggerHidden]
		public <AbortSceneLoad>d__106(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x0010E4D0 File Offset: 0x0010C6D0
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
				this.<>1__state = -1;
				if (sceneIndex == -1)
				{
					CustomMapLoader.shouldUnloadMod = true;
				}
				CustomMapLoader.isLoading = false;
				if (CustomMapLoader.shouldUnloadMod)
				{
					if (CustomMapLoader.sceneLoadingCoroutine != null)
					{
						CustomMapLoader.instance.StopCoroutine(CustomMapLoader.sceneLoadingCoroutine);
						CustomMapLoader.sceneLoadingCoroutine = null;
					}
					this.<>2__current = CustomMapLoader.UnloadAllScenesCoroutine();
					this.<>1__state = 1;
					return true;
				}
				this.<>2__current = CustomMapLoader.UnloadSceneCoroutine(sceneIndex, null);
				this.<>1__state = 2;
				return true;
			case 1:
			{
				this.<>1__state = -1;
				if (CustomMapLoader.mapBundle != null)
				{
					CustomMapLoader.mapBundle.Unload(true);
				}
				CustomMapLoader.mapBundle = null;
				Action abortModLoadCallback = CustomMapLoader.abortModLoadCallback;
				if (abortModLoadCallback != null)
				{
					abortModLoadCallback();
				}
				break;
			}
			case 2:
				this.<>1__state = -1;
				break;
			default:
				return false;
			}
			CustomMapLoader.abortModLoadCallback = null;
			CustomMapLoader.shouldAbortSceneLoad = false;
			CustomMapLoader.shouldUnloadMod = false;
			return false;
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060033D7 RID: 13271 RVA: 0x0010E5B9 File Offset: 0x0010C7B9
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x0010E5B9 File Offset: 0x0010C7B9
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040CB RID: 16587
		private int <>1__state;

		// Token: 0x040040CC RID: 16588
		private object <>2__current;

		// Token: 0x040040CD RID: 16589
		public int sceneIndex;
	}

	// Token: 0x02000812 RID: 2066
	[CompilerGenerated]
	private sealed class <CloseDoorAndUnloadModCoroutine>d__110 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033DA RID: 13274 RVA: 0x0010E5C1 File Offset: 0x0010C7C1
		[DebuggerHidden]
		public <CloseDoorAndUnloadModCoroutine>d__110(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x0010E5D0 File Offset: 0x0010C7D0
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this.<>1__state = -1;
				return false;
			}
			else
			{
				this.<>1__state = -1;
				if (!CustomMapLoader.IsModLoaded(0L) || CustomMapLoader.IsLoading)
				{
					return false;
				}
				if (CustomMapLoader.instance.accessDoor != null)
				{
					CustomMapLoader.instance.accessDoor.CloseDoor();
				}
				CustomMapLoader.shouldUnloadMod = true;
				if (CustomMapLoader.mapBundle != null)
				{
					CustomMapLoader.mapBundle.Unload(true);
				}
				CustomMapLoader.mapBundle = null;
				CustomMapTelemetry.EndMapTracking();
				CMSSerializer.ResetSyncedMapObjects();
				this.<>2__current = CustomMapLoader.UnloadAllScenesCoroutine();
				this.<>1__state = 1;
				return true;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x0010E678 File Offset: 0x0010C878
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060033DF RID: 13279 RVA: 0x0010E678 File Offset: 0x0010C878
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040CE RID: 16590
		private int <>1__state;

		// Token: 0x040040CF RID: 16591
		private object <>2__current;
	}

	// Token: 0x02000813 RID: 2067
	[CompilerGenerated]
	private sealed class <LoadAssetBundle>d__104 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033E0 RID: 13280 RVA: 0x0010E680 File Offset: 0x0010C880
		[DebuggerHidden]
		public <LoadAssetBundle>d__104(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x0010E690 File Offset: 0x0010C890
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
				this.<>1__state = -1;
				if (CustomMapLoader.isLoading)
				{
					Action<bool, bool> action = OnLoadComplete;
					if (action != null)
					{
						action(false, false);
					}
					return false;
				}
				this.<>2__current = CustomMapLoader.CloseDoorAndUnloadModCoroutine();
				this.<>1__state = 1;
				return true;
			case 1:
			{
				this.<>1__state = -1;
				if (CustomMapLoader.shouldAbortSceneLoad)
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(-1);
					this.<>1__state = 2;
					return true;
				}
				CustomMapLoader.isLoading = true;
				CustomMapLoader.attemptedLoadID = mapModID;
				Action<MapLoadStatus, int, string> modLoadProgressCallback = CustomMapLoader.modLoadProgressCallback;
				if (modLoadProgressCallback != null)
				{
					modLoadProgressCallback(MapLoadStatus.Loading, 1, "GRABBING LIGHTMAP DATA");
				}
				CustomMapLoader.lightmaps = new LightmapData[LightmapSettings.lightmaps.Length];
				if (CustomMapLoader.lightmapsToKeep.Count > 0)
				{
					CustomMapLoader.lightmapsToKeep.Clear();
				}
				CustomMapLoader.lightmapsToKeep = new List<Texture2D>(LightmapSettings.lightmaps.Length * 2);
				for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
				{
					CustomMapLoader.lightmaps[i] = LightmapSettings.lightmaps[i];
					if (LightmapSettings.lightmaps[i].lightmapColor != null)
					{
						CustomMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapColor);
					}
					if (LightmapSettings.lightmaps[i].lightmapDir != null)
					{
						CustomMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapDir);
					}
				}
				Action<MapLoadStatus, int, string> modLoadProgressCallback2 = CustomMapLoader.modLoadProgressCallback;
				if (modLoadProgressCallback2 != null)
				{
					modLoadProgressCallback2(MapLoadStatus.Loading, 2, "LOADING PACKAGE INFO");
				}
				MapPackageInfo packageInfo;
				try
				{
					packageInfo = CustomMapLoader.GetPackageInfo(packageInfoFilePath);
				}
				catch (Exception ex)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback3 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback3 != null)
					{
						modLoadProgressCallback3(MapLoadStatus.Error, 0, ex.Message);
					}
					return false;
				}
				if (packageInfo == null)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback4 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback4 != null)
					{
						modLoadProgressCallback4(MapLoadStatus.Error, 0, "FAILED TO READ FILE AT " + packageInfoFilePath);
					}
					OnLoadComplete(false, false);
					return false;
				}
				CustomMapLoader.initialSceneName = packageInfo.initialScene;
				Action<MapLoadStatus, int, string> modLoadProgressCallback5 = CustomMapLoader.modLoadProgressCallback;
				if (modLoadProgressCallback5 != null)
				{
					modLoadProgressCallback5(MapLoadStatus.Loading, 3, "PACKAGE INFO LOADED");
				}
				string text = Path.GetDirectoryName(packageInfoFilePath) + "/" + packageInfo.pcFileName;
				Action<MapLoadStatus, int, string> modLoadProgressCallback6 = CustomMapLoader.modLoadProgressCallback;
				if (modLoadProgressCallback6 != null)
				{
					modLoadProgressCallback6(MapLoadStatus.Loading, 12, "LOADING MAP ASSET BUNDLE");
				}
				loadBundleRequest = AssetBundle.LoadFromFileAsync(text);
				this.<>2__current = loadBundleRequest;
				this.<>1__state = 3;
				return true;
			}
			case 2:
				this.<>1__state = -1;
				OnLoadComplete(false, true);
				return false;
			case 3:
			{
				this.<>1__state = -1;
				CustomMapLoader.mapBundle = loadBundleRequest.assetBundle;
				if (CustomMapLoader.shouldAbortSceneLoad)
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(-1);
					this.<>1__state = 4;
					return true;
				}
				if (CustomMapLoader.mapBundle == null)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback7 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback7 != null)
					{
						modLoadProgressCallback7(MapLoadStatus.Error, 0, "CUSTOM MAP ASSET BUNDLE FAILED TO LOAD");
					}
					OnLoadComplete(false, false);
					return false;
				}
				if (!CustomMapLoader.mapBundle.isStreamedSceneAssetBundle)
				{
					CustomMapLoader.mapBundle.Unload(true);
					Action<MapLoadStatus, int, string> modLoadProgressCallback8 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback8 != null)
					{
						modLoadProgressCallback8(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
					}
					OnLoadComplete(false, false);
					return false;
				}
				Action<MapLoadStatus, int, string> modLoadProgressCallback9 = CustomMapLoader.modLoadProgressCallback;
				if (modLoadProgressCallback9 != null)
				{
					modLoadProgressCallback9(MapLoadStatus.Loading, 20, "MAP ASSET BUNDLE LOADED");
				}
				CustomMapLoader.mapBundle.GetAllAssetNames();
				CustomMapLoader.assetBundleSceneFilePaths = CustomMapLoader.mapBundle.GetAllScenePaths();
				if (CustomMapLoader.assetBundleSceneFilePaths.Length == 0)
				{
					CustomMapLoader.mapBundle.Unload(true);
					Action<MapLoadStatus, int, string> modLoadProgressCallback10 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback10 != null)
					{
						modLoadProgressCallback10(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
					}
					OnLoadComplete(false, false);
					return false;
				}
				foreach (string text2 in CustomMapLoader.assetBundleSceneFilePaths)
				{
					if (text2.Equals(CustomMapLoader.instance.dontDestroyOnLoadSceneName, StringComparison.OrdinalIgnoreCase))
					{
						CustomMapLoader.mapBundle.Unload(true);
						Action<MapLoadStatus, int, string> modLoadProgressCallback11 = CustomMapLoader.modLoadProgressCallback;
						if (modLoadProgressCallback11 != null)
						{
							modLoadProgressCallback11(MapLoadStatus.Error, 0, "Map name is " + text2 + " this is an invalid name");
						}
						OnLoadComplete(false, false);
						return false;
					}
				}
				OnLoadComplete(true, false);
				return false;
			}
			case 4:
				this.<>1__state = -1;
				OnLoadComplete(false, true);
				return false;
			default:
				return false;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x0010EABC File Offset: 0x0010CCBC
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060033E5 RID: 13285 RVA: 0x0010EABC File Offset: 0x0010CCBC
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040D0 RID: 16592
		private int <>1__state;

		// Token: 0x040040D1 RID: 16593
		private object <>2__current;

		// Token: 0x040040D2 RID: 16594
		public Action<bool, bool> OnLoadComplete;

		// Token: 0x040040D3 RID: 16595
		public long mapModID;

		// Token: 0x040040D4 RID: 16596
		public string packageInfoFilePath;

		// Token: 0x040040D5 RID: 16597
		private AssetBundleCreateRequest <loadBundleRequest>5__2;
	}

	// Token: 0x02000814 RID: 2068
	[CompilerGenerated]
	private sealed class <LoadSceneFromAssetBundle>d__108 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033E6 RID: 13286 RVA: 0x0010EAC4 File Offset: 0x0010CCC4
		[DebuggerHidden]
		public <LoadSceneFromAssetBundle>d__108(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0010EAD4 File Offset: 0x0010CCD4
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
			{
				this.<>1__state = -1;
				LoadSceneParameters loadSceneParameters = new LoadSceneParameters
				{
					loadSceneMode = LoadSceneMode.Additive,
					localPhysicsMode = LocalPhysicsMode.None
				};
				if (CustomMapLoader.shouldAbortSceneLoad)
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(sceneIndex);
					this.<>1__state = 1;
					return true;
				}
				CustomMapLoader.runningAsyncLoad = true;
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback != null)
					{
						modLoadProgressCallback(MapLoadStatus.Loading, 30, "LOADING MAP SCENE");
					}
				}
				CustomMapLoader.attemptedSceneToLoad = CustomMapLoader.assetBundleSceneFilePaths[sceneIndex];
				sceneName = CustomMapLoader.GetSceneNameFromFilePath(CustomMapLoader.attemptedSceneToLoad);
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(CustomMapLoader.attemptedSceneToLoad, loadSceneParameters);
				this.<>2__current = asyncOperation;
				this.<>1__state = 2;
				return true;
			}
			case 1:
				this.<>1__state = -1;
				OnLoadComplete(false, true, "");
				return false;
			case 2:
			{
				this.<>1__state = -1;
				CustomMapLoader.runningAsyncLoad = false;
				if (CustomMapLoader.shouldAbortSceneLoad)
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(sceneIndex);
					this.<>1__state = 3;
					return true;
				}
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback2 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback2 != null)
					{
						modLoadProgressCallback2(MapLoadStatus.Loading, 50, "SANITIZING MAP");
					}
				}
				GameObject[] rootGameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
				List<MapDescriptor> list = new List<MapDescriptor>();
				for (int i = 0; i < rootGameObjects.Length; i++)
				{
					list.AddRange(rootGameObjects[i].GetComponentsInChildren<MapDescriptor>());
				}
				MapDescriptor mapDescriptor = null;
				bool flag = false;
				foreach (MapDescriptor mapDescriptor2 in list)
				{
					if (!mapDescriptor2.IsNull())
					{
						if (!mapDescriptor.IsNull())
						{
							flag = true;
							break;
						}
						mapDescriptor = mapDescriptor2;
					}
				}
				if (flag)
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(sceneIndex);
					this.<>1__state = 4;
					return true;
				}
				if (mapDescriptor.IsNull())
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(sceneIndex);
					this.<>1__state = 5;
					return true;
				}
				GameObject gameObject = mapDescriptor.gameObject;
				if (!CustomMapLoader.SanitizeObject(gameObject, gameObject))
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(sceneIndex);
					this.<>1__state = 6;
					return true;
				}
				CustomMapLoader.totalObjectsInLoadingScene = 0;
				for (int j = 0; j < rootGameObjects.Length; j++)
				{
					CustomMapLoader.SanitizeObjectRecursive(rootGameObjects[j], gameObject);
				}
				CustomMapLoader.CheckVirtualStumpOverlap(sceneName);
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback3 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback3 != null)
					{
						modLoadProgressCallback3(MapLoadStatus.Loading, 70, "MAP SCENE LOADED");
					}
				}
				CustomMapLoader.leafGliderIndex = 0;
				this.<>2__current = CustomMapLoader.ProcessAndInstantiateMap(gameObject, useProgressCallback);
				this.<>1__state = 7;
				return true;
			}
			case 3:
				this.<>1__state = -1;
				OnLoadComplete(false, true, "");
				return false;
			case 4:
				this.<>1__state = -1;
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback4 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback4 != null)
					{
						modLoadProgressCallback4(MapLoadStatus.Error, 0, "MAP CONTAINS MULTIPLE MAP DESCRIPTOR OBJECTS");
					}
				}
				OnLoadComplete(false, false, "");
				return false;
			case 5:
				this.<>1__state = -1;
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback5 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback5 != null)
					{
						modLoadProgressCallback5(MapLoadStatus.Error, 0, "MAP SCENE DOES NOT CONTAIN A MAP DESCRIPTOR");
					}
				}
				OnLoadComplete(false, false, "");
				return false;
			case 6:
				this.<>1__state = -1;
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback6 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback6 != null)
					{
						modLoadProgressCallback6(MapLoadStatus.Error, 0, "MAP DESCRIPTOR HAS UNAPPROVED COMPONENTS ON IT");
					}
				}
				OnLoadComplete(false, false, "");
				return false;
			case 7:
				this.<>1__state = -1;
				this.<>2__current = null;
				this.<>1__state = 8;
				return true;
			case 8:
				this.<>1__state = -1;
				if (CustomMapLoader.shouldAbortSceneLoad)
				{
					this.<>2__current = CustomMapLoader.AbortSceneLoad(sceneIndex);
					this.<>1__state = 9;
					return true;
				}
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback7 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback7 != null)
					{
						modLoadProgressCallback7(MapLoadStatus.Loading, 99, "FINALIZING MAP");
					}
				}
				CustomMapLoader.loadedSceneFilePaths.AddIfNew(CustomMapLoader.attemptedSceneToLoad);
				CustomMapLoader.loadedSceneNames.AddIfNew(sceneName);
				CustomMapLoader.loadedSceneIndexes.AddIfNew(sceneIndex);
				OnLoadComplete(true, false, sceneName);
				return false;
			case 9:
				this.<>1__state = -1;
				OnLoadComplete(false, true, "");
				if (CustomMapLoader.cachedExceptionMessage.Length > 0 && useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback8 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback8 != null)
					{
						modLoadProgressCallback8(MapLoadStatus.Error, 0, CustomMapLoader.cachedExceptionMessage);
					}
				}
				return false;
			default:
				return false;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x0010EF58 File Offset: 0x0010D158
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x0010EF58 File Offset: 0x0010D158
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040D6 RID: 16598
		private int <>1__state;

		// Token: 0x040040D7 RID: 16599
		private object <>2__current;

		// Token: 0x040040D8 RID: 16600
		public int sceneIndex;

		// Token: 0x040040D9 RID: 16601
		public Action<bool, bool, string> OnLoadComplete;

		// Token: 0x040040DA RID: 16602
		public bool useProgressCallback;

		// Token: 0x040040DB RID: 16603
		private string <sceneName>5__2;
	}

	// Token: 0x02000815 RID: 2069
	[CompilerGenerated]
	private sealed class <LoadScenesCoroutine>d__102 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033EC RID: 13292 RVA: 0x0010EF60 File Offset: 0x0010D160
		[DebuggerHidden]
		public <LoadScenesCoroutine>d__102(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0010EF70 File Offset: 0x0010D170
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			if (num == 0)
			{
				this.<>1__state = -1;
				i = 0;
				goto IL_0084;
			}
			if (num != 1)
			{
				return false;
			}
			this.<>1__state = -1;
			IL_0074:
			int num2 = i;
			i = num2 + 1;
			IL_0084:
			if (i >= sceneIndexes.Length)
			{
				return false;
			}
			if (!CustomMapLoader.loadedSceneFilePaths.Contains(CustomMapLoader.assetBundleSceneFilePaths[sceneIndexes[i]]))
			{
				this.<>2__current = CustomMapLoader.LoadSceneFromAssetBundle(sceneIndexes[i], false, new Action<bool, bool, string>(CustomMapLoader.OnIncrementalLoadComplete));
				this.<>1__state = 1;
				return true;
			}
			goto IL_0074;
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x0010F012 File Offset: 0x0010D212
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060033F1 RID: 13297 RVA: 0x0010F012 File Offset: 0x0010D212
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040DC RID: 16604
		private int <>1__state;

		// Token: 0x040040DD RID: 16605
		private object <>2__current;

		// Token: 0x040040DE RID: 16606
		public int[] sceneIndexes;

		// Token: 0x040040DF RID: 16607
		private int <i>5__2;
	}

	// Token: 0x02000816 RID: 2070
	[CompilerGenerated]
	private sealed class <LoadZoneCoroutine>d__101 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033F2 RID: 13298 RVA: 0x0010F01A File Offset: 0x0010D21A
		[DebuggerHidden]
		public <LoadZoneCoroutine>d__101(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x0010F02C File Offset: 0x0010D22C
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
				this.<>1__state = -1;
				if (!unloadScenes.IsNullOrEmpty<int>())
				{
					this.<>2__current = CustomMapLoader.UnloadScenesCoroutine(unloadScenes);
					this.<>1__state = 1;
					return true;
				}
				break;
			case 1:
				this.<>1__state = -1;
				break;
			case 2:
				this.<>1__state = -1;
				goto IL_007E;
			default:
				return false;
			}
			if (!loadScenes.IsNullOrEmpty<int>())
			{
				this.<>2__current = CustomMapLoader.LoadScenesCoroutine(loadScenes);
				this.<>1__state = 2;
				return true;
			}
			IL_007E:
			if (CustomMapLoader.sceneLoadingCoroutine != null)
			{
				CustomMapLoader.instance.StopCoroutine(CustomMapLoader.sceneLoadingCoroutine);
				CustomMapLoader.sceneLoadingCoroutine = null;
			}
			if (CustomMapLoader.queuedLoadZoneRequests.Count > 0)
			{
				CustomMapLoader.LoadZoneRequest loadZoneRequest = CustomMapLoader.queuedLoadZoneRequests[0];
				CustomMapLoader.queuedLoadZoneRequests.RemoveAt(0);
				CustomMapLoader.LoadZoneTriggered(loadZoneRequest.sceneIndexesToLoad, loadZoneRequest.sceneIndexesToUnload, loadZoneRequest.onSceneLoadedCallback, loadZoneRequest.onSceneUnloadedCallback);
			}
			return false;
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x0010F117 File Offset: 0x0010D317
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060033F7 RID: 13303 RVA: 0x0010F117 File Offset: 0x0010D317
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040E0 RID: 16608
		private int <>1__state;

		// Token: 0x040040E1 RID: 16609
		private object <>2__current;

		// Token: 0x040040E2 RID: 16610
		public int[] unloadScenes;

		// Token: 0x040040E3 RID: 16611
		public int[] loadScenes;
	}

	// Token: 0x02000817 RID: 2071
	[CompilerGenerated]
	private sealed class <ProcessAndInstantiateMap>d__120 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033F8 RID: 13304 RVA: 0x0010F11F File Offset: 0x0010D31F
		[DebuggerHidden]
		public <ProcessAndInstantiateMap>d__120(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x0010F130 File Offset: 0x0010D330
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
				this.<>1__state = -1;
				if (map.IsNull() || !CustomMapLoader.hasInstance)
				{
					return false;
				}
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback != null)
					{
						modLoadProgressCallback(MapLoadStatus.Loading, 73, "PROCESSING ROOT MAP OBJECT");
					}
				}
				CustomMapLoader.loadedMapDescriptor = map.GetComponent<MapDescriptor>();
				if (CustomMapLoader.loadedMapDescriptor.IsNull())
				{
					return false;
				}
				if (CustomMapLoader.loadedMapDescriptor.IsInitialScene && CustomMapLoader.loadedMapDescriptor.UseUberShaderDynamicLighting)
				{
					GameLightingManager.instance.SetCustomDynamicLightingEnabled(true);
					GameLightingManager.instance.SetAmbientLightDynamic(CustomMapLoader.loadedMapDescriptor.UberShaderAmbientDynamicLight);
					CustomMapLoader.usingDynamicLighting = true;
				}
				CustomMapLoader.objectsProcessedForLoadingScene = 0;
				CustomMapLoader.objectsProcessedThisFrame = 0;
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback2 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback2 != null)
					{
						modLoadProgressCallback2(MapLoadStatus.Loading, 75, "PROCESSING CHILD OBJECTS");
					}
				}
				CustomMapLoader.initializePhaseTwoComponents.Clear();
				CustomMapLoader.agentsToCreate.Clear();
				this.<>2__current = CustomMapLoader.ProcessChildObjects(map, useProgressCallback);
				this.<>1__state = 1;
				return true;
			case 1:
				this.<>1__state = -1;
				if (CustomMapLoader.shouldAbortSceneLoad)
				{
					return false;
				}
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback3 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback3 != null)
					{
						modLoadProgressCallback3(MapLoadStatus.Loading, 95, "PROCESSING COMPLETE");
					}
				}
				this.<>2__current = null;
				this.<>1__state = 2;
				return true;
			case 2:
				this.<>1__state = -1;
				CustomMapLoader.InitializeComponentsPhaseTwo();
				CustomMapLoader.placeholderReplacements.Clear();
				if (useProgressCallback)
				{
					Action<MapLoadStatus, int, string> modLoadProgressCallback4 = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback4 != null)
					{
						modLoadProgressCallback4(MapLoadStatus.Loading, 97, "PROCESSING COMPLETE");
					}
				}
				if (CustomMapLoader.loadedMapDescriptor.IsInitialScene)
				{
					CustomMapLoader.maxPlayersForMap = (byte)Math.Clamp(CustomMapLoader.loadedMapDescriptor.MaxPlayers, 1, 10);
					VirtualStumpReturnWatch.SetWatchProperties(CustomMapLoader.loadedMapDescriptor.GetReturnToVStumpWatchProps());
				}
				return false;
			default:
				return false;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060033FB RID: 13307 RVA: 0x0010F2FE File Offset: 0x0010D4FE
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060033FD RID: 13309 RVA: 0x0010F2FE File Offset: 0x0010D4FE
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040E4 RID: 16612
		private int <>1__state;

		// Token: 0x040040E5 RID: 16613
		private object <>2__current;

		// Token: 0x040040E6 RID: 16614
		public GameObject map;

		// Token: 0x040040E7 RID: 16615
		public bool useProgressCallback;
	}

	// Token: 0x02000818 RID: 2072
	[CompilerGenerated]
	private sealed class <ProcessChildObjects>d__121 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060033FE RID: 13310 RVA: 0x0010F306 File Offset: 0x0010D506
		[DebuggerHidden]
		public <ProcessChildObjects>d__121(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x0010F318 File Offset: 0x0010D518
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
				this.<>1__state = -1;
				if (parent == null || CustomMapLoader.placeholderReplacements.Contains(parent))
				{
					return false;
				}
				i = 0;
				goto IL_01A6;
			case 1:
				this.<>1__state = -1;
				if (CustomMapLoader.shouldAbortSceneLoad)
				{
					return false;
				}
				break;
			case 2:
				this.<>1__state = -1;
				goto IL_0194;
			default:
				return false;
			}
			IL_010B:
			if (CustomMapLoader.shouldAbortSceneLoad)
			{
				return false;
			}
			CustomMapLoader.objectsProcessedForLoadingScene++;
			CustomMapLoader.objectsProcessedThisFrame++;
			if (CustomMapLoader.objectsProcessedThisFrame >= CustomMapLoader.numObjectsToProcessPerFrame)
			{
				CustomMapLoader.objectsProcessedThisFrame = 0;
				if (useProgressCallback)
				{
					float num = (float)CustomMapLoader.objectsProcessedForLoadingScene / (float)CustomMapLoader.totalObjectsInLoadingScene;
					int num2 = Mathf.FloorToInt(20f * num);
					Action<MapLoadStatus, int, string> modLoadProgressCallback = CustomMapLoader.modLoadProgressCallback;
					if (modLoadProgressCallback != null)
					{
						modLoadProgressCallback(MapLoadStatus.Loading, 75 + num2, "PROCESSING CHILD OBJECTS");
					}
				}
				this.<>2__current = null;
				this.<>1__state = 2;
				return true;
			}
			IL_0194:
			int num3 = i;
			i = num3 + 1;
			IL_01A6:
			if (i < parent.transform.childCount)
			{
				Transform child = parent.transform.GetChild(i);
				if (child == null)
				{
					goto IL_0194;
				}
				GameObject gameObject = child.gameObject;
				if (gameObject == null || CustomMapLoader.placeholderReplacements.Contains(gameObject))
				{
					goto IL_0194;
				}
				try
				{
					CustomMapLoader.SetupCollisions(gameObject);
					CustomMapLoader.ReplaceDataOnlyScripts(gameObject);
					CustomMapLoader.ReplacePlaceholders(gameObject);
					CustomMapLoader.SetupDynamicLight(gameObject);
					CustomMapLoader.StoreAIAgent(gameObject);
					CustomMapLoader.InitializeComponentsPhaseOne(gameObject);
				}
				catch (Exception ex)
				{
					CustomMapLoader.shouldAbortSceneLoad = true;
					CustomMapLoader.cachedExceptionMessage = ex.Message;
					return false;
				}
				if (gameObject.transform.childCount > 0)
				{
					this.<>2__current = CustomMapLoader.ProcessChildObjects(gameObject, useProgressCallback);
					this.<>1__state = 1;
					return true;
				}
				goto IL_010B;
			}
			return false;
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x0010F4F8 File Offset: 0x0010D6F8
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x0010F4F8 File Offset: 0x0010D6F8
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040E8 RID: 16616
		private int <>1__state;

		// Token: 0x040040E9 RID: 16617
		private object <>2__current;

		// Token: 0x040040EA RID: 16618
		public GameObject parent;

		// Token: 0x040040EB RID: 16619
		public bool useProgressCallback;

		// Token: 0x040040EC RID: 16620
		private int <i>5__2;
	}

	// Token: 0x02000819 RID: 2073
	[CompilerGenerated]
	private sealed class <ResetLightmaps>d__115 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06003404 RID: 13316 RVA: 0x0010F500 File Offset: 0x0010D700
		[DebuggerHidden]
		public <ResetLightmaps>d__115(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x0010F510 File Offset: 0x0010D710
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
			{
				this.<>1__state = -1;
				CustomMapLoader.instance.dayNightManager.RequestRepopulateLightmaps();
				LoadSceneParameters loadSceneParameters = new LoadSceneParameters
				{
					loadSceneMode = LoadSceneMode.Additive,
					localPhysicsMode = LocalPhysicsMode.None
				};
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(10, loadSceneParameters);
				this.<>2__current = asyncOperation;
				this.<>1__state = 1;
				return true;
			}
			case 1:
			{
				this.<>1__state = -1;
				AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(10);
				this.<>2__current = asyncOperation;
				this.<>1__state = 2;
				return true;
			}
			case 2:
				this.<>1__state = -1;
				return false;
			default:
				return false;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06003407 RID: 13319 RVA: 0x0010F5AA File Offset: 0x0010D7AA
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06003409 RID: 13321 RVA: 0x0010F5AA File Offset: 0x0010D7AA
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040ED RID: 16621
		private int <>1__state;

		// Token: 0x040040EE RID: 16622
		private object <>2__current;
	}

	// Token: 0x0200081A RID: 2074
	[CompilerGenerated]
	private sealed class <UnloadAllScenesCoroutine>d__111 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0600340A RID: 13322 RVA: 0x0010F5B2 File Offset: 0x0010D7B2
		[DebuggerHidden]
		public <UnloadAllScenesCoroutine>d__111(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x0010F5C4 File Offset: 0x0010D7C4
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
				this.<>1__state = -1;
				CustomMapLoader.isLoading = false;
				CustomMapLoader.isUnloading = true;
				CustomMapLoader.CanLoadEntities = false;
				ZoneShaderSettings.ActivateDefaultSettings();
				CustomMapLoader.RemoveCustomMapATM();
				if (CustomMapLoader.assetBundleSceneFilePaths.IsNullOrEmpty<string>())
				{
					goto IL_0094;
				}
				sceneIndex = 0;
				break;
			case 1:
			{
				this.<>1__state = -1;
				int num = sceneIndex;
				sceneIndex = num + 1;
				break;
			}
			case 2:
			{
				this.<>1__state = -1;
				CustomMapLoader.assetBundleSceneFilePaths = new string[] { "" };
				CustomMapLoader.loadedMapModId = 0L;
				CustomMapLoader.initialSceneIndex = 0;
				CustomMapLoader.initialSceneName = "";
				CustomMapLoader.maxPlayersForMap = 10;
				Action unloadModCallback = CustomMapLoader.unloadModCallback;
				if (unloadModCallback != null)
				{
					unloadModCallback();
				}
				CustomMapLoader.unloadModCallback = null;
				CustomMapLoader.shouldUnloadMod = false;
				CustomMapLoader.usingDynamicLighting = false;
				GameLightingManager.instance.SetCustomDynamicLightingEnabled(false);
				GameLightingManager.instance.SetAmbientLightDynamic(Color.black);
				return false;
			}
			default:
				return false;
			}
			if (sceneIndex < CustomMapLoader.assetBundleSceneFilePaths.Length)
			{
				this.<>2__current = CustomMapLoader.UnloadSceneCoroutine(sceneIndex, null);
				this.<>1__state = 1;
				return true;
			}
			IL_0094:
			CustomMapLoader.loadedMapDescriptor = null;
			CustomMapLoader.loadedSceneFilePaths.Clear();
			CustomMapLoader.loadedSceneNames.Clear();
			CustomMapLoader.loadedSceneIndexes.Clear();
			for (int i = 0; i < CustomMapLoader.instance.leafGliders.Length; i++)
			{
				CustomMapLoader.instance.leafGliders[i].CustomMapUnload();
				CustomMapLoader.instance.leafGliders[i].enabled = false;
				CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].transform.GetChild(0).gameObject.SetActive(false);
			}
			GorillaNetworkJoinTrigger.EnableTriggerJoins();
			LightmapSettings.lightmaps = CustomMapLoader.lightmaps;
			CustomMapLoader.UnloadLightmaps();
			Resources.UnloadUnusedAssets();
			CustomMapLoader.isUnloading = false;
			if (CustomMapLoader.shouldUnloadMod)
			{
				this.<>2__current = CustomMapLoader.ResetLightmaps();
				this.<>1__state = 2;
				return true;
			}
			return false;
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x0010F7A8 File Offset: 0x0010D9A8
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x0010F7A8 File Offset: 0x0010D9A8
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040EF RID: 16623
		private int <>1__state;

		// Token: 0x040040F0 RID: 16624
		private object <>2__current;

		// Token: 0x040040F1 RID: 16625
		private int <sceneIndex>5__2;
	}

	// Token: 0x0200081B RID: 2075
	[CompilerGenerated]
	private sealed class <UnloadSceneCoroutine>d__112 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06003410 RID: 13328 RVA: 0x0010F7B0 File Offset: 0x0010D9B0
		[DebuggerHidden]
		public <UnloadSceneCoroutine>d__112(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x0010F7C0 File Offset: 0x0010D9C0
		bool IEnumerator.MoveNext()
		{
			switch (this.<>1__state)
			{
			case 0:
				this.<>1__state = -1;
				if (!CustomMapLoader.hasInstance)
				{
					return false;
				}
				if (sceneIndex < 0 || sceneIndex >= CustomMapLoader.assetBundleSceneFilePaths.Length)
				{
					Debug.LogError(string.Format("[CustomMapLoader::UnloadSceneCoroutine] SceneIndex of {0} is invalid! ", sceneIndex) + string.Format("The currently loaded AssetBundle contains {0} scenes.", CustomMapLoader.assetBundleSceneFilePaths.Length));
					return false;
				}
				break;
			case 1:
				this.<>1__state = -1;
				break;
			case 2:
			{
				this.<>1__state = -1;
				CustomMapLoader.loadedSceneFilePaths.Remove(scenePathWithExtension);
				CustomMapLoader.loadedSceneNames.Remove(sceneName);
				CustomMapLoader.loadedSceneIndexes.Remove(sceneIndex);
				Action<string> sceneUnloadedCallback = CustomMapLoader.sceneUnloadedCallback;
				if (sceneUnloadedCallback != null)
				{
					sceneUnloadedCallback(sceneName);
				}
				Action action = OnUnloadComplete;
				if (action != null)
				{
					action();
				}
				return false;
			}
			default:
				return false;
			}
			if (CustomMapLoader.runningAsyncLoad)
			{
				this.<>2__current = null;
				this.<>1__state = 1;
				return true;
			}
			UnloadSceneOptions unloadSceneOptions = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
			scenePathWithExtension = CustomMapLoader.assetBundleSceneFilePaths[sceneIndex];
			string[] array = scenePathWithExtension.Split(".", StringSplitOptions.None);
			string text = "";
			sceneName = "";
			if (!array.IsNullOrEmpty<string>())
			{
				text = array[0];
				if (text.Length > 0)
				{
					sceneName = Path.GetFileName(text);
				}
			}
			Scene sceneByName = SceneManager.GetSceneByName(text);
			if (sceneByName.IsValid())
			{
				if (CustomMapLoader.customMapATM.IsNotNull() && CustomMapLoader.customMapATM.gameObject.scene.Equals(sceneByName))
				{
					CustomMapLoader.RemoveCustomMapATM();
				}
				CustomMapLoader.RemoveUnloadingStorePrefabs(sceneByName);
				AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenePathWithExtension, unloadSceneOptions);
				this.<>2__current = asyncOperation;
				this.<>1__state = 2;
				return true;
			}
			return false;
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06003413 RID: 13331 RVA: 0x0010F992 File Offset: 0x0010DB92
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x0010F992 File Offset: 0x0010DB92
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040F2 RID: 16626
		private int <>1__state;

		// Token: 0x040040F3 RID: 16627
		private object <>2__current;

		// Token: 0x040040F4 RID: 16628
		public int sceneIndex;

		// Token: 0x040040F5 RID: 16629
		public Action OnUnloadComplete;

		// Token: 0x040040F6 RID: 16630
		private string <scenePathWithExtension>5__2;

		// Token: 0x040040F7 RID: 16631
		private string <sceneName>5__3;
	}

	// Token: 0x0200081C RID: 2076
	[CompilerGenerated]
	private sealed class <UnloadScenesCoroutine>d__103 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06003416 RID: 13334 RVA: 0x0010F99A File Offset: 0x0010DB9A
		[DebuggerHidden]
		public <UnloadScenesCoroutine>d__103(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x0010F9AC File Offset: 0x0010DBAC
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this.<>1__state = -1;
				int num2 = i;
				i = num2 + 1;
			}
			else
			{
				this.<>1__state = -1;
				i = 0;
			}
			if (i >= sceneIndexes.Length)
			{
				return false;
			}
			this.<>2__current = CustomMapLoader.UnloadSceneCoroutine(sceneIndexes[i], null);
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x0010FA23 File Offset: 0x0010DC23
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x0010FA23 File Offset: 0x0010DC23
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040040F8 RID: 16632
		private int <>1__state;

		// Token: 0x040040F9 RID: 16633
		private object <>2__current;

		// Token: 0x040040FA RID: 16634
		public int[] sceneIndexes;

		// Token: 0x040040FB RID: 16635
		private int <i>5__2;
	}
}
