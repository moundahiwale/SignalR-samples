namespace SignalR.Helper
{
    public partial class OrderChecker
    {
        int index;

        readonly string[] Status = {"Pizza has been Ordered", "Preparing your pizza", 
            "Baking", "Quality Check", "Out for Delivery", "Delivered"};

        public CheckResult GetUpdate(int orderNo)
        {
            //In a real world/PROD scenario, the status would be fetched from a DB, etc.
            if (index < Status.Length)
            {
                var result = new CheckResult
                {
                    New = true,
                    Update = Status[index],
                    Finished = Status.Length - 1 == index
                };
                index++;
                return result;
            }

            return new CheckResult { New = false };
        }
    }
}