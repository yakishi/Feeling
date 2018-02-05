using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class CharacterImage : ScriptableObject {

    [SerializeField]
    public List<Sprite> charaImgList = new List<Sprite>();

#if UNITY_EDITOR
    [MenuItem ("Assets/Create/CharacterImages")]
#endif
    static void CreateCharacterImageAssetInstance()
    {
        CharacterImage characterImage = CreateInstance<CharacterImage>();
#if UNITY_EDITOR
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/ScriptableObject/CharacterImages.asset");
        AssetDatabase.CreateAsset(characterImage, path);
        AssetDatabase.Refresh();
#endif
    }
}
