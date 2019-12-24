using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SigmaTestTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SigmaTestTask
{
    public class AuthRepository : IDisposable
    {
        private OrderContext _ctx;

        private UserManager<SigmaUser> _userManager;

        public AuthRepository()
        {
            _ctx = new OrderContext();
            _userManager = new UserManager<SigmaUser>(new UserStore<SigmaUser>(_ctx));
        }

        public bool ValidateUser(string username, string password)
        {
            return true;
        }


        public async Task<SigmaUser> FindUser(string userName, string password)
        {
            SigmaUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}