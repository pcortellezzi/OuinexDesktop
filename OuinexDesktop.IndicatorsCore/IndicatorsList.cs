using CNergyTrader.Indicator.Indicators;
using System;
using System.Collections.Generic;

namespace CNergyTrader.Indicator
{
    public static class IndicatorsList
    {
        public static Dictionary<string, Type> Indicators
        {
            get
            {
                return new Dictionary<string, Type>()
                {
                    {"ATR", typeof(ATR) },
                    {"ATR Stop", typeof(ATRStop) },
                    {"Bollinger Bands", typeof(BollingerBands) },
                    {"Bull & Bear power", typeof(BullBearPower) },
                    {"CCI", typeof(CCI) },
                    {"Exponential moving average", typeof(ExponentialMovingAverage)},
                    {"Ichimoku", typeof(Ichimoku) },
                    {"Raghee Horner Wave", typeof(RagheeWave) },
                    {"Raghee Oscillator", typeof(RagheeOscillator) },
                    {"MACD", typeof(MACD) },
                    {"Momentum", typeof(Momentum) },
                    //{"OBV", typeof(OBV) },
                    {"RSI", typeof(RSI) },
                    {"Simple moving average", typeof(SimpleMovingAverage) },
                    {"Stochastic", typeof(Stochastic) },
                    //{"Volumes", typeof(Volume) }
                };
            }
        }
    }
}
