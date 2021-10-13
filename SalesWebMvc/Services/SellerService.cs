using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services {
    public class SellerService {

        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context) {
            _context = context; 
        }

        public async Task<List<Seller>> FindAll() {
            return await _context.Seller.ToListAsync();

        }

        public async Task Insert(Seller obj) {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindById(int id) {
            //using Include to do join Department and Seller. Call this eager loading
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task Remove(int id) {

            try {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            } catch (DbUpdateException e) {
                throw new IntegrityException("Can not delete seller because she/he has sales.");
            }
        }

        public async Task Update(Seller obj) {

            bool hasAnyValue = await _context.Seller.AnyAsync(x => x.Id == obj.Id);

            if (!hasAnyValue) {
                throw new NotFoundException("Id not found");
            }

            try {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e){
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
