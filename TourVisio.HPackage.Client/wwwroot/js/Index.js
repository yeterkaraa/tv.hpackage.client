$(document).ready(function ()
{

    $("[name = 'Departure']").change(function () {
        $.ajax({
            url: "/Home/GetArrivals?departureLocationId=" + $(this).val() + "&departureLocationType=" + $(this).find("option:selected").data("type"),
            type: 'GET',
            dataType: "json",
            contentType: "application/json; charset = utf - 8",
            success: function (response)
            {
                var mySelect = $("[name = 'Arrival']");
                var JSONData = eval(response);
                mySelect.empty();
                $.each(JSONData, function (index, itemData)
                {
                    if (itemData.type != 1)
                    {
                        mySelect.append("<option value='" + itemData.id + "' data-type='" + itemData.type + "' > " + itemData.name + "</option > ");
                    }
                    else
                    {
                        mySelect.append("<option class='font-weight-bold' disabled value='" + itemData.id + "' data-type='" + itemData.type + "'> " + itemData.name + "</option > ");
                    }
                   
                })
            }
        });

    });

    $("[name = 'Arrival']").change(function () {
        $.ajax({
            url: "/Home/GetCheckInDates?arrivalLocationId=" + $(this).val() + "&arrivalLocationType=" + $(this).find("option:selected").data("type") + "&departureLocationType=" + $("[name = 'Departure']").find("option:selected").data("type") + "&departureLocationId=" + $("[name = 'Departure']").val(),
            type: 'GET',
            dataType: "json",
            contentType: "application/json; charset = utf - 8",
            success: function (response) {
                var mySelect = $("[name = 'CheckInDate']");
                var availabledDates = eval(response);
                mySelect.datepicker({
                    beforeShowDay: function (date) {
                        var string = $.datepicker.formatDate('yy-mm-ddT00:00:00', date);
                        //var string = $.datepicker.formatDate('yy-mm-ddT', date);
                        return [availabledDates.indexOf(string) > -1]
                    }
                });
                
            }
        });

    });

    $("[name = 'CheckInDate']").change(function () {
        $.ajax({
            url: "/Home/GetNights?arrivalLocationId=" + $("[name = 'Arrival']").val() + "&arrivalLocationType=" + $("[name = 'Arrival']").find("option:selected").data("type") + "&departureLocationType=" + $("[name = 'Departure']").find("option:selected").data("type") + "&departureLocationId=" + $("[name = 'Departure']").val() + "&checkInDate=" + $("[name = 'CheckInDate']").val(),
            type: 'GET',
            dataType: "json",
            contentType: "application/json; charset = utf - 8",
            success: function (response) {
                var mySelect = $("[name = 'Nights']");
                var JSONData = eval(response);
                mySelect.empty();
                $.each(JSONData, function (index, itemData)
                {
                    mySelect.append("<option value='" + itemData +  "'>"  + itemData  + "</option>");

                })
            }
        });

    });


    $(document).ready(function () {

        $("[name='Child1']").change(function () {

            var value = $(this).val();
            for (i = 1; i <= 4; i++) {
                $("[name='ChildAge1" + i + "']").attr("disabled", value >= i ? false : true);
            }
        });

        $("[name='Child2']").change(function () {
            var value = $(this).val();
            for (i = 1; i <= 4; i++) {
                $("[name='ChildAge2" + i + "']").attr("disabled", value >= i ? false : true);
            }
        });

        $("[name='Child3']").change(function () {
            var value = $(this).val();
            for (i = 1; i <= 4; i++) {
                $("[name='ChildAge3" + i + "']").attr("disabled", value >= i ? false : true);
            }
        });

        $("[name='Child4']").change(function () {
            var value = $(this).val();
            for (i = 1; i <= 4; i++) {
                $("[name='ChildAge4" + i + "']").attr("disabled", value >= i ? false : true);
            }
        });

        $("[name='Room']").change(function () {
            var value = $(this).val();
            for (i = 1; i <= 4; i++) {
                $("[name='Adult" + i + "']").attr("disabled", value >= i ? false : true);
                $("[name='Child" + i + "']").val(0);
                $("[name='Child" + i + "']").attr("disabled", value >= i ? false : true);
                $("[name='ChildAge" + i + "1']").attr("disabled", true);
                $("[name='ChildAge" + i + "2']").attr("disabled", true);
                $("[name='ChildAge" + i + "3']").attr("disabled", true);
                $("[name='ChildAge" + i + "4']").attr("disabled", true);

                if (value >= i) {
                    $("[name='roomRow" + i + "']").show();
                } else {
                    $("[name='roomRow" + i + "']").hide();
                }
            }
        });


    });





    $("[name='btnPost']").click(function () {
        var searchForm = new Object();
        searchForm.ArrivalId = $("[name='Arrival']").val();
        searchForm.DepartureId = $("[name='Departure']").val();
        searchForm.CheckInDate = $("[name='CheckInDate']").val();
        searchForm.Nights = $("[name='Nights']").val();
        searchForm.Nationality = $("[name='Nationality']").val();
        searchForm.Currency = $("[name='Currency']").val();
        searchForm.Room = parseInt($("[name='Room']").val());

        searchForm.Adult1 = $("[name='Adult1']").is(":disabled") ? 0 : parseInt($("[name='Adult1']").val());
        searchForm.Child1 = parseInt($("[name='Child1']").val());
        searchForm.ChildAge11 = $("[name='ChildAge11']").is(":disabled") ? 0 : parseInt($("[name='ChildAge11']").val());
        searchForm.ChildAge12 = $("[name='ChildAge12']").is(":disabled") ? 0 : parseInt($("[name='ChildAge12']").val());
        searchForm.ChildAge13 = $("[name='ChildAge13']").is(":disabled") ? 0 : parseInt($("[name='ChildAge13']").val());
        searchForm.ChildAge14 = $("[name='ChildAge14']").is(":disabled") ? 0 : parseInt($("[name='ChildAge14']").val());

        searchForm.Adult2 = $("[name='Adult2']").is(":disabled") ? 0 : parseInt($("[name='Adult2']").val());
        searchForm.Child2 = parseInt($("[name='Child2']").val());
        searchForm.ChildAge21 = $("[name='ChildAge21']").is(":disabled") ? 0 : parseInt($("[name='ChildAge21']").val());
        searchForm.ChildAge22 = $("[name='ChildAge22']").is(":disabled") ? 0 : parseInt($("[name='ChildAge22']").val());
        searchForm.ChildAge23 = $("[name='ChildAge23']").is(":disabled") ? 0 : parseInt($("[name='ChildAge23']").val());
        searchForm.ChildAge24 = $("[name='ChildAge24']").is(":disabled") ? 0 : parseInt($("[name='ChildAge24']").val());

        searchForm.Adult3 = $("[name='Adult3']").is(":disabled") ? 0 : parseInt($("[name='Adult3']").val());
        searchForm.Child3 = parseInt($("[name='Child3']").val());
        searchForm.ChildAge31 = $("[name='ChildAge31']").is(":disabled") ? 0 : parseInt($("[name='ChildAge31']").val());
        searchForm.ChildAge32 = $("[name='ChildAge32']").is(":disabled") ? 0 : parseInt($("[name='ChildAge32']").val());
        searchForm.ChildAge33 = $("[name='ChildAge33']").is(":disabled") ? 0 : parseInt($("[name='ChildAge33']").val());
        searchForm.ChildAge34 = $("[name='ChildAge34']").is(":disabled") ? 0 : parseInt($("[name='ChildAge34']").val());

        searchForm.Adult4 = $("[name='Adult4']").is(":disabled") ? 0 : parseInt($("[name='Adult4']").val());
        searchForm.Child4 = parseInt($("[name='Child4']").val());
        searchForm.ChildAge41 = $("[name='ChildAge41']").is(":disabled") ? 0 : parseInt($("[name='ChildAge41']").val());
        searchForm.ChildAge42 = $("[name='ChildAge42']").is(":disabled") ? 0 : parseInt($("[name='ChildAge42']").val());
        searchForm.ChildAge43 = $("[name='ChildAge43']").is(":disabled") ? 0 : parseInt($("[name='ChildAge43']").val());
        searchForm.ChildAge44 = $("[name='ChildAge44']").is(":disabled") ? 0 : parseInt($("[name='ChildAge44']").val());



        if (searchForm != null) {
            $.ajax({
                type: "POST",
                url: "/Home/Search",
                data: JSON.stringify(searchForm),
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                success: function (response) {
                    $('#searchResult').html(response);
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }
    });

    $("#searchResult").on("click", ".hotel", function () {
        var id = $(this).attr("id");
        $("[name='offers" + id + "']").toggle();
    });

    $("#searchResult").on("click", ".book", function () {
        var id = $(this).attr("offerId");
        window.location.href = "/Home/Reservation?offerId=" + id + "&currency=" + $("[name='Currency']").val();
    });
    $("[name='btnBook']").click(function () {

        var roomCount = $("[name='RoomCount']").val();

        var resForm = new Object();
        var transactionId = $("[name='TransactionId']").val();
        resForm.TransactionId = transactionId;

        var currency = $("[name='Currency']").val();
        resForm.Currency = currency;

        var agencyResNumber = $("[name='AgencyReservationNumber']").val();
        resForm.AgencyReservationNumber = agencyResNumber;

        var resInfo = $("[name='ReservationInfo']").val();
        resForm.ReservationInfo = resInfo;

        var customerInfo = $("[name='CustomerInfo']").val();
        resForm.CustomerInfo = customerInfo;

        var birthDate = $("[name='BirthDate']").val();
        resForm.BirthDate = birthDate;
        var rooms = new Array();

        for (var r = 1; r <= roomCount; r++) {
            var travellerCount = $("[name='TravellerCount" + r + "']").val();
            var room = new Object();
            room.RoomNumber = r;
            var travellers = new Array();

            for (var t = 1; t <= travellerCount; t++) {
                var traveller = new Object();
                traveller.Title = $("[name='title" + r + t + "']").val();
                traveller.Name = $("[name='Name" + r + t + "']").val();
                traveller.Surname = $("[name='Surname" + r + t + "']").val();
                traveller.Nationality = $("[name='Nationality" + r + t + "']").val();
                traveller.Code = $("[name='Code" + r + t + "']").val();
                traveller.PhoneNumber = $("[name='PhoneNumber" + r + t + "']").val();
                traveller.PassportNo = $("[name='PassportNo" + r + t + "']").val();
                traveller.ExpireDate = $("[name='ExpireDate" + r + t + "']").val();
                traveller.IssueDate = $("[name='IssueDate" + r + t + "']").val();
                traveller.IssueCountry = $("[name='IssueCountry" + r + t + "']").val();
                traveller.travellerId = $("[name='TravellerId" + r + t + "']").val();
                traveller.BirthDate = $("[name='BirthDate" + r + t + "']").val();
                travellers.push(traveller);
            }
            room.Travellers = travellers;
            rooms.push(room);
        }
        resForm.Rooms = rooms;
        if (resForm != null) {
            $.ajax({
                type: "POST",
                url: "/Home/Booking",
                data: JSON.stringify(resForm),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.transactionId != null) {
                        window.location.href = "/Home/Result?transactionId=" + response.transactionId;
                    } else {
                        alert(response.error);
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }
    });


})


                 