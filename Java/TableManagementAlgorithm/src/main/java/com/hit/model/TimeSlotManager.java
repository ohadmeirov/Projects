package com.hit.model;


import java.time.LocalTime;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class TimeSlotManager {
    private static final int RESERVATION_DURATION_HOURS = 1;
    private static final int RESERVATION_DURATION_MINUTES = 30;
    
    // מפות מקוננות: מסעדה -> יום -> שעה -> שולחן -> לקוח
    private final Map<Restaurant, Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Customer>>>> reservations;
    // מפות מקוננות: מסעדה -> יום -> שעה -> שולחן -> תור המתנה
    private final Map<Restaurant, Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Queue<Customer>>>>> waitlists;
    private final ReadWriteLock scheduleLock;

    public TimeSlotManager() {
        this.reservations = new ConcurrentHashMap<>();
        this.waitlists = new ConcurrentHashMap<>();
        this.scheduleLock = new ReentrantReadWriteLock();
    }

    public void initializeRestaurant(Restaurant restaurant) {
        scheduleLock.writeLock().lock();
        try {
            // מאתחל מפות עבור המסעדה
            Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Customer>>> restaurantReservations = new ConcurrentHashMap<>();
            Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Queue<Customer>>>> restaurantWaitlists = new ConcurrentHashMap<>();

            // מאתחל לכל יום ושעה
        for (DayOfTheWeek day : DayOfTheWeek.values()) {
                Map<LocalTime, Map<Integer, Customer>> dayReservations = new ConcurrentHashMap<>();
                Map<LocalTime, Map<Integer, Queue<Customer>>> dayWaitlists = new ConcurrentHashMap<>();

                // מאתחל את כל השעות האפשריות
                for (int hour = restaurant.getOpeningHour(); hour < restaurant.getClosingHour(); hour++) {
                    LocalTime time = LocalTime.of(hour, 0);
                    Map<Integer, Customer> timeReservations = new ConcurrentHashMap<>();
                    Map<Integer, Queue<Customer>> timeWaitlists = new ConcurrentHashMap<>();

                    // מאתחל את כל השולחנות של המסעדה
                    for (Table table : restaurant.getTables()) {
                        timeWaitlists.put(table.getId(), new LinkedList<>());
                    }

                    dayReservations.put(time, timeReservations);
                    dayWaitlists.put(time, timeWaitlists);
                }

                restaurantReservations.put(day, dayReservations);
                restaurantWaitlists.put(day, dayWaitlists);
            }

            reservations.put(restaurant, restaurantReservations);
            waitlists.put(restaurant, restaurantWaitlists);
        } finally {
            scheduleLock.writeLock().unlock();
        }
    }

    public boolean isTableReserved(Restaurant restaurant, int tableId, DayOfTheWeek day, LocalTime time) {
        scheduleLock.readLock().lock();
        try {
            Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Customer>>> restaurantReservations = reservations.get(restaurant);
            if (restaurantReservations == null) {
                initializeRestaurant(restaurant);
                restaurantReservations = reservations.get(restaurant);
            }
            Map<LocalTime, Map<Integer, Customer>> dayReservations = restaurantReservations.get(day);
            if (dayReservations == null) {
                return false;
            }
            Map<Integer, Customer> timeReservations = dayReservations.get(time);
            if (timeReservations == null) {
                return false;
            }
            return timeReservations.containsKey(tableId);
        } finally {
            scheduleLock.readLock().unlock();
        }
    }

    public Table reserveTable(Restaurant restaurant, Table table, Customer customer, DayOfTheWeek day, LocalTime time) {
        scheduleLock.writeLock().lock();
        try {
            // בודק אם השולחן פנוי בשעות הסמוכות
            Set<Table> availableTables = getAvailableTables(restaurant, day, time);
            boolean found = availableTables.stream().anyMatch(t -> t.getId() == table.getId());
            if (!found) {
                return null;
            }

            // מוודא שהמסעדה מאותחלת
            Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Customer>>> restaurantReservations = reservations.get(restaurant);
            if (restaurantReservations == null) {
                initializeRestaurant(restaurant);
                restaurantReservations = reservations.get(restaurant);
            }

            // שומר את ההזמנה בכל השעות הסמוכות
            LocalTime startTime = time.minusHours(RESERVATION_DURATION_HOURS).minusMinutes(RESERVATION_DURATION_MINUTES);
            LocalTime endTime = time.plusHours(RESERVATION_DURATION_HOURS).plusMinutes(RESERVATION_DURATION_MINUTES);
            LocalTime currentTime = startTime;

            while (!currentTime.isAfter(endTime)) {
                if (currentTime.isAfter(LocalTime.of(restaurant.getOpeningHour(), 0)) && 
                    currentTime.isBefore(LocalTime.of(restaurant.getClosingHour(), 0))) {
                    Map<LocalTime, Map<Integer, Customer>> dayReservations = restaurantReservations.get(day);
                    if (dayReservations == null) {
                        dayReservations = new ConcurrentHashMap<>();
                        restaurantReservations.put(day, dayReservations);
                    }
                    Map<Integer, Customer> timeReservations = dayReservations.get(currentTime);
                    if (timeReservations == null) {
                        timeReservations = new ConcurrentHashMap<>();
                        dayReservations.put(currentTime, timeReservations);
                    }
                    timeReservations.put(table.getId(), customer);
                }
                currentTime = currentTime.plusMinutes(30);
            }

            customer.setReservedTableId(table.getId());
            // ודא שהשולחן שמוזמן הוא זה שמנוהל ע\"י המסעדה
            Table realTable = restaurant.getTables().stream()
                .filter(t -> t.getId() == table.getId())
                .findFirst()
                .orElse(table);
            realTable.reserve();

            return realTable;
        } finally {
            scheduleLock.writeLock().unlock();
        }
    }

    public boolean cancelReservation(Restaurant restaurant, Customer customer) {
        scheduleLock.writeLock().lock();
        try {
            DayOfTheWeek day = customer.getDayToCome();
            LocalTime time = customer.getTimeToCome();
            int tableId = customer.getReservedTableId();

            // מחזיר את השולחן למפה בשעה המבוקשת
            Table table = restaurant.getTables().stream()
                .filter(t -> t.getId() == tableId)
                .findFirst()
                .orElse(null);
            
            if (table == null) {
                return false;
            }

            // מוודא שהמסעדה מאותחלת
            Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Customer>>> restaurantReservations = reservations.get(restaurant);
            if (restaurantReservations == null) {
                return false;
            }

            // מוחק את ההזמנה מכל השעות הסמוכות
            LocalTime startTime = time.minusHours(RESERVATION_DURATION_HOURS).minusMinutes(RESERVATION_DURATION_MINUTES);
            LocalTime endTime = time.plusHours(RESERVATION_DURATION_HOURS).plusMinutes(RESERVATION_DURATION_MINUTES);
            LocalTime currentTime = startTime;

            while (!currentTime.isAfter(endTime)) {
                if (currentTime.isAfter(LocalTime.of(restaurant.getOpeningHour(), 0)) && 
                    currentTime.isBefore(LocalTime.of(restaurant.getClosingHour(), 0))) {
                    Map<LocalTime, Map<Integer, Customer>> dayReservations = restaurantReservations.get(day);
                    if (dayReservations != null) {
                        Map<Integer, Customer> timeReservations = dayReservations.get(currentTime);
                        if (timeReservations != null) {
                            timeReservations.remove(tableId);
                        }
                    }
                }
                currentTime = currentTime.plusMinutes(30);
            }

            customer.setReservedTableId(-1);
            table.release();

            return true;
        } finally {
            scheduleLock.writeLock().unlock();
        }
    }

    public Customer getReservation(Restaurant restaurant, int tableId, DayOfTheWeek day, LocalTime time) {
        scheduleLock.readLock().lock();
        try {
            return reservations.get(restaurant).get(day).get(time).get(tableId);
        } finally {
            scheduleLock.readLock().unlock();
        }
    }

    public Set<Table> getAvailableTables(Restaurant restaurant, DayOfTheWeek day, LocalTime time) {
        scheduleLock.readLock().lock();
        try {
            Set<Table> availableTables = new HashSet<>(restaurant.getTables());
            // Remove tables that are already marked as RESERVED
            Map<DayOfTheWeek, Map<LocalTime, Map<Integer, Customer>>> restaurantReservations = reservations.get(restaurant);
            if (restaurantReservations == null) {
                initializeRestaurant(restaurant);
                restaurantReservations = reservations.get(restaurant);
            }

            // מחשב את טווח הזמנים שצריך לבדוק
            LocalTime startTime = time.minusHours(RESERVATION_DURATION_HOURS).minusMinutes(RESERVATION_DURATION_MINUTES);
            LocalTime endTime = time.plusHours(RESERVATION_DURATION_HOURS).plusMinutes(RESERVATION_DURATION_MINUTES);
            LocalTime currentTime = startTime;

            // בודק את כל הזמנים בטווח
            while (!currentTime.isAfter(endTime)) {
                if (currentTime.isAfter(LocalTime.of(restaurant.getOpeningHour(), 0)) &&
                        currentTime.isBefore(LocalTime.of(restaurant.getClosingHour(), 0))) {
                    Map<LocalTime, Map<Integer, Customer>> dayReservations = restaurantReservations.get(day);
                    if (dayReservations != null) {
                        Map<Integer, Customer> timeReservations = dayReservations.get(currentTime);
                        if (timeReservations != null) {
                            // מסיר את כל השולחנות שכבר מוזמנים
                            for (int tableId : timeReservations.keySet()) {
                                availableTables.removeIf(table -> table.getId() == tableId);
                            }
                        }
                    }
                }
                currentTime = currentTime.plusMinutes(30);
            }

            return availableTables;
        } finally {
            scheduleLock.readLock().unlock();
        }
    }

    public void addToWaitlist(Restaurant restaurant, Customer customer, Table table, DayOfTheWeek day, LocalTime time) {
        scheduleLock.writeLock().lock();
        try {
            Queue<Customer> tableWaitlist = waitlists.get(restaurant).get(day).get(time).get(table.getId());
            tableWaitlist.add(customer);
        } finally {
            scheduleLock.writeLock().unlock();
        }
    }

    public Customer pollNextFromWaitlist(Restaurant restaurant, Table table, DayOfTheWeek day, LocalTime time) {
        scheduleLock.writeLock().lock();
        try {
            Queue<Customer> tableWaitlist = waitlists.get(restaurant).get(day).get(time).get(table.getId());
            return tableWaitlist != null ? tableWaitlist.poll() : null;
        } finally {
            scheduleLock.writeLock().unlock();
        }
    }

    public Queue<Customer> getWaitlist(Restaurant restaurant, Table table, DayOfTheWeek day, LocalTime time) {
        scheduleLock.readLock().lock();
        try {
            return waitlists.get(restaurant).get(day).get(time).get(table.getId());
        } finally {
            scheduleLock.readLock().unlock();
        }
    }

    public List<TimeSlot> findAlternativeTimeSlots(Restaurant restaurant, int numberOfGuests, DayOfTheWeek day, LocalTime time) {
        scheduleLock.readLock().lock();
        try {
            List<TimeSlot> alternatives = new ArrayList<>();
            
            // מחפש זמנים חלופיים באותו יום
            for (int hour = restaurant.getOpeningHour(); hour < restaurant.getClosingHour(); hour++) {
                LocalTime alternativeTime = LocalTime.of(hour, 0);
                if (alternativeTime.equals(time)) continue;

                Table availableTable = findAvailableTable(restaurant, numberOfGuests, day, alternativeTime);
                if (availableTable != null) {
                    alternatives.add(new TimeSlot(availableTable, day, alternativeTime));
                }
            }

            // מחפש זמנים חלופיים ביום הבא
            DayOfTheWeek nextDay = getNextDay(day);
            for (int hour = restaurant.getOpeningHour(); hour < restaurant.getClosingHour(); hour++) {
                LocalTime alternativeTime = LocalTime.of(hour, 0);
                Table availableTable = findAvailableTable(restaurant, numberOfGuests, nextDay, alternativeTime);
                if (availableTable != null) {
                    alternatives.add(new TimeSlot(availableTable, nextDay, alternativeTime));
                }
            }

            return alternatives;
        } finally {
            scheduleLock.readLock().unlock();
        }
    }

    public Table findAvailableTable(Restaurant restaurant, int numberOfGuests, DayOfTheWeek day, LocalTime time) {
        scheduleLock.readLock().lock();
        try {
            Set<Table> availableTables = getAvailableTables(restaurant, day, time);
            if (availableTables.isEmpty()) {
                return null;
            }
            Table chosen = availableTables.stream()
                    .filter(table -> table.getCapacity() >= numberOfGuests)
                    .min(Comparator.comparingInt(Table::getCapacity))
                    .orElse(null);
            return chosen;
        } finally {
            scheduleLock.readLock().unlock();
        }
    }

    public static DayOfTheWeek getNextDay(DayOfTheWeek currentDay) {
        DayOfTheWeek[] days = DayOfTheWeek.values();
        int nextIndex = (currentDay.ordinal() + 1) % days.length;
        return days[nextIndex];
    }

}