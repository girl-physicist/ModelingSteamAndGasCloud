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
        public void СalculateDensityOfAblationProducts()
        {

        }
        public void СalculateDensityOfWaterVapor()
        {

        }
        public void СalculateFlowRateOfAblationProducts()
        {

        }
        public void СalculateFlowRateOfWaterVapor()
        {

        }
        
        public void СalculateTemperature()
        {

        }
        public double СalculatePressureOfSteamGasMixture(double constA, double constB, double currentTime, double tDelta, double Ui, double Vi, double ro1i, double ro2i, double pi)
        {
           return GetAcomponent(currentTime, tDelta, Vi, Ui) * GetBcomponent(constA, constB, currentTime, tDelta, Ui, Vi, ro1i, ro2i, pi);
        }
        //ToDo:  c5, c6
        private double GetBcomponent(double constA, double constB, double currentTime, double tDelta, double Ui, double Vi, double ro1i, double ro2i, double pi)
        {
            var c1 = GetCcomponent(Ui, currentTime, pi, ro2i, tDelta, ClassConst.mu2);
            var c2 = GetCcomponent(Vi, currentTime, pi, ro1i, tDelta, ClassConst.mu1);

            var c3 = ro1i / ClassConst.mu1;
            var c4 = ro2i / ClassConst.mu2;

            var c5 = 1; // A(t)delta(r-r0)
            var c6 = 1; // B(t)delta(r-r0)

            var c7 = (2 * ro1i * Vi) / (ClassConst.mu1 * GetValueDeltaR(tDelta, Vi, currentTime - tDelta, currentTime) * GetRadius(currentTime));
            var c8 = (2 * ro2i * Ui) / (ClassConst.mu2 * GetValueDeltaR(tDelta, Ui, currentTime - tDelta, currentTime) * GetRadius(currentTime));

            return c1+c3+c5-c7+c2+c4+c6-c8;
        }
        
        private double GetAcomponent(double currentTime, double tDelta, double Vi, double Ui)
        {
            var up = ClassConst.universalGasConstant * GetTi();

            var down1 = 1 - up;
            var deltaR = GetDeltaRadius(currentTime - tDelta, currentTime);

            double calc(double mui, double value)
            {
                var valueDeltaR = GetValueDeltaR(tDelta, value, currentTime - tDelta, currentTime);
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

        private double GetCcomponent(double value, double currentTime, double pi, double roi, double tDelta, double mu)
        {
            var up = GetValueToR(value, currentTime, pi, roi, tDelta);
            var down1 = GetValueDeltaR(tDelta, value, currentTime - tDelta, currentTime);
            var deltaR = GetDeltaRadius(currentTime - tDelta, currentTime);
            var down2 = down1 * down1 * mu * deltaR * deltaR;

            return up / down2;
        }


        //value can be Vi or Ui
        //roi can be ro1i or ro2i
        private double GetValueToR(double value, double currentTime, double pi, double roi, double tDelta)
        {
            return pi + roi * GetDeltaRadius(currentTime - tDelta, currentTime) * value * 2 * value / GetRadius(currentTime);
        }

        private double GetValueDeltaR(double tDelta, double value, double time1, double time2)
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

        private double GetRadius(double time)
        {
            double coefA = 1.8559;// метод подбора y=coefA * x ^ coefB (формула диаметра пузыря от времени y-диаметр x-время)
            double coefB = 0.1911;// метод подбора

            return coefA * Math.Pow(time, coefB) / 2;
        }
    }
}
