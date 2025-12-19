using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TriangleData
{
    public int Depan;
    public int Samping;
    public int Miring;
    public string SoalDisederhanakan;
    public float JawabanBenar;
}

public class TriangleDataGenerator : MonoBehaviour
{
    private List<(int, int, int)> triples = new List<(int, int, int)>
    {
        (3, 4, 5), (5, 12, 13), (8, 15, 17), (7, 24, 25)
    };

    public TriangleData GenerateNewQuestion()
    {
        (int a, int b, int c) triple = triples[Random.Range(0, triples.Count)];
        bool isADepan = Random.Range(0, 2) == 0;

        TriangleData data = new TriangleData();
        data.Depan = isADepan ? triple.a : triple.b;
        data.Samping = isADepan ? triple.b : triple.a;
        data.Miring = triple.c;

        int questionType = Random.Range(0, 3);
        switch (questionType)
        {
            case 0: // Sin
                data.SoalDisederhanakan = "Sin\u03B8"; // Unicode theta
                data.JawabanBenar = (float)data.Depan / data.Miring;
                break;
            case 1: // Cos
                data.SoalDisederhanakan = "Cos\u03B8"; // Unicode theta
                data.JawabanBenar = (float)data.Samping / data.Miring;
                break;
            case 2: // Tan
                data.SoalDisederhanakan = "Tan\u03B8"; // Unicode theta
                data.JawabanBenar = (float)data.Depan / data.Samping;
                break;
        }
        return data;
    }
}