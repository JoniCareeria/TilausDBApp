namespace TilausDBApp.ViewModels
{
    using System;
    using TilausDBApp.Models;
    using System.Drawing;

    public class OrderSummaryData
    {
        public string Nimi { get; set; }
        public Nullable<decimal> Ahinta { get; set; }
        public byte[] Kuva { get; set; }
        public string ImageLink { get; set; }
        public int TilausriviID { get; set; }
        public Nullable<int> TilausID { get; set; }
        public Nullable<int> TuoteID { get; set; }
        public Nullable<int> Maara { get; set; }
        public virtual Tilaukset Tilaukset { get; set; }
        public virtual Tuotteet Tuotteet { get; set; }
        public Nullable<int> AsiakasID { get; set; }
        public string Toimitusosoite { get; set; }
        public string Postinumero { get; set; }
        public Nullable<System.DateTime> Tilauspvm { get; set; }
        public Nullable<System.DateTime> Toimituspvm { get; set; }
        public string Postitoimipaikka { get; set; }
        public string WeekdayName { get; set; }
    }
}