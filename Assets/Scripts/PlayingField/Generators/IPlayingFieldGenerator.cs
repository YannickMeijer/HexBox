using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayingFieldGenerator
{
    void GenerateField(PlayingFieldController playingField, GameOptions gameOptions);
}
