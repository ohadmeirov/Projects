package com.hit.service;

import com.hit.model.*;

public interface ITableReservationService {
    /**
     * Attempts to reserve a table for a customer at a specific time slot
     *
     * @param customer     The customer making the reservation
     * @param restaurant   The restaurant to reserve at
     * @param timeSlot     The desired time slot
     * @param dayOfWeek    The day of the week
     * @param strategyType The strategy to use for finding available tables
     * @return true if reservation was successful, false otherwise
     */
    Table reserveTable(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                       DayOfTheWeek dayOfWeek, StrategyType strategyType);

    /**
     * Cancels an existing reservation
     * @param customer The customer who made the reservation
     * @param restaurant The restaurant where the reservation was made
     * @param timeSlot The time slot of the reservation
     * @param dayOfWeek The day of the week
     * @return true if cancellation was successful, false otherwise
     */
    boolean cancelReservation(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                              DayOfTheWeek dayOfWeek);

    /**
     * Adds a customer to the wait list for a specific time slot
     * @param customer The customer to add to wait list
     * @param restaurant The restaurant to wait for
     * @param timeSlot The desired time slot
     * @param dayOfWeek The day of the week
     * @return true if successfully added to wait list, false otherwise
     */
    boolean joinToWaitlist(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                           DayOfTheWeek dayOfWeek);
}