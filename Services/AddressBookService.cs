using ContactPro.Services.Interfaces;
using ContactPro.Data;
using ContactPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContactPro.Services
{

    public class AddressBookService : IAddressBookService
    {
        private readonly ApplicationDbContext _context;
        public AddressBookService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddCategoriesToContactAsync(List<int> categoryIds, int contactId)
        {
            try
            {
                //get the contact to add categories to
                Contact? contact = await _context.Contacts.Include(c => c.Categories).FirstOrDefaultAsync(cont => cont.Id == contactId);
                if (contact == null) return;
                // loop through each category ID
                foreach (int categoryId in categoryIds)
                {
                    // make sure each category exists
                    Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                    // if it does add the contact to that category
                    if (category != null)
                    {
                        contact.Categories.Add(category);
                    }
                }
                //when done save changes to database
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveCategoriesFromContactAsync(int contactId)
        {
            try
            {
                //find the contact by ID 
                Contact? contact = await _context.Contacts.Include(c => c.Categories).FirstOrDefaultAsync(cont => cont.Id == contactId);

                if (contact != null)
                {
                    //remove all of their categories
                    contact.Categories.Clear();
                    //save those changes to the database
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
