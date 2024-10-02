using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Interfaces
{
	public interface IGenaricRepository<T> where T : BaseModel
	{
		public Task<IEnumerable<T>> GetAllAsync();
		public Task<T> GetByIdAsync(int id);
		public Task<int> CreateAsync(T t);
		public Task<int> UpdateAsync(T t);
		public Task<int> DeleteAsync(T t);
	}
}
