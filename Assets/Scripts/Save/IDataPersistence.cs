using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);

    void SaveData(GameData data);
}


public interface IPermanentDataPersistence
{
    void LoadData(PermanentGameData data);

    void SaveData(PermanentGameData data);
}