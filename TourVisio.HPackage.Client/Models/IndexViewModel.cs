using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TourVisio.WebService.Adapter.Models;

namespace TourVisio.HPackage.Client.Models
{
    public class IndexViewModel
    {
        public IEnumerable<mdlCurrency> Currencies { get; set; }
        public IEnumerable<mdlLocation> Departures { get; set; }
        public IEnumerable<mdlLocation> Arrivals { get; set; }

        [DataType(DataType.Date)]
        public List<DateTime> CheckInDates { get; set; }

    }
}