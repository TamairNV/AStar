using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private SortedList<float, T> sortedList = new SortedList<float, T>();

    public void Enqueue(float priority, T item)
    {
        sortedList.Add(priority, item);
    }

    public T Dequeue()
    {
        if (sortedList.Count == 0)
            throw new InvalidOperationException("The queue is empty.");

        var item = sortedList.Values[0];
        sortedList.RemoveAt(0);
        return item;
    }

    public int Count => sortedList.Count;
}