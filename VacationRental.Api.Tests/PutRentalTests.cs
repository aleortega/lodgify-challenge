using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Application.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        private async Task CreateBooking(int rentalId, int nights, DateTime date)
        {
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = nights,
                Start = date
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
                Assert.True(postBookingResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GivenRequestWithoutPreparationTimeInDays_WhenPutRental_ThenItIsSuccesfullyUpdated()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));
            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 03));

            var putRequest = new RentalBindingModel { Units = 1 };

            RentalViewModel putResult;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                putResult = await putResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(putRequest.Units, getResult.Units);
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={putResult.Id}&start=2001-01-01&nights=4"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(putResult.Id, getCalendarResult.RentalId);
                Assert.Equal(postResult.Id, getCalendarResult.RentalId);
                Assert.Equal(4, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2001, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Single(getCalendarResult.Dates[2].Bookings);
                Assert.Empty(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Empty(getCalendarResult.Dates[3].PreparationTimes);
            }
        }

        [Fact]
        public async Task GivenRequestWithPreparationTimeInDays_WhenPutRentalWithoutPreparationTimeInDays_ThenItIsSuccesfullyUpdated()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));
            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 05));

            var putRequest = new RentalBindingModel { Units = 1, PreparationTimeInDays = 0 };

            RentalViewModel putResult;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                putResult = await putResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(putRequest.Units, getResult.Units);
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={putResult.Id}&start=2001-01-01&nights=8"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(putResult.Id, getCalendarResult.RentalId);
                Assert.Equal(postResult.Id, getCalendarResult.RentalId);
                Assert.Equal(8, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2001, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Empty(getCalendarResult.Dates[2].Bookings);
                Assert.Empty(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Empty(getCalendarResult.Dates[3].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Single(getCalendarResult.Dates[4].Bookings);
                Assert.Empty(getCalendarResult.Dates[4].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 06), getCalendarResult.Dates[5].Date);
                Assert.Single(getCalendarResult.Dates[5].Bookings);
                Assert.Empty(getCalendarResult.Dates[5].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 07), getCalendarResult.Dates[6].Date);
                Assert.Empty(getCalendarResult.Dates[6].Bookings);
                Assert.Empty(getCalendarResult.Dates[6].PreparationTimes);
            }
        }

        [Fact]
        public async Task GivenRequestWithPreparationTimeInDays_WhenPutRentalWithPreparationTimeInDays_ThenItIsSuccesfullyUpdated()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));
            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 05));

            var putRequest = new RentalBindingModel { Units = 1, PreparationTimeInDays = 1 };

            RentalViewModel putResult;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                putResult = await putResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(putRequest.Units, getResult.Units);
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={putResult.Id}&start=2001-01-01&nights=8"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postResult.Id, getCalendarResult.RentalId);
                Assert.Equal(8, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2001, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Empty(getCalendarResult.Dates[2].Bookings);
                Assert.Single(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Empty(getCalendarResult.Dates[3].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Single(getCalendarResult.Dates[4].Bookings);
                Assert.Empty(getCalendarResult.Dates[4].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 06), getCalendarResult.Dates[5].Date);
                Assert.Single(getCalendarResult.Dates[5].Bookings);
                Assert.Empty(getCalendarResult.Dates[5].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 07), getCalendarResult.Dates[6].Date);
                Assert.Empty(getCalendarResult.Dates[6].Bookings);
                Assert.Single(getCalendarResult.Dates[6].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 08), getCalendarResult.Dates[7].Date);
                Assert.Empty(getCalendarResult.Dates[7].Bookings);
                Assert.Empty(getCalendarResult.Dates[7].PreparationTimes);
            }
        }

        [Fact]
        public async Task GivenRequestWithoutPreparationTimeInDays_WhenPutRentalWithPreparationTimeInDays_ThenItIsSuccesfullyUpdated()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));
            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 05));

            var putRequest = new RentalBindingModel { Units = 1, PreparationTimeInDays = 1 };

            RentalViewModel putResult;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                putResult = await putResponse.Content.ReadAsAsync<RentalViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(putRequest.Units, getResult.Units);
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={putResult.Id}&start=2001-01-01&nights=8"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postResult.Id, getCalendarResult.RentalId);
                Assert.Equal(8, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2001, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Empty(getCalendarResult.Dates[2].Bookings);
                Assert.Single(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Empty(getCalendarResult.Dates[3].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Single(getCalendarResult.Dates[4].Bookings);
                Assert.Empty(getCalendarResult.Dates[4].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 06), getCalendarResult.Dates[5].Date);
                Assert.Single(getCalendarResult.Dates[5].Bookings);
                Assert.Empty(getCalendarResult.Dates[5].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 07), getCalendarResult.Dates[6].Date);
                Assert.Empty(getCalendarResult.Dates[6].Bookings);
                Assert.Single(getCalendarResult.Dates[6].PreparationTimes);

                Assert.Equal(new DateTime(2001, 01, 08), getCalendarResult.Dates[7].Date);
                Assert.Empty(getCalendarResult.Dates[7].Bookings);
                Assert.Empty(getCalendarResult.Dates[7].PreparationTimes);
            }
        }

        [Fact]
        public async Task GivenRequestWithoutPreparationTimeInDays_WhenPutRental_ThenItFailsDueToBookingConflict()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));
            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));

            var putRequest = new RentalBindingModel { Units = 1 };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
                {
                }
            });
        }

        [Fact]
        public async Task GivenRequestWithPreparationTimeInDays_WhenPutRentalWithPreparationTimeInDays_ThenItFailsDueToBookingConflict()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));
            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 04));

            var putRequest = new RentalBindingModel { Units = 1, PreparationTimeInDays = 2 };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
                {
                }
            });
        }

        [Fact]
        public async Task GivenRequestWithoutPreparationTimeInDays_WhenPutRentalWithPreparationTimeInDays_ThenItFailsDueToBookingConflict()
        {
            var postRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 01));
            await this.CreateBooking(postResult.Id, 2, new DateTime(2001, 01, 03));

            var putRequest = new RentalBindingModel { Units = 1, PreparationTimeInDays = 1 };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", putRequest))
                {
                }
            });
        }
    }
}
