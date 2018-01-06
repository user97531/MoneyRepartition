using System;
using System.Collections.Generic;

namespace PythonBackup
{
    /*=============================================================================
	Class
	=============================================================================*/
    public class Experiment
    {
        /* to be done :
		* melting of money
		* track members :
		    -inactive (accumulating only)
		    -very active selling
		    -very active buying

		    -customised
		population percentage behaving in a specific way*/

        //Fields, methods, properties can be written in any order in a class resulting in the same compiled code
        //But it's easier to read when they are grouped by type, (non-)static etc

        //Regions have absolutely no impact on compiled code, it just helps the human reader
        //to see how the code is organized. Regions can be defined inside regions and be (un-)folded
        #region Fields
        public int month; //The current experiment month
        private string lawName;
        /*The law used to calculate the dividend 
        1- feedback
        2- Exponentially constant */

        private int curr_div;//The current dividend of the experiment month
        //A List needs a using directive (see first line of this file) but it's REALLY useful
        private List<int> dividend;//The register of the monthly dividends
        private float[] div_c;//calculated at the end of the month
        private float[] div_m;//calculated at the end of the month

        public Bank bank;
        private Sensus sensus;
        #endregion

        //A constructor of the experiment class
        public Experiment(string lawName)
        {
            //This line is really ugly as we named the method parameter the same as the class field. But it's well interpreted by the compiler
            //Having a different name for the parameter would be the best
            this.lawName = lawName;//
            month = 0;
            dividend = new List<int>();//We initialize the list. As it's a non basic object it's value is null when not initialized, leading to exception when manipulated (Add, Count etc). Now it has 0 elements
            div_c = new float[Constants.ExpLength];//The table is initialized with Constants.ExpLength cells containing 0
            div_m = new float[Constants.ExpLength];
            bank = new Bank(this);//create the emitting bank
            sensus = new Sensus(this);//create a community of members
        }

        #region Methods
        //Commentaries with 3 slashes are used for commentaries on fields, class, etc and are interpreted
        //by development tool, that way you can generate code documentation automatically.
        //When you hover the move over a method Visual studio displays the documentation for quick information
        //By triple slashing before a field, class, etc the documentation template is automacally generating including parameters, generic types etc

        ///<summury>
        ///Loop over the months of the experiments 
        ///</summury>		
        public void RunExp()
        {
            int n = Constants.NStart;
            for (int i = 0; i < n; i++)
            {
                sensus.CreateNewMember();
            }
            //We use Console.WiteLine to write in the console instead of Console.Write so that the next time we write it will be on a new line
            //We don't have to cast n to string as, the + plus sign when associated with string automatically calls the object.ToString method for non string objects
            //Console.WriteLine(text) is equivalent to Console.Write(text + "\r\n") or the prettier Console.Write(text + Environment.NewLine) that works for linux too
            //As it writs in the console, it can only be seen if it is called by a console application...
            //If you want to write in the development tool output use Debug instead of Console
            Console.WriteLine("0) There are " + n + " members (month 0)");
            //When writing text with parameters inside, it easier to use
            //Console.WriteLine(string.Format("0) There are {0} members (month 0)", n));

            for (int i = 0; i < Constants.ExpLength; i++)
            {
                month = i;
                n = sensus.NumberOfMembers();
                //If we had used string.Format well, this string would have been defined just once
                //Console.WriteLine(string.Format("{0}) There are {1} members (month {0})", i, n));
                Console.WriteLine("1) There are " + n + " members (month " + month + ")");

                //UpdateNewMonth();Removed because useless with table int[], useful with List<int>
                if (month > 0)
                {
                    //useless
                    //bank.UpdateNewMonth();
                    foreach (Member member in sensus.community)
                    {
                        member.UpdateNewMonth();
                    }
                }

                //We create a random number generator. It's not a good idea to instantiate it here as the same instance could be used for all random integers generation of the program. This would be a nice member of the Constants class...
                Random random = new Random();

                //When regions are nested inside a method, it usually means that it should have been split in several methods
                //The it's here, the split would be really easy as only n is shared
                #region Member flow
                //We don't use new as variable name as it's a C# keyword to call a constructor
                //Note that suprisingly Math.Round() returns a double (float) instead of an expected integer (or long integer) hence its cast to int
                int memberFlow = random.Next(0, (int)Math.Round(n * Constants.NewProp) + 2); // +1 is needed in case of N being really small
                                                                                             //ToString method is available on all object and works well for simple types. For complex types it return the type name of the object (which is usually not what's expected). However it can be overridden to define exactly what's displayed
                Console.WriteLine(memberFlow.ToString());

                if (n + memberFlow > Constants.MaxMembers)
                {
                    memberFlow = (int)Math.Round(Constants.MaxMembers) - n;
                }
                while (memberFlow > 0)
                {
                    sensus.CreateNewMember();
                    //memberFolw--; is more usual
                    memberFlow -= 1;
                }

                int going = random.Next(0, (int)Math.Round(n * Constants.LeavingProp) + 1);
                if (going != 0)
                {
                    sensus.DeleteLeaving(going);
                }
                n = sensus.NumberOfMembers();
                Console.WriteLine(" There are " + n + " members (month " + month + ")");
                #endregion

                #region Transactions
                #endregion

                #region End of month
                int dividend_c = 0; //test
                                    //grab it from the bank
                bank.CashOut(dividend_c);

                //float dividend_m = dividend_c/n; would have been a classic mistake. 
                //In C# when a division is between integers it returns euclidean quotient (~) which is an integer; wheras a%b return the remainder
                //To have the floating division we have to define one (at least) of the parameters as non integer (float, double, decimal, ...)
                //To do that we do a cast (changing the type of something) here (float)dividend_c means create a float that has the "same" value as dividend_c.
                //Casts for numbers are well implemented, but for complex object you have to explicitly code them
                float dividend_m = ((float)dividend_c) / n; //calculate individual dividend

                //give it to each member
                foreach (Member member in sensus.community)
                {
                    member.CashIn(dividend_m);
                }
                //record dividend
                div_c[month] = dividend_c;
                div_m[month] = dividend_m;
                #endregion
            }
            if (sensus.oldCollection != null)
            {
                sensus.gdistr.collection = sensus.oldCollection;
                sensus.gdistr.PlotOld(sensus.oldCollection);
            }
        }

        public float GetCurrDiv_m()
        {
            //No need to check for nullity as div_m table as been initialized and the default value of a number is 0
            return div_m[month];
        }
        //Maybe a readonly property would be nicer
        /*public int CurrDiv_m
		{
			get
			{
				return div_m[month]; 
			}
		}*/

        //This method has been removed as the table has been initialized. It would be useful if div_m had be typed as List<int>
        /*private UpdateNewMonth()
		{
			...
		}
		*/

        //This method is only called if the current project is a NOT a class library 
        //As some operations are drawing plots, we need the program to have an HMI,
        //A good idea would be to transfer inner code to the method called when the button is clicked...
        /*=============================================================================
		MAIN    
		=============================================================================*/
        //this code has been moved to Program.cs
        /*static void Main()
		{
			
		}*/
        #endregion
    }
}