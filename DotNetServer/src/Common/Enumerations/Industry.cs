namespace Common.Enumerations
{
    public class Industry : Enumeration<Industry>
    {
        public Industry(string value, string displayName, int displayOrder)
            : base(value, displayName, displayOrder)
        {
        }

        public static readonly Industry Agriculture = new Industry("AG", "Agriculture", 0);
        public static readonly Industry Accounting = new Industry("AC", "Accounting", 0);
        public static readonly Industry Advertising = new Industry("AD", "Advertising", 0);
        public static readonly Industry Aerospace = new Industry("AE", "Aerospace", 0);
        public static readonly Industry Aircraft = new Industry("AI", "Aircraft", 0);
        public static readonly Industry Airline = new Industry("AR", "Airline", 0);
        public static readonly Industry ApparelAccessories = new Industry("AA", "Apparel & Accessories", 0);
        public static readonly Industry Automotive = new Industry("AU", "Automotive", 0);
        public static readonly Industry Banking = new Industry("BA", "Banking", 0);
        public static readonly Industry BeverageTobacco = new Industry("BT", "Beverage & Tobacco", 0);
        public static readonly Industry Broadcasting = new Industry("BR", "Broadcasting", 0);
        public static readonly Industry Brokerage = new Industry("BO", "Brokerage", 0);
        public static readonly Industry Biotechnology = new Industry("BI", "Biotechnology", 0);
        public static readonly Industry CallCentre = new Industry("CC", "Call Centre", 0);
        public static readonly Industry CargoHandling = new Industry("CH", "Cargo Handling", 0);
        public static readonly Industry Chemical = new Industry("CE", "Chemical", 0);
        public static readonly Industry Computer = new Industry("CO", "Computer", 0);
        public static readonly Industry Consulting = new Industry("CN", "Consulting", 0);
        public static readonly Industry ConsumerProducts = new Industry("CP", "Consumer Products", 0);
        public static readonly Industry Cosmetics = new Industry("CS", "Cosmetics", 0);
        public static readonly Industry Defence = new Industry("DF", "Defence", 0);
        public static readonly Industry DepartmentStores = new Industry("DS", "Department Stores", 0);
        public static readonly Industry Education = new Industry("ED", "Education", 0);
        public static readonly Industry Electronics = new Industry("EL", "Electronics", 0);
        public static readonly Industry Energy = new Industry("EN", "Energy", 0);
        public static readonly Industry EntertainmentLeisure = new Industry("EL", "Entertainment & Leisure", 0);
        public static readonly Industry ExecutiveSearch = new Industry("ES", "Executive Search", 0);
        public static readonly Industry FinancialServices = new Industry("FS", "Financial Services", 0);
        public static readonly Industry Food = new Industry("FO", "Food", 0);
        public static readonly Industry Grocery = new Industry("GR", "Grocery", 0);
        public static readonly Industry HealthCare = new Industry("HC", "Health Care", 0);
        public static readonly Industry InternetPublishing = new Industry("IP", "Internet Publishing", 0);
        public static readonly Industry InvestmentBanking = new Industry("IB", "Investment Banking", 0);
        public static readonly Industry Legal = new Industry("LE", "Legal", 0);
        public static readonly Industry Manufacturing = new Industry("MA", "Manufacturing", 0);
        public static readonly Industry MotionPictureVideo = new Industry("MO", "Motion Picture & Video", 0);
        public static readonly Industry Music = new Industry("MU", "Music", 0);
        public static readonly Industry NewspaperPublishers = new Industry("NP", "Newspaper Publishers", 0);
        public static readonly Industry OnLineAuctions = new Industry("OL", "On-line Auctions", 0);
        public static readonly Industry PensionFunds = new Industry("PF", "Pension Funds", 0);
        public static readonly Industry Pharmaceuticals = new Industry("PH", "Pharmaceuticals", 0);
        public static readonly Industry PrivateEquity = new Industry("PE", "Private Equity", 0);
        public static readonly Industry Publishing = new Industry("PU", "Publishing", 0);
        public static readonly Industry RealEstate = new Industry("RE", "Real Estate", 0);
        public static readonly Industry RetailWholesale = new Industry("RW", "Retail & Wholesale", 0);
        public static readonly Industry SecuritiesCommodityExchanges = new Industry("SC", "Securities & Commodity Exchanges", 0);
        public static readonly Industry Service = new Industry("SE", "Service", 0);
        public static readonly Industry SoapDetergen = new Industry("SD", "Soap & Detergent", 0);
        public static readonly Industry Software = new Industry("SO", "Software", 0);
        public static readonly Industry Sports = new Industry("SP", "Sports", 0);
        public static readonly Industry Technology = new Industry("TE", "Technology", 0);
        public static readonly Industry Telecommunications = new Industry("TL", "Telecommunications", 0);
        public static readonly Industry Television = new Industry("TV", "Television", 0);
        public static readonly Industry Transportation = new Industry("TR", "Transportation", 0);
        public static readonly Industry Trucking = new Industry("TU", "Trucking", 0);
        public static readonly Industry VentureCapital = new Industry("VC", "Venture Capital",0);
    }
}