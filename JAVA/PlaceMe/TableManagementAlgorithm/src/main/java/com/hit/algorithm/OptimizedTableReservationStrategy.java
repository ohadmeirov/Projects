package com.hit.algorithm;

import com.hit.model.*;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Optional;
import java.util.Set;

public class OptimizedTableReservationStrategy implements IAlgoTableReservationStrategy {

    public OptimizedTableReservationStrategy() {
    }

    @Override
    public Table reserveTable(Restaurant restaurant, Customer customer) {
        // בדיקות תקינות
        if (customer.getNumberOfGuests() <= 0) {
            return null;
        }
        
        LocalTime time = customer.getTimeToCome();
        if (time.isBefore(LocalTime.of(restaurant.getOpeningHour(), 0)) || 
            time.isAfter(LocalTime.of(restaurant.getClosingHour(), 0))) {
            return null;
        }

        // מחפש שולחן פנוי שמתאים למספר האורחים
        Table exactFitTable = restaurant.getTimeSlotManager().findAvailableTable(
            restaurant, customer.getNumberOfGuests(), customer.getDayToCome(), customer.getTimeToCome());
        
        if (exactFitTable != null && exactFitTable.getCapacity() >= customer.getNumberOfGuests()) {
            Table reservedTable = restaurant.getTimeSlotManager().reserveTable(
                restaurant, exactFitTable, customer, customer.getDayToCome(), customer.getTimeToCome());
            if (reservedTable != null) {
                customer.setReservedTableId(reservedTable.getId());
                return reservedTable;
            }
        }

        // מחפש שולחן פנוי עם קיבולת גדולה ב-1
        Table plusOneTable = restaurant.getTimeSlotManager().findAvailableTable(
            restaurant, customer.getNumberOfGuests() + 1, customer.getDayToCome(), customer.getTimeToCome());
        
        if (plusOneTable != null && plusOneTable.getCapacity() >= customer.getNumberOfGuests() + 1) {
            Table reservedTable = restaurant.getTimeSlotManager().reserveTable(
                restaurant, plusOneTable, customer, customer.getDayToCome(), customer.getTimeToCome());
            if (reservedTable != null) {
                customer.setReservedTableId(reservedTable.getId());
                return reservedTable;
            }
        }

        // מחפש שולחן פנוי עם קיבולת גדולה ב-2
        Table plusTwoTable = restaurant.getTimeSlotManager().findAvailableTable(
            restaurant, customer.getNumberOfGuests() + 2, customer.getDayToCome(), customer.getTimeToCome());
        
        if (plusTwoTable != null && plusTwoTable.getCapacity() >= customer.getNumberOfGuests() + 2) {
            Table reservedTable = restaurant.getTimeSlotManager().reserveTable(
                restaurant, plusTwoTable, customer, customer.getDayToCome(), customer.getTimeToCome());
            if (reservedTable != null) {
                customer.setReservedTableId(reservedTable.getId());
                return reservedTable;
            }
        }

        // אם לא נמצא שולחן מתאים, מחזיר null כדי שהקורא יוכל להציג זמנים חלופיים
        return null;
    }

    @Override
    public boolean cancelReservation(Restaurant restaurant, Customer customer) {
        return restaurant.getTimeSlotManager().cancelReservation(restaurant, customer);
    }

    
    public List<TimeSlot> findAlternativeTimeSlots(Restaurant restaurant, Customer customer) {
        List<TimeSlot> alternatives = new ArrayList<>();
        DayOfTheWeek day = customer.getDayToCome();
        LocalTime requestedTime = customer.getTimeToCome();
        
        // מחפש זמנים חלופיים בטווח של שעה וחצי לפני ואחרי
        LocalTime startTime = requestedTime.minusHours(1).minusMinutes(30);
        LocalTime endTime = requestedTime.plusHours(1).plusMinutes(30);
        
        // מחלק את הטווח ל-30 דקות
        int totalMinutes = (int) java.time.Duration.between(startTime, endTime).toMinutes();
        int slots = totalMinutes / 30;
        
        for (int i = 0; i <= slots; i++) {
            final LocalTime time = startTime.plusMinutes(i * 30);
            if (time.isAfter(LocalTime.of(restaurant.getOpeningHour(), 0)) && 
                time.isBefore(LocalTime.of(restaurant.getClosingHour(), 0))) {
                
                // בודק אם יש שולחן פנוי בזמן הנוכחי
                boolean hasAvailableTable = restaurant.getTables().stream()
                    .anyMatch(table -> !restaurant.getTimeSlotManager().isTableReserved(
                        restaurant, table.getId(), day, time));
                
                if (hasAvailableTable) {
                    alternatives.add(new TimeSlot(day, time));
                }
            }
        }
        
        return alternatives;
    }

    @Override
    public boolean joinToWaitlist(Restaurant restaurant, Customer customer) {
        // מחפש שולחן מתאים
        Optional<Table> suitableTable = restaurant.getTables().stream()
            .filter(table -> table.getCapacity() >= customer.getNumberOfGuests())
            .min(Comparator.comparingInt(table -> table.getCapacity() - customer.getNumberOfGuests()));

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
        Set<Table> availableTables = restaurant.getTimeSlotManager().getAvailableTables(restaurant, day, time);
        
        // ממיין את השולחנות לפי קיבולת
        return availableTables.stream()
            .sorted(Comparator.comparingInt(Table::getCapacity))
            .collect(java.util.stream.Collectors.toCollection(LinkedHashSet::new));
    }
} 