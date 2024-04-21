using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamAndGasCloud
{
    internal class CalculationClass
    {
        List<double> ro1; // densityOfAblationProducts;    
        List<double> ro2; // densityOfWaterVapor;   
        List<double> V; // flowRateOfAblationProducts;  
        List<double> U; // flowRateOfWaterVapor; 
        List<double> P; // pressureOfSteamGasMixture; 
        List<double> T; // temperature; 
        List<double> r; // bubble radius; 
        double dt;
        double dr;

        public void SetBoundaryConditions()
        {

        }

        public void СalculateTemperature()
        {

        }


        public double СalculateDensityOfAblationProducts(double deltaFunc, double constValueA, double currentTime, double tDelta, double Vi, double ViPlus1, double ro1i)
        {
            return ro1i + (GetConstWithDeltaFunc(deltaFunc, constValueA, currentTime) - GetAcomponentFromDensity(currentTime, tDelta, Vi, ViPlus1, ro1i)) / GetValueDeltaRDeltaT(tDelta, Vi, currentTime - tDelta, currentTime);
        }
        public double СalculateDensityOfWaterVapor(double deltaFunc, double constValueB, double currentTime, double tDelta, double Ui, double UiPlus1, double ro2i)
        {
            return ro2i + (GetConstWithDeltaFunc(deltaFunc, constValueB, currentTime) - GetAcomponentFromDensity(currentTime, tDelta, Ui, UiPlus1, ro2i)) / GetValueDeltaRDeltaT(tDelta, Ui, currentTime - tDelta, currentTime);

        }

        private double GetAcomponentFromDensity(double currentTime, double tDelta, double value, double valuePlus1, double ro)
        {
            var a1 = Get2ValueToRi(value, currentTime);
            var a2 = (valuePlus1 - value) / GetDeltaRadius(currentTime - tDelta, currentTime);
            return ro * (a1 + a2);
        }


        public double СalculateFlowRateOfAblationProducts(double currentTime, double tDelta, double Vi, double piPlus1, double pi, double ro1i)
        {
            return GetAcomponentFromFlowRate(currentTime, tDelta, Vi) * GetBcomponentFromFlowRate(currentTime, tDelta, Vi) - GetCcomponentFromFlowRate(currentTime, tDelta, Vi, piPlus1, pi, ro1i);
        }
        public double СalculateFlowRateOfWaterVapor(double currentTime, double tDelta, double Ui, double piPlus1, double pi, double ro2i)
        {
            return GetAcomponentFromFlowRate(currentTime, tDelta, Ui) * GetBcomponentFromFlowRate(currentTime, tDelta, Ui) - GetCcomponentFromFlowRate(currentTime, tDelta, Ui, piPlus1, pi, ro2i);
        }

        private double GetAcomponentFromFlowRate(double currentTime, double tDelta, double value)
        {
            return value / GetValueDeltaRDeltaT(tDelta, value, currentTime - tDelta, currentTime);
        }

        private double GetBcomponentFromFlowRate(double currentTime, double tDelta, double value)
        {
            var b1 = Get2ValueToRi(value, currentTime);
            var b2 = GetValueDeltaRDeltaT(tDelta, value, currentTime - tDelta, currentTime);
            return b2 - b1;
        }

        private double GetCcomponentFromFlowRate(double currentTime, double tDelta, double value, double piPlus1, double pi, double roi)
        {
            var c1 = piPlus1 - pi;
            var c2 = GetValueDeltaRDeltaT(tDelta, value, currentTime - tDelta, currentTime) * roi * GetDeltaRadius(currentTime - tDelta, currentTime);
            return c1 / c2;
        }
        public double СalculatePressureOfSteamGasMixture(double deltaFunk, double constA, double constB, double currentTime, double tDelta, double Ui, double Vi, double ro1i, double ro2i, double pi)
        {
            return GetAcomponentFromPi(currentTime, tDelta, Vi, Ui) * GetBcomponentFromPi(deltaFunk, constA, constB, currentTime, tDelta, Ui, Vi, ro1i, ro2i, pi);
        }

        private double GetConstWithDeltaFunc(double deltaFunc, double constValue, double currentTime)
        {
            return constValue * deltaFunc * (GetRadius(currentTime) - ClassConst.r0);
        }
        private double GetBcomponentFromPi(double deltaFunc, double constA, double constB, double currentTime, double tDelta, double Ui, double Vi, double ro1i, double ro2i, double pi)
        {
            var c1 = GetCcomponentFromPi(Ui, currentTime, pi, ro2i, tDelta, ClassConst.mu2);
            var c2 = GetCcomponentFromPi(Vi, currentTime, pi, ro1i, tDelta, ClassConst.mu1);

            var c3 = ro1i / ClassConst.mu1;
            var c4 = ro2i / ClassConst.mu2;

            var valueDeltaR = GetValueDeltaRDeltaT(tDelta, Vi, currentTime - tDelta, currentTime);

            var c5 = GetConstWithDeltaFunc(deltaFunc, constA, currentTime) / (ClassConst.mu1 * valueDeltaR);
            var c6 = GetConstWithDeltaFunc(deltaFunc, constB, currentTime) / (ClassConst.mu2 * valueDeltaR); ;

            var c7 = (2 * ro1i * Vi) / (ClassConst.mu1 * valueDeltaR * GetRadius(currentTime));
            var c8 = (2 * ro2i * Ui) / (ClassConst.mu2 * valueDeltaR * GetRadius(currentTime));

            return c1 + c3 + c5 - c7 + c2 + c4 + c6 - c8;
        }

        private double GetAcomponentFromPi(double currentTime, double tDelta, double Vi, double Ui)
        {
            var up = ClassConst.universalGasConstant * GetTi();

            var down1 = 1 - up;
            var deltaR = GetDeltaRadius(currentTime - tDelta, currentTime);

            double calc(double mui, double value)
            {
                var valueDeltaR = GetValueDeltaRDeltaT(tDelta, value, currentTime - tDelta, currentTime);
                var a = mui * deltaR * deltaR * valueDeltaR * valueDeltaR;

                return 1 / a;
            }

            var down2 = calc(ClassConst.mu1, Vi);
            var down3 = calc(ClassConst.mu2, Ui);

            return up / (down1 * (down2 + down3));
        }


        private double GetTi()
        {
            return 0.0;
        }

        private double GetCcomponentFromPi(double value, double currentTime, double pi, double roi, double tDelta, double mu)
        {
            var up = GetNumeratorCcomponentFromPi(value, currentTime, pi, roi, tDelta);
            var down1 = GetValueDeltaRDeltaT(tDelta, value, currentTime - tDelta, currentTime);
            var deltaR = GetDeltaRadius(currentTime - tDelta, currentTime);
            var down2 = down1 * down1 * mu * deltaR * deltaR;

            return up / down2;
        }


        //value can be Vi or Ui
        //roi can be ro1i or ro2i
        private double GetNumeratorCcomponentFromPi(double value, double currentTime, double pi, double roi, double tDelta)
        {
            return pi + roi * GetDeltaRadius(currentTime - tDelta, currentTime) * value * Get2ValueToRi(value, currentTime);
        }
        private double Get2ValueToRi(double value, double currentTime)
        {
            return 2 * value / GetRadius(currentTime);
        }
        private double GetValueDeltaRDeltaT(double tDelta, double value, double time1, double time2)
        {
            var a = 1 / tDelta;

            var deltaR = GetDeltaRadius(time1, time2);

            var b = value / deltaR;

            return a + b;
        }

        private double GetDeltaRadius(double time1, double time2)
        {
            return GetRadius(time2) - GetRadius(time1);
        }
        // get ri
        private double GetRadius(double time)
        {
            double coefA = 1.8559;// метод подбора y=coefA * x ^ coefB (формула диаметра пузыря от времени y-диаметр x-время)
            double coefB = 0.1911;// метод подбора

            return coefA * Math.Pow(time, coefB) / 2;
        }
    }
}
