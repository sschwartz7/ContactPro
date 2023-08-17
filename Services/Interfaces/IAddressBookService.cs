namespace ContactPro.Services.Interfaces
{
    public interface IAddressBookService
    {
        public Task AddCategoriesToContactAsync(List<int> categoryIds, int contactId);
        public Task RemoveCategoriesFromContactAsync(int contactId);
    }
}
