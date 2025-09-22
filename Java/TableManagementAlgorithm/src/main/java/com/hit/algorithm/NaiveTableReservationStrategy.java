package com.hit.algorithm;

import com.hit.model.*;

import java.util.*;
import java.time.LocalTime;

public class NaiveTableReservationStrategy implements IAlgoTableReservationStrategy {
    @Override
    public Table reserveTable(Restaurant restaurant, Customer customer) {
        // מחפש שולחן פנוי כלשהו שמתאים למספר האורחים
        Optional<Table> availableTable = restaurant.getTables().stream()
            .filter(table -> !restaurant.getTimeSlotManager().isTableReserved(
                restaurant, table.getId(), customer.getDayToCome(), customer.getTimeToCome()))
            .filter(table -> table.getCapacity() >= customer.getNumberOfGuests())
            .findFirst();

        if (availableTable.isPresent()) {
            Table table = availableTable.get();
            Table reservedTable = restaurant.getTimeSlotManager().reserveTable(
                restaurant, table, customer, customer.getDayToCome(), customer.getTimeToCome());
            if (reservedTable != null) {
                customer.setReservedTableId(reservedTable.getId());
                return reservedTable;
            }
        }

        return null;
    }

    @Override
    public boolean cancelReservation(Restaurant restaurant, Customer customer) {
        return restaurant.getTimeSlotManager().cancelReservation(restaurant, customer);
    }

    @Override
    public boolean joinToWaitlist(Restaurant restaurant, Customer customer) {
        // מחפש שולחן מתאים
        Optional<Table> suitableTable = restaurant.getTables().stream()
            .filter(table -> table.getCapacity() >= customer.getNumberOfGuests())
            .findFirst();

        if (suitableTable.isPresent()) {
            Table table = suitableTable.get();
            restaurant.getTimeSlotManager().addToWaitlist(
                restaurant, customer, table, customer.getDayToCome(), customer.getTimeToCome());
            return true;
        }

        return false;
    }

    public Set<Table> getAvailableTables(Restaurant restaurant, DayOfTheWeek day, LocalTime time) {
        // בדיקות תקינות
        if (time.isBefore(LocalTime.of(restaurant.getOpeningHour(), 0)) || 
            time.isAfter(LocalTime.of(restaurant.getClosingHour(), 0))) {
            return new HashSet<>();
        }

        // מקבל את כל השולחנות הפנויים
        return restaurant.getTimeSlotManager().getAvailableTables(restaurant, day, time);
    }
} 