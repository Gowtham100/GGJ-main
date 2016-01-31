using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 32;
    public int rows = 32;
    public Count wallCount = new Count(70, 70); // min 5 wells per level max 10
    public int npcMinimum = 5;
    public int npcMaximum = 10;
    public List<GameObject> shelfTiles;
    public GameObject shelfBackTile;
    public GameObject floorTile;
    public GameObject npcTile;
    public GameObject wallTile;
    public GameObject doorTile;
    private Transform boardHolder; // keeps hierarchy clean


    private List<Vector3> gridPositions = new List<Vector3>(); //keeps track of all spots in gameboard
    private List<Vector3> walledGridPositions = new List<Vector3>(); // keeps track of all of the spaces where walls have already been placed
    private List<Vector3> freePositions = new List<Vector3>();
    

    private Vector3 topRightCornerPos = new Vector3(32 - 1, 32 - 1);
    private Vector3 topLeftCornerPos = new Vector3(1, 32 - 1);
    private Vector3 bottomRightCornerPos = new Vector3(32 - 1, 0);
    private Vector3 bottomLeftCornerPos = new Vector3(0, 0);





    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y =  1; y < columns - 1; y++)
            {
                // this is creating a list of spaces where stuff (walls, people can be placed)
                gridPositions.Add(new Vector3(x, y, 0f));
                //loops dont go from 0 to rows and instead form 1 to columns
                // we want a clear loop around the outside of the screen
            }
        }
    }

    void BoardSetup()
    {
        // sets up outer wall and floor of the game
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x ++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // these go to cols + 1 because the edge is outside of the outer edge of the screen
                //prepares to instantiate a floortile
                GameObject toInstantiate = floorTile;
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    // if the tile is on the outer wall instantiate it as a wall tile instad of a floor tile
                    toInstantiate = wallTile;
                    if ((x == -1 && Math.Abs(y - (rows / 2)) < 1) || (x == columns && Math.Abs(y - (rows / 2)) < 1) ||
                        (y == -1 && Math.Abs(x - (columns / 2)) < 1) || (y == columns && Math.Abs(x - (columns / 2)) < 1))
                        // these are the spaces on the map where doors should be
                        toInstantiate = doorTile;
                    {
                        //
                    }
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); // removes the floortile in the space to replace it w/ something else
        return randomPosition;
    }

    private Boolean canCreateBlockAtPos(Vector3 pos)
    {
        Vector3 pos1 = pos;
        Vector3 pos2 = new Vector3(pos.x + 1, pos.y);
        Vector3 pos3 = new Vector3(pos.x - 1, pos.y);
        Vector3 pos4 = new Vector3(pos.x, pos.y + 1);
        Vector3 pos5 = new Vector3(pos.x, pos.y + 1);
        Vector3 pos6 = new Vector3(pos.x + 1, pos.y + 1);
        Vector3 pos7 = new Vector3(pos.x - 1, pos.y - 1);
        Vector3 pos8 = new Vector3(pos.x - 1, pos.y + 1);
        Vector3 pos9 = new Vector3(pos.x + 1, pos.y - 1);
        if (walledGridPositions.Contains(pos) || walledGridPositions.Contains(pos2) || walledGridPositions.Contains(pos3) || walledGridPositions.Contains(pos4) || walledGridPositions.Contains(pos5)
             || walledGridPositions.Contains(pos6) || walledGridPositions.Contains(pos7) || walledGridPositions.Contains(pos8) || walledGridPositions.Contains(pos9)
             || pos.x < 1 || pos.y < 1 || pos.x > columns - 2 || pos.y > rows - 2)
            return false;
        else
        {
            return true;
        }
    }

    void LayoutObjectAtRandom(List<GameObject> tileTypesToPlace, int minimumTiles, int maximumTiles)
    {
        // determines the amount of objects to spawn
        int objectCount = Random.Range(minimumTiles, maximumTiles + 1);
        int loopCount = 0; // this basically should stop the program from running out of memory, if this reaches like 200 while loop will stop
        for (int i = 0; i < objectCount; i++)
        {
            //TODO: the next thing that I should do is to make it so that it will only create a tile if pos1 pos2 pos3 and pos4 all 
            // have floor tiles on all sides beofre they are instantiated 



            // if the block is a square block it makes walls at (x,y)(x-1,y-1)(x-1,y)(y-1,x)
            // if the block is a t shaped block it makes walls at (x,y)(x,y-1)(x-1,y-1)(x+1,y-1)
            // if the block is I shaped it makes walls at (x,y)(x+1,y)(x+2,y)(x+3,y)
            // if the block is L shaped it makes walls at (x,y)(x+1,y)(x,y-1)(x,y-2)

            // side i (x,y)(x,y+1)(x,y+2)(x,y+3) /
            // upside down t (x,y)(x,y + 1)(x+1,y+1)(x-1,y+1) /
            // sideways T (x,y)(x+1,y)(x+1,y+1)(x+1,y-1) /
            // other sideways T (x,y) (x-1,y) (x-1,y+1) (x-1,y-1)/
            // flipped horizontally L (x,y) (x-1,y) (x,y-1)(x,y-2) /
            // flipped vertically L (x,y)(x+1,y)(x,y+1)(x,y+2) /
            // flipped vert and horiz L (x,y)(x-1,y)(x,y+1)(x,y+2) /


            int shapeType = Random.Range(0, 11); // creates a number 0,1,2,3... 10
            // if tile is 0 square block, if tile is 1 T block, if type is 2 I block, if type is 3 L block
            Vector3 pos1;
            Vector3 pos2;
            Vector3 pos3;
            Vector3 pos4;
            int pos1Type = 0; // type of a pos is 0 or 1, 0 means its normal and will display normally, 1 means it's a vertical block 
            // and it should only display the shelf back
            int pos2Type = 0;
            int pos3Type = 0;
            int pos4Type = 0;

            Vector3 randomPosition = RandomPosition();

            //while (!canCreateBlockAtPos(randomPosition))
            do
            {
                loopCount++;
                randomPosition = RandomPosition();

                if (shapeType == 0)
                {
                    // square DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos1Type = 1;
                    pos2 = new Vector3(randomPosition.x - 1, randomPosition.y - 1);
                    pos3 = new Vector3(randomPosition.x, randomPosition.y - 1);
                    pos4 = new Vector3(randomPosition.x - 1, randomPosition.y);
                    pos4Type = 1;
                }
                else if (shapeType == 1)
                {
                    // T DONE

                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos1Type = 1;
                    pos2 = new Vector3(randomPosition.x, randomPosition.y - 1);
                    pos3 = new Vector3(randomPosition.x - 1, randomPosition.y - 1);
                    pos4 = new Vector3(randomPosition.x + 1, randomPosition.y - 1);
                }
                else if (shapeType == 2)
                {
                    // I DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    
                    pos2 = new Vector3(randomPosition.x + 1, randomPosition.y);
                    pos3 = new Vector3(randomPosition.x + 2, randomPosition.y);
                    pos4 = new Vector3(randomPosition.x + 3, randomPosition.y);
                }
                else  if (shapeType == 3)// if shapetile == 3
                {
                    // L DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos1Type = 1;
                    pos2 = new Vector3(randomPosition.x + 1, randomPosition.y);
                    
                    pos3 = new Vector3(randomPosition.x, randomPosition.y - 1);
                    pos3Type = 1;
                    pos4 = new Vector3(randomPosition.x, randomPosition.y - 2);
                    
                } else if (shapeType == 4)
                {
                    //(x, y)(x, y+1)(x, y+2)(x, y+3) sideways I DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                   
                    pos2 = new Vector3(randomPosition.x, randomPosition.y + 1);
                    pos2Type = 1;
                    pos3 = new Vector3(randomPosition.x, randomPosition.y + 2);
                    pos3Type = 1;
                    pos4 = new Vector3(randomPosition.x, randomPosition.y + 3);
                    pos4Type = 1;
                } else if (shapeType == 5)
                {
                    //(x, y)(x, y +1)(x + 1,y + 1)(x - 1,y + 1) / DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x, randomPosition.y + 1);
                    pos2Type = 1;
                    pos3 = new Vector3(randomPosition.x + 1, randomPosition.y + 1);
                    pos4 = new Vector3(randomPosition.x - 1, randomPosition.y + 1);
                } else if (shapeType == 6)
                {
                    // sideways T (x,y)(x+1,y)(x+1,y+1)(x+1,y-1) / DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x + 1, randomPosition.y);
                    pos2Type = 1;
                    pos3 = new Vector3(randomPosition.x + 1,  randomPosition.y + 1);
                    pos3Type = 1;
                    pos4 = new Vector3(randomPosition.x + 1, randomPosition.y - 1);
                    

                } else if (shapeType == 7)
                {
                    // other sideways T (x,y) (x-1,y) (x-1,y+1) (x-1,y-1)/ DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x - 1, randomPosition.y);
                    pos2Type = 1;
                    pos3 = new Vector3(randomPosition.x - 1, randomPosition.y + 1);
                    pos3Type = 1;
                    pos4 = new Vector3(randomPosition.x - 1, randomPosition.y - 1);
                    
                } else if (shapeType == 8)
                {
                    // flipped horizontally L (x,y) (x-1,y) (x,y-1)(x,y-2) / DONE
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos1Type = 1;
                    pos2 = new Vector3(randomPosition.x - 1, randomPosition.y);
                    
                    pos3 = new Vector3(randomPosition.x, randomPosition.y - 1);
                    pos3Type = 1;
                    pos4 = new Vector3(randomPosition.x, randomPosition.y - 2);
                    
                } else if (shapeType == 9)
                {
                    // flipped vertically L (x,y)(x+1,y)(x,y+1)(x,y+2) /
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x + 1, randomPosition.y);
                    pos3 = new Vector3(randomPosition.x, randomPosition.y + 1);
                    pos3Type = 1;
                    pos4 = new Vector3(randomPosition.x, randomPosition.y + 2);
                    pos4Type = 1;
                } else // if shapeType == 10
                {
                    // flipped vert and horiz L (x,y)(x-1,y)(x,y+1)(x,y+2) /
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x - 1, randomPosition.y);
                    pos3 = new Vector3(randomPosition.x, randomPosition.y + 1);
                    pos3Type = 1;
                    pos4 = new Vector3(randomPosition.x, randomPosition.y + 2);
                    pos4Type = 1;
                }
                
            } while ((!canCreateBlockAtPos(randomPosition) || !canCreateBlockAtPos(pos1) || !canCreateBlockAtPos(pos2) || !canCreateBlockAtPos(pos3) || !canCreateBlockAtPos(pos4)) && loopCount < 200);

            int tileTypeToPlace = Random.Range(0, 5); // will generate 0,1,2,3,4;
            if (pos1Type == 0)
            {
                Instantiate(tileTypesToPlace[tileTypeToPlace], pos1, Quaternion.identity);
                walledGridPositions.Add(pos1);
            } else
            {
                Instantiate(shelfBackTile, pos1, Quaternion.identity);
                walledGridPositions.Add(pos1);
            }
            if (pos2Type == 0)
            {
                Instantiate(tileTypesToPlace[tileTypeToPlace], pos2, Quaternion.identity);
                walledGridPositions.Add(pos2);
            } else
            {
                Instantiate(shelfBackTile, pos2, Quaternion.identity);
                walledGridPositions.Add(pos2);
            }
            if (pos3Type == 0)
            {
                Instantiate(tileTypesToPlace[tileTypeToPlace], pos3, Quaternion.identity);
                walledGridPositions.Add(pos3);
            } else
            {
                Instantiate(shelfBackTile, pos3, Quaternion.identity);
                walledGridPositions.Add(pos3);
            }
            if (pos4Type == 0)
            {
                Instantiate(tileTypesToPlace[tileTypeToPlace], pos4, Quaternion.identity);
                walledGridPositions.Add(pos4);
            } else
            {
                Instantiate(shelfBackTile, pos4, Quaternion.identity);
                walledGridPositions.Add(pos4);
            }

            if (loopCount > 200)
            {
                break;
            }
        }
    }

    
    void addNPCs()
    {
        Vector3 baseNPCPos = new Vector3(1, 1, 0f);
        List<Vector3> posToRemoveFromFree = new List<Vector3>();
        Instantiate(npcTile, baseNPCPos, Quaternion.identity);
        freePositions = gridPositions; // initially set free positions to all grid positions
        // then for all free positions if walledPositions contains the position remove it from freepositions
        foreach (Vector3 pos in gridPositions)
        {
            if (walledGridPositions.Contains(pos))
            {
                if (freePositions.Contains(pos))
                {
                    posToRemoveFromFree.Add(pos);
                }
            }
        }

        foreach(Vector3 pos in posToRemoveFromFree)
        {
            freePositions.Remove(pos);
        }
        Vector3 npcPosition;
        int npcCount = Random.Range(npcMinimum, npcMaximum + 1);

        for (int i = 0; i < npcCount; i++)
        {
            do
            {
                //int randomIndex = Random.Range(0, gridPositions.Count);
                //Vector3 randomPosition = gridPositions[randomIndex];
                Vector3 randomPosition = new Vector3(Random.Range(0, 32), Random.Range( 0, 32));
                //freePositions.RemoveAt(randomIndex); // removes the floortile in the space to replace it w/ something else
                npcPosition = randomPosition;
            } while (!freePositions.Contains(npcPosition));

            Instantiate(npcTile, npcPosition, Quaternion.identity);
        }
        



    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        int tileTypeToPlace = Random.Range(0, 5); // will generate 0,1,2,3,4;
        LayoutObjectAtRandom(shelfTiles, wallCount.minimum, wallCount.maximum);
        addNPCs();

        // more stuff here
    }
	// Use this for initialization
	void Start () {
      
}

 
	
	// Update is called once per frame
	void Update () {
	
	}
}
