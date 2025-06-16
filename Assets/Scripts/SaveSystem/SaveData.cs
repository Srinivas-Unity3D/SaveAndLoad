using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerSaveData playerData;
    public List<EnemySaveData> enemyData;
    public int score;
    public int currentLevel;
    public float gameTime;
    public DateTime saveDateTime;

    public SaveData()
    {
        enemyData = new List<EnemySaveData>();
        saveDateTime = DateTime.Now;
    }
}

[Serializable]
public class PlayerSaveData
{
    public float health;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public int ammo;
    public float score;
}

[Serializable]
public class EnemySaveData
{
    public string enemyId;
    public string enemyType;
    public float health;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public bool isActive;
    public bool isDead;
}

[Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public struct SerializableQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
} 