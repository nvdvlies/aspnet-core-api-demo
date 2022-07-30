namespace Demo.Application.Customers.Commands.UpdateCustomer.Dtos;

public class UpdateCustomerLocationDto
{
    public string DisplayName { get; set; }
    public string StreetName { get; set; }
    public string HouseNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string CountryCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
