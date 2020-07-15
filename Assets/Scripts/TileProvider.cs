using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProvider : MonoBehaviour
{
    public Settings settings;
    private IDictionary<string, TilePack> packs;

    private void Start()
    {
        // Is this necessary? How many times should I expect Start to be called?
        if (packs == null || packs.Count == 0)
        {
            LoadPacks();
        }
    }

    private void LoadPacks()
    {
        packs = new Dictionary<string, TilePack>();
        foreach (Settings.Pack packSetting in settings.Packs)
        {
            packs.Add(
                packSetting.Name,
                new TilePack(packSetting.Folder));
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

}
