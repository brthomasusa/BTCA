using System;
using System.Collections.Generic;
using BTCA.Common.BusinessObjects;
using BTCA.Common.Entities;
using BTCA.DataAccess.EF;

namespace BTCA.Tests 
{
    public static class HOSTestData
    {
        public static void LoadDutyStatusTable(HOSContext ctx)
        {
            var data = new List<DutyStatus>
            {
                new DutyStatus {DutyStatusID = 1, ShortName = "Off Duty", LongName = "Off Duty", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatus {DutyStatusID = 2, ShortName = "Sleeper Berth", LongName = "Sleeper Berth", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatus {DutyStatusID = 3, ShortName = "Driving", LongName = "On Duty Driving", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatus {DutyStatusID = 4, ShortName = "On Duty", LongName = "On Duty Not Driving", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            ctx.DutyStatuses.AddRange(data);
            ctx.SaveChanges();            
        }

        public static void LoadDutyStatusActivityTable(HOSContext ctx)
        {
            var data = new List<DutyStatusActivity>
            {
                new DutyStatusActivity {DutyStatusActivityID = 1, Activity = "Pre-Trip", Description = "Pre-trip inspection", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 2, Activity = "Post-Trip", Description = "Post-trip inspection", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 3, Activity = "Loading", Description = "Arrive shipper/loading", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 4, Activity = "Unloading", Description = "Arrive receiver/unloading", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 5, Activity = "D.O.T.", Description = "D.O.T. inspection", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 6, Activity = "Maint.", Description = "Vehicle maintenance", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 7, Activity = "Fueling", Description = "Vehicle fueling", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 8, Activity = "Misc", Description = "Unspecified on duty activity", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 9, Activity = "Off Duty", Description = "Off Duty", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 10, Activity = "Sleeper", Description = "Sleeper berth", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 11, Activity = "Driving", Description = "Driving", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            ctx.DutyStatusActivities.AddRange(data);
            ctx.SaveChanges();
        }

        public static void LoadCompanyTable(HOSContext ctx)
        {
            var data = new List<Company> 
            {
                new Company {ID = 1, CompanyCode = "ADMIN001", CompanyName = "Btechnical Consulting", DOT_Number = "000000", MC_Number = "MC-000000", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 2, CompanyCode = "FCT001", CompanyName = "First Choice Transport", DOT_Number = "123456", MC_Number = "MC-123456", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 3, CompanyCode = "SWIFT001", CompanyName = "Swift Transportation", DOT_Number = "937712", MC_Number = "MC-987665", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 4, CompanyCode = "SWIFT004", CompanyName = "Swift Trans LLC", DOT_Number = "712025", MC_Number = "MC-987665", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 5, CompanyCode = "GWTM001", CompanyName = "GreatWIDe Truckload Management", DOT_Number = "430147", MC_Number = "MC-014987", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 6, CompanyCode = "CARD001", CompanyName = "Cardinal Logistics", DOT_Number = "703028", MC_Number = "MC-654321", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},                                
                new Company {ID = 7, CompanyCode = "GWLS001", CompanyName = "Greatwide Logistics Services", DOT_Number = "380085", MC_Number = "MC-665871", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            ctx.Companies.AddRange(data);
            ctx.SaveChanges();
        }

        public static void LoadStateProvinceCodeTable(HOSContext ctx)
        {
            var data = new List<StateProvinceCode> 
            { 
                new StateProvinceCode {ID = 1,StateCode = "AK", StateName = "Alaska",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 2,StateCode = "AL", StateName = "Alabama",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 3,StateCode = "AR", StateName = "Arkansas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 4,StateCode = "AZ", StateName = "Arizona",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 5,StateCode = "CA", StateName = "California",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 6,StateCode = "CO", StateName = "Colorado",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 7,StateCode = "CT", StateName = "Connecticut",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 8,StateCode = "DC", StateName = "District Of Columbia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 9,StateCode = "DE", StateName = "Delaware",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 10,StateCode = "FL", StateName = "FlorIDa",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 11,StateCode = "GA", StateName = "Georgia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 12,StateCode = "HI", StateName = "Hawaii",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 13,StateCode = "IA", StateName = "Iowa",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 14,StateCode = "ID", StateName = "IDaho",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 15,StateCode = "IL", StateName = "Illinois",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 16,StateCode = "IN", StateName = "Indiana",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 17,StateCode = "KS", StateName = "Kansas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 18,StateCode = "KY", StateName = "Kentucky",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 19,StateCode = "LA", StateName = "Louisiana",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 20,StateCode = "MA", StateName = "Massachusetts",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 21,StateCode = "ME", StateName = "Maine",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 22,StateCode = "MD", StateName = "Maryland",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 23,StateCode = "MI", StateName = "Michigan",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 24,StateCode = "MN", StateName = "Minnesota",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 25,StateCode = "MO", StateName = "Missouri",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 26,StateCode = "MS", StateName = "Mississippi",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 27,StateCode = "MT", StateName = "Montana",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 28,StateCode = "NC", StateName = "North Carolina",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 29,StateCode = "ND", StateName = "North Dakota",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 30,StateCode = "NE", StateName = "Nebraska",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 31,StateCode = "NH", StateName = "New Hampshire",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 32,StateCode = "NJ", StateName = "New Jersey",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 33,StateCode = "NM", StateName = "New Mexico",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 34,StateCode = "NV", StateName = "Nevada",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 35,StateCode = "NY", StateName = "New York",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 36,StateCode = "OH", StateName = "Ohio",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 37,StateCode = "OK", StateName = "Oklahoma",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 38,StateCode = "OR", StateName = "Oregon",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 39,StateCode = "PA", StateName = "Pennsylvania",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 40,StateCode = "PR", StateName = "Puerto Rico",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 41,StateCode = "RI", StateName = "Rhode Island",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 42,StateCode = "SC", StateName = "South Carolina",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 43,StateCode = "SD", StateName = "South Dakota",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 44,StateCode = "TN", StateName = "Tennessee",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 45,StateCode = "TX", StateName = "Texas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 46,StateCode = "UT", StateName = "Utah",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 47,StateCode = "VT", StateName = "Vermont",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 48,StateCode = "VA", StateName = "Virginia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 49,StateCode = "WA", StateName = "Washington",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 50,StateCode = "WI", StateName = "Wisconsin",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 51,StateCode = "WV", StateName = "West Virginia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 52,StateCode = "WY", StateName = "Wyoming",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 53,StateCode = "AB", StateName = "Alberta", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 54,StateCode = "BC", StateName = "British Columbia", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 55,StateCode = "MB", StateName = "Manitoba", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 56,StateCode = "NB", StateName = "New Brunswick", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 57,StateCode = "NL", StateName = "Newfoundland and Labrador", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 58,StateCode = "NT", StateName = "Northwest Territories", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 59,StateCode = "NS", StateName = "Nova Zcotia", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 60,StateCode = "NU", StateName = "Nunavut", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 61,StateCode = "ON", StateName = "Ontario", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 62,StateCode = "PE", StateName = "Prince Edward Island", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 63,StateCode = "QC", StateName = "Québec", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 64,StateCode = "SK", StateName = "Saskatchewan", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 65,StateCode = "YT", StateName = "Yukon", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
            };

            ctx.StateProvinceCodes.AddRange(data);       
            ctx.SaveChanges();
        }           


        public static void LoadCompanyAddresses(HOSContext ctx)
        {
            var data = new List<Address>()
            {
                new Address() 
                { 
                    ID = 1,
                    AddressLine1 = "1346 Markum Ranch Rd",
                    AddressLine2 = "Ste 100",
                    City = "Fort Worth",
                    StateProvinceId = 45,
                    Zipcode = "76126",
                    IsHQ = true,
                    CompanyId = 2,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                    
                },
                
                new Address() 
                { 
                    ID = 2,
                    AddressLine1 = "6591 Brighton Blvd",
                    City = "Commerce City",
                    StateProvinceId = 6,
                    Zipcode = "80022",
                    IsHQ = false,
                    CompanyId = 2,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },   
                new Address() 
                { 
                    ID = 3,
                    AddressLine1 = "12404 Park Central D",
                    AddressLine2 = "Ste 300",
                    City = "Dallas",
                    StateProvinceId = 45,
                    Zipcode = "75251",
                    IsHQ = true,
                    CompanyId = 7,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },
                new Address() 
                { 
                    ID = 4,
                    AddressLine1 = "2150 Cabot Boulevard West",
                    City = "Langhorne",
                    StateProvinceId = 39,
                    Zipcode = "19047",
                    IsHQ = true,
                    CompanyId = 5,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                }, 
                new Address() 
                { 
                    ID = 5,
                    AddressLine1 = "3250 N Longhorn Dr",
                    City = "Lancaster",
                    StateProvinceId = 45,
                    Zipcode = "75134",
                    IsHQ = false,
                    CompanyId = 3,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },
                new Address() 
                { 
                    ID = 6,
                    AddressLine1 = "22 South 75th Street",
                    City = "Phoenix",
                    StateProvinceId = 4,
                    Zipcode = "85043",
                    IsHQ = true,
                    CompanyId = 3,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                }, 
                                                          
            };

            ctx.AddRange(data);
            ctx.SaveChanges();
        }        
    }    
}