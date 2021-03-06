using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    /// <summary>
    /// Shuffles the contents of a list
    /// </summary>
    /// <remarks>
    /// This method was taken from https://stackoverflow.com/a/1262619/13874150 under the CC BY-SA 3.0 License and is unchanged
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">the <see cref="IList"/> to be shuffled</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
