package com.hit.model;

import java.time.LocalTime;
import java.util.*;

public class TableScheduleManager {

    private final Map<Integer, TableStatus> tableStatusMap;  // שולחן -> סטטוס
    private final Map<Integer, LinkedList<Customer>> waitlists; // שולחן -> רשימת ממתינים
    private final Map<LocalTime, Set<Integer>> reservedTimes;  // זמן -> שולחנות תפוסים

    public TableScheduleManager(Set<Integer> reservedTables) {
        this.tableStatusMap = new HashMap<>();
        this.waitlists = new HashMap<>();
        this.reservedTimes = new HashMap<>();
    }

    public TableStatus getTableStatus(int tableId) {
        return tableStatusMap.getOrDefault(tableId, TableStatus.AVAILABLE);
    }

    public Map<Integer, LinkedList<Customer>> getWaitlists() {
        return waitlists;
    }

    public boolean reserveTable(int tableId, Customer customer, LocalTime time) {
        // שמירה אם השולחן פנוי בזמן הנתון
        if (getTableStatus(tableId) == TableStatus.AVAILABLE && isTimeAvailable(time)) {
            tableStatusMap.put(tableId, TableStatus.RESERVED);
            // הוספת השולחן לזמן תפוס
            reservedTimes.computeIfAbsent(time, k -> new HashSet<>()).add(tableId);
            // עדכון השעות לפני ואחריו
            updateAffectedTimes(time, tableId);
            return true;
        }
        return false;
    }

    public void cancelReservation(int tableId, LocalTime time) {
        // הסרת השולחן מהסטטוס
        tableStatusMap.put(tableId, TableStatus.AVAILABLE);
        reservedTimes.getOrDefault(time, new HashSet<>()).remove(tableId);

        // עדכון השעות לפני ואחריו
        updateAffectedTimes(time, tableId);

        // הוספת הלקוח הראשון לרשימת ההמתנה
        LinkedList<Customer> waitlist = waitlists.get(tableId);
        if (waitlist != null && !waitlist.isEmpty()) {
            Customer nextCustomer = waitlist.poll();
            reserveTable(tableId, nextCustomer, time);
        }
    }

    public void addToWaitlist(int tableId, Customer customer) {
        waitlists.computeIfAbsent(tableId, k -> new LinkedList<>()).add(customer);
    }

    public Table findAvailableTable(Collection<Table> tables, int numberOfGuests) {
        for (Table table : tables) {
            if (table.getCapacity() >= numberOfGuests && getTableStatus(table.getId()) == TableStatus.AVAILABLE) {
                return table;
            }
        }
        return null;
    }

    private boolean isTimeAvailable(LocalTime time) {
        // בודק אם יש שולחנות תפוסים בשעה הזאת
        return reservedTimes.getOrDefault(time, new HashSet<>()).isEmpty();
    }

    private void updateAffectedTimes(LocalTime time, int tableId) {
        // עדכון השעות לפני ואחריו (חצי שעה, שעה ושעה וחצי)
        updateTimeSlot(time.minusMinutes(30), tableId);
        updateTimeSlot(time.minusHours(1), tableId);
        updateTimeSlot(time.minusMinutes(90), tableId);

        updateTimeSlot(time.plusMinutes(30), tableId);
        updateTimeSlot(time.plusHours(1), tableId);
        updateTimeSlot(time.plusMinutes(90), tableId);
    }

    private void updateTimeSlot(LocalTime time, int tableId) {
        // אם השולחן תפוס או פנוי, עדכון הסטטוס של הזמן
        Set<Integer> tablesAtTime = reservedTimes.computeIfAbsent(time, k -> new HashSet<>());
        // אם השולחן תפוס, הוספה שלו לזמן (לא משנה אם הוא פנוי או תפוס)
        tablesAtTime.add(tableId);
    }
}



