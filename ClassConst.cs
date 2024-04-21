using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAndGasCloud
{
    public static class ClassConst
    {
        public const double mu1 = 207.2 / 1000; // kg/mol molarMassOfAblationProducts
        public const double mu2 = 18 / 1000;  // kg/mol   molarMassOfWaterVapor
        public const double universalGasConstant = 8.31; // J/mol*K
        public const double Cp1 = 140.3716; // J/kg*K   heatCapacityOfAblationProducts
        public const double Cp2 = 2034; // J/kg*K  heatCapacityOfWaterVapor
        public const double alpha1 = 31.5; // W/m*K  coefficientOfThermalConductivityOfAblationProducts
        public const double  alpha2 = 237.2; // W/m*K coefficientOfThermalConductivityOfWaterVapor
        public const int h  = 2300000; // J/kg   specificHeatOfWaterVaporization
        public const int atmospherePressure = 101000; // Па
        public const int massOfWaterInCuvette = 1; // kg ???????????????????????????????????
        public const double surfaceAreaOfStaemGasBubble = 1; // m2 ????????????????????????????
        public const double accelerationOfSurfaceStaemGasBubble = 1; // m/s2 ????????????????????????????
        public const double powerDensityOfSourceOfAblationProducts = 1; // A(t)d(r-r0) ????????????
        public const double powerDensityOfSourceOfWaterVapor = 1; // B(t)d(r-r0) ????????????
        public const double D = 1; // ?????????????????????

        // border conditions
        public const int T0 = 7000; //K (r=r0)
        public const int Tb = 373; //K (r=rb)
        public const double r0 = 2 / 1000; //m
        

    }
}
