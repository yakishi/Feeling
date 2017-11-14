using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲーム共通用のクラス
/// ロード時に値が入る予定
/// </summary>
public class GV
{
    #region Classes

    /// <summary>
    /// プレイヤーの情報
    /// 変動する情報を保存用
    /// </summary>
    [Serializable]
    public class PlayerParam
    {
        public int ID;
        public int Lv;
        public int HP;
        public int MP;
        public int Atk;
        public int Def;
        public int Int;
        public int Mgr;
        public int Agl;
        public int Luc;
        public int StatusPoint;
        public int[] currentEquipment = new int[6];
    }

    /// <summary>
    /// Save するゲームデータ
    /// </summary>
    [Serializable]
    public class GameData
    {
        #region Other
        public int playTime;
        #endregion

        #region Player
        public List<PlayerParam> Players;
        public int PartyCount;
        #endregion

        #region Items
        // 所持している装備のIDの配列
        public List<int> Equipments;
        #endregion
    }

    // ゲームデータを読み込むときは先にセーブデータのロードまたはnewGame で関数を初期化して使用
    static GameData gameData;
    static public GameData GData 
    {
        get {
            return gameData;
        }
    }

    [Serializable]
    public class SystemData
    {
        public int saveCount;
    }
    static SystemData systemData;
    static public SystemData SData
    {
        get {
            if (systemData == null)
            {
                newGame();
            }
            return systemData;
        }
    }

    #endregion

    #region Properties
    #endregion

    #region Methods

    /// <summary>
    /// GameData をセーブする
    /// </summary>
    /// <param name="slot">セーブするスロット(0はシステムデータ保存用)</param>
    static public void save(int slot = 1)
    {
        gameData = new GameData();
        gameData.playTime = 20;

        gameData.Players = new List<PlayerParam>();
        var player = new PlayerParam();
        gameData.Players.Add(player);


        SaveData.setClass("GameData", GData);
        SaveData.save();
    }

    static public void load(int slot = 1)
    {
        SaveData.setSlot(slot);
        SaveData.load();
        gameData = SaveData.getClass<GameData>("GameData", null);

        Debug.Log(gameData.playTime);
    }
    #endregion

    /// <summary>
    /// ゲームを新しく始めるときに呼び出される関数
    /// </summary>
    static public void newGame()
    {
        // SystemDataの読み込み
        SaveData.setSlot(0);
        SaveData.load();
        systemData = SaveData.getClass<SystemData>("SystemData", null);
        
        gameData = new GameData();

        // 装備の仮データ読み込み
        {
            gameData.Equipments = new List<int>();
            // 仮で全装備を ID + 1 所持している状態にする
            var equipments = EquipmentManager.getEquipmentList();
            foreach (var equipment in equipments) {
                for (int i = 0; i <= equipment.ID; ++i) {
                    gameData.Equipments.Add(equipment.ID);
                }
            }
        }

        // プレイヤーの仮作成
        {
            gameData.Players = new List<PlayerParam>();
            for( int i = 0; i < 6; i++ ) {
				// プレイヤーの初期値を作るときはここでファイルから読み込めばイケル
				// テキトウな値で初期化
				PlayerParam newPlayer = new PlayerParam( );
                newPlayer.ID = i + 1/*PlayerManagerCSV.GetPlayers[ 0 ]._Player1*/;
				newPlayer.Lv = ( i + 1 ) * 10;
                newPlayer.HP = ( i + 1 ) * 2/*PlayerManagerCSV.GetPlayers[ i ].HP*/;
                newPlayer.MP = ( i + 1 ) * 3/*PlayerManagerCSV.GetPlayers[ i ].MP*/;
                newPlayer.Atk = ( i + 1 ) * 4/*PlayerManagerCSV.GetPlayers[ i ].ATK*/;
                newPlayer.Def = ( i + 1 ) * 5/*PlayerManagerCSV.GetPlayers[ i ].DEF*/;
                newPlayer.Int = ( i + 1 ) * 6/*PlayerManagerCSV.GetPlayers[ i ].INT*/;
                newPlayer.Mgr = ( i + 1 ) * 7/*PlayerManagerCSV.GetPlayers[ i ].MDEF*/;
                newPlayer.Agl = ( i + 1 ) * 8/*PlayerManagerCSV.GetPlayers[ i ].SPD*/;
                newPlayer.Luc = ( i + 1 ) * 9/*PlayerManagerCSV.GetPlayers[ i ].LUCKY*/;
				newPlayer.StatusPoint = ( i + 1 ) * 11;
                
                gameData.Players.Add(newPlayer);

            }

        }


    }


}
