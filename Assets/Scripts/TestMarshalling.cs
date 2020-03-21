using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class TestMarshalling : MonoBehaviour
{
    [DllImport("Project2")]
    internal static extern int FillIntArray(int [] a,int length);

    private bool isFilled = false;
    private int[] array;
    // Start is called before the first frame update
    void Start()
    {
        array = new int[5];
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFilled)
        {
            FillIntArray(array, 5);
            Debug.Log("array contents: "+ array[0]+ array[1]+ array[2]+ array[3]+ array[4]);
            isFilled = true;
        }
    }
}
