using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace stringify_backend.Models
{
    public partial class User
    {
        public int Id { get; set; }

        public string Nev { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public string Jelszo { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefonszam { get; set; } = null!;

        public int Jogosultsag { get; set; }

        public int Aktiv { get; set; }

        public virtual Jogok? JogosultsagNavigation { get; set; } = null!;
    }
}
