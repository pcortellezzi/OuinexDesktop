﻿using System;
using System.Diagnostics;
using System.Linq;

namespace CNergyTrader.Indicator
{
    public static class Formulas
    {
        public static double GetSMA(this double[] input, int index, int period)
        {
            var result = 0.0;

            if (index >= period - 1 && period > 0)
            {
                for (int i = 0; i < period; i++)
                {
                    result += input[index - i];
                }

                result /= period;
            }
            else
            {
                result = double.NaN;
            }

            return result;
        }

        public static double GetSMA(this XYSerie input, int index, int period)
        {
            var array = input.ToArray();

            return array.GetSMA(index, period);
        }

        public static double GetEMA(this double[] input, int index, int period, double previous)
        {
            var result = 0.0;

            if (period > 0)
            {
                double pr = 2.0 / (period + 1.0);
                result = input[index] * pr + previous * (1 - pr);
            }
            else
            {
                result = double.NaN;
            }

            return result;
        }

        public static double GetEMA(this XYSerie input, int index, int period, double previous)
        {
            var array = input.ToArray();

            return array.GetEMA(index, period, previous);
        }

        public static double GetStdDev(this double[]input, int index, int period, double[] inputMa)
        {
            double result = 0.0;

            if (index > period)
            {
                for (int i = 0; i < period; i++)
                {
                    result += Math.Pow(input[index - i] - inputMa[index - 1], 2);
                }

                result = Math.Sqrt(result / period);
            }
            else
            {
                result = double.NaN;
            }

            return (result);
        }

        public static double GetStdDev(this XYSerie input, int index, int period, double[] inputMa)
        {
            var array = input.ToArray();

            return array.GetStdDev(index, period, inputMa);
        }

        public static double GetLowest(this double[]input, int index, int period)
        {
            if (index < period)
            {
                return double.NaN;
            }

            var res = input[index];

            for (var i = index; i > index - period && i >= 0; i--)
            {
                if (res > input[i]) res = input[i];
            }

            return res;
        }

        public static double GetLowest(this XYSerie input, int index, int period)
        {
            var array = input.ToArray();

            return array.GetLowest(index, period);
        }

        public static double GetHighest(this double[] input, int index, int period)
        {
            if (index < period)
            {
                return double.NaN;
            }

            var res = input[index];

            for (var i = index; i > index - period && i >= 0; i--)
            {
                 res = Math.Max(res, input[i]);
            }

            Debug.WriteLine(index+" - "+res);

            return res;
        }

        public static double GetHighest(this XYSerie input, int index, int period)
        {
            var array = input.ToArray();

            return array.GetHighest(index, period);
        }

        public static double FindHighestPrice(this double[] prices, int index, int period)
        {
            int start_index = Math.Max(0, index - period + 1);
            int end_index = Math.Min(index + 1, prices.Length);
            double highest_price = double.MinValue;
            for (int i = start_index; i < end_index; i++)
            {
                if (prices[i] > highest_price)
                {
                    highest_price = prices[i];
                }
            }
            return highest_price;
        }
    }
}
