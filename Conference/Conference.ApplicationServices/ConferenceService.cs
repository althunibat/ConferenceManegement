using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ConferenceManagementSystem.Conference.Contracts.Events;
using ConferenceManagementSystem.Conference.Model;
using ConferenceManagementSystem.Conference.Model.IRepository;
using MassTransit;
using MassTransit.Logging;
using RapidTransit.Core;

namespace ConferenceManagementSystem.Conference.ApplicationServices {
    public class ConferenceService : IConferenceService {
        private readonly IOrderRepository _orderRepository;
        private readonly IConferenceRepository _conferenceRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IHostServiceBus _hostServiceBus;
        private readonly ILogger _logger;
        public ConferenceService(IOrderRepository orderRepository, IConferenceRepository conferenceRepository, IHostServiceBus hostServiceBus, ILogger logger, ISeatRepository seatRepository) {
            _orderRepository = orderRepository;
            _conferenceRepository = conferenceRepository;
            _hostServiceBus = hostServiceBus;
            _logger = logger;
            _seatRepository = seatRepository;
        }

        public async Task CreateConference(ConferenceInfo conference) {
            if (await _conferenceRepository.CheckExistenceBySlug(conference.Slug))
                throw new DuplicateNameException("The chosen conference slug is already taken.");

            // Conference publishing is explicit. 
            if (conference.IsPublished)
                conference.IsPublished = false;

            await _conferenceRepository.Create(conference);
            _hostServiceBus.Publish<IConferenceCreated>(new ConferenceCreated(conference));
        }

        public async Task CreateSeat(SeatType seat) {
            if (!await _conferenceRepository.CheckExistenceById(seat.ConferenceId))
                throw new ApplicationException("Conference record wasn't found!");

            await _seatRepository.AddConferenceSeat(seat);

            var conference = await _conferenceRepository.Get(seat.ConferenceId);

            // Don't publish new seats if the conference was never published 
            // (and therefore is not published either).
            if (conference.WasEverPublished)
                _hostServiceBus.Publish<ISeatCreated>(new SeatCreated(seat));

        }

        public async Task<ConferenceInfo> FindConference(string slug) {
            return await _conferenceRepository.Get(slug);
        }

        public async Task<ConferenceInfo> FindConference(string email, string accessCode) {
            return await _conferenceRepository.Get(email, accessCode);
        }

        public async Task<IEnumerable<SeatType>> FindSeatTypes(Guid conferenceId) {
            return await _seatRepository.FindSeatTypes(conferenceId);
        }

        public async Task<SeatType> FindSeatType(Guid seatTypeId) {
            return await _seatRepository.FindSeatType(seatTypeId);
        }

        public async Task<IEnumerable<Order>> FindOrders(Guid conferenceId)
        {
            return await _orderRepository.FindOrders(conferenceId);
        }

        public async Task UpdateConference(ConferenceInfo conference) {
            if (!await _conferenceRepository.CheckExistenceById(conference.Id))
                throw new ApplicationException("Conference record wasn't found!");
            await _conferenceRepository.UpdateConference(conference);
        }

        public async Task UpdateSeat( SeatType seat)
        {
            if (!await _seatRepository.CheckExistenceById(seat.Id))
                throw new ApplicationException("Seat record wasn't found!");

            await _seatRepository.UpdateSeat(seat);
            var conference = await _conferenceRepository.Get(seat.ConferenceId);
            // Don't publish seat updates if the conference was never published 
            // (and therefore is not published either).
            if (conference.WasEverPublished)
                _hostServiceBus.Publish<ISeatUpdated>(new SeatUpdated(seat));

        }

        public async Task Publish(Guid conferenceId)
        {
            if (!await _conferenceRepository.CheckExistenceById(conferenceId))
                throw new ApplicationException("Conference record wasn't found!");

            var conference = await _conferenceRepository.Get(conferenceId);
            conference.IsPublished = true;
            if (!conference.WasEverPublished)
            {
                conference.WasEverPublished = true;
                await _conferenceRepository.UpdateConference(conference);
                var seats = await _seatRepository.FindSeatTypes(conferenceId);
                foreach (var seat in seats)
                {
                    _hostServiceBus.Publish<ISeatCreated>(new SeatCreated(seat));
                }
            }
            else
            {
                await _conferenceRepository.UpdateConference(conference);
            }
            _hostServiceBus.Publish<IConferencePublished>(new ConferencePublished(conferenceId));
        }

