using System;
using System.Collections.Generic;
using UnityEngine;


public class Settings : MonoBehaviour
{
    public TextAsset settingsFile;
    public string Level { get; private set; }
    public IList<Player> Players { get; private set; }
    public IList<Pack> Packs { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        SettingsJson settingsJson = JsonUtility.FromJson<SettingsJson>(settingsFile.text);
        Level = settingsJson.Level;
        Players = new List<Player>();
        Packs = new List<Pack>();
        foreach (PlayerJson playerJson in settingsJson.Players)
        {
            Players.Add(new Player(playerJson.id, playerJson.characterId));
        }
        foreach (PackJson packJson in settingsJson.Packs)
        {
            Packs.Add(new Pack(packJson.name, packJson.folder));
        }
    }

    public class Player
    {
        public int Id { get; private set; }
        public int CharacterId { get; private set; }

        public Player(int id, int characterId)
        {
            Id = id;
            CharacterId = characterId;
        }
    }

    public class Pack
    {
        public string Name { get; private set; }
        public string Folder { get; private set; }

        public Pack(string name, string folder)
        {
            Name = name;
            Folder = folder;
        }
    }

    [Serializable]
    private class SettingsJson
    {
        public string Level;
        public List<PlayerJson> Players;
        public List<PackJson> Packs;
    }

    [Serializable]
    private class PlayerJson
    {
        public int id;
        public int characterId;
    }

    [Serializable]
    private class PackJson
    {
        public string name;
        public string folder;
    }
}
