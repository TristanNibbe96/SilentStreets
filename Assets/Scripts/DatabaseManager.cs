using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager shared;

    private string databasePath = "Assets/Packages/DataFiles/UserData/PlayerSave.xml";
    private Dictionary<string, bool> flagDataBase = new Dictionary<string, bool>();

    void Awake()
    {
        DontDestroyOnLoad(this);
        if(shared == null)
        {
            shared = this;
        }
        else if (shared != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void ResetFlagDatabase()
    {
        flagDataBase = new Dictionary<string, bool>();
        XmlDocument databaseDoc = new XmlDocument();
        XmlNode rootNode = databaseDoc.CreateElement("flags");
        databaseDoc.AppendChild(rootNode);
        databaseDoc.Save(databasePath);
    }

    public void LoadFlagDatabaseFromFile()
    {
        XmlDocument databaseDoc = new XmlDocument();
        databaseDoc.Load(databasePath);
        foreach( XmlNode xmlNode in databaseDoc.DocumentElement)
        {
            shared.SetFlagState(xmlNode.InnerText,true);
        }
    }

    public void SaveFlagDatabaseFromFile()
    {
        XmlDocument databaseWriter = new XmlDocument();

        IDictionaryEnumerator iterator = shared.flagDataBase.GetEnumerator();
        XmlNode rootNode = databaseWriter.CreateElement("flags");
        databaseWriter.AppendChild(rootNode);

        while (iterator.MoveNext())
        {
            XmlNode newFlag = databaseWriter.CreateElement("flag");
            newFlag.InnerText = iterator.Key.ToString();
            rootNode.AppendChild(newFlag);
        }
        databaseWriter.Save(databasePath);
    }

    public void GetDatabase()
    {
        IDictionaryEnumerator iterator = shared.flagDataBase.GetEnumerator();
    }
    public void SetFlagState(string flagToSet,bool state)
    {
        if (flagDataBase.ContainsKey(flagToSet))
        {
            flagDataBase.Remove(flagToSet);
        }
        flagDataBase.Add(flagToSet, state);
    }

    public bool GetFlagState(string flagToGet)
    {
        bool flagState;
        flagDataBase.TryGetValue(flagToGet, out flagState);
        return flagState;
    }
}
