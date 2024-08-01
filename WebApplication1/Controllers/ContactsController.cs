using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;


namespace ContactsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;
        private ContactService @object;

        public ContactsController()
        {
            _contactService = new ContactService();
        }

        public ContactsController(ContactService @object)
        {
            this.@object = @object;
        }

        [HttpGet]
        [EnableCors("")]
        public ActionResult<List<Contact>> GetContacts()
        {
            return Ok(_contactService.GetAllContacts());
        }

        [HttpGet("{id}")]
        [EnableCors("")]
        public ActionResult<Contact> GetContact(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        [EnableCors("")]
        public ActionResult CreateContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _contactService.CreateContact(contact);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        [EnableCors("")]
        public ActionResult UpdateContact(int id, [FromBody] Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingContact = _contactService.GetContactById(id);
            if (existingContact == null)
            {
                return NotFound();
            }

            _contactService.UpdateContact(contact);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EnableCors("")]
        public ActionResult DeleteContact(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null)
            {
                return NotFound();
            }

            _contactService.DeleteContact(id);
            return NoContent();
        }
    }
}
