package com.hit.algorithm;

import com.hit.model.*;

import java.time.LocalTime;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.Map;
import java.util.Set;

public class OptimizedTableReservationManager implements IAlgoTableReservationSystem {

    private final Restaurant restaurant;
    private final TimeSlotManager timeSlotManager;
    private final WaitlistManager waitlistManager;
    private final Map<String, Integer> customerToTableMap;

    public OptimizedTableReservationManager(Restaurant restaurant) {
        this.restaurant = restaurant;
        this.timeSlotManager = new TimeSlotManager();
        this.waitlistManager = new WaitlistManager();
        this.customerToTableMap = new HashMap<>();
    }

    @Override
    public int reserveTable(Customer customer) {
        int numberOfGuests = customer.getNumberOfGuests();
        DayOfTheWeek day = customer.getDayToCome(); // הגדרת היום
        LocalTime time = customer.getTimeToCome(); // הגדרת הזמן

        // חיפוש שולחן פנוי עם ניהול שעות
        TableScheduleManager tableScheduleManager = timeSlotManager.getTableScheduleManagerForTimeSlot(day, time);
        Table availableTable = tableScheduleManager.findAvailableTable(restaurant.getTables().values(), numberOfGuests);

        if (availableTable != null) {
            // שמירה של השולחן המוזמן
            tableScheduleManager.reserveTable(availableTable.getId(), customer, time); // הוספת הזמן
            customerToTableMap.put(customer.getName(), availableTable.getId());
            return availableTable.getId();
        } else {
            // הוספה לרשימת המתנה אם לא נמצא שולחן פנוי
            waitlistManager.addToWaitlist(customer);
            return -1;
        }
    }

    @Override
    public boolean cancelReservation(String customerName) {
        if (customerToTableMap.containsKey(customerName)) {
            int tableId = customerToTableMap.remove(customerName);
            // שליפת ה-TableScheduleManager עבור הזמן והיום של השולחן
            Customer customer = getCustomerFromMap(customerName); // שליפת הלקוח מהרשימה, עליך להוסיף לוגיקה לכך
            TableScheduleManager tableScheduleManager = timeSlotManager.getTableScheduleManagerForTimeSlot(
                    customer.getDayToCome(), // השם ביום הלקוח ביקש
                    customer.getTimeToCome() // הזמן שהלקוח ביקש
            );
            tableScheduleManager.cancelReservation(tableId, customer.getTimeToCome()); // הוספת הזמן
            return true;
        } else {
            return waitlistManager.removeFromWaitlist(customerName);
        }
    }

    @Override
    public void joinWaitlist(Customer customer) {
        waitlistManager.addToWaitlist(customer);
    }

    private Customer getCustomerFromMap(String customerName) {
        // קודם נחפש ברשימת ההמתנה הראשית
        for (Customer customer : waitlistManager.getAll()) {
            if (customer.getName().equals(customerName)) {
                return customer;
            }
        }

        // נעבור על כל הימים והזמנים בשבוע
        for (DayOfTheWeek day : DayOfTheWeek.values()) {
            Map<LocalTime, Set<Integer>> timeSlots = timeSlotManager.getFullSchedule().get(day);
            if (timeSlots == null) continue;

            for (LocalTime time : timeSlots.keySet()) {
                TableScheduleManager tableSchedule = timeSlotManager.getTableScheduleManagerForTimeSlot(day, time);
                Map<Integer, LinkedList<Customer>> waitlists = tableSchedule.getWaitlists();

                for (LinkedList<Customer> queue : waitlists.values()) {
                    for (Customer customer : queue) {
                        if (customer.getName().equals(customerName)) {
                            return customer;
                        }
                    }
                }
            }
        }

        return null; // לא נמצא
    }
}


