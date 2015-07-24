using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public GameObject blackTile;
	public GameObject whitetile;

	public int tileSize = 1;

	Level level = new Level();

	// Wasn't sure to support multple tile types or not but whatever
	public enum TileTypes{
		black, // blak tiles are used on outskirts 
		white,
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Assumes tile coordinates
	// Retuurns true if we did place said tile
	public bool placeTile(Vector2 pos, TileTypes type, bool overwrite=false){
		if(overwrite){
			// Remove the tile before we begni if there is already one at that position
			Tile oldTile = level.removeTile(pos);
			if (oldTile != null){
				Destroy(oldTile.go);
			}
		}else{
			Tile t = level.getTileAt(pos);
			if(t != null){
				return false;
			}
		}

		Vector3 realPos =  new Vector3(pos.x * tileSize, pos.y * tileSize, 0);

		GameObject prefab;
		switch(type){
			case TileTypes.black:
				prefab = blackTile;
				break;
			case TileTypes.white:
				prefab = whitetile;
				break;
			default:
				prefab = whitetile;
				break;
		}

		Tile tile = new Tile();
		tile.x = pos.x;
		tile.y = pos.y;
		tile.type = type;

		GameObject tObj = Instantiate(prefab, realPos, Quaternion.identity) as GameObject;
		tile.go = tObj;

		level.setTile(tile);

		return true;
	}

	public void removeTile(Vector2 pos){
		Tile tile = level.removeTile(pos);
		if(tile != null){
			Destroy(tile.go);
		}
	}

	public void clear(){
		foreach(Tile t in level.tiles){
			if(t.go != null){
				Destroy(t.go);
			}
		}
		level.tiles = new List<Tile>();
	}

	// Loads the level from file fname
	public void load(string fName){
		// use Systtem.IO.ReadAllLines
		// and 	JsonConvert.DeserializeObject Method (String)
		string content = File.ReadAllText(fName + ".sqbrwl");
		if(content == ""){
			return;
		}

		// Clear
		clear();

		Level lvl = JsonConvert.DeserializeObject<Level>(content);
		level = lvl;

		// spawn the shit
		foreach(Tile t in level.tiles){
			GameObject prefab;
			switch(t.type){
				case TileTypes.black:
					prefab = blackTile;
					break;
				case TileTypes.white:
					prefab = whitetile;
					break;
				default:
					prefab = whitetile;
					break;
			}

			Vector3 realPos =  new Vector3(t.x * tileSize, t.y * tileSize, 0);
			GameObject tObj = Instantiate(prefab, realPos, Quaternion.identity) as GameObject;
			t.go = tObj;
		}
	}

	// Saves the level to file fname
	public void save(string fName){
		string serialized = JsonConvert.SerializeObject(level, Formatting.Indented);
		File.WriteAllText(fName+".sqbrwl", serialized);
	}

	// Call after load to instantiate
	public void Instantiate(){

	}
}

// The actual level, all functions deal in tile coordinates
// 1,1 is one tile, 2,1 is the tile next to that etc...
public class Level {
	public List<Tile> tiles = new List<Tile>();


	public Tile getTileAt(Vector2 pos){
		foreach(Tile t in tiles){
			if(t.x == pos.x && t.y == pos.y){
				return t;
			}
		}
		return null;
	}

	// Sets a tile, overwrites if there is allready a tile at that position(Wont remove it from the scene!!)
	public void setTile(Tile tile){
		// Check if it exists first and remove if so
		foreach (Tile t in tiles){
			if(t.x == tile.x && t.y == tile.y){
				tiles.Remove(t);
				break;
			}
		}
		tiles.Add(tile);
	}

	// Removes a tile from the level
	// (Dosen't acutally destroy it in the scene, but returns it)
	public Tile removeTile(Vector2 pos){
		foreach (Tile t in tiles){
			if(t.x == pos.x && t.y == pos.y){
				tiles.Remove(t);
				return t;
			}
		}

		return null;
	}
}

// A single tile
public class Tile {
	public float x;
	public float y;
	public LevelManager.TileTypes type;

	[JsonIgnoreAttribute] 
	public GameObject go;
}