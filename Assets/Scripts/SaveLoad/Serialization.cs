using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// List 用の SerializeField クラス
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
class Serialization<T>
{
    [SerializeField]
    List<T> target_;
    public List<T> Target {
        get { return target_; }
    }

    public Serialization(List<T> target)
    {
        target_ = target;
    }
}

/// <summary>
/// Dictionary 用の Serializa のクラス
/// </summary>
[Serializable]
class Serialization<KeyType, ValuseType> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<KeyType> keys;
    [SerializeField]
    List<ValuseType> values;

    Dictionary<KeyType, ValuseType> target_;
    public Dictionary<KeyType, ValuseType> Target {
        get { return target_; }
    }

    public Serialization(Dictionary<KeyType, ValuseType> target)
    {
        target_ = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<KeyType>(target_.Keys);
        values = new List<ValuseType>(target_.Values);
    }

    public void OnAfterDeserialize()
    {
        target_ = keys.Select((key, index) =>
        {
            var value = values[index];
            return new { key, value };
        })
        .ToDictionary(x => x.key, x => x.value);

        keys.Clear();
        values.Clear();
    }
}
