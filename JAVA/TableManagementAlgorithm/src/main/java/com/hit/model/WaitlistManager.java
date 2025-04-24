package com.hit.model;

import java.util.*;

public class WaitlistManager {

    private final Queue<Customer> waitlist = new LinkedList<>();

    public void addToWaitlist(Customer customer) {
        waitlist.offer(customer);
    }

    public boolean removeFromWaitlist(String customerName) {
        return waitlist.removeIf(c -> c.getName().equals(customerName));
    }

    public Customer peekNext() {
        return waitlist.peek();
    }

    public List<Customer> getAll() {
        return new ArrayList<>(waitlist);
    }
}

