package com.hit.service;

import com.hit.model.Customer;
import com.hit.model.DayOfTheWeek;
import com.hit.model.TimeSlot;

import java.time.LocalTime;

public class CustomerService {
    // Business logic for customers
    ///•	כל Service מקבל Dao ו־Algorithm (Strategy) ב־constructor
    /// •	Service מבצע את כל הלוגיקה העסקית.

    public Customer getDefaultCustomer() {
        // For now, return a stub Customer with default values
        return new Customer("Default Customer", 2, DayOfTheWeek.MONDAY, LocalTime.of(13, 0));
    }

    // SET operations
    public void setName(Customer customer, String newName) {
        customer.setName(newName);
    }

    public void setNumberOfGuests(Customer customer, int numberOfGuests) {
        customer.setNumberOfGuests(numberOfGuests);
    }

    public void setDayToCome(Customer customer, DayOfTheWeek day) {
        customer.setDayToCome(day);
    }

    public void setTimeToCome(Customer customer, TimeSlot timeSlot) {
        customer.setDayToCome(timeSlot.getDay());
        customer.setTimeToCome(timeSlot.getTime());
    }

    // GET operations
    public String getName(Customer customer) {
        return customer.getName();
    }

    public int getNumberOfGuests(Customer customer) {
        return customer.getNumberOfGuests();
    }

    public DayOfTheWeek getDayToCome(Customer customer) {
        return customer.getDayToCome();
    }

    public LocalTime getTimeToCome(Customer customer) {
        return customer.getTimeToCome();
    }

    public TimeSlot getTimeSlot(Customer customer) {
        return new TimeSlot(customer.getDayToCome(), customer.getTimeToCome());
    }
} 