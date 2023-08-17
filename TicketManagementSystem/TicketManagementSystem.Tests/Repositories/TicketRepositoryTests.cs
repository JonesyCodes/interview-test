using NUnit.Framework;
using TicketManagementSystem.Models;
using TicketManagementSystem.Repositories;

namespace TicketManagementSystem.Tests.Repositories
{
    [TestFixture]
    public class TicketRepositoryTests
    {
        private Ticket[] _tickets = new Ticket[] {
            new Ticket() { Title = "ticket1" },
            new Ticket() { Title = "ticket2" },
            new Ticket() { Title = "ticket3" }
        };

        [Test]
        [Ignore("Ignore test")]
        public void CreateTicket_Adds_Ticket_And_Returns_New_Ticket_With_Incremented_Id() 
        {
            var firstTicketId = TicketRepository.CreateTicket(_tickets[0]);
            var secondTicketId = TicketRepository.CreateTicket(_tickets[1]);
            var thirdTicketId = TicketRepository.CreateTicket(_tickets[2]);

            Assert.That(firstTicketId, Is.EqualTo(1));
            Assert.That(secondTicketId, Is.EqualTo(2));
            Assert.That(thirdTicketId, Is.EqualTo(3));
        }

        [Test]
        [Ignore("Ignore test")]
        public void GetTicket_Returns_a_Ticket_For_A_Valid_Id()
        {
            var returnedTicket = TicketRepository.GetTicket(2);

            Assert.That(returnedTicket.Title, Is.EqualTo(_tickets[1].Title));
        }

        [Test]
        [Ignore("Ignore test")]
        public void GetTicket_Returns_Null_For_An_Invalid_Id()
        {
            var returnedTicket = TicketRepository.GetTicket(4);

            Assert.IsNull(returnedTicket);
        }

        [Test]
        [Ignore("Ignore test")]
        public void UpdateTicket_Updates_A_Ticket_For_A_Valid_Id()
        {
            var updatedTitle = "updateTicket1";
            var returnedTicket = TicketRepository.GetTicket(1);

            Assert.That(returnedTicket.Title, Is.EqualTo(_tickets[0].Title));

            var updateTicket = new Ticket { Title = updatedTitle, Id = 1 };

            TicketRepository.UpdateTicket(updateTicket);

            returnedTicket = TicketRepository.GetTicket(1);

            Assert.That(returnedTicket.Title, Is.EqualTo(updatedTitle));
        }
    }
}
