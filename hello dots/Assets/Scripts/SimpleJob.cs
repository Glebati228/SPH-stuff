using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public struct SimpleJob : IJobParallelFor
{
    public NativeArray<int> array;

    public void Execute(int index)
    {
        array[index] = index * index; 
    }
}
