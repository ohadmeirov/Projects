package com.hit.model;

import java.time.LocalTime;
import java.util.*;

public class TimeSlotManager {

    private final Map<DayOfTheWeek, Map<LocalTime, Set<Integer>>> schedule = new HashMap<>();
    private final Map<Integer, ReservationSlot> tableReservationMap = new HashMap<>();

    public TimeSlotManager() {
        for (DayOfTheWeek day : DayOfTheWeek.values()) {
            schedule.put(day, new HashMap<>());
        }
    }

    // פונקציה לעדכון השעות סביב השולחן שנבחר
    private void updateSurroundingTimeSlots(DayOfTheWeek day, LocalTime time, int tableId, boolean isReserved) {
        LocalTime[] surroundingTimes = new LocalTime[]{
                time.minusMinutes(90), time.minusMinutes(60), time.minusMinutes(30),
                time.plusMinutes(30), time.plusMinutes(60), time.plusMinutes(90)
        };

        for (LocalTime surroundingTime : surroundingTimes) {
            Set<Integer> reservedTables = schedule.get(day).computeIfAbsent(surroundingTime, k -> new HashSet<>());
            if (isReserved) {
                reservedTables.add(tableId);  // אם השולחן נתפס, הוסף אותו לרשימת השולחנות התפוסים
            } else {
                reservedTables.remove(tableId); // אם השולחן שוחרר, הסר אותו
            }
        }
    }

    public Map<DayOfTheWeek, Map<LocalTime, Set<Integer>>> getFullSchedule() {
        return schedule;
    }

    public Table tryFindAlternativeSlotOrAddToWaitlist(List<Table> tables, int numGuests, Customer customer, DayOfTheWeek day, LocalTime requestedTime) {
        int[] timeOffsets = {-90, -60, -30, 30, 60, 90}; // דקות קדימה ואחורה

        // ננסה שעות קרובות
        for (int offset : timeOffsets) {
            LocalTime altTime = requestedTime.plusMinutes(offset);
            if (altTime.isBefore(LocalTime.of(10, 0)) || altTime.isAfter(LocalTime.of(22, 0)))
                continue; // שעות לא סבירות למסעדה

            Table availableTable = findAvailableTable(tables, numGuests, day, altTime);
            if (availableTable != null) {
                System.out.println("No table at " + requestedTime + ", but we found one at " + altTime + " on table " + availableTable.getId());
                reserveTable(day, altTime, availableTable.getId());
                return availableTable;
            }
        }

        // אם לא מצאנו שום דבר - נוסיף לרשימת המתנה
        System.out.println("No available tables for " + customer.getName() + ". Added to waitlist.");
        for (Table t : tables) {
            if (t.getCapacity() >= numGuests) {
                t.addToWaitlist(customer); // ניתן לשפר ולהוסיף לפי עומס
                break;
            }
        }

        return null;
    }

    public void addTimeSlot(DayOfTheWeek day, LocalTime time) {
        // אם הזמן לא קיים בלוח הזמנים, ניצור אותו
        schedule.computeIfAbsent(day, k -> new HashMap<>())
                .computeIfAbsent(time, k -> new HashSet<>());
    }

    public Table findAvailableTable(Collection<Table> tables, int numberOfGuests, DayOfTheWeek day, LocalTime time) {
        Set<Integer> reservedTables = schedule.get(day).getOrDefault(time, new HashSet<>());
        return tables.stream()
                .filter(t -> t.getCapacity() >= numberOfGuests)
                .filter(t -> !reservedTables.contains(t.getId()))
                .min(Comparator.comparingInt(Table::getCapacity))
                .orElse(null);
    }

    public void reserveTable(DayOfTheWeek day, LocalTime time, int tableId) {
        schedule.get(day).computeIfAbsent(time, k -> new HashSet<>()).add(tableId);
        tableReservationMap.put(tableId, new ReservationSlot(day, time));
        updateSurroundingTimeSlots(day, time, tableId, true); // עדכון השעות סביב השולחן
    }

    public void cancelReservation(int tableId) {
        ReservationSlot slot = tableReservationMap.remove(tableId);
        if (slot != null) {
            Set<Integer> reserved = schedule.get(slot.day).get(slot.time);
            if (reserved != null) {
                reserved.remove(tableId);
            }
            updateSurroundingTimeSlots(slot.day, slot.time, tableId, false); // עדכון השעות סביב השולחן כששולחן מתפנה
        }
    }

    // פונקציה שמחזירה את ה-TableScheduleManager
    public TableScheduleManager getTableScheduleManagerForTimeSlot(DayOfTheWeek day, LocalTime time) {
        Set<Integer> reservedTables = schedule.get(day).getOrDefault(time, new HashSet<>());
        return new TableScheduleManager(reservedTables);  // מעביר את הסט של השולחנות המוזמנים
    }

    private static class ReservationSlot {
        DayOfTheWeek day;
        LocalTime time;

        ReservationSlot(DayOfTheWeek day, LocalTime time) {
            this.day = day;
            this.time = time;
        }
    }
}


