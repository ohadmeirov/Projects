package com.hit.dao;

import com.hit.model.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;
import static org.junit.jupiter.api.Assertions.*;

public class MyDMSQLiteImplTest {
    private MyDMSQLiteImpl dao;
    private Customer testCustomer;
    private Restaurant testRestaurant;
    private Table testTable;
    private TimeSlot testTimeSlot;
    private DayOfTheWeek testDay;

    @BeforeEach
    public void setUp() {
        dao = new MyDMSQLiteImpl();
        testCustomer = new Customer("Test Customer", 2, DayOfTheWeek.MONDAY, LocalTime.of(13, 0));
        List<Table> tables = new ArrayList<>();
        testRestaurant = new Restaurant("Test Restaurant", tables, 8, 22);
        testTable = new Table(1, 4);
        testTimeSlot = new TimeSlot(DayOfTheWeek.MONDAY, LocalTime.of(13, 0));
        testDay = DayOfTheWeek.MONDAY;
    }

    @Test
    public void testSaveAndDeleteReservation() {
        // Save reservation
        boolean saveResult = dao.saveReservation(testCustomer, testRestaurant, testTable, testTimeSlot, testDay);
        assertTrue(saveResult);

        // Verify reservation exists
        List<TimeSlot> customerReservations = dao.getCustomerReservations(testCustomer);
        assertNotNull(customerReservations);
        assertTrue(customerReservations.contains(testTimeSlot));

        // Delete reservation
        boolean deleteResult = dao.deleteReservation(testCustomer, testRestaurant, testTimeSlot, testDay);
        assertTrue(deleteResult);

        // Verify reservation is deleted
        customerReservations = dao.getCustomerReservations(testCustomer);
        assertTrue(customerReservations.isEmpty());
    }

    @Test
    public void testGetRestaurantReservations() {
        // Save reservation
        dao.saveReservation(testCustomer, testRestaurant, testTable, testTimeSlot, testDay);

        // Get restaurant reservations
        List<TimeSlot> restaurantReservations = dao.getRestaurantReservations(testRestaurant, testDay);
        assertNotNull(restaurantReservations);
        assertTrue(restaurantReservations.contains(testTimeSlot));

        // Clean up
        dao.deleteReservation(testCustomer, testRestaurant, testTimeSlot, testDay);
    }

    @Test
    public void testAddToWaitList() {
        // Add to wait list
        boolean result = dao.addToWaitList(testCustomer, testRestaurant, testTimeSlot, testDay);
        assertTrue(result);
    }
} 