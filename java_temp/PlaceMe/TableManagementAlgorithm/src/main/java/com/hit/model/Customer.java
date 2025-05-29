package com.hit.model;

/**
 * מייצג לקוח שמזמין מקום במסעדה.
 */

import java.time.LocalTime;
import java.util.Objects;

public class Customer {
    private String name;
    private int numberOfGuests;
    private DayOfTheWeek dayToCome;
    private LocalTime timeToCome;
    private int reservedTableId;

    public void setName(String name) {
        this.name = name;
    }

    public void setNumberOfGuests(int numberOfGuests) {
        this.numberOfGuests = numberOfGuests;
    }

    public void setDayToCome(DayOfTheWeek dayToCome) {
        this.dayToCome = dayToCome;
    }

    public void setTimeToCome(LocalTime timeToCome) {
        this.timeToCome = timeToCome;
    }

    public Customer(String name, int numberOfGuests, DayOfTheWeek dayToCome, LocalTime timeToCome) {
        this.name = name;
        this.numberOfGuests = numberOfGuests;
        this.dayToCome = dayToCome;
        this.timeToCome = timeToCome;
        this.reservedTableId = -1; // לא הוזמן שולחן
    }

    public String getName() {
        return name;
    }

    public int getNumberOfGuests() {
        return numberOfGuests;
    }

    public DayOfTheWeek getDayToCome() {
        return dayToCome;
    }

    public LocalTime getTimeToCome() {
        return timeToCome;
    }

    public int getReservedTableId() {
        return reservedTableId;
    }

    public void setReservedTableId(int reservedTableId) {
        this.reservedTableId = reservedTableId;
    }

    @Override
    public String toString() {
        return "Customer{name='" + name + "', guests=" + numberOfGuests +
                ", time=" + timeToCome + ", day=" + dayToCome + "}";
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Customer customer = (Customer) o;
        return Objects.equals(name, customer.name) &&
               Objects.equals(timeToCome, customer.timeToCome) &&
               Objects.equals(dayToCome, customer.dayToCome);
    }

    @Override
    public int hashCode() {
        return Objects.hash(name, timeToCome, dayToCome);
    }
}

