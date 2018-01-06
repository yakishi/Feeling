using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/*
 * このクラスはシングルトンで作成されています。
 * 
 * セーブスロットについてですが、3つ必要という要件でしたが4つ分作成しています。
 * slot の 0 にはセーブデータの数など全セーブデータで共通するものを保存するようです
 * 
 * 使い方は 基本的に PlayerPrefs と変わりません
 * ですがセーブとロードはそれぞれ一括で行うため関数を呼び出さないとセーブ/ロードを行いません
 * 
 * Saveファイルのディレクトリは Directory.GetCurrentDirectory()/Save ないに保存されます
 * Save データは 現在 JSON のテキストデータは暗号化したバイナリデータで保存しています
 * 
 * 暗号化についてですが、ソースコードに暗号化用のパスが埋め込まれているため、
 * Github でソースコードを見れば簡単に復号化できるため、別の方法を使うかもしれません
 * 
 * Save はプリミティブ型および自作クラスを扱うことができます
 * 自作クラスを使用する場合は宣言時に [Serializable]を指定する必要があります
 * またセーブされるデータは public または [SerializeField] の設定が必要がです
 *
 * 使うかはわかりませんが現在GVというGlovalVariables用のクラスを作成して、このクラスを使用いるので参考にしてください
 * 
 */

/// <summary>
/// Save 用のクラス
/// </summary>
public class SaveData
{
    const int MaxData = 4;
    public const int SaveSlotCount = MaxData - 1;
    static SaveBase[] saveBase;
    static SaveBase[] SaveObj
    {
        get
        {
            if (saveBase == null)
            {
                saveBase = new SaveBase[MaxData];
                for (int i = 0; i < MaxData; ++i)
                {
                    string path = Directory.GetCurrentDirectory() + "/Save";
                    string fileName = "save" + i.ToString() + ".sav";
                    saveBase[i] = new SaveBase(path, fileName);
                }
            }
            return saveBase;
        }
    }
    static int saveSlot = 1;

    #region Setter
    /// <summary>
    /// int 型を書き込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">書き込む値</param>
    static public void setInt(string key, int value)
    {
        SaveObj[saveSlot].setInt(key, value);
    }
    /// <summary>
    /// float 型を書き込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">書き込む値</param>
    static public void setFloat(string key, float value)
    {
        SaveObj[saveSlot].setFloat(key, value);
    }
    /// <summary>
    /// bool 型を書き込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">書き込む値</param>
    static public void setBool(string key, bool value)
    {
        SaveObj[saveSlot].setBool(key, value);
    }
    /// <summary>
    /// string 型を書き込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">書き込む値</param>
    static public void setString(string key, string value)
    {
        SaveObj[saveSlot].setString(key, value);
    }
    /// <summary>
    /// class 型を書き込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="obj">書き込む値</param>
    static public void setClass<T>(string key, T obj) where T : class, new()
    {
        SaveObj[saveSlot].setClass<T>(key, obj);
    }
    /// <summary>
    /// list 型を書き込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="list">書き込む値</param>
    static public void setList<T>(string key, List<T> list)
    {
        SaveObj[saveSlot].setList<T>(key, list);
    }
    #endregion

    #region Getter
    /// <summary>
    /// int 型を読み込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="defaultValue">keyが見つからなかったときの値</param>
    /// <returns>見つかった値を返します</returns>
    static public int getInt(string key, int defaultValue)
    {
        return SaveObj[saveSlot].getInt(key, defaultValue);
    }
    /// <summary>
    /// float 型を読み込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="defaultValue">keyが見つからなかったときの値</param>
    /// <returns>見つかった値を返します</returns>
    static public float getFloat(string key, float defaultValue)
    {
        return SaveObj[saveSlot].getFloat(key, defaultValue);
    }
    /// <summary>
    /// bool 型を読み込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="defaultValue">keyが見つからなかったときの値</param>
    /// <returns>見つかった値を返します</returns>
    static public bool getBool(string key, bool defaultValue)
    {
        return SaveObj[saveSlot].getBool(key, defaultValue);
    }
    /// <summary>
    /// string 型を読み込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="defaultValue">keyが見つからなかったときの値</param>
    /// <returns>見つかった値を返します</returns>
    static public string getString(string key, string defaultValue = "")
    {
        return SaveObj[saveSlot].getString(key, defaultValue);
    }
    /// <summary>
    /// class 型を読み込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="defaultValue">keyが見つからなかったときの値</param>
    /// <returns>見つかった値を返します</returns>
    static public T getClass<T>(string key, T defaultValue = null) where T : class, new()
    {
        return SaveObj[saveSlot].getClass(key, defaultValue);
    }
    /// <summary>
    /// list 型を読み込みます
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="defaultValue">keyが見つからなかったときの値</param>
    /// <returns>見つかった値を返します</returns>
    static public List<T> getList<T>(string key, List<T> defaultValue = null)
    {
        return SaveObj[saveSlot].getList(key, defaultValue);
    }
    #endregion

    /// <summary>
    /// save するスロットを設定します
    /// </summary>
    /// <param name="slot"></param>
    static public void setSlot(int slot)
    {
        saveSlot = slot;
    }

