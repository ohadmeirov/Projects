package com.hit.service;

import com.hit.model.Customer;
import com.hit.model.DayOfTheWeek;
import com.hit.model.TimeSlot;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.time.LocalTime;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

public class CustomerServiceTest {
    private CustomerService customerService;
    private Customer customer;

    @BeforeEach
    public void setUp() {
        customerService = new CustomerService();
        customer = customerService.getDefaultCustomer();
    }

    @Test
    public void testGetDefaultCustomer() {
        assertNotNull(customer);
        assertEquals("Default Customer", customerService.getName(customer));
        assertEquals(2, customerService.getNumberOfGuests(customer));
        assertEquals(DayOfTheWeek.MONDAY, customerService.getDayToCome(customer));
        assertEquals(LocalTime.of(13, 0), customerService.getTimeToCome(customer));
    }

    @Test
    public void testSetName() {
        String newName = "New Customer Name";
        customerService.setName(customer, newName);
        assertEquals(newName, customerService.getName(customer));
    }

    @Test
    public void testSetNumberOfGuests() {
        int newNumberOfGuests = 4;
        customerService.setNumberOfGuests(customer, newNumberOfGuests);
        assertEquals(newNumberOfGuests, customerService.getNumberOfGuests(customer));
    }

    @Test
    public void testSetDayToCome() {
        DayOfTheWeek newDay = DayOfTheWeek.TUESDAY;
        customerService.setDayToCome(customer, newDay);
        assertEquals(newDay, customerService.getDayToCome(customer));
    }

    @Test
    public void testSetTimeToCome() {
        TimeSlot newTimeSlot = new TimeSlot(DayOfTheWeek.MONDAY, LocalTime.of(14, 0));
        customerService.setTimeToCome(customer, newTimeSlot);
        assertEquals(newTimeSlot, customerService.getTimeSlot(customer));
    }
} 