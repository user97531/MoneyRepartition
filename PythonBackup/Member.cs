using System;
using PythonBackup.Settings;

namespace PythonBackup
{
	/*=============================================================================
	CLASSES
	=============================================================================*/
	public class Member
	{
		#region Fields
		private bool endOfMonth;
		private bool active;
		public int month;
		public int age;
		public float[] balance;
		#endregion
		
		public Member(/*int startCurr, int lastId*/)
		{
			this.balance = new float[Constants.ExpLength];
			month = 0;
			age = (new Random()).Next(0, Constants.LifeExpectancy * 12);//age of the new member when they joined, in month
			endOfMonth = false;
			active = true;
		}
		
		#region Methods
		
		//useless
		public void StartBalance(float amount)
		{
			if(month == 0)
			{
				balance[month] = amount;
			}
			else
			{
				//Would be better to throw an Exception here instead ? As the problem comes from the developer 
				//throw new Exception("tried to provide Start up money after first month !");
				Console.WriteLine("tried to provide Start up money after first month !");
			}
		}
		
		public void CashIn(float amount)
		{
			balance[month] += amount;			
		}
		
		public void CashOut(float amount)
		{
			balance[month] -= amount;			
		}
		
		public void UpdateNewMonth()
		{
            if(month > 0 /*&& month < balance.Length*/)
            {
                balance[month] = balance[month - 1];
                age += 1;
                month += 1;
            }
		}
		
		public float GetCurrBal()
		{
			return balance[month];
		}
		
		public void Deactivate()
		{
			active = false;
		}
		#endregion
	}
}