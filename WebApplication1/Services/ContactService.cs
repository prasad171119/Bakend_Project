using WebApplication1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ContactService
    {
        private readonly string _filePath = "C:/BackEnd_Test_Project/Project/WebApplication1/contacts.json";

        public List<Contact> GetAllContacts()
        {
            return ReadContactsFromFile();
        }

        public Contact GetContactById(int id)
        {
            var contacts = ReadContactsFromFile();
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public void CreateContact(Contact contact)
        {
            var contacts = ReadContactsFromFile();
            contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
            contacts.Add(contact);
            WriteContactsToFile(contacts);
        }

        public void UpdateContact(Contact updatedContact)
        {
            var contacts = ReadContactsFromFile();
            var contact = contacts.FirstOrDefault(c => c.Id == updatedContact.Id);
            if (contact != null)
            {
                contact.FirstName = updatedContact.FirstName;
                contact.LastName = updatedContact.LastName;
                contact.Email = updatedContact.Email;
                WriteContactsToFile(contacts);
            }
        }

        public void DeleteContact(int id)
        {
            var contacts = ReadContactsFromFile();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contacts.Remove(contact);
                WriteContactsToFile(contacts);
            }
        }

        private List<Contact> ReadContactsFromFile()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Contact>>(json);
        }

        private void WriteContactsToFile(List<Contact> contacts)
        {
            var json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
