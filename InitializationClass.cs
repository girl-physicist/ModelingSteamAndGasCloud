using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAndGasCloud
{
    internal static class InitializationClass
    {
        static List<double> densityOfAblationProducts;    // ro1
        static List<double> densityOfWaterVapor;    // ro2
        static List<double> flowRateOfAblationProducts;  // V
        static List<double> flowRateOfWaterVapor; // U
        static List<double> pressureOfSteamGasMixture; // P

        const double molarMassOfAblationProducts = 207.2 / 1000; // kg/mol
        const double molarMassOfWaterVapor = 18 / 1000;  // kg/mol
        const double universalGasConstant = 8.31; // J/mol*K
        const double heatCapacityOfAblationProducts = 140.3716; // J/kg*K
        const double heatCapacityOfWaterVapor = 2034; // J/kg*K
        const double coefficientOfThermalConductivityOfAblationProducts = 31.5; // W/m*K 
        const double coefficientOfThermalConductivityOfWaterVapor = 237.2; // W/m*K 
        const int specificHeatOfWaterVaporization = 2300000; // J/kg
        const int atmospherePressure = 101000; // Па
        const int massOfWaterInCuvette = 1; // kg ???????????????????????????????????
        const double surfaceAreaOfStaemGasBubble = 1; // m2 ????????????????????????????
        const double accelerationOfSurfaceStaemGasBubble = 1; // m/s2 ????????????????????????????
        const double powerDensityOfSourceOfAblationProducts = 1; // A(t)d(r-r0) ????????????
        const double powerDensityOfSourceOfWaterVapor = 1; // B(t)d(r-r0) ????????????

        // border conditions
        const int T0 = 7000; //K (r=r0)
        const int Tb = 373; //K (r=rb)


    }
}
