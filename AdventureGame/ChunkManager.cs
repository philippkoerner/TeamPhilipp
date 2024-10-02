using FastConsole.Engine.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace AdventureGame;
public class ChunkManager
{

    public int _chunkkey = 0;
    public Dictionary<int, Map> chunks = new Dictionary<int, Map>();
    List<int> keys = new List<int>();
    
    public void GenerateChunk(int chunkkey)
    {
        bool thekeydontexist = true;
        foreach (int element in keys)
        {
            if (element == chunkkey)
            {
                thekeydontexist = false;
            }
        }
        if (thekeydontexist)
        {
            chunks.Add(chunkkey, new Map());
        }
    }
}

