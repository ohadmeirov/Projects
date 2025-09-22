package com.hit.controller;

import com.hit.server.Request;
import com.hit.server.Response;
import com.hit.service.RestaurantService;
import com.hit.model.Restaurant;

public class RestaurantController implements Controller {
    private final RestaurantService restaurantService;

    public RestaurantController(RestaurantService restaurantService) {
        this.restaurantService = restaurantService;
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
                return handleGetRestaurant(request);
            case "update":
                return handleUpdateRestaurant(request);
            default:
                return new Response<>(false, "Unknown operation: " + operation, null);
        }
    }

    private Response<?> handleGetRestaurant(Request<?> request) {
        try {
            Restaurant restaurant = restaurantService.getDefaultRestaurant();
            return new Response<>(true, "Restaurant retrieved successfully", restaurant);
        } catch (Exception e) {
            return new Response<>(false, "Failed to get restaurant: " + e.getMessage(), null);
        }
    }

    private Response<?> handleUpdateRestaurant(Request<?> request) {
        try {
            Restaurant restaurant = (Restaurant) request.getBody();
            restaurantService.setName(restaurant, restaurant.getName());
            restaurantService.setOpeningHour(restaurant, restaurant.getOpeningHour());
            restaurantService.setClosingHour(restaurant, restaurant.getClosingHour());
            return new Response<>(true, "Restaurant updated successfully", restaurant);
        } catch (Exception e) {
            return new Response<>(false, "Failed to update restaurant: " + e.getMessage(), null);
        }
    }
} 