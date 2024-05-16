using SE161838.ProductManagement.Repo.Interface;
using SE161838.ProductManagement.Repo.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SE161838.ProductManagement.Repo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FstoreDbContext _context;
        private IGenericRepository<Category> _categoryRepository;
        private IGenericRepository<Member> _memberRepository;
        private IGenericRepository<Order> _orderRepository;
        private IGenericRepository<OrderDetail> _orderDetailRepository;
        private IGenericRepository<Product> _productRepository;

        public UnitOfWork(FstoreDbContext fstoreDbContext)
        {
            _context = fstoreDbContext;
        }

        public IGenericRepository<Category> CategorysRepository
        {
            get
            {
                return _categoryRepository ??= new GenericRepository<Category>(_context);

            }
        }

        public IGenericRepository<Member> MembersRepository
        {
            get
            {
                return _memberRepository ??= new GenericRepository<Member>(_context);

            }
        }

        public IGenericRepository<Order> OrdersRepository
        {
            get
            {
                return _orderRepository ??= new GenericRepository<Order>(_context);

            }
        }

        public IGenericRepository<OrderDetail> OrderDetailsRepository
        {
            get
            {
                return _orderDetailRepository ??= new GenericRepository<OrderDetail>(_context);

            }
        }

        public IGenericRepository<Product> ProductsRepository
        {
            get
            {
                return _productRepository ??= new GenericRepository<Product>(_context);

            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }       
    }
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            if (left == null)
                return right;
            var invokedExpr = Expression.Invoke(right, left.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left.Body, invokedExpr), left.Parameters);
        }
    }
}
