namespace DataAccess
{
    public class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