    /// <summary>
    /// セーブデータを削除します
    /// </summary>
    /// <param name="slot"></param>
    static public void remove(int slot)
    {
        SaveObj[saveSlot].remove(slot);
    }

	/// <summary>
	/// 指定した key を削除します
	/// </summary>
	/// <param name="key"></param>
	static public void remove(string key)
    {
        SaveObj[saveSlot].remove(key);
    }

    /// <summary>
    /// key が保存されているか返します
    /// </summary>
    /// <param name="key">検索するkey</param>
    /// <returns>true 持っている場合, false key が保存されていない</returns>
    static public bool containsKey(string key)
    {
        return SaveObj[saveSlot].containsKey(key);
    }

    /// <summary>
    /// 保存されいる key の一覧を返します
    /// </summary>
    /// <returns>key の一覧</returns>
    static public List<string> getKeys()
    {
        return SaveObj[saveSlot].getKeys();
    }

    /// <summary>
    /// ファイル書き込み
    /// </summary>
    public static void save()
    {
        SaveObj[saveSlot].save();
    }

    /// <summary>
    /// ファイル読み込み
    /// </summary>
    /// <returns>セーブデータがなければ False</returns>
    public static bool load()
    {
        return SaveObj[saveSlot].load();
    }

    /// <summary>
    /// save データを実際に扱うクラス
    /// </summary>
    class SaveBase
    {
        string savePath_;
        string fileName_;

        Dictionary<string, string> saveData;
        Rijndael rijndael;

        public SaveBase(string savePath, string fileName)
        {
            savePath_ = savePath + "/";
            fileName_ = fileName;

            saveData = new Dictionary<string, string>();
            rijndael = new Rijndael();
        }

        #region Setter
        public void setInt(string key, int value)
        {
            saveData[key] = value.ToString();
        }
        public void setFloat(string key, float value)
        {
            saveData[key] = value.ToString();
        }
        public void setBool(string key, bool value)
        {
            saveData[key] = value.ToString();
        }
        public void setString(string key, string value)
        {
            saveData[key] = value;
        }
        public void setClass<T>(string key, T obj) where T : class, new()
        {
            var json = JsonUtility.ToJson(obj);
            saveData[key] = json;
        }
        public void setList<T>(string key, List<T> list)
        {
            var json = JsonUtility.ToJson(new Serialization<T>(list));
            saveData[key] = json;
        }
        #endregion

        #region Getter
        public int getInt(string key, int defaultValue)
        {
            if (!saveData.ContainsKey(key)) {
                return defaultValue;
            }

            return int.Parse(saveData[key]);
        }
        public float getFloat(string key, float defaultValue)
        {
            if (!saveData.ContainsKey(key)) {
                return defaultValue;
            }

            return float.Parse(saveData[key]);
        }
        public bool getBool(string key, bool defaultValue)
        {
            if (!saveData.ContainsKey(key)) {
                return defaultValue;
            }

            return bool.Parse(saveData[key]);
        }
        public string getString(string key, string defaultValue = "")
        {
            if (!saveData.ContainsKey(key)) {
                return defaultValue;
            }

            return saveData[key];
        }
        public T getClass<T>(string key, T defaultValue) where T : class, new()
        {
            if (!saveData.ContainsKey(key)) {
                return defaultValue;
            }

            var json = saveData[key];
            var obj = JsonUtility.FromJson<T>(json);

            return obj;
        }
        public List<T> getList<T>(string key, List<T> defaultValue)
        {
            if (!saveData.ContainsKey(key)) {
                return defaultValue;
            }

            var json = saveData[key];
            var list = JsonUtility.FromJson<Serialization<T>>(json).Target;
            return list;
        }
        #endregion

        public void save()
        {
            if (!File.Exists(savePath_)) {
                Directory.CreateDirectory(@savePath_);
            }
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(savePath_ + fileName_));
            var str = JsonUtility.ToJson(new Serialization<string, string>(saveData));
            var data = rijndael.encryption(str);
            bw.Write(data.Length);
            bw.Write(data);
            bw.Close();
        }

        public bool load()
        {
            if (!File.Exists(savePath_ + fileName_)) {
                return false;
            }

            var file = File.OpenRead(savePath_ + fileName_);
            BinaryReader br = new BinaryReader(file);
            var size = br.ReadInt32();
            var data = br.ReadBytes(size);
            br.Close();
            var str = rijndael.decryption(data);
            var temp = JsonUtility.FromJson<Serialization<string, string>>(str);
            saveData = temp.Target;
            return true;
        }

		public void remove( ) {
			//// ファイルを削除する
			//System.IO.File.Delete( savePath_ + fileName_ );
		}

        // TODO: 今後実装
        public void remove(int slot)
        {
			GV.Instance.SData.usedSave[ slot - 1 ] = false;

			SaveData.setSlot( 0 );
			SaveData.setClass( "SystemData", GV.Instance.SData );
			SaveData.save( );

			// ファイルを削除する
			System.IO.File.Delete( savePath_ + fileName_ );
		}

        public void remove(string key)
        {

        }

        public bool containsKey(string key)
        {
            return saveData.ContainsKey(key);
        }

        public List<string> getKeys()
        {
            return saveData.Keys.ToList<string>();
        }
    }
}
