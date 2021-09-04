using System;

namespace HubUfpr.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float NoteApp { get; set; }
        public string Email { get; set; }
        public string GRR { get; set; }
        public DateTime LastLogon { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ActivationCode { get; set; }
        public string Token { get; set; }
        public bool IsVendedor { get; set; }
    }
}