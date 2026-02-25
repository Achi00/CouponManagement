namespace CouponApp.Domain.Entity
{
    public class ErrorLog
    {
        public Guid Id { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public string? Message { get; set; }

        public string? Exception { get; set; }

        public string? StackTrace { get; set; }

        public string? Path { get; set; }

        public string? Method { get; set; }

        public string? UserId { get; set; }

        public string? IpAddress { get; set; }

        public int? StatusCode { get; set; }
    }
}
