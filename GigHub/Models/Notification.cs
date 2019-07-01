using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public string Venue { get; private set; }
        public DateTime? OrignalDateTime { get; private set; }

        [Required]
        public Gig Gig { get; private set; }

        public Notification()
        {

        }
        private Notification(Gig gig, NotificationType type)
        {
            if (gig == null)
                throw new ArgumentNullException();
            DateTime = DateTime.Now;
            Gig = gig;
            Type = type;
        }
        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig, NotificationType.GigUpdated);
        }

        public static Notification GigUpdated(Gig newGig, DateTime orignalDateTime, string orignalVenue)
        {
            var notification = new Notification(newGig, NotificationType.GigUpdated);
            notification.OrignalDateTime = orignalDateTime;
            notification.Venue = orignalVenue;
            return notification;
        }
        public static Notification GigCanceled(Gig gig)
        {
            var notification = new Notification(gig, NotificationType.GigCanceled);
            return new Notification(gig, NotificationType.GigCanceled);
        }
    }
}