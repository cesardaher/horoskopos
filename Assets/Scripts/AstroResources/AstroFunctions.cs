using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroResources
{
    public class AstroFunctions : MonoBehaviour
    {
        // Gives an absolute distance between two longitude points, regardless of the order
        // This considers a 360 cycle
        public static double GetAbsolute360Distance(double x, double y)
        {
            // get distance
            double diff = x - y;

            // convert to 360 format
            if (Math.Abs(diff) > 180)
            {
                diff = Math.Abs(diff);
                diff -= 360;
            }

            // return absolute value
            return Math.Abs(diff);
        }

        // Gives a distance between two longitude points in 360, taking the order into account
        public static double Get360Distance(double firstLon, double secondLon)
        {
            // get distance and store the sign
            double diff = firstLon - secondLon;
            bool isPositive = diff >= 0;

            // convert to 360 format
            if (Math.Abs(diff) > 180)
            {
                diff = Math.Abs(diff);
                diff -= 360;
            }

            // apply sign to end result
            diff = Math.Abs(diff);
            if (!isPositive) diff *= -1;

            return diff;
        }

        // Converts decimal degrees to an array of 00° 00' 00"
        public static void DecimalToMinSec(double val, out int[] degMinSec)
        {
            degMinSec = new int[3];

            degMinSec[0] = (int)Math.Truncate(val % 30);

            double tempMin = val - Math.Truncate(val);

            degMinSec[1] = (int)Math.Truncate(tempMin * 60);

            double tempSec = tempMin * 60 - Math.Truncate(tempMin * 60);

            degMinSec[2] = (int)Math.Truncate(tempSec * 60);

        }

        // Converts overflowing degrees values to 360
        public static double ConvertDegreesTo360(double val)
        {
            if (val >= 360)
                val %= 360;

            return val;
        }

        // Converts decimal degrees to a string of 00° 00' 00"
        public static string DegreesStringFormat(int[] deg)
        {
            string minus = "-";
            string val = "";

            if (deg[0] < 0 || deg[1] < 0 || deg[2] < 0) val = minus;

            val += ToDoubleDigit(Mathf.Abs(deg[0])) + "°" + ToDoubleDigit(Mathf.Abs(deg[1])) + "\'" + ToDoubleDigit(Mathf.Abs(deg[2])) + "\"";

            return val;
        }

        // Changes an integer from 0-9 into double digit
        public static string ToDoubleDigit(int val)
        {
            string finalVal = val.ToString();

            if (val < 10)
            {
                finalVal = "0" + finalVal;
                return finalVal;
            }

            return finalVal;
        }

        // Changes an string from 0-9 into double digit
        public static string ToDoubleDigit(string val)
        {
            int intVal = int.Parse(val);

            if (intVal < 10)
            {
                val = 0 + val;
                return val;
            }

            return val;

        }

        // Gets horizontal coordinates and converts them to cartesian
        public static Vector3 HorizontalToCartesian(double azimuth, double altitude)
        {
            double x, y, z;
            float radius = 10000;

            double alt = altitude;
            double az = -azimuth;

            double a = radius * Math.Cos(alt * Mathf.Deg2Rad);
            x = a * Math.Cos(az * Mathf.Deg2Rad);
            y = radius * Math.Sin(alt * Mathf.Deg2Rad);
            z = a * Math.Sin(az * Mathf.Deg2Rad);

            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}
