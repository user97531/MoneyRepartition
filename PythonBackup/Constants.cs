// =============================================================================
// MONEYWISE
// =============================================================================
namespace PythonBackup
{
    public static class Constants
    {
        /*These paramters should not be declared as constants, as it would be nice to run several experiments
		with differents values for these without having to compile the program again.
		They shouldn't even be declared as static to be able able to run several experiment at the
		same time with different parameters.
		In my opinion, they should be parameters specific to an instance of an experiment*/

        //Is equivalent to
        //public static int StartCurrency = 100;
        //but has little better performance that can be changed during execution
        //and even very little better performance than
        //public static readonly int StartCurrency = 100;
        //that also can't be be changed during execution, because the value is assigned during compilation
        public const int StartCurrency = 100;

        /* =============================================================================
		MEMBER NUMBERS
		=============================================================================*/
        public const int NStart = 20;
        public const int SteadyMembers = 500;
        //The f after 1.1 means float as litterals cannot always be deducted as the expected one. 1.1 without the f would be equivalent to 1.1d which is a 'double' float (64 bits) instead of 32 
        //1.1m would have meant 'decimal' which is floating type specifically designed for base 10
        public const float MaxMembers = 1.1f * SteadyMembers;
        public const float MinMembers = 0.9f * SteadyMembers;
        public const float NewProp = 0.005f; //dictates the growth
        public const float LeavingProp = 0.004f; //dictates the decline

        /*=============================================================================
		TIMEWISE
		=============================================================================*/
        //As 1 year has not a fixed length we use days. The Timespan class allow to store durations not specific to any unit. You can manipulate them more easily than values stored in different units.
        //Like adding/substracting (negative values allowed) several of them, multiplying them by a number or adding them to a date to get another date.
        public static readonly int LifeExpectancy = 80; //average life expectancy in year
        public static readonly float MaxLongevity = 1.5f * LifeExpectancy; //max longevity of a human
        public static readonly float Sigma = 5; //deviation around the average life expectancy in years
        public static readonly int ExpLength = 3 * LifeExpectancy * 12; //experiment length in month
    }
}