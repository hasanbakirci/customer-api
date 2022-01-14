using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repositories.Settings;
using CustomerService.Model;
using CustomerService.Repositories.Interfaces;
using MongoDB.Driver;

namespace CustomerService.Repositories
{
    public class CustomerRepository :  ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _customer;
        public CustomerRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.Server);
            var database = client.GetDatabase(settings.Database);
            _customer = database.GetCollection<Customer>(settings.Collection);
        }

        public async Task<ICollection<Customer>> GetAll()
        {
            return await _customer.Find(s => true).ToListAsync();
        }

        public async Task<Customer> Get(Guid id)
        {
            var result = await _customer.FindAsync(c => c.Id == id && c.isDeleted == false);
            return result.FirstOrDefault();
        }

        public async Task<Guid> Create(Customer customer)
        {
            customer.CreatedAt = DateTime.UtcNow;
            customer.isDeleted = false;
            await _customer.InsertOneAsync(customer);
            return customer.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _customer.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount == 1 ? true : false;
        }

        
        public async Task<bool> Update(Guid id, Customer entity)
        {
            var result = await _customer.FindOneAndReplaceAsync(c => c.Id == id, entity);
            return result is not null ? true : false;
        }


        public async Task<IEnumerable<Customer>> GetAllBySoftDeleted()
        {
            return await _customer.Find(s => s.isDeleted == true).ToListAsync();
        }

        public async Task<bool> CreateMany(List<Customer> customers)
        {
            foreach(var customer in customers){
                customer.CreatedAt = DateTime.Now;
                customer.isDeleted = false;
            }
            await _customer.InsertManyAsync(customers);
            
            return customers.Count() > 0 ? true : false;
        }

        public async Task<bool> SoftDelete(Guid id)
        {
            var filter = Builders<Customer>.Filter.Eq(f => f.Id,id);
            var update = Builders<Customer>.Update
                .Set(c => c.isDeleted, true)
                .Set(c => c.UpdatedAt, DateTime.Now);
            var result = await _customer.UpdateOneAsync(filter,update);
            return result.ModifiedCount > 0 ? true : false;
        }

        public async Task<bool> Validate(Guid id)
        {
            var result = await _customer.FindAsync(c => c.Id == id);
            return result.Any();   
        }

        public async Task<IEnumerable<Customer>> Page(int from, int size)
        {
            var customers = await _customer.Find(c => c.isDeleted == false).Skip(from).Limit(size).ToListAsync();
            return customers;
        }

        public async Task<long> TotalCountOfCustomer()
        {
            var totalCount = await _customer.CountDocumentsAsync(c => c.isDeleted == false);
            return totalCount;
        }
    }
}