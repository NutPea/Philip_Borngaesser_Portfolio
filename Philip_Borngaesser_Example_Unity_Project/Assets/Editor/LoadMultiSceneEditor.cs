using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
public class LoadMultiSceneEditor 
{

    [MenuItem("Assets/___LoadFullGameScenes___/Test/TestScene1")]
    private static void LoadTest1()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("TestScene")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/Test/TestScene2")]
    private static void LoadTest2()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("TestScene 2")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/NoGameScenes/MainMenu")]
    private static void LoadMenu()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("MainMenu")));
    }


    [MenuItem("Assets/___LoadFullGameScenes___/NoGameScenes/Intro")]
    private static void LoadBar()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("Intro")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/NoGameScenes/ThanksForPlay")]
    private static void LoadThanksForPlay()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("ThanksForPlaying")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/StartArea/BanStoneInside")]
    private static void LoadMarktPlatz()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("BanStone_Inside")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/StartArea/BanStoneOutside")]
    private static void LoadHUB()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("BanStone_Outside")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/StartArea/Pasture")]
    private static void LoadMaeuseRennbahn()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("Pasture")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/HauntedCity/HauntedCity")]
    private static void LoadSpukhaus()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("HauntedCity")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/StartArea/BrokenClifs")]
    private static void LoadCombatTest()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("BrokenClifs")));
    }


    [MenuItem("Assets/___LoadFullGameScenes___/StartArea/TraderHouse")]
    private static void LoadTraderHouse()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("TraderHouse")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/HauntedCity/Undergrounds")]
    private static void LoadUndergroundsTest()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("Undergrounds")));
    }


    [MenuItem("Assets/___LoadFullGameScenes___/Wooods/BeforeWoods")]
    private static void LoadBeforeWoods()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("BeforeWoods")));
    }


    [MenuItem("Assets/___LoadFullGameScenes___/Wooods/Woods")]
    private static void LoadWoods()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("Woods")));
    }


    [MenuItem("Assets/___LoadFullGameScenes___/Wooods/WoodsUnderground")]
    private static void LoadWoodsÚnderground()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("WoodsUnderground")));
    }


    [MenuItem("Assets/___LoadFullGameScenes___/Wooods/SpiderMamaForecourt")]
    private static void LoadSpiderMamaFore()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("SpiderMamaForecourt")));
    }

    [MenuItem("Assets/___LoadFullGameScenes___/Wooods/SpiderMamaHouse")]
    private static void LoadSpiderMamaHouse()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(GetScenePath(SceneIndexFromName("SpiderMamaHouse")));
    }



    private static string GetScenePath(int buildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        return path;
    }

    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    private static int SceneIndexFromName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            string testedScreen = NameFromIndex(i);
            //print("sceneIndexFromName: i: " + i + " sceneName = " + testedScreen);
            if (testedScreen == sceneName)
                return i;
        }
        return -1;
    }


#endif
}
