using System;
using System.ComponentModel.DataAnnotations;
using MassTransit;
using MongoDB.Bson.Serialization.Attributes;

namespace ConferenceManagementSystem.Conference.Model
{
    public class SeatType
    {
        [BsonConstructor]
        public SeatType()
        {
            Id = NewId.NextGuid();
        }

        public SeatType(Guid conferenceId) : this()
        {
            ConferenceId = conferenceId;
        }

        [BsonId]
        public Guid Id { get; set; }

        public Guid ConferenceId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(70, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public string Description { get; set; }

        [Range(0, 100000)]
        public int Quantity { get; set; }

        [Range(0, 50000)]
        public decimal Price { get; set; }
    }
}