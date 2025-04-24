package com.hit.algorithm;


import com.hit.model.Customer;

public interface IAlgoTableReservationSystem {

    /**
     * Attempts to make a reservation for the customer.
     * If no table is available, the customer is added to the waitlist.
     *
     * @param customer the customer making the reservation
     * @return tableId if reserved successfully, or -1 if added to waitlist
     */
    int reserveTable(Customer customer);

    /**
     * Cancels a customer's reservation or removes them from the waitlist.
     *
     * @param customerName name of the customer to cancel
     * @return true if successful, false otherwise
     */
    boolean cancelReservation(String customerName);

    /**
     * Manually joins a customer to the waitlist (used optionally).
     *
     * @param customer the customer to add
     */
    void joinWaitlist(Customer customer);
}



