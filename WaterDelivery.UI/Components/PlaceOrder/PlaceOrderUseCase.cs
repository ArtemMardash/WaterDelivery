using WaterDelivery.Contracts.Bills.Dtos;
using WaterDelivery.Contracts.CustomersAddresses.Dtos;
using WaterDelivery.Contracts.Enums;
using WaterDelivery.Contracts.Orders.Dtos;

namespace WaterDelivery.UI.Components.PlaceOrder;

/// <summary>
/// Places an order from the checkout page:
///   1. Resolves the delivery address. A newly typed address is saved into the customer's
///      address book (de-duplicated); an existing saved address is used as-is.
///   2. Creates the order.
///   3. Creates a bill in WaitForPayment status.
/// The delivery itself is created later, when the customer confirms payment on the order-status page.
/// </summary>
public class PlaceOrderUseCase
{
    private readonly HttpClient _backendClient;

    public PlaceOrderUseCase(HttpClient backendClient)
    {
        _backendClient = backendClient;
    }

    public async Task<PlaceOrderResult> ExecuteAsync(PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        var addressId = await ResolveAddressAsync(request, cancellationToken);

        var createOrderDto = new CreateOrderDto
        {
            CustomerId = request.CustomerId,
            Items = request.Items
        };
        var orderId = await CreateOrderAsync(createOrderDto, cancellationToken);

        var orderForBill = new OrderDto
        {
            Id = orderId,
            CustomerId = request.CustomerId,
            Items = request.Items
        };
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

    /// <summary>
    /// Ensures the address exists in the customer's address book and returns its id.
    /// </summary>
    private async Task<Guid> ResolveAddressAsync(PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        // Existing saved address already has a valid id -> nothing to persist.
        if (!request.IsNewAddress && request.Address.Id != Guid.Empty)
        {
            return request.Address.Id;
        }

        var dto = new AddAddressToCustomerDto
        {
            CustomerId = request.CustomerId,
            Address = request.Address
        };

        var response = await _backendClient.PostAsJsonAsync(
            "/api/waterDelivery/customerAddress/add-address",
            dto,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to save the delivery address.");

        var id = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);
        if (id == Guid.Empty)
            throw new InvalidOperationException("Backend returned an invalid address id.");

        return id;
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
