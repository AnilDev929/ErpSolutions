namespace ERP_SOLUTIONS.Helpers
{
    public static class DateHelper
    {
        public static int CalculateAge(DateTime dob)
        {
            return DateTime.Now.Year - dob.Year;
        }
    }
}
