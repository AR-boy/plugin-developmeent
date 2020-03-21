using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class callTestLibSort : MonoBehaviour
{
    [DllImport("test_native")]
    private static extern void TestSort(int [] a, int length);
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        int[] a = new int[5] {1,5,4,2,3};
        TestSort(a, a.Length);
        Debug.Log("a contents: "+ a[0] + ","+  a[1] + ","+  a[2] + "," +  a[3] + "," +  a[4] + ".");
    }
}
