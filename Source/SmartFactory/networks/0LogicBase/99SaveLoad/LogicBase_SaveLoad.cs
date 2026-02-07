// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

partial class LogicBase
{
    void IJsonSaveLoadable.OnSave()
    {
        m_saveDataJson = UnityEngine.JsonUtility.ToJson(new PrivateSaveData(this));

        if (ModOptions.onDebug)
            ModDebug.Log("SAVE >> " + UniqueId + ": " + m_saveDataJson);

        OnSave();
    }

    void IJsonSaveLoadable.OnLoad()
    {
        OnLoad();

        if (ModOptions.onDebug)
            ModDebug.Log("LOAD >> " + UniqueId + ": " + m_saveDataJson);

        if (string.IsNullOrEmpty(m_saveDataJson))
            return;

        var saveData = UnityEngine.JsonUtility.FromJson<PrivateSaveData>(m_saveDataJson);
        saveData.LoadAll(this);
    }

    protected virtual void OnSave() { }
    protected virtual void OnLoad() { }
}
