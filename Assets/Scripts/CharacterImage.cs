using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterImage : ScriptableObject {

    [SerializeField]
    public List<Sprite> charaImgList = new List<Sprite>();


    [MenuItem ("Assets/Create/CharacterImages")]

    static void CreateCharacterImageAssetInstance()
    {
        CharacterImage characterImage = CreateInstance<CharacterImage>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/ScriptableObject/CharacterImages.asset");
        AssetDatabase.CreateAsset(characterImage, path);
        AssetDatabase.Refresh();
    }
}
