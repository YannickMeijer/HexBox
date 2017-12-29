using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePlayingFieldGenerator : IPlayingFieldGenerator
{
    public void GenerateField(PlayingFieldController playingField, GameOptions gameOptions)
    {
        int size = gameOptions.WorldSize / 2;

        for (int x = -size; x <= size; x++)
            for (int z = -size; z <= size; z++)
            {
                // Skip every even tile on the first z, so the playing field is symmetrical for opposing players.
                if (z == -size && x % 2 == 0)
                    continue;

                playingField.CreateTile(new Point(x, z));
            }
    }
}
