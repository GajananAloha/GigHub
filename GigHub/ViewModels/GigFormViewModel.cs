using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public string Heading { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
        public int Id { get; set; }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(String.Format("{0} {1}", Date, Time));
        }
        public string Action
        {
            get
            {
                Expression<Func<GigsController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<GigsController, ActionResult>> Create = (c => c.Create(this));
                var action = Id != 0 ? update : Create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }
    }
}