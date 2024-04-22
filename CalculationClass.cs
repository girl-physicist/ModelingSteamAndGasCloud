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
        //ro1[i+1]
        public double СalculateDensityOfAblationProducts(double deltaFunc, double constValueA, double currentTime, double tDelta, double Vi, double ViPlus1, double ro1i)
        {
            return ro1i + (GetConstWithDeltaFunc(deltaFunc, constValueA, currentTime) - GetAcomponentFromDensity(currentTime, tDelta, Vi, ViPlus1, ro1i)) / GetValueFirstComponent(tDelta, Vi, currentTime - tDelta, currentTime);
        }
        //ro2[i+1]
        public double СalculateDensityOfWaterVapor(double deltaFunc, double constValueB, double currentTime, double tDelta, double Ui, double UiPlus1, double ro2i)
        {
            return ro2i + (GetConstWithDeltaFunc(deltaFunc, constValueB, currentTime) - GetAcomponentFromDensity(currentTime, tDelta, Ui, UiPlus1, ro2i)) / GetValueFirstComponent(tDelta, Ui, currentTime - tDelta, currentTime);

        }
        //V[i+1]
        public double СalculateFlowRateOfAblationProducts(double currentTime, double tDelta, double Vi, double piPlus1, double pi, double ro1i)
        {
            return GetFlowRate(currentTime, tDelta, Vi, piPlus1, pi, ro1i);
        }
        //U[i+1]
        public double СalculateFlowRateOfWaterVapor(double currentTime, double tDelta, double Ui, double piPlus1, double pi, double ro2i)
        {
            return GetFlowRate(currentTime, tDelta, Ui, piPlus1, pi, ro2i);
        }
        // P[i+1]
        public double СalculatePressureOfSteamGasMixture(double deltaFunk, double constA, double constB, double currentTime, double tDelta, double Ui, double Vi, double ro1i, double ro2i, double pi, double TiPlus1)
        {
            return GetAcomponentFromPi(currentTime, tDelta, Vi, Ui, TiPlus1) * GetBcomponentFromPi(deltaFunk, constA, constB, currentTime, tDelta, Ui, Vi, ro1i, ro2i, pi);
        }
        // T[i+1]
        public double СalculateTemperature(double ro1i, double ro2i, double tDelta, double Vi, double Ui, double currentTime, double TiMinus1, double TiPlus1)
        {
            return GetUpFromT(ro1i, ro2i, tDelta, Vi, Ui, currentTime, TiMinus1, TiPlus1) / GetDownFromT(ro1i, ro2i, tDelta, Vi, Ui, currentTime);
        }

        private double GetUpFromT(double ro1i, double ro2i, double tDelta, double Vi, double Ui, double currentTime, double TiMinus1, double Ti)
        {
            var up1 = GetAcomponentFromT(currentTime, tDelta);
            var up2 = TiMinus1 - 2 * Ti - 2 * Ti / GetRadius(currentTime);
            var up3 = Ti * GetBcomponentFromT(ro1i, ro2i, ClassConst.Cp1, ClassConst.Cp1, tDelta);
            var up4 = Ti * GetCcomponentFromT(ro1i, ClassConst.Cp1, ro2i, ClassConst.Cp2, Vi, Ui, currentTime, tDelta);

            return up1 * up2 + up3 + up4;
        }
        private double GetDownFromT(double ro1i, double ro2i, double tDelta, double Vi, double Ui, double currentTime)
        {
            var down1 = GetBcomponentFromT(ro1i, ro2i, ClassConst.Cp1, ClassConst.Cp1, tDelta);
            var down2 = GetCcomponentFromT(ro1i, ClassConst.Cp1, ro2i, ClassConst.Cp2, Vi, Ui, currentTime, tDelta);
            var down3 = GetAcomponentFromT(currentTime, tDelta) * (1 + 2 / GetRadius(currentTime));

            return down1 + down2 - down3;
        }
        private double GetCcomponentFromT(double ro1i, double cp1, double ro2i, double cp2, double Vi, double Ui, double currentTime, double tDelta)
        {
            return (ro1i * cp1 * Vi + ro2i * cp2 * Ui) / GetDeltaRadius(currentTime - tDelta, currentTime);
        }
        private double GetBcomponentFromT(double roi, double ro2i, double cp1, double cp2, double tDelta)
        {
            return (roi * cp1 + ro2i * cp2) / tDelta;
        }
        private double GetAcomponentFromT(double currentTime, double tDelta)
        {
            return (ClassConst.alpha1 + ClassConst.alpha2) / GetDeltaRadius(currentTime - tDelta, currentTime);
        }
        private double GetAcomponentFromDensity(double currentTime, double tDelta, double value, double valuePlus1, double ro)
        {
            var a1 = GetValueSecondComponent(value, currentTime);
            var a2 = (valuePlus1 - value) / GetDeltaRadius(currentTime - tDelta, currentTime);
            return ro * (a1 + a2);
        }

        private double GetFlowRate(double currentTime, double tDelta, double value, double piPlus1, double pi, double roi)
        {
            var c1 = piPlus1 - pi;
            var c2 = roi * GetDeltaRadius(currentTime - tDelta, currentTime);
            var c3 = GetValueSecondComponent(value, currentTime) * value;

            return value - ((c1 / c2) + c3) / GetValueFirstComponent(tDelta, value, currentTime - tDelta, currentTime);
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

            var c5 = GetConstWithDeltaFunc(deltaFunc, constA, currentTime) - GetValueSecondComponent(Vi, currentTime) * ro1i;
            var c6 = GetConstWithDeltaFunc(deltaFunc, constB, currentTime) - GetValueSecondComponent(Ui, currentTime) * ro2i;

            var c7 = c5 / (ClassConst.mu1 * GetValueFirstComponent(tDelta, Vi, currentTime - tDelta, currentTime));
            var c8 = c6 / (ClassConst.mu2 * GetValueFirstComponent(tDelta, Ui, currentTime - tDelta, currentTime));

            return c3 + c4 + c1 + c7 + c2 + c8;
        }
        private double GetAcomponentFromPi(double currentTime, double tDelta, double Vi, double Ui, double Tiplus1)
        {
            var up = ClassConst.universalGasConstant * Tiplus1;

            var down1 = 1 - up;
            var deltaR = GetDeltaRadius(currentTime - tDelta, currentTime);

            double calc(double mui, double value)
            {
                var valueDeltaR = GetValueFirstComponent(tDelta, value, currentTime - tDelta, currentTime);
                var a = mui * deltaR * deltaR * valueDeltaR * valueDeltaR;

                return 1 / a;
            }

            var down2 = calc(ClassConst.mu1, Vi);
            var down3 = calc(ClassConst.mu2, Ui);

            return up / (down1 * (down2 + down3));
        }
        private double GetCcomponentFromPi(double value, double currentTime, double pi, double roi, double tDelta, double mu)
        {
            var up = GetNumeratorCcomponentFromPi(value, currentTime, pi, roi, tDelta);
            var down1 = GetValueFirstComponent(tDelta, value, currentTime - tDelta, currentTime);
            var deltaR = GetDeltaRadius(currentTime - tDelta, currentTime);
            var down2 = down1 * down1 * mu * deltaR * deltaR;

            return up / down2;
        }
        //value can be Vi or Ui
        //roi can be ro1i or ro2i
        private double GetNumeratorCcomponentFromPi(double value, double currentTime, double pi, double roi, double tDelta)
        {
            return -pi + roi * GetDeltaRadius(currentTime - tDelta, currentTime) * value * GetValueSecondComponent(value, currentTime);
        }
        private double GetValueSecondComponent(double value, double currentTime)
        {
            return 2 * value / GetRadius(currentTime);
        }
        private double GetValueFirstComponent(double tDelta, double value, double time1, double time2)
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
