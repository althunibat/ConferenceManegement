using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConferenceManagementSystem.Conference.Model.Properties;
using MongoDB.Bson.Serialization.Attributes;

namespace ConferenceManagementSystem.Conference.Model
{
    public class Order {
        public enum OrderStatus {
            Pending,
            Paid,
        }

        public Order(Guid conferenceId, Guid orderId, string accessCode)
            : this() {
            Id = orderId;
            ConferenceId = conferenceId;
            AccessCode = accessCode;
        }

        public Order() {
            Seats = new List<OrderSeat>();
        }

        [BsonId]
        public Guid Id { get; set; }
        public Guid ConferenceId { get; set; }

        /// <summary>
        /// Used for correlating with the seat assignments.
        /// </summary>
        public Guid? AssignmentsId { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "OrderCode")]
        public string AccessCode { get; set; }
        [Display(ResourceType = typeof (Resources), Name = "RegistrantName")]
        public string RegistrantName { get; set; }
        [Display(ResourceType = typeof (Resources), Name = "RegistrantEmail")]
        public string RegistrantEmail { get; set; }
        [Display(ResourceType = typeof (Resources), Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

    
        public OrderStatus Status { get; set; }

        public ICollection<OrderSeat> Seats { get; set; }
    }
}