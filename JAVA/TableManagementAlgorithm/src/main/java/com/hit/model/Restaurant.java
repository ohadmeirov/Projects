package com.hit.model;

import java.util.HashMap;
import java.util.Map;

import java.time.LocalTime;
import java.util.*;

public class Restaurant {
    private String name;
    private Map<Integer, Table> tables; // tableId -> Table
    private TimeSlotManager timeSlotManager; // מנהל השעות

    public Restaurant(String name) {
        this.name = name;
        this.tables = new HashMap<>();
        this.timeSlotManager = new TimeSlotManager();  // יוצר את מנהל השעות
    }

    public void addTable(int id, int capacity) {
        if (!tables.containsKey(id)) {
            tables.put(id, new Table(id, capacity));
        }
    }

    public Table getTableById(int id) {
        return tables.get(id);
    }

    public Map<Integer, Table> getTables() {
        return tables;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    // עדכון סטטוס השולחן
    public void updateTableStatus(int tableId, TableStatus status) {
        Table table = tables.get(tableId);
        if (table != null) {
            table.setStatus(status);  // עדכון הסטטוס של השולחן
        }
    }

    // פונקציה שמחפשת שולחן פנוי על פי יום ושעה
    public boolean hasAvailableTableAt(DayOfTheWeek day, LocalTime time, int partySize) {
        Table availableTable = timeSlotManager.findAvailableTable(tables.values(), partySize, day, time);
        return availableTable != null;
    }

    // פונקציה להזמין שולחן או להציע זמן חלופי
    public void tryReserveOrSuggestAlternative(DayOfTheWeek day, LocalTime requestedTime, Customer customer) {
        Table availableTable = timeSlotManager.findAvailableTable(tables.values(), customer.getNumberOfGuests(), day, requestedTime);
        if (availableTable != null) {
            timeSlotManager.reserveTable(day, requestedTime, availableTable.getId());
            System.out.println("שולחן הוזמן לשעה " + requestedTime + " עבור " + customer.getName());
        } else {
            Table alternativeTable = timeSlotManager.tryFindAlternativeSlotOrAddToWaitlist(
                    new ArrayList<>(tables.values()), customer.getNumberOfGuests(), customer, day, requestedTime);
            if (alternativeTable != null) {
                System.out.println("הזמנה לשעה " + requestedTime + " לא הצליחה. שולחן הוזמן לשעה חלופין.");
            } else {
                System.out.println("אין שולחנות פנויים, הוספת ל רשימת המתנה.");
            }
        }
    }

    // פונקציה להוסיף זמן חדש למנהל השעות
    public void addTimeSlot(DayOfTheWeek day, LocalTime time) {
        timeSlotManager.addTimeSlot(day, time);
    }
}



