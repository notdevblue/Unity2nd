using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Enemy : MonoBehaviour, ISerializable
{
    public int hp = 10;
    public string name = "º’»Ø¡Í";

    private void Start()
    {
        //
        FindObjectOfType<SaveManager>().objToSaveList.Add(this);
    }

    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject returnVal = JObject.Parse(jsonString);

        return returnVal;
    }

    public void DeSerialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }

    public string GetJsonKey()
    {
        return this.gameObject.name;
    }
}
