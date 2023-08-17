using Moq;
using NUnit.Framework;
using System;
using TicketManagementSystem.CustomExceptions;
using TicketManagementSystem.Interfaces;
using TicketManagementSystem.Models;
using TicketManagementSystem.Services;

namespace TicketManagementSystem.Tests
{
    [TestFixture]
    public class TicketServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly User _user = new User()
        {
            Username = "TestUser",
            FirstName = "Test",
            LastName = "User"
        };

        [Test]
        public void CreateTicket_Returns_An_Id_For_Valid_Parameters()
        {
            _mockUserRepository.Setup(x => x.GetUser(_user.Username)).Returns(_user);

            var ticketService = new TicketService(_mockUserRepository.Object);

            var id = ticketService.CreateTicket(
                title: "title",
                priority: Priority.Low,
                assignedTo: _user.Username,
                description: "description",
                creationDateTime: DateTime.Now,
                isPayingCustomer: false);

            Assert.That(id, Is.EqualTo(1));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CreateTicket_Throws_An_InvalidTicketException_ForInvalid_Titles(string title)
        {
            var ticketService = new TicketService(_mockUserRepository.Object);

            Assert.That(() => ticketService.CreateTicket(
                title: title,
                priority: Priority.Low,
                assignedTo: "assignedTo",
                description: "description",
                creationDateTime: DateTime.Now,
                isPayingCustomer: true
                ), 
                Throws.TypeOf<InvalidTicketException>());
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CreateTicket_Returns_UnknownUserException_For_Invalid_Assigned_User(string assignedUser)
        {
            _mockUserRepository.Setup(x => x.GetUser(assignedUser)).Returns((User)null);

            var ticketService = new TicketService(_mockUserRepository.Object);

            Assert.That(() => ticketService.CreateTicket(
                title: "title",
                priority: Priority.Low,
                assignedTo: assignedUser,
                description: "description",
                creationDateTime: DateTime.Now,
                isPayingCustomer: true
                ),
                Throws.TypeOf<UnknownUserException>());
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void AssignTicket_Throws_An_InvalidTicketException_ForInvalid_Username(string username)
        {
            _mockUserRepository.Setup(x => x.GetUser(username)).Returns((User)null);

            var ticketService = new TicketService(_mockUserRepository.Object);

            Assert.That(() => ticketService.AssignTicket(
                id: 1,
                username: username),
                Throws.TypeOf<UnknownUserException>());
        }
    }
}
