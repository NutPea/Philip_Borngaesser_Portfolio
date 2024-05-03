using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Completly Unoptymised change switchcase to index!!!
/// </summary>
public static class SceneNames 
{
    public static Name currentSceneName; 

    public enum Name
    {
        BANSTONE_INSIDE = 1,
        BANSTONE_OUTSIDE = 2,
        BROKEN_CLIFS = 3,
        PASTURE = 4,
        HAUNTEDCITY = 5,
        UNDERGROUNDS = 6,
        BEFORE_WOODS = 7,
        WOODS = 8,
        WOODS_UNDERGROUND = 9,
        SPIDER_MAMA_FORECLIFS = 10,
        SPIDER_MAMA_HOUSE = 11,
        TRADER_HOUSE = 12,
        THANKS_FOR_PLAYING = -1,
        MAINMENU = 0,
        INTRO = 999,
        TESTSCENE = 1000,
        TESTSCENE2 = 1001
    }

    public static string GetSceneNameDescriptionBySceneName(string name)
    {
        Name choosenName = Name.BROKEN_CLIFS;
        string[] enumNames = System.Enum.GetNames(typeof(Name));
        for (int i = 0; i < enumNames.Length; i++)
        {
            if (enumNames[i].ToLower().Contains(name.ToLower())) 
            {
                choosenName = (Name)i;
                break;
            }
        }

        string choosenDiscription = "";

        switch (choosenName)
        {
            case Name.MAINMENU: choosenDiscription = "MainMenu"; break;
            case Name.BANSTONE_INSIDE: choosenDiscription = "BanStone Inside"; break;
            case Name.BANSTONE_OUTSIDE: choosenDiscription = "BanStone Outside"; break;
            case Name.BROKEN_CLIFS: choosenDiscription = "Broken Cliffs"; break;
            case Name.PASTURE: choosenDiscription = "Pasture"; break;
            case Name.HAUNTEDCITY: choosenDiscription = "Haunted City"; break;
            case Name.BEFORE_WOODS: choosenDiscription = "BeforeWoods"; break;
            case Name.WOODS: choosenDiscription = "Woods"; break;
            case Name.WOODS_UNDERGROUND: choosenDiscription = "WoodsUnderground"; break;
            case Name.SPIDER_MAMA_FORECLIFS: choosenDiscription = "SpiderMamaForecourt"; break;
            case Name.SPIDER_MAMA_HOUSE: choosenDiscription = "SpiderMamaHouse"; break;
            case Name.TRADER_HOUSE: choosenDiscription = "TraderHouse"; break;
            case Name.THANKS_FOR_PLAYING: choosenDiscription = "ThanksForPlaying"; break;
            case Name.UNDERGROUNDS: choosenDiscription = "Undergrounds"; break;
            case Name.INTRO: choosenDiscription = "Intro"; break;
            case Name.TESTSCENE: choosenDiscription = "Intro"; break;
              
        }
        return choosenDiscription;
    }

    public static int GetSceneName(Name name)
    {
        string choosenName = "";

        switch (name)
        {
            case Name.MAINMENU: choosenName = "MainMenu"; break;
            case Name.BANSTONE_INSIDE: choosenName = "BanStone_Inside"; break;
            case Name.BANSTONE_OUTSIDE: choosenName ="BanStone_Outside"; break;
            case Name.BROKEN_CLIFS:choosenName = "BrokenClifs"; break;
            case Name.PASTURE: choosenName = "Pasture"; break;
            case Name.HAUNTEDCITY: choosenName = "HauntedCity"; break;
            case Name.UNDERGROUNDS: choosenName = "Undergrounds"; break;
            case Name.BEFORE_WOODS: choosenName = "BeforeWoods"; break;
            case Name.WOODS: choosenName = "Woods"; break;
            case Name.WOODS_UNDERGROUND: choosenName = "WoodsUnderground"; break;
            case Name.SPIDER_MAMA_FORECLIFS: choosenName = "SpiderMamaForecourt"; break;
            case Name.SPIDER_MAMA_HOUSE: choosenName = "SpiderMamaHouse"; break;
            case Name.TRADER_HOUSE: choosenName = "TraderHouse"; break;
            case Name.THANKS_FOR_PLAYING: choosenName = "ThanksForPlaying"; break;
            case Name.INTRO: choosenName = "Intro"; break;
            case Name.TESTSCENE: choosenName = "TestScene"; break;
            case Name.TESTSCENE2: choosenName = "TestScene 2"; break;
        }

        return SceneIndexFromName(choosenName);
    }

    private static int SceneIndexFromName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string testedPath = SceneUtility.GetScenePathByBuildIndex(i);
            string testedScene = NameFromIndex(testedPath);
            if (sceneName == testedScene)
                return i;
        }
        Debug.Log("Scene not found!" + "---" + sceneName);
        return -1;
    }

    private static string NameFromIndex(string path)
    {
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
}
