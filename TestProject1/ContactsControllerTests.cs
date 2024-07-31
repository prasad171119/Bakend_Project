
using WebApplication1.Controllers;
using ContactsApi.Models;
using WebApplication1.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace TestProject1
{
    public class ContactsControllerTests
    {
        private readonly ContactsController _controller;
        private readonly Mock<ContactService> _serviceMock;

        public ContactsControllerTests()
        {
            _serviceMock = new Mock<ContactService>();
            _controller = new ContactsController(_serviceMock.Object);
        }

        [Fact]
        public void GetContacts_ReturnsOkResult_WithContacts()
        {
            
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };
            _serviceMock.Setup(s => s.GetAllContacts()).Returns(contacts);

           
            var result = _controller.GetContacts();

         
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Contact>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void GetContact_ReturnsNotFound_WhenContactDoesNotExist()
        {
            _serviceMock.Setup(s => s.GetContactById(1)).Returns((Contact)null);

            
            var result = _controller.GetContact(1);

            
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetContact_ReturnsOkResult_WithContact()
        {
           
            var contact = new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            _serviceMock.Setup(s => s.GetContactById(1)).Returns(contact);

           
            var result = _controller.GetContact(1);

           
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Contact>(okResult.Value);
            Assert.Equal("John", returnValue.FirstName);
        }

        [Fact]
        public void CreateContact_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
           
            _controller.ModelState.AddModelError("FirstName", "Required");

            var contact = new Contact { LastName = "Doe", Email = "john.doe@example.com" };

            
            var result = _controller.CreateContact(contact);

            
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateContact_ReturnsCreatedAtActionResult_WhenContactIsValid()
        {
            
            var contact = new Contact { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            
            var result = _controller.CreateContact(contact);

            
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetContact", createdAtActionResult.ActionName);
        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            
            var contact = new Contact { Id = 2, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            
            var result = _controller.UpdateContact(1, contact);

           
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeleteContact_ReturnsNotFound_WhenContactDoesNotExist()
        {
          
            _serviceMock.Setup(s => s.GetContactById(1)).Returns((Contact)null);

          
            var result = _controller.DeleteContact(1);

           
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteContact_ReturnsNoContent_WhenContactIsDeleted()
        {
          
            var contact = new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            _serviceMock.Setup(s => s.GetContactById(1)).Returns(contact);

            
            var result = _controller.DeleteContact(1);

            
            Assert.IsType<NoContentResult>(result);
        }
    }
}
