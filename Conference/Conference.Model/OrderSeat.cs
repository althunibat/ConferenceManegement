using System;

namespace ConferenceManagementSystem.Conference.Model
{
    public class OrderSeat {
        public OrderSeat(Guid orderId, int position, Guid seatTypeId)
            : this() {
            OrderId = orderId;
            Position = position;
            SeatTypeId = seatTypeId;
            }

        protected OrderSeat() {
            // Complex type properties can never be 
            // null.
            Attendee = new Attendee();
        }

        public int Position { get; set; }
        public Guid OrderId { get; set; }
        public Attendee Attendee { get; set; }

        public Guid SeatTypeId { get; set; }
    }
}