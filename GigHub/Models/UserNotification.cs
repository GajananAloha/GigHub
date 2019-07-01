using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class UserNotification
    {
        [Key]
        [Column(Order =1)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; set; }

        public Notification Notification { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsRead { get; set; }

        public UserNotification()
        {

        }
        public UserNotification(ApplicationUser user, Notification notification)
        {
            if (user == null || notification == null)
                throw new ArgumentNullException();
            User = user;
            Notification = notification;
        }

        public void Read()
        {
            IsRead = true;
        }
    }
}