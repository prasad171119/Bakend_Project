

using WebApplication1.Models;
using WebApplication1.Services;
using Moq;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Newtonsoft.Json;
using WebApplication1;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.DotNet.Scaffolding.Shared;

namespace TestProject1
{
    public class ContactServiceTests
    {
        private readonly ContactService _service;
        private readonly Mock<IFileSystem> _fileSystemMock;

        public ContactServiceTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
           //_service = new ContactService(_fileSystemMock.Object);
        }

        [Fact]
        public void GetAllContacts_ReturnsAllContacts()
        {
            
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new Contact { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
            };
            _fileSystemMock.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(contacts));

          
            var result = _service.GetAllContacts();

      
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Jane", result[1].FirstName);
        }

        [Fact]
        public void GetContactById_ReturnsCorrectContact()
        {
          
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };
            _fileSystemMock.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(contacts));

           
            var result = _service.GetContactById(1);

     
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public void CreateContact_AddsNewContact()
        {
            
            var contacts = new List<Contact>();
            _fileSystemMock.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(contacts));
            _fileSystemMock.Setup(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));

            var newContact = new Contact { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

       
            _service.CreateContact(newContact);

            _fileSystemMock.Verify(fs => fs.WriteAllText(It.IsAny<string>(), It.Is<string>(s => s.Contains("John"))), Times.Once);
        }

        [Fact]
        public void UpdateContact_UpdatesExistingContact()
        {
            
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };
            _fileSystemMock.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(contacts));
            _fileSystemMock.Setup(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));

            var updatedContact = new Contact { Id = 1, FirstName = "Johnny", LastName = "Doe", Email = "john.doe@example.com" };

            
            _service.UpdateContact(updatedContact);

       
            _fileSystemMock.Verify(fs => fs.WriteAllText(It.IsAny<string>(), It.Is<string>(s => s.Contains("Johnny"))), Times.Once);
        }

        [Fact]
        public void DeleteContact_RemovesContact()
        {
            
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };
            _fileSystemMock.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(contacts));
            _fileSystemMock.Setup(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));

            _service.DeleteContact(1);

            _fileSystemMock.Verify(fs => fs.WriteAllText(It.IsAny<string>(), It.Is<string>(s => !s.Contains("John"))), Times.Once);
        }
    }
}
