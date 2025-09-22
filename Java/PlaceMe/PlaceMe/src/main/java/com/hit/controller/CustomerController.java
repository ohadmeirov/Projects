package com.hit.controller;

import com.hit.model.Customer;
import com.hit.server.Request;
import com.hit.server.Response;
import com.hit.service.CustomerService;

public class CustomerController implements Controller {
    private final CustomerService customerService;

    public CustomerController(CustomerService customerService) {
        this.customerService = customerService;
    }

    @Override
    public Response<?> handleRequest(String action, Request<?> request) {
        String[] parts = action.split("/");
        if (parts.length < 2) {
            return new Response<>(false, "Invalid action format", null);
        }

        String operation = parts[1];
        switch (operation) {
            case "get":
                return handleGetCustomer(request);
            case "update":
                return handleUpdateCustomer(request);
            default:
                return new Response<>(false, "Unknown operation: " + operation, null);
        }
    }

    private Response<?> handleGetCustomer(Request<?> request) {
        try {
            Customer customer = (Customer) request.getBody();
            return new Response<>(true, "Customer retrieved successfully", customer);
        } catch (Exception e) {
            return new Response<>(false, "Failed to get customer: " + e.getMessage(), null);
        }
    }

    private Response<?> handleUpdateCustomer(Request<?> request) {
        try {
            Customer customer = (Customer) request.getBody();
            // Add customer update logic here
            return new Response<>(true, "Customer updated successfully", customer);
        } catch (Exception e) {
            return new Response<>(false, "Failed to update customer: " + e.getMessage(), null);
        }
    }
} 