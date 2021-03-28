using System.Collections;
using System.Collections.Generic;

public class Iterator<T> : IEnumerator
{
    object IEnumerator.Current => throw new System.NotImplementedException();

    static int defaultCapacity = 10;
    int count = 0;
    int currentCapacity = defaultCapacity;

    T[] arrayOfElements = new T[defaultCapacity];
    int currentElement = 0;

    public void Add(T newElement)
    {
        if(count >= currentCapacity)
        {
            ExtendCapacity(currentCapacity*2);
        }
        arrayOfElements[count] = newElement;
        count++;
    }
    
    public void ExtendCapacity(int amount)
    {
        currentCapacity = amount;
        T[] newArray = new T[currentCapacity];
        Copy(arrayOfElements, newArray);
        arrayOfElements = newArray;
    }

    public void Copy(T[] source, T[] dest)
    {
        for(int i = 0; i < source.Length; i++)
        {
            dest[i] = source[i];
        }
    }

    public bool MoveNext()
    {
        currentElement++;
        if(currentElement == count)
        {
            currentElement = 0;
        }
        return true;
    }

    public void MovePrevious()
    {
        currentElement--;
        if(currentElement < 0)
        {
            currentElement = count - 1;
        }
    }

    public void Reset()
    {
        currentElement = 0;
        currentCapacity = defaultCapacity;
        arrayOfElements = new T[currentCapacity];
        count = 0;
    }
    
    public int GetCount()
    {
        return count;
    }

    public T Current()
    {
        return arrayOfElements[currentElement];
    }

}
