using WaterDelivery.Contracts.Addresses.Dtos;
using WaterDelivery.Contracts.Bills.Dtos;
using WaterDelivery.Contracts.CustomersAddresses.Dtos;
using WaterDelivery.Contracts.Enums;
using WaterDelivery.Contracts.Orders.Dtos;

namespace WaterDelivery.UI.Components.PlaceOrder;

public class PlaceOrderUseCase
{
    private readonly HttpClient _backendClient;

    public PlaceOrderUseCase(HttpClient backendClient)
    {
        _backendClient = backendClient;
    }

    public async Task<PlaceOrderResult> ExecuteAsync(PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        var addressDto = new CreateAddressDto
        {
            Street = request.Street,
            HouseNumber = request.HouseNumber,
            AptNumber = request.AptNumber,
            City = request.City,
            State = request.State
        };
        var addressId = await CreateAddressAsync(addressDto, cancellationToken);
        
        //ToDo get addresses by customerId and add current address if its not there
        var customerAddressesDto = new CreateCustomerAddressesDto
        {
            CustomerId = request.CustomerId,
            Addresses = new List<AddressDto>()
        };
        await CreateCustomerAddressAsync(customerAddressesDto, cancellationToken);

        var orderDto = new CreateOrderDto
        {
            CustomerId = request.CustomerId,
            Items = request.Items
        };
        var orderId = await CreateOrderAsync(orderDto, cancellationToken);

        var orderForBill = new OrderDto();//ToDo get order by orderId for bill
        var billDto = new CreateBillDto
        {
            Order = orderForBill,
            CreationDate = DateTime.UtcNow,
            PaymentDate = DateTime.UtcNow.AddDays(7),
            Status = BillStatus.WaitForPayment
        };
        var billId = await CreateBillAsync(billDto, cancellationToken);

        return new PlaceOrderResult
        {
            AddressId = addressId,
            OrderId = orderId,
            BillId = billId
        };
    }

    private async Task<Guid> CreateAddressAsync(CreateAddressDto dto, CancellationToken cancellationToken)
    {
        var response = await _backendClient.PostAsJsonAsync(
            "/api/waterDelivery/address",
            dto,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to save address.");

        var id = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);

        if (id == Guid.Empty)
            throw new InvalidOperationException("Backend returned an invalid address id.");

        return id;
    }

    private async Task CreateCustomerAddressAsync(CreateCustomerAddressesDto dto, CancellationToken cancellationToken)
    {
        var response = await _backendClient.PostAsJsonAsync(
            "/api/waterDelivery/customerAddress",
            dto,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to link address to customer.");
    }

    private async Task<Guid> CreateOrderAsync(CreateOrderDto dto, CancellationToken cancellationToken)
    {
        var response = await _backendClient.PostAsJsonAsync(
            "/api/waterDelivery/order",
            dto,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to create order.");

        var id = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);

        if (id == Guid.Empty)
            throw new InvalidOperationException("Backend returned an invalid order id.");

        return id;
    }

    private async Task<Guid> CreateBillAsync(CreateBillDto dto, CancellationToken cancellationToken)
    {
        var response = await _backendClient.PostAsJsonAsync(
            "/api/waterDelivery/bill",
            dto,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to create bill.");

        var id = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);

        if (id == Guid.Empty)
            throw new InvalidOperationException("Backend returned an invalid bill id.");

        return id;
    }
}