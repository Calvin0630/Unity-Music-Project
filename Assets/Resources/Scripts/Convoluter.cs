using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Convoluter {
    public static float e = 2.71828f;

    //coputes the convolutin of the kernel and the data using mirroring on the sides
    public static float[] Convolve(float[] kernel, float[] data ) {
        if (kernel.Length>data.Length) {
            float[] tmp = data;
            data = kernel;
            kernel = tmp;
        }
        //if the length is odd
        if(kernel.Length%2==0) {
            Debug.LogError("The Kernel cant have an even length");
        }
        float[] result = new float[data.Length];
        for (int i=0;i<data.Length;i++) {
            //for each element in data calculate the sum of the kernel*the relative data
            float sum = 0;
            //j iterates through kernal from back to front
            for (int j=kernel.Length-1;j>-1;j--) {
                //k represents the kernel position relative to the element in data[i]
                int k = j - kernel.Length / 2;

                //if i+k overflows data
                if (i + k > data.Length-1) sum += data[k] * kernel[j];
                //if i+k underflows data
                else if (i + k < 0) sum += data[data.Length+k] * kernel[j];
                //else i+k must lie in the domain 0 - data.Length
                else sum += data[i + k] * kernel[j];
            }
            result[i] = sum;
        }
        return result;
	}

    //alpha is the height of the bell curve
    public static float[] GetGaussian(int size, float stdDev, float alpha) {
        //if the length is odd
        if (size % 2 == 0) {
            Debug.LogError("The Kernel cant have an even length");
        }
        float[] result = new float[size];
        for (int i=0;i<size;i++) {
            //j is the position on the gaussian distribution
            int j = i - size / 2;
            //need these helper floats because C# does division stupid
            float helper = (float)j / stdDev;
            float secondHelper = -(1/2f)*helper*helper;
            //let j be the x variable on the gaussian distribution
            result[i] = alpha*Mathf.Pow(e, secondHelper);
        }
        return result;
    }
}
