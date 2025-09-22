package com.hit.dao;

import com.hit.model.*;

import java.util.List;

/**
 * Data Access Object for managing all data operations.
 * 
 * Recommended SQLite table structure:
 * 
 * 1. RESTAURANTS table:
 *    - id (PRIMARY KEY)
 *    - name
 *    - address
 *    - phone
 *    - opening_hours (JSON string with hours for each day)
 * 
 * 2. TABLES table:
 *    - id (PRIMARY KEY)
 *    - restaurant_id (FOREIGN KEY to RESTAURANTS)
 *    - capacity
 *    - status (AVAILABLE, RESERVED, etc.)
 * 
 * 3. CUSTOMERS table:
 *    - id (PRIMARY KEY)
 *    - name
 *    - phone
 *    - email
 * 
 * 4. RESERVATIONS table:
 *    - id (PRIMARY KEY)
 *    - customer_id (FOREIGN KEY to CUSTOMERS)
 *    - restaurant_id (FOREIGN KEY to RESTAURANTS)
 *    - table_id (FOREIGN KEY to TABLES)
 *    - day_of_week
 *    - start_time
 *    - end_time
 *    - status (CONFIRMED, CANCELLED, etc.)
 *    - created_at
 *    - updated_at
 * 
 * 5. WAIT_LIST table:
 *    - id (PRIMARY KEY)
 *    - customer_id (FOREIGN KEY to CUSTOMERS)
 *    - restaurant_id (FOREIGN KEY to RESTAURANTS)
 *    - day_of_week
 *    - start_time
 *    - end_time
 *    - status (WAITING, NOTIFIED, etc.)
 *    - created_at
 *    - updated_at
 */
public interface IDao {
    /**
     * Saves a new reservation
     * @param customer The customer making the reservation
     * @param restaurant The restaurant being reserved
     * @param table The table being reserved
     * @param timeSlot The time slot of the reservation
     * @param dayOfWeek The day of the week
     * @return true if successful, false otherwise
     */
    boolean saveReservation(Customer customer, Restaurant restaurant, Table table,
                            TimeSlot timeSlot, DayOfTheWeek dayOfWeek);

    /**
     * Deletes an existing reservation
     * @param customer The customer who made the reservation
     * @param restaurant The restaurant where the reservation was made
     * @param timeSlot The time slot of the reservation
     * @param dayOfWeek The day of the week
     * @return true if successful, false otherwise
     */
    boolean deleteReservation(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                            DayOfTheWeek dayOfWeek);

    /**
     * Adds a customer to the wait list
     * @param customer The customer to add
     * @param restaurant The restaurant to wait for
     * @param timeSlot The desired time slot
     * @param dayOfWeek The day of the week
     * @return true if successful, false otherwise
     */
    boolean addToWaitList(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                         DayOfTheWeek dayOfWeek);

    List<TimeSlot> getCustomerReservations(Customer customer);
    
    List<TimeSlot> getRestaurantReservations(Restaurant restaurant, DayOfTheWeek dayOfWeek);
}
