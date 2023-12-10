namespace CycleTLS.Models
{
    public class CycleRequestCookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public string Domain { get; set; }
        public string Expires { get; set; }
        public int MaxAge { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public string SameSite { get; set; }
    }
}
