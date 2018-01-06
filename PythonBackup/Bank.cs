namespace PythonBackup
{
    public class Bank
    {
        private float[] balance;
        private Experiment parent;

        public Bank(Experiment parent)
        {
            this.parent = parent;
            balance = new float[Constants.ExpLength];
        }

        public void CashOut(float amount)
        {
            CashIn(-amount);
        }

        public void CashIn(float amount)
        {
            balance[parent.month] += amount;
        }        
    }
}