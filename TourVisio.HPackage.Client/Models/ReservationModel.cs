﻿using System;
using System.Collections.Generic;
using TourVisio.WebService.Adapter.Models;
using TourVisio.WebService.Adapter.ServiceModels.BookingModels;

namespace TourVisio.HPackage.Client.Models
{
    public class ReservationModel
    {
        public IEnumerable<mdlOfferDetail> offer;
        public string ErrorMsg { get; set; }
        public TransactionResponse TransactionResponse { get;  set; }
        public IEnumerable<RoomingBlock> Rooms { get; set; }
        public IEnumerable<mdlNationalityItem> Nationalities { get; set; }
        public DateTime BirthDate { get; set; }
    }
}