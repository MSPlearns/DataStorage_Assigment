﻿using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;

public class CustomerFactory : ICustomerFactory
{
    public Customer FromForm(CreateCustomerForm form)
    {
        return new Customer
        {
            CustomerName = form.CustomerName,
        };
    }

    public Customer FromForm(UpdateCustomerForm form) {

        return new Customer
        {
            CustomerName = form.CustomerName,
        };

    }
}
