using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSS
{
    class Element
    {
        public double[] e;
        public Element()
        {
            e = new double[5];
        }
    }
    class FuzzyTOPSIS
    {

        public double[] VL = new double[] { 0, 0, 0.1, 0.2, 1 };
        public double[] L = new double[] { 0.1, 0.2, 0.2, 0.3, 1 };
        public double[] ML = new double[] { 0.2, 0.3, 0.4, 0.5, 1 };
        public double[] M = new double[] { 0.4, 0.5, 0.5, 0.6, 1 };
        public double[] MH = new double[] { 0.5, 0.6, 0.7, 0.8, 1 };
        public double[] H = new double[] { 0.7, 0.8, 0.8, 0.9, 1 };
        public double[] VH = new double[] { 0.8, 0.9, 1.0, 1.0, 1 };


        public double[] VP = new double[] { 0, 0, 1, 2, 1 };
        public double[] P = new double[] { 1, 2, 2, 3, 1 };
        public double[] MP = new double[] { 2, 3, 4, 5, 1 };
        public double[] F = new double[] { 4, 5, 5, 6, 1 };
        public double[] MG = new double[] { 5, 6, 7, 8, 1 };
        public double[] G = new double[] { 7, 8, 8, 9, 1 };
        public double[] VG = new double[] { 8, 9, 10, 10, 1 };
        public void Run()
        {

        }
        public double[] Do(List<Element[]> WD, List<Element[][]> FDM, int k, int n, int m, int ideal, int[] criteria)
        {
            double[][] Waag = AggregateW(k, n, WD);
            double[][][] FDMA = new double[m][][];
            double[][][] FDMN = new double[m][][];
            double[][][] FDMNW = new double[m][][];
            double[][] matrixB = new double[m][];
            double[][] matrixC = new double[m][];
            double[][] maxi2 = new double[m][];
            double[][] FPIS = new double[n][];
            double[][] FNIS = new double[n][];
            double[][] Sstar = new double[m][];
            double[][] Sneg = new double[m][];
            double[] dividerB = new double[n];
            double[] dividerC = new double[n];
            for (int i = 0; i < FNIS.Length; i++)
            {
                FPIS[i] = new double[4];
                FNIS[i] = new double[4];
            }
            for (int i = 0; i < maxi2.Length; i++)
            {
                maxi2[i] = new double[4];
            }
            for (int i = 0; i < matrixB.Length; i++)
            {
                matrixB[i] = new double[n];
                matrixC[i] = new double[n];
            }
            for (int i = 0; i < FDMN.Length; i++)
            {

            }
            for (int i = 0; i < m; i++)
            {
                FDMA[i] = AggregateFDM(k, n, i, FDM);
                FDMN[i] = AggregateFDM(k, n, i, FDM);
                FDMNW[i] = AggregateFDM(k, n, i, FDM);
            }
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    matrixB[i][j] = FDMA[i][j][3];
                    matrixC[i][j] = FDMA[i][j][0];
                }
            }
            for (int j = 0; j < n; j++)
            {
                double[] temp = new double[matrixB.Length];
                double[] temp2 = new double[matrixC.Length];
                for (int i = 0; i < matrixB.Length; i++)
                {
                    temp[i] = matrixB[i][j];
                    temp2[i] = matrixC[i][j];
                }
                dividerB[j] = temp.Max();
                dividerC[j] = temp2.Min();
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (criteria[j] == 1)
                    {
                        FDMN[i][j][0] = FDMA[i][j][0] / dividerB[j];
                        FDMN[i][j][1] = FDMA[i][j][1] / dividerB[j];
                        FDMN[i][j][2] = FDMA[i][j][2] / dividerB[j];
                        FDMN[i][j][3] = FDMA[i][j][3] / dividerB[j];

                    }
                    if (criteria[j] == 2)
                    {
                        FDMN[i][j][0] = dividerC[j] / FDMA[i][j][3];
                        FDMN[i][j][1] = dividerC[j] / FDMA[i][j][2];
                        FDMN[i][j][2] = dividerC[j] / FDMA[i][j][1];
                        FDMN[i][j][3] = dividerC[j] / FDMA[i][j][0];

                    }

                }
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    FDMNW[i][j][0] = FDMN[i][j][0] * Waag[j][0];
                    FDMNW[i][j][1] = FDMN[i][j][1] * Waag[j][1];
                    FDMNW[i][j][2] = FDMN[i][j][2] * Waag[j][2];
                    FDMNW[i][j][3] = FDMN[i][j][3] * Waag[j][3];
                }
            }
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    maxi2[i] = FDMNW[i][j];
                }
                for (int f = 0; f < 4; f++)
                {
                    double[] temp = new double[maxi2.Length];
                    double[] temp2 = new double[maxi2.Length];
                    for (int i = 0; i < maxi2.Length; i++)
                    {
                        temp[i] = maxi2[i][f];
                        temp2[i] = maxi2[i][f];
                    }
                    FPIS[j][f] = temp.Max();
                    FNIS[j][f] = temp2.Min();
                }
            }
            double[] SstarAgg = new double[m];
            double[] SnegAgg = new double[m];
            for (int i = 0; i < m; i++)
            {
                Sstar[i] = fuzzysimveca(FDMNW[i], FPIS, n);
                SstarAgg[i] = Sstar[i].Average();
            }
            for (int i = 0; i < m; i++)
            {
                Sneg[i] = fuzzysimveca(FDMNW[i], FNIS, n);
                SnegAgg[i] = Sneg[i].Average();
            }
            double[] CCS = new double[m];
            for (int i = 0; i < m; i++)
            {
                CCS[i] = SstarAgg[i] / (SstarAgg[i] + SnegAgg[i]);
            }
            return CCS;

        }
        private double[] fuzzysimveca(double[][] FDMNW, double[][] FPIS, int n)
        {
            double[] sstar = new double[n];
            for (int i = 0; i < n; i++)
            {
                sstar[i] = similarity(FDMNW[i], FPIS[i]);

            }
            return sstar;
        }
        private double similarity(double[] a, double[] b)
        {
            double pa = (double)(a[0] + 2 * a[1] + 2 * a[2] + a[3]) / (double)6;
            double pb = (double)(b[0] + 2 * b[1] + 2 * b[2] + b[3]) / (double)6;
            double dab = Math.Abs(pa - pb);
            return (double)1 / (double)(1 + dab);

        }
        private double[][] AggregateW(int k, int n, List<Element[]> WD)
        {
            double[] wj1a = new double[n];
            double[] wj2a = new double[n];
            double[] wj3a = new double[n];
            double[] wj4a = new double[n];
            double[][] W = new double[4][];
            double[][] wv = new double[n][];
            for (int i = 0; i < wv.Length; i++)
            {
                wv[i] = new double[4];
            }
            for (int i = 0; i < W.Length; i++)
            {
                W[i] = new double[n];
            }
            for (int i = 0; i < n; i++)
            {
                double[][] tmp = new double[4][];
                for (int m = 0; m < 4; m++)
                {
                    tmp[m] = new double[k];
                }
                for (int j = 0; j < k; j++)
                {
                    tmp[0][j] = WD[j][i].e[0];
                    tmp[1][j] = WD[j][i].e[1];
                    tmp[2][j] = WD[j][i].e[2];
                    tmp[3][j] = WD[j][i].e[3];
                }
                wj1a[i] = tmp[0].Min();
                wj2a[i] = tmp[1].Sum() / (double)k;
                wj3a[i] = tmp[2].Sum() / (double)k;
                wj4a[i] = tmp[3].Max();
            }
            W[0] = wj1a;
            W[1] = wj2a;
            W[2] = wj3a;
            W[3] = wj4a;
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < W.Length; i++)
                {
                    wv[j][i] = W[i][j];
                }
            }
            return wv;
        }
        private double[][] AggregateFDM(int k, int n, int z, List<Element[][]> WD)
        {
            double[] wj1a = new double[n];
            double[] wj2a = new double[n];
            double[] wj3a = new double[n];
            double[] wj4a = new double[n];
            double[][] W = new double[4][];
            double[][] FDM2 = new double[n][];
            for (int i = 0; i < FDM2.Length; i++)
            {
                FDM2[i] = new double[n];
            }
            for (int i = 0; i < W.Length; i++)
            {
                W[i] = new double[n];
            }
            for (int i = 0; i < n; i++)
            {
                double[][] tmp = new double[4][];
                for (int m = 0; m < 4; m++)
                {
                    tmp[m] = new double[k];
                }
                for (int j = 0; j < k; j++)
                {
                    tmp[0][j] = WD[j][z][i].e[0];
                    tmp[1][j] = WD[j][z][i].e[1];
                    tmp[2][j] = WD[j][z][i].e[2];
                    tmp[3][j] = WD[j][z][i].e[3];
                }
                wj1a[i] = tmp[0].Min();
                wj2a[i] = tmp[1].Sum() / (double)k;
                wj3a[i] = tmp[2].Sum() / (double)k;
                wj4a[i] = tmp[3].Max();
            }
            W[0] = wj1a;
            W[1] = wj2a;
            W[2] = wj3a;
            W[3] = wj4a;
            for (int j = 0; j < n; j++)
            {
                Element e = new Element();
                for (int i = 0; i < W.Length; i++)
                {
                    e.e[i] = W[i][j];
                }
                FDM2[j] = e.e;
            }
            return FDM2;
        }
    }
}

