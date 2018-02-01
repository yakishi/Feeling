using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EquipmentData
{
    public class GUID
    {
        public GUID()
        {

        }

        public static GUID Generate()
        {
            return new GUID();
        }
    }

    public readonly GUID id;
    public GUID ID { get { return id; } }
    public string name;
    public Status status;

    public int buy;
    public int sell;

    public EquipmentData(string inName = "", Status inStatus = new Status(), int inBuy = 0, int inSell = 0)
    {
        id     = GUID.Generate();
        name   = inName;
        status = inStatus;
        buy    = inBuy;
        sell   = inSell;
    }
}
