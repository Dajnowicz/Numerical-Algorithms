﻿using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace AproximationAltitudeProfile
{
    public static class DataParser
    {
        public static List<DataPoint> ParsePointData(string fileName, int routeNumber)
        {
            var resultPointList = new List<DataPoint>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var counter = -1;

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            var valString = reader.GetValue(1).ToString();
                            if (valString == "elevation")
                                continue;
                            resultPointList.Add(new DataPoint(counter, double.Parse(valString.Replace(".", ","))));
                            counter++;
                        }
                    } while (reader.NextResult());
                }
            }

            resultPointList.RemoveAt(0);

            return resultPointList;
        }
    }
}
