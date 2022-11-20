namespace Essentials;

public static class Extensions
{
    public static float[] ConvertToFloats(this double[] arr)
    {
        float[] temp = new float[arr.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            temp[i] = (float)arr[i];
        }

        return temp;
    }
}