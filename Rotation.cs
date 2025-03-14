using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rotation
{
    public enum PlaneOfRototation
    {
        XY,
        XZ,
        XW,
        YZ,
        YW,
        ZW
    }

    public static Vector4 Rotate(this Vector4 vector, PlaneOfRototation plane, float angle)
    {
        float[,] vectorMatrix = new float[1, 4] { { vector.x, vector.y, vector.z, vector.w } };

        float[,] rotationMatrix;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        // Thanks to https://math.stackexchange.com/a/3311905
        switch (plane)
        {
            case PlaneOfRototation.XY:
                rotationMatrix = new float[4,4] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, cos, -sin },
                    { 0, 0, sin, cos }
                };
                break;
            case PlaneOfRototation.XZ:
                rotationMatrix = new float[4,4] {
                    { 1, 0, 0, 0 },
                    { 0, cos, 0, -sin },
                    { 0, 0, 1, 0 },
                    { 0, sin, 0, cos }
                };
                break;
            case PlaneOfRototation.XW:
                rotationMatrix = new float[4,4] {
                    { 1, 0, 0, 0 },
                    { 0, cos, -sin, 0 },
                    { 0, sin, cos, 0 },
                    { 0, 0, 0, 1 }
                };
                break;
            case PlaneOfRototation.YZ:
                rotationMatrix = new float[4,4] {
                    { cos, 0, 0, -sin },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { sin, 0, 0, cos }
                };
                break;
            case PlaneOfRototation.YW:
                rotationMatrix = new float[4,4] {
                    { cos, 0, -sin, 0 },
                    { 0, 1, 0, 0 },
                    { sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                };
                break;
            case PlaneOfRototation.ZW:
                rotationMatrix = new float[4,4] {
                    { cos, -sin, 0, 0 },
                    { sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
                break;
            default:
                throw new System.ArgumentException("Invalid plane of rotation");
        }

        float[,] resultMatrix = Helpers.MultiplyMatrix(vectorMatrix, rotationMatrix);

        return new Vector4(resultMatrix[0, 0], resultMatrix[0, 1], resultMatrix[0, 2], resultMatrix[0, 3]);
    }
}
