namespace raccoonLog.IntegrationTests
{
    public class Address
    {
        public Address(string street, int floor, string alley, int zoneCode)
        {
            Street = street;
            Floor = floor;
            Alley = alley;
            ZoneCode = zoneCode;
        }

        public string Street { get; set; }

        public int Floor { get; set; }

        public string Alley { get; set; }

        public int ZoneCode { get; set; }
    }
}