using System.Collections.Generic;
using System.Drawing;
using System.Linq;
//Here are using directives we can't have on a linux machine or a website
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PythonBackup
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void UpdateChart(List<int> collection)
        {
            //This code is not quite the same as the Python one. HMI are really technology specific and their implementation can
            // be different between two technologies even when they use the same language

            Series series = chart1.Series.First();

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
            chart1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            chart1.ChartAreas[0].AxisX.Title = "Age of members (yrs)";
            chart1.ChartAreas[0].AxisY.Title = "Frequency";
        }
    }
}