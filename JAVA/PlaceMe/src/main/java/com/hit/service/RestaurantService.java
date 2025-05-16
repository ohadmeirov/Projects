package com.hit.service;

// import com.hit.dao.IDao;

import com.hit.model.Restaurant;
import com.hit.model.Table;

import java.util.ArrayList;
import java.util.List;

public class RestaurantService {

    private List<Table> tables;

    // Returns a default Restaurant. In a real implementation, this could fetch from DB.
    public Restaurant getDefaultRestaurant() {
        // For now, return a stub Restaurant (with dummy values)
        tables = new ArrayList<>();
        return new Restaurant("Default Restaurant", tables, 8, 22);
    }

    // SET operations
    public void setName(Restaurant restaurant, String newName) {
        restaurant.setName(newName);
    }

    public void setOpeningHour(Restaurant restaurant, int openingHour) {
        restaurant.setOpeningHour(openingHour);
    }

    public void setClosingHour(Restaurant restaurant, int closingHour) {
        restaurant.setClosingHour(closingHour);
    }

    // GET operations
    public String getName(Restaurant restaurant) {
        return restaurant.getName();
    }

    public int getOpeningHour(Restaurant restaurant) { return restaurant.getOpeningHour(); }

    public int getClosingHour(Restaurant restaurant) {
        return restaurant.getClosingHour();
    }
} 