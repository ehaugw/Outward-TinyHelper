using System;
using System.Collections.Generic;
using System.Linq;

public static class TinyHelpers
{
    public static string OrConcat<T>(this ICollection<T> collection)
    {
        var list = collection.ToList();
        var result = "";

        for (int i = 0; i < list.Count; i++)
        {
            result += list[i].ToString();

            if (i == list.Count - 2)
            {
                result += " or ";
            }
            else if (i < list.Count - 2)
            {
                result += ", ";
            }
        }

        return result;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = list.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);

            T value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }
}