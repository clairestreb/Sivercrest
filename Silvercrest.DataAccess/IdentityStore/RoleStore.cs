using Microsoft.AspNet.Identity;
using Silvercrest.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.IdentityStore
{
    public class RoleStore : IQueryableRoleStore<AspNetRole, string>
    {
        private readonly SLVR_DEVEntities db;

        public RoleStore(SLVR_DEVEntities db)
        {
            this.db = db;
        }

        public IQueryable<AspNetRole> Roles
        {
            get { return this.db.AspNetRoles; }
        }

        public virtual Task CreateAsync(AspNetRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.AspNetRoles.Add(role);
            return this.db.SaveChangesAsync();
        }

        public Task DeleteAsync(AspNetRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.AspNetRoles.Remove(role);
            return this.db.SaveChangesAsync();
        }

        public Task<AspNetRole> FindByIdAsync(string roleId)
        {
            return this.db.AspNetRoles.FindAsync(new[] { roleId });
        }

        public Task<AspNetRole> FindByNameAsync(string roleName)
        {
            return this.db.AspNetRoles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public Task UpdateAsync(AspNetRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Entry(role).State = EntityState.Modified;
            return this.db.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.db != null)
            {
                this.db.Dispose();
            }
        }
    }
}
