﻿using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AccountService :
        IAccountService
    {

        private readonly IApplicationDbContext _applicationDbContext;

        public AccountService(
            IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _applicationDbContext.Account
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Account.FindAsync(id);
        }

        public async Task<int> RegisterAsync(Account account)
        {
            try
            {
                _applicationDbContext.Account
                    .Add(account);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException($"Account with email {account.Email} is already exist", ex);
            }
        }

        public async Task<int> RemoveAsync(Account entity)
        {
            _applicationDbContext.Account.Remove(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Account>> SearchAsync(AccountFilter filter, int from = 0, int size = 10)
        {
            var query = _applicationDbContext.Account.AsQueryable();

            if(filter.FirstName != null)
                query.Where(x => x.FirstName.Contains(filter.FirstName, StringComparison.OrdinalIgnoreCase));
            if(filter.LastName != null)
                query.Where(x => x.LastName.Contains(filter.LastName, StringComparison.OrdinalIgnoreCase));
            if(filter.Email != null)
                query.Where(x => x.Email.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));

            return await query
                .OrderBy(x => x.Id)
                .Skip(from)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> UpdateAsync(Account entity)
        {
            _applicationDbContext.Account.Update(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }
    }
}
