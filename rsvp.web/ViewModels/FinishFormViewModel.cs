﻿using DonorGateway.Domain;
using System;


namespace rsvp.web.ViewModels
{
    public class FinishFormViewModel
    {
        public int EventId { get; set; }

        public string CurrentDate { get; private set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string InsideSalutation { get; set; }
        public string EmailSalutation { get; set; }
        public bool IsAttending { get; set; }
        public bool IsWaiting { get; set; }

        public Template Template { get; set; }

        public FinishFormViewModel()
        {
            CurrentDate = DateTime.Today.ToString("d");
        }

    }
}