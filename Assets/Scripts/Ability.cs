using System.Collections;
using System.Collections.Generic;

public class Ability
{
    public float range = 1.5f;
    public float moveSpeed = 2.0f;
    public int hp = 0;
    public int power = 0;

    public Ability(float dist, float moveSpeed, int hp, int power)
    {
        this.range = dist;
        this.moveSpeed = moveSpeed;

        this.hp = hp;
        this.power = power;
    }
}