        public async Task Unpublish(Guid conferenceId) {
            if (!await _conferenceRepository.CheckExistenceById(conferenceId))
                throw new ApplicationException("Conference record wasn't found!");

            var conference = await _conferenceRepository.Get(conferenceId);
            conference.IsPublished = false;
            await _conferenceRepository.UpdateConference(conference);
            _hostServiceBus.Publish<IConferenceUnpublished>(new ConferenceUnpublished(conferenceId));
        }

        public async Task DeleteSeat(Guid seatTypeId) {
            if(!await _seatRepository.CheckExistenceById(seatTypeId))
                throw new ApplicationException("Seat record wasn't found!");
            var seat = await _seatRepository.FindSeatType(seatTypeId);
            var conference = await _conferenceRepository.Get(seat.ConferenceId);
            if(conference.WasEverPublished)
                throw new InvalidOperationException("Can't delete seats from a conference that has been published at least once.");

            await _seatRepository.DeleteSeat(seatTypeId);
        }
    }

    class ConferenceCreated : IConferenceCreated {
        public ConferenceCreated(ConferenceInfo conference) {
            CorrelationId = conference.Id;
            Name = conference.Name;
            Description = conference.Description;
            Location = conference.Location;
            Slug = conference.Slug;
            Tagline = conference.Tagline;
            TwitterSearch = conference.TwitterSearch;
            StartDate = conference.StartDate;
            EndDate = conference.EndDate;
            Owner = new Owner(conference.OwnerName, conference.OwnerEmail);
            Id = NewId.NextGuid();
            TimeStamp = DateTimeOffset.UtcNow;
            Version = 1;
        }

        public Guid CorrelationId { get; }
        public Guid Id { get; }
        public int Version { get; }
        public DateTimeOffset TimeStamp { get; }
        public string Name { get; }
        public string Description { get; }
        public string Location { get; }
        public string Slug { get; }
        public string Tagline { get; }
        public string TwitterSearch { get; }
        public DateTimeOffset StartDate { get; }
        public DateTimeOffset EndDate { get; }
        public Owner Owner { get; }
    }

    class SeatCreated : ISeatCreated {
        public SeatCreated(SeatType seat) {
            CorrelationId = seat.ConferenceId;
            Name = seat.Name;
            Description = seat.Description;
            Price = seat.Price;
            Quantity = seat.Quantity;
            Id = seat.Id;
            TimeStamp = DateTimeOffset.UtcNow;
            Version = 1;
        }

        public Guid CorrelationId { get; }
        public Guid Id { get; }
        public int Version { get; }
        public DateTimeOffset TimeStamp { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public int Quantity { get; }
    }

    class SeatUpdated : ISeatUpdated {
        public SeatUpdated(SeatType seat) {
            CorrelationId = seat.ConferenceId;
            Name = seat.Name;
            Description = seat.Description;
            Price = seat.Price;
            Quantity = seat.Quantity;
            Id = seat.Id;
            TimeStamp = DateTimeOffset.UtcNow;
            Version = 1;
        }

        public Guid CorrelationId { get; }
        public Guid Id { get; }
        public int Version { get; }
        public DateTimeOffset TimeStamp { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public int Quantity { get; }
    }

    class ConferencePublished: IConferencePublished {
        public ConferencePublished(Guid correlationId)
        {
            CorrelationId = correlationId;
            Id = NewId.NextGuid();
            Version = 1;
            TimeStamp = DateTimeOffset.UtcNow;
        }

        public Guid CorrelationId { get; }
        public Guid Id { get; }
        public int Version { get; }
        public DateTimeOffset TimeStamp { get; }
    }

    class ConferenceUnpublished : IConferenceUnpublished {
        public ConferenceUnpublished(Guid correlationId) {
            CorrelationId = correlationId;
            Id = NewId.NextGuid();
            Version = 1;
            TimeStamp = DateTimeOffset.UtcNow;
        }

        public Guid CorrelationId { get; }
        public Guid Id { get; }
        public int Version { get; }
        public DateTimeOffset TimeStamp { get; }
    }
}