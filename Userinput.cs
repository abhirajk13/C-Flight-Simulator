using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Glider_Simulation_V1
{
    public class Userinput
    {
        public float glidermass;// = 200; 
        public float dragcoef; //= 0.75f;
        public float launchheight;//= 100;
        public float temperature; // = 288.15f; //in kelvins
        public float initialvelocity; //= 50;
        public float crosssectionalarea;//= 6400; //depending on the type of glider // the plane falling slower when csa is lower.
        public float thrust;
        public float fuel;
        

    }
}
