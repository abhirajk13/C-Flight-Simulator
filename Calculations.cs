using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Glider_Simulation_V1
{
    class Calculations
    {
        private double massearth = 5.972 * Math.Pow(10, 24); //acceleration downwards
        private int radiusearth = 6378100; //acceleration downwards
        private double gravconst = 6.674 * Math.Pow(10, -11); //acceleration downwards
        private float gasconstantair = 8.31447f; //calculate density
        private float absolutepressure = 101.325f; //calculate pressure
        private float lapserateoftemp = 0.0065f; //calculate pressure
        private float molarmassair = 0.0289644f; //calculate pressure
        private Userinput userinput;

        public Calculations(Userinput ui)
        {
            userinput = ui;
        }


        public float accelerationduetogravity()
        {
            float totradius = radiusearth + userinput.launchheight;
            return (float)((gravconst * massearth) / Math.Pow(totradius, 2));
        }

        public float calcpressure()
        {
            float accelerationduetograv = accelerationduetogravity();

            float pressureexponent = ((float)accelerationduetograv * molarmassair) / (gasconstantair * lapserateoftemp);

            float pressure = absolutepressure * (float)Math.Pow((1 - ((lapserateoftemp * userinput.launchheight) / userinput.temperature)), pressureexponent);
            return (float)pressure;
        }

        public float calcdensity()
        {
            float pressure = calcpressure();
            float density = pressure / (gasconstantair * userinput.temperature);
            return (float)density;
        }

        public float getRealDragFactor()
        {
            float density = calcdensity();
            float dragForceLocal = (float)0.5 * (float)density * (float)Math.Pow(userinput.initialvelocity, 2) * (float)userinput.dragcoef * (float)userinput.crosssectionalarea;

            

            return (dragForceLocal / 20000000);
            // glider will fall with g
            // glider will decelerate with drag force
        }

        public float getRealWeightFactor()
        {

            float weight = (userinput.glidermass + userinput.fuel) * 9.81f;

            return (weight / 125000);
        }

        public float getThrustFactor()
        {
            float thrustaccel = userinput.thrust / (userinput.glidermass + userinput.fuel);
            return (thrustaccel*1.25f);



        }

        public float getRealAccelerationFactor()
        {
            float density = calcdensity();
            float dragForceLocal = (float)0.5 * (float)density * (float)Math.Pow(userinput.initialvelocity, 2) * (float)userinput.dragcoef * (float)userinput.crosssectionalarea;

            float deaccelxaxis = dragForceLocal / (userinput.glidermass);
            return (deaccelxaxis / 30000);
        }


        

    }
}

