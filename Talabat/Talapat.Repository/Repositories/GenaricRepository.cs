using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Interfaces;
using Talabat.Core.Models;
using Talabat.Repository.Connections;

namespace Talabat.Repository.Repositories
{
	public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseModel
	{
		private readonly TalabatDBContext _talabatDBContext;

		public GenaricRepository(TalabatDBContext talabatDBContext)
		{
			_talabatDBContext = talabatDBContext;
		}
		public async Task<int> CreateAsync(T t)
		{
			_talabatDBContext.Set<T>().Add(t);
			return await _talabatDBContext.SaveChangesAsync(); 
		}

		public async Task<int> DeleteAsync(T t)
		{
			_talabatDBContext.Set<T>().Remove(t);
			return await _talabatDBContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			if(typeof(T)==typeof(Product))
			{
				return (IEnumerable<T>) await _talabatDBContext.Set<Product>().Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
			}
			return await _talabatDBContext.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			if (typeof(T) == typeof(Product))
			{
				return await _talabatDBContext.Set<Product>().Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync() as T;
			}
			return await _talabatDBContext.Set<T>().FindAsync(id);
		}

		public async Task<int> UpdateAsync(T t)
		{
			_talabatDBContext.Set<T>().Update(t);
			return await _talabatDBContext.SaveChangesAsync();
		}
	}
}
