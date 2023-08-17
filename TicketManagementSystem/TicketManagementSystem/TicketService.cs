using EmailService;
using System;
using System.IO;
using System.Text.Json;
using TicketManagementSystem.CustomExceptions;
using TicketManagementSystem.Interfaces;
using TicketManagementSystem.Models;
using TicketManagementSystem.Repositories;

namespace TicketManagementSystem
{
    public class TicketService
    {
        private readonly IUserRepository _userRepository;

        public TicketService() : this(new UserRepository()) { }

        public TicketService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int CreateTicket(string title, Priority priority, string assignedTo, string description, DateTime creationDateTime, bool isPayingCustomer)
        {
            User accountManager = null;
            var priorityRaised = false;
            double price = 0;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                throw new InvalidTicketException("Title or description were null or empty");
            }


            var user = _userRepository.GetUser(assignedTo ?? string.Empty);


            if (user is null)
            {
                throw new UnknownUserException($"User {assignedTo} not found");
            }

            if (creationDateTime < DateTime.UtcNow.AddHours(-1))
            {
                priority = RaisePriorityLevel(priority, ref priorityRaised);
            }

            if (!priorityRaised &&
                (
                title.Contains("Crash") ||
                title.Contains("Important") ||
                title.Contains("Failure")
                ))
            {
                priority = RaisePriorityLevel(priority, ref priorityRaised);
            }

            if (priority == Priority.High)
            {
                var emailService = new EmailServiceProxy();
                emailService.SendEmailToAdministrator(title, assignedTo);
            }

            if (isPayingCustomer)
            {
                // Only paid customers have an account manager.

                accountManager = _userRepository.GetAccountManager();


                price = priority == Priority.High ? 100 : 50;
            }

            var ticket = new Ticket()
            {
                Title = title,
                AssignedUser = user,
                Priority = priority,
                Description = description,
                Created = creationDateTime,
                PriceDollars = price,
                AccountManager = accountManager
            };

            return TicketRepository.CreateTicket(ticket);
        }

        public void AssignTicket(int id, string username)
        {

            var user = _userRepository.GetUser(username ?? string.Empty);

            if (user is null)
            {
                throw new UnknownUserException("User not found");
            }

            var ticket = TicketRepository.GetTicket(id);

            if (ticket is null)
            {
                throw new ApplicationException($"No ticket found for id {id}");
            }

            ticket.AssignedUser = user;

            TicketRepository.UpdateTicket(ticket);
        }

        private static Priority RaisePriorityLevel(Priority currentPriority, ref bool priorityRaised)
        {
            switch (currentPriority)
            {
                case Priority.Low:
                    priorityRaised = true;
                    return Priority.Medium;
                case Priority.Medium:
                    priorityRaised = true;
                    return Priority.High;
                default:
                    return currentPriority;
            }
        }

        private void WriteTicketToFile(Ticket ticket)
        {
            var ticketJson = JsonSerializer.Serialize(ticket);
            File.WriteAllText(Path.Combine(Path.GetTempPath(), $"ticket_{ticket.Id}.json"), ticketJson);
        }
    }

    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
