using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace GaussLinear
{
    public class EfficiencyTest
    {
        private int _dimensions;
        private double[] _time;
        private double[] _diff;
        private int _maxValue = 65536; // 2^16

        public EfficiencyTest(int dim)
        {
            _dimensions = dim;
            _time = new double[9];
            _diff = new double[9];
        }

        public async Task<string> Run()
        {
            Console.WriteLine("Test: " + _dimensions);

            var rand = new Random();
            var matrix = new int[_dimensions, _dimensions];
            var vector = new int[_dimensions];
            for (int i = 0; i < _dimensions; i++)
            {
                matrix[i, 0] = (rand.Next(-_maxValue, _maxValue - 1));
                vector[i] = (rand.Next(-_maxValue, _maxValue - 1));
                for (int j = 0; j < _dimensions; j++)
                {
                    matrix[i, j] = (rand.Next(-_maxValue, _maxValue - 1));
                }
            }
            var tasks = new List<Task>
            {
            Task.Factory.StartNew(() =>
            {
                RunFraction(GaussType.Basic, matrix, vector);
            }),
            Task.Factory.StartNew(() =>
            {
                RunFraction(GaussType.Part, matrix, vector);
            }),
            Task.Factory.StartNew(() =>
            {
                RunFraction(GaussType.Full, matrix, vector);
            }),
            Task.Factory.StartNew(() =>
            {
              RunFloat(GaussType.Basic, matrix, vector);
            }),
            Task.Factory.StartNew(() =>
            {
              RunFloat(GaussType.Part, matrix, vector);
            }),
            Task.Factory.StartNew(() =>
            {
               RunFloat(GaussType.Full, matrix, vector);
             }),
            Task.Factory.StartNew(() =>
            {
               RunDouble(GaussType.Basic, matrix, vector);
            }),
            Task.Factory.StartNew(() =>
            {
               RunDouble(GaussType.Part, matrix, vector);
            }),
            Task.Factory.StartNew(() =>
            {
               RunDouble(GaussType.Full, matrix, vector);
            })};

            await Task.WhenAll(tasks);

            return string.Join(";", _time) + ";" + string.Join(";", _diff);
        }


        public void RunFloat(GaussType gausstype, int[,] matrix, int[] vector)
        {
            var floatMatrix = new MyMatrix<float>(_dimensions);

            for (int y = 0; y < _dimensions; y++)
            {
                floatMatrix.SetVectorB(y, 0);
                floatMatrix.SetVectorX(y, ((float)vector[y] / _maxValue));
                for (int x = 0; x < _dimensions; x++)
                {
                    floatMatrix.SetMatrixA(x, y, ((float)matrix[y, x] / _maxValue));
                }
            }
            floatMatrix.Multiplication();

            var sw = new Stopwatch();
            switch (gausstype)
            {
                case GaussType.Basic:
                    sw.Start();
                    floatMatrix.CalculateG();
                    sw.Stop();
                    _time[0] = sw.Elapsed.TotalMilliseconds;
                    _diff[0] = floatMatrix.CalculateDiff();
                    break;
                case GaussType.Part:
                    sw.Start();
                    floatMatrix.CalculateGP();
                    sw.Stop();
                    _time[1] = sw.Elapsed.TotalMilliseconds;
                    _diff[1] = floatMatrix.CalculateDiff();
                    break;
                case GaussType.Full:
                    sw.Start();
                    floatMatrix.CalculateGF();
                    sw.Stop();
                    _time[2] = sw.Elapsed.TotalMilliseconds;
                    _diff[2] = floatMatrix.CalculateDiff();
                    break;
            }

            sw.Stop();
        }

        public void RunDouble(GaussType gausstype, int[,] matrix, int[] vector)
        {
            var doubleMatrix = new MyMatrix<double>(_dimensions);
            for (int y = 0; y < _dimensions; y++)
            {
                doubleMatrix.SetVectorB(y, 0);
                doubleMatrix.SetVectorX(y, ((double)vector[y] / _maxValue));
                for (int x = 0; x < _dimensions; x++)
                {
                    doubleMatrix.SetMatrixA(x, y, ((double)matrix[y, x] / _maxValue));
                }
            }
            doubleMatrix.Multiplication();

            var sw = new Stopwatch();

            switch (gausstype)
            {
                case GaussType.Basic:
                    sw.Start();
                    doubleMatrix.CalculateG();
                    sw.Stop();
                    _time[3] = sw.Elapsed.TotalMilliseconds;
                    _diff[3] = doubleMatrix.CalculateDiff();
                    break;
                case GaussType.Part:
                    sw.Start();
                    doubleMatrix.CalculateGP();
                    sw.Stop();
                    _time[4] = sw.Elapsed.TotalMilliseconds;
                    _diff[4] = doubleMatrix.CalculateDiff();
                    break;
                case GaussType.Full:
                    sw.Start();
                    doubleMatrix.CalculateGF();
                    sw.Stop();
                    _time[5] = sw.Elapsed.TotalMilliseconds;
                    _diff[5] = doubleMatrix.CalculateDiff();
                    break;
            }
        }

        public void RunFraction(GaussType gausstype, int[,] matrix, int[] vector)
        {
            var fractionMatrix = new MyMatrix<Fraction>(_dimensions);

            for (int y = 0; y < _dimensions; y++)
            {
                fractionMatrix.SetVectorB(y, 0);
                fractionMatrix.SetVectorX(y, new Fraction(vector[y], _maxValue));
                for (int x = 0; x < _dimensions; x++)
                {
                    fractionMatrix.SetMatrixA(x, y, new Fraction(matrix[y, x], _maxValue));
                }
            }
            fractionMatrix.Multiplication();

            var sw = new Stopwatch();

            switch (gausstype)
            {
                case GaussType.Basic:
                    sw.Start();
                    fractionMatrix.CalculateG();
                    sw.Stop();
                    _time[6] = sw.Elapsed.TotalMilliseconds;
                    _diff[6] = fractionMatrix.CalculateDiff();
                    break;
                case GaussType.Part:
                    sw.Start();
                    fractionMatrix.CalculateGP();
                    sw.Stop();
                    _time[7] = sw.Elapsed.TotalMilliseconds;
                    _diff[7] = fractionMatrix.CalculateDiff();
                    break;
                case GaussType.Full:
                    sw.Start();
                    fractionMatrix.CalculateGF();
                    sw.Stop();
                    _time[8] = sw.Elapsed.TotalMilliseconds;
                    _diff[8] = fractionMatrix.CalculateDiff();
                    break;
            }
        }

        private double CalculateDoubleDiff(double[] vectorB, Fraction fraction)
        {
            double valueOfVectorB = 0;
            foreach (var item in vectorB)
            {
                valueOfVectorB += item;
            }
            var doubleFraction = (double)fraction;
            return Math.Abs((valueOfVectorB - doubleFraction) / doubleFraction);
        }
        private float CalculateFloatDiff(float[] vectorB, Fraction fraction)
        {
            float valueOfVectorB = 0;
            foreach (var item in vectorB)
            {
                valueOfVectorB += item;
            }
            var floatFraction = (float)fraction;
            return Math.Abs((valueOfVectorB - floatFraction) / floatFraction);
        }

        private Fraction CalculateAverageDiffForFraction(List<Fraction> vectorB)
        {
            var numeratorResultList = new List<BigInteger>();
            var numeratorList = vectorB.Select(c => c.Numerator);
            foreach (var item in numeratorList)
            {
                numeratorResultList.Add(item);
            }

            return new Fraction(numeratorResultList.Sum(), _maxValue);
        }
    }
}
