using GigHub.Models;
using System;

namespace GigHub.Dtos
{
    public class NotificationDto
    {
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public string Venue { get; private set; }
        public DateTime? OrignalDateTime { get; private set; }
        public GigDto Gig { get; private set; }
    }
}