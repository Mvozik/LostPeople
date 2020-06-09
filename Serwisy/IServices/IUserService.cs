using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanko3.Serwisy.IServices
{
	public interface IUserService
	{
		public Task<bool> Login(string username, string password);
		public Task<bool> Register(string username, string password);

	}
}
