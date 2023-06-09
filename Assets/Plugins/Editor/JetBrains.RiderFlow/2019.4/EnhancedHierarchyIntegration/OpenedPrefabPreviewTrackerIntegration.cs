using System;
using System.Reflection;
using JetBrains.RiderFlow.Core.Services.Caches.GameObjects;
using JetBrains.RiderFlow.Core.Utils;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
// Need to use prefabAssetPath in Unity 2019.4 so warning is disabled
#pragma warning disable CS0618

namespace JetBrains.RiderFlow.Since2019_4.EnhancedHierarchyIntegration
{
    public static class OpenedPrefabPreviewTrackerIntegration
    {
        public static void Initialize()
        {
            Action<PrefabStage> callbackOnPrefabOpened = null;
            Action<PrefabStage> callbackOnPrefabClosing = null;
            OpenedPrefabPreviewTracker.SubscribeForPrefabPreviewEvents = tracker =>
            {
                callbackOnPrefabOpened = stage => OnPrefabOpened(stage, tracker);
                PrefabStage.prefabStageOpened += callbackOnPrefabOpened;
                callbackOnPrefabClosing = stage => OnPrefabClosing(stage, tracker);
                PrefabStage.prefabStageClosing += callbackOnPrefabClosing;
            };
            OpenedPrefabPreviewTracker.UnsubscribeFromPrefabPreviewEvents = tracker =>
            {
                if (callbackOnPrefabOpened != null) PrefabStage.prefabStageOpened -= callbackOnPrefabOpened;
                if (callbackOnPrefabClosing != null) PrefabStage.prefabStageClosing -= callbackOnPrefabClosing;
            };

            GameObjectUtils.PreviewGuid = PreviewGuid;
            GameObjectUtils.PreviewScene = PreviewScene;
            GameObjectUtils.PreviewPath = PreviewPath;
            GameObjectUtils.IsPreviewAutoSaveEnabled = IsPrefabAutoSaveEnabled;
            GameObjectUtils.SavePrefabPreview = SavePrefabPreview;
        }

        private static void OnPrefabOpened(PrefabStage stage, OpenedPrefabPreviewTracker tracker)
        {
            var preview = new OpenedPrefabPreviewTracker.Stage(stage.scene, stage.prefabAssetPath);
            tracker.OpenedPrefabPreview.Add(preview);
        }

        private static void OnPrefabClosing(PrefabStage stage, OpenedPrefabPreviewTracker tracker)
        {
            var preview = new OpenedPrefabPreviewTracker.Stage(stage.scene, stage.prefabAssetPath);
            tracker.OpenedPrefabPreview.Remove(preview);
        }

        private static GUID? PreviewGuid(GameObject g)
        {
            var prefabStage = PrefabStageUtility.GetPrefabStage(g);
            if (prefabStage is null) return null;
            return new GUID(AssetDatabase.AssetPathToGUID(prefabStage.prefabAssetPath));
        }

        private static string PreviewPath()
        {
            return PrefabStageUtility.GetCurrentPrefabStage()?.prefabAssetPath;
        }

        private static Scene? PreviewScene()
        {
            return PrefabStageUtility.GetCurrentPrefabStage()?.scene;
        }

        private static bool IsPrefabAutoSaveEnabled()
        {
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage is null || stage.prefabAssetPath == "") return true;
            var autoSave = typeof(PrefabStage)
                .GetProperty("autoSave", BindingFlags.Instance | BindingFlags.NonPublic)?
                .GetValue(stage);
            return autoSave is null || (bool)autoSave;
        }
        
        private static bool SavePrefabPreview()
        {
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage is null || stage.prefabAssetPath == "") return false;
            var save = typeof(PrefabStage).GetMethod("SavePrefabWithVersionControlDialogAndRenameDialog", 
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (save is null) return false;
            return (bool)save.Invoke(stage, null);
        }
    }
}