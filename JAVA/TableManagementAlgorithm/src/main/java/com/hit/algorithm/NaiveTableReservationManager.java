package com.hit.algorithm;

import com.hit.model.Customer;
import com.hit.model.Restaurant;
import com.hit.model.Table;
import com.hit.model.TableStatus;

import java.util.*;

public class NaiveTableReservationManager implements IAlgoTableReservationSystem {

    private Restaurant restaurant;
    private Map<Integer, String> tableToCustomerMap; // tableId -> customerName
    private Queue<Customer> waitlistQueue;

    public NaiveTableReservationManager(Restaurant restaurant) {
        this.restaurant = restaurant;
        this.tableToCustomerMap = new HashMap<>();
        this.waitlistQueue = new LinkedList<>();

        // Initialize the map: mark all tables as available (null = free)
        for (Integer tableId : restaurant.getTables().keySet()) {
            tableToCustomerMap.put(tableId, null);
        }
    }

    @Override
    public int reserveTable(Customer customer) {
        // פשוט מחפש שולחן פנוי שמתאים לכמות האורחים, ללא שום חישוב מתוחכם
        for (Map.Entry<Integer, Table> entry : restaurant.getTables().entrySet()) {
            int tableId = entry.getKey();
            Table table = entry.getValue();

            // שולחן פנוי והקיבולת שלו מתאימה למספר האורחים
            if (tableToCustomerMap.get(tableId) == null && table.getCapacity() >= customer.getNumberOfGuests()) {
                // עדכון הסטטוס ל RESERVED
                restaurant.updateTableStatus(tableId, TableStatus.RESERVED);
                tableToCustomerMap.put(tableId, customer.getName());
                System.out.println("Reserved table " + tableId + " for " + customer.getName());
                return tableId;
            }
        }

        // אם לא נמצא שולחן, נוסיף את הלקוח לרשימת המתנה
        joinWaitlist(customer);
        return -1; // -1 means added to waitlist
    }

    @Override
    public boolean cancelReservation(String customerName) {
        for (Map.Entry<Integer, String> entry : tableToCustomerMap.entrySet()) {
            if (customerName.equals(entry.getValue())) {
                int tableId = entry.getKey();
                tableToCustomerMap.put(tableId, null);
                System.out.println("Reservation cancelled for " + customerName + " on table " + tableId);

                // עדכון הסטטוס ל- AVAILABLE
                restaurant.updateTableStatus(tableId, TableStatus.AVAILABLE);

                // מנסים להקצות שולחן ללקוח הראשון ברשימת ההמתנה
                if (!waitlistQueue.isEmpty()) {
                    Customer nextInLine = waitlistQueue.poll();
                    reserveTable(nextInLine); // מנסה להזמין שוב ללקוח הראשון
                }

                return true;
            }
        }
        return false;
    }

    @Override
    public void joinWaitlist(Customer customer) {
        // מנגנון המתנה פשוט עם שיקול לא להעמיס יותר מידי על מערכת ההזמנות
        if (waitlistQueue.size() < 5) { // לא נרצה יותר מדי לקוחות בתור
            waitlistQueue.add(customer);
            System.out.println(customer.getName() + " was added to the waitlist.");
        } else {
            System.out.println(customer.getName() + " couldn't be added to the waitlist. The waitlist is full.");
        }
    }

    public List<String> getCurrentWaitlist() {
        List<String> names = new ArrayList<>();
        for (Customer c : waitlistQueue) {
            names.add(c.getName());
        }
        return names;
    }
}
