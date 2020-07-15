using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProvider : MonoBehaviour
{
    public TextAsset packsConfig;
    private IDictionary<string, TilePack> packs;

    private void Awake()
    {
        // Is this necessary? How many times should I expect Awake to be called?
        if (packs == null || packs.Count == 0)
        {
            LoadPacks();
        }
    }

    private void LoadPacks()
    {
        packs = new Dictionary<string, TilePack>();
        PackData[] packDataArray = JsonHelper.FromJson<PackData>(packsConfig.text);
        foreach (PackData packData in packDataArray)
        {
            packs.Add(
                packData.folderName,
                new TilePack(packData.folderName));
        }
    }

    public Tile GetBoardTile(string pack, string room, string name)
    {
        if (packs.ContainsKey(pack))
        {
            return packs[pack].GetBoardTile(room, name);
        }
        return null;
    }

    public Tile GetPlayerTile(string pack, string name)
    {
        if (packs.ContainsKey(pack))
        {
            return packs[pack].GetPlayerTile(name);
        }
        return null;
    }

    public Tile GetOverlayTile(string pack, string name)
    {
        if (packs.ContainsKey(pack))
        {
            return packs[pack].GetOverlayTile(name);
        }
        return null;
    }

    [Serializable]
    private class PackData
    {
        public string id;
        public string folderName;
    }
}
