package com.hit.service;

import com.hit.model.Restaurant;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class RestaurantServiceTest {
    private RestaurantService restaurantService;
    private Restaurant restaurant;

    @BeforeEach
    public void setUp() {
        restaurantService = new RestaurantService();
        restaurant = restaurantService.getDefaultRestaurant();
    }

    @Test
    public void testGetDefaultRestaurant() {
        assertNotNull(restaurant);
        assertEquals("Default Restaurant", restaurantService.getName(restaurant));
        assertEquals(8, restaurantService.getOpeningHour(restaurant));
        assertEquals(22, restaurantService.getClosingHour(restaurant));
    }

    @Test
    public void testSetName() {
        String newName = "New Restaurant Name";
        restaurantService.setName(restaurant, newName);
        assertEquals(newName, restaurantService.getName(restaurant));
    }

    @Test
    public void testSetHours() {
        int newOpeningHour = 9;
        int newClosingHour = 23;
        restaurantService.setOpeningHour(restaurant, newOpeningHour);
        restaurantService.setClosingHour(restaurant, newClosingHour);
        assertEquals(newOpeningHour, restaurantService.getOpeningHour(restaurant));
        assertEquals(newClosingHour, restaurantService.getClosingHour(restaurant));
    }
} 