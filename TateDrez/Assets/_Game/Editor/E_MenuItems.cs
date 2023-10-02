using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class E_MenuItems 
{
    [MenuItem("_Scenes/Game")]
    
    private static void Game()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/_Game/Scenes/Game.unity");
    }
    
    [MenuItem("_Scenes/_Base")]
    private static void BaseScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/_Game/Scenes/Base.unity");
    }
    

}
