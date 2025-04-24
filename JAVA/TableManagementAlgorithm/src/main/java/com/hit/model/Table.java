package com.hit.model;

import java.util.List;

public class Table {
    private int id;
    private int capacity;
    private TableStatus status;
    private final WaitlistManager waitlistManager = new WaitlistManager();  // ✅ רשימת המתנה פרטית

    public Table(int id, int capacity) {
        this.id = id;
        this.capacity = capacity;
        this.status = TableStatus.AVAILABLE;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getCapacity() {
        return capacity;
    }

    public void setCapacity(int capacity) {
        this.capacity = capacity;
    }

    public TableStatus getStatus() {
        return status;
    }

    public void setStatus(TableStatus status) {
        this.status = status;
    }

    // ✅ פונקציות ניהול רשימת המתנה לשולחן

    public void addToWaitlist(Customer customer) {
        waitlistManager.addToWaitlist(customer);
    }

    public boolean removeFromWaitlist(String customerName) {
        return waitlistManager.removeFromWaitlist(customerName);
    }

    public Customer peekNextInWaitlist() {
        return waitlistManager.peekNext();
    }

    public List<Customer> getWaitlist() {
        return waitlistManager.getAll();
    }

    public boolean hasWaitlist() {
        return !waitlistManager.getAll().isEmpty();
    }
}


