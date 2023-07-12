using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uof;
        private readonly IBasketRepository _basketRepo;
        
        public OrderService(IUnitOfWork uof,IBasketRepository basketRepo)
        {
            _basketRepo=basketRepo;
            _uof = uof;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket= await _basketRepo.GetBasketAsync(basketId);

            var items=new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var productItem = await _uof.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id,productItem.Name,productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered,productItem.Price,item.Quantity);
                items.Add(orderItem);
            } 

            var deliveryMethod=await _uof.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var subtotal=items.Sum(item=>item.Price * item.Quantity);
            //create order
            var order=new Order(items,buyerEmail,shippingAddress,deliveryMethod,subtotal);
            
            _uof.Repository<Order>().Add(order);

            var result=await _uof.Complete();

            if(result <= 0) return null;

            await _basketRepo.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _uof.Repository<DeliveryMethod>().ListAllAync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id,buyerEmail);
            return  await _uof.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await _uof.Repository<Order>().ListAsync(spec);
        }
    }
}