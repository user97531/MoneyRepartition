/*=============================================================================
CLASSES
=============================================================================*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

//Here are using directives we can't have on a linux machine or a website
//The gaussian part, which is the core of this class does not need them.
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PythonBackup
{
    public class GaussDistr
    {
        #region Fields
        private Sensus parent;
        private int mu;
        private float sig;
        public List<int> collection;

        //Initializing like this is the same as doing it in the constructor 
        Random rand = new Random();
        #endregion

        public GaussDistr(Sensus parent, int mu, float sig)
        {
            this.parent = parent;
            this.mu = mu;
            this.sig = sig;
            this.collection = new List<int>();
        }

        public List<double> CreateDistr(int nN)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < nN; i++)
            {
                result.Add(DrawRand());
            }
            return result;
        }

        //This is not a real gaussian distribution just an approximation
        public double DrawRand()
        {
            //Create 2 random numbers (uniform distribution) in (0;1]
            //It's the same as rand.NextDouble() but be don't want 0 (1 is accepted) because of the ln (noted Log)
            double x1 = 1 - rand.NextDouble();
            double x2 = 1 - rand.NextDouble();
            double gaussianRandom = Math.Sqrt(-2 * Math.Log(x1)) * Math.Sin(2 * Math.PI * x2);
            return mu + sig * gaussianRandom;
        }

        //This code should be declared in Form1 maybe but for sure not here
        public void PlotOld(List<int> collection)
        {
            //This code is not quite the same as the Python one. HMI are really technology specific and their implementation can
            // be different between two technologies even when they use the same language

            //Here we create windows HMI component in a class that should not contain them
            Form form = Program.mainForm;
            Chart chart = form.Controls.OfType<Chart>().First();
            Series series = chart.Series.First();

            //Every time you define a litteral string it should not be like that...
            //A better way is to declare 
            //const string blahblah = "" in Constants.cs for example
            //An even better way is to declare it in resource file so that translation would be easier
            series.Name = "Distribution of leaving old members";
            foreach (int values in collection)
            {
                series.Points.AddY(values);
            }
            series.ChartType = SeriesChartType.Line; 
            //Defines the thickness of the line
            series.BorderWidth = 3;
            //Defines the color of the line
            series.Color = Color.Blue;
            
            //Stretches the chart to the frame size in all directions. Here it's a bit tricky. 
            //Each AnchorStyles (except AnchorStyles.None) value has corresponding int value that is a power of 2
            //They form a mask, even if | is the "binary or" her we have chart.anchor ~ 15 or 1111 in binary
            chart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            chart.ChartAreas[0].AxisX.Title= "Age of members (yrs)";
            chart.ChartAreas[0].AxisY.Title = "Frequency";
        }
    }
}