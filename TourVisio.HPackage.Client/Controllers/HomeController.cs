using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using TourVisio.Hotel.Client.Models;
using TourVisio.HPackage.Client.Models;
using TourVisio.WebService.Adapter.Enums;
using TourVisio.WebService.Adapter.Models;
using TourVisio.WebService.Adapter.Models.Booking;
using TourVisio.WebService.Adapter.ServiceModels.BookingModels;
using TourVisio.WebService.Adapter.ServiceModels.LookupModels;
using TourVisio.WebService.Adapter.ServiceModels.ProductModels;
using TourVisio.WebService.Client;

namespace TourVisio.HPackage.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

          List<enmProductType> RoomingSupportedProductTypes = new List<enmProductType>() { enmProductType.Hotel, enmProductType.Cruise };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            if (User != null && User.Identity.IsAuthenticated)
            {
                var pRequest = new GetCurrenciesRequest() { SearchType = enmCurrencySearchType.ForSearch };
                

                LookupRepository lookUp = new LookupRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                
                var rResponse = lookUp.GetCurrencies(pRequest);
                


                IndexViewModel c1 = new IndexViewModel();
                if (rResponse.Header.Success)
                {
                    c1.Currencies = rResponse.Body.Currencies;
                    
                }

                var pRequest2 = new GetDeparturesRequest(){ ProductType = enmProductType.HolidayPackage} ;

                ProductRepository pRepo = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rResponse2 = pRepo.GetDepartures(pRequest2);
                

                if (rResponse2.Header.Success)
                {
                    var locations = rResponse2.Body.Locations;
                    var countries = locations.GroupBy(l => l.CountryId);
                    var departures = new List<mdlLocation>();
                    foreach (var country in countries)
                    {
                        departures.AddRange(country.OrderBy(l => l.Type).ToList());
                    }
                    c1.Departures = departures;
                }

               


                var pRequest3 = new GetArrivalsRequest() { ProductType = enmProductType.HolidayPackage, DepartureLocations = rResponse2.Body.Locations.ToList()};
     
                ProductRepository pRepop = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rReponse3 = pRepop.GetArrivals(pRequest3);

                if (rReponse3.Header.Success)
                {
                    var locations = rReponse3.Body.Locations;
                    var countries = locations.GroupBy(l => l.CountryId);
                    var arrivals = new List<mdlLocation>();
                    foreach (var country in countries)
                    {
                        arrivals.AddRange(country.OrderBy(l => l.Type).ToList());
                    }
                    c1.Arrivals = arrivals;
                    
                }


                var pRequest4 = new GetCheckInDatesRequest()  { ProductType = enmProductType.HolidayPackage, DepartureLocations = rResponse2.Body.Locations.ToList(), ArrivalLocations = rReponse3.Body.Locations.ToList(), IncludeSubLocations = true };

                ProductRepository prepop2 = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rResponse4 = prepop2.GetCheckInDates(pRequest4);

                if (rResponse4.Header.Success)
                {
                    c1.CheckInDates = rResponse4.Body.Dates;
                }


                return View(c1);
            }

            return View();
        }
        
        public IActionResult GetArrivals(string departureLocationId, int departureLocationType)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                var departureLocation = new mdlLocation();
                departureLocation.Id = departureLocationId;
                departureLocation.Type = (enmLocationType)departureLocationType;
                var departureLocations = new List<mdlLocation>();
                departureLocations.Add(departureLocation);

                var pRequest3 = new GetArrivalsRequest() { ProductType = enmProductType.HolidayPackage, DepartureLocations = departureLocations };

                ProductRepository pRepop = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rReponse3 = pRepop.GetArrivals(pRequest3);

                if (rReponse3.Header.Success)
                {
                    var locations = rReponse3.Body.Locations;
                    var countries = locations.GroupBy(l => l.CountryId);
                    var arrivals = new List<mdlLocation>();
                    foreach (var country in countries)
                    {
                        arrivals.AddRange(country.OrderBy(l => l.Type).ToList());
                    }
                    //c1.Arrivals = arrivals;


                    return Json(arrivals);
                }
                else
                {
                    return Json("");
                }
            }
            else
            {
                return Json("");
            }

        } 
       

        public IActionResult GetCheckInDates(string arrivalLocationId, int arrivalLocationType, string departureLocationId, int departureLocationType)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                var arrivalLocation = new mdlLocation();
                arrivalLocation.Id = arrivalLocationId;
                arrivalLocation.Type = (enmLocationType)arrivalLocationType;
                var arrivallocations = new List<mdlLocation>();
                arrivallocations.Add(arrivalLocation);

                var departureLocation = new mdlLocation();
                departureLocation.Id = departureLocationId;
                departureLocation.Type = (enmLocationType)departureLocationType;
                var departureLocations = new List<mdlLocation>();
                departureLocations.Add(departureLocation);

                var pRequest4 = new GetCheckInDatesRequest() { ProductType = enmProductType.HolidayPackage, ArrivalLocations = arrivallocations, DepartureLocations = departureLocations, IncludeSubLocations = true };

                ProductRepository pRepop = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rReponse3 = pRepop.GetCheckInDates(pRequest4);

                if (rReponse3.Header.Success)
                {
                    return Json(rReponse3.Body.Dates);
                }
                else
                {
                    return Json("");
                }

            }
                 else
                {
                    return Json("");
                }
            
        }

        public IActionResult GetNights(string arrivalLocationId, int arrivalLocationType, string departureLocationId, int departureLocationType, DateTime checkInDate)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                var arrivalLocation = new mdlLocation();
                arrivalLocation.Id = arrivalLocationId;
                arrivalLocation.Type = (enmLocationType)arrivalLocationType;
                var arrivallocations = new List<mdlLocation>();
                arrivallocations.Add(arrivalLocation);

                var departureLocation = new mdlLocation();
                departureLocation.Id = departureLocationId;
                departureLocation.Type = (enmLocationType)departureLocationType;
                var departureLocations = new List<mdlLocation>();
                departureLocations.Add(departureLocation);

         
                


                var pRequest5 = new GetNightsRequest() { ProductType = enmProductType.HolidayPackage, ArrivalLocations = arrivallocations, DepartureLocations = departureLocations, CheckIn = checkInDate, IncludeSubLocations = true };

                ProductRepository pRepo5 = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rReponse5 = pRepo5.GetNights(pRequest5);

                if (rReponse5.Header.Success)
                {
                    return Json(rReponse5.Body.Nights);
                }
                else
                {
                    return Json("");
                }
            }
            else
            {
                return Json("");
            }

        }

        [HttpPost]
        public IActionResult Search([FromBody] SearchViewModel searchForm) 
        {
            

            SearchResultModel result = new SearchResultModel();

            if (User != null && User.Identity.IsAuthenticated)
            {
                var roomCriteria = new List<mdlRoomCriteria>();
                var room1 = new mdlRoomCriteria();
                room1.Adult = searchForm.Adult1;
                var childAges1 = new List<int>();
                if (searchForm.Child1 > 0)
                {
                    if (searchForm.ChildAge11 > 0)
                    {
                        childAges1.Add(searchForm.ChildAge11);
                    }
                    if (searchForm.ChildAge12 > 0)
                    {
                        childAges1.Add(searchForm.ChildAge12);
                    }
                    if (searchForm.ChildAge13 > 0)
                    {
                        childAges1.Add(searchForm.ChildAge13);
                    }
                    if (searchForm.ChildAge14 > 0)
                    {
                        childAges1.Add(searchForm.ChildAge14);
                    }
                    room1.ChildAges = childAges1;
                }
                roomCriteria.Add(room1);

                if (searchForm.Room >= 2)
                {
                    var room2 = new mdlRoomCriteria();
                    room2.Adult = searchForm.Adult2;
                    var childAges2 = new List<int>();
                    if (searchForm.Child2 > 0)
                    {
                        if (searchForm.ChildAge21 > 0)
                        {
                            childAges2.Add(searchForm.ChildAge21);
                        }
                        if (searchForm.ChildAge22 > 0)
                        {
                            childAges2.Add(searchForm.ChildAge22);
                        }
                        if (searchForm.ChildAge23 > 0)
                        {
                            childAges2.Add(searchForm.ChildAge23);
                        }
                        if (searchForm.ChildAge24 > 0)
                        {
                            childAges2.Add(searchForm.ChildAge24);
                        }
                        room2.ChildAges = childAges2;

                    }
                    roomCriteria.Add(room2);
                }

                if (searchForm.Room >= 3)
                {
                    var room3 = new mdlRoomCriteria();
                    room3.Adult = searchForm.Adult3;
                    var childAges3 = new List<int>();
                    if (searchForm.Child3 > 0)
                    {
                        if (searchForm.ChildAge31 > 0)
                        {
                            childAges3.Add(searchForm.ChildAge31);
                        }
                        if (searchForm.ChildAge32 > 0)
                        {
                            childAges3.Add(searchForm.ChildAge32);
                        }
                        if (searchForm.ChildAge33 > 0)
                        {
                            childAges3.Add(searchForm.ChildAge33);
                        }
                        if (searchForm.ChildAge34 > 0)
                        {
                            childAges3.Add(searchForm.ChildAge34);
                        }
                        room3.ChildAges = childAges3;

                    }
                    roomCriteria.Add(room3);
                }
                if (searchForm.Room >= 4)
                {
                    var room4 = new mdlRoomCriteria();
                    room4.Adult = searchForm.Adult4;
                    var childAges4 = new List<int>();
                    if (searchForm.Child4 > 0)
                    {

                        if (searchForm.ChildAge41 > 0)
                        {
                            childAges4.Add(searchForm.ChildAge41);
                        }
                        if (searchForm.ChildAge42 > 0)
                        {
                            childAges4.Add(searchForm.ChildAge42);
                        }
                        if (searchForm.ChildAge43 > 0)
                        {
                            childAges4.Add(searchForm.ChildAge43);
                        }
                        if (searchForm.ChildAge44 > 0)
                        {
                            childAges4.Add(searchForm.ChildAge44);
                        }
                        room4.ChildAges = childAges4;

                    }
                    roomCriteria.Add(room4);
                }
               
                var pRequest3 = new PriceSearchRequest()
                {
                    CheckIn = DateTime.ParseExact(searchForm.CheckInDate, "MM/dd/yyyy", null),
                    Night = int.Parse(searchForm.Nights),
                    RoomCriteria = roomCriteria != null ? roomCriteria : new List<mdlRoomCriteria>(),
                    Currency = searchForm.Currency != null ? searchForm.Currency : string.Empty,
                    ProductType = enmProductType.HolidayPackage,
                    Nationality = searchForm.Nationality,
                    CheckAllotment = false,
                    CheckStopSale = false,
                    GetOnlyDiscountedPrice = false
                };
 
                    var deplocations = new List<mdlLocation>();
                    var deplocation = new mdlLocation();
                    deplocation.Id = searchForm.DepartureId;
                    deplocation.Type = enmLocationType.City;
                    deplocations.Add(deplocation);
                    pRequest3.DepartureLocations = deplocations;
                    
                
                    var arvlocations = new List<mdlLocation>();
                    var arvlocation = new mdlLocation();
                    arvlocation.Id = searchForm.ArrivalId;
                    arvlocation.Type = enmLocationType.City;
                    arvlocations.Add(arvlocation);
                    pRequest3.ArrivalLocations = arvlocations;


                ProductRepository productRepo = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rResponse3 = productRepo.PriceSearch(pRequest3);

                if (rResponse3.Header.Success)
                {
                    result.Hpackages = rResponse3.Body.Hotels;
                }
                else
                {
                    result.ErrorMsg = rResponse3.Header.Messages.First().Message;
                }
            }
            else

            {
                result.ErrorMsg = "Please Login first!";
            }

            return PartialView("SearchResult", result);
            
        }



        public IEnumerable<mdlTraveller> GetTravellers(string[] pTravellers, TransactionResponse pTransactionResponse)
        {
            return pTransactionResponse.ReservationData.Travellers.Where(w => pTravellers.Contains(w.TravellerId));
        }

        public IActionResult Privacy()
            {
                return View();
            }

            //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        IEnumerable<RoomingBlock> GetRooms(TransactionResponse pTransactionResponse)
        {
            var groups = pTransactionResponse.ReservationData.Services.Where(w => RoomingSupportedProductTypes.Contains(w.ProductType) && w.IsExtraService == false && (w.AdditionalFields == null || (w.AdditionalFields != null && !w.AdditionalFields.ContainsKey("removable"))))
                .GroupBy(g => new { g.ProductType, g.Code });
            var roomNumber = 1;
            foreach (var group in groups)
            {
                foreach (var service in group.ToList())
                {
                    yield return new RoomingBlock()
                    {
                        RoomNumber = roomNumber++,
                        Travellers = GetTravellers(service.Travellers, pTransactionResponse)
                    };
                }
            }
        }

        [HttpGet]
        public IActionResult Reservation(string offerId, string currency)
        {
            ReservationModel result = new ReservationModel();
            if (User != null && User.Identity.IsAuthenticated)
            {
                var pRequest4 = new GetOfferDetailsRequest()
                {
                    OfferIds = new string[] { offerId }
                };
                ProductRepository product = new ProductRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rResponse4 = product.GetOfferDetails(pRequest4);
                if (rResponse4.Header.Success)
                {
                    result.offer = rResponse4.Body.OfferDetails;
                    var pRequest5 = new BeginTransactionRequest()
                    {
                        OfferIds = new string[] { offerId },
                        Currency = currency,
                    };

                    BookingRepository booking = new BookingRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                    var rResponse5 = booking.BeginTransaction(pRequest5);
                    if (rResponse5.Header.Success)
                    {
                        result.TransactionResponse = rResponse5.Body;
                        result.Rooms = GetRooms(rResponse5.Body);

                        var pRequest6 = new GetNationalitiesRequest();
                        LookupRepository lookup1 = new LookupRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                        var rResponse1 = lookup1.GetNationalities(pRequest6);


                        if (rResponse1.Header.Success)
                        {
                            result.Nationalities = rResponse1.Body.Nationalities;
                        }
                        return View(result);
                    }
                    else
                    {
                        result.ErrorMsg = rResponse5.Header.Messages.First().Message;
                    }
                }

                else
                {
                    result.ErrorMsg = rResponse4.Header.Messages.First().Message;
                }
            }
            return View(result);
        }



        [HttpGet]
        public IActionResult Result(string transactionId)
        {

            ResultModelView r1 = new ResultModelView();

            if (User != null && User.Identity.IsAuthenticated)
            {
                var pRequest = new CommitTransactionRequest();
                pRequest.TransactionId = Guid.Parse(transactionId);
                BookingRepository booking = new BookingRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rResponse = booking.CommitTransaction(pRequest);

                if (rResponse.Header.Success)
                {
                    r1.TransactionId = rResponse.Body.TransactionId.ToString();
                    r1.BookingNumber = rResponse.Body.ReservationNumber;

                    var pRequest1 = new GetReservationDetailRequest();
                    pRequest1.ReservationNumber = rResponse.Body.ReservationNumber;
                    var rResponse1 = booking.GetReservationDetail(pRequest1);



                    if (rResponse1.Header.Success)
                    {
                        r1.Travellers = rResponse1.Body.ReservationData.Travellers;
                        r1.Services = rResponse1.Body.ReservationData.Services;
                        r1.TotalPrice = rResponse1.Body.ReservationData.ReservationInfo.TotalPrice;

                    }
                    else
                    {
                        r1.ErrorMsg = rResponse1.Header.Messages.First().Message;
                    }
                }
                else
                {
                    r1.ErrorMsg = rResponse.Header.Messages.First().Message;
                }
            }
            else
            {
                r1.ErrorMsg = "Please login first.";
            }

            return View(r1);
        }

        [HttpPost]

        public ActionResult Booking([FromBody] BookingViewModel resForm)
        {
            BookingReservationModel b1 = new BookingReservationModel();
            if (User != null && User.Identity.IsAuthenticated)
            {
                var pRequest = new SetReservationInfoRequest();
                pRequest.TransactionId = Guid.Parse(resForm.TransactionId);

                pRequest.Currency = resForm.Currency;

                pRequest.AgencyReservationNumber = resForm.AgencyReservationNumber;

                pRequest.ReservationInfo = JsonConvert.DeserializeObject<mdlReservationInfo>(resForm.ReservationInfo);

                pRequest.CustomerInfo = resForm.CustomerInfo;

                List<mdlTraveller> travellers = new List<mdlTraveller>();
                foreach (var room in resForm.Rooms)
                {
                    foreach (var trav in room.Travellers)
                    {
                        var trv = new mdlTraveller();

                        trv.Title = (enmTitle)trav.Title;
                        trv.Name = trav.Name;
                        trv.Surname = trav.Surname;
                        trv.TravellerId = trav.TravellerId;
                        trv.BirthDate = trav.BirthDate;
                        var nationality = new mdlNationality();
                        nationality.TwoLetterCode = trav.Nationality;
                        trv.Nationality = nationality;
                        var passNo = new mdlPassportInfo();
                        passNo.Number = trav.PassportNo;
                        passNo.IssueCountryCode = trav.IssueCountry;
                        passNo.IssueDate = DateTime.Parse(trav.IssueDate);
                        passNo.IssueDate = DateTime.Parse(trav.ExpireDate);

                        trv.PassportInfo = passNo;

                        var phone = new mdlPhoneNumber();
                        phone.CountryCode = trav.Code;
                        phone.PhoneNumber = trav.PhoneNumber;

                        var address = new WebService.Adapter.Models.Booking.mdlAddress();
                        address.ContactPhone = phone;

                        trv.Address = address;
                        travellers.Add(trv);
                        
                    }
                }
                pRequest.Travellers = travellers.ToArray();

                BookingRepository booking = new BookingRepository(User.Claims.First().Value, "https://prod-services.tourvisio.com/v2/");
                var rResponse = booking.SetReservationInfo(pRequest);

                if (rResponse.Header.Success)
                {
                    b1.TransactionId = rResponse.Body.TransactionId.ToString();
                }
                else
                {
                    b1.ErrorMsg = rResponse.Header.Messages.First().Message;
                }
            }
            else
            {
                b1.ErrorMsg = "Please login First";
            }
            return Json(b1);
        }
    }
}
