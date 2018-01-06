/*=============================================================================
CLASSES
=============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace PythonBackup
{
    public class Sensus
    {
        #region Fields
        public List<Member> community;
        private int n;
        private Experiment parent;
        private Bank bank;
        //private float nn;
        public GaussDistr gdistr;
        public List<int> oldCollection;
        #endregion

        public Sensus(Experiment parent)
        {
            community = new List<Member>();
            n = 0;
            this.parent = parent;
            bank = parent.bank;
            //nn = (float)Constants.ExpLength / (Constants.LifeExpectancy * 12);
            gdistr = new GaussDistr(this, Constants.LifeExpectancy, Constants.Sigma);
            oldCollection = new List<int>();
            Console.WriteLine("Sensus instance created");
        }
    
        private int GetExp_Month()
        {
            return parent.month;
        }

        //Create member
        public void CreateNewMember()
        {
            if(NumberOfMembers() < Constants.MaxMembers)
            {
                Member member = new Member();
                community.Add(member);
                ProvideToNewMember(member);
            }
        }

        private void ProvideToNewMember(Member member)
        {
            int month = GetExp_Month();
            Console.WriteLine(month);
            float amount = 0;
            if (month>0)
            {
                amount = Constants.StartCurrency;
            }
            else if(month == 0)
            {
                amount = parent.GetCurrDiv_m();
            }
            member.StartBalance(amount);
            GrabMoneyFromBank(amount);
        }
        
        private void GrabMoneyFromBank(float amount)
        {
            bank.CashOut(amount);
        }

        //Delete member
        //find out how many old are leaving
        private List<Member> OldLeaving(int going)
        {
            List<Member> oldGoing = new List<Member>();
            List<int> age = new List<int>();
            for(int i =0; i < going; i++)
            {
                age.Add((int)Math.Round(gdistr.DrawRand() * 12));
            }
            foreach(Member memb in community)
            {
                if(memb.age > Constants.MaxLongevity)
                {
                    oldGoing.Add(memb);
                }
                if (age.Count != 0)
                {
                    //Min() method is a linq (see using directories on top of the file) that can be applied to collections which generic type (int here) is comparable
                    if(memb.age > age.Min())
                    {
                        oldGoing.Add(memb);
                        age.Remove(age.Min());
                    }
                }
            }
            return oldGoing;
        }

        //delete them
        public void DeleteLeaving(int going)
        {
            //old members
            List<Member> oldGoing = OldLeaving(going);
            foreach(Member memb in oldGoing)
            {
                oldCollection.Add((int)Math.Round((float)memb.age / 12));
                DeleteMember(memb);
            }
            //other leaving members
            while(going > oldGoing.Count)
            {
                //What does this mean
                //random.choice(self.community)
                //Get a random element ? In this case, the member is not removed. Is that really the expected behaviour ? Why

                Member m = community[(new Random()).Next(0, community.Count - 1)];
                going -= 1;
            }
        }

        //actually delete member
        private void DeleteMember(Member member)
        {
            SendMoneyToBank(member);
            if (community.Contains(member))
            {
                community.Remove(member);
            }
        }

        //send member's money to bank
        private void SendMoneyToBank(Member member)
        {
            int month = member.month;
            float amount = member.balance[month];
            bank.CashIn(amount);
        }

        //Others
        public int NumberOfMembers()
        {
            //Why storing as a class field the number of members as it's calculted each time needed already ?
            //return community.Count;
            n = community.Count;
            return n;
        }
    }
}