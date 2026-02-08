namespace WaterDelivery.Backend.Core.Entities;

public class Address
{
    private const int STREET_MAX_LENGTH = 70;
    private const int APTNUMBER_MAX_LENGTH = 10;
    private const int HOUSENUMBER_MAX_LENGTH = 10;
    private const int CITY_MAX_LENGTH = 30;
    private const int STATE_MAX_LENGTH = 30;

    public Guid Id { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string? AptNumber { get; set; }

    public string City { get; set; }

    public string State { get; set; }
    
    public bool IsDeleted { get; set; }

    public Address(string street, string houseNumber, string? aptNumber, string city, string state)
    {
        Id = Guid.NewGuid();
        SetStreet(street);
        SetHouseNumber(houseNumber);
        SetAptNumber(aptNumber);
        SetCity(city);
        SetState(state);
    }

    public Address(Guid id, string street, string houseNumber, string? aptNumber, string city, string state, bool isDeleted)
    {
        Id = id;
        SetStreet(street);
        SetHouseNumber(houseNumber);
        SetAptNumber(aptNumber);
        SetCity(city);
        SetState(state);
        IsDeleted = isDeleted;
    }

    public void SetStreet(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > STREET_MAX_LENGTH)
        {
            throw new ArgumentException("Invalid Street", nameof(Street));
        }

        Street = input;
    }

    public void SetHouseNumber(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > HOUSENUMBER_MAX_LENGTH)
        {
            throw new ArgumentException("Invalid House number", nameof(HouseNumber));
        }

        HouseNumber = input;
    }


    public void SetAptNumber(string input)
    {
        if (input.Length > APTNUMBER_MAX_LENGTH)
        {
            throw new ArgumentException("Invalid Apt number", nameof(AptNumber));
        }

        AptNumber = input;
    }

    public void SetCity(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > CITY_MAX_LENGTH)
        {
            throw new ArgumentException("Invalid City", nameof(City));
        }

        City = input;
    }

    public void SetState(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > STATE_MAX_LENGTH)
        {
            throw new ArgumentException("Invalid State", nameof(State));
        }

        State = input;
    }
}