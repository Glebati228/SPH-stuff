using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MoveSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation data, ref SpeedComponent speed) =>
        {
            data.Value.y += speed.speed * Time.deltaTime;
            if (data.Value.y > 5f)
                speed.speed = -math.abs(speed.speed);

            else if (data.Value.y < -5f)
                speed.speed = math.abs(speed.speed);
        });
    }
}
